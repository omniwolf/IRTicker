using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;

namespace IRTicker {
    public class DCE {
        private string _primaryCodesStr;
        private string _secondaryCodesStr;
        private ConcurrentDictionary<string, List<Tuple<DateTime, decimal>>> priceHistory = new ConcurrentDictionary<string, List<Tuple<DateTime, decimal>>>();
        private ConcurrentDictionary<string, List<DataPoint>> spreadHistory = new ConcurrentDictionary<string, List<DataPoint>>();
        private ConcurrentDictionary<string, List<DataPoint>> spreadHistoryCSV = new ConcurrentDictionary<string, List<DataPoint>>();
        //private ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> OfferOrderBook_IR = new ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>();  // outer decimal is price, inner decimal is guid
        //private ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> BidOrderBook_IR = new ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>();
        // this next thing is hectic.  a dictionry of tuples.  The key is the crypto pair, the tuple in the order books (bid,offer) (which is represented by a dictionary (price) of dictionaries (order guids)
        public ConcurrentDictionary<string, Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>>> IR_OBs = new ConcurrentDictionary<string, Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>>>();

        private Dictionary<string, MarketSummary> cryptoPairs;
        public Dictionary<string, OrderBook> orderBooks;  // string format is eg "XBT-AUD" - caps with a dash

        // websocket stuff
        public ConcurrentDictionary<string, int> channelNonce = new ConcurrentDictionary<string, int>();
        public ConcurrentDictionary<string, bool> nonceErrorTracker = new ConcurrentDictionary<string, bool>();  // false means no error.  false is good.
        public ConcurrentDictionary<string, bool> OBResetFlag = new ConcurrentDictionary<string, bool>();  // if true, we need to dump OB and get a new one once nonce has settled down
        public DateTime HeartBeat = new DateTime(2000, 1, 1);  // set it way in the past.  use this as an initialisation value
        public bool socketsReset = false;

        // constructor
        public DCE(string _codeName, string _friendlyName) {
            cryptoPairs = new Dictionary<string, MarketSummary>();
            orderBooks = new Dictionary<string, OrderBook>();
            CodeName = _codeName;
            FriendlyName = _friendlyName;
        }
        
        public string CodeName { get; }

        public string FriendlyName { get; }

        public bool NetworkAvailable { get; set; } = true;

        public bool HasStaticData { get; set; } = false;  // this will be false until we can pull the DCE static data (eg currency pairs, etc - data that will never change in a session).  Once true always true for a session.
        //public bool HasDynamicData { get; set; } = false; // set to true once we have received our first socket data

        // "Online" if everything is fine, anything else will cause the UI to display this string in the DCE group box text
        public string CurrentDCEStatus { get; set; }

        public List<Tuple<DateTime, decimal>> GetPriceList(string pair) {
            if (priceHistory.ContainsKey(pair.ToUpper())) {
                priceHistory.TryGetValue(pair.ToUpper(), out List<Tuple<DateTime, decimal>> result);
                return result;
            }
            else {
                return new List<Tuple<DateTime, decimal>>();
            }
        }

        public ConcurrentDictionary<string, List<DataPoint>> GetSpreadHistory() {
            lock (spreadHistory) {
                return new ConcurrentDictionary<string, List<DataPoint>>(spreadHistory);
            }
        }

        // we clear this one every time we use it so it's only new data
        public ConcurrentDictionary<string, List<DataPoint>> GetSpreadHistoryCSV() {
            lock (spreadHistoryCSV) {
                ConcurrentDictionary<string, List<DataPoint>> sprd = new ConcurrentDictionary<string, List<DataPoint>>(spreadHistoryCSV);
                spreadHistoryCSV.Clear();
                return sprd;
            }
        }


