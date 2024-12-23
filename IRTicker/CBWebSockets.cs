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
using static IRTicker.CBWebSockets;

namespace IRTicker {
    internal class CBWebSockets {

        private readonly Uri _wsUrl = new Uri("wss://ws-feed.exchange.coinbase.com");
        private WebsocketClient _client;
        private string _productId = "USDT-USD";  // this is the pair, eg "USDT-USD"
        private readonly OrderBook _orderBook = new OrderBook();

        private readonly string _apiKey;
        private readonly string _apiSecret;     // base64 secret
        private readonly string _apiPassphrase;

        ConcurrentDictionary<string, Order> openOrders = new ConcurrentDictionary<string, Order>();  // holds all current open orders
        List<Order> closedOrders = new List<Order>();  // holds all closed orders.  Don't need a dictionary as we won't be needing to pick out entries to manipulate


        // this bit is super fancy.  It creates an event of type Action.  Action has no return type, so when the event
        // is called, it's like a broadcast to anyone subscribed.  You can see further down when we get an L2Update
        // type event that we broadcast to this onOrderBookUpdated event.  It is listened to in the UI forms class
        // and the arguments of the event (the bids and offers) are sent with it, allowing us to update the UI.
        public event Action<IEnumerable<OrderBookEntry>, IEnumerable<OrderBookEntry>> OnOrderBookUpdated;
        public event Action<ConcurrentDictionary<string, Order>> OnOpenOrdersUpdated;
        public event Action<List<Order>> OnClosedOrdersUpdated;
        public event Action OnFailedToLoad;
        public event Action<List<Products>> OnProductsUpdated;
        public event Action OnFinishNetworkTasks;

        public CBWebSockets(string apiKey, string apiSecret, string apiPassphrase) {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _apiPassphrase = apiPassphrase;

            getAndParseProducts();  // only need to do this once
        }

        public async void Start(string productId) {
            _productId = productId;

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
            bool success = await parseOpenOrders();

            if (success) {
                success = await parseClosedOrders();
            }

            if (!success) {
                OnFailedToLoad?.Invoke();  // something failed in loading API data, close the form
            }
            else {
                // enable the pair drop down menu again
                OnFinishNetworkTasks?.Invoke();
            }
        }

        private async Task<bool> getAndParseProducts() {
            var trading_pairs = await CoinbaseClient.CB_get_pairs();

            List<Products> trading_pairs_list;
            try {
                trading_pairs_list = JsonConvert.DeserializeObject<List<Products>>(trading_pairs);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling pairs." + Environment.NewLine + Environment.NewLine + "Response: " + trading_pairs + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                return false;
            }

            // now re-order the list to be in alphabetical order
            var orderedProducts = trading_pairs_list.OrderBy(p => p.id).ToList();

            OnProductsUpdated?.Invoke(orderedProducts);
            return true;
        }

        private async Task<bool> parseOpenOrders() {

            var openOrders_raw = await CoinbaseClient.CB_get_open_orders();

            List<Order> openOrders_list;
            try {
                openOrders_list = JsonConvert.DeserializeObject<List<Order>>(openOrders_raw);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling open orders." + Environment.NewLine + Environment.NewLine + "Response: " + openOrders_raw + Environment.NewLine + Environment.NewLine  + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                return false;
            }

            // convert the list to a dictionary with the order_id as the key
            foreach (var order in openOrders_list) {
                if (!openOrders.ContainsKey(order.id)) // Avoid duplicate keys
                {
                    openOrders[order.id] = order;
                }
            }

            OnOpenOrdersUpdated?.Invoke(openOrders);
            return true;
        }

        private async Task<bool> parseClosedOrders() {

            var closedOrders_raw = await CoinbaseClient.CB_get_settled(_productId);

            try {
                closedOrders = JsonConvert.DeserializeObject<List<Order>>(closedOrders_raw);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling closed orders."+ Environment.NewLine + Environment.NewLine +  "Response: " + closedOrders_raw + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                return false;
            }

            OnClosedOrdersUpdated?.Invoke(closedOrders);
            return true;
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
                case "received":  // actually i think we should do nothing on received.  open and done should be for open and closed.. ?
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

        public class Products {
            public string id { get; set; }
            public string base_currency { get; set; }
            public string quote_currency { get; set; }
            public string quote_increment { get; set; }
            public string base_increment { get; set; }
            public string display_name { get; set; }
            public string min_market_funds { get; set; }
            public bool margin_enabled { get; set; }
            public bool post_only { get; set; }
            public bool limit_only { get; set; }
            public bool cancel_only { get; set; }
            public string status { get; set; }
            public string status_message { get; set; }
            public bool trading_disabled { get; set; }
            public bool fx_stablecoin { get; set; }
            public string max_slippage_percentage { get; set; }
            public bool auction_mode { get; set; }
            public string high_bid_limit_percentage { get; set; }
        }
    }
}
