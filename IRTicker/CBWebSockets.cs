using System;
using System.Reactive.Linq;
using Websocket.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using Telegram.Bot.Requests;

namespace IRTicker {
    internal class CBWebSockets {

        private readonly Uri _wsUrl = new Uri("wss://ws-feed.exchange.coinbase.com");
        private WebsocketClient _client;
        private readonly string _productId;  // this is the pair, eg "USDT-USD"
        private readonly OrderBook _orderBook = new OrderBook();

        private readonly string _apiKey;
        private readonly string _apiSecret;     // base64 secret
        private readonly string _apiPassphrase;

        ConcurrentDictionary<string, Order> openOrders = new ConcurrentDictionary<string, Order>();  // holds all current open orders
        ConcurrentDictionary<string, Order> closedOrders = new ConcurrentDictionary<string, Order>();  // holds all fills


        // this bit is super fancy.  It creates an event of type Action.  Action has no return type, so when the event
        // is called, it's like a broadcast to anyone subscribed.  You can see further down when we get an L2Update
        // type event that we broadcast to this onOrderBookUpdated event.  It is listened to in the UI forms class
        // and the arguments of the event (the bids and offers) are sent with it, allowing us to update the UI.
        public event Action<IEnumerable<OrderBookEntry>, IEnumerable<OrderBookEntry>> OnOrderBookUpdated;
        public event Action<ConcurrentDictionary<string, Order>> OnOpenOrdersUpdated;
        public event Action<ConcurrentDictionary<string, Order>> OnClosedOrdersUpdated;

        public CBWebSockets(string productId, string apiKey, string apiSecret, string apiPassphrase) {
            _productId = productId;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _apiPassphrase = apiPassphrase;
        }

