using System;
using System.Reactive.Linq;
using Websocket.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using System.Collections.Concurrent;

using System.Net.WebSockets;
using System.Windows;
using IRTicker.Coinbase_trade.Models;
using static IRTicker.DCE;
using Telegram.Bot.Types;
using System.Runtime.InteropServices;
using static IRTicker.Balance;

namespace IRTicker {
    internal class CBWebSockets {

        private readonly Uri _wsUrl = new Uri("wss://ws-feed.exchange.coinbase.com");
        private WebsocketClient _wsClient;
        private string _productId = "USDT-USD";  // this is the pair, eg "USDT-USD"
        private readonly CB_Orderbook _orderBook = new CB_Orderbook();

        private string _apiKey;
        private string _apiSecret;     // base64 secret
        private string _apiPassphrase;

        private ConcurrentDictionary<string, CB_Order> openOrders = new ConcurrentDictionary<string, CB_Order>();  // holds all current open orders
        private List<CB_Order> closedOrders = new List<CB_Order>();  // holds all closed orders.  Don't need a dictionary as we won't be needing to pick out entries to manipulate
        private ConcurrentDictionary<string, CB_Accounts> accounts;  // holds all account details, key is currency (product)

        // buffering and sending to UI on a timer
        private System.Timers.Timer _throttleTimer;
        private volatile bool _snapshotDirty;
        private readonly object _snapshotLock = new object();
        private IEnumerable<CB_OrderBookEntry> _currentBids;
        private IEnumerable<CB_OrderBookEntry> _currentAsks;

        // some baiter variables
        private bool baiter_Active = false;
        private string baiter_order_id;
        private decimal baiter_RemainingSize;
        private string baiter_Side;
        private string baiter_Pair;
        private decimal baiter_price;
        private int baiter_pause_and_retry = 0;  // if 0, nothing to do.  If above 0 and below 5, then we increase it, if above 5, we try and re-create the order
        private int baiter_retries = 0;  // how many times we have paused and retried.  give up after.. say 5?
        private bool baiter_changing_price = false;  // we set to true when we're in the process of cancelling and re-creating the baiter order so we don't try a million times

        // this bit is super fancy.  It creates an event of type Action.  Action has no return type, so when the event
        // is called, it's like a broadcast to anyone subscribed.  You can see further down when we get an L2Update
        // type event that we broadcast to this onOrderBookUpdated event.  It is listened to in the UI forms class
        // and the arguments of the event (the bids and offers) are sent with it, allowing us to update the UI.
        public event Action<IEnumerable<CB_OrderBookEntry>, IEnumerable<CB_OrderBookEntry>, ConcurrentDictionary<string, CB_Order>> OnOrderBookUpdated;
        public event Action<ConcurrentDictionary<string, CB_Order>> OnOpenOrdersUpdated;
        public event Action<List<CB_Order>> OnClosedOrdersUpdated;
        public event Action OnFailedToLoad;
        public event Action<List<CB_Products>> OnProductsUpdated;
        public event Action OnFinishNetworkTasks;
        public event Action<CB_Accounts, CB_Accounts> OnUpdatedPairBalance;
        public event Action OnBaiterComplete;
        public event Action<string> OnBaiterStarted;

        private CBWebSockets(string apiKey, string apiSecret, string apiPassphrase) {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _apiPassphrase = apiPassphrase;

            _throttleTimer = new System.Timers.Timer(150);
            _throttleTimer.Elapsed += (s, e) => ThrottleTimerElapsed();
            _throttleTimer.Start();
        }

        /// <summary>
        /// We want to run some async tasks from the constructor, but you cannot mark
        /// the constructor as async.  So we create this "factory method".  We make the constructor
        /// method private, so when creating the instance of the class we don't use "new"
        /// (it won't work), we call this factory method first, which in turn calls the 
        /// constructor to create the instance. From the factory method we run our async
        /// tasks, and then return the instance object back to the caller, so it appears
        /// to the caller that they have created a new instance of the class, in just the
        /// same way they would if they used the "new" keyword.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="apiPassphrase"></param>
        /// <returns></returns>
        public static async Task<CBWebSockets> CreateAsync(string apiKey, string apiSecret, string apiPassphrase) {

            CBWebSockets instance = new CBWebSockets(apiKey, apiSecret, apiPassphrase);
            
             // now done in the .start() function
            /*bool success = await instance.getAndParseProducts();
            if (!success) {
                MessageBox.Show("Failed to pull Products from Coinbase API");
                return null;
            }*/

            bool success = await instance.getAndParseAccounts();
            if (!success) {
                MessageBox.Show("Failed to pull Accounts from Coinbase API");
                return null;
            }

            return instance;
        }

