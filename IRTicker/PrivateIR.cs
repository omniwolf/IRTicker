using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace IRTicker {
    public class PrivateIR {

        //private static readonly HttpClient client = new HttpClient();
        private Client IRclient;
        public Dictionary<string, Account> accounts = new Dictionary<string, Account>();
        private ApiCredential IRcreds;
        private IRTicker IRT;
        private ConcurrentQueue<IRClientData> IRQueue = new ConcurrentQueue<IRClientData>();
        private bool isDequeuing = false;
        private static readonly Object pIR_Lock = new Object();

        public string OrderBookSide = "Bid";  //  maintains which side of the order book we show in the AccountOrders_listview
        public string BaiterBookSide = "Bid"; // maintains which book we're baitin' on
        public string OrderTypeStr = "Market";
        public string BuySell = "Buy";
        public decimal Volume = 0;
        public decimal LimitPrice = 0;
        public string Crypto = "XBT";
        IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBids;
        IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedOffers;
        public ConcurrentDictionary<Guid, BankHistoryOrder> openOrders = new ConcurrentDictionary<Guid, BankHistoryOrder>();

        public bool marketBaiterActive = false;

        private DCE DCE_IR;
        private TelegramBot TGBot;

        /*public PrivateIR(string _BaseURL, string APIKey, string APISecret, IRTicker _IRT, DCE _DCE_IR) {
            PrivateIR_init(_BaseURL, APIKey, APISecret, _IRT, _DCE_IR);
        }*/  // don't think this constructor is used anymore..

        public PrivateIR() {
            // 
        }

        public void PrivateIR_init(string _BaseURL, string APIKey, string APISecret, IRTicker _IRT, DCE _DCE_IR, TelegramBot _TGBot) {
            IRT = _IRT;
            DCE_IR = _DCE_IR;
            TGBot = _TGBot;

            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(APISecret)) {
                Debug.Print(DateTime.Now + " cannot do private IR stuff, missing API key(s)");
                return;
            }

            IRcreds = new ApiCredential(APIKey, APISecret);

            var IRconf = new ApiConfig {
                BaseUrl = _BaseURL,
                Credential = IRcreds
            };
            IRclient = Client.Create(IRconf);
        }

        public void setTGBot(TelegramBot _TGBot) {
            TGBot = _TGBot;
        }

        public Dictionary<string, Account> GetAccounts() {
            lock (pIR_Lock) {
                try {
                    //accounts = (IRclient.GetAccounts()).ToDictionary(x => x.CurrencyCode.ToString().ToUpper(), x => x);
                    accounts =(IRclient.GetAccounts()).ToDictionary(x => x.CurrencyCode.ToString().ToUpper(), x => x);

                }
                catch (Exception ex) {
                    MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                        ex.Message, "Error - GetAccounts", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            return accounts;
        }

        public DigitalCurrencyDepositAddress GetDepositAddress(string crypto) {
            lock (pIR_Lock) {
                return IRclient.GetDigitalCurrencyDepositAddress(convertCryptoStrToCryptoEnum(crypto));
            }
        }

        //
        public DigitalCurrencyDepositAddress CheckAddressNow(string crypto, string address) {
            DigitalCurrencyDepositAddress result;
            lock (pIR_Lock) {
                try {
                    result = IRclient.SynchDigitalCurrencyDepositAddressWithBlockchain(address, convertCryptoStrToCryptoEnum(crypto));
                }
                catch (Exception ex) {
                    MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                        ex.Message, "Error - CheckAddressNow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            return result;
        }

        public BankOrder PlaceLimitOrder(string crypto, string fiat, OrderType? orderType, decimal price, decimal volume) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            if (orderType == null) orderType = (BuySell == "Buy" ? OrderType.LimitBid : OrderType.LimitOffer);
            if (price < 0) price = LimitPrice;
            if (volume < 0) volume = Volume;

            BankOrder orderResult;
            lock (pIR_Lock) {
                try {
                    orderResult = IRclient.PlaceLimitOrder(enumCrypto, enumFiat, orderType.Value, price, volume);
                }
                catch (Exception ex) {
                    MessageBox.Show("API error: " + ex.InnerException.Message, "Limit Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    orderResult = null;
                }
            }
            return orderResult;
        }

        public BankOrder PlaceMarketOrder(string crypto, string fiat, OrderType? orderType, decimal volume) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            if (orderType == null) orderType = (BuySell == "Buy" ? OrderType.MarketBid : OrderType.MarketOffer);
            if (volume < 0) volume = Volume;

            BankOrder orderResult;
            lock (pIR_Lock) {
                orderResult = IRclient.PlaceMarketOrder(enumCrypto, enumFiat, orderType.Value, volume);
            }
            return orderResult;
        }

        public Page<BankHistoryOrder> GetOpenOrders(string crypto, string fiat) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);
            Page<BankHistoryOrder> openOs;

            lock (pIR_Lock) {
                openOs = IRclient.GetOpenOrders(enumCrypto, enumFiat, 1, 7);
            }

            openOrders.Clear(); // clear the old one
            foreach (BankHistoryOrder order in openOs.Data) {
                if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) continue;
                openOrders.TryAdd(order.OrderGuid, order);
            }

            return openOs;
        }

        public Page<BankHistoryOrder> GetClosedOrders(string crypto, string fiat) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);
            Page<BankHistoryOrder> cOrders = null;
            List<BankHistoryOrder> allCOrders = new List<BankHistoryOrder>();

            int page = 1;
            do {
                lock (pIR_Lock) {
                    try {
                        cOrders = IRclient.GetClosedOrders(enumCrypto, enumFiat, page, 50);
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " - Failed to pull GetClosedOrders on page " + page + " - " + ex.Message);
                        throw ex;
                    }
                }

                foreach (BankHistoryOrder order in cOrders.Data) {
                    allCOrders.Add(order);
                }
                page++;
            }  while (page <= cOrders.TotalPages);

            if (page < cOrders.TotalPages) return null;  // we don't want to send partial results, we either get it all or die trying
            cOrders.Data = allCOrders;
            if ((TGBot != null) && ((cOrders.Data.Count() > 0))) TGBot.closedOrders(cOrders);
            return cOrders;
        }

        public BankOrder CancelOrder(string guid) {
            lock (pIR_Lock) {
                try {
                    return IRclient.CancelOrder(new Guid(guid));
                }
                catch (Exception ex) {
                    MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                        ex.InnerException.Message, "Error - CancelOrder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw ex;
                }
            }
        }

        private CurrencyCode convertCryptoStrToCryptoEnum(string crypto) {
            Enum.TryParse(Utilities.FirstLetterToUpper(crypto), out CurrencyCode enumCrypto);
            return enumCrypto;
        }

        public class IRClientData {
            public PrivateIREndPoints EndPoint { get; set; }
            public decimal LimitPrice { get; set; }
            public decimal Volume { get; set; }
            public CurrencyCode Crypto { get; set; }
            public CurrencyCode Fiat { get; set; }
            public int PageNum { get; set; }
            public int PageSize { get; set; }
            public OrderType orderType { get; set; }
            public string CryptoAddress { get; set; }
            public Guid guid { get; set; }
        }

        public void Enqueue(IRClientData IRdata) {
            IRQueue.Enqueue(IRdata);
            if (!isDequeuing) {
                isDequeuing = true;
                Dequeue();
            }
        }

        public void Enqueue(List<IRClientData> IRdataList) {
            foreach (IRClientData dat in IRdataList) {
                IRQueue.Enqueue(dat);
            }
            if (!isDequeuing) {
                isDequeuing = true;
                Dequeue();
            }
        }

        private async void Dequeue() {
            while (IRQueue.Count > 0) {
                if (IRQueue.TryDequeue(out IRClientData data)) {
                    switch (data.EndPoint) {
                        case PrivateIREndPoints.CancelOrder:
                            BankOrder bo = CancelOrder(data.guid.ToString());
                            break;
                        case PrivateIREndPoints.CheckAddress:
                            CheckAddressNow(data.Crypto.ToString(), data.CryptoAddress);
                            break;
                        case PrivateIREndPoints.GetAccounts:
                            // this one has a result.  should capture it and then call the draw func
                            break;
                    }
                }
            }
            isDequeuing = false;
        }

        public void compileAccountOrderBookAsync(string pair) {

            if (pair != (Crypto + "-" + DCE_IR.CurrentSecondaryCurrency)) return;
            if (!IRT.IRAccount_panel.Visible) {
                if (!marketBaiterActive) return;
            }

            List<string[]> accOrderListView = new List<string[]>();
            decimal estValue = 0;

            // here we grab the buy or sell order book, make a copy, and then sort it

                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCE_IR.IR_OBs[pair].Item2.ToArray();  // because we're buying from the sell orders
                orderedOffers = arrayBook.OrderBy(k => k.Key);
                //Debug.Print("--- Account picked the sell side, top order is: " + orderedBook.First().Key);

                arrayBook = DCE_IR.IR_OBs[pair].Item1.ToArray();  // because we're selling to the buy orders
                orderedBids = arrayBook.OrderByDescending(k => k.Key);
                //Debug.Print("--- Account picked the buy side, top order is: " + orderedBook.First().Key);

            int count = 1;
            decimal cumulativeVol = 0;
            decimal cumulativeValue = 0;
            decimal totalOrderValue = 0;
            decimal trackedOrderVolume = -1;

            // if we can parse the volume box, and it's a market order, let's work out the order value.  No need to track for limit order, can just do simple maths
            if (OrderTypeStr == "Market") {
                if (Volume > 0) {
                    trackedOrderVolume = Volume;
                }
            }
            //Debug.Print("OrderBookSide: " + OrderBookSide);

            foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> pricePoint in (OrderBookSide == "Offer" ? orderedOffers : orderedBids)) {
                decimal totalVolume = 0;
                bool includesMyOrder = false;

                foreach (KeyValuePair<string, DCE.OrderBook_IR> order in pricePoint.Value) {
                    totalVolume += order.Value.Volume;
                    if (trackedOrderVolume != -1) {  // only bother working out this stuff we have a real Tracked value
                        if (trackedOrderVolume > order.Value.Volume) {
                            totalOrderValue += (order.Value.Volume * pricePoint.Key);
                            trackedOrderVolume -= order.Value.Volume;
                        }
                        else {  // ok, this is the last order to fill our proposed order
                            totalOrderValue += (trackedOrderVolume * pricePoint.Key);
                            trackedOrderVolume = 0;  // no more counting
                        }
                    }

                    if (openOrders.ContainsKey(new Guid(order.Key))) {
                        includesMyOrder = true;
                    }
                }

                if (count < 10) {  // there are 9 rows on the OB listview
                    cumulativeVol += totalVolume;
                    cumulativeValue += pricePoint.Key * totalVolume;
                    accOrderListView.Add(new string[] { count.ToString(), pricePoint.Key.ToString(), Utilities.FormatValue(totalVolume), Utilities.FormatValue(cumulativeVol), Utilities.FormatValue(cumulativeValue), (includesMyOrder ? "true" : "false")  });
                    count++;
                }
                // this can be read like: "if we've finished populating the listview, but we still have more orders required 
                // to calculate our market order size, then keep looping
                if ((count > 9) && (trackedOrderVolume <= 0)) break;
            }

            if ((OrderTypeStr == "Market") && (trackedOrderVolume >= 0)) {
                if (trackedOrderVolume > 0) {
                    estValue = -1; //"Not enough depth!";
                }
                else {
                    estValue = totalOrderValue;
                }
            }
            // if it's a limit order, then the AccountEstOrderValue field is calculated manually (no need for OB), so here we need to make sure we don't clear it
            // this else is saying "if it's a market order, but we didn't engage trackedOrderVolume, then they probably have unparsable text in the vol box, so clear the estimate value label"
            else if (OrderTypeStr == "Market") estValue = -2; // ""
            IRT.drawAccountOrderBook(new Tuple<decimal, List<string[]>>(estValue, accOrderListView), pair);
           // return Task.CompletedTask;
        }


        public async Task marketBaiterLoopAsync(string crypto, string fiat, decimal volume, decimal limitPrice) {

            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> baiterBook;
            BankOrder placedOrder = null;
            Task rateLimitPlaceOrder = Task.Delay(1);
            string pair = crypto + "-" + fiat;
            
            decimal distanceFromTopOrder = (decimal)(Math.Pow(0.1, DCE_IR.currencyFiatDivision[crypto]) * 5);  // how far infront of the best order should we be?  will be different for different cryptos
            if (BaiterBookSide == "Offer") distanceFromTopOrder = distanceFromTopOrder * -1;

            Debug.Print("MBAIT: distance from top: " + distanceFromTopOrder);
            IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Starting market baiter!"));

            while (marketBaiterActive) {
                if (BaiterBookSide == "Offer") baiterBook = orderedOffers;
                else baiterBook = orderedBids;
                

                //if ((baiterBook.First().Value).ElementAt(0).Value.OrderType.EndsWith(BaiterBookSide)) {  // first make sure we have the right order book
                if (placedOrder == null) {  // no order.  let's create one.
                    Debug.Print(DateTime.Now + " - MBAIT: no bait guid, lets create it. Top order: " + baiterBook.FirstOrDefault().Key);

                    decimal orderPrice;

                    // now we need to make sure this orderPrice is not bigger/smaller than the best offer/bid (ie turning the order into a market order)

                    // this stuff doesn't work yet!  more testing and fixes needed.
                    // pulled this feature.  Would need to pull GetOpenOrders at every loop to make sure it wasn't our order at the top
                    bool OurOrderAtTop = false;  // let's try and discover if the best current order is a separate order made by this acccount.  If so, pretend it doesn't exist
                    foreach (var openO in openOrders) {
                        foreach (var topOrder in baiterBook.First().Value) {
                            if (openO.Key.ToString() == topOrder.Key) {
                                OurOrderAtTop = true;
                                Debug.Print("MBAIT: we have an order at the top already, let's try and ignore it - $" + topOrder.Value.Price);
                                break;
                            }
                        }
                        if (OurOrderAtTop) break;
                    }

                    // if our order is at the top, use the second order to base the price off.
                    orderPrice = (OurOrderAtTop ? baiterBook.ElementAt(1).Key + distanceFromTopOrder : baiterBook.First().Key + distanceFromTopOrder);

                    //orderPrice = baiterBook.First().Key + distanceFromTopOrder;
                    
                    if (BaiterBookSide == "Bid") {
                        Debug.Print("MBAIT: bid order price: " + orderPrice);

                        // here we check if the order is too high for the OB, or too high for the limit price we set
                        if (orderPrice > DCE_IR.IR_OBs[pair].Item2.Keys.Min()) {
                            Debug.Print("MBAIT: orderPrice (" + orderPrice + ") is greater than the lowest bid - " + DCE_IR.IR_OBs[pair].Item2.Keys.Min());
                            IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "The spread is too toight to fit in an order!  Maybe just market sell?"));
                            Thread.Sleep(10000);
                            continue;  // master while loop
                        }
                        else if (orderPrice > limitPrice) {
                            orderPrice = limitPrice;  // never go over the limitPrice
                            Debug.Print("MBAIT: order too high, limited to " + limitPrice);
                        }
                    }
                    else {
                        Debug.Print("MBAIT: offer order price: " + orderPrice);

                        // check if the order is too low for the OB or lower than our set limit
                        if (orderPrice < DCE_IR.IR_OBs[pair].Item1.Keys.Max()) {
                            Debug.Print("MBAIT: orderPrice (" + orderPrice + ") is less than the highest offer - " + DCE_IR.IR_OBs[pair].Item1.Keys.Max());
                            IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "The spread is too toight to fit in an order!  Maybe just market buy?"));
                            Thread.Sleep(10000);
                            continue;  // master while loop
                        }
                        else if (orderPrice < limitPrice) {
                            orderPrice = limitPrice;  // never go under the limitPrice
                            Debug.Print("MBAIT: order too low, limited to " + limitPrice);
                        }
                    }
                    Debug.Print("MBAIT: placing order at " + orderPrice);
                    try {
                        if (!rateLimitPlaceOrder.IsCompleted) await rateLimitPlaceOrder;  // if we haven't waited a full second yet, wait.
                        placedOrder = PlaceLimitOrder(crypto, DCE_IR.CurrentSecondaryCurrency,
                            (BaiterBookSide == "Bid" ? OrderType.LimitBid : OrderType.LimitOffer), orderPrice, volume);
                        if (placedOrder == null) {
                            var res = MessageBox.Show("Failed to place the order.  Do you want to cancel market baiter?", "Market Baiter", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (res == DialogResult.Yes) marketBaiterActive = false;
                        }
                        //Thread.Sleep(1050 - (Properties.Settings.Default.UITimerFreq + 50));  // an order must be left alive for at least a second or rate limiting will happen
                        rateLimitPlaceOrder = Task.Delay(1001);  // start a timer.  can only try and create a new order after a second has passed
                    }
                    catch (Exception ex) {
                        Debug.Print("MBAIT: trid to create an order, but it failed: " + ex.Message);
                        //MessageBox.Show("Error creating market baiter order, cancelling market baiter. Error below." + Environment.NewLine + Environment.NewLine +
                         //   ex.Message, "Market baiter error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //marketBaiterActive = false;
                    }
                    IRT.updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, /*PrivateIREndPoints.GetAccounts, */PrivateIREndPoints.UpdateOrderBook });

                }
                else {  // an order is in play
                    // keeps track of how many pricePoint order dictionaries we have gone through.  the first is special - 
                    // if we find our order in the first it means we're still at the spread which is good.
                    int pricePointCount = 0;
                    bool foundOrder = false;
                    bool retryRequired = false;  // set to true if we need to repeat the master loop again, probably due to nonce error

                    foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> pricePoint in baiterBook) {
                        // let's see if this price level has an order in it that is one of our OTHER (non-market baiter) orders.  If it is, pretend it's not there
                        // I've put this idea to rest.  would need to pull the GetOpenOrders every time we looped to see if the top order was mine.  Too inefficient
                        if (pricePointCount == 0) {
                            if (!pricePoint.Value.ContainsKey(placedOrder.OrderGuid.ToString())) {  // before we check let's make sure our actual baiter order isn't here
                                bool continueBaiterLoop = false;
                                foreach (KeyValuePair<string, DCE.OrderBook_IR> orderAtPrice in pricePoint.Value) {
                                    foreach (var openO in openOrders) {
                                        if ((orderAtPrice.Key == openO.Key.ToString()) && (orderAtPrice.Key != placedOrder.OrderGuid.ToString())) {  // it's ours, but not the bait order
                                            continueBaiterLoop = true;  // the order at the spread is ours, let's not compete against it.
                                            //Debug.Print("MBAIT: It appears an order at price $" + pricePoint.Key + " is our own.  Ignore and move to the next price level");
                                            break;
                                        }
                                    }
                                    if (continueBaiterLoop) break;
                                }
                                // why do we duplicate this search?  Because if we already know about this order, then let's not pull GetOpenOrders every time.  Should only
                                // be duplicated the first time, and then we'll know about our existing order, and we'll be good.
                                if (!continueBaiterLoop) {  // OK, we didn't find it.  let's grab the openOrders and search again, maybe we only recently created it
                                    Page<BankHistoryOrder> openOs;
                                    try {
                                        openOs = GetOpenOrders(Crypto, DCE_IR.CurrentSecondaryCurrency);
                                    }
                                    catch (Exception ex) {
                                        Debug.Print("MBAIT: failed to get open orders due to: " + ex.Message);
                                        retryRequired = true;
                                        break;
                                    }
                                    foreach (KeyValuePair<string, DCE.OrderBook_IR> orderAtPrice in pricePoint.Value) {
                                        foreach (var openO in openOrders) {
                                            if ((orderAtPrice.Key == openO.Key.ToString()) && (orderAtPrice.Key != placedOrder.OrderGuid.ToString())) {  // it's ours, but not the bait order
                                                continueBaiterLoop = true;  // the order at the spread is ours, let's not compete against it.
                                                Debug.Print("MBAIT: After pulling new open orders, it appears an order at price $" + pricePoint.Key + " is our own.  Ignore and move to the next price level");
                                                break;
                                            }
                                        }
                                        if (continueBaiterLoop) break;
                                    }
                                }
                                if (continueBaiterLoop) continue;  // continues the baitorBook loop, ie moves to the second price level, but does not increase pricePointCount
                            }
                        }


                        if (pricePoint.Value.ContainsKey(placedOrder.OrderGuid.ToString())) {
                            foundOrder = true;
                            if (pricePointCount > 0) {  // our order has been beaten by another. lez cancel and start again.  if == 0 then we're the top of the book, do nothing.
                                if (placedOrder.Price != limitPrice) {  // if we're at the limit price, just leave the order, do not cancel.
                                    Debug.Print("MBAIT: our order has been beaten.  cancelling it...");
                                    BankOrder bo = new BankOrder();
                                    try {
                                        bo = CancelOrder(placedOrder.OrderGuid.ToString());
                                    }
                                    catch (Exception ex) {
                                        Debug.Print("MBAIT: trying to cancel the order because it got beat, but failed due to: " + ex.Message);
                                        retryRequired = true;
                                        break;
                                    }
                                    if (bo.Status == OrderStatus.Cancelled) {
                                        Debug.Print("MBAIT: cancel order was successful");
                                        //if (bo.VolumeFilled != 0) IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Nibble..."));

                                        volume = bo.VolumeOrdered - bo.VolumeFilled;
                                        IRT.updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });
                                        placedOrder = null;
                                    }
                                    else {
                                        Debug.Print("MBAIT: FAILED TO CANCEL ORDER!  why?  current status: " + bo.Status);
                                    }
                                }
                                //else Debug.Print("MBAIT: our order is at the limit, just gonna leave it.  price: " + placedOrder.Price);
                            }
                            else {
                                if (pricePoint.Value[placedOrder.OrderGuid.ToString()].Volume != volume) {
                                    volume = pricePoint.Value[placedOrder.OrderGuid.ToString()].Volume;
                                    IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Nibble...."));
                                }
                            }
                            break;  // order book foreach
                        }
                        pricePointCount++;
                    }
                    if (retryRequired) { Thread.Sleep(100); continue; }
                    if (!foundOrder) {
                        Debug.Print("MBAIT: Our order doesn't exist in the OB, possibly filled? " + placedOrder.OrderGuid.ToString());
                        Page<BankHistoryOrder> closedOs;
                        try {
                            closedOs = GetClosedOrders(crypto, fiat);
                        }
                        catch (Exception ex) {
                            Debug.Print("MBAIT: Damnit, can't pull closed orders for some reason: " + ex.Message);
                            Thread.Sleep(100);
                            continue;
                        }
                        
                        foreach (BankHistoryOrder closedO in closedOs.Data) {
                            if (closedO.OrderGuid == placedOrder.OrderGuid) {
                                if (closedO.Status == OrderStatus.Filled) {
                                    Debug.Print("MBAIT: our order got filled.  sweet.");
                                    IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Order filled!"), true);
                                    placedOrder = null;
                                    marketBaiterActive = false;
                                    break;  // closed orders foreach
                                }
                                else {
                                    Debug.Print("MBAIT: order is closed, but not filled?  Status: " + closedO.Status);
                                }
                            }
                        }

                        if (null != placedOrder) {
                            Debug.Print("MBAIT: OK, let's see if it's still open.");
                            Page<BankHistoryOrder> openOs = GetOpenOrders(crypto, fiat);
                            foreach (BankHistoryOrder openO in openOs.Data) {
                                if (openO.OrderGuid == placedOrder.OrderGuid) {
                                    foundOrder = true;
                                    if (openO.Status == OrderStatus.Open) {
                                        Debug.Print("MBAIT: The order is still open??");
                                    }
                                    else {
                                        Debug.Print("MBAIT: the order is not open, it is: " + openO.Status + " orright, let's cancel it.  something weird is happening");
                                        BankOrder cancelledOrder;
                                        try {
                                            cancelledOrder = CancelOrder(placedOrder.OrderGuid.ToString());
                                        }
                                        catch (Exception ex) {
                                            Debug.Print("MBAIT: couldn't cancel the order?? will try again.  Error:" + ex.Message);
                                            Thread.Sleep(100);
                                            continue;
                                        }
                                        Debug.Print("MBAIT: cancelled status: " + cancelledOrder.Status);
                                        if (cancelledOrder.Status == OrderStatus.Cancelled) {
                                            placedOrder = null;
                                        }
                                    }
                                    foundOrder = true;  // just so we don't get caught below and claim we didn't find it
                                    break;
                                }
                            }
                            IRT.synchronizationContext.Post(new SendOrPostCallback(o => { IRT.drawOpenOrders((IEnumerable<BankHistoryOrder>)o); }), openOs.Data);
                            if (!foundOrder) {  // if we still haven't found it (ie it's not in the open or closed order lists), it must be cancelled I guess?  re-create.
                                Debug.Print("MBAIT: order wasn't in open or closed orders, so we'll re-create it");
                                placedOrder = null;
                            }
                        }

                        // if we get here and the marketBaiterActive is still true, then either a) the order has been manually cancelled by the user, or maybe it's actually there, but it wasn't present in
                        // the order book when we searched it.. maybe too early.  let's pause and try searching again
                        /*if (IRT.marketBaiterActive && !stillOpen) {
                            Debug.Print("MBAIT: nope, order not filled.  maybe cancelled?");
                            if (OrderSearchCount > 0) {
                                Debug.Print("MBAIT: still can't find it.  creating a new order...");
                                placedOrder = null;
                                OrderSearchCount = 0;  // reset it
                            }
                            else {
                                Thread.Sleep(500);
                                OrderSearchCount++;  // let's loop another time, maybe the order will appear
                                Debug.Print("MBAIT: let's loop again, maybe we'll find it...");
                            }
                        }*/
                    }
                }
                //Debug.Print("sleeping for " + (Properties.Settings.Default.UITimerFreq + 50).ToString());
                Thread.Sleep(Properties.Settings.Default.UITimerFreq + 50);  // refresh a bit slower than our OB updates, so any updates should be made before this loop tries to read them
            }  // end master while loop

            if (placedOrder != null) {
                Debug.Print("MBAIT: master loop finished, let's cancel the order if it still exists...");
                BankOrder bo;
                try {
                    bo = CancelOrder(placedOrder.OrderGuid.ToString());
                }
                catch (Exception ex) {
                    Debug.Print("MBAIT: couldn't cancel the order... weird.  message: " + ex.Message);
                    bo = new BankOrder() { Status = OrderStatus.Open };  // fake it for below if statement
                }
                if (bo.Status == OrderStatus.Cancelled) {
                    Debug.Print("MBAIT: cancel order was successful");
                    IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Market baiter stopped, existing order cancelled."));
                    placedOrder = null;
                }
                else {
                    Debug.Print("MBAIT: couldn't cancel the order?? guid: " + bo.OrderGuid);
                    IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Market baiter stopped, but order couldn't be cancelled?"));
                }
            }
        }

        public enum PrivateIREndPoints {
            GetAccounts,
            GetAddress,
            GetOpenOrders,
            GetClosedOrders,
            CheckAddress,
            PlaceMarketOrder,
            PlaceLimitOrder,
            CancelOrder,
            UpdateOrderBook
        }
    }
}