        public async void Start() {
            if (_client != null) {
                _client.Dispose();
                _client = null;
            }

            _client = new WebsocketClient(_wsUrl);
            _client.MessageReceived
                .Where(msg => msg.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                .Subscribe(msg =>
                {
                    HandleMessage(msg.Text);
                });

            await _client.Start();

            SubscribeL2_and_user(_productId);

            // now we pull the open orders to make sure we can see them all.
            //var openOrders = await CoinbaseClient.CB_GET(_apiKey, _apiSecret, _apiPassphrase, "orders");
            var openOrders = await CoinbaseClient.CB_get_open_orders();
            parseOpenOrders(openOrders);

            var closedOrders = await CoinbaseClient.CB_get_settled("USDT-USD");

            parseClosedOrders(closedOrders);

        }

        private void parseOpenOrders(string openOrders_raw) {

            List<Order> openOrders_list = JsonConvert.DeserializeObject<List<Order>>(openOrders_raw);

            // convert the list to a dictionary with the order_id as the key
            foreach (var order in openOrders_list) {
                if (!openOrders.ContainsKey(order.id)) // Avoid duplicate keys
                {
                    openOrders[order.id] = order;
                }
            }

            if (openOrders.Count > 0) OnOpenOrdersUpdated?.Invoke(openOrders);
        }

        private void parseClosedOrders(string closedOrders_raw) {

            List<Order> closedOrders_list = JsonConvert.DeserializeObject<List<Order>>(closedOrders_raw);
            ConcurrentDictionary<string, Order> closedOrders_temp = new ConcurrentDictionary<string, Order>();

            // convert the list to a dictionary with the order_id as the key
            foreach (var order in closedOrders_list) {
                if (!closedOrders.ContainsKey(order.id)) // Avoid duplicate keys
                {
                    closedOrders[order.id] = order;
                }
            }

            if (closedOrders.Count > 0) OnClosedOrdersUpdated?.Invoke(closedOrders);
        }

        private void updateOpenOrders(Order changedOrder) {
            var type = changedOrder.OrderType?.ToString();
            string order_id = changedOrder.id?.ToString();

            if (null != type) {
                switch (type) {
                    case "done":
                        if (!string.IsNullOrEmpty(order_id)) {
                            if (openOrders.TryRemove(order_id, out Order removed)) {
                                Debug.Print("CB-trade: order removed from open orders, price: " + changedOrder.price?.ToString());
                            }
                            else {
                                Debug.Print("CB-trade - can't' remove open order - order_id: " + order_id + ", price: " + changedOrder.price?.ToString());
                            }
                        }
                        break;

                    case "received":
                        // the open order request has been received.  Should also receive a type==open event once confirmed?
                        // ok, i guess we need to map the wss data into the REST format
                        // will receive something like this:
                        // "[""order_id"": ""32520899-9fc8-4a37-bddb-cf8530a87cbc"", ""order_type"": ""limit"", ""size"": ""16"", ""price"": ""1.00123""...]"

                        OpenOrders_Add(changedOrder);

                        break;

                    case "open":
                        // this event (I think) is when an event of yours is confirmed open
                        // we should already have an order with the guid..

                        if (!string.IsNullOrEmpty(order_id)) {
                            if (openOrders.ContainsKey(order_id)) {
                                // let's update the order
                                openOrders[order_id].remaining_size = changedOrder.remaining_size?.ToString();
                            }
                        }
                        break;


                }

                // now we broadcast an event that we need to update the open orders UI
                OnOpenOrdersUpdated?.Invoke(openOrders);
            }
        }

        // creates the Order object and adds it to the openOrders global dictionary
        private void OpenOrders_Add(Order orderEvent) {

            if (openOrders.ContainsKey(orderEvent.id)) {  // check if the order is already in the dictionary.. if it is remove it.
                Debug.Print("CB-trade - strangely we were given a new order, but it's already in the openOrders dictionary.  ID: " + orderEvent.id);
                if (!openOrders.TryRemove(orderEvent.id, out Order removed)) {
                    Debug.Print("-- CB-trade - Tried to remove and it failed.. id: " + orderEvent.id);
                }
            }
            // now let's add it
            if (openOrders.TryAdd(orderEvent.id, orderEvent)) {
                Debug.Print("CB-trade - added new order, price: " + orderEvent.price);
            }
            else {
                Debug.Print("CB-trade - tried to add new order, but failed.  price: " + orderEvent.price);
            }
        }

        public void Stop() {
            _client?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Stopped");
        }

        private void SubscribeL2_and_user(string productId) {
            // Authenticated subscribe
            // According to Coinbase docs for authentication:
            // prehash = timestamp + "GET" + "/users/self/verify"
            // signature = HMAC_SHA256(prehash, secret) base64-encoded

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var prehash = timestamp + "GET" + "/users/self/verify";
            var signature = CoinbaseClient.HashString(prehash, Convert.FromBase64String(_apiSecret));

            var subMsg = new
            {
                type = "subscribe",
                product_ids = new string[] { productId },
                channels = new string[] { "level2", "user" },
                key = _apiKey,
                passphrase = _apiPassphrase,
                timestamp = timestamp,
                signature = signature
            };

            string json = JsonConvert.SerializeObject(subMsg);
            _client.Send(json);
        }

        private void HandleMessage(string json) {
            var msg = JObject.Parse(json);
            var type = msg["type"]?.ToString();  // ? here safely evaluates the expression to null if the "type" property doesn't exist or is null.  no need to code around it

            switch (type) {
                case "snapshot":
                    var bids = msg["bids"] as JArray;
                    var asks = msg["asks"] as JArray;

                    var bidEntries = ConvertArrayToEntries(bids);
                    var askEntries = ConvertArrayToEntries(asks);

                    _orderBook.LoadSnapshot(bidEntries, askEntries);
                    OnOrderBookUpdated?.Invoke(_orderBook.Bids, _orderBook.Asks);
                    break;
                case "l2update":
                    var changes = msg["changes"] as JArray;
                    if (changes != null) {
                        foreach (var change in changes) {
                            var side = change[0].ToString(); // "buy" or "sell"
                            if (!decimal.TryParse(change[1].ToString(), out var price)) continue;
                            if (!decimal.TryParse(change[2].ToString(), out var size)) continue;
                            _orderBook.UpdateFromL2Update(side, price, size);
                        }
                        OnOrderBookUpdated?.Invoke(_orderBook.Bids, _orderBook.Asks);
                    }
                    break;

                case "error":
                    // Handle errors (like auth errors)
                    Console.WriteLine("Error: " + msg.ToString());
                    break;

                // these are openOrder tings
                case "done":
                case "open":
                case "received":
                    Order updatedOrder = JsonConvert.DeserializeObject<Order>(json);

                    // now we have to map some properties as wss uses different names
                    if (!string.IsNullOrEmpty(msg["order_id"]?.ToString())) {  // if the order_id property exists, then map it to the id variable
                        updatedOrder.id = msg["order_id"].ToString();
                    }

                    if (!string.IsNullOrEmpty(msg["time"]?.ToString())) {  // if the order_id property exists, then map it to the id variable

                        if (!DateTime.TryParse(msg["time"].ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime createdAt)) {
                            createdAt = DateTime.MinValue; // or handle as needed
                        }
                        updatedOrder.created_at = createdAt;
                    }

                    updateOpenOrders(updatedOrder);
                    // re-draw the open orders thing
                    break;

                default:
                    Console.WriteLine(DateTime.Now + " - CB-trade - type: " + type);
                    break;
            }
        }

        private IEnumerable<OrderBookEntry> ConvertArrayToEntries(JArray arr) {
            var result = new List<OrderBookEntry>();
            foreach (var item in arr) {
                if (!decimal.TryParse(item[0].ToString(), out var price)) continue;
                if (!decimal.TryParse(item[1].ToString(), out var size)) continue;

                result.Add(new OrderBookEntry { Price = price, Size = size });
            }
            return result;
        }

        public class OrderBookEntry {
            public decimal Price { get; set; }
            public decimal Size { get; set; }
        }

        public class OrderBook {
            // Bids stored in descending order by price
            private readonly SortedDictionary<decimal, decimal> _bids = new SortedDictionary<decimal, decimal>(new DescendingComparer<decimal>());
            // Asks stored in ascending order by price
            private readonly SortedDictionary<decimal, decimal> _asks = new SortedDictionary<decimal, decimal>();

            public IEnumerable<OrderBookEntry> Bids => _bids.Select(kv => new OrderBookEntry { Price = kv.Key, Size = kv.Value }).ToList();
            public IEnumerable<OrderBookEntry> Asks => _asks.Select(kv => new OrderBookEntry { Price = kv.Key, Size = kv.Value }).ToList();

            /**
             * this is tricky.  the _bids and _asks properties of the OrderBook class are automatically sorted
             * So, by simply adding the price/size object (OrderBookEntry) to the _bids and _offers property,
             * they get sorted in the correct order.
             */
            public void LoadSnapshot(IEnumerable<OrderBookEntry> bids, IEnumerable<OrderBookEntry> asks) {
                _bids.Clear();
                _asks.Clear();
                foreach (var b in bids) _bids[b.Price] = b.Size;
                foreach (var a in asks) _asks[a.Price] = a.Size;
            }

            public void UpdateFromL2Update(string side, decimal price, decimal size) {
                if (side == "buy") {
                    if (size == 0m)
                        _bids.Remove(price);
                    else
                        _bids[price] = size;
                }
                else if (side == "sell") {
                    if (size == 0m)
                        _asks.Remove(price);
                    else
                        _asks[price] = size;
                }
            }
        }

        public class DescendingComparer<T> : IComparer<T> where T : IComparable<T> {
            public int Compare(T x, T y) => y.CompareTo(x);
        }

        // this is the format we get from the REST /orders endpoint
        // have to convert WSS format (order_id -> id, time -> created_at)
        public class Order {
            public string id { get; set; }
            public string client_oid { get; set; }
            public string price { get; set; }
            public string size { get; set; }
            public string remaining_size { get; set; }
            public string product_id { get; set; }
            public string profile_id { get; set; }
            public string side { get; set; }
            [JsonProperty("type")]
            public string OrderType { get; set; }
            public string time_in_force { get; set; }
            public bool post_only { get; set; }
            public DateTime created_at { get; set; }
            public string fill_fees { get; set; }
            public string filled_size { get; set; }
            public string executed_value { get; set; }
            public string market_type { get; set; }
            public string status { get; set; }
            public bool settled { get; set; }
            public DateTime done_at { get; set; }
            public string done_reason { get; set; }
            public string funding_currency { get; set; }
        }

        public class Fill {
            public int trade_id { get; set; }
            public string product_id { get; set; }
            public string order_id { get; set; }
            public string user_id { get; set; }
            public string profile_id { get; set; }
            public string liquidity { get; set; }
            public string price { get; set; }
            public string size { get; set; }
            public string fee { get; set; }
            public DateTime created_at { get; set; }
            public string side { get; set; }
            public bool settled { get; set; }
            public string usd_volume { get; set; }
            public string market_type { get; set; }
            public string funding_currency { get; set; }
        }
    }
}