        /// <summary>
        /// Starts everything off.  Downloads pairs, accounts, open orders, closed orders, and subscribes to the wss 
        /// </summary>
        /// <param name="productId"></param> eg "USDT-USD"
        /// <param name="getProducts"></param> if true, we pull the products from Coinbase API.  Should only be true once per opening of the form
        /// <param name="ignoreSockets"></param> if true, we don't try and recraete or subscribe to sockets, just do REST things
        /// <returns></returns>
        public async Task Start(string productId, bool getProducts, bool ignoreSockets) {
            _productId = productId;

            // first we check if we have a list of products already.
            // if not, we pull it
            if (getProducts) {
                await getAndParseProducts();
            }

            if (!ignoreSockets) {
                Debug.Print("CB-trade - running sockets startup tasks");
                if (_wsClient != null) {
                    Debug.Print("CB-trade - _wsClient isRunning: " + _wsClient.IsRunning);
                    _wsClient.Dispose();
                    _wsClient = null;
                }
                else {
                    Debug.Print("CB-trade - _wsClient is null");
                }

                _wsClient = new WebsocketClient(_wsUrl);
                _wsClient.MessageReceived
                    .Where(msg => msg.MessageType == WebSocketMessageType.Text)
                    .Subscribe(msg =>
                    {
                        //Task.Run(async () => await HandleMessage(msg.Text));
                        HandleMessage(msg.Text);
                    });

                _wsClient.ReconnectionHappened.Subscribe(info =>
                {
                    Debug.Print("CB-trade - sockets reconnection happened.  Resubscribing...");
                    SubscribeL2_and_user(_productId);
                });

                await _wsClient.Start();

                // turns out when you connect, it calls the ReconnectionHappened block, which will subscribe
                // automatically, so we don't need to do it here.
                //SubscribeL2_and_user(_productId);
            }

            // now we pull the open orders to make sure we can see them all.
            bool success = await parseOpenOrders();

            if (success) {
                success = await parseClosedOrders();
            }

            if (success) {
                success = await getAndParseAccount(_productId);
            }

            if (!success && !ignoreSockets) {  // let's only fully bail if we're doing a full reconnect.  If we're just refreshing REST endpoints and it fails, ignore and move on
                OnFailedToLoad?.Invoke();  // something failed in loading API data, close the form
            }
            else {
                // enable the pair drop down menu again
                OnFinishNetworkTasks?.Invoke();
            }
        }

        private async void ThrottleTimerElapsed() {
            // If there's no new data, do nothing
            if (!_snapshotDirty) return;

            IEnumerable<CB_OrderBookEntry> bids, asks;
            lock (_snapshotLock) {
                bids = _currentBids.ToList();
                asks = _currentAsks.ToList();
                _snapshotDirty = false;
            }

            OnOrderBookUpdated?.Invoke(bids, asks, openOrders);

            // let's see if baiter needs any help
            if (baiter_pause_and_retry > 0) {
                if (baiter_pause_and_retry < 6) {
                    baiter_pause_and_retry++;
                }
                else {  // OK we have paused long enough, let's do something
                    // first let's see if the order exists
                    Debug.Print("CB-trade - baiter needs to retry");
                    if (openOrders.ContainsKey(baiter_order_id)) {
                        Debug.Print("CB-trade - but the order appears to already exist.. so let's do nothing");
                        baiter_pause_and_retry = 0;
                    }
                    else {
                        Debug.Print("CB-trade - let's try and re-create the baiter order");
                        bool orderCreatedSuccessfully = await TryCreateBaiterOrder();
                        if (!orderCreatedSuccessfully) {

                            baiter_retries++;
                            Debug.Print("CB-trade-baiter - failed to recreate the baiter order " + baiter_retries + " time(s)");

                            if (baiter_retries > 6) {
                                MessageBox.Show("Baiter has failed 5 times in a row to create the new order, stopping now.");
                                baiter_Active = false;
                                OnBaiterComplete?.Invoke();
                            }
                        }
                    }
                }
            }
        }

