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
        private int _chosenPrimaryCurrency = 0;
        private int _chosenSecondaryCurrency = 0;
        public Dictionary<string, MarketSummary> cryptoPairs;

        // constructor
        public DCE() {
            cryptoPairs = new Dictionary<string, MarketSummary>();
        }

        public int chosenPrimaryCurrency {
            get {
                return _chosenPrimaryCurrency;
            }
            set {
                _chosenPrimaryCurrency = value;
            }
        }

        public int chosenSecondaryCurrency {
            get {
                return _chosenSecondaryCurrency;
            }
            set {
                _chosenSecondaryCurrency = value;
            }
        }

        /// <summary>
        /// Crypto, needs to take codes as comma separated string with quotation marks around each symbol, eg "\"XBT\",\"BCH\",\"ETH\""
        /// </summary>
        public string primaryCurrencyCodes {
            get {
                return _primaryCodesStr;
            }
            set {
                _primaryCodesStr = value.ToUpper();
            }
        }

        public string currentPrimaryCurrency {
            get {
                return primaryCurrencyList[_chosenPrimaryCurrency];
            }
        }

        public List<string> primaryCurrencyList {
            get {
                List<string> codesList = new List<string>();
                string[] codess = _primaryCodesStr.Split(',');
                foreach(string cc in codess) {
                    string cc2 = Utilities.trimEnds(cc);
                    codesList.Add(cc2);
                }
                return codesList;
            }
        }

        /// <summary>
        /// Fiat, needs to take codes as comma separated string with quotation marks around each symbol, eg "\"AUD\",\"USD\",\"NZD\""
        /// </summary>
        public string secondaryCurrencyCodes {
            get {
                return _secondaryCodesStr;
            }
            set {
                _secondaryCodesStr = value.ToUpper();
            }
        }

        public List<string> secondaryCurrencyList {
            get {
                List<string> codesList = new List<string>();
                string[] codess = _secondaryCodesStr.Split(',');
                foreach(string cc in codess) {
                    string cc2 = Utilities.trimEnds(cc);
                    codesList.Add(cc2);
                }
                return codesList;
            }
        }

        public string currentSecondaryCurrency {
            get {
                return secondaryCurrencyList[_chosenSecondaryCurrency];
            }
        }

        /// <summary>
        /// This moves the chosenSecondaryCurrency value to the next one (or to the first if we're on the last one)
        /// </summary>
        public void nextSecondaryCurrency() {
            if (_chosenSecondaryCurrency == secondaryCurrencyList.Count - 1) {
                _chosenSecondaryCurrency = 0;
            }
            else {
                _chosenSecondaryCurrency++;
            }
        }

        // this is the class that is used for all DCEs. It is based on IR's JSON.
        public class MarketSummary {

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
            public string PrimaryCurrencyCode { get; set; }  // crypto
            /// <summary>
            /// fiat
            /// </summary>
            public string SecondaryCurrencyCode { get; set; }  // fiat
            public string CreatedTimestampUTC { get; set; }

            public double spread {
                get {
                    return CurrentLowestOfferPrice - CurrentHighestBidPrice;
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
    }
}
