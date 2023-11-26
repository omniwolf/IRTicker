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
        private IRAccountsForm IRAF;  // yeah lazy, instead of renaming this to IRAF, just leave it as IRT and then the rest of the code all works
        private static readonly Object pIR_Lock = new Object();

        public string OrderBookSide = "Bid";  //  maintains which side of the order book we show in the AccountOrders_listview
        public string BaiterBookSide = "Bid"; // maintains which book we're baitin' on
        public string OrderTypeStr = "Market";
        public string BuySell = "Buy";
        public decimal Volume = 0;
        public decimal LimitPrice = 0;
        public string SelectedCrypto = "XBT";
        public List<string> AvgPriceSelectedCrypto = new List<string>();  // This will list all the cryptos chosen in any AccAvgPrice forms that are open, so we know which crypto we need to get more closed orders for
        public ConcurrentBag<string> fiatCurrenciesSelected = new ConcurrentBag<string>();  // it's important that only the key is used here, because the value (the control and 
        IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBids;
        IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedOffers;
        public ConcurrentBag<Guid> openOrders = new ConcurrentBag<Guid>();
        public DateTime? earliestClosedOrderRequired = null;  // optimise closed orders by only pulling what's required.  If null we just pull a static 7 to cover the closed orders UI
        //private bool firstClosedOrdersPullDone = false;  // we need to pull ALL orders intially so we have a record of all guids for announcing new closed orders
        private List<string> getClosedOrdersLock = new List<string>();  // will hold crypto pairs.  If a crypto pair is in this List, it means we're currently pulling data from the API, so don't try and do it again.
        private Dictionary<string, long> closedOrdersCount;  // keeps a count of how many closed orders each pair has so we can maybe try and get to the bottom of this weird issue where I sometimes see less closed orders than actually exists
        //private bool reportClosedOrders = true;  // whether we use the closed orders list view to report the progress of bulk pulling all the closed orders (it's a long process)
        private DateTime APIKeyChanged = DateTime.Now;  // records when we changed the APIKey so we can wait for a period (5 seconds?) before believing that closed orders are from the new APIKey.  If we use them immediately then often we get ClosedOrders from the old APIKey

        public BankOrder placedOrder = null;
        public decimal baiterLiveVol = 0;  // this holds the current value of the baiter order, ie if there has been nibbles this will be the orginal volume minus nibbles

        public bool marketBaiterActive = false;

        private DCE DCE_IR;
        private TelegramBot TGBot;

        /*public PrivateIR(string _BaseURL, string APIKey, string APISecret, IRTicker _IRT, DCE _DCE_IR) {
            PrivateIR_init(_BaseURL, APIKey, APISecret, _IRT, _DCE_IR);
        }*/  // don't think this constructor is used anymore..

        public PrivateIR() {
            // 
        }

        public void PrivateIR_init(string APIKey, string APISecret, IRAccountsForm _IRAF, DCE _DCE_IR, TelegramBot _TGBot) {
            IRAF = _IRAF;  // reminder - this is the IRAccountsFrom object
            DCE_IR = _DCE_IR;
            TGBot = _TGBot;

            //firstClosedOrdersPullDone = false;  // reset to false
            closedOrdersCount = new Dictionary<string, long>();

            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(APISecret)) {
                Debug.Print(DateTime.Now + " cannot do private IR stuff, missing API key(s)");
                return;
            }

            IRcreds = new ApiCredential(APIKey, APISecret);

            var IRconf = new ApiConfig {
                BaseUrl = DCE_IR.BaseURL,
                Credential = IRcreds
            };
            IRclient = Client.Create(IRconf);
            Task.Run(() => {
                GetAccounts();  // maybe need to put a try around this?  API fails shouldn't cause an app crash
                if ((null != IRAF) && !IRAF.IsDisposed) {
                    IRAF.DrawIRAccounts(accounts);
                    IRAF.drawOpenOrders(GetOpenOrders(SelectedCrypto, DCE_IR.CurrentSecondaryCurrency).Data);
                }

                populateClosedOrders();  // i don't think we need to do this - just whenever the closed orders for a pair gets pulled, that's the first pull?
            });
        }

        public void setTGBot(TelegramBot _TGBot) {
            TGBot = _TGBot;
            Task.Run(() => populateClosedOrders());  // i don't think we need to do this - just whenever the closed orders for a pair gets pulled, that's the first pull?
        }

        public void setIRAF(IRAccountsForm _IRAF) {
            IRAF = _IRAF;
            //Task.Run(() => populateClosedOrders());  // don't think we need to do this anymore - we already do a big pull when initialising pIR
        }

        public Dictionary<string, Account> GetAccounts(string APIKey = "", string APISecret = "") {
            if (string.IsNullOrEmpty(APIKey) || (APIKey == Properties.Settings.Default.IRAPIPubKey)) {
                lock (pIR_Lock) {
                    accounts = (IRclient.GetAccounts()).ToDictionary(x => x.CurrencyCode.ToString().ToUpper(), x => x);
                }
                return accounts;
            }
            else {  // we have been sent custom credentials, create a new client and use it.  Usually used for balance checker, checking SGD account or something.
                ApiCredential IRcredsAlt = new ApiCredential(APIKey, APISecret);

                var IRconf = new ApiConfig
                {
                    BaseUrl = DCE_IR.BaseURL,
                    Credential = IRcredsAlt
                };
                        
                Client IRclientAlt = Client.Create(IRconf);
                return (IRclientAlt.GetAccounts()).ToDictionary(x => x.CurrencyCode.ToString().ToUpper(), x => x);
            }
        }

        public DigitalCurrencyDepositAddress GetDepositAddress(string crypto) {
            if (crypto.ToUpper() == "BTC") crypto = "XBT";
            lock (pIR_Lock) {
                return IRclient.GetDigitalCurrencyDepositAddress(convertCryptoStrToCryptoEnum(crypto));
            }
        }

        //
        public DigitalCurrencyDepositAddress CheckAddressNow(string crypto, string address) {
            if (crypto.ToUpper() == "BTC") crypto = "XBT";
            DigitalCurrencyDepositAddress result;
            lock (pIR_Lock) {
                result = IRclient.SynchDigitalCurrencyDepositAddressWithBlockchain(address, convertCryptoStrToCryptoEnum(crypto));
            }
            return result;
        }

        public BankOrder PlaceLimitOrder(string crypto, string fiat, OrderType? orderType, decimal price, decimal volume) {
            if (crypto.ToUpper() == "BTC") crypto = "XBT";
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            if (orderType == null) orderType = (BuySell == "Buy" ? OrderType.LimitBid : OrderType.LimitOffer);
            if (price < 0) price = LimitPrice;
            if (volume < 0) volume = Volume;

            BankOrder orderResult;
            lock (pIR_Lock) {
                orderResult = IRclient.PlaceLimitOrder(enumCrypto, enumFiat, orderType.Value, price, volume);
            }
            if ((orderResult.Status == OrderStatus.Open) || (orderResult.Status == OrderStatus.PartiallyFilled))
                openOrders.Add(orderResult.OrderGuid);
            return orderResult;
        }

        public BankOrder PlaceMarketOrder(string crypto, string fiat, OrderType? orderType, decimal volume) {
            if (crypto.ToUpper() == "BTC") crypto = "XBT";
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
            if (crypto.ToUpper() == "BTC") crypto = "XBT";
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);
            Page<BankHistoryOrder> openOs;

            lock (pIR_Lock) {
                openOs = IRclient.GetOpenOrders(enumCrypto, enumFiat, 1, 7);
            }

            openOrders = new ConcurrentBag<Guid>(); // clear the old one
            foreach (BankHistoryOrder order in openOs.Data) {
                if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) continue;
                openOrders.Add(order.OrderGuid);
            }

            return openOs;
        }

        // When the user changes the API Key, this method gets called and records the timestamp.  Then the GetClosedOrders(...) method can know when an APIKey change was made
        // and ignore Closed Orders made around this time, as I have often found I receive old closed orders from the old API key and it gets mixed in with the new one.
        public void APIKeyHasChanged() {
            APIKeyChanged = DateTime.Now;
            //firstClosedOrdersPullDone = false;

            if ((null != IRAF) && (!IRAF.IsDisposed)) {
                IRAF.ResetLabels();  // set all the account values to "-"
                IRAF.drawClosedOrders(null);
                IRAF.drawOpenOrders(null);
            }
        }

        // this thing runs through all IR pairs and pulls all orders for the pair to the notifiedOrders dictionary
        // so that we have a list of closed orders to compare when a new one comes in
        // only gets called if we know the IRAF form is running
        public void populateClosedOrders() {
            // now we pull all closed orders for all pairs to ensure we have all order guids listed in the TG Bot notifiedOrders dictionary

            Page<BankHistoryOrder> cOrders;
            //reportClosedOrders = true;

            foreach (string primaryCode in DCE_IR.PrimaryCurrencyList) {
                Debug.Print("Big pull of closed orders, pulling " + primaryCode);
                foreach (string secondaryCode in DCE_IR.SecondaryCurrencyList) {
                    try {
                        cOrders = GetClosedOrders(primaryCode, secondaryCode, true);  // grab the closed orders on a schedule, this way we will know if an order has been filled and can alert.

                        // need to go if the current primary/secondary is what's shown on IRAccounts, then draw it
                        if ((SelectedCrypto == primaryCode) && (DCE_IR.CurrentSecondaryCurrency == secondaryCode) &&
                            (null != IRAF) && !IRAF.IsDisposed && (null != cOrders)) 
                        {
                            IRAF.drawClosedOrders(cOrders.Data);
                            //reportClosedOrders = false;  // stop reporting, we have drawn the actual orders.
                        }
                    }
                    catch (Exception ex) {
                        string errorMsg = ex.Message;
                        if (ex.InnerException != null) {
                            errorMsg = ex.InnerException.Message;
                        }
                        Debug.Print(DateTime.Now + " - PrivateIR_init sub, trying to pull closed orders for " + primaryCode + "-" + secondaryCode + ", but it failed: " + errorMsg);
                    }
                }
                Debug.Print("Big pull of closed orders done: " + primaryCode);
            }
            //firstClosedOrdersPullDone = true;

            // if the IRAF is open, we should display the closed orders
            if ((null != IRAF) && !IRAF.IsDisposed) IRAF.drawClosedOrders(GetClosedOrders(SelectedCrypto, DCE_IR.CurrentSecondaryCurrency).Data);
        }

        /// <summary>
        /// Actually - this uses GetClosedFilledOrders(...)
        /// </summary>
        /// <param name="crypto"></param>
        /// <param name="fiat"></param>
        /// <param name="initialPull"></param>
        /// <returns></returns>
        public Page<BankHistoryOrder> GetClosedOrders(string crypto, string fiat, bool initialPull = false) {

            //if (!firstClosedOrdersPullDone && !initialPull) return null;  // If we haven't done the first giant pull, and something tries to do a closed order pull, ignore it.  Only start servicing calls once the initial pull is complete

            if (crypto.ToUpper() == "BTC") crypto = "XBT";
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);
            string pair = enumCrypto.ToString() + "-" + enumFiat.ToString();

            if (getClosedOrdersLock.Contains(pair)) return null;
            else {
                getClosedOrdersLock.Add(pair);  // no attempts to pull closed orders on this pair until the function is finished.
            }


            Page<BankHistoryOrder> cOrders = null;
            List<BankHistoryOrder> allCOrders = new List<BankHistoryOrder>();
            string APIkey;  // let's try and track which key we use
            if (!closedOrdersCount.ContainsKey(pair)) {
                closedOrdersCount.Add(pair, 0);
            }

            int pageSize = 10;  // we only need 7 for the UI, but grab 10 in case 
            // don't do this anymore - we don't ever need a big pull.
            // Either we have a date, need to pull all orders newer than or equal to this date, or it's the first run and we need to pull everything
            // also - we only pull  more than 8 if the crypto we're pulling is the currently chosen crypto.  `Crypto` is the currently chosen crypto... (i know.. great var name)
            //if ((earliestClosedOrderRequired.HasValue && AvgPriceSelectedCrypto.Contains(crypto) && (fiatCurrenciesSelected.Contains(fiat))) || initialPull)  {  
                //pageSize = 5000;
            //}

            int page = 1;
            do {
                APIkey = IRcreds.Key;
                lock (pIR_Lock) {
                    cOrders = IRclient.GetClosedFilledOrders(enumCrypto, enumFiat, page, pageSize);  // we don't care about cancelled orders
                }
                //if (initialPull && (null != IRAF) && !IRAF.IsDisposed && (null != cOrders)) IRAF.ReportClosedOrderStatus(crypto, page + "/" + cOrders.TotalPages);

                if (APIkey != IRcreds.Key) {  // i don't think this will ever happen.. but who knows  /// ok.. seems to happen every time we change APIkey.  but if stops errors, so leave it
                    Debug.Print("uh oh.. it's unclear which API key we used.. probably should just bail" + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                    getClosedOrdersLock.Remove(pair);
                    return null;
                }

                if (cOrders.TotalItems <= 0) break;  // we have no orders, let's get out of here
                //if ((page == 1) && (crypto == "XBT") && (fiat == "AUD")) Debug.Print(DateTime.Now + " - GetClosedOrders(" + crypto + "-" + fiat + "): total pages: " + cOrders.TotalPages + " and total items: " + cOrders.TotalItems + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);

                foreach (BankHistoryOrder order in cOrders.Data) {
                    allCOrders.Add(order);
                }
                page++;
                // flipping this do/while - now we always break if there's no avgPrice form open.  only reason we spin through is to get all orders to calculate values for that form.
                /*if (!initialPull) {  // only want to consider breaking out of this loop early if this isn't the first pull.  If it's the first pull we need ALL closed orders
                    if (!AvgPriceSelectedCrypto.Contains(crypto) || (!fiatCurrenciesSelected.Contains(fiat))) break;  // if we're pulling orders for some different crypto, just bail
                    if (!earliestClosedOrderRequired.HasValue) break;  // we only need to get the first page if we don't have a date
                    else {  // ok we do have a date, need to work out if we bail or continue here
                        if (allCOrders.Last().CreatedTimestampUtc < earliestClosedOrderRequired.Value.ToUniversalTime()) {
                            break;
                        }
                    }
                }*/

                if (!earliestClosedOrderRequired.HasValue) break;  // we only need to get the first page if we don't have a date

            } while (allCOrders.Last().CreatedTimestampUtc >= earliestClosedOrderRequired.Value.ToUniversalTime());

            if (cOrders.TotalItems >= closedOrdersCount[pair]) {
                closedOrdersCount[pair] = cOrders.TotalItems;
            }
            else {
                Debug.Print("pIR: We have LESS closed orders for " + pair + " than we did at the last closedOrders pull?? why???  Before: " + closedOrdersCount[pair] + " after: " + cOrders.TotalItems);
            }

            if ((crypto == "XBT") && (fiat == "AUD")) Debug.Print("TOTAL ITEMS pulled for BTC-AUD: " + allCOrders.Count);

                //if (page < cOrders.TotalPages) return null;  // we don't want to send partial results, we either get it all or die trying  // AACTTUALLY... partial results are good now
                cOrders.Data = allCOrders;
            if (cOrders.Data.Count() > 0) {
                // only call the TG closed orders sub if we've waited 5 seconds after an APIKey change or it's the initial pull of all orders
                //if ((TGBot != null) && (DateTime.Now > APIKeyChanged + TimeSpan.FromMinutes(1))) TGBot.closedOrders(cOrders, APIkey);
                if ((TGBot != null) /*&& ((DateTime.Now > APIKeyChanged + TimeSpan.FromMinutes(1)) || initialPull)*/) TGBot.closedOrders(cOrders, APIkey);
                if ((null != IRAF) && !IRAF.IsDisposed) IRAF.SignalAveragePriceUpdate(cOrders);
                //if (initialPull && IRT.IRAccount_panel.Visible) IRT.drawClosedOrders(cOrders.Data);  // this isn't the right place to do this
            }
            //else Debug.Print("gecClosed orders, no orders for " + crypto + "-" + fiat);
            getClosedOrdersLock.Remove(pair);
            return cOrders;
        }

        public BankOrder CancelOrder(string guid) {
            lock (pIR_Lock) {
                return IRclient.CancelOrder(new Guid(guid));
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

        public void compileAccountOrderBookAsync(string pair) {

            if (pair != (SelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency)) return;
            if ((null == IRAF) || IRAF.IsDisposed) return;

            List<decimal[]> accOrderListView = new List<decimal[]>();
            decimal estValue = 0;  // this appears to be the total value of the order currently in the fields on the form

            // here we grab the buy and sell order book, make a copy, and then sort it
            // why sort both?  because even though we only view one side, we may need the other  to do some calculations or whatever
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
            decimal trackedOrderVolume = -1;  // this holds the size of the order so we can work out how much it's worth by simulating a market order as we iterate through the orders

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

                    if (openOrders.Contains(new Guid(order.Key))) {
                        includesMyOrder = true;
                    }
                }

                if (count < 10) {  // there are 9 rows on the OB listview
                    cumulativeVol += totalVolume;
                    cumulativeValue += pricePoint.Key * totalVolume;
                    accOrderListView.Add(new decimal[] { count, pricePoint.Key, totalVolume, cumulativeVol, cumulativeValue, (includesMyOrder ? 1 : 0) }); ;
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
            IRAF.drawAccountOrderBook(new Tuple<decimal, List<decimal[]>>(estValue, accOrderListView), pair);  // why a tuple and not just separate variables?  We need it as one to insert into the synchronisation thing in the drawAccountOrderBook sub
           // return Task.CompletedTask;
        }

        public async Task marketBaiterLoopAsync(string crypto, string fiat, decimal volume, decimal limitPrice) {

            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> baiterBook;
            Task rateLimitPlaceOrder = Task.Delay(1);
            string pair = crypto + "-" + fiat;
            baiterLiveVol = volume;
            
            decimal distanceFromTopOrder = (decimal)(Math.Pow(0.1, DCE_IR.currencyDecimalPlaces[crypto].Item2) * 5);  // how far infront of the best order should we be?  will be different for different cryptos
            if (BaiterBookSide == "Offer") distanceFromTopOrder = distanceFromTopOrder * -1;

            Debug.Print("MBAIT: distance from top: " + distanceFromTopOrder);
            IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Starting market baiter!"));

            while (marketBaiterActive) {
                if (BaiterBookSide == "Offer") baiterBook = orderedOffers;
                else baiterBook = orderedBids;

                if ((baiterBook == null) || (baiterBook.Count() == 0)) {
                    Debug.Print("MBAIT: no orders in the order book to evaluate..");
                }
                //if ((baiterBook.First().Value).ElementAt(0).Value.OrderType.EndsWith(BaiterBookSide)) {  // first make sure we have the right order book
                else if (placedOrder == null) {  // no order.  let's create one.
                    Debug.Print(DateTime.Now + " - MBAIT: no bait guid, lets create it. Top order: " + baiterBook.FirstOrDefault().Key);

                    decimal orderPrice;

                    // now we need to make sure this orderPrice is not bigger/smaller than the best offer/bid (ie turning the order into a market order)

                    // this stuff doesn't work yet!  more testing and fixes needed.
                    // pulled this feature.  Would need to pull GetOpenOrders at every loop to make sure it wasn't our order at the top
                    bool OurOrderAtTop = false;  // let's try and discover if the best current order is a separate order made by this acccount.  If so, pretend it doesn't exist
                    foreach (var openO in openOrders) {
                        foreach (var topOrder in baiterBook.First().Value) {
                            if (openO.ToString() == topOrder.Key) {
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
                            IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "The spread is too toight to fit in an order!  Maybe just market sell?"));
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
                            IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "The spread is too toight to fit in an order!  Maybe just market buy?"));
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
                            (BaiterBookSide == "Bid" ? OrderType.LimitBid : OrderType.LimitOffer), orderPrice, baiterLiveVol);
                        /*if (placedOrder == null) {  // i don't think we should have this.  IF the result is somehow null and doesn't throw to the catch block, then just loop and try again.
                            var res = MessageBox.Show("Failed to place the order.  Do you want to cancel market baiter?", "Market Baiter", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (res == DialogResult.Yes) marketBaiterActive = false;
                        }*/
                        //Thread.Sleep(1050 - (Properties.Settings.Default.UITimerFreq + 50));  // an order must be left alive for at least a second or rate limiting will happen
                        rateLimitPlaceOrder = Task.Delay(1001);  // start a timer.  can only try and create a new order after a second has passed
                    }
                    catch (Exception ex) {
                        string errorMsg = ex.Message;
                        if (ex.InnerException != null) {
                            errorMsg = ex.InnerException.Message;
                        }
                        Debug.Print("MBAIT: trid to create an order, but it failed.  Will retry.  Error: " + errorMsg);

                        if (errorMsg.Contains("Order volume must be greater or equal to")) {  // order size too small now, just finish.
                            Debug.Print("MBAIT: OK, order is too small, so we just stop.");
                            IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter 🎣", crypto.ToUpper() + "-" + fiat.ToUpper() + " order mostly filled, stopping as order now too small to continue.  Remaining: " + crypto.ToUpper() + " " + baiterLiveVol), true);
                            placedOrder = null;
                            marketBaiterActive = false;
                        }
                    }
                    // sholudnt' call the bulkupdate method from here, it should only be called from the UI as it can result in messageboxes, etc.  Also I shouldn't need to cal UpdateOrderBook...
                    //IRT.updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, /*PrivateIREndPoints.GetAccounts, */PrivateIREndPoints.UpdateOrderBook });
                    try {
                        Page<BankHistoryOrder> oOrders = GetOpenOrders(crypto, fiat);
                        IRAF.drawOpenOrders(oOrders.Data);
                    }
                    catch (Exception ex) {
                        Debug.Print("MBAIT: failed to pull GetOpenOrders after placing a new order.. error: " + ex.Message);
                    }

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
                        if (pricePointCount == 0) {  // only for the first price level, let's see if it's us, and if so if it's not the baiter order, we can ignore
                            if (!pricePoint.Value.ContainsKey(placedOrder.OrderGuid.ToString())) {  // before we check let's make sure our actual baiter order isn't here
                                bool continueBaiterLoop = false;
                                foreach (KeyValuePair<string, DCE.OrderBook_IR> orderAtPrice in pricePoint.Value) {
                                    foreach (var openO in openOrders) {
                                        if ((orderAtPrice.Key == openO.ToString()) /*&& (orderAtPrice.Key != placedOrder.OrderGuid.ToString())*/) {  // it's ours, but not the bait order.  The part i have commented out here - i don't think we need to check this because 4 lines above we have already made sure the bait order is not in this price level
                                            continueBaiterLoop = true;  // the order at the spread is ours, let's not compete against it.
                                            //Debug.Print("MBAIT: It appears an order at price $" + pricePoint.Key + " is our own.  Ignore and move to the next price level");
                                            break;
                                        }
                                    }
                                    if (continueBaiterLoop) break;
                                }
                                // why do we duplicate this search?  Because if we already know about this order, then let's not pull GetOpenOrders every time.  Should only
                                // be duplicated the first time, and then we'll know about our existing order, and we'll be good.
                                // what the hell was I trying to do here?
                                // alright, in my great wisdow i think maybe this doesn't make sense.  Why duplicate this check?  Just in case?  how shitty.  Let's see how things survive without double checking.  Also I think it wasn't coded right anyway, which probably means that it was never used
                                /*if (!continueBaiterLoop) {  // OK, we didn't find it.  let's grab the openOrders and search again, maybe we only recently created it
                                    Page<BankHistoryOrder> openOs;
                                    try {
                                        openOs = GetOpenOrders(crypto, DCE_IR.CurrentSecondaryCurrency);
                                    }
                                    catch (Exception ex) {
                                        Debug.Print("MBAIT: failed to get open orders due to: " + ex.Message);
                                        retryRequired = true;
                                        break;
                                    }
                                    foreach (KeyValuePair<string, DCE.OrderBook_IR> orderAtPrice in pricePoint.Value) {
                                        foreach (var openO in openOrders) {
                                            if ((orderAtPrice.Key == openO.ToString()) /*&& (orderAtPrice.Key != placedOrder.OrderGuid.ToString())) {  // it's ours, but not the bait order
                                                continueBaiterLoop = true;  // the order at the spread is ours, let's not compete against it.
                                                Debug.Print("MBAIT: After pulling new open orders, it appears an order at price $" + pricePoint.Key + " is our own.  Ignore and move to the next price level");
                                                break;
                                            }
                                        }
                                        if (continueBaiterLoop) break;
                                    }
                                    if (continueBaiterLoop) continue;  // we will only get here if on the second look we found the order.  Still don't know why we need to look twice.
                                }
                                else continue; */ // continues the baitorBook loop, ie moves to the second price level, but does not increase pricePointCount
                                if (continueBaiterLoop) continue;  // continues the baitorBook loop, ie moves to the second price level, but does not increase pricePointCount
                            }
                            else {  // the baiting order IS in the first price level!
                                if (pricePoint.Value[placedOrder.OrderGuid.ToString()].Volume != baiterLiveVol) {
                                    baiterLiveVol = pricePoint.Value[placedOrder.OrderGuid.ToString()].Volume;
                                    IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Nibble.... (" + (placedOrder.PrimaryCurrencyCode.ToString().ToUpper() == "XBT" ? "BTC" : placedOrder.PrimaryCurrencyCode.ToString().ToUpper()) + " " + baiterLiveVol + " remaining)"));
                                }
                                foundOrder = true;
                                break;  // we don't need to continue, the order is where we want it.
                            }
                        }

                        // now we look for the baiter order
                        if (pricePoint.Value.ContainsKey(placedOrder.OrderGuid.ToString())) {
                            foundOrder = true;
                            if (pricePointCount > 0) {  // our order has been beaten by another. lez cancel and start again.  if == 0 then we're the top of the book, do nothing.
                                if (placedOrder.Price != limitPrice) {  // if we're at the limit price, just leave the order, do not cancel.
                                    //Debug.Print("MBAIT: our order has been beaten.  cancelling it...");
                                    BankOrder bo = new BankOrder();
                                    try {
                                        bo = CancelOrder(placedOrder.OrderGuid.ToString());
                                    }
                                    catch (Exception ex) {
                                        string errorMsg = ex.Message;
                                        if (ex.InnerException != null) {
                                            errorMsg = ex.InnerException.Message;
                                            if (errorMsg.Contains("(Filled)")) {  // have seen an error like this: {"Order is in an invalid state to be cancelled (Filled)"}
                                                foundOrder = false;  // this will trigger code below to check closed orders, we should find our closed order.
                                            }
                                            else {  // some new error, should check it out and potentially handle it
                                                Debug.Print("MBAIT: trying to cancel the order because it got beat, but failed due to inner exception: " + ex.InnerException.Message);
                                                retryRequired = true;
                                            }
                                        }
                                        else {
                                            Debug.Print("MBAIT: trying to cancel the order because it got beat, but failed due to: " + ex.Message);
                                            retryRequired = true;
                                        }
                                        break;
                                    }
                                    if ((bo.Status == OrderStatus.Cancelled) || (bo.Status == OrderStatus.PartiallyFilledAndCancelled)) {
                                        //Debug.Print("MBAIT: cancel order was successful");
                                        //if (bo.VolumeFilled != 0) IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Nibble..."));

                                        baiterLiveVol = bo.VolumeOrdered - bo.VolumeFilled;
                                        // bulkupdate thingo should only be called from the UI as it can result in messageboxes.  also I think we don't need to call updateOrderBook, there should have already been a new event comet through the websockets that would update it?
                                        //IRT.updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });
                                        try {
                                            Page<BankHistoryOrder> oOrders = GetOpenOrders(crypto, fiat);
                                            IRAF.drawOpenOrders(oOrders.Data);
                                        }
                                        catch (Exception ex) {
                                            Debug.Print("MBAIT: failed to pull GetOpenOrders after cancelling an order.. error: " + ex.Message);
                                        }
                                        placedOrder = null;
                                    }
                                    else {
                                        Debug.Print("MBAIT: FAILED TO CANCEL ORDER!  why?  current status: " + bo.Status);
                                    }
                                }
                                //else Debug.Print("MBAIT: our order is at the limit, just gonna leave it.  price: " + placedOrder.Price);
                            }
                            /*else {
                                // we used to have code here for if the order was in the first price level, but have moved it up
                                if (pricePoint.Value[placedOrder.OrderGuid.ToString()].Volume != baiterLiveVol) {
                                    baiterLiveVol = pricePoint.Value[placedOrder.OrderGuid.ToString()].Volume;
                                    IRT.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Nibble.... (" + (placedOrder.PrimaryCurrencyCode.ToString().ToUpper() == "XBT" ? "BTC" : placedOrder.PrimaryCurrencyCode.ToString().ToUpper()) + " " + baiterLiveVol + " remaining)"));
                                }
                            }*/
                            break;  // we found the baiter order, so berak out of the order book foreach
                        }
                        pricePointCount++;
                    }
                    if (retryRequired) { Thread.Sleep(100); continue; }
                    if (!foundOrder) {
                        Debug.Print("MBAIT: Our order doesn't exist in the OB, possibly filled? " + placedOrder.OrderGuid.ToString());
                        Page<BankHistoryOrder> closedOs;
                        try {
                            closedOs = GetClosedOrders(crypto, fiat);  // let's check to see if we have it
                            IRAF.drawClosedOrders(closedOs.Data);
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
                                    IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter 🎣", crypto.ToUpper() + "-" + fiat.ToUpper() + " order filled!"), true);
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
                            Page<BankHistoryOrder> openOs = new Page<BankHistoryOrder>();
                            try {
                                openOs = GetOpenOrders(crypto, fiat);
                            }
                            catch (Exception ex) {
                                Debug.Print("MBAIT: failed to pull GetOpenOrders while trying to see where the order is. error: " + ex.Message);
                                continue;  // let's move on, let the sytem auto-correct.
                            }
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
                                        if ((cancelledOrder.Status == OrderStatus.Cancelled) || (cancelledOrder.Status == OrderStatus.PartiallyFilledAndCancelled)) {
                                            baiterLiveVol = cancelledOrder.VolumeOrdered - cancelledOrder.VolumeFilled;
                                            // bulkupdate thingo should only be called from the UI as it can result in messageboxes.  also I think we don't need to call updateOrderBook, there should have already been a new event comet through the websockets that would update it?
                                            //IRT.updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });
                                            try {
                                                Page<BankHistoryOrder> oOrders = GetOpenOrders(crypto, fiat);  // we need to pull this again because we just cancelled the order
                                                IRAF.drawOpenOrders(oOrders.Data);
                                            }
                                            catch (Exception ex) {
                                                Debug.Print("MBAIT: failed to pull GetOpenOrders after discovering our order was cancelled. error: " + ex.Message);
                                            }
                                            placedOrder = null;
                                        }
                                    }
                                    foundOrder = true;  // just so we don't get caught below and claim we didn't find it
                                    break;
                                }
                            }
                            // IRT.synchronizationContext.Post(new SendOrPostCallback(o => { IRT.drawOpenOrders((IEnumerable<BankHistoryOrder>)o); }), openOs.Data);  // whoa.. don't think we need to do this.  The drawOpenOrders already does it
                            IRAF.drawOpenOrders(openOs.Data);

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
                if ((bo.Status == OrderStatus.Cancelled) || (bo.Status == OrderStatus.PartiallyFilledAndCancelled)) {
                    Debug.Print("MBAIT: cancel order was successful");
                    IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Market baiter stopped, existing order cancelled."));
                    placedOrder = null;
                }
                else {
                    Debug.Print("MBAIT: couldn't cancel the order?? guid: " + bo.OrderGuid);
                    IRAF.notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Market baiter stopped, but order couldn't be cancelled?"));
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