        /// <summary>
        /// pair is format "XBT-AUD"
        /// Note - this needs to be called from a non UI thread as we put a wait in it so we're not trying to add to it while we're reading it
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="mSummary"></param>
        public void CryptoPairsAdd(string pair, MarketSummary mSummary) {
            // ok here want to add it to the cryptopairs dictionary, but we also want to add the last price to a list so we can see trends
            pair = pair.ToUpper();
            lock (cryptoPairs) {
                // if the pair already exists, we need to update it.  if not, we create a new entry.
                if (cryptoPairs.ContainsKey(pair)) {
                    // if the mSummary object that's been sent to us has properties that are 0, then don't update cryptoPair element
                    if (mSummary.CreatedTimestampUTC != "") cryptoPairs[pair].CreatedTimestampUTC = mSummary.CreatedTimestampUTC;
                    if (mSummary.CurrentHighestBidPrice != 0) cryptoPairs[pair].CurrentHighestBidPrice = mSummary.CurrentHighestBidPrice;
                    if (mSummary.CurrentLowestOfferPrice != 0) cryptoPairs[pair].CurrentLowestOfferPrice = mSummary.CurrentLowestOfferPrice;
                    if (mSummary.DayAvgPrice != 0) cryptoPairs[pair].DayAvgPrice = mSummary.DayAvgPrice;
                    if (mSummary.DayHighestPrice != 0) cryptoPairs[pair].DayHighestPrice = mSummary.DayHighestPrice;
                    if (mSummary.DayLowestPrice != 0) cryptoPairs[pair].DayLowestPrice = mSummary.DayLowestPrice;
                    if (mSummary.DayVolume != 0) cryptoPairs[pair].DayVolume = mSummary.DayVolume;
                    if (mSummary.DayVolumeInSecondaryCurrency != 0) cryptoPairs[pair].DayVolumeInSecondaryCurrency = mSummary.DayVolumeInSecondaryCurrency;
                    if (mSummary.DayVolumeXbt != 0) cryptoPairs[pair].DayVolumeXbt = mSummary.DayVolumeXbt;
                    if (mSummary.LastPrice != 0) cryptoPairs[pair].LastPrice = mSummary.LastPrice;

                    // these 3 will be 0 or "" if the mSummary object has come from a REST pull
                    // if the data comes from a REST pull, then we just take the latest bid/offer/timestamp deets as they'll likely be more up to date than what comes through REST
                    if (mSummary.CurrentHighestBidPrice == 0) mSummary.CurrentHighestBidPrice = cryptoPairs[pair].CurrentHighestBidPrice;
                    if (mSummary.CurrentLowestOfferPrice == 0) mSummary.CurrentLowestOfferPrice = cryptoPairs[pair].CurrentLowestOfferPrice;
                    if (mSummary.CreatedTimestampUTC == "") mSummary.CreatedTimestampUTC = cryptoPairs[pair].CreatedTimestampUTC;
                }
                else {  // new element
                    cryptoPairs[pair] = mSummary;
                }

            }

            if (CodeName == "IR") {
                // do nothing
                ;
            }

            if (!priceHistory.ContainsKey(pair)) {  // if this crypto/fiat pair hasn't come up before, create a new empty dictionary kvp
                priceHistory.TryAdd(pair, new List<Tuple<DateTime, decimal>>());
            }
            lock (priceHistory[pair]) {  // we're locking on the List, not the ConcurrentDictionary
                priceHistory[pair].Add(new Tuple<DateTime, decimal>(DateTime.Now, ((mSummary.CurrentHighestBidPrice + mSummary.CurrentLowestOfferPrice) / 2)));  // add the time and price to the kvp's value list
                if (CodeName == "IR" && pair == "XBT-AUD" && priceHistory[pair].Last().Item2 == 0) {
                    ;
                }
            }
            
            lock (spreadHistory) {
                if (!spreadHistory.ContainsKey(pair)) spreadHistory.TryAdd(pair, new List<DataPoint>());
                spreadHistory[pair].Add(new DataPoint(DateTime.Now.ToOADate(), (double)mSummary.spread));
            }
            lock (spreadHistoryCSV) {
                if (!spreadHistoryCSV.ContainsKey(pair)) spreadHistoryCSV.TryAdd(pair, new List<DataPoint>());
                spreadHistoryCSV[pair].Add(new DataPoint(DateTime.Now.ToOADate(), (double)mSummary.spread));
            }
        }

        // returns a copy of the dictionary so we can mess with it without fear of reproach
        public Dictionary<string, MarketSummary> GetCryptoPairs() {
            Dictionary<string, MarketSummary> cPairs;
            lock (cryptoPairs) {
                 cPairs = new Dictionary<string, MarketSummary>(cryptoPairs);
            }
            return cPairs;
        }

        public string BuySell { get; set; }
        public string NumCoinsStr { get; set; } = "";
        public string CryptoCombo { get; set; } = "";

        public void RemoveOrderBook(string pair) {
            if (orderBooks.ContainsKey(pair)) orderBooks.Remove(pair);
        }


