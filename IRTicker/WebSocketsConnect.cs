using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//using WebSocketSharp;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Collections.Concurrent;
using Websocket.Client;


namespace IRTicker {
    public class WebSocketsConnect {

        private Dictionary<string, DCE> DCEs;
        //private WebSocket wSocket_BTCM;
        private WebsocketClient client_IR, client_BTCM;
        private BackgroundWorker pollingThread;
        private Thread UITimerThread;
        private bool UITimerThreadProceed = true;
        //private ManualResetEvent startSocket_exitEvent = new ManualResetEvent(false);
        private string IRSocketsURL = "wss://websockets.independentreserve.com";
        //private string IRSocketsURL = "ws://dev.pushservice.independentreserve.net";
        List<string> IRdExchanges = new List<string>() { "IR", "IRUSD", "IRSGD" };
        private PrivateIR pIR;
        private List<DateTime> ThrottleConnection_BTCM = new List<DateTime>();  // tracks .Start() attempts on the wss API
        private List<DateTime> ThrottleSubscription_BTCM = new List<DateTime>();  // tracks .Send() subscription attempts on the API

        // constructor
        public WebSocketsConnect(Dictionary<string, DCE> _DCEs, BackgroundWorker _pollingThread, PrivateIR _pIR) {
            DCEs = _DCEs;
            pollingThread = _pollingThread;
            pIR = _pIR;

            // IR

            Debug.Print("IR (+SGD, USD) websocket connecting..");

            //Task.Factory.StartNew(() => {
            startSockets(IRdExchanges, IRSocketsURL);
            //startSockets("IRSGD", IRSocketsURL);
            //startSockets("IRUSD", IRSocketsURL);
            // })
            //;
            Debug.Print("after first start sockets");
            //subscribe_unsubscribe_new("IR", true);  // subscribe to all the things


            // BTCM

            BTCM_Connect_v3();

        }

        public void GetOrderBook_IR(string dExchange, string crypto, string fiat) {

            string pair = crypto + "-" + fiat;
            Tuple<bool, string> orderBookTpl = Utilities.Get(DCEs[dExchange].BaseURL + "/Public/GetAllOrders?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);

            if (orderBookTpl.Item1) {
                DCE.OrderBook orderBook = JsonConvert.DeserializeObject<DCE.OrderBook>(orderBookTpl.Item2);
                DCEs[dExchange].orderBooks[pair] = orderBook;
                if ((DCEs[dExchange].orderBooks[pair].BuyOrders.Count == 0) || (DCEs[dExchange].orderBooks[pair].SellOrders.Count == 0)) {
                    Debug.Print("One of the order books is empty... not continuing.  number of buy orders: " + DCEs[dExchange].orderBooks[pair].BuyOrders.Count + ", and sell orders: " + DCEs[dExchange].orderBooks[pair].SellOrders.Count);
                    return;
                }

                // next we need to convert this orderbook into a concurrent dictionary of OrderBook_IR objects
                // so yeah.. the "orderBook" object doesn't really get used anymore.  it's just like a staging area
                bool OBPulled = DCEs[dExchange].ConvertOrderBook_IR(pair);

                if (!OBPulled) {  // if the ob conversion process failed, then try again
                    Debug.Print(DateTime.Now + " - IR (" + dExchange + ") OB conversion (" + crypto + "-" + fiat + ") failed for some reason, trying again.");
                    subscribe_unsubscribe_new(dExchange, subscribe: true, crypto, fiat);
                    return;
                }

                Debug.Print(DateTime.Now.ToString() + " " + dExchange + " OB " + pair + " pulled, " + (DCEs[dExchange].orderBuffer_IR.ContainsKey(pair) ? DCEs[dExchange].orderBuffer_IR[pair].Count.ToString() : "n/a") + " ordes in the buffer");
                Debug.Print("and we have " + DCEs[dExchange].IR_OBs[pair].Item1.Count + " bids and " + DCEs[dExchange].IR_OBs[pair].Item2.Count + " offers");

                //int remainingBuffer = ApplyBuffer_IR(pair);
                //Print("(" + pair + ") Buffer applied, there are " + remainingBuffer + " left in the buffer (should be 0)");
                DCEs[dExchange].pulledSnapShot[pair] = true;
            }
            else {
                Debug.Print(DateTime.Now.ToString() + " | " + dExchange + " - couldn't download REST OB? - " + pair);
            }
        }

        // if subscribe is false then we unsubscribe
        public void subscribe_unsubscribe_new(string dExchange, bool subscribe, string crypto = "none", string fiat = "none") {
            if (fiat == "none") fiat = DCEs[dExchange].CurrentSecondaryCurrency;
            Debug.Print("subscribe_unsubscribe! -- " + dExchange + " -- did we subscribe: " + subscribe.ToString() + ", pair: " + crypto + "-" + fiat);
            string channel = "";
            JObject channel_obj = new JObject();  // using proper objects to build the subscribe request, not silly strings
            List<string> pairs = new List<string>();

            switch (dExchange) {
                case "IR":
                //case "IRUSD":
                //case "IRSGD":
                    channel_obj["Event"] = "Subscribe";
                    JArray data = new JArray();
                    if (crypto == "none") {  // unsubscribe or subscribe to EVERYTHING
                        //stopSockets("IR");  // don't want to stop everything, we just need to unsubscribe like we said we would.
                        //List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
                        foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                            if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + fiat)) {
                                string crypto1 = primaryCode;
                                data.Add("orderbook-" + crypto1.ToLower());
                            }
                        }
                        channel += "]} ";
                    }
                    else {  // or just one pair

                        data.Add("orderbook-" + crypto.ToLower());
                    }

