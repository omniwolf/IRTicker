using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTicker {
    class DCE {

        // not used
        private string _primaryCodesStr;
        private string _secondaryCodesStr;
        private Dictionary<string, List<Tuple<DateTime, double>>> priceHistory = new Dictionary<string, List<Tuple<DateTime, double>>>();

        public Dictionary<string, MarketSummary> cryptoPairs;
        public Dictionary<string, OrderBook> orderBooks;  // string format is eg "XBT-AUD" - caps with a dash

        // constructor
        public DCE(string _friendlyName) {
            cryptoPairs = new Dictionary<string, MarketSummary>();
            orderBooks = new Dictionary<string, OrderBook>();
            FriendlyName = _friendlyName;
        }

        public string FriendlyName { get; }

        // "Online" if everything is fine, anything else will cause the UI to display this string in the DCE group box text
        public string CurrentDCEStatus { get; set; }

        public List<Tuple<DateTime, double>> GetPriceList(string pair) {
            if (priceHistory.ContainsKey(pair)) {
                return priceHistory[pair];
            }
            else {
                return new List<Tuple<DateTime, double>>();
            }
        }

        public void CryptoPairsAdd(string pair, MarketSummary mSummary) {
            // ok here want to add it to the cryptopairs dictionary, but we also want to add the last price to a list so we can see trends
            pair = pair.ToUpper();
            // can delete this (hopefully) as cyptoPair[pair] = mSummary does an add or update automatically.  no need to remove.
            /*if (cryptoPairs.ContainsKey(pair)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                cryptoPairs.Remove(pair);
            }*/

            cryptoPairs[pair] = mSummary;

            if (!priceHistory.ContainsKey(pair)) {  // if this crypto/fiat pair hasn't come up before, create a new empty dictionary kvp
                priceHistory.Add(pair, new List<Tuple<DateTime, double>>());
            }
            priceHistory[pair].Add(new Tuple<DateTime, double>(DateTime.Now, mSummary.LastPrice));  // add the time and price to the kvp's value list
        }

        public string BuySell { get; set; }
        public string NumCoinsStr { get; set; } = "";
        public string CryptoCombo { get; set; } = "";

        /////////////////////////////////////////////////////////
        ////////////////      CURRENCIES      ///////////////////
        /////////////////////////////////////////////////////////

        public bool ChangedSecondaryCurrency { get; set; } = true;

        //private int ChosenPrimaryCurrency { get; set; } = 0;
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

        /*public string CurrentPrimaryCurrency {
            get {
                return PrimaryCurrencyList[ChosenPrimaryCurrency];
            }
        }*/

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

        public Dictionary<string, products_GDAX> ExchangeProducts { get; set; }


        /////////////////////////////////////////////////////////
        //////////////      JSON STUFF     //////////////////////
        /////////////////////////////////////////////////////////

        // this is the class that is used for all DCEs. It is based on IR's JSON.
        public class MarketSummary {

            private string _PrimaryCurrencyCode;
            private string _SecondaryCurrencyCode;

            public double DayHighestPrice { get; set; }
            public double DayLowestPrice { get; set; }
            public double DayAvgPrice { get; set; }
            public double DayVolume { get; set; }
            public double DayVolumeInSecondaryCurrency { get; set; }
            public double CurrentLowestOfferPrice { get; set; }
            public double CurrentHighestBidPrice { get; set; }
            public double LastPrice { get; set; }
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

            public double spread {
                get {
                    return CurrentLowestOfferPrice - CurrentHighestBidPrice;
                }
            }

            public string pair {
                get {
                    return PrimaryCurrencyCode + "-" + SecondaryCurrencyCode;
                }
            }
        }

        // this is the class used to deserialise BTC Market's market summary data
        public class MarketSummary_BTCM {
            public double bestBid { get; set; }
            public double bestAsk { get; set; }
            public double lastPrice { get; set; }
            public string currency { get; set; }  // fiat currency
            public string instrument { get; set; }  // cryptocurrency
            public double timestamp { get; set; }
            public double volume24h { get; set; }
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

        public class Order {

            public Order(string _orderType, double _price, double _volume) {
                OrderType = _orderType;
                Price = _price;
                Volume = _volume;
            }

            public string OrderType { get; set; }
            public double Price { get; set; }
            public double Volume { get; set; }
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
            public List<List<double>> asks { get; set; }
            public List<List<double>> bids { get; set; }
        }

        public class OrderBook_GDAX {
            public long sequence { get; set; }
            public List<List<string>> bids { get; set; }
            public List<List<string>> asks { get; set; }
        }
    }
}