        /////////////////////////////////////////////////////////
        ////////////////      CURRENCIES      ///////////////////
        /////////////////////////////////////////////////////////

        public bool ChangedSecondaryCurrency { get; set; } = true;  // used for avg price stuff
        private int ChosenSecondaryCurrency { get; set; } = 0;

        /// <summary>
        /// Crypto, needs to take codes as comma separated string with quotation marks around each symbol, eg "\"XBT\",\"BCH\",\"ETH\""
        /// </summary>
        public string PrimaryCurrencyCodes {
            get {
                return _primaryCodesStr;
            }
            set {
                _primaryCodesStr = value.ToUpper();
            }
        }

        public List<string> PrimaryCurrencyList {
            get {
                List<string> codesList = new List<string>();
                string[] codess = _primaryCodesStr.Split(',');
                foreach(string cc in codess) {
                    string cc2 = Utilities.TrimEnds(cc);
                    codesList.Add(cc2);
                }
                return codesList;
            }
        }

        /// <summary>
        /// Fiat, needs to take codes as comma separated string with quotation marks around each symbol, eg "\"AUD\",\"USD\",\"NZD\""
        /// </summary>
        public string SecondaryCurrencyCodes {
            get {
                return _secondaryCodesStr;
            }
            set {
                _secondaryCodesStr = value.ToUpper();
            }
        }

        public List<string> SecondaryCurrencyList {
            get {
                List<string> codesList = new List<string>();
                string[] codess = _secondaryCodesStr.Split(',');
                foreach(string cc in codess) {
                    string cc2 = Utilities.TrimEnds(cc);
                    codesList.Add(cc2);
                }
                return codesList;
            }
        }

        public string CurrentSecondaryCurrency {
            get {
                return SecondaryCurrencyList[ChosenSecondaryCurrency];
            }
        }

        /// <summary>
        /// This moves the chosenSecondaryCurrency value to the next one (or to the first if we're on the last one)
        /// </summary>
        public void NextSecondaryCurrency() {
            if (ChosenSecondaryCurrency == SecondaryCurrencyList.Count - 1) {
                ChosenSecondaryCurrency = 0;
            }
            else {
                ChosenSecondaryCurrency++;
            }
        }

        /// <summary>
        /// string is pair with dash eg XBT-AUD, products_GDAX has all sorts of pair info
        /// </summary>
        public Dictionary<string, products_GDAX> ExchangeProducts { get; set; } = new Dictionary<string, products_GDAX>();

        /// <summary>
        /// returns a list of dash pairs (XBT-AUD) that both the app and exchange supports
        /// Can only be used if ExchangeProducts has been populated.
        /// </summary>
        /// <param name="cryptoList"></param>
        /// <param name="fiatList"></param>
        /// <param name="dExchange"></param>
        /// <returns></returns>
        public List<string> UsablePairs() {
            List<string> usablePairs = new List<string>();
            foreach (string crypto in PrimaryCurrencyList) {
                foreach (string fiat in SecondaryCurrencyList) {
                    if (ExchangeProducts.ContainsKey(crypto + "-" + fiat)) {
                        usablePairs.Add(crypto + "-" + fiat);
                    }
                }
            }
            return usablePairs;
        }


        /////////////////////////////////////////////////////////
        //////////////      IR Order Book    ////////////////////
        /////////////////////////////////////////////////////////