                    channel_obj["Data"] = data;
                    channel = channel_obj.ToString();
                    Debug.Print(dExchange + " websocket subcribe/unsubscribe - " + (subscribe ? "subscribe" : "unsubscribe") + " event: " + channel);

                    if (client_IR.IsRunning) {
                        //Task.Run(() => 
                        client_IR.Send(channel)
                        //)
                        ;
                    }
                    else {
                        Debug.Print(DateTime.Now + " - " + dExchange + " sockets down when trying to " + (subscribe ? "subscribe" : "unsubscribe"));
                        DCEs[dExchange].socketsReset = true;
                        break;
                    }
                    if (subscribe) {  // if subscribing then grab the order books too.
                                      //List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
                        foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                            if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + fiat)) {
                                //pairList.Add(new Tuple<string, string>("XBT", "AUD"));
                                //pairList.Add(new Tuple<string, string>("XBT", "USD"));
                                //pairList.Add(new Tuple<string, string>("XBT", "NZD"));
                                //pairList.Add(new Tuple<string, string>(primaryCode, fiat));
                                if ((crypto == "none") || (crypto == primaryCode))
                                    GetOrderBook_IR(dExchange, primaryCode, fiat);
                            }
                        }

                    }

                    break;

                case "IRUSD":
                    if (subscribe) {  // if subscribing then grab the order books too.

                        foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                            if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + fiat)) {
                                if ((crypto == "none") || (crypto == primaryCode))
                                    GetOrderBook_IR(dExchange, primaryCode, fiat);
                            }
                        }
                    }

                    break;

                case "IRSGD":
                    if (subscribe) {  // if subscribing then grab the order books too.

                        foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                            if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + fiat)) {
                                if ((crypto == "none") || (crypto == primaryCode))
                                    GetOrderBook_IR(dExchange, primaryCode, fiat);
                            }
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

                    if (client_BTCM.IsRunning) {
                        channel_obj["messageType"] = "subscribe";
                        JArray channels = new JArray
                        {
                            "tick",
                            "heartbeat"
                        };

                        JArray marketIds = new JArray();

                        //channel = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\", \"heartbeat\"], \"marketIds\":[";  // we only ever subscribe, no scenario where we need to unsubscribe.  Unsubscribing is a pain for BTCM, see here https://api.btcmarkets.net/doc/v3#tag/WS_Overview
                        foreach (string crypto1 in pairs) {
                            string crypto2 = crypto1;
                            if (crypto2 == "XBT") crypto2 = "BTC";

                            marketIds.Add(crypto2 + "-" + fiat);
                            //channel += "\"" + crypto2 + "-" + fiat + "\", ";
                        }
                        //channel = channel.Substring(0, channel.Length - 2) + "]}";
                        channel_obj["channels"] = channels;
                        channel_obj["marketIds"] = marketIds;
                        channel = channel_obj.ToString();

                        Debug.Print("BTCHH channel subscription string: " + channel);

                        //pairList = "{\"messageType\":\"subscribe\", \"channels\":[\"tick\"], \"marketIds\":[\"BTC-AUD\"]}";

                        // this is where we subscribe to the btcm tick channel.  Somehow my app has spammed them badly in the past, so we need to try and throttle any connections.
                        // BTCM seems to allow about 1000 connection attempts in an hour (I was doing between 10k and 23k when they banned me).  Even 1000 is way more than
                        // I need, let's stop connecting if I have done 100 in an hour
                        bool tooManyConnectionAttempts = false;
                        if (ThrottleSubscription_BTCM.Count >= 100) {
                            Debug.Print(DateTime.Now + " - we have over 100 attempts to sub to BTCM channels.  Checking if it's within the last hour...");
                            DateTime hundredthFromNewest = ThrottleSubscription_BTCM[ThrottleSubscription_BTCM.Count - 100];
                            if (DateTime.Now < (hundredthFromNewest + TimeSpan.FromHours(1))) {
                                Debug.Print(" -- it is, throttle code has kicked in.  Won't connect");
                                DCEs["BTCM"].socketsAlive = false;
                                DCEs["BTCM"].CurrentDCEStatus = "Internally throttled";
                                tooManyConnectionAttempts = true;
                                Thread.Sleep(1000);
                            }
                        }

                        if (!tooManyConnectionAttempts) {
                            Debug.Print("Sending subscription request to BTCM...");
                            /*Task.Run(() => */client_BTCM.Send(channel);  // i don't think I need to spawn a new thread for this?
                            ThrottleSubscription_BTCM.Add(DateTime.Now);
                            if (ThrottleSubscription_BTCM.Count > 500) ThrottleSubscription_BTCM.RemoveAt(0);  // clean up the list, don't let it grow forever
                        }
                    }
                    else {
                        Debug.Print("BTCM tried to subscribe, but we're not connected.");
                        DCEs[dExchange].socketsReset = true;
                        Thread.Sleep(1000);
                    }

                    break;

                default:
                    Debug.Print(" ------ whoops, subscribe_unsubscribe_enw doesn't support this exchange: " + dExchange);
                    break;
            }
        }

        private void stopSockets(List<string> dExchanges) {
            client_IR.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "byee");
            foreach (string dExchange in dExchanges) DCEs[dExchange].socketsAlive = false;
            //startSocket_exitEvent.Set();  // hopefully this should let the existing startSockets() sub complete
            Debug.Print("IR (+SGD, USD) sockets stop command sent");
        }

        private void stopSockets_BTCM() {
            client_BTCM.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "byee");
            DCEs["BTCM"].socketsAlive = false;
            //startSocket_exitEvent.Set();  // hopefully this should let the existing startSockets() sub complete
            Debug.Print("BTCM sockets stop command sent");
        }

        private void startSockets(List<string> dExchanges, string socketsURL, bool doSubscribe = false) {
            var url = new Uri(socketsURL);
            foreach (string dExchange in dExchanges) DCEs[dExchange].socketsAlive = false;
            Debug.Print(DateTime.Now + " - startSockets called for IR (+SGD, USD)");

            //using (client_IR = new WebsocketClient(url)) {  getting rid of using statement..
            client_IR = new WebsocketClient(url);
            client_IR.ReconnectTimeout = TimeSpan.FromSeconds(70);
            client_IR.ReconnectionHappened.Subscribe(info =>
            {
                if (info.Type == ReconnectionType.Initial) {
                    Debug.Print("Initial 'reconnection', ignored");
                    foreach (string dExchange in dExchanges) DCEs[dExchange].socketsAlive = true;
                    foreach (string dExchange in dExchanges) DCEs[dExchange].socketsReset = false;
                }
                /*else if (info.Type == ReconnectionType.Lost) {
                    Debug.Print("Lost 'reconnection' ignored, attached to a Reset button click?");
                }*/
                else {
                    Debug.Print(DateTime.Now + " - (IR (+SGD, USD) reconnection) - clearing OB sub dicts...");
                    foreach (string dExchange in dExchanges) {
                        DCEs[dExchange].socketsAlive = false;
                        DCEs[dExchange].CurrentDCEStatus = "Reconnected";
                        Reinit_sockets(dExchange);
                        Debug.Print($"Reconnection happened, type: {info.Type}, resubscribing...");
                    }

                    // "IR" has to be first, so can't include this in the loop :/
                    subscribe_unsubscribe_new("IR", subscribe: true, crypto: "none", fiat: DCEs["IR"].CurrentSecondaryCurrency);  // resubscriibe to all pairs
                    subscribe_unsubscribe_new("IRUSD", subscribe: true, crypto: "none", fiat: DCEs["IRUSD"].CurrentSecondaryCurrency);  // resubscriibe to all pairs
                    subscribe_unsubscribe_new("IRSGD", subscribe: true, crypto: "none", fiat: DCEs["IRSGD"].CurrentSecondaryCurrency);  // resubscriibe to all pairs

                    //subscribe_unsubscribe_new("IR", subscribe: true, crypto: "none", fiat: "none");  // resubscriibe to all pairs, only "IR" because 
                    return;  // if we're subscribing, we shouldn't need to start the client or anything..?
                }

            });

            client_IR.MessageReceived.Subscribe(msg =>
            {
                //switch (dExchange) {
                //    case "IR":
                        MessageRX_IR(msg.Text);
                //        break;
                //}
            });

            Task.Run(() => client_IR.Start());

            //DCEs["IR"].socketsReset = false;  // i think this needs to be set.

            Debug.Print(DateTime.Now + " - about to start the UI timer!");
                                
            UITimerThread = new Thread(new ThreadStart(updateUITimer));
            // this command to start the thread
            UITimerThread.Start();
            Debug.Print("UI timer storted.");
            //await Task.Run(() => client_IR.Send("1"));
            //Debug.Print(DateTime.Now + " - we have moved on after the client_IR.send where we subscribe!");

            //startSocket_exitEvent.WaitOne();
            //}  trying to remove the using statement

            // if we want this code to re-subscribe (at time of writing this is only for when the user clicks the Reset button), then we loop until we have detected that
            // the sockets is up and running, and THEN subscribe.  Maybe there's a better IRQ style way of doing this, but i'm a pleb.
            if (doSubscribe) {
                bool keepLooping = true;
                int loopCounter = 0;
                do {
                    if (client_IR.IsRunning) {
                        subscribe_unsubscribe_new("IR", subscribe: true, crypto: "none", fiat: DCEs["IR"].CurrentSecondaryCurrency);  // resubscriibe to all pairs
                        foreach (string dExchange in dExchanges) {  // commenting this out to test subscribing just to the crypto, not the pair (which should subscribe to all secondard currencies?
                            if (dExchange != "IR")  // did "IR" above already, must do it first
                                subscribe_unsubscribe_new(dExchange, subscribe: true, crypto: "none", fiat: DCEs[dExchange].CurrentSecondaryCurrency);  // resubscriibe to all pairs
                        }
                        //subscribe_unsubscribe_new("IR", subscribe: true, crypto: "none", fiat: "none");  // resubscriibe to all pairs


                        keepLooping = false;
                    }
                    else {
                        Thread.Sleep(500);
                        loopCounter++;
                    }
                } while (keepLooping && (loopCounter < 10)) ;  // 5 seconds and we're still not connected?  something else is wrong.
            }
        }

        // shuts down the UITimerThread.  Only called when the app is terminating.
        public void stopUITimerThread() {
            if (UITimerThread != null && UITimerThread.IsAlive) UITimerThread.Abort();
        }


        private void updateUITimer() {

            while (UITimerThreadProceed) {
                foreach (string dExchange in IRdExchanges) {
                    foreach (KeyValuePair<string, ConcurrentDictionary<int, Ticker_IR>> pair in DCEs[dExchange].orderBuffer_IR) {
                        if (DCEs[dExchange].newOrders[pair.Key] > 0) {
                            if ((dExchange != "IRSGD") && (dExchange != "IRUSD") && (pIR != null))  // only do this for IR - the other two don't have an IRAccounts panel or anything
                                Task.Run(() => pIR.compileAccountOrderBookAsync(pair.Key));  // hopefully this will just run this method asynchronously
                                                                                                          //pollingThread.ReportProgress(20, pair.Key);  // this will tell the accounts panel to update it's OB view
                            if ((DCEs[dExchange].orderBuffer_IR[pair.Key].Count > 0) && DCEs[dExchange].pulledSnapShot[pair.Key]) 
                                applyBufferToOB(pair.Key, dExchange);
                            DCEs[dExchange].newOrders[pair.Key] = 0;  // the buffer is being drained, so we reset the newOrders count
                        }
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
                stopSockets(IRdExchanges);
            }
        }

        public void BTCM_Disconnect() {
            //UITimerThreadProceed = false;  don't think we actually need to stop this running ever..
            if (client_BTCM.IsRunning) {
                Debug.Print(DateTime.Now + " - BTCM_Disconnect sub: BTCM running, will stop");
                stopSockets_BTCM();
            }
        }

        // BTCM version of startSockets()
        private async Task BTCM_Connect_v3() {
            var url = new Uri("wss://socket.btcmarkets.net/v2");
            DCEs["BTCM"].socketsAlive = false;
            Debug.Print(DateTime.Now + " - BTCM_Connect_v3 called for BTCM");

            client_BTCM = new WebsocketClient(url);
            client_BTCM.ReconnectTimeout = TimeSpan.FromSeconds(70);
            
            client_BTCM.ReconnectionHappened.Subscribe(info =>
            {
                if (info.Type == ReconnectionType.Initial) {
                    Debug.Print("BTCM Initial 'reconnection', ignored");
                    DCEs["BTCM"].socketsAlive = true;
                    DCEs["BTCM"].socketsReset = false;
                }
                /*else if (info.Type == ReconnectionType.Lost) {
                    Debug.Print("Lost 'reconnection' ignored, attached to a Reset button click?");
                }*/
                else if (info.Type == ReconnectionType.NoMessageReceived) {
                    Debug.Print(DateTime.Now + " - NoMessageReceived 'reconnection' ignored, have seen data still coming through when receiving this");
                }
                else {
                    Debug.Print(DateTime.Now + " - (BTCM reconnection)");
                    DCEs["BTCM"].socketsAlive = false;
                    DCEs["BTCM"].CurrentDCEStatus = "Reconnected";

                    Debug.Print($"Reconnection happened, type: {info.Type}, resubscribing...");
                    subscribe_unsubscribe_new("BTCM", subscribe: true, crypto: "none", fiat: DCEs["BTCM"].CurrentSecondaryCurrency);  // resubscriibe to all pairs

                    return;  // if we're subscribing, we shouldn't need to start the client or anything..?
                }

            });

            client_BTCM.MessageReceived.Subscribe(msg =>
            {
                MessageRX_BTCMv2(msg.Text);
            });

            // this is where we connect to the btcm sockets.  Somehow my app has spammed them badly in the past, so we need to try and throttle any connections.
            // BTCM seems to allow about 1000 connection attempts in an hour (I was doing between 10k and 23k when they banned me).  Even 1000 is way more than
            // I need, let's stop connecting if I have done 100 in an hour
            bool tooManyConnectionAttempts = false;
            if (ThrottleConnection_BTCM.Count >= 100) {
                Debug.Print(DateTime.Now + " - we have over 100 attempts to connect to BTCM sockets.  Checking if it's within the last hour...");
                DateTime hundredthFromNewest = ThrottleConnection_BTCM[ThrottleConnection_BTCM.Count - 100];
                if (DateTime.Now < (hundredthFromNewest + TimeSpan.FromHours(1))) {
                    Debug.Print(" -- it is, throttle code has kicked in.  Won't connect");
                    DCEs["BTCM"].socketsAlive = false;
                    DCEs["BTCM"].CurrentDCEStatus = "Internally throttled";
                    tooManyConnectionAttempts = true;
                }
            }

            if (!tooManyConnectionAttempts) {
                await client_BTCM.Start();
                ThrottleConnection_BTCM.Add(DateTime.Now);
                if (ThrottleConnection_BTCM.Count > 500) ThrottleConnection_BTCM.RemoveAt(0);  // clean up the list, don't let it grow forever
            }

            //Task.Run(() => client_BTCM.Start());

            // if we want this code to re-subscribe (at time of writing this is only for when the user clicks the Reset button), then we loop until we have detected that
            // the sockets is up and running, and THEN subscribe.  Maybe there's a better IRQ style way of doing this, but i'm a pleb.
            if (false) {
                bool keepLooping = true;
                int loopCounter = 0;
                do {
                    if (client_BTCM.IsRunning) {
                        subscribe_unsubscribe_new("BTCM", subscribe: true, crypto: "none");  // resubscriibe to all pairs
                        keepLooping = false;
                    }
                    else {
                        Thread.Sleep(500);
                        loopCounter++;
                    }
                } while (keepLooping && (loopCounter < 10));  // 5 seconds and we're still not connected?  something else is wrong.
            }
        }

    

    /*private void BTCM_Connect_v2() {

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
        }*/

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
            subscribe_unsubscribe_new(dExchange, subscribe:false, crypto, (oldFiat == "none" ? DCEs[dExchange].CurrentSecondaryCurrency : oldFiat));
            Reinit_sockets(dExchange, crypto, newFiat);
            subscribe_unsubscribe_new(dExchange, subscribe:true, crypto, (newFiat == "none" ? DCEs[dExchange].CurrentSecondaryCurrency : newFiat));
        }


        public async Task WebSocket_Reconnect(string dExchange) {
            Debug.Print("WebSocket_Reconnect for " + dExchange);
            if (!DCEs[dExchange].HasStaticData) {
                Debug.Print("Trying to reconnect to " + dExchange + " but there's no static data.  will not.");
                return;
            }
            switch (dExchange) {
                case "IRUSD":
                case "IRSGD":
                case "IR":  
                    // IR is always IR + SGD + USD
                    Debug.Print("WebSocket_Reconnect: IR?? this shouldn't be called?  shouldn't it auto-reconnect?");
                    if (client_IR.IsRunning) {
                        Debug.Print(DateTime.Now + " - IR (+SGD, USD) running, will stop");
                        DCEs[dExchange].CurrentDCEStatus = "Reconnecting...";
                        stopSockets(IRdExchanges);  
                    }
                    stopUITimerThread();  // if it hasn't stopped by now, we force it.

                    foreach (string dExchange1 in IRdExchanges) Reinit_sockets(dExchange1);
                    startSockets(IRdExchanges, IRSocketsURL, doSubscribe: true);  // the "true" here tells the sub to also subscribe to all channels as well
                    foreach (string dExchange1 in IRdExchanges) DCEs[dExchange1].HeartBeat = DateTime.Now;
                    //}
                    break;
                case "BTCM":
                    if (client_BTCM.IsRunning) {
                        await client_BTCM.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "byee");
                    }
                    await BTCM_Connect_v3();
                    subscribe_unsubscribe_new("BTCM", subscribe: true, crypto: "none", fiat: DCEs["BTCM"].CurrentSecondaryCurrency);  // resubscriibe to all pairs
                    break;
            }
            DCEs[dExchange].socketsReset = false;  // when closing the stream, the OnClose method is called, which sets the socketsReset to true.  need to turn this off so we don't reconnect forever

            //re-subscribe?
            Debug.Print(dExchange + " - re-subscribing to all pairs...");

            // for a reconnect, the IR code will automatically subscribe once the socket is active (see the loop/sleep shitty code in startSockets).  this way we only subscribe once the socket is up, and don't have to rely on the slow doWork loop to detect an issue
            // shouldn't need this, subscribe_ussubscribe() should be called already above
            //if ((dExchange != "IR") && (dExchange != "IRUSD") && (dExchange != "IRSGD")) subscribe_unsubscribe_new(dExchange, true);  // subscribe to all
        }

        // only actually called for reconnections
        public void Reinit_sockets(string dExchange, string crypto = "none", string fiat = "none") {
            //if (fiat == "none") fiat = DCEs[dExchange].CurrentSecondaryCurrency;
            //if (crypto == "none") {
            Debug.Print(DateTime.Now + " - Reinit sockets for crypto: " + crypto + " and fiat: " + fiat);
            foreach (string crypto1 in DCEs[dExchange].PrimaryCurrencyList) {
                foreach (string fiat1 in DCEs[dExchange].SecondaryCurrencyList) {
                    if (((crypto == crypto1) || (crypto == "none")) &&
                        ((fiat == fiat1) || (fiat == "none"))) {
                        DCEs[dExchange].pulledSnapShot[crypto1 + "-" + fiat1] = false;
                        DCEs[dExchange].positiveSpread[crypto1 + "-" + fiat1] = true;
                        DCEs[dExchange].OBResetFlag["ORDERBOOK-" + crypto1 + "-" + fiat1] = false;
                        DCEs[dExchange].channelNonce["ORDERBOOK-" + crypto1 + "-" + fiat1] = 0;
                        DCEs[dExchange].newOrders[crypto1 + "-" + fiat1] = 0;
                        //if (DCEs[dExchange].orderBuffer_IR.ContainsKey(crypto1 + "-" + fiat1)) DCEs[dExchange].orderBuffer_IR[crypto1 + "-" + fiat1].Clear();
                        /*else*/ DCEs[dExchange].orderBuffer_IR[crypto1 + "-" + fiat1] = new ConcurrentDictionary<int, Ticker_IR>();
                        DCEs[dExchange].ClearOrderBookSubDicts(crypto1, fiat1);
                    }
                }
            }

           /* }
            else {
                DCEs[dExchange].pulledSnapShot[crypto + "-" + fiat] = false;
                DCEs[dExchange].positiveSpread[crypto + "-" + fiat] = true;
                DCEs[dExchange].OBResetFlag["ORDERBOOK-" + crypto + "-" + fiat] = false;
                DCEs[dExchange].channelNonce["ORDERBOOK-" + crypto + "-" + fiat] = 0;
                DCEs[dExchange].ClearOrderBookSubDicts(crypto, fiat);
                DCEs[dExchange].newOrders[crypto + "-" + fiat] = 0;
                if (DCEs[dExchange].orderBuffer_IR.ContainsKey(crypto + "-" + fiat)) DCEs[dExchange].orderBuffer_IR[crypto + "-" + fiat].Clear();
                else DCEs[dExchange].orderBuffer_IR[crypto + "-" + fiat] = new ConcurrentDictionary<int, Ticker_IR>();
            }*/
        }

        /* Sample socket data:
            {
                "Channel":"orderbook-eth",
                "Nonce":28,
                "Data":{
                    "OrderType":"LimitBid",
                    "OrderGuid":"dbe7b832-b9b7-4eac-84ce-9f49c2a93b87",
                    "Price":{  // if you subscribe to a pair (orderbook-xbt-aud) then the price comes through asa decimal, not an array as shown here
                        "aud":2500,
                        "usd":1816.5,
                        "nzd":2587.5,
                        "sgd":2453
                    },
                    "Volume":1
                },
                "Event":"NewOrder"
            }

            {
                "Channel":"orderbook-eth",
                "Nonce":30,
                "Data":{
                    "OrderType":"LimitBid",
                    "OrderGuid":"e64fb6e2-a9f8-4f52-95e7-5a3c7a9f8f53",
                    "Volume":0.09646808
                },
                "Event":"OrderChanged"
            }

            {
                "Channel":"orderbook-eth",
                "Nonce":29,
                "Data":{
                    "OrderType":"LimitBid",
                    "OrderGuid":"dbe7b832-b9b7-4eac-84ce-9f49c2a93b87"
                },
                "Event":"OrderCanceled"
            }
        */
        private void MessageRX_IR(string message) {
            if (message == null) return;
            //Debug.Print("IR MSG ---- " + message);

            // catching order cancelled events and logging it to try and figure out what's up.
            //if (message.Contains("OrderChanged") && message.Contains("xbt-aud")) {
            //    Debug.Print("changed GUID: " + message);  //message.Substring(message.IndexOf("OrderGuid\":\"")));
            //}

            foreach (string dExchange in IRdExchanges) DCEs[dExchange].socketsAlive = true;
            if (message.Contains("\"Event\":\"Subscriptions\"")) {
                // ignore the subscriptions event.  it breaks parsing too :/
                Debug.Print("IGNORING - " + message);
                return;
            }
            if (message.Contains("Error")) {
                Debug.Print("IR ERROR in websockets, resetting.  error: " + message);
                DCEs["IR"].socketsReset = true;  // it seems when we start getting errors, the socket is unrecoverable, need to start again. 29/4/2022 (ben says maybe there's issues on the server)
                DCEs["IR"].socketsAlive = false;
                DCEs["IR"].CurrentDCEStatus = "Resetting...";
                stopSockets(IRdExchanges);
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
            foreach (string dExchange in IRdExchanges) DCEs[dExchange].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat
            foreach (string dExchange in IRdExchanges) DCEs[dExchange].CurrentDCEStatus = "Online";


            //Ticker_IR tickerStream = new Ticker_IR();  // don't think we care about the pair at this stage... let's see.
            //tickerStream = JsonConvert.DeserializeObject<Ticker_IR>(message);

            Ticker_IR tickerStream = JsonConvert.DeserializeObject<Ticker_IR>(message, new Ticker_IR_Converter());

            /*string eventStr = tickerStream.Event;
            string crypto = eventStr.Replace("orderbook-", "");
            string pair = crypto + "-" + CurrentSecondaryCurrency;

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
            //string pair = tickerStream.Data.Pair.ToUpper();
            //string channel = tickerStream.Channel.ToUpper();
            //Debug.Print("---- Nonce received: " + tickerStream.Nonce);

            string crypto = tickerStream.Channel.Replace("orderbook-", "").ToUpper();

            // here we have the pricing for all 4 IR currencies, so figure out which we care about and throw em into buffers

            foreach (string dExchange in IRdExchanges) {
                if (!DCEs[dExchange].PrimaryCurrencyList.Contains(crypto)) return;  // only consider cryptos this exchange supports
                string pair = crypto + "-" + DCEs[dExchange].CurrentSecondaryCurrency;
                if (!DCEs[dExchange].pulledSnapShot.Keys.Contains(pair) || !DCEs[dExchange].orderBuffer_IR.ContainsKey(pair)) return;  // can happen when we're switching currencies
                
                if (DCEs[dExchange].pulledSnapShot[pair]) {  // if we haven't even got the OB yet, or maybe we're in the middle of resetting the dictionaries and we haven't populated the pulledSnapshot dict with all the pairs yet..
                    DCEs[dExchange].newOrders[pair]++;
                }
                DCEs[dExchange].orderBuffer_IR[pair][tickerStream.Nonce] = tickerStream;  // add this event to the buffer
            }
        }

        // this sub does some nonce maintenance and then spins through the orderbookBuffer, applying sequential buffered events
        private void applyBufferToOB(string pair, string dExchange) {

            string channel = "ORDERBOOK-" + pair;

            // if we don't got a nonce yet
            if (DCEs[dExchange].channelNonce[channel] == 0) {
                // orderBuffer_IR must have at least one order in it, so we should be able to safely request the minimum key.
                DCEs[dExchange].channelNonce[channel] = DCEs[dExchange].orderBuffer_IR[pair].Keys.Min() - 1;  // find the smallest nonce in the buffer, and set the channel nonce to one below that
                //Debug.Print("just set the Nonce to 1 before the first we got, it is: " + DCEs["IR"].channelNonce[channel]);
            }

            while (DCEs[dExchange].orderBuffer_IR[pair].ContainsKey(DCEs[dExchange].channelNonce[channel] + 1)) {  // if the buffer has the next nonce...
                DCEs[dExchange].channelNonce[channel]++;  // cool, let's advance the nonce
                if (DCEs[dExchange].orderBuffer_IR[pair].TryRemove(DCEs[dExchange].channelNonce[channel], out Ticker_IR ticker)) {  // pop the ticker object,
                    //if (pair == "XBT-AUD") Debug.Print(DateTime.Now + " - (" + pair + ") parsing nonce " + ticker.Nonce + " from buffer, there are " + (DCEs["IR"].orderBuffer_IR[pair].Count) + " other buffered events in there");
                    parseTicker_IR(ticker, dExchange);  // and parse it
                }
                else {
                    Debug.Print(DateTime.Now + " - can't pop ticker object from buffer?? channel: " + channel);
                    DCEs[dExchange].OBResetFlag[channel] = true;  // let's start again.
                    DCEs[dExchange].channelNonce[channel]--;  // make sure it fails
                    return;
                }
            }
            if (DCEs[dExchange].orderBuffer_IR[pair].Count > 0) {
                /*if (pair == "XBT-AUD")*/ Debug.Print(DateTime.Now + " - (" + pair + ") ooo nonce - " + DCEs[dExchange].orderBuffer_IR[pair].Count + " if only 1, it is: " + (DCEs[dExchange].orderBuffer_IR[pair].Count == 1 ? DCEs[dExchange].orderBuffer_IR[pair].Keys.FirstOrDefault().ToString() : "") + " and the current nonce is " + DCEs[dExchange].channelNonce[channel]);
                pollingThread.ReportProgress(27, new Tuple<bool, string, string>(true, Utilities.SplitPair(pair).Item1, dExchange));  // update pair text colour to gray - true = nonce issues
            }

            // we should check how full our buffer is. If there's more than x items (??) then it's probably too full.
            if (((DCEs[dExchange].orderBuffer_IR[pair].Count > 5) && DCEs[dExchange].pulledSnapShot[pair]) || (DCEs[dExchange].OBResetFlag[channel])) {
                Debug.Print("NONCE - too many buffered nonces, can't recover " + channel + ", or the OBResetFlag is true, time to dump and restart. dExchange: " + dExchange);

                if (Properties.Settings.Default.FlashForm) pollingThread.ReportProgress(26);  // flash the window if the setting is enabled

                // now subscribe back to the channel
                WebSocket_Resubscribe("IR", Utilities.SplitPair(pair).Item1);  // IR first, then the others
                WebSocket_Resubscribe("IRUSD", Utilities.SplitPair(pair).Item1);  
                WebSocket_Resubscribe("IRSGD", Utilities.SplitPair(pair).Item1);  
            }
        }
        

        // event is clean, correct nonce etc, lets parse it.
        public void parseTicker_IR(Ticker_IR tickerStream, string dExchange) { 

            if (!tickerStream.Data.OrderType.StartsWith("Limit")) {
                Debug.Print(DateTime.Now + " - (" + tickerStream.Channel + ") ignoring a " + tickerStream.Data.OrderType + " order - " + tickerStream.Channel.Substring(10) + " " + tickerStream.Data.Volume + ".  event: " + tickerStream.Event + ".  guid: " + tickerStream.Data.OrderGuid);
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
                    DCE.MarketSummary mSummary = DCEs[dExchange].OrderBookEvent_IR(tickerStream);
                    if (mSummary != null) {
                        //Debug.Print("spread changing event: " + message);
                        if (mSummary.spread < 0) {
                            Debug.Print(DateTime.Now + " " + dExchange + " spread (" + tickerStream.Channel + " / " + mSummary.pair + ") is " + mSummary.spread + " :(  bid: " + mSummary.CurrentHighestBidPrice + " and offer: " + mSummary.CurrentLowestOfferPrice);
                        }
                        if ((DCEs["IR"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode) ||
                                (mSummary.SecondaryCurrencyCode == "USD") ||
                                (mSummary.SecondaryCurrencyCode == "SGD" )) {  //eventPair.Item2.ToUpper()) {
                            if (pollingThread.IsBusy)
                                pollingThread.ReportProgress(21, mSummary);  // do update_pairs thing
                        }
                        else if (Properties.Settings.Default.ShowOB && mSummary.pair.ToUpper() == "XBT-AUD") {  // "else if" because we call the "25" report progress from within the "21" one, don't want to call it twice if we can help it.
                            if (pollingThread.IsBusy)
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
                /*if (DCEs["BTCM"].CurrentSecondaryCurrency == mSummary.SecondaryCurrencyCode)*/ pollingThread.ReportProgress(31, mSummary);  // only update the UI for pairs we care about // we only subscribe to AUD, so no point checking
                DCEs["BTCM"].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat

            }
            else if (message.Contains("\"messageType\":\"heartbeat\"")) {
                // let's keep track of this.
                //Debug.Print(DateTime.Now + " - BTCMv2 - legit heartbeat");
                DCEs["BTCM"].HeartBeat = DateTime.Now;  // any message through the socket counts as a heartbeat
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
                case "IRUSD":
                case "IRSGD":
                    /*if (wSocket_IR.IsAlive)*/ return true;
                   // return false;
                case "BTCM":
                    if (client_BTCM.IsRunning) return true;
                    return false;
            }
            Debug.Print("Sockets, checking a socket alive, we have reached the end without returning.  we never should.  DCE: " + dExchange);
            return false;
        }

        // let's us create a case insensive dictionary inside the Ticker_IR object that we deserialise from the websockets stream
        // we want a case insensitive Price dictionary so we can reference "aud" and "AUD" and not care.
        public class Ticker_IR_Converter : JsonConverter<Ticker_IR> {
            public override Ticker_IR ReadJson(JsonReader reader, Type objectType, Ticker_IR existingValue, bool hasExistingValue, JsonSerializer serializer) {
                JObject obj = JObject.Load(reader);

                Ticker_IR ticker = new Ticker_IR();
                serializer.Populate(obj.CreateReader(), ticker);

                // Convert the Price dictionary to a case-insensitive dictionary if it's not null
                if (ticker.Data.Price != null) {
                    ticker.Data.Price = new Dictionary<string, decimal>(ticker.Data.Price, StringComparer.OrdinalIgnoreCase);
                }

                return ticker;
            }

            public override void WriteJson(JsonWriter writer, Ticker_IR value, JsonSerializer serializer) {
                throw new NotImplementedException();
            }
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
    }
}