        private void ForceOrderBookRedraw() {
            lock (_snapshotLock) {
                _snapshotDirty = true;
            }
            ThrottleTimerElapsed();
        }

        // gets ALL accounts
        private async Task<bool> getAndParseAccounts() {
            string accounts_raw = await CoinbaseClient.CB_get_accounts();

            List<CB_Accounts> accounts_list;
            try {
                accounts_list = JsonConvert.DeserializeObject<List<CB_Accounts>>(accounts_raw);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling accounts." + Environment.NewLine + Environment.NewLine + "Response: " + accounts + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                Debug.Print("CB-trade - couuldn't parse accounts.  error: " + ex.Message);
                return false;
            }
            accounts = new ConcurrentDictionary<string, CB_Accounts>();
            foreach (CB_Accounts acc in accounts_list) {
                //accounts[acc.currency] = acc;
                accounts.TryAdd(acc.currency, acc);
            }
            return true;
        }

        // this will pull the account (balance) details of a pair
        // (one call for base and one for terms) and display the balance on the UI
        private async Task<bool> getAndParseAccount(string pair) {
            // first split the pair into two currencies
            string[] currencies = pair.Split('-');

            if (currencies.Length == 2) {
                // now get each account value and update the accounts thing
                if (accounts.ContainsKey(currencies[0]) && accounts.ContainsKey(currencies[1])) {
                    var currency1 = await CoinbaseClient.CB_get_accounts(accounts[currencies[0]].id);
                    var currency2 = await CoinbaseClient.CB_get_accounts(accounts[currencies[1]].id);

                    CB_Accounts currency1_acc;
                    CB_Accounts currency2_acc;
                    try {
                        currency1_acc = JsonConvert.DeserializeObject<CB_Accounts>(currency1);
                        currency2_acc = JsonConvert.DeserializeObject<CB_Accounts>(currency2);
                    }
                    catch (Exception ex) {
                        System.Windows.Forms.MessageBox.Show("Failed to parse currency1 or 2 (1: " + currencies[0] + ")(2: " + currencies[1] + ")" + Environment.NewLine + Environment.NewLine + "Response: " + accounts + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                        Debug.Print("CB-trade - couuldn't parse account (1: " + currencies[0] + ")(2: " + currencies[1] + ").  error: " + ex.Message);
                        return false;
                    }

                    if ((null == currency1_acc) || (null == currency2_acc)) return false;

                    OnUpdatedPairBalance?.Invoke(currency1_acc, currency2_acc);
                    return true;
                }
            }
            return false;
        }
        private async Task<bool> getAndParseProducts() {
            var trading_pairs = await CoinbaseClient.CB_get_pairs();

            List<CB_Products> trading_pairs_list;
            try {
                trading_pairs_list = JsonConvert.DeserializeObject<List<CB_Products>>(trading_pairs);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling pairs." + Environment.NewLine + Environment.NewLine + "Response: " + trading_pairs + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                return false;
            }

            if (null == trading_pairs_list) return false;

            // now re-order the list to be in alphabetical order
            var orderedProducts = trading_pairs_list.OrderBy(p => p.id).ToList();

            OnProductsUpdated?.Invoke(orderedProducts);
            return true;
        }

        public async Task<bool> CB_start_baiter(string pair, string side, decimal startingSize) {
            if (baiter_Active) return false;
            
            baiter_RemainingSize = startingSize;
            baiter_Side = side;
            baiter_Pair = pair;
            baiter_changing_price = false;
            baiter_pause_and_retry = 0;
            baiter_retries = 0;

            Debug.Print("CB-trade-baiter - starting : " + side + " " + pair + " " + startingSize);

            // next we need to figure out what the top of the order book is and place the order there
            var ourOrderbook = (side == "buy" ? _currentBids : _currentAsks);
            decimal topOrderPrice;
            // we don't need to lock this - it's read only basically
            lock (_snapshotLock) {
                CB_OrderBookEntry topOrder = ourOrderbook.FirstOrDefault();
                topOrderPrice = topOrder.Price;
            }

            Debug.Print("CB-trade-baiter: starting price: " + topOrderPrice);
            baiter_price = topOrderPrice;

            // now we place the order quick smart
            baiter_Active = await TryCreateBaiterOrder();

            return baiter_Active;
        }

        private async Task<bool> TryCreateBaiterOrder() {

            var create_baiter_order = await CB_place_order(baiter_Pair, baiter_Side, baiter_price.ToString(), baiter_RemainingSize.ToString(), "limit", true);

            if (null != create_baiter_order) {
                Debug.Print("CB-trade-baiter - new order response not null...");
                if (null != create_baiter_order.status) {
                    Debug.Print("CB-trade-baiter - new order status: " + create_baiter_order.status);
                    if (create_baiter_order.status != "rejected") {
                        // but if it is rejected or null, how do we signal a pause then a retry?
                        baiter_order_id = create_baiter_order.order_id;
                        baiter_changing_price = false;  // ok, we should be finished with this price update, can resume checks
                        baiter_pause_and_retry = 0;
                        baiter_retries = 0;
                        Debug.Print("CB-trade-baiter - all good on order placement - changing price false again");

                        // now we check the current open orders, there's a good chance the baiter order
                        // has been already added to the list, and so is not coloured correctly
                        OnBaiterStarted?.Invoke(baiter_order_id);
                        return true;
                    }
                    else {
                        baiter_pause_and_retry = 1;
                        Debug.Print("CB-trade-baiter - baiter order was rejected, will try again in a bit");
                    }
                }
                else {
                    baiter_pause_and_retry = 1;
                    Debug.Print("CB-trade-baiter - baiter order status was null, will try again in a bit");
                }
            }
            else {
                baiter_pause_and_retry = 1;
                Debug.Print("CB-trade-baiter - baiter order had a null response, will try again in a bit");
            }
            return false;
        }

        // should only be called at the start and if a "Rrefresh all" is called,
        // uses the REST end point for the full list
        private async Task<bool> parseOpenOrders() {

            var openOrders_raw = await CoinbaseClient.CB_get_open_orders();

            List<CB_Order> openOrders_list;
            try {
                openOrders_list = JsonConvert.DeserializeObject<List<CB_Order>>(openOrders_raw);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling open orders." + Environment.NewLine + Environment.NewLine + "Response: " + openOrders_raw + Environment.NewLine + Environment.NewLine  + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                return false;
            }

            openOrders.Clear();

            // convert the list to a dictionary with the order_id as the key
            foreach (var order in openOrders_list) {
                if (!openOrders.ContainsKey(order.id)) // Avoid duplicate keys
                {
                    openOrders[order.id] = order;

                    if (baiter_Active && (order.id == baiter_order_id)) {
                        openOrders[order.id].isBaiter = true;
                    }
                }
            }

            OnOpenOrdersUpdated?.Invoke(openOrders);
            return true;
        }

        private async Task<bool> parseClosedOrders() {

            var closedOrders_raw = await CoinbaseClient.CB_get_settled(_productId);

            try {
                closedOrders = JsonConvert.DeserializeObject<List<CB_Order>>(closedOrders_raw);
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Failed to start Coinbase when pulling closed orders."+ Environment.NewLine + Environment.NewLine +  "Response: " + closedOrders_raw + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + Environment.NewLine + ex.Message);
                return false;
            }

            // clean it up
            foreach (CB_Order closedOrder in closedOrders) {
                if (string.IsNullOrEmpty(closedOrder.order_id) && !string.IsNullOrEmpty(closedOrder.id)) {
                    closedOrder.order_id = closedOrder.id;
                }

                // let's check here if we have the baiter order and it's closed.  If so, and baiter
                // is still active, shut it down
                if (baiter_Active) {
                    if (closedOrder.order_id == baiter_order_id) {
                        baiter_Active = false;
                        OnBaiterComplete?.Invoke();
                    }
                }
            }

            OnClosedOrdersUpdated?.Invoke(closedOrders);
            return true;
        }

        // gets updated open orders from sockets
        private void updateOpenOrders(CB_Order changedOrder) {
            var type = changedOrder.OrderType;
            string order_id = changedOrder.id;

            if (null != type) {
                switch (type) {
                    case "done":
                        if (!string.IsNullOrEmpty(order_id)) {
                            if (openOrders.TryRemove(order_id, out CB_Order removed)) {
                                Debug.Print("CB-trade: order removed from open orders, price: " + changedOrder.price.ToString());
                            }
                            else {
                                Debug.Print("CB-trade - can't' remove open order - order_id: " + order_id + ", price: " + changedOrder.price.ToString());
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
                                openOrders[order_id].remaining_size = changedOrder.remaining_size;
                                if (order_id == baiter_order_id) {
                                    openOrders[order_id].isBaiter = true;
                                }
                                else {
                                    openOrders[order_id].isBaiter = false;
                                }
                            }
                        }
                        break;
                }

                // now we broadcast an event that we need to update the open orders UI
                OnOpenOrdersUpdated?.Invoke(openOrders);
                ForceOrderBookRedraw();  // force the order book to be updated (eg to remove highlighting for own order if it's been cancelled)
            }
        }

        // creates the Order object and adds it to the openOrders global dictionary
        private void OpenOrders_Add(CB_Order orderEvent) {

            if (openOrders.ContainsKey(orderEvent.id)) {  // check if the order is already in the dictionary.. if it is remove it.
                Debug.Print("CB-trade - strangely we were given a new order, but it's already in the openOrders dictionary.  ID: " + orderEvent.id);
                if (!openOrders.TryRemove(orderEvent.id, out CB_Order removed)) {
                    Debug.Print("-- CB-trade - Tried to remove and it failed.. id: " + orderEvent.id);
                }
            }
            // now let's add it
            Debug.Print("CB-trade - baiter about to be checked.  order id: " + orderEvent.id + ", baiter id: " + baiter_order_id);
            if (orderEvent.id == baiter_order_id) orderEvent.isBaiter = true;
            if (openOrders.TryAdd(orderEvent.id, orderEvent)) {
                Debug.Print("CB-trade - added new order, price: " + orderEvent.price);
            }
            else {
                Debug.Print("CB-trade - tried to add new order, but failed.  price: " + orderEvent.price);
            }
        }
            
        public void Stop() {
            //_wsClient.IsReconnectionEnabled = false;
            //Task.Run(() => _wsClient?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Stopped"));
            //_wsClient.IsReconnectionEnabled = true;


            // Turn off reconnection, maybe this is what was screwing up my re-starting attempts..
            _wsClient.IsReconnectionEnabled = false;

            // .Stop will never return if you await it (sometimes), so we must just try to stop and then continue with our lives.
            Task.Run(() => _wsClient.Stop(WebSocketCloseStatus.NormalClosure, "Stopped"));

            // just make it go away.  we're starting again.
            _wsClient.Dispose();
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
                channels = new string[] { "level2_batch", "user" },
                key = _apiKey,
                passphrase = _apiPassphrase,
                timestamp = timestamp,
                signature = signature
            };

            string json = JsonConvert.SerializeObject(subMsg);
            _wsClient.Send(json);
        }

