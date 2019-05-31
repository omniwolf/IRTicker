using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections.Concurrent;



namespace IRTicker {
    class WebSocketsConnect {

        private Dictionary<string, DCE> DCEs;
        private WebSocket wSocket_BFX, wSocket_GDAX, wSocket_IR, wSocket_BTCM;
        private Socket socket_BTCM;
        public Dictionary<string, Subscribed_BFX> channel_Dict_BFX = new Dictionary<string, Subscribed_BFX>();  // string is a string version of the channel ID
        private BackgroundWorker pollingThread;

        // constructor
        public WebSocketsConnect(Dictionary<string, DCE> _DCEs, BackgroundWorker _pollingThread) {
            DCEs = _DCEs;
            pollingThread = _pollingThread;

            // IR
            IR_Connect();
            
            // BTCM

            BTCM_Connect_v2();


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
                pollingThread.ReportProgress(12, "BFX");  // 12 is error
                WebSocket_Reconnect("BFX");
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
                pollingThread.ReportProgress(12, "GDAX");  // 12 is error
                WebSocket_Reconnect("GDAX");
            };

            wSocket_GDAX.OnClose += (sender, e) => {
                Debug.Print("GDAX stream was closed.. should be because we disconnected on purpose. preceeded by ded?  " + DateTime.Now.ToString());
                //WebSocket_Disconnect("GDAX");
            };
            wSocket_GDAX.Connect();
        }

        public void WebSocket_Subscribe(string dExchange, List<Tuple<string, string>> pairs) {
            string channel = "";
            switch (dExchange) {
                case "IR":
                    //Debug.Print("subscrbe IR: " + "{\"Event\":\"Subscribe\",\"Data\":[\"ticker-" + crypto + "-" + fiat + "\", \"" + "\"orderbook-" + crypto + "-" + fiat + "\"]} ");
                    //wSocket_IR.Send("{\"Event\":\"Subscribe\",\"Data\":[\"ticker-" + crypto + "-" + fiat + "\", \"orderbook-" + crypto + "-" + fiat + "\"]} ");
                    if (wSocket_IR.IsAlive) {
                        channel = "{\"Event\":\"Subscribe\",\"Data\":[";
                        foreach (Tuple<string, string> pair in pairs) {
                            string crypto = pair.Item1;
                            string fiat = pair.Item2;
                            channel += "\"orderbook-" + crypto + "-" + fiat + "\", ";
                            DCEs[dExchange].channelNonce[("ORDERBOOK-" + crypto + "-" + fiat).ToUpper()] = 0;  // initialise the nonce dictionary
                            DCEs[dExchange].nonceErrorTracker[("ORDERBOOK-" + crypto + "-" + fiat).ToUpper()] = DCEs[dExchange].OBResetFlag[("ORDERBOOK-" + crypto + "-" + fiat).ToUpper()] = false;  // false means no error, no need to dump OB
                        }
                        channel += "]} ";
                        wSocket_IR.Send(channel);
                        //wSocket_IR.Send("{\"Event\":\"Subscribe\",\"Data\":[\"orderbook-" + crypto + "-" + fiat + "\"]} ");
                    }
                    else DCEs["IR"].socketsReset = true;
                    break;
                case "BTCM":
                    if (true) {
                        
                        channel = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\", \"heartbeat\"], \"marketIds\":[";
                        foreach (Tuple<string, string> pair in pairs) {
                            string crypto = pair.Item1;
                            string fiat = pair.Item2;
                            if (crypto == "XBT") crypto = "BTC";
                            if (crypto == "BCH") crypto = "BCHABC";

                            channel += "\"" + crypto + "-" + fiat + "\", ";
                        }
                        channel = channel.Substring(0, channel.Length - 2) + "]}";

                        //pairList = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\"], \"marketIds\":[\"BTC-AUD\"]}";
                        wSocket_BTCM.Send(channel);
                    }
                    else {
                        //Debug.Print("trying to subscribe to BTCM " + crypto);

                       /* if (crypto == "XBT") crypto = "BTC";
                        if (crypto == "BCH") crypto = "BCHABC";
                        socket_BTCM.Emit("join", "Ticker-BTCMarkets-" + crypto + "-" + fiat);*/
                    }

                    break;
                case "BFX":
                    if (wSocket_BFX.IsAlive) {
                        foreach (Tuple<string, string> pair in pairs) {
                            string crypto = pair.Item1;
                            string fiat = pair.Item2;

                            if (crypto == "XBT") crypto = "BTC";
                            if (crypto == "BCH") crypto = "BAB";
                            channel = "{\"event\":\"subscribe\", \"channel\":\"ticker\", \"pair\":\"" + crypto + fiat + "\"}";
                            wSocket_BFX.Send(channel);
                        }
                    }

                    break;
                case "GDAX":
                    if (wSocket_GDAX.IsAlive) {
                        channel = "{\"type\": \"subscribe\", \"channels\": [{\"name\": \"ticker\", \"product_ids\":[";
                        foreach (Tuple<string, string> pair in pairs) {
                            string crypto = pair.Item1;
                            string fiat = pair.Item2;
                            if (crypto == "XBT") crypto = "BTC";
                            channel += "\"" + crypto + "-" + fiat + "\",";
                        }
                        channel = channel.Substring(0, channel.Length - 1);
                        channel += "] } ] }";
                        wSocket_GDAX.Send(channel);
                    }
                    break;
            }
        }