        // this gets called when we receive an order book change/add/remove. 
        public bool OrderBookEvent_IR(string eventStr, OrderBook_IR order) {

            // before we do anything, take a copy of the first elements of each order book.  If these change, then the spread has changed and we need to update the UI
            bool OrderWillChangeSpread = false;

            ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> OB_IR;  // an order will only ever be a limit or a bid, so sort it out up top to reduce code duplication
            ConcurrentDictionary<string, OrderBook_IR> TopOrder;
            Decimal TopPrice;  // this will be the price of the order we're looking at.  Have to grab it separarely as the API doesn't tell it to us depending on the event :(
            switch (order.OrderType) {
                case "LimitBid":
                    OB_IR = IR_OBs[order.Pair.ToUpper()].Item1;
                    TopPrice = IR_OBs[order.Pair.ToUpper()].Item1.Keys.Max();
                    TopOrder = (IR_OBs[order.Pair.ToUpper()].Item1)[TopPrice];
                    break;
                case "LimitOffer":
                    OB_IR = IR_OBs[order.Pair.ToUpper()].Item2;
                    TopPrice = IR_OBs[order.Pair.ToUpper()].Item2.Keys.Min();
                    TopOrder = (IR_OBs[order.Pair.ToUpper()].Item2)[TopPrice];

                    break;
                default:

                    // ok this is a market order i guess, which probably means it's an orderchanged event
                    if (eventStr == "OrderChanged") {
                        if (order.OrderType.EndsWith("Bid")) {
                            if (order.Pair.ToUpper() == "ETH-AUD") {
                                //Debug.Print("ETH bid order changed");
                            }
                            OB_IR = IR_OBs[order.Pair.ToUpper()].Item1;
                            TopPrice = IR_OBs[order.Pair.ToUpper()].Item1.Keys.Max();
                            TopOrder = (IR_OBs[order.Pair.ToUpper()].Item1)[TopPrice];
                        }
                        else {
                            if (order.Pair.ToUpper() == "ETH-AUD") {
                                //Debug.Print("ETH offer order changed");
                            }
                            OB_IR = IR_OBs[order.Pair.ToUpper()].Item2;
                            TopPrice = IR_OBs[order.Pair.ToUpper()].Item2.Keys.Min();
                            TopOrder = (IR_OBs[order.Pair.ToUpper()].Item2)[TopPrice];
                        }
                    }
                    else {
                        Debug.Print("IR ws - a new order that wasn't a bid or offer was sent to us? " + order.OrderType + " price: " + order.Price + " event: " + eventStr);
                        return false;
                    }
                    break;
                    //return false;
            }

            // if it's the first order, so this changes the spread
            // i need to discover this up here, because if the event is a OrderChanged (with vol of 0) or OrderCanceled then I delete the orderbook_IR object, so i have nothing to compare to. 

            

            //foreach (OrderBook_IR Price in OB_IR.First().Value){
                if (eventStr == "OrderChanged" && TopOrder.ContainsKey(order.OrderGuid) && order.Volume == 0 && TopOrder.Count == 1) {
                    // this is a spread changing event... do something?
                    OrderWillChangeSpread = true;

                    if (order.Pair.ToUpper() == "ETH-AUD") {

                    //Debug.Print("a changed ETHAUD order will change the spread - " + order.OrderType);
                    }
                //break;
                }
                else if (eventStr == "NewOrder" && order.OrderType == "LimitBid" && order.Price > TopPrice) { // pick the "First()" one just arbitrary - all elements of this dictionary have the same price
                    // spread changing order
                    OrderWillChangeSpread = true;

                if (order.Pair.ToUpper() == "ETH-AUD") {

                    //Debug.Print("a new offer ETHAUD order will change the spread - " + order.Price);
                }
                //break;
            }
                else if (eventStr == "NewOrder" && order.OrderType == "LimitOffer" && order.Price < TopPrice) {
                    // spread changing order
                    OrderWillChangeSpread = true;

                //if (order.Pair.ToUpper() == "ETH-AUD") {

                    //Debug.Print("a new buy ETHAUD order will change the spread - " + order.Price);
                //}
                //break;
            }
                else if (eventStr == "OrderCanceled" && TopOrder.ContainsKey(order.OrderGuid) && TopOrder.Count == 1) {  // if the cancelled order is at the top, and it's the only one at that price, spread will change.
                if (order.Pair.ToUpper() == "ETH-AUD") {

                    //Debug.Print("a canceled ETHAUD order will change the spread - " + TopOrder.First().Value.Price);
                }
                OrderWillChangeSpread = true;
            }
            //}

            // here we actually adjust the order book in accordance with the event we just received
            switch (eventStr) {
                case "NewOrder":  // API should send us OrderGuid, Pair, Price, OrderType, Volume

                    if (OB_IR.ContainsKey(order.Price)) {  // this is a new order at an existing price step in the OB
                        //if (OB_IR[order.Price].ContainsKey(order.OrderGuid)) {
                            //Debug.Print("weird, trying to add a new order, but the guid is already in the dictionary?? - " + order.OrderGuid);
                            //break;
                        //}

                        OB_IR[order.Price].TryAdd(order.OrderGuid, order);
                        //Debug.Print("New order existing price - " + order.Price);
                    }
                    else {  // this is a new price
                        ConcurrentDictionary<string, OrderBook_IR> tempCD = new ConcurrentDictionary<string, OrderBook_IR>();
                        tempCD.TryAdd(order.OrderGuid, order);
                        OB_IR.TryAdd(order.Price, tempCD);
                        //Debug.Print("New order new price - " + order.Price);
                    }
                    break;

                case "OrderChanged":  // API should send us OrderGuid, Pair, OrderType, Volume

                    //Debug.Print(DateTime.Now + " IR - order changed, pair: " + order.Pair + ", type: " + order.OrderType + ", volume: " + order.Volume);
                    // Roman had an idea here where I maintain 2 dictionaries, one where the key is the price and one where the key is the guid.  find the guid; find the price.

                    // this was a bad idea, we only remove orders if they're the top?  so dumb.
                    /*if (TopOrder.ContainsKey(order.OrderGuid) && order.Volume == 0) {  // ok this change order is top level
                        if (TopOrder.Count > 1) TopOrder.TryRemove(order.OrderGuid, out OrderBook_IR ignore);  // more than one order at this price
                        else OB_IR.TryRemove(TopPrice, out ConcurrentDictionary<string, OrderBook_IR> ignore);  // only one order at this price, we delete the whole cDictionary entry.
                    }*/

                    // OK I think  (roman yet to confirm) that if we get a market order and the volume is 0, then we just remove the top order.  hopefully the top price
                    // doesn't have multiple orders in it.. let's alert if we discover this
                    if (order.OrderType.ToUpper().StartsWith("MARKET") && order.Volume == 0) {
                        if (TopOrder.Count > 1) {
                            Debug.Print("market order with vol 0, there are multiple top orders!");
                        }
                        else {
                            Debug.Print("market order with vol 0, only 1 top order");
                        }
                        OB_IR.TryRemove(TopPrice, out ConcurrentDictionary<string, OrderBook_IR> ignore); // just trash the first order
                        break;
                    }

                    foreach (KeyValuePair<decimal, ConcurrentDictionary<string, OrderBook_IR>> Price in OB_IR) {  // IR_OB is a one sided order book for the current pair

                        if (Price.Value.ContainsKey(order.OrderGuid)) {  // we found the needle in the haystack :/   
                            if (order.Volume == 0) {  // this means the order was fully filled, let's remove the order.
                                if (Price.Value.Count > 1) Price.Value.TryRemove(order.OrderGuid, out OrderBook_IR ignore);  // more than one order at this price
                                else OB_IR.TryRemove(Price.Key, out ConcurrentDictionary<string, OrderBook_IR> ignore);  // only one order at this price, we delete the whole cDictionary entry.
                                //Debug.Print("Filled order and price - " + order.Price);
                            }
                            else {  // just need to update the order
                                Price.Value[order.OrderGuid].Volume = order.Volume;
                                //Debug.Print("Filled order - " + order.Price);
                            }
                            break;  // break out of the foreach
                        }
                    }
                    break;

                case "OrderCanceled":  // API should send us OrderGuid, Pair, OrderType
                    decimal cancelledPrice = 0;
                    foreach (KeyValuePair<decimal, ConcurrentDictionary<string, OrderBook_IR>> Price in OB_IR) {  // this might be too inefficient.  might have to do Roman's idea

                        if (Price.Value.ContainsKey(order.OrderGuid)) {
                            if (Price.Value.Count > 1) Price.Value.TryRemove(order.OrderGuid, out OrderBook_IR ignore);  // more than one order at this price
                            else {
                                // we found the price, and it's the only one.   let's break out of this loop and then remove the element from OB_IR
                                cancelledPrice = Price.Key;
                                //if (order.Pair.ToUpper() == "ETH-AUD") {
                                    //Debug.Print("IR ETHAUD canceled order - just tried to remove the outer dictionary element as this was the only order at this price - " + Price.Key);
                                //}
                            }
                            //Debug.Print("Order cancelled - " + order.OrderGuid);
                            break;  // break out of the foreach
                        }
                    }

                    if (cancelledPrice > 0) {
                        OB_IR.TryRemove(cancelledPrice, out ConcurrentDictionary<string, OrderBook_IR> ignore);  // only one order at this price, we delete the whole cDictionary entry.
                    }

                    break;
            }

            // if this order has changed the spread, then let's update the cryptoPairs dictionary
            //foreach (OrderBook_IR PriceOrder in OB_IR.First().Value) {
                if (OrderWillChangeSpread) {
                    DateTimeOffset DTO = DateTimeOffset.Now;
                    MarketSummary mSummary = new MarketSummary();
                    mSummary.CreatedTimestampUTC = DTO.LocalDateTime.ToString("o");
                mSummary.CurrentHighestBidPrice = IR_OBs[order.Pair.ToUpper()].Item1.Keys.Max();
                    mSummary.CurrentLowestOfferPrice = IR_OBs[order.Pair.ToUpper()].Item2.Keys.Min();
                mSummary.pair = order.Pair.ToUpper();
                    CryptoPairsAdd(order.Pair.ToUpper(), mSummary);
                //Debug.Print("OCE: " + order.Pair + " " + eventStr + " " + mSummary.CurrentHighestBidPrice + " " + mSummary.CurrentLowestOfferPrice);

                    return true;
                }
            //}
            return false;
        }