        private async Task HandleMessage(string json) {
            var msg = JObject.Parse(json);
            var type = msg["type"]?.ToString();  // ? here safely evaluates the expression to null if the "type" property doesn't exist or is null.  no need to code around it

            switch (type) {
                case "snapshot":
                    var bids = msg["bids"] as JArray;
                    var asks = msg["asks"] as JArray;

                    var bidEntries = ConvertArrayToEntries(bids);
                    var askEntries = ConvertArrayToEntries(asks);

                    _orderBook.LoadSnapshot(bidEntries, askEntries);

                    lock (_snapshotLock) {
                        _currentBids = _orderBook.Bids.ToList();  // snapshot
                        _currentAsks = _orderBook.Asks.ToList();
                        _snapshotDirty = true;
                    }

                    //OnOrderBookUpdated?.Invoke(_orderBook.Bids, _orderBook.Asks);
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

                        // for baiter, we grab the top of book as well
                        decimal topBid, topAsk = 0;

                        lock (_snapshotLock) {
                            _currentBids = _orderBook.Bids.ToList();  // snapshot
                            _currentAsks = _orderBook.Asks.ToList();
                            _snapshotDirty = true;
                            topBid = _currentBids.FirstOrDefault().Price;
                            topAsk = _currentAsks.FirstOrDefault().Price;
                        }

                        // now should decide if the baiter is active and order is not at top of book, re-create order
                        // don't bother with baiter stuff if 1. it's not even active, 2. we're already trying to change the price, or 3. we're pausing to try again a bit later
                        if (baiter_Active && !baiter_changing_price && (baiter_pause_and_retry == 0)) {
                            //Debug.Print("CB-trade-baiter - baiter is active, we are not currently changing price");
                            baiter_changing_price = true;  // OK, don't try and change price again until we have created the new order

                            decimal baiter_new_price = 0;
                            if (baiter_Side == "buy") {
                                //Debug.Print("CB-trade-baiter - baiter is a buy order");
                                if (topBid > baiter_price) {
                                    Debug.Print("CB-trade-baiter - best bid (" + _currentBids.FirstOrDefault().Price + ") is greater than baiter price (" + baiter_price + ")");
                                    // ok, we need to change.  set some variables
                                    baiter_new_price = topBid;
                                }
                            }
                            else {  // sell
                                //Debug.Print("CB-trade-baiter - baiter is a sell order");
                                if (topAsk < baiter_price) {
                                    Debug.Print("CB-trade-baiter - best offer (" + _currentAsks.FirstOrDefault().Price + ") is less than baiter price (" + baiter_price + ")");

                                    // ok, we need to change.  set some variables
                                    baiter_new_price = topAsk;
                                }
                            }

                            if (baiter_new_price > 0) {
                                Debug.Print("CB-trade-baiter - about to cancel old order");
                                bool cancel_response = await CB_cancel_order(baiter_order_id);
                                if (cancel_response) {
                                    Debug.Print("CB-trade-baiter - cancelling successful, creating new " + baiter_Side + " order at price " + baiter_new_price.ToString());
                                    baiter_price = baiter_new_price;
                                    await TryCreateBaiterOrder();
                                }
                                else {
                                    Debug.Print("CB-trade-baiter - couldn't cancel the order?");
                                    baiter_changing_price = false;
                                }
                            }
                            else {
                                baiter_changing_price = false;  // this order doesn't require a cancel and re-post
                            }
                        }                        
                    }
                    break;

                case "error":
                    // Handle errors (like auth errors)
                    Console.WriteLine("Error: " + msg.ToString());
                    break;

                // these are openOrder tings
                case "open":
                case "received":  // will add the order to the dictionary here, but will only actually show on the UI when the open event is received
                    CB_Order updatedOrder;
                    try {
                        updatedOrder = JsonConvert.DeserializeObject<CB_Order>(json);
                        if (updatedOrder == null) throw new Exception("deserialized object was null");
                    }
                    catch (Exception ex) {
                        Debug.Print("CB-trade - tried to deserialize the wss received/open event in handleMessage, but failed.  json: " + json);
                        return;
                    }

                    // now we have to map some properties as wss uses different names
                    /*if (!string.IsNullOrEmpty(msg["order_id"]?.ToString())) {  // if the order_id property exists, then map it to the id variable
                        updatedOrder.id = msg["order_id"].ToString();
                    }*/

                    if (updatedOrder.order_id != null) {
                        updatedOrder.id = updatedOrder.order_id;
                    }

                    if (updatedOrder.order_id == baiter_order_id) {
                        updatedOrder.isBaiter = true;
                    }

                    if (!string.IsNullOrEmpty(msg["time"]?.ToString())) {  // if the order_id property exists, then map it to the id variable

                        if (!DateTime.TryParse(msg["time"].ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime createdAt)) {
                            createdAt = DateTime.MinValue; // or handle as needed
                        }
                        updatedOrder.created_at = createdAt;
                    }

                    updateOpenOrders(updatedOrder);  // no need to await, we are sending all the data it needs to update the open Orders
                    await getAndParseAccount(_productId);  // need to await this as it calls the accounts/account_id and pulls balance data from the CB API
                    break;

                case "match":  // when one of our orders is matched (partially)
                    CB_Order_matched match;
                    try {  // convert the json to the CB_Order_match object
                        match = JsonConvert.DeserializeObject<CB_Order_matched>(json);
                    }
                    catch (Exception ex) {
                        Debug.Print("CB-trade - tried to deserialize the wss match event in handleMessage, but failed.  json: " + json);
                        return;
                    }

                    parseOrderMatched(match);
                    break;

                case "done":  // when an order is done, don't try and update it, just grab all new data from the REST API and throw it up on the UI
                    Debug.Print("OK, should be a completed order?");

                    // refresh the closed orders, don't try and update with the sockets, why bother when it's a rare event that we can just pull the whole thing for.
                    await parseOpenOrders();
                    await parseClosedOrders();
                    await getAndParseAccount(_productId);

                    // check if this was market baiter
                    // need to deserialise?

                    CB_Order done_order;
                    try {  // convert the json to the CB_Order_match object
                        done_order = JsonConvert.DeserializeObject<CB_Order>(json);
                    }
                    catch (Exception ex) {
                        Debug.Print("CB-trade - tried to deserialize the wss done event in handleMessage, but failed.  json: " + json);
                        return;
                    }

                    if (!baiter_changing_price && (done_order.order_id == baiter_order_id)) {
                        Debug.Print("CB-trade - it seems the baiter order is complete/cancelled");
                        baiter_Active = false;
                        // i guess should raise an event for hte UI
                        OnBaiterComplete?.Invoke();
                    }

                    break;

                default:
                    Console.WriteLine(DateTime.Now + " - CB-trade - type: " + type);
                    break;
            }
        }