        public void IR_Connect() {
            wSocket_IR = new WebSocket("wss://websockets.independentreserve.com");
            wSocket_IR.OnMessage += (sender, e) => {
                if (e.IsText) {
                    //Debug.Print(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss") + " - IR sockets: " + e.Data);
                    MessageRX_IR(e.Data);
                }
                else Debug.Print("IR ws stream is not text?? - " + e.RawData.ToString());
            };

            wSocket_IR.OnOpen += (sender, e) => {
                Debug.Print("ws onopen - IR");
            };

            wSocket_IR.OnError += (sender, e) => {
                Debug.Print("ws onerror - IR");
                wSocket_IR.Close();
                DCEs["IR"].NetworkAvailable = false;
                DCEs["IR"].CurrentDCEStatus = "Socket error";
                //DCEs["IR"].HasStaticData = false;  // i don't think we should ever set this to false... once we have the static data that's all we need.
                pollingThread.ReportProgress(12, "IR");  // 12 is error
                //WebSocket_Reconnect("IR");
                DCEs["IR"].socketsReset = true;
            };

            wSocket_IR.OnClose += (sender, e) => {
                Debug.Print(DateTime.Now + " IR stream closed... should be preceeded by some ded thingo " + DateTime.Now.ToString());
            };

            wSocket_IR.Connect();
        }

        public void BTCM_Connect() {
            // BTCM

            IO.Options op = new IO.Options();
            op.Secure = true;
            op.Upgrade = false;
            op.Transports = System.Collections.Immutable.ImmutableList.Create("websocket");


            Debug.Print("starting btcm socket...");
            socket_BTCM = IO.Socket("https://socket.btcmarkets.net", op);
            socket_BTCM.On(Socket.EVENT_CONNECT, () => {
                Debug.Print("connecting to btcm channel...");
                //socket_BTCM.Emit("join", "Ticker-BTCMarkets-BTC-AUD");

            });

            socket_BTCM.On("newTicker", (data) => {
                MessageRX_BTCM(data.ToString());
            });

            socket_BTCM.On(Socket.EVENT_ERROR, (e) => {
                Debug.Print("ws onerror - btcm");
                socket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";
                //DCEs["BTCM"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                WebSocket_Reconnect("BTCM");
            });

            socket_BTCM.On(Socket.EVENT_CONNECT_ERROR, (e) => {
                Debug.Print("ws connection error - btcm");
                socket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket connection error";
                //DCEs["BTCM"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                WebSocket_Reconnect("BTCM");
            });

            socket_BTCM.On(Socket.EVENT_CONNECT_TIMEOUT, (e) => {
                Debug.Print("ws connection timeout - btcm");
                socket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket timeout";
                //DCEs["BTCM"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                WebSocket_Reconnect("BTCM");
            });

            socket_BTCM.On(Socket.EVENT_DISCONNECT, () => {
                // aww shit
                Debug.Print("BTCM socket disconnected.  reconnecting...");
                WebSocket_Reconnect("BTCM");
            });
        }

