using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WebSocketSharp;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections.Concurrent;
using Websocket.Client;

namespace IRTicker {
    public class WebSocketsConnect {

        private Dictionary<string, DCE> DCEs;
        private WebSocket wSocket_BFX, wSocket_GDAX, /*wSocket_IR,*/ wSocket_BTCM;
        private WebsocketClient client_IR;
        public Dictionary<string, Subscribed_BFX> channel_Dict_BFX = new Dictionary<string, Subscribed_BFX>();  // string is a string version of the channel ID
        private BackgroundWorker pollingThread;
        private Thread UITimerThread;
        private bool UITimerThreadProceed = true;
        private ManualResetEvent startSocket_exitEvent = new ManualResetEvent(false);
        private string IRSocketsURL = "wss://websockets.independentreserve.com";
        //private string IRSocketsURL = "ws://dev.pushservice.independentreserve.net";
        private PrivateIR pIR;

        // constructor
        public WebSocketsConnect(Dictionary<string, DCE> _DCEs, BackgroundWorker _pollingThread, PrivateIR _pIR) {
            DCEs = _DCEs;
            pollingThread = _pollingThread;
            pIR = _pIR;

            // IR

            Debug.Print("IR websocket connecting..");

            Task.Factory.StartNew(() => {
                startSockets("IR", IRSocketsURL);
            })
            ;
            Debug.Print("after first start sockets");
            //subscribe_unsubscribe_new("IR", true);  // subscribe to all the things


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
                //wSocket_BFX.Close();
                DCEs["BFX"].NetworkAvailable = false;
                DCEs["BFX"].socketsAlive = false;
                DCEs["BFX"].socketsReset = true;
                DCEs["BFX"].CurrentDCEStatus = "Socket error";
                DCEs["BFX"].HasStaticData = false;
                pollingThread.ReportProgress(12, "BFX");  // 12 is error
                //WebSocket_Reconnect("BFX");
            };

            wSocket_BFX.OnClose += (sender, e) => {
                Debug.Print("BFX stream closed... should be preceeded by some ded thingo " + DateTime.Now.ToString());
                
               // trying to comment this out in the hope that it will stop the reconnect loop we get into after hibernation
                /*DCEs["BFX"].socketsAlive = false;
                DCEs["BFX"].socketsReset = true;
                DCEs["BFX"].CurrentDCEStatus = "Socket error";
                pollingThread.ReportProgress(12, "BFX");  // 12 is error*/
                //WebSocket_Reconnect("BFX");
            }; 
            wSocket_BFX.Connect();


            // GDAX
            wSocket_GDAX = new WebSocket("wss://ws-feed.pro.coinbase.com");
            wSocket_GDAX.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

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
                //wSocket_GDAX.Close();
                DCEs["GDAX"].NetworkAvailable = false;
                DCEs["GDAX"].socketsAlive = false;
                DCEs["GDAX"].socketsReset = true;
                DCEs["GDAX"].CurrentDCEStatus = "Socket error";
                DCEs["GDAX"].HasStaticData = false;
                pollingThread.ReportProgress(12, "GDAX");  // 12 is error
                //WebSocket_Reconnect("GDAX");
            };