        private void parseOrderMatched(CB_Order_matched match) {
            // first, update the openOrders
            // acttuuaally first we need to figure out what the order_id is.
            // coinbase makes this difficult
            string order_id = "";
            if (match.taker_user_id != null) {
                // OK, this kinda means we are the taker?
                order_id = match.taker_order_id;
            }
            else if (match.maker_user_id != null) {
                order_id = match.maker_order_id;
            }

            if (!string.IsNullOrEmpty(order_id)) {

                if (openOrders.ContainsKey(order_id)) {
                    openOrders[order_id].remaining_size = openOrders[order_id].remaining_size - match.size;
                    
                    // now, baiter
                    if (openOrders[order_id].isBaiter) {
                        if (order_id == baiter_order_id) {
                            baiter_RemainingSize -= match.size;
                        }
                    }
                }
            }
        }

        public async Task<CB_Order> CB_place_order(string pair, string side, string price, string size, string type, bool post_only = false, string client_uid = "") {
            var response = await CoinbaseClient.CB_post_order(pair, side, price, size, type, post_only);

            // convert to Order
            CB_Order openedOrder;
            try {
                openedOrder = JsonConvert.DeserializeObject<CB_Order>(response);
                if (openedOrder == null) throw new Exception("deserialized object was null");
            }
            catch (Exception ex) {
                Debug.Print("CB-trade - tried to deserialize the response from opening a new order, but failed.  json: " + response);
                return null;
            }

            if (string.IsNullOrEmpty(openedOrder.order_id) && !string.IsNullOrEmpty(openedOrder.id)) {
                openedOrder.order_id = openedOrder.id;
            }

            return openedOrder;
        }

        /// <summary>
        /// cancels an order
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns>order ID of the order cancelled (if successful)</returns>
        public async Task<bool> CB_cancel_order(string order_id) {
            string response = await CoinbaseClient.CB_cancel_order(order_id);

            if (!string.IsNullOrEmpty(response)) {
                if (response == "\"" + order_id + "\"") return true;
            }
            Debug.Print("CB-trade - failed to cancel order?  respose: " + response + ", order id: " + order_id);
            return false;
        }

        private IEnumerable<CB_OrderBookEntry> ConvertArrayToEntries(JArray arr) {
            var result = new List<CB_OrderBookEntry>();
            foreach (var item in arr) {
                if (!decimal.TryParse(item[0].ToString(), out var price)) continue;
                if (!decimal.TryParse(item[1].ToString(), out var size)) continue;

                result.Add(new CB_OrderBookEntry { Price = price, Size = size });
            }
            return result;
        }

        public class DescendingComparer<T> : IComparer<T> where T : IComparable<T> {
            public int Compare(T x, T y) => y.CompareTo(x);
        }

    }
}