        public void BTCM_Connect_v2() {

            wSocket_BTCM = new WebSocket("wss://socket.btcmarkets.net/v2");
            wSocket_BTCM.OnMessage += (sender, e) => {
                //Debug.Print("!!! bTCMv2 got a message: " + e.Data);
                if (e.IsText) {
                    //Debug.Print(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss") + " - BTCMv2 sockets: " + e.Data);
                    MessageRX_BTCMv2(e.Data);
                }
                else Debug.Print("BTCMv2 ws stream is not text?? - " + e.RawData.ToString());
            };

            wSocket_BTCM.OnOpen += (sender, e) => {
                Debug.Print("ws onopen - BTCMv2");
            };

            wSocket_BTCM.OnError += (sender, e) => {
                Debug.Print("ws onerror - BTCMv2");
                wSocket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";
                
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                DCEs["BTCM"].socketsReset = true;
            };

            wSocket_BTCM.OnClose += (sender, e) => {
                Debug.Print(DateTime.Now + " BTCMv2 stream closed... should be preceeded by some ded thingo " + DateTime.Now.ToString());
            };

            wSocket_BTCM.Connect();
        }


        public void WebSocket_Reconnect(string dExchange) {
            Debug.Print("WebSocket_Reconnect for " + dExchange);
            if (!DCEs[dExchange].HasStaticData) {
                Debug.Print("Trying to reconnect to " + dExchange + " but there's no static data.  will not.");
                return;
            }
            switch (dExchange) {
                case "IR":
                    Debug.Print("switched to IR");
                    if (wSocket_IR.IsAlive) {
                        Debug.Print("IR - websockets is alive, closing websocket");
                        wSocket_IR.Close();
                        Debug.Print("IR - closed websocket");
                    }

                    IR_Connect();  // create all the sockets stuff again from scratch :/
                    DCEs["IR"].HeartBeat = DateTime.Now;
                    // clean out all the OBs
                    //DCEs[dExchange].IR_OBs = new ConcurrentDictionary<string, Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>>>();
                    Debug.Print(DateTime.Now + " IR - About to .clear the IR_OBs");
                    DCEs[dExchange].ClearOrderBookSubDicts();
                    Debug.Print(dExchange + " - cleared the order book dictionary, IR_OBs size: " + DCEs["IR"].IR_OBs.Count);
                    break;
                case "BTCM":
                    wSocket_BTCM.Close();
                    BTCM_Connect_v2();  // with sockets.io we need to start from scratch
                    break;
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
            Debug.Print(dExchange + " - re-subscribing to all pairs...");
            List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
            foreach (string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {
                foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                    if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + secondaryCode)) {
                        pairList.Add(new Tuple<string, string>(primaryCode, secondaryCode));
                    }
                }
            }
            WebSocket_Subscribe(dExchange, pairList);

