﻿using System;
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
    public class WebSocketsConnect {

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
                    DCEs["BFX"].socketsAlive = true;
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
                DCEs["BFX"].socketsAlive = false;
                DCEs["BFX"].CurrentDCEStatus = "Socket error";
                DCEs["BFX"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BFX");  // 12 is error
                WebSocket_Reconnect("BFX");
            };

            wSocket_BFX.OnClose += (sender, e) => {
                Debug.Print("BFX stream closed... should be preceeded by some ded thingo " + DateTime.Now.ToString());
                DCEs["BFX"].socketsAlive = false;
                //WebSocket_Reconnect("BFX");
            }; 
            wSocket_BFX.Connect();


            // GDAX
            wSocket_GDAX = new WebSocket("wss://ws-feed.pro.coinbase.com");

            wSocket_GDAX.OnMessage += (sender, e) => {
                if (e.IsText) {
                    DCEs["GDAX"].socketsAlive = true;
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
                DCEs["GDAX"].socketsAlive = false;
                DCEs["GDAX"].CurrentDCEStatus = "Socket error";
                DCEs["GDAX"].HasStaticData = false;
                pollingThread.ReportProgress(12, "GDAX");  // 12 is error
                WebSocket_Reconnect("GDAX");
            };

            wSocket_GDAX.OnClose += (sender, e) => {
                Debug.Print("GDAX stream was closed.. should be because we disconnected on purpose. preceeded by ded?  " + DateTime.Now.ToString());
                DCEs["GDAX"].socketsAlive = false;
                //WebSocket_Disconnect("GDAX");
            };
            wSocket_GDAX.Connect();
        }

        public void GetOrderBook_IR(string crypto, string fiat) {
            if (crypto == "USDT") crypto = "UST";
            string pair = crypto + "-" + fiat;
            Tuple<bool, string> orderBookTpl = Utilities.Get("https://api.independentreserve.com/Public/GetAllOrders?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);

            // have to change back ughhh
            if (crypto == "UST") crypto = "USDT";
            pair = crypto + "-" + fiat;

            if (orderBookTpl.Item1) {
                DCE.OrderBook orderBook = JsonConvert.DeserializeObject<DCE.OrderBook>(orderBookTpl.Item2);
                if (orderBook.PrimaryCurrencyCode.ToUpper() == "UST") orderBook.PrimaryCurrencyCode = "USDT";
                DCEs["IR"].orderBooks[pair] = orderBook;

                // next we need to convert this orderbook into a concurrent dictionary of OrderBook_IR objects
                // so yeah.. the "orderBook" object doesn't really get used anymore.  it's just like a staging area
                DCEs["IR"].ConvertOrderBook_IR(pair);

                Debug.Print(DateTime.Now.ToString() + " IR OB " + crypto + fiat + " done");

                //int remainingBuffer = ApplyBuffer_IR(pair);
                //Print("(" + pair + ") Buffer applied, there are " + remainingBuffer + " left in the buffer (should be 0)");
                DCEs["IR"].pulledSnapShot[pair] = true;
            }
            else {
                Debug.Print(DateTime.Now.ToString() + " | IR - couldn't download REST OB? - " + pair);
            }
        }

        private int ApplyBuffer_IR(string pair) {
            // this pair doesn't even exist in the OB
            // the pair is there, but nothing in it.
            if (!DCEs["IR"].orderBuffer_IR.ContainsKey(pair) || DCEs["IR"].orderBuffer_IR[pair].Count == 0) {
                DCEs["IR"].pulledSnapShot[pair] = true;
                return 0;
            }

            if (DCEs["IR"].channelNonce["ORDERBOOK-" + pair] == 0) {
                DCEs["IR"].channelNonce["ORDERBOOK-" + pair] = DCEs["IR"].orderBuffer_IR[pair].Keys.Min() - 1;
            }

            Debug.Print(DateTime.Now + " --- about to apply buffer.  missing change or remove order errors are probably expected. current nonce: " + DCEs["IR"].channelNonce["ORDERBOOK-" + pair] + " -- -");

            // there's some problem here.  it doesn't appear to be popping all the buffers?
            while (DCEs["IR"].orderBuffer_IR[pair].ContainsKey(DCEs["IR"].channelNonce["ORDERBOOK-" + pair] + 1)) {
                DCEs["IR"].channelNonce["ORDERBOOK-" + pair] += 1;
                Debug.Print(DateTime.Now + " - popping an order (nonce: " + DCEs["IR"].channelNonce["ORDERBOOK-" + pair] + ") from the " + pair + " buffer, total prior to pop: " + DCEs["IR"].orderBuffer_IR[pair].Count);
                if (DCEs["IR"].orderBuffer_IR[pair].TryRemove(DCEs["IR"].channelNonce["ORDERBOOK-" + pair], out Ticker_IR ticker)) {
                    Debug.Print("1TryRemove supposedly worked, total now is " + DCEs["IR"].orderBuffer_IR[pair].Count + ", popped nonce is: " + ticker.Nonce);
                    if (DCEs["IR"].orderBuffer_IR[pair].ContainsKey(ticker.Nonce)) {
                        Debug.Print("1yep defo in there -- BAD");
                    }
                    else {
                        Debug.Print("1nup not in there -- good");
                    }
                    parseTicker_IR(ticker);
                }
                else {
                    Debug.Print(DateTime.Now + " - couldn't remove order from orderbuffer??");
                }
                Debug.Print("2TryRemove supposedly worked, total now is " + DCEs["IR"].orderBuffer_IR[pair].Count + ", popped nonce is: " + ticker.Nonce);
                Debug.Print("next nonce in the buffer is: " + DCEs["IR"].orderBuffer_IR[pair].Keys.Min());
                if (DCEs["IR"].orderBuffer_IR[pair].ContainsKey(DCEs["IR"].channelNonce["ORDERBOOK-" + pair])) {
                    Debug.Print("2yep defo in there -- bad");
                }
                else {
                    Debug.Print("2nup not in there - good");
                }
                if (DCEs["IR"].orderBuffer_IR[pair].ContainsKey(DCEs["IR"].channelNonce["ORDERBOOK-" + pair] + 1)) {
                    Debug.Print("next nonce (" + (DCEs["IR"].channelNonce["ORDERBOOK-" + pair] + 1) + " is there waiting! -- good");
                }
                else {
                    Debug.Print("next nonce notin buffer BAD, even though buffer size is " + DCEs["IR"].orderBuffer_IR[pair].Count);
                }
            }

            DCEs["IR"].pulledSnapShot[pair] = true;
            //Debug.Print(DateTime.Now + " --- buffer has been applied, any new errors are probably real");

            return DCEs["IR"].orderBuffer_IR[pair].Count;
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
                            DCEs["IR"].pulledSnapShot[crypto + "-" + fiat] = false;  // initialise the pulledSnapShot variable for this pair
                            if (crypto == "USDT") crypto = "UST";
                            channel += "\"orderbook-" + crypto + "-" + fiat + "\", ";
                            if (crypto == "UST") crypto = "USDT";
                            DCEs[dExchange].channelNonce[("ORDERBOOK-" + crypto + "-" + fiat)] = 0;  // initialise the nonce dictionary
                            DCEs[dExchange].OBResetFlag["ORDERBOOK-" + crypto + "-" + fiat] = false;  // false means no error, no need to dump OB
                        }
                        channel += "]} ";
                        Debug.Print("IR websocket subscribe: " + channel);
                        wSocket_IR.Send(channel);
                        //wSocket_IR.Send("{\"Event\":\"Subscribe\",\"Data\":[\"orderbook-" + crypto + "-" + fiat + "\"]} ");

                        foreach (Tuple<string, string> pair in pairs) {
                            GetOrderBook_IR(pair.Item1, pair.Item2);
                        }


                    }
                    else {
                        DCEs["IR"].socketsReset = true;
                        Debug.Print(DateTime.Now + " - Trying to subscribe but sockets ain't alive!  IR");
                    }
                    break;
                case "BTCM":
                    if (true) {
                        if (wSocket_BTCM.IsAlive) {
                            channel = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\", \"heartbeat\"], \"marketIds\":[";
                            foreach (Tuple<string, string> pair in pairs) {
                                string crypto = pair.Item1;
                                string fiat = pair.Item2;
                                if (crypto == "XBT") crypto = "BTC";
                                //if (crypto == "BCH") crypto = "BCHABC";

                                channel += "\"" + crypto + "-" + fiat + "\", ";
                            }
                            channel = channel.Substring(0, channel.Length - 2) + "]}";
                            Debug.Print("BTCH channel subscription string: " + channel);

                            //pairList = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\"], \"marketIds\":[\"BTC-AUD\"]}";
                            wSocket_BTCM.Send(channel);
                        }
                        else DCEs["BTCM"].socketsReset = true;
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
                            if (crypto == "USDT") {
                                crypto = "UST";
                            }
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
            wSocket_IR.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            wSocket_IR.OnMessage += (sender, e) => {
                if (e.IsText) {
                    //Debug.Print(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss") + " - IR sockets: " + e.Data);
                    DCEs["IR"].socketsAlive = true;
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
                DCEs["IR"].socketsAlive = false;
            };

            wSocket_IR.OnClose += (sender, e) => {
                Debug.Print(DateTime.Now + " IR stream closed... should be preceeded by some ded thingo " + DateTime.Now.ToString());
                DCEs["IR"].socketsAlive = false;
            };

            wSocket_IR.Connect();
        }

        public void IR_Disconnect() {
            wSocket_IR.Close();
            DCEs["IR"].ClearOrderBookSubDicts();
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
                DCEs["BTCM"].socketsAlive = true;
                MessageRX_BTCM(data.ToString());
            });

            socket_BTCM.On(Socket.EVENT_ERROR, (e) => {
                Debug.Print("ws onerror - btcm");
                socket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";
                //DCEs["BTCM"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                WebSocket_Reconnect("BTCM");
            });

            socket_BTCM.On(Socket.EVENT_CONNECT_ERROR, (e) => {
                Debug.Print("ws connection error - btcm");
                socket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket connection error";
                //DCEs["BTCM"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                WebSocket_Reconnect("BTCM");
            });

            socket_BTCM.On(Socket.EVENT_CONNECT_TIMEOUT, (e) => {
                Debug.Print("ws connection timeout - btcm");
                socket_BTCM.Close();
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket timeout";
                //DCEs["BTCM"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                WebSocket_Reconnect("BTCM");
            });

            socket_BTCM.On(Socket.EVENT_DISCONNECT, () => {
                // aww shit
                Debug.Print("BTCM socket disconnected.  reconnecting...");
                DCEs["BTCM"].socketsAlive = false;
                WebSocket_Reconnect("BTCM");
            });
        }

        public void BTCM_Connect_v2() {

            wSocket_BTCM = new WebSocket("wss://socket.btcmarkets.net/v2");
            wSocket_BTCM.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            wSocket_BTCM.OnMessage += (sender, e) => {
                //Debug.Print("!!! bTCMv2 got a message: " + e.Data);
                if (e.IsText) {
                    //Debug.Print(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss") + " - BTCMv2 sockets: " + e.Data);
                    DCEs["BTCM"].socketsAlive = true;
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
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";
                
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                DCEs["BTCM"].socketsReset = true;
            };

            wSocket_BTCM.OnClose += (sender, e) => {
                Debug.Print(DateTime.Now + " BTCMv2 stream closed... should be preceeded by some ded thingo ");
                DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";

                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                // shouldn't actually need to set socketsreset to true here because we already set it in the OnError method, and otherwise why would we close the stream?
                //DCEs["BTCM"].socketsReset = true;
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

            // don't need this anymore because we pull the OB in the subscribe code.
            /*if (dExchange == "IR") {  // only for IR do we need to grab OBs via REST _after_ we have started the socket machine.  This is to reduce missed events.
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
            }*/
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


        public void Init_IR(string pair) {
            DCEs["IR"].pulledSnapShot[pair] = false;
            DCEs["IR"].OBResetFlag["ORDERBOOK-" + pair] = false;
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
            if (message == null) return;
            if (message.Contains("\"Event\":\"Subscriptions\"")) {
                // ignore the subscriptions event.  it breaks parsing too :/
                Debug.Print("IGNORING - " + message);
                return;
            }
            if (message.Contains("Error")) {
                Debug.Print("IR ERROR in websockets: " + message);
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
                Debug.Print(DateTime.Now + " IR - official heartbeat");
                return;
            }
            DCEs["IR"].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat

            Ticker_IR tickerStream = new Ticker_IR();
            tickerStream = JsonConvert.DeserializeObject<Ticker_IR>(message);
            if (tickerStream.Data.Pair.ToUpper().Contains("UST")) tickerStream.Data.Pair = tickerStream.Data.Pair.Replace(tickerStream.Data.Pair.Substring(0, 3), "USDT");
            if (tickerStream.Channel.ToUpper().Contains("-UST-")) tickerStream.Channel = tickerStream.Channel.Replace(tickerStream.Channel.Substring(10, 3), "USDT");

            // still trying to get to the bottom of orders that should be deleted that aren't
            /*if (tickerStream.Data.Pair == "xbt-aud") {
                if (tickerStream.Event == "NewOrder") {
                    //Debug.Print(DateTime.Now + " - NEW ORDA: " + tickerStream.Data.OrderGuid);
                }
                else if (tickerStream.Event == "OrderCanceled") {
                    //Debug.Print(DateTime.Now + " - TO CANCEL: " + tickerStream.Data.OrderGuid);
                }
                else if (tickerStream.Event == "OrderChanged" && tickerStream.Data.Volume == 0) {
                    //Debug.Print(DateTime.Now + " - TO CHANGE to 0: " + tickerStream.Data.OrderGuid);
                }

                if (tickerStream.Data.OrderType.StartsWith("Market")) {
                    //Debug.Print(DateTime.Now + " - TO MARKET! guid: " + tickerStream.Data.OrderGuid + " event: " + tickerStream.Event);
                }
            }*/

            validateNonce(tickerStream);
        }

        // validate nonce will also add to a buffer if we're still yet to properly pull the REST OB. 
        // i think i might try and use the buffer concept every time we have an out of order nonce so it's possible to recover from
        // a few out of order nonces if they're all there, rather than just dumping everything every time.
        public void validateNonce(Ticker_IR tickerStream) {
            string pair = tickerStream.Data.Pair.ToUpper();
            string channel = tickerStream.Channel.ToUpper();
            //Debug.Print("---- Nonce received: " + tickerStream.Nonce);

            // first do orderBuffer stuff
            if (!DCEs["IR"].orderBuffer_IR.ContainsKey(pair)) {  // make sure there exists the dictionary element
                if (!DCEs["IR"].orderBuffer_IR.TryAdd(pair, new ConcurrentDictionary<int, WebSocketsConnect.Ticker_IR>())) {
                    Debug.Print(DateTime.Now + " - can't add orderBuffer_IR concurrent dicsh for " + pair);
                    return;
                }
            }
            if (!DCEs["IR"].pulledSnapShot[pair]) {  // if we haven't even got the OB yet
                DCEs["IR"].orderBuffer_IR[pair][tickerStream.Nonce] = tickerStream;  // add this event to the buffer
                Debug.Print(" ! just added " + tickerStream.Nonce + " to the buf");
                return;  // bail.
            }

            //Debug.Print(DateTime.Now + " - (" + pair + ") adding to buffer.  current nonce: " + DCEs["IR"].channelNonce[channel] + ", nonce we just received: " + tickerStream.Nonce);
            DCEs["IR"].orderBuffer_IR[pair][tickerStream.Nonce] = tickerStream;

            // if we don't got a nonce yet
            if (DCEs["IR"].channelNonce[channel] == 0) {
                // orderBuffer_IR must have at least one order in it, so we should be able to safely request the minimum key.
                DCEs["IR"].channelNonce[channel] = DCEs["IR"].orderBuffer_IR[pair].Keys.Min() - 1;  // find the smallest nonce in the buffer, and set the channel nonce to one below that
            }

            // we should check how full our buffer is. If there's more than 10 items (??) then it's probably too full.
            if ((DCEs["IR"].orderBuffer_IR[pair].Count > 100) || (DCEs["IR"].OBResetFlag[channel])) {
                Debug.Print("NONCE - too many buffered nonces, can't recover " + tickerStream.Channel + ", time to dump and restart");

                if (wSocket_IR.IsAlive) {

                    wSocket_IR.Send("{\"Event\":\"Unsubscribe\",\"Data\":[\"" + tickerStream.Channel + "\"]} ");

                    // now need to dump the OBs. 
                    DCEs["IR"].IR_OBs[pair].Item1.Clear();
                    DCEs["IR"].IR_OBs[pair].Item2.Clear();
                    DCEs["IR"].orderBuffer_IR[pair].Clear();

                    Init_IR(pair);  // only reset it once the OBs are clear

                    // now subscribe back to the channel
                    Tuple<string, string> pairTup = Utilities.SplitPair(pair);
                    List<Tuple<string, string>> tempList = new List<Tuple<string, string>>();
                    tempList.Add(new Tuple<string, string>(pairTup.Item1, pairTup.Item2));
                    WebSocket_Subscribe("IR", tempList);
                    return;
                }
                else DCEs["IR"].socketsReset = true;
                return;
            }

            // always want to try and process the buffer, maybe the next nonce is in there.
            //return;  // no need to interpret the rest of the event, we've either pushed onto the buffer, or we're starting fresh.

            //else DCEs["IR"].nonceErrorTracker[channel] = false;  // if this is false and we're setting it again to false, fine - normal operation.  If this was true and we're now setting it to false, this means that we had some nonce errors but they seem to have settled down, and we can now dump and reconnect


            // commented this out because I'm now trying to buffer out of order nonces, so the buffer process should be the only one to update the nonce
            //DCEs["IR"].channelNonce[channel] = tickerStream.Nonce;  // regardless of whether it was in sequence, update it.



            // have removed this because i think we should be prescriptive about getting the order book.  If we just always get it when it's not there
            // then we might grab it 5 times in one second
            /*if (!DCEs["IR"].IR_OBs.ContainsKey(tickerStream.Data.Pair.ToUpper())) {
                Debug.Print(DateTime.Now.ToString() + " IR - receieved an event we don't have a pair for (" + tickerStream.Data.Pair + "), will grab it");

                Tuple<string, string> tempTup = Utilities.SplitPair(tickerStream.Data.Pair.ToUpper());

                DCEs["IR"].GetIROrderBook(tempTup.Item1, tempTup.Item2);
                //WebSocket_Subscribe("IR", tempTup.Item1, tempTup.Item2);
                //return;
            }*/

            // commented the below out, maybe we have all the right nonces in the buffer.  let's just process the buffer regardless.
            // to get here we must have a good nonce
            //DCEs["IR"].channelNonce[channel] = tickerStream.Nonce; 
            //parseTicker_IR(tickerStream);

            // OK let's check if the buffer has some more events to add
            //Debug.Print(DateTime.Now + " - starting buffer loop, " + DCEs["IR"].orderBuffer_IR[pair].Count + " events buffered");
            while (DCEs["IR"].orderBuffer_IR[pair].ContainsKey(DCEs["IR"].channelNonce[channel] + 1)) {  // if the buffer has the next nonce...
                DCEs["IR"].channelNonce[channel]++;  // cool, let's advance the nonce
                if (DCEs["IR"].orderBuffer_IR[pair].TryRemove(DCEs["IR"].channelNonce[channel], out Ticker_IR ticker)) {  // pop the ticker object,
                    //Debug.Print(DateTime.Now + " - (" + pair + ") parsing nonce " + ticker.Nonce + " from buffer, there are " + (DCEs["IR"].orderBuffer_IR[pair].Count) + " other buffered events in there");
                    parseTicker_IR(ticker);  // and parse it
                }
                else {
                    Debug.Print(DateTime.Now + " - can't pop ticker object from buffer?? channel: " + channel);
                    DCEs["IR"].OBResetFlag[channel] = true;  // let's start again.
                    DCEs["IR"].channelNonce[channel]--;  // make sure it fails
                    return;
                }
            }
        }
        

        // event is clean, correct nonce etc, lets parse it.
        public void parseTicker_IR(Ticker_IR tickerStream) { 

            if (!tickerStream.Data.OrderType.StartsWith("Limit")) {
                Debug.Print(DateTime.Now + " - ignoring a " + tickerStream.Data.OrderType + " order.  event: " + tickerStream.Event);
                return;  // ignore market orders
            }


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
                case "OrderChanged":
                case "OrderCanceled":

                    /*Debug.Print("IR new: " + message);
                    break;*/

                    // this if block is pure debugging
                    /*if (((tickerStream.Event == "OrderChanged" && tickerStream.Data.Volume == 0) || (tickerStream.Event == "OrderCanceled")) && tickerStream.Data.Pair.ToUpper() == "PLA-AUD") {
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
                    }*/


                    // if this OrderBookEvent_IR function returns a legit MarketSummary obj, it means the event we just received made changes to the spread.  let's update the UI.
                    // if it returns null, then there was no spread change.
                    // this method also updates the OBs and cryptoPairs obj (cryptoPairs only if there was a spread change)
                    DCE.MarketSummary mSummary = DCEs["IR"].OrderBookEvent_IR(tickerStream);
                    if (mSummary != null) {
                        //Debug.Print("spread changing event: " + message);
                        if (mSummary.spread < 0) {
                            Debug.Print(DateTime.Now + " IR spread (" + tickerStream.Data.Pair + ") is " + mSummary.spread + " :(  bid: " + mSummary.CurrentHighestBidPrice + " and offer: " + mSummary.CurrentLowestOfferPrice);
                        }
                        if (DCEs["IR"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode) {  //eventPair.Item2.ToUpper()) {
                            pollingThread.ReportProgress(21, mSummary);  // do update_pairs thing
                            //pollingThread.ReportProgress(25, mSummary);  // update the OBView thingo
                        }
                        else {
                            //pollingThread.ReportProgress(25, mSummary);  // update the OBView thingo
                        }
                        if (tickerStream.Data.Pair.ToUpper() == "XBT-AUD") {
                            pollingThread.ReportProgress(25, mSummary);  // update the OBView thingo
                        }
                    }

                    break;
            }
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
                DCEs["BTCM"].socketsAlive = true;
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
                        if (mSummary.PrimaryCurrencyCode.ToUpper() == "UST") mSummary.PrimaryCurrencyCode = "USDT";
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
