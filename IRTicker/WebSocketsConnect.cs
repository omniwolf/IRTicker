using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;


namespace IRTicker {
    class WebSocketsConnect {

        private Dictionary<string, DCE> DCEs;
        private WebSocket wSocket_BFX;
        private WebSocket wSocket_GDAX;
        public Dictionary<string, Subscribed_BFX> channel_Dict_BFX = new Dictionary<string, Subscribed_BFX>();  // string is a string version of the channel ID
        private BackgroundWorker pollingThread;

        public WebSocketsConnect(Dictionary<string, DCE> _DCEs, BackgroundWorker _pollingThread) {
            DCEs = _DCEs;
            pollingThread = _pollingThread;

            // BFX
            wSocket_BFX = new WebSocket("wss://api.bitfinex.com/ws");
            wSocket_BFX.OnMessage += (sender, e) => {
                if (e.IsText) {
                    MessageRX_BFX(e.Data);
                }
                else Debug.Print("BFX ws stream is not text?? - " + e.RawData.ToString());
            };

            wSocket_BFX.OnOpen += (sender, e) => {
                Debug.Print("ws onopen - bfx");
            };

            wSocket_BFX.OnError += (sender, e) => {
                Debug.Print("ws onerror - bfx");
                wSocket_BFX.Close();
                DCEs["BFX"].NetworkAvailable = false;
                DCEs["BFX"].CurrentDCEStatus = "Socket error";
                DCEs["BFX"].HasStaticData = false;
                pollingThread.ReportProgress(52);  // 52 is error
                wSocket_BFX.Connect();
            };

            wSocket_BFX.OnClose += (sender, e) => {
                Debug.Print("BFX stream closed... should be preceeded by some ded thingo " + DateTime.Now.ToString());
                //WebSocket_Reconnect("BFX");
            };
            wSocket_BFX.Connect();


            // GDAX
            wSocket_GDAX = new WebSocket("wss://ws-feed.pro.coinbase.com");

            wSocket_GDAX.OnMessage += (sender, e) => {
                if (e.IsText) {
                    MessageRX_GDAX(e.Data);
                    //Debug.Print("GDAX SOCKET: " + e.Data);
                }
                else Debug.Print("GDAX ws stream is not text?? - " + e.RawData.ToString());
            };

            wSocket_GDAX.OnOpen += (sender, e) => {
                Debug.Print("ws onopen - gdax");
            };

            wSocket_GDAX.OnError += (sender, e) => {
                Debug.Print("ws onerror - gdax - " + DateTime.Now.ToString());
                wSocket_GDAX.Close();
                DCEs["GDAX"].NetworkAvailable = false;
                DCEs["GDAX"].CurrentDCEStatus = "Socket error";
                DCEs["GDAX"].HasStaticData = false;
                pollingThread.ReportProgress(62);  // 62 is GDAX error
                wSocket_GDAX.Connect();
            };

            wSocket_GDAX.OnClose += (sender, e) => {
                Debug.Print("GDAX stream was closed.. should be because we disconnected on purpose. preceeded by ded?  " + DateTime.Now.ToString());
                //WebSocket_Disconnect("GDAX");
            };
            wSocket_GDAX.Connect();
        }

        public void WebSocket_Subscribe(string dExchange, string crypto, string fiat) {
            switch (dExchange) {
                case "BFX":
                    if (wSocket_BFX.IsAlive) {
                        if (crypto == "XBT") crypto = "BTC";
                        string channel = "{\"event\":\"subscribe\", \"channel\":\"ticker\", \"pair\":\"" + crypto + fiat + "\"}";
                        wSocket_BFX.Send(channel);
                    }

                    break;
                case "GDAX":
                    if (wSocket_GDAX.IsAlive) {
                        if (crypto == "XBT") crypto = "BTC";
                        string channel = "{\"type\": \"subscribe\", \"channels\": [{\"name\": \"ticker\", \"product_ids\": [\""+ crypto + "-" + fiat + "\"] } ] }";
                        wSocket_GDAX.Send(channel);
                    }
                    break;
            }
        }

