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

namespace IRTicker {
    internal class CBWebSockets {

        private readonly Uri _wsUrl = new Uri("wss://ws-feed.exchange.coinbase.com");
        private WebsocketClient _client;
        private readonly string _productId;  // this is the pair, eg "USDT-USD"
        private readonly OrderBook _orderBook = new OrderBook();

        private readonly string _apiKey;
        private readonly string _apiSecret;     // base64 secret
        private readonly string _apiPassphrase;

        // this bit is super fancy.  It creates an event of type Action.  Action has no return type, so when the event
        // is called, it's like a broadcast to anyone subscribed.  You can see further down when we get an L2Update
        // type event that we broadcast to this onOrderBookUpdated event.  It is listened to in the UI forms class
        // and the arguments of the event (the bids and offers) are sent with it, allowing us to update the UI.
        public event Action<IEnumerable<OrderBookEntry>, IEnumerable<OrderBookEntry>> OnOrderBookUpdated;

        public CBWebSockets(string productId, string apiKey, string apiSecret, string apiPassphrase) {
            _productId = productId;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _apiPassphrase = apiPassphrase;
        }

        public void Start() {
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

            _client.Start();

            SubscribeToLevel2(_productId);
        }

        public void Stop() {
            _client?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Stopped");
        }

        private void SubscribeToLevel2(string productId) {
            // Authenticated subscribe
            // According to Coinbase docs for authentication:
            // prehash = timestamp + "GET" + "/users/self/verify"
            // signature = HMAC_SHA256(prehash, secret) base64-encoded

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var prehash = timestamp + "GET" + "/users/self/verify";
            var signature = CreateSignature(prehash, _apiSecret);

            var subMsg = new
            {
                type = "subscribe",
                product_ids = new string[] { productId },
                channels = new string[] { "level2" },
                key = _apiKey,
                passphrase = _apiPassphrase,
                timestamp = timestamp,
                signature = signature
            };

            string json = JsonConvert.SerializeObject(subMsg);
            _client.Send(json);
        }

        private string CreateSignature(string prehash, string secret) {
            var secretBytes = Convert.FromBase64String(secret);
            using (var hmac = new HMACSHA256(secretBytes)) {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(prehash));
                return Convert.ToBase64String(hash);
            }
        }

        private void HandleMessage(string json) {
            var msg = JObject.Parse(json);
            var type = msg["type"]?.ToString();  // ? here safely evaluates the expression to null if the "type" property doesn't exist or is null.  no need to code around it

            if (type == "snapshot") {
                var bids = msg["bids"] as JArray;
                var asks = msg["asks"] as JArray;

                var bidEntries = ConvertArrayToEntries(bids);
                var askEntries = ConvertArrayToEntries(asks);

                _orderBook.LoadSnapshot(bidEntries, askEntries);
                OnOrderBookUpdated?.Invoke(_orderBook.Bids, _orderBook.Asks);
            }
            else if (type == "l2update") {
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
            }
            else if (type == "error") {
                // Handle errors (like auth errors)
                Console.WriteLine("Error: " + msg.ToString());
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
    }
}