            if (dExchange == "IR") {  // only for IR do we need to grab OBs via REST _after_ we have started the socket machine.  This is to reduce missed events.
                // re-populate the OBs using REST
                Debug.Print(dExchange + " - building new REST OBs");
                foreach (string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {
                    foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                        //if (DCEs[dExchange].IR_OBs.ContainsKey(primaryCode + "-" + secondaryCode)) {
                            DCEs[dExchange].GetIROrderBook(primaryCode, secondaryCode);
                        //}
                    }
                }
                Debug.Print("Rest OBs built.  IR_OBs size: " + DCEs[dExchange].IR_OBs.Count);
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

        /* Sample socket data:
        {
        "Event":"Trade",
        "Channel":"ticker-xbt-aud",
        "Nonce":1,
        "Data":{
            "TradeGuid":"c5bde544-d8ae-4e38-9e90-405a3f93b6d6",
            "TradeDate":"2009-01-03T18:15:05.9321664+00:00",
            "Volume":50.0,
            "Price":10270.0,
            "Pair":"xbt-aud",
            "BidGuid":"ebbeca4b-7148-4230-ad8f-833a3ccf35c2",
            "OfferGuid":"ad5ece89-083b-49fc-8bc1-bdb7482a9b9a",
            "Side":"Buy"
                }
        }*/
        private void MessageRX_IR(string message) {
            if (message.Contains("\"Event\":\"Subscriptions\"")) {
                // ignore the subscriptions event.  it breaks parsing too :/
                Debug.Print("IGNORING - " + message);
                return;
            }

            if (message.Contains("\"Event\":\"Heartbeat\"")) {
                // let's keep track of this.
                /*if (DCEs["IR"].HeartBeat.Year != 2000 && DCEs["IR"].HeartBeat + TimeSpan.FromSeconds(80) < DateTime.Now) {  // should be every 60 secs, but give it 20 secs leeway 
                    // ok we have lost the heartbeat.  close the socket re-download all OBs, then open the socket and re-subscribe to all channels
                    Debug.Print(DateTime.Now + " IR websockts hasn't received a heartbeat in over 80 seconds.  Starting fresh...");
                    //WebSocket_Reconnect("IR");
                    DCEs["IR"].socketsReset = true;
                    return;
                }*/
                Debug.Print(DateTime.Now + " IR - legit heartbeat");
                return;
            }
            DCEs["IR"].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat
             
            if (message.Contains("OrderChanged")) {
                //Debug.Print("IR order change: " + message);
            }
            Ticker_IR tickerStream = new Ticker_IR();
            tickerStream = JsonConvert.DeserializeObject<Ticker_IR>(message);

            // Nonce work.  make sure the nonce is sequential
            if (DCEs["IR"].channelNonce[tickerStream.Channel.ToUpper()] > 0) {  // 0 means we have never seen a nonce for this channel
                if (DCEs["IR"].channelNonce[tickerStream.Channel.ToUpper()] + 1 != tickerStream.Nonce) {  // why not??
                    Debug.Print(DateTime.Now.ToString() + " NONCE ERROR: " + tickerStream.Channel + ", old nonce: " + DCEs["IR"].channelNonce[tickerStream.Channel.ToUpper()] + ", new nonce: " + tickerStream.Nonce);
                    DCEs["IR"].nonceErrorTracker[tickerStream.Channel.ToUpper()] = DCEs["IR"].OBResetFlag[tickerStream.Channel.ToUpper()] = true;

                    // delete OB and re-download it, would be good to wait until the nonce stops throwing errors
                }
                else DCEs["IR"].nonceErrorTracker[tickerStream.Channel.ToUpper()] = false;  // if this is false and we're setting it again to false, fine - normal operation.  If this was true and we're now setting it to false, this means that we had some nonce errors but they seem to have settled down, and we can now dump and reconnect
            }
            DCEs["IR"].channelNonce[tickerStream.Channel.ToUpper()] = tickerStream.Nonce;  // regardless of whether it was in sequence, update it.

            // do we need to dump the OB?
            // if the nonceErrorTracker is true, this means the nonce has recovered from an error run.  if the OBResetFlag is true, it means we need to dump the OB.
            // because the nonce has settled down, we can now "safely" dump the OB and start again.
            if (!DCEs["IR"].nonceErrorTracker[tickerStream.Channel.ToUpper()] && DCEs["IR"].OBResetFlag[tickerStream.Channel.ToUpper()]) {
                Debug.Print("NONCE - we have recovered from an error in " + tickerStream.Channel + ", time to dump and restart");
                DCEs["IR"].OBResetFlag[tickerStream.Channel.ToUpper()] = false;

                if (wSocket_IR.IsAlive) {

                    //wSocket_IR.Close();  // don't do this, we need to unsubscribe from JUST the channel!
                    wSocket_IR.Send("{\"Event\":\"Unsubscribe\",\"Data\":[\"" + tickerStream.Channel + "\"]} ");

                    // now need to dump the OBs. 
                    //DCEs["IR"].IR_OBs.TryRemove(tickerStream.Data.Pair.ToUpper(), out Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> ignore);
                    DCEs["IR"].IR_OBs[tickerStream.Data.Pair.ToUpper()].Item1.Clear();
                    DCEs["IR"].IR_OBs[tickerStream.Data.Pair.ToUpper()].Item2.Clear();

                    // now subscribe back to the channel
                    Tuple<string, string> pairTup = Utilities.SplitPair(tickerStream.Data.Pair);
                    List<Tuple<string, string>> tempList = new List<Tuple<string, string>>();
                    tempList.Add(new Tuple<string, string>(pairTup.Item1, pairTup.Item2));
                    WebSocket_Subscribe("IR", tempList);

                    // re-populate the OB using REST
                    DCEs["IR"].GetIROrderBook(pairTup.Item1, pairTup.Item2);
                }
                else DCEs["IR"].socketsReset = true;

                return;  // no need to interpret the rest of the event, we starting fresh on this orderbook.
            }

            // have removed this because i think we should be prescriptive about getting the order book.  If we just always get it when it's not there
            // then we might grab it 5 times in one second
            /*if (!DCEs["IR"].IR_OBs.ContainsKey(tickerStream.Data.Pair.ToUpper())) {
                Debug.Print(DateTime.Now.ToString() + " IR - receieved an event we don't have a pair for (" + tickerStream.Data.Pair + "), will grab it");

                Tuple<string, string> tempTup = Utilities.SplitPair(tickerStream.Data.Pair.ToUpper());

                DCEs["IR"].GetIROrderBook(tempTup.Item1, tempTup.Item2);
                //WebSocket_Subscribe("IR", tempTup.Item1, tempTup.Item2);
                //return;
            }*/


            switch (tickerStream.Event) {
                case "Heartbeat":
                    // do nothing.  Maybe we could get a timestamp and check that we're receiving regular heartbeats?  something for later
                    // this shouldn't get hit anymore, we filter it out above.  This means every event that gets below the first IF of the function above 
                    // will have a nonce.  if we decide later we want to use the heartbeat, will have to only check nonces for OB or ticker channels
                    Debug.Print("IR HB");
                    break;
                case "Trade":
                    DCE.OrderBook_IR obIR = new DCE.OrderBook_IR();
                    //tickerStream.
                    break;
                case "NewOrder":
                    /*Debug.Print("IR new: " + message);
                    break;*/
                case "OrderChanged":
                case "OrderCanceled":

                    // this if block is pure debugging
                    if (((tickerStream.Event == "OrderChanged" && tickerStream.Data.Volume == 0) || (tickerStream.Event == "OrderCanceled")) && tickerStream.Data.Pair.ToUpper() == "PLA-AUD") {
                        bool foundCancelled = false;
                        //Debug.Print("EVENT changed, worknig out price...");
                        if (tickerStream.Data.OrderType.ToUpper().EndsWith("BID")) {
                                                       
                            //var BidOrders = DCEs["IR"].IR_OBs[tickerStream.Data.Pair.ToUpper()].Item1;
                            //foreach (var PriceLevel in BidOrders) {
                                //if (PriceLevel.Value.ContainsKey(tickerStream.Data.OrderGuid)) {
                                if (DCEs["IR"].OrderGuid_IR_OBs[tickerStream.Data.Pair.ToUpper()].Item1.ContainsKey(tickerStream.Data.OrderGuid)) { 
                                    Debug.Print(DateTime.Now.ToString() + " | EVENT(" + tickerStream.Data.Pair + ") " + tickerStream.Event + ": " + DCEs["IR"].OrderGuid_IR_OBs[tickerStream.Data.Pair.ToUpper()].Item1[tickerStream.Data.OrderGuid]);
                                    foundCancelled = true;
                                }
                            //}
                        }
                        else {
                            //var OfferOrders = DCEs["IR"].IR_OBs[tickerStream.Data.Pair.ToUpper()].Item2;
                           // foreach (var PriceLevel in OfferOrders) {
                                //if (PriceLevel.Value.ContainsKey(tickerStream.Data.OrderGuid)) {
                                if (DCEs["IR"].OrderGuid_IR_OBs[tickerStream.Data.Pair.ToUpper()].Item2.ContainsKey(tickerStream.Data.OrderGuid)) { 
                                    Debug.Print(DateTime.Now.ToString() + " | EVENT(" + tickerStream.Data.Pair + ") " + tickerStream.Event + ": " + DCEs["IR"].OrderGuid_IR_OBs[tickerStream.Data.Pair.ToUpper()].Item2[tickerStream.Data.OrderGuid]);
                                    foundCancelled = true;
                                }
                            
                        }
                        if (!foundCancelled) {
                            Debug.Print("we have a " + tickerStream.Event + " order, but can't find it in either orderbook? " + tickerStream.Data.OrderGuid + " " + tickerStream.Data.OrderType);
                        }
                    }


                    // if this OrderBookEvent_IR function returns true, it means the event we just received made changes to the spread.  let's update the UI.
                    if (DCEs["IR"].OrderBookEvent_IR(tickerStream.Event, tickerStream.Data)) {
                        // create an mSummary object, and report it with code 21
                        // first need to find out what pair this event is talking about.
                        Tuple<string, string> eventPair = Utilities.SplitPair(tickerStream.Data.Pair.ToUpper());

                        // next we need to pull the mSummary object out of the cryptoPairs array :/
                        //Debug.Print("spread changing event: " + message);
                        if (DCEs["IR"].GetCryptoPairs()[tickerStream.Data.Pair.ToUpper()].spread < 0) {
                            Debug.Print(DateTime.Now + " IR spread (" + tickerStream.Data.Pair + ") is " + DCEs["IR"].GetCryptoPairs()[tickerStream.Data.Pair.ToUpper()].spread + " :(  bid: " + DCEs["IR"].GetCryptoPairs()[tickerStream.Data.Pair.ToUpper()].CurrentHighestBidPrice + " and offer: " + DCEs["IR"].GetCryptoPairs()[tickerStream.Data.Pair.ToUpper()].CurrentLowestOfferPrice);
                            //WebSocket_Reconnect("IR");
                            //DCEs["IR"].socketsReset = true;
                            //return;
                        }
                        if (DCEs["IR"].CurrentSecondaryCurrency == eventPair.Item2.ToUpper()) {
                            DCE.MarketSummary mSummary = DCEs["IR"].GetCryptoPairs()[tickerStream.Data.Pair.ToUpper()];
                            pollingThread.ReportProgress(21, mSummary);
                        }
                        else {
                            DCE.MarketSummary mSummary = DCEs["IR"].GetCryptoPairs()[tickerStream.Data.Pair.ToUpper()];
                            pollingThread.ReportProgress(25, mSummary);
                        }
                    }
                    break;
            }

            //mSummary.CurrentHighestBidPrice = tickerStream.Data.   // need to finish this later i guess?

        }


        /* Sample socket data:
         * {
         *      "volume24h": 21357294294,
         *      "bestBid": 964871000000,
         *      "bestAsk": 965578000000,
         *      "lastPrice": 965600000000,
         *      "timestamp": 1535538694373,
         *      "snapshotId": 1535538694373000,
         *      "marketId": 2001,
         *      "currency": "AUD",
         *      "instrument": "BTC"
         * }
         */
        private void MessageRX_BTCM(string message) {
            //Debug.Print("BTCM STREAM: " + message);

            Ticker_BTCM tickerStream = new Ticker_BTCM();
            tickerStream = JsonConvert.DeserializeObject<Ticker_BTCM>(message);

            DCE.MarketSummary mSummary = new DCE.MarketSummary();

            if (tickerStream.instrument.ToUpper().StartsWith("BTC")) tickerStream.instrument = tickerStream.instrument.Replace(tickerStream.instrument.Substring(0, 3), "XBT");
            if (tickerStream.instrument.ToUpper().StartsWith("BCHABC")) tickerStream.instrument = tickerStream.instrument.Replace(tickerStream.instrument.Substring(0, 6), "BCH");
            mSummary.DayVolume = tickerStream.volume24h / 100000000;  // 100 million
            mSummary.CurrentHighestBidPrice = tickerStream.bestBid / 100000000; // 100 million
            mSummary.CurrentLowestOfferPrice = tickerStream.bestAsk / 100000000;  // 100 mil
            mSummary.LastPrice = tickerStream.lastPrice / 100000000;  // 100 mil

            // had to comment this out because i changed the Ticker_BTCM class timestame property type
            //DateTimeOffset DTO = DateTimeOffset.FromUnixTimeMilliseconds(tickerStream.timestamp);
            //mSummary.CreatedTimestampUTC = DTO.LocalDateTime.ToString("o");

            mSummary.SecondaryCurrencyCode = tickerStream.currency;
            mSummary.PrimaryCurrencyCode = tickerStream.instrument;

            // market summary should be complete now
            DCEs["BTCM"].CryptoPairsAdd(mSummary.pair, mSummary);

            // BTCM only has one secondary currency, so it will always be hit.  keep this here in case they get more i guess.
            if (DCEs["BTCM"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode) pollingThread.ReportProgress(31, mSummary);  // only update the UI for pairs we care about
        }

    /// <summary>
        /// sample tick return payload
        /// { 
        ///     marketId: 'BTC-AUD',
        ///     timestamp: '2019-04-08T18:56:17.405Z',
        ///     bestBid: '7309.12',
        ///     bestAsk: '7326.88',
        ///     lastPrice: '7316.81',
        ///     volume24h: '299.12936654',
        ///     messageType: 'tick' 
        /// }
    /// </summary>
    /// <param name="message"></param>
    private void MessageRX_BTCMv2(string message) {
            //Debug.Print("BTCM STREAM: " + message);

            if (message.Contains("\"messageType\":\"tick\"")) {    
                Ticker_BTCM tickerStream = new Ticker_BTCM();
                tickerStream = JsonConvert.DeserializeObject<Ticker_BTCM>(message);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                if (tickerStream.marketId.ToUpper().StartsWith("BTC")) tickerStream.marketId = tickerStream.marketId.Replace(tickerStream.marketId.Substring(0, 3), "XBT");
                if (tickerStream.marketId.ToUpper().StartsWith("BCHABC")) tickerStream.marketId= tickerStream.marketId.Replace(tickerStream.marketId.Substring(0, 6), "BCH");

                mSummary.DayVolume = tickerStream.volume24h;
                mSummary.CurrentHighestBidPrice = tickerStream.bestBid;
                mSummary.CurrentLowestOfferPrice = tickerStream.bestAsk;
                mSummary.LastPrice = tickerStream.lastPrice;

                mSummary.CreatedTimestampUTC = tickerStream.timestamp;
                Tuple<string, string> pair = Utilities.SplitPair(tickerStream.marketId);
                mSummary.SecondaryCurrencyCode = pair.Item2;
                mSummary.PrimaryCurrencyCode = pair.Item1;

                // market summary should be complete now
                DCEs["BTCM"].CryptoPairsAdd(mSummary.pair, mSummary);

                // BTCM only has one secondary currency, so it will always be hit.  keep this here in case they get more i guess.
                if (DCEs["BTCM"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode) pollingThread.ReportProgress(31, mSummary);  // only update the UI for pairs we care about
                DCEs["BTCM"].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat

            }
            else if (message.Contains("\"messageType\":\"heartbeat\"")) {
                // let's keep track of this.
                //Debug.Print(DateTime.Now + " - BTCMv2 - legit heartbeat");
                DCEs["BTCM"].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat
            }
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
                if (decimal.TryParse(tickerStream.price, out decimal price)) {
                    mSummary.LastPrice = price;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert price: " + tickerStream.price);

                if (decimal.TryParse(tickerStream.volume_24h, out decimal vol)) {
                    mSummary.DayVolume = vol;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert volume: " + tickerStream.volume_24h);

                if (decimal.TryParse(tickerStream.low_24h, out decimal low)) {
                    mSummary.DayLowestPrice = low;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert low: " + tickerStream.low_24h);

                if (decimal.TryParse(tickerStream.high_24h, out decimal high)) {
                    mSummary.DayHighestPrice = high;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert high: " + tickerStream.high_24h);

                if (decimal.TryParse(tickerStream.best_bid, out decimal bid)) {
                    mSummary.CurrentHighestBidPrice = bid;
                }
                else Debug.Print("Error GDAX sockets - couldn't convert bid: " + tickerStream.best_bid);

                if (decimal.TryParse(tickerStream.best_ask, out decimal offer)) {
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
                //DCEs["GDAX"].HasStaticData = false;
                pollingThread.ReportProgress(12, "GDAX");  // ?2 is error
                wSocket_GDAX.Connect();
            }
            else {
                Debug.Print("rando message from GDAX sockets: " + message);
            }
        }

        /// <summary>
        /// Do not assume any exchange can be handled here... BTCM cannot for example.  Only call this method with specific known good DCEs
        /// </summary>
        /// <param name="dExchange"></param>
        /// <returns></returns>
        public bool IsSocketAlive(string dExchange) {

            switch (dExchange) {
                case "IR":
                    if (wSocket_IR.IsAlive) return true;
                    return false;
                case "BTCM":
                    if (wSocket_BTCM.IsAlive) return true;
                    return false;
                case "BFX":
                    if (wSocket_BFX.IsAlive) return true;
                    return false;
                case "GDAX":
                    if (wSocket_GDAX.IsAlive) return true;
                    return false;
            }
            Debug.Print("Sockets, checking a socket alive, we have reached the end without returning.  we never should.  DCE: " + dExchange);
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
                    //Debug.Print("subscribed to " + subscription.chanId.ToString() + " which is " + subscription.pair);
                }
                else if (message.Contains("\"event\":\"error\"")) {  // uh oh we done bad.  could look like this: {"channel":"ticker","pair":"BTCUSD","event":"error","msg":"subscribe: dup","code":10301}
                    Debug.Print("Error from BFX socket: " + message);
                    // so i guess at this stage we want to try again
                    wSocket_BFX.Close();
                    DCEs["BFX"].CurrentDCEStatus = "API response error";
                    DCEs["BFX"].NetworkAvailable = false;
                    //DCEs["BFX"].HasStaticData = false;
                    pollingThread.ReportProgress(12, "BFX");  // 12 is error
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
                //Debug.Print("BFX MESSAGE: " + message);
                message = Utilities.TrimEnds(message);  // remove the [ ] characters from the ends
                string[] streamParts = message.Split(',');
                if (channel_Dict_BFX.ContainsKey(streamParts[0])) {
                    if (channel_Dict_BFX[streamParts[0]].channel == "ticker" && !message.Contains(",\"hb\"]") && streamParts.Length == 11) {  // OK it's a ticker.  as long as it's not a heartbeat, let's convert all dem strings to dubs and put them in our DCE object
                        int partCount = 1;  // start at 1 because we already pulled out the channel ID
                        DCE.MarketSummary mSummary = new DCE.MarketSummary();
                        mSummary.PrimaryCurrencyCode = channel_Dict_BFX[streamParts[0]].pair.Substring(0, 3);
                        mSummary.SecondaryCurrencyCode = channel_Dict_BFX[streamParts[0]].pair.Substring(3, 3);
                        do {
                            if (decimal.TryParse(streamParts[partCount], out decimal result)) {
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

        public class Data_IR_Ticker {
            public string TradeGuid { get; set; }
            public string TradeDate { get; set; }
            public decimal Volume { get; set; }
            public decimal Price { get; set; }
            public string Pair { get; set; }
            public string BidGuid { get; set; }
            public string OfferGuid { get; set; }
            public string Side { get; set; }
        }

        public class Ticker_IR {
            public string Event { get; set; }
            public string Channel { get; set; }
            public int Nonce { get; set; }
            public DCE.OrderBook_IR Data { get; set; }
        }

        public class Ticker_BTCM {
            public decimal volume24h { get; set; }  // needs to be divided by 100 000 000 yes 100 million.  ffs
            public decimal bestBid { get; set; }
            public decimal bestAsk { get; set; }
            public decimal lastPrice { get; set; }  // this and the 2 above need to be divided by 100 million to get the price.  
            public string timestamp { get; set; }  // for websockets v1 this is a long, for v2 it's a string
            public double snapshotId { get; set; }
            public string marketId { get; set; }
            public string currency { get; set; }
            public string instrument { get; set; }
            public string messageType { get; set; }
        }

    public class Subscribed_BFX {
            private string _pair;

            public string @event { get; set; }
            public string channel { get; set; }
            public int chanId { get; set; }
            public string pair {
                get {
                    if (_pair.StartsWith("BTC")) {
                        return _pair.Replace("BTC", "XBT");
                    }
                    if (_pair.StartsWith("BAB")) {
                        return _pair.Replace("BAB", "BCH");
                    }
                    else return _pair;
                }
                set {
                    if (value.StartsWith("BTC")) {
                        _pair = value.Replace("BTC", "XBT");
                    }
                    if (value.StartsWith("BAB")) {
                        _pair = value.Replace("BAB", "BCH");
                    }
                    else _pair = value;
                }
            }

            public string pairDash {
                get {
                    string __pair = _pair;
                    if (_pair.StartsWith("BTC")) {
                        __pair = _pair.Replace("BTC", "XBT");
                    }
                    if (_pair.StartsWith("BAB")) {
                        __pair = _pair.Replace("BAB", "BCH");
                    }

                    if (__pair.Length == 6) {
                        return __pair.Substring(0, 3) + "-" + __pair.Substring(3, 3);
                    }
                    else if (__pair.Length == 7) {  // laaammmeee  actually just looked at the BFX spec and all pairs are 6 characters.  will leave this in anyawy, but it shouldn't ever come to it
                        return __pair.Substring(0, 4) + "-" + __pair.Substring(4, 3);
                    }
                    else {
                        Debug.Print("We have a pair from the BFX socket that isn't 3 or 4 chars?  howww - " + __pair);
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