        // this should be called once we have the orderbooks variable populated.  this method will split the orderbooks object into
        // 2 sorted lists BidOrderBook_IR and OfferOrderBook_IR
        public void InitialiseOrderBook_IR(string pair) {  // !!!!!!!!!!!!!! need to probably change all adds to TryAdd to make sure they're safe, work out how to handle duplicate adds

            pair = pair.ToUpper();  // always uppercase

            // fix this.  need to create new dictionaries and whatevs when a pair we haven't seen before comes along
            if (!IR_OBs.ContainsKey(pair.ToUpper())) {
                // OK if it doesn't contain this pair, we have to create some shiz

                ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> tempbuy = new ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>();
                ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> tempsell = new ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>();

                Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>> tempTup = new Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>>>(tempbuy, tempsell);
                IR_OBs.TryAdd(pair, tempTup);
            }

            ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> bidOB = IR_OBs[pair].Item1;
            ConcurrentDictionary<decimal, ConcurrentDictionary<string, OrderBook_IR>> offerOB = IR_OBs[pair].Item2;

            // buy orders first.  
            //lock (BidOrderBook_IR) {
                foreach (Order order in orderBooks[pair].BuyOrders) {
                    //OrderBookEvent_IR("NewOrder", new DCE.OrderBook_IR(order.Guid, crypto + "-" + DCEs["IR"].CurrentSecondaryCurrency, order.Price, "LimitBid", order.Volume));
                    if (bidOB.ContainsKey(order.Price)) {  // this price already has order(s)
                        bidOB[order.Price].TryAdd(order.Guid, new OrderBook_IR(order.Guid, pair, order.Price, order.OrderType, order.Volume));
                    }
                    else {  // new price, create the dictionary
                        ConcurrentDictionary<string, OrderBook_IR> tempCD = new ConcurrentDictionary<string, OrderBook_IR>();
                        tempCD.TryAdd(order.Guid, new OrderBook_IR(order.Guid, pair, order.Price, order.OrderType, order.Volume));
                    bidOB.TryAdd(order.Price, tempCD);
                    }
                }
            //}

            // now sell orders
            // buy orders first.  
            //lock (OfferOrderBook_IR) {
                foreach (Order order in orderBooks[pair].SellOrders) {
                    if (offerOB.ContainsKey(order.Price)) {
                        offerOB[order.Price].TryAdd(order.Guid, new OrderBook_IR(order.Guid, pair, order.Price, order.OrderType, order.Volume));
                    }
                    else {
                    ConcurrentDictionary<string, OrderBook_IR> tempCD = new ConcurrentDictionary<string, OrderBook_IR>();
                    tempCD.TryAdd(order.Guid, new OrderBook_IR(order.Guid, pair, order.Price, order.OrderType, order.Volume));
                    offerOB.TryAdd(order.Price, tempCD);
                    }
                }

                // order books for this pair have been created, now we get the spread and put it in the cryptoPairs dictionary
            DateTimeOffset DTO = DateTimeOffset.Now;
            MarketSummary mSummary = new MarketSummary();
            mSummary.CreatedTimestampUTC = DTO.LocalDateTime.ToString("o");
            mSummary.CurrentHighestBidPrice = bidOB.Keys.Max();
            mSummary.CurrentLowestOfferPrice = offerOB.Keys.Min();
            mSummary.pair = pair;
            CryptoPairsAdd(pair, mSummary);
            //}
        }