            wSocket_GDAX.OnClose += (sender, e) => {
                Debug.Print("GDAX stream was closed.. should be because we disconnected on purpose. preceeded by ded?  " + DateTime.Now.ToString());
                // trying to comment this out in the hope that it will stop the reconnect loop we get into after hibernation
                /*DCEs["GDAX"].socketsAlive = false;
                DCEs["GDAX"].socketsReset = true;
                DCEs["GDAX"].CurrentDCEStatus = "Socket error";
                pollingThread.ReportProgress(12, "GDAX");  // 12 is error*/
                //WebSocket_Disconnect("GDAX");
            };
            wSocket_GDAX.Connect();
        }

        public void GetOrderBook_IR(string crypto, string fiat) {
            if (crypto == "USDT") crypto = "UST";
            string pair = crypto + "-" + fiat;
            Tuple<bool, string> orderBookTpl = Utilities.Get(DCEs["IR"].BaseURL + "/Public/GetAllOrders?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);

            // have to change back ughhh
            if (crypto.ToUpper() == "UST") crypto = "USDT";
            pair = crypto + "-" + fiat;

            if (orderBookTpl.Item1) {
                DCE.OrderBook orderBook = JsonConvert.DeserializeObject<DCE.OrderBook>(orderBookTpl.Item2);
                if (orderBook.PrimaryCurrencyCode.ToUpper() == "UST") orderBook.PrimaryCurrencyCode = "USDT";
                DCEs["IR"].orderBooks[pair] = orderBook;
                if ((DCEs["IR"].orderBooks[pair].BuyOrders.Count == 0) || (DCEs["IR"].orderBooks[pair].SellOrders.Count == 0)) {
                    Debug.Print("One of the order books is empty... not continuing.  number of buy orders: " + DCEs["IR"].orderBooks[pair].BuyOrders.Count + ", and sell orders: " + DCEs["IR"].orderBooks[pair].SellOrders.Count);
                    return;
                }

                // next we need to convert this orderbook into a concurrent dictionary of OrderBook_IR objects
                // so yeah.. the "orderBook" object doesn't really get used anymore.  it's just like a staging area
                DCEs["IR"].ConvertOrderBook_IR(pair);

                Debug.Print(DateTime.Now.ToString() + " IR OB " + pair + " pulled, " + (DCEs["IR"].orderBuffer_IR.ContainsKey(pair) ? DCEs["IR"].orderBuffer_IR[pair].Count.ToString() : "n/a") + " ordes in the buffer");

                //int remainingBuffer = ApplyBuffer_IR(pair);
                //Print("(" + pair + ") Buffer applied, there are " + remainingBuffer + " left in the buffer (should be 0)");
                DCEs["IR"].pulledSnapShot[pair] = true;
            }
            else {
                Debug.Print(DateTime.Now.ToString() + " | IR - couldn't download REST OB? - " + pair);
            }
        }

        // this isn't used anymore
        public void WebSocket_Subscribe_old(string dExchange, List<Tuple<string, string>> pairs) {
            string channel = "";
            switch (dExchange) {
                case "IR":

                    channel = "{\"Event\":\"Subscribe\",\"Data\":[";
                    foreach (Tuple<string, string> pair in pairs) {
                        string crypto = pair.Item1;
                        string fiat = pair.Item2;
                        DCEs["IR"].pulledSnapShot[crypto + "-" + fiat] = false;  // initialise the pulledSnapShot variable for this pair
                        DCEs["IR"].positiveSpread[crypto + "-" + fiat] = true;  // initialise the positiveSpread variable for this pair, always assume the spread is positive
                        DCEs["IR"].negSpreadCount[crypto + "-" + fiat] = 0;  // init
                        if (crypto == "USDT") crypto = "UST";
                        channel += "\"orderbook-" + crypto + "-" + fiat + "\", ";
                        if (crypto == "UST") crypto = "USDT";
                        DCEs[dExchange].channelNonce[("ORDERBOOK-" + crypto + "-" + fiat)] = 0;  // initialise the nonce dictionary
                        DCEs[dExchange].OBResetFlag["ORDERBOOK-" + crypto + "-" + fiat] = false;  // false means no error, no need to dump OB

                        // initialise orderbuffers
                        if (!DCEs["IR"].orderBuffer_IR.ContainsKey(crypto + "-" + fiat)) {  // make the dictionary element doesn't already exist
                            if (!DCEs["IR"].orderBuffer_IR.TryAdd(crypto + "-" + fiat, new ConcurrentDictionary<int, WebSocketsConnect.Ticker_IR>())) {
                                Debug.Print(DateTime.Now + " - can't add orderBuffer_IR concurrent dicsh for " + crypto + "-" + fiat);
                                return;
                            }
                        }

                        
                    }
                    channel += "]} ";
                    Debug.Print("IR websocket subscribe: " + channel);
                    //wSocket_IR.Send(channel);

                    //startSockets("IR", IRSocketsURL, channel);


                    foreach (Tuple<string, string> pair in pairs) {
                        GetOrderBook_IR(pair.Item1, pair.Item2);
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

        public void subscribe_unsubscribe_new(string dExchange, bool subscribe, string crypto = "none", string fiat = "none") {
            if (fiat == "none") fiat = DCEs[dExchange].CurrentSecondaryCurrency;
            Debug.Print("subscribe_unsubscribe! -- " + dExchange + " -- did we subscribe: " + subscribe.ToString() + ", pair: " + crypto + "-" + fiat);
            string channel = "";
            List<string> pairs = new List<string>();
            switch (dExchange) {
                case "IR":
                    channel = subscribe ? "{\"Event\":\"Subscribe\",\"Data\":[" : "{\"Event\":\"Unsubscribe\",\"Data\":[";
                    if (crypto == "none") {  // unsubscribe or subscribe to EVERYTHING
                        //stopSockets("IR");  // don't want to stop everything, we just need to unsubscribe like we said we would.
                        //List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
                        foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                            if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + fiat)) {
                                string crypto1 = primaryCode;
                                if (crypto1 == "USDT") crypto1 = "UST";
                                channel += "\"orderbook-" + crypto1.ToLower() + "-" + fiat.ToLower() + "\", ";
                            }
                        }

                        channel += "]} ";
                    }
                    else {  // or just one pair
                        if (crypto.ToUpper() == "USDT") crypto = "ust";
                        channel += "\"orderbook-" + crypto.ToLower() + "-" + fiat.ToLower() + "\"]}";
                    }
                    Debug.Print("IR websocket subcribe/unsubscribe - " + (subscribe ? "subscribe" : "unsubscribe") + " event: " + channel);

                    if (client_IR.IsRunning) {
                        Task.Run(() => client_IR.Send(channel));
                    }
                    else {
                        Debug.Print(DateTime.Now + " - IR sockets down when trying to " + (subscribe ? "subscribe" : "unsubscribe"));
                        DCEs["IR"].socketsReset = true;
                        break;
                    }
                    if (subscribe) {  // if subscribing then grab the order books too.
                        if (crypto == "none") {
                            //List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
                            foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                                if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + fiat)) {
                                    //pairList.Add(new Tuple<string, string>("XBT", "AUD"));
                                    //pairList.Add(new Tuple<string, string>("XBT", "USD"));
                                    //pairList.Add(new Tuple<string, string>("XBT", "NZD"));
                                    //pairList.Add(new Tuple<string, string>(primaryCode, fiat));
                                    GetOrderBook_IR(primaryCode, fiat);
                                }
                            }

                            // wtf why did i have a separate loop for this
                            //foreach (Tuple<string, string> pair1 in pairList) {
                            //    GetOrderBook_IR(pair1.Item1, pair1.Item2);
                            //}
                        }
                        else {
                            GetOrderBook_IR(crypto, fiat);
                        }
                    }

                    break;

                case "BTCM":
                    Debug.Print("trying to subscribe to BTCM");
                    if (crypto == "none") {
                        foreach (string primarycode in DCEs[dExchange].PrimaryCurrencyList) {
                            pairs.Add(primarycode);
                        }
                    }
                    else pairs.Add(crypto);

                    if (wSocket_BTCM.IsAlive) {
                        channel = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\", \"heartbeat\"], \"marketIds\":[";  // we only ever subscribe, no scenario where we need to unsubscribe.  Unsubscribing is a pain for BTCM, see here https://api.btcmarkets.net/doc/v3#tag/WS_Overview
                        foreach (string crypto1 in pairs) {
                            string crypto2 = crypto1;
                            if (crypto2 == "XBT") crypto2 = "BTC";

                            channel += "\"" + crypto2 + "-" + fiat + "\", ";
                        }
                        channel = channel.Substring(0, channel.Length - 2) + "]}";
                        Debug.Print("BTCHH channel subscription string: " + channel);

                        //pairList = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\"], \"marketIds\":[\"BTC-AUD\"]}";
                        wSocket_BTCM.Send(channel);
                    }
                    else DCEs[dExchange].socketsReset = true;

                    break;

                case "BFX":

                    if (!subscribe) {  // there is no unsubscribing in BFX, so we just close it down for resubscription.  better hope we don't try and do it for 1 crypto
                        wSocket_BFX.Close();
                        wSocket_BFX.Connect();
                    }
                    else {
                        if (crypto == "none") {
                            foreach (string primarycode in DCEs[dExchange].PrimaryCurrencyList) {
                                pairs.Add(primarycode);
                            }
                        }
                        else pairs.Add(crypto);

                        if (wSocket_BFX.IsAlive) {
                            foreach (string crypto1 in pairs) {
                                string crypto2 = crypto1;
                                if (crypto2 == "XBT") crypto2 = "BTC";
                                if (crypto2 == "BCH") crypto2 = "BAB";
                                if (crypto2 == "USDT") crypto2 = "UST";
                                if (!DCEs[dExchange].ExchangeProducts.ContainsKey(crypto1 + "-" + fiat)) continue;  // only try to subscribe to pairs that this BFX supports
                                channel = "{\"event\":\"subscribe\", \"channel\":\"ticker\", \"pair\":\"" + crypto2 + fiat + "\"}";
                                Debug.Print("BFX subscribing to: " + channel);
                                wSocket_BFX.Send(channel);
                            }
                        }
                        else {
                            Debug.Print(DateTime.Now + " - BFX - trying to " + (subscribe ? "subscribe" : "unsubscribe" + " from channel(s) but BFX websockets is dead"));
                            DCEs[dExchange].socketsReset = true;
                        }
                    }

                    break;

                case "GDAX":

                    if (crypto == "none") {
                        foreach (string primarycode in DCEs[dExchange].PrimaryCurrencyList) {
                            pairs.Add(primarycode);
                        }
                    }
                    else pairs.Add(crypto);

                    if (wSocket_GDAX.IsAlive) {
                        channel = "{\"type\": \"" + (subscribe ? "subscribe" : "unsubscribe") + "\", \"channels\": [{\"name\": \"ticker\", \"product_ids\":[";
                        foreach (string crypto1 in pairs) {
                            if (!DCEs[dExchange].ExchangeProducts.ContainsKey(crypto1 + "-" + fiat)) continue;  // only try to subscribe to pairs that this BFX supports
                            string crypto2 = crypto1;
                            if (crypto2 == "XBT") crypto2 = "BTC";
                            channel += "\"" + crypto2 + "-" + fiat + "\",";
                        }

                        channel = channel.Substring(0, channel.Length - 1);
                        channel += "] } ] }";
                        Debug.Print("GDAX subscribing to: " + channel);
                        wSocket_GDAX.Send(channel);
                    }
                    else {
                        Debug.Print(DateTime.Now + " - GDAX - trying to " + (subscribe ? "subscribe" : "unsubscribe" + " from channel(s) but GDAX websockets is dead"));
                        DCEs[dExchange].socketsReset = true;
                    }


                    break;

                default:
                    Debug.Print(" ------ whoops, subscribe_unsubscribe_enw doesn't support this exchange: " + dExchange);
                    break;
            }
        }

        private void stopSockets(string dExchange, string pair = "none") {
            client_IR.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "byee");
            DCEs[dExchange].socketsAlive = false;
            startSocket_exitEvent.Set();  // hopefully this should let the existing startSockets() sub complete
            Debug.Print("IR sockets stop command sent");
        }

        private void startSockets(string dExchange, string socketsURL) {
            var url = new Uri(socketsURL);
            DCEs[dExchange].socketsAlive = false;
            Debug.Print(DateTime.Now + " - startSockets called for " + dExchange);

            //using (client_IR = new WebsocketClient(url)) {  getting rid of using statement..
            client_IR = new WebsocketClient(url);
                client_IR.ReconnectTimeout = TimeSpan.FromSeconds(70);
                client_IR.ReconnectionHappened.Subscribe(info => {
                    if (info.Type == ReconnectionType.Initial) {
                        Debug.Print("Initial 'reconnection', ignored");
                        DCEs[dExchange].socketsAlive = true;
                        DCEs[dExchange].socketsReset = false;
                    }
                    /*else if (info.Type == ReconnectionType.Lost) {
                        Debug.Print("Lost 'reconnection' ignored, attached to a Reset button click?");
                    }*/
                    else { 
                        Debug.Print(DateTime.Now + " - (" + dExchange + " reconnection) - clearing OB sub dicts...");
                        DCEs[dExchange].socketsAlive = false;
                        DCEs["IR"].CurrentDCEStatus = "Reconnected";
                        /*DCEs[dExchange].ClearOrderBookSubDicts();
                        Debug.Print("creating a new buffer dict...");
                        DCEs[dExchange].orderBuffer_IR = new ConcurrentDictionary<string, ConcurrentDictionary<int, Ticker_IR>>();
                        Debug.Print("setting the pulledSnapShot dict entries to all false and initialising the orderbuffer dicts...");
                        List<string> tempPairs = new List<string>();
                        tempPairs.Add("XBT-AUD");
                        tempPairs.Add("XBT-USD");
                        tempPairs.Add("XBT-NZD");
                        /*foreach(string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {  // now set all pulled OB flags to false
                            foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {*/
                        /*foreach (string pair in tempPairs) {
                            if (DCEs[dExchange].ExchangeProducts.ContainsKey(pair)) {
                                DCEs[dExchange].pulledSnapShot[pair] = false;
                            }
                            // initialise orderbuffers
                            if (!DCEs["IR"].orderBuffer_IR.ContainsKey(pair)) {  // make the dictionary element doesn't already exist
                                if (!DCEs["IR"].orderBuffer_IR.TryAdd(pair, new ConcurrentDictionary<int, WebSocketsConnect.Ticker_IR>())) {
                                    Debug.Print(DateTime.Now + " - can't add orderBuffer_IR concurrent dicsh for " + pair);
                                    return;
                                }
                            }
                            //}
                        }*/
                        Reinit_sockets(dExchange);
                        Debug.Print($"Reconnection happened, type: {info.Type}, resubscribing...");
                        subscribe_unsubscribe_new(dExchange, true);  // resubscriibe to all pairs
                        // commented out the below because 1. use subscribe_unsubscribe_new() instead of re-writing code, and 2. stoping and starting the timer again seems pointless
                        /*Task.Run(() => client_IR.Send(subscribeStr));
                        Debug.Print("Pulling the REST OBs...");
                        foreach (string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {  // now set all pulled OB flags to false
                            foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                                if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + secondaryCode)) {
                                    //GetOrderBook_IR("XBT", "AUD");
                                    //GetOrderBook_IR("XBT", "USD");
                                    //GetOrderBook_IR("XBT", "NZD");
                                    GetOrderBook_IR(primaryCode, secondaryCode);
                                }
                            }
                        }

                    // why do we stop and start the timer here?  the socket is already up and running.. i think this is dumb.
                        stopUITimerThread();

                        Debug.Print(DateTime.Now + " - RECONNECT: about to start the UI timer!");
                        UITimerThreadProceed = true;
                        UITimerThread = new Thread(new ThreadStart(updateUITimer));
                        // this command to start the thread
                        UITimerThread.Start();
                        Debug.Print("RECONNECT: UI timer storted.");*/
                    }

                });

                client_IR.MessageReceived.Subscribe(msg => {
                    switch (dExchange) {
                        case "IR":
                            MessageRX_IR(msg.Text);
                            break;
                    }
                });

                client_IR.Start();

                Debug.Print(DateTime.Now + " - about to start the UI timer!");
                                
                UITimerThread = new Thread(new ThreadStart(updateUITimer));
                // this command to start the thread
                UITimerThread.Start();
                Debug.Print("UI timer storted.");
                //await Task.Run(() => client_IR.Send("1"));
                //Debug.Print(DateTime.Now + " - we have moved on after the client_IR.send where we subscribe!");

                startSocket_exitEvent.WaitOne();
            //}  trying to remove the using statement
        }

        // shuts down the UITimerThread.  Only called when the app is terminating.
        public void stopUITimerThread() {
            if (UITimerThread != null && UITimerThread.IsAlive) UITimerThread.Abort();
        }


        private void updateUITimer() {

            while (UITimerThreadProceed) {
                foreach (KeyValuePair<string, ConcurrentDictionary<int, Ticker_IR>> pair in DCEs["IR"].orderBuffer_IR) {
                    if (DCEs["IR"].newOrders[pair.Key] > 0) {
                        if (pIR != null) Task.Run(() => pIR.compileAccountOrderBookAsync(pair.Key));  // hopefully this will just run this method asynchronously
                        //pollingThread.ReportProgress(20, pair.Key);  // this will tell the accounts panel to update it's OB view
                        if ((DCEs["IR"].orderBuffer_IR[pair.Key].Count > 0) && DCEs["IR"].pulledSnapShot[pair.Key]) applyBufferToOB(pair.Key);
                        DCEs["IR"].newOrders[pair.Key] = 0;
                    }
                }

                Thread.Sleep(Properties.Settings.Default.UITimerFreq);
            }
            Debug.Print("UITimer thread while loop completed...");
        }

        public void IR_Disconnect() {
            //UITimerThreadProceed = false;  don't think we actually need to stop this running ever..
            if (client_IR.IsRunning) {
                Debug.Print(DateTime.Now + " - IR_Disconnect sub: IR running, will stop");
                stopSockets("IR");
            }
            DCEs["IR"].ClearOrderBookSubDicts();
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
                //DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";
                
                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
                DCEs["BTCM"].socketsReset = true;
            };

            wSocket_BTCM.OnClose += (sender, e) => {
                Debug.Print(DateTime.Now + " BTCMv2 stream closed... should be preceeded by some ded thingo ");
                //DCEs["BTCM"].NetworkAvailable = false;
                DCEs["BTCM"].socketsAlive = false;
                DCEs["BTCM"].socketsReset = true;
                DCEs["BTCM"].CurrentDCEStatus = "Socket error";

                pollingThread.ReportProgress(12, "BTCM");  // 12 is error
            };

            wSocket_BTCM.Connect();
        }

        /// <summary>
        /// UNSUBSCRIBes, re-initialises the dictionaries, then resubscribes from pairs.  if required will do all pairs or just a specific pair
        /// </summary>
        /// <param name="dExchange">exchange</param>
        /// <param name="crypto">crypto we're reinitialising, "none" to do all the cryptos</param>
        /// <param name="oldFiat">if we're unsubscribing from one fiat and re-subscribing to a new one, this and newFiat need to be populated</param>
        /// <param name="newFiat">as above, if we're moving from one fiat to another this needs to be populated along with oldFiat</param>
        public void WebSocket_Resubscribe(string dExchange, string crypto = "none", string oldFiat = "none", string newFiat = "none") {

            if ((oldFiat == "none" && newFiat != "none") || (oldFiat != "none" && newFiat == "none")) {
                Debug.Print("WebSocket_Resubscribe has been called in an illegal state - both oldfiat (" + oldFiat + ") and  newFiat (" + newFiat + ") need to both be 'none', or both be cryptos.  One can't be 'none' and the other note");
                return;
            }
            subscribe_unsubscribe_new(dExchange, false, crypto, (oldFiat == "none" ? DCEs[dExchange].CurrentSecondaryCurrency : oldFiat));
            Reinit_sockets(dExchange, crypto, newFiat);
            subscribe_unsubscribe_new(dExchange, true, crypto, (newFiat == "none" ? DCEs[dExchange].CurrentSecondaryCurrency : newFiat));
        }


        public void WebSocket_Reconnect(string dExchange) {
            Debug.Print("WebSocket_Reconnect for " + dExchange);
            if (!DCEs[dExchange].HasStaticData) {
                Debug.Print("Trying to reconnect to " + dExchange + " but there's no static data.  will not.");
                return;
            }
            switch (dExchange) { 
                case "IR":  // this should never be called because the IR sockets should automatically recover
                    //if (!client_IR.IsRunning) {
                        Debug.Print("WebSocket_Reconnect: IR?? this shouldn't be called?  shouldn't it auto-reconnect?");
                        if (client_IR.IsRunning) {
                            Debug.Print(DateTime.Now + " - IR running, will stop");
                            stopSockets("IR");  
                        }
                        stopUITimerThread();  // if it hasn't stopped by now, we force it.

                        Reinit_sockets("IR");
                        startSockets("IR", IRSocketsURL);
                        //IR_Connect();  // create all the sockets stuff again from scratch :/
                        DCEs["IR"].HeartBeat = DateTime.Now;
                    //}
                    break;
                case "BTCM":
                    wSocket_BTCM.Close();
                    wSocket_BTCM.Connect();
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
            DCEs[dExchange].socketsReset = false;  // when closing the stream, the OnClose method is called, which sets the socketsReset to true.  need to turn this off so we don't reconnect forever

            //re-subscribe?
            Debug.Print(dExchange + " - re-subscribing to all pairs...");

            // don't do it this way anymore
            /*List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
            if (true ) { //dExchange == "IR") {
                //pairList.Add(new Tuple<string, string>("XBT", "AUD"));
                //pairList.Add(new Tuple<string, string>("XBT", "USD"));
                //pairList.Add(new Tuple<string, string>("XBT", "NZD"));
                foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                    if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + DCEs[dExchange].CurrentSecondaryCurrency)) {
                        pairList.Add(new Tuple<string, string>(primaryCode, DCEs[dExchange].CurrentSecondaryCurrency));
                    }
                }
            }
            else {
                foreach (string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {
                    foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                        if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + secondaryCode)) {
                            pairList.Add(new Tuple<string, string>(primaryCode, secondaryCode));
                        }
                    }
                }
            }
            WebSocket_Subscribe(dExchange, pairList);*/
            subscribe_unsubscribe_new(dExchange, true);  // subscribe to all 
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

        // only actually called for reconnections
        public void Reinit_sockets(string dExchange, string crypto = "none", string fiat = "none") {
            if (fiat == "none") fiat = DCEs[dExchange].CurrentSecondaryCurrency;
            if (crypto == "none") {
                foreach (string crypto1 in DCEs[dExchange].PrimaryCurrencyList) {
                    DCEs[dExchange].pulledSnapShot[crypto1 + "-" + fiat] = false;
                    DCEs[dExchange].positiveSpread[crypto1 + "-" + fiat] = true;
                    DCEs[dExchange].OBResetFlag["ORDERBOOK-" + crypto1 + "-" + fiat] = false;
                    DCEs[dExchange].channelNonce["ORDERBOOK-" + crypto1 + "-" + fiat] = 0;
                    DCEs[dExchange].newOrders[crypto1 + "-" + fiat] = 0;
                    if (DCEs[dExchange].orderBuffer_IR.ContainsKey(crypto1 + "-" + fiat)) DCEs[dExchange].orderBuffer_IR[crypto1 + "-" + fiat].Clear();
                    else DCEs[dExchange].orderBuffer_IR[crypto1 + "-" + fiat] = new ConcurrentDictionary<int, Ticker_IR>();
                }
                
                DCEs[dExchange].ClearOrderBookSubDicts("none", fiat);
            }
            else {
                DCEs[dExchange].pulledSnapShot[crypto + "-" + fiat] = false;
                DCEs[dExchange].positiveSpread[crypto + "-" + fiat] = true;
                DCEs[dExchange].OBResetFlag["ORDERBOOK-" + crypto + "-" + fiat] = false;
                DCEs[dExchange].channelNonce["ORDERBOOK-" + crypto + "-" + fiat] = 0;
                DCEs[dExchange].ClearOrderBookSubDicts(crypto, fiat);
                DCEs[dExchange].newOrders[crypto + "-" + fiat] = 0;
                if (DCEs[dExchange].orderBuffer_IR.ContainsKey(crypto + "-" + fiat)) DCEs[dExchange].orderBuffer_IR[crypto + "-" + fiat].Clear();
                else DCEs[dExchange].orderBuffer_IR[crypto + "-" + fiat] = new ConcurrentDictionary<int, Ticker_IR>();
            }
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
            //Debug.Print("IR MSG ---- " + message);
            DCEs["IR"].socketsAlive = true;
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
            DCEs["IR"].CurrentDCEStatus = "Online";


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

            addToBuffer(tickerStream);
        }

        // validate nonce will also add to a buffer if we're still yet to properly pull the REST OB. 
        // i think i might try and use the buffer concept every time we have an out of order nonce so it's possible to recover from
        // a few out of order nonces if they're all there, rather than just dumping everything every time.
        public void addToBuffer(Ticker_IR tickerStream) {
            string pair = tickerStream.Data.Pair.ToUpper();
            //string channel = tickerStream.Channel.ToUpper();
            //Debug.Print("---- Nonce received: " + tickerStream.Nonce);

            if (!DCEs["IR"].pulledSnapShot[pair]) {  // if we haven't even got the OB yet
                DCEs["IR"].orderBuffer_IR[pair][tickerStream.Nonce] = tickerStream;  // add this event to the buffer
                //Debug.Print(" ! just added " + tickerStream.Nonce + " to the buf");
                return;  // bail.
            }

            //Debug.Print(DateTime.Now + " - (" + pair + ") adding to buffer.  current nonce: " + DCEs["IR"].channelNonce[channel] + ", nonce we just received: " + tickerStream.Nonce);
            DCEs["IR"].orderBuffer_IR[pair][tickerStream.Nonce] = tickerStream;
            DCEs["IR"].newOrders[pair]++;

        }

        // this sub does some nonce maintenance and then spins through the orderbookBuffer, applying sequential buffered events
        private void applyBufferToOB(string pair) {

            string channel = "ORDERBOOK-" + pair;

            // if we don't got a nonce yet
            if (DCEs["IR"].channelNonce[channel] == 0) {
                // orderBuffer_IR must have at least one order in it, so we should be able to safely request the minimum key.
                DCEs["IR"].channelNonce[channel] = DCEs["IR"].orderBuffer_IR[pair].Keys.Min() - 1;  // find the smallest nonce in the buffer, and set the channel nonce to one below that
                //Debug.Print("just set the Nonce to 1 before the first we got, it is: " + DCEs["IR"].channelNonce[channel]);
            }

            while (DCEs["IR"].orderBuffer_IR[pair].ContainsKey(DCEs["IR"].channelNonce[channel] + 1)) {  // if the buffer has the next nonce...
                DCEs["IR"].channelNonce[channel]++;  // cool, let's advance the nonce
                if (DCEs["IR"].orderBuffer_IR[pair].TryRemove(DCEs["IR"].channelNonce[channel], out Ticker_IR ticker)) {  // pop the ticker object,
                    //if (pair == "XBT-AUD") Debug.Print(DateTime.Now + " - (" + pair + ") parsing nonce " + ticker.Nonce + " from buffer, there are " + (DCEs["IR"].orderBuffer_IR[pair].Count) + " other buffered events in there");
                    parseTicker_IR(ticker);  // and parse it
                }
                else {
                    Debug.Print(DateTime.Now + " - can't pop ticker object from buffer?? channel: " + channel);
                    DCEs["IR"].OBResetFlag[channel] = true;  // let's start again.
                    DCEs["IR"].channelNonce[channel]--;  // make sure it fails
                    return;
                }
            }
            if (DCEs["IR"].orderBuffer_IR[pair].Count > 0) {
                /*if (pair == "XBT-AUD")*/ Debug.Print("(" + pair + ") ooo nonce - " + DCEs["IR"].orderBuffer_IR[pair].Count + " if only 1, it is: " + (DCEs["IR"].orderBuffer_IR[pair].Count == 1 ? DCEs["IR"].orderBuffer_IR[pair].Keys.FirstOrDefault().ToString() : "") + " and the current nonce is " + DCEs["IR"].channelNonce[channel]);
                pollingThread.ReportProgress(27, new Tuple<bool, string>(true, Utilities.SplitPair(pair).Item1));  // update pair text colour to gray
            }

            // we should check how full our buffer is. If there's more than x items (??) then it's probably too full.
            if (((DCEs["IR"].orderBuffer_IR[pair].Count > 5) && DCEs["IR"].pulledSnapShot[pair]) || (DCEs["IR"].OBResetFlag[channel])) {
                Debug.Print("NONCE - too many buffered nonces, can't recover " + channel + ", or the OBResetFlag is true, time to dump and restart");

                if (Properties.Settings.Default.FlashForm) pollingThread.ReportProgress(26);  // flash the window if the setting is enabled

                // now subscribe back to the channel
                WebSocket_Resubscribe("IR", Utilities.SplitPair(pair).Item1);
            }
        }
        

        // event is clean, correct nonce etc, lets parse it.
        public void parseTicker_IR(Ticker_IR tickerStream) { 

            if (!tickerStream.Data.OrderType.StartsWith("Limit")) {
                Debug.Print(DateTime.Now + " - (" + tickerStream.Channel + ") ignoring a " + tickerStream.Data.OrderType + " order.  event: " + tickerStream.Event);
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
                        }

                        if (Properties.Settings.Default.ShowOB && tickerStream.Data.Pair.ToUpper() == "XBT-AUD") {
                            pollingThread.ReportProgress(25, mSummary);  // update the OBView thingo
                        }
                    }

                    break;
            }
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
                DCEs["BTCM"].CurrentDCEStatus = "Online";
                Ticker_BTCM tickerStream = new Ticker_BTCM();
                tickerStream = JsonConvert.DeserializeObject<Ticker_BTCM>(message);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                if (tickerStream.marketId.ToUpper().StartsWith("BTC")) tickerStream.marketId = tickerStream.marketId.Replace(tickerStream.marketId.Substring(0, 3), "XBT");
                if (tickerStream.marketId.ToUpper().StartsWith("BCHABC")) tickerStream.marketId= tickerStream.marketId.Replace(tickerStream.marketId.Substring(0, 6), "BCH");

                mSummary.DayVolumeXbt = tickerStream.volume24h;
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
                DCEs["GDAX"].CurrentDCEStatus = "Online";
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
                    mSummary.DayVolumeXbt = vol;
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
                    /*if (wSocket_IR.IsAlive)*/ return true;
                   // return false;
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
                    DCEs["BFX"].CurrentDCEStatus = "API response error";
                    Debug.Print("BFX sent us an error, let's reset " + DateTime.Now.ToString());
                    DCEs["BFX"].socketsAlive = false;
                    DCEs["BFX"].socketsReset = true;
                    pollingThread.ReportProgress(12, "BFX");  // 12 is error
                }
                else if (message.Contains("unsubscribed")) {
                    Debug.Print("BFX UNSUBSCRIBED!  message: " + message);
                }
                else {
                    Debug.Print("rando message from BFX sockets: " + message);
                }
            }
            else if (message.StartsWith("[")) {  // is this how I tell if it's real socket data?  seems dodgy
                //Debug.Print("BFX MESSAGE: " + message);
                DCEs["BFX"].CurrentDCEStatus = "Online";
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
                                        mSummary.DayVolumeXbt = result;
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
