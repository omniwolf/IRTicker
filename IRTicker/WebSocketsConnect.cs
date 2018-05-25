using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Diagnostics;
using Newtonsoft.Json;


namespace IRTicker {
    class WebSocketsConnect {

        private Dictionary<string, DCE> DCEs;
        private WebSocket wSocket_BFX;
        private WebSocket wSocket_GDAX;
        private Dictionary<string, Subscribed_BFX> channel_Dict_BFX = new Dictionary<string, Subscribed_BFX>();

        public WebSocketsConnect(Dictionary<string, DCE> _DCEs) {
            DCEs = _DCEs;

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
            };

            wSocket_BFX.OnClose += (sender, e) => {
                //Debug.Print("ws onclose");
            };
            wSocket_BFX.Connect();


            // GDAX ?
        }

        public void WebSocket_Connect(string dExchange, string crypto, string fiat) {
            switch (dExchange) {
                case "BFX":
                    if (crypto == "XBT") crypto = "BTC";
                    string channel = "{\"event\":\"subscribe\", \"channel\":\"ticker\", \"pair\":\"" + crypto + fiat + "\"}";
                    wSocket_BFX.Send(channel);

                    break;
                case "GDAX":

                    break;
            }
        }

        public void WebSocket_Disconect(string dExchange) {
            switch (dExchange) {
                case "BFX":
                    wSocket_BFX.Close();
                    break;
            }
        }

        public void Unsubscribe_BFX(string channelID) {
            wSocket_BFX.Send("{\"event\":\"unsubscribe\",\"chanId\":\"" + channelID + "\"");
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
            Debug.Print("BFX stream: " + message);

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
                }
                else if (message.Contains("\"event\":\"error\"")) {  // uh oh we done bad.  could look like this: {"channel":"ticker","pair":"BTCUSD","event":"error","msg":"subscribe: dup","code":10301}
                    Debug.Print("Error from BFX socket: " + message);
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
                    }
                }
                else Debug.Print("weird.. BFX socket sent us a channel ID we don't have in the dict? - " + message);
            }
            else Debug.Print("BFX message from socket starts with something weird?? - " + message);
        }




        public class Subscribed_BFX {
            private string _pair;
            private string _pairDash;

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



    }
}