        public void GetIROrderBook(string crypto, string fiat) {
            Tuple<bool, string> orderBookTpl = Utilities.Get("https://api.independentreserve.com/Public/GetAllOrders?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);
            if (orderBookTpl.Item1) {
                DCE.OrderBook orderBook = JsonConvert.DeserializeObject<DCE.OrderBook>(orderBookTpl.Item2);
                orderBooks[crypto + "-" + fiat] = orderBook;

                // next we need to convert this orderbook into a concurrent dictionary of OrderBook_IR objects
                // so yeah.. the "orderBook" object doesn't really get used anymore.  it's just like a staging area
                InitialiseOrderBook_IR(crypto + "-" + fiat);

                Debug.Print(DateTime.Now.ToString() + " IR OB " + crypto + fiat + " done");
            }
        }




        /////////////////////////////////////////////////////////
        //////////////      JSON STUFF     //////////////////////
        /////////////////////////////////////////////////////////

        // this is the class that is used for all DCEs. It is based on IR's JSON.
        public class MarketSummary {

            private string _PrimaryCurrencyCode;
            private string _SecondaryCurrencyCode;

            public decimal DayHighestPrice { get; set; }
            public decimal DayLowestPrice { get; set; }
            public decimal DayAvgPrice { get; set; }
            public decimal DayVolumeXbt { get; set; }
            public decimal DayVolumeInSecondaryCurrency { get; set; }
            public decimal CurrentLowestOfferPrice { get; set; }
            public decimal CurrentHighestBidPrice { get; set; }
            public decimal LastPrice { get; set; }
            /// <summary>
            /// crypto
            /// </summary>
            public string PrimaryCurrencyCode {
                get {
                    return _PrimaryCurrencyCode.ToUpper();
                }
                set {
                    _PrimaryCurrencyCode = value;
                }  // crypto
            }
            /// <summary>
            /// fiat
            /// </summary>
            public string SecondaryCurrencyCode {
                get {
                    return _SecondaryCurrencyCode.ToUpper();
                }
                set {
                    _SecondaryCurrencyCode = value;
                }
            } // fiat
            public string CreatedTimestampUTC { get; set; }

            public decimal spread {
                get {
                    return CurrentLowestOfferPrice - CurrentHighestBidPrice;
                }
            }

            // whoops, originally called this property "DayVolume" when it should have been "DayVolumeXbt".  I coded everywhere to "DayVolume", so I've just
            // added in this redirect so they both work :/
            public decimal DayVolume {
                get {
                    return DayVolumeXbt;
                }
                set {
                    DayVolumeXbt = value;
                }
            }

            
            /// <summary>
            /// setting this must be in "crypto-fiat" format.  You only have to set this if you haven't set the primary and secondary individually.
            /// </summary>
            public string pair {
                get {
                    return PrimaryCurrencyCode + "-" + SecondaryCurrencyCode;
                }
                set {
                    Tuple<string, string> pairs = Utilities.SplitPair(value);
                    PrimaryCurrencyCode = pairs.Item1;
                    SecondaryCurrencyCode = pairs.Item2;
                }
            }
        }

        // this is the class used to deserialise BTC Market's market summary data
        public class MarketSummary_BTCM {
            public decimal bestBid { get; set; }
            public decimal bestAsk { get; set; }
            public decimal lastPrice { get; set; }
            public string currency { get; set; }  // fiat currency
            public string instrument { get; set; }  // cryptocurrency
            public double timestamp { get; set; }
            public decimal volume24h { get; set; }
        }

        public class MarketSummary_GDAX {
            public double trade_id { get; set; }
            public string price { get; set; }
            public string size { get; set; }
            public string bid { get; set; }
            public string ask { get; set; }
            public string volume { get; set; }
            public string time { get; set; }
        }

        public class currencies_GDAX {
            public string id { get; set; }
            public string name { get; set; }
            public string min_size { get; set; }
            public string status { get; set; }
            public string message { get; set; }
        }

        // this class holds what currency pairs are possible in GDAX
        public class products_GDAX {
            public string id { get; set; }
            public string base_currency { get; set; }
            public string quote_currency { get; set; }
            public string base_min_size { get; set; }
            public string base_max_size { get; set; }
            public string quote_increment { get; set; }
            public string display_name { get; set; }
            public string status { get; set; }
            public bool margin_enabled { get; set; }
            public string status_message { get; set; }
            public string min_market_funds { get; set; }
            public string max_market_funds { get; set; }
            public bool post_only { get; set; }
            public bool limit_only { get; set; }
            public bool cancel_only { get; set; }

            public products_GDAX() { }  // default constructor
            public products_GDAX(string _id) {  // overload it to allow the setting of the id (pair)
                id = _id;
            }

        }

        public class products_BFX {
            public string pair { get; set; }
            public int price_precision { get; set; }
            public string initial_margin { get; set; }
            public string minimum_margin { get; set; }
            public string maximum_order_size { get; set; }
            public string minimum_order_size { get; set; }
            public string expiration { get; set; }
            public bool margin { get; set; }
        }

        public class MarketSummary_BFX {
            public string mid { get; set; }
            public string bid { get; set; }
            public string ask { get; set; }
            public string last_price { get; set; }
            public string low { get; set; }
            public string high { get; set; }
            public string volume { get; set; }
            public string timestamp { get; set; }
        }

        public class Crypto_CSPT {
            public string bid { get; set; }
            public string ask { get; set; }
            public string last { get; set; }
            public string ticker { get; set; }  // manually set as the ticker, eg btc or eth
        }

        public class Prices_CSPT {
            public Crypto_CSPT btc { get; set; }
            public Crypto_CSPT ltc { get; set; }
            public Crypto_CSPT doge { get; set; }
            public Crypto_CSPT eth { get; set; }
            public Crypto_CSPT xrp { get; set; }

            // create a list of the coins so we can iterate through them in the main code.
            public List<Crypto_CSPT> cryptoList = new List<Crypto_CSPT>();
            public void CreateCryptoList() {
                cryptoList.Add(btc);
                cryptoList.Add(ltc);
                cryptoList.Add(doge);
                cryptoList.Add(eth);
                cryptoList.Add(xrp);
                btc.ticker = "XBT";
                ltc.ticker = "LTC";
                doge.ticker = "DOGE";
                eth.ticker = "ETH";
                xrp.ticker = "XRP";
            }
        }

        public class MarketSummary_CSPT {
            public string status { get; set; }
            public Prices_CSPT prices { get; set; }
        }

        public class Order {

            public Order(string _orderType, decimal _price, decimal _volume, string _guid) {
                OrderType = _orderType;
                Price = _price;
                Volume = _volume;
                Guid = _guid;
            }

            public string OrderType { get; set; }
            public decimal Price { get; set; }
            public decimal Volume { get; set; }
            public string Guid { get; set; }
        }   

        public class OrderBook {
            public List<Order> BuyOrders { get; set; }
            public List<Order> SellOrders { get; set; }
            public string PrimaryCurrencyCode { get; set; }
            public string SecondaryCurrencyCode { get; set; }
            public DateTime CreatedTimestampUtc { get; set; }

            public OrderBook() {
                BuyOrders = new List<Order>();
                SellOrders = new List<Order>();
            }
        }

        public class OrderBook_BTCM {
            public string currency { get; set; }
            public string instrument { get; set; }
            public int timestamp { get; set; }
            public List<List<decimal>> asks { get; set; }
            public List<List<decimal>> bids { get; set; }
        }

        public class OrderBook_GDAX {
            public long sequence { get; set; }
            public List<List<string>> bids { get; set; }
            public List<List<string>> asks { get; set; }
        }

        public class BidAsk_BFX {
            public string price { get; set; }
            public string amount { get; set; }
            public string timestamp { get; set; }
        }

        public class OrderBook_BFX {
            public List<BidAsk_BFX> bids { get; set; }
            public List<BidAsk_BFX> asks { get; set; }
        }

        public class OrderBook_IR {
            public string OrderGuid { get; set; }
            public string Pair { get; set; }
            public decimal Price { get; set; }
            public string OrderType { get; set; }
            public decimal Volume { get; set; }

            public OrderBook_IR(string _orderGuid, string _pair, decimal _price, string _orderType, decimal _volume) {
                OrderGuid = _orderGuid;
                Pair = _pair;
                Price = _price;
                OrderType = _orderType;
                Volume = _volume;
            }
            public OrderBook_IR() { }
        }
    }

    class DescComparer<T> : IComparer<T> {
        public int Compare(T x, T y) {
            return Comparer<T>.Default.Compare(y, x);
        }
    }
}