        public void WebSocket_Reconnect(string dExchange) {
            switch (dExchange) {
                case "BFX":
                    wSocket_BFX.Close();
                    wSocket_BFX.Connect();

                    break;
                case "GDAX":
                    wSocket_GDAX.Close();
                    wSocket_GDAX.Connect();
                    break;
            }

            //re-subscribe?
            foreach (string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {
                foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                    if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + secondaryCode)) {
                        WebSocket_Subscribe(dExchange, primaryCode, secondaryCode);
                    }
                }
            }
        }

        // after calling this sub, please remove the KVP of the channel you're unsubscribing from from the channel_Dict_BFX dictionary
        public void Unsubscribe_BFX(int channelID) {
            try {
                if (wSocket_BFX.IsAlive) {
                    wSocket_BFX.Send("{\"event\":\"unsubscribe\",\"chanId\":\"" + channelID.ToString() + "\"}");
                }
            }
            catch (Exception ex) {
                Debug.Print("Exception when trying to unsubscribe - can't trust the .IsAlive property of the socket :( - " + ex.ToString());
            }
            //Debug.Print("just unsubscribed from " + channelID.ToString());
        }

        public void RemoveChannels(List<string> channelsToDelete) {
            foreach (string chans in channelsToDelete) channel_Dict_BFX.Remove(chans);
        }

        public Dictionary<string, Subscribed_BFX> GetChannelsDictionary_BFX() {
            return new Dictionary<string, Subscribed_BFX>(channel_Dict_BFX);
        }

        private void MessageRX_GDAX(string message) {

            if (message.Contains("\"type\":\"ticker\"")) {
                Ticker_GDAX tickerStream = new Ticker_GDAX();
                tickerStream = JsonConvert.DeserializeObject<Ticker_GDAX>(message);

                // now we convert it into a classic MarketSummary obj, and add it to cryptopairs
                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                // make it all XBT
                if (tickerStream.product_id.ToUpper().StartsWith("BTC")) tickerStream.product_id = tickerStream.product_id.Replace(tickerStream.product_id.Substring(0, 3), "XBT");

                mSummary.pair = tickerStream.product_id.ToUpper();
                if (double.TryParse(tickerStream.price, out double price)) {
                    mSummary.LastPrice = price;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert price: " + tickerStream.price);

                if (double.TryParse(tickerStream.volume_24h, out double vol)) {
                    mSummary.DayVolume = vol;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert volume: " + tickerStream.volume_24h);

                if (double.TryParse(tickerStream.low_24h, out double low)) {
                    mSummary.DayLowestPrice = low;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert low: " + tickerStream.low_24h);

                if (double.TryParse(tickerStream.high_24h, out double high)) {
                    mSummary.DayHighestPrice = high;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert high: " + tickerStream.high_24h);

                if (double.TryParse(tickerStream.best_bid, out double bid)) {
                    mSummary.CurrentHighestBidPrice = bid;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert bid: " + tickerStream.best_bid);

                if (double.TryParse(tickerStream.best_ask, out double offer)) {
                    mSummary.CurrentLowestOfferPrice = offer;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert ask: " + tickerStream.best_ask);

                mSummary.CreatedTimestampUTC = tickerStream.time.ToString("o");

                // market summary should be complete now
                DCEs["GDAX"].CryptoPairsAdd(mSummary.pair, mSummary);
                
                if (DCEs["GDAX"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode) pollingThread.ReportProgress(41, mSummary);  // only update the UI for pairs we care about
            }
            else if (message.Contains("\"type\": \"error\"")) {
                Debug.Print("Error from GDAX socket: " + message);
                // so i guess at this stage we want to try again
                wSocket_GDAX.Close();
                DCEs["GDAX"].CurrentDCEStatus = "API response error";
                DCEs["GDAX"].NetworkAvailable = false;
                DCEs["GDAX"].HasStaticData = false;
                pollingThread.ReportProgress(42, "GDAX");  // ?2 is error
                wSocket_GDAX.Connect();
            }
            else {
                Debug.Print("rando message from GDAX sockets: " + message);
            }
        }

        public bool IsSocketAlive(string dExchange) {

            switch (dExchange) {
                case "BFX":
                    if (wSocket_BFX.IsAlive) return true;
                    return false;
                case "GDAX":
                    if (wSocket_GDAX.IsAlive) return true;
                    return false;
            }
            Debug.Print("Sockets, checking a socket alive, we have reached the end without returning.  we never should.");
            return false;
        }

        /* BFX format:
            Fields 	            Type 	Description
            CHANNEL_ID 	        integer Channel ID
            BID 	            float 	Price of last highest bid
            BID_SIZE 	        float 	Size of the last highest bid
            ASK 	            float 	Price of last lowest ask
            ASK_SIZE 	        float 	Size of the last lowest ask
            DAILY_CHANGE    	float 	Amount that the last price has changed since yesterday
            DAILY_CHANGE_PERC 	float 	Amount that the price has changed expressed in percentage terms
            LAST_PRICE 	        float 	Price of the last trade.
            VOLUME          	float 	Daily volume
            HIGH 	            float 	Daily high
            LOW 	            float 	Daily low*/
        private void MessageRX_BFX(string message) {
            //Debug.Print("BFX stream: " + message);

            if (message.StartsWith("{")) {  // it's JSON, let's parse it as such
                if (message.Contains("\"event\":\"info\"")) {  // kinda like a header, we don't really care about this line.  it might look like this: {"event":"info","version":1.1,"platform":{"status":1}}
                    if (!message.Contains("\"status\":1")) {
                        Debug.Print("weird, we got a non 1 status? - " + message);
                    }
                }
                else if (message.Contains("\"event\":\"subscribed\"")) {  // OK, subscription notice.  let's grab the channel ID and store it.   line might look like this: {"event":"subscribed","channel":"ticker","chanId":60,"pair":"BTCUSD"}
                    Subscribed_BFX subscription = new Subscribed_BFX();
                    subscription = JsonConvert.DeserializeObject<Subscribed_BFX>(message);
                    channel_Dict_BFX[subscription.chanId.ToString()] = subscription;  // update or add
                    //Debug.Print("subscribed to " + subscription.chanId.ToString());
                }
                else if (message.Contains("\"event\":\"error\"")) {  // uh oh we done bad.  could look like this: {"channel":"ticker","pair":"BTCUSD","event":"error","msg":"subscribe: dup","code":10301}
                    Debug.Print("Error from BFX socket: " + message);
                    // so i guess at this stage we want to try again
                    wSocket_BFX.Close();
                    DCEs["BFX"].CurrentDCEStatus = "API response error";
                    DCEs["BFX"].NetworkAvailable = false;
                    DCEs["BFX"].HasStaticData = false;
                    pollingThread.ReportProgress(42, "BFX");  // 52 is error
                    wSocket_BFX.Connect();
                }
                else if (message.Contains("unsubscribed")) {
                    //Debug.Print("UNSUBSCRIBED!  message: " + message);
                }
                else {
                    Debug.Print("rando message from BFX sockets: " + message);
                }
            }
            else if (message.StartsWith("[")) {  // is this how I tell if it's real socket data?  seems dodgy
                message = Utilities.TrimEnds(message);  // remove the [ ] characters from the ends
                string[] streamParts = message.Split(',');
                if (channel_Dict_BFX.ContainsKey(streamParts[0])) {
                    if (channel_Dict_BFX[streamParts[0]].channel == "ticker" && !message.Contains(",\"hb\"]") && streamParts.Length == 11) {  // OK it's a ticker.  as long as it's not a heartbeat, let's convert all dem strings to dubs and put them in our DCE object
                        int partCount = 1;  // start at 1 because we already pulled out the channel ID
                        DCE.MarketSummary mSummary = new DCE.MarketSummary();
                        mSummary.PrimaryCurrencyCode = channel_Dict_BFX[streamParts[0]].pair.Substring(0, 3);
                        mSummary.SecondaryCurrencyCode = channel_Dict_BFX[streamParts[0]].pair.Substring(3, 3);
                        do {
                            if (double.TryParse(streamParts[partCount], out double result)) {
                                switch (partCount) {
                                    case 1:  // BID
                                        mSummary.CurrentHighestBidPrice = result;
                                        break;
                                    case 3: // ASK
                                        mSummary.CurrentLowestOfferPrice = result;
                                        break;
                                    case 7:  // LAST_PRICE
                                        mSummary.LastPrice = result;
                                        break;
                                    case 8:  // VOLUME
                                        mSummary.DayVolume = result;
                                        break;
                                    case 9:  // HIGH
                                        mSummary.DayHighestPrice = result;
                                        break;
                                    case 10:  // LOW
                                        mSummary.DayLowestPrice = result;
                                        break;
                                    /*default:
                                        Debug.Print("parsing the ticker info for BFX and have a default?  this shouldn't be possible.  parseCount: " + partCount + " and message: " + message);
                                        break;*/
                                }
                            }
                            else Debug.Print("Parsing ticker info in bFX, couldn't parse the string to a dub?? - " + streamParts[partCount]);

                            partCount++;

                        } while (partCount < 11);  // there should only be 11 entries

                        // market summary should be complete now
                        DCEs["BFX"].CryptoPairsAdd(channel_Dict_BFX[streamParts[0]].pairDash, mSummary);
                        //Debug.Print("just received pair: " + mSummary.pair + ", and chanID is: " + streamParts[0]);
                        if (DCEs["BFX"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode) pollingThread.ReportProgress(51, mSummary);  // only update the UI for pairs we care about
                        
                    }
                }
                else Debug.Print("weird.. BFX socket sent us a channel ID we don't have in the dict? - " + message);
            }
            else Debug.Print("BFX message from socket starts with something weird?? - " + message);
        }


        public class Subscribed_BFX {
            private string _pair;

            public string @event { get; set; }
            public string channel { get; set; }
            public int chanId { get; set; }
            public string pair {
                get {
                    return _pair;
                }
                set {
                    if (value.StartsWith("BTC")) {
                        _pair = value.Replace("BTC", "XBT");
                    }
                    else _pair = value;
                }
            }

            public string pairDash {
                get {
                    if (_pair.Length == 6) {
                        return _pair.Substring(0, 3) + "-" + _pair.Substring(3, 3);
                    }
                    else if (_pair.Length == 7) {  // laaammmeee  actually just looked at the BFX spec and all pairs are 6 characters.  will leave this in anyawy, but it shouldn't ever come to it
                        return _pair.Substring(0, 4) + "-" + _pair.Substring(4, 3);
                    }
                    else {
                        Debug.Print("We have a pair from the BFX socket that isn't 3 or 4 chars?  howww - " + _pair);
                        return "";
                    }
                }
            }
        }

        public class Ticker_GDAX {
            public string type { get; set; }
            public long sequence { get; set; }
            public string product_id { get; set; }
            public string price { get; set; }
            public string open_24h { get; set; }
            public string volume_24h { get; set; }
            public string low_24h { get; set; }
            public string high_24h { get; set; }
            public string volume_30d { get; set; }
            public string best_bid { get; set; }
            public string best_ask { get; set; }
            public string side { get; set; }
            public DateTime time { get; set; }
            public int trade_id { get; set; }
            public string last_size { get; set; }
        }
    }
}
