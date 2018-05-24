using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Controls;
using WebSocketSharp;

// todo:

// implement coinspot
// hover text over the last price should show the best bid and offer price
// de-prioritise BCH and LTC from BitFinex to help with rate limiting - needs more work... maybe look to see if there's a private API that doesn't get rate limited?


namespace IRTicker {
    public partial class IRTicker : Form {
        private const String APP_ID = "Nikola.IRTicker";
        private const int minRefreshFrequency = 20;

        private Dictionary<string, DCE> DCEs;  // the master dictionary that holds all the data we pull from all the crypto APIs
        private CE.MarketSummary_OER fiatRates;  // as we only have one fiat source, just hold the market summary class directly
        private bool fiatIsUSD = true;  // to keep track of which fiat base is currently being shown.  I suppose we could inspect the groupbox text field.. but meh
        private bool OER_NetworkAvailable = true;
        private bool refreshFiat = true;
        private Dictionary<string, UIControls> UIControls_Dict;
        private List<string> Exchanges;
        private string cryptoDir = "";
        private List<string> shitCoins = new List<string>() { "BCH", "LTC", "XRP", "DOGE" };  // we don't poll the shit coins as often to help with rate limiting
        private int shitCoinPollRate = 3; // this is how many polls we loop before we call shit coin APIs.  eg 3 means we only poll the shit coins once every 3 polls.

        public IRTicker() {
            InitializeComponent();

            //cryptoDir = @"C:\ntemp\Crypto\";
            cryptoDir = Path.Combine(System.IO.Path.GetTempPath(), @"Crypto\");
            //System.Windows.MessageBox.Show("crypto path: " + cryptoDir);

            if (!Directory.Exists(cryptoDir)) {
                Directory.CreateDirectory(cryptoDir);
            }

            folderDialogTextbox.Text = cryptoDir;

            Exchanges = new List<string> {
                { "IR" },
                { "BTCM" },
                { "GDAX" },
                { "BFX" },
                { "CSPT" }
            };

            DCEs = new Dictionary<string, DCE> {

                // seed the DCEs dictionary with empty DCEs for the DCEs we will be interrogating
                { "IR", new DCE("Independent Reserve") },
                { "BTCM", new DCE("BTC Markets") },
                { "GDAX", new DCE("GDAX") },
                { "BFX", new DCE("BitFinex") },
                { "CSPT", new DCE("CoinSpot") }
            };

            // BTCM, BFX, and CSPT have no APIs that let you download the currency pairs, so just set them manually
            DCEs["BTCM"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\"";
            DCEs["BTCM"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["BTCM"].HasStaticData = true;  // we don't poll for static data, so just say we have it.

            DCEs["BFX"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\"";
            DCEs["BFX"].SecondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            DCEs["CSPT"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"DOGE\",\"LTC\"";
            DCEs["CSPT"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["CSPT"].HasStaticData = true;  // we don't poll for static data, so just say we have it.

            InitialiseUIControls();

            // if they have somehow set it below 20 secs, force back to 20.
            if (int.TryParse(Properties.Settings.Default.RefreshFreq, out int freq)) {
                if (freq < minRefreshFrequency) Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            }
            else Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();

            fiatInvert_checkBox.Checked = Properties.Settings.Default.FiatInverse;
            refreshFrequencyTextbox.Text = Properties.Settings.Default.RefreshFreq.ToString(); ;

            VersionLabel.Text = "IR Ticker version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            pollingThread.RunWorkerAsync();
        }

        // super manual function to push the UI controls into objects so we can read them programattically
        private void InitialiseUIControls() {

            UIControls_Dict = new Dictionary<string, UIControls> {

                // seed the UIControls_Dict dictionary with empty UIControls
                { "IR", new UIControls() },
                { "BTCM", new UIControls() },
                { "GDAX", new UIControls() },
                { "BFX", new UIControls() },
                { "CSPT", new UIControls() }
            };

            // IR
            UIControls_Dict["IR"].Name = "IR";
            UIControls_Dict["IR"].dExchange_GB = IR_GroupBox;
            UIControls_Dict["IR"].XBT_Label = IR_XBT_Label1;
            UIControls_Dict["IR"].XBT_Price = IR_XBT_Label2;
            UIControls_Dict["IR"].XBT_Spread = IR_XBT_Label3;
            UIControls_Dict["IR"].XBT_PriceTT = IR_XBT_PriceTT;
            UIControls_Dict["IR"].ETH_Label = IR_ETH_Label1;
            UIControls_Dict["IR"].ETH_Price = IR_ETH_Label2;
            UIControls_Dict["IR"].ETH_Spread = IR_ETH_Label3;
            UIControls_Dict["IR"].ETH_PriceTT = IR_ETH_PriceTT;
            UIControls_Dict["IR"].BCH_Label = IR_BCH_Label1;
            UIControls_Dict["IR"].BCH_Price = IR_BCH_Label2;
            UIControls_Dict["IR"].BCH_Spread = IR_BCH_Label3;
            UIControls_Dict["IR"].BCH_PriceTT = IR_BCH_PriceTT;
            UIControls_Dict["IR"].LTC_Label = IR_LTC_Label1;
            UIControls_Dict["IR"].LTC_Price = IR_LTC_Label2;
            UIControls_Dict["IR"].LTC_Spread = IR_LTC_Label3;
            UIControls_Dict["IR"].LTC_PriceTT = IR_LTC_PriceTT;
            UIControls_Dict["IR"].AvgPrice_BuySell = IR_BuySellComboBox;
            UIControls_Dict["IR"].AvgPrice_NumCoins = IR_NumCoinsTextBox;
            UIControls_Dict["IR"].AvgPrice_Crypto = IR_CryptoComboBox;
            UIControls_Dict["IR"].AvgPrice = IR_AvgPrice_Label;

            // BTCM
            UIControls_Dict["BTCM"].Name = "BTCM";
            UIControls_Dict["BTCM"].dExchange_GB = BTCM_GroupBox;
            UIControls_Dict["BTCM"].XBT_Label = BTCM_XBT_Label1;
            UIControls_Dict["BTCM"].XBT_Price = BTCM_XBT_Label2;
            UIControls_Dict["BTCM"].XBT_Spread = BTCM_XBT_Label3;
            UIControls_Dict["BTCM"].XBT_PriceTT = BTCM_XBT_PriceTT;
            UIControls_Dict["BTCM"].ETH_Label = BTCM_ETH_Label1;
            UIControls_Dict["BTCM"].ETH_Price = BTCM_ETH_Label2;
            UIControls_Dict["BTCM"].ETH_Spread = BTCM_ETH_Label3;
            UIControls_Dict["BTCM"].ETH_PriceTT = BTCM_ETH_PriceTT;
            UIControls_Dict["BTCM"].BCH_Label = BTCM_BCH_Label1;
            UIControls_Dict["BTCM"].BCH_Price = BTCM_BCH_Label2;
            UIControls_Dict["BTCM"].BCH_Spread = BTCM_BCH_Label3;
            UIControls_Dict["BTCM"].BCH_PriceTT = BTCM_BCH_PriceTT;
            UIControls_Dict["BTCM"].LTC_Label = BTCM_LTC_Label1;
            UIControls_Dict["BTCM"].LTC_Price = BTCM_LTC_Label2;
            UIControls_Dict["BTCM"].LTC_Spread = BTCM_LTC_Label3;
            UIControls_Dict["BTCM"].LTC_PriceTT = BTCM_LTC_PriceTT;
            UIControls_Dict["BTCM"].XRP_Label = BTCM_XRP_Label1;
            UIControls_Dict["BTCM"].XRP_Price = BTCM_XRP_Label2;
            UIControls_Dict["BTCM"].XRP_Spread = BTCM_XRP_Label3;
            UIControls_Dict["BTCM"].XRP_PriceTT = BTCM_XRP_PriceTT;
            UIControls_Dict["BTCM"].AvgPrice_BuySell = BTCM_BuySellComboBox;
            UIControls_Dict["BTCM"].AvgPrice_NumCoins = BTCM_NumCoinsTextBox;
            UIControls_Dict["BTCM"].AvgPrice_Crypto = BTCM_CryptoComboBox;
            UIControls_Dict["BTCM"].AvgPrice = BTCM_AvgPrice_Label;

            // GDAX
            UIControls_Dict["GDAX"].Name = "GDAX";
            UIControls_Dict["GDAX"].dExchange_GB = GDAX_GroupBox;
            UIControls_Dict["GDAX"].XBT_Label = GDAX_XBT_Label1;
            UIControls_Dict["GDAX"].XBT_Price = GDAX_XBT_Label2;
            UIControls_Dict["GDAX"].XBT_Spread = GDAX_XBT_Label3;
            UIControls_Dict["GDAX"].XBT_PriceTT = GDAX_XBT_PriceTT;
            UIControls_Dict["GDAX"].ETH_Label = GDAX_ETH_Label1;
            UIControls_Dict["GDAX"].ETH_Price = GDAX_ETH_Label2;
            UIControls_Dict["GDAX"].ETH_Spread = GDAX_ETH_Label3;
            UIControls_Dict["GDAX"].ETH_PriceTT = GDAX_ETH_PriceTT;
            UIControls_Dict["GDAX"].BCH_Label = GDAX_BCH_Label1;
            UIControls_Dict["GDAX"].BCH_Price = GDAX_BCH_Label2;
            UIControls_Dict["GDAX"].BCH_Spread = GDAX_BCH_Label3;
            UIControls_Dict["GDAX"].BCH_PriceTT = GDAX_BCH_PriceTT;
            UIControls_Dict["GDAX"].LTC_Label = GDAX_LTC_Label1;
            UIControls_Dict["GDAX"].LTC_Price = GDAX_LTC_Label2;
            UIControls_Dict["GDAX"].LTC_Spread = GDAX_LTC_Label3;
            UIControls_Dict["GDAX"].LTC_PriceTT = GDAX_LTC_PriceTT;
            UIControls_Dict["GDAX"].AvgPrice_BuySell = GDAX_BuySellComboBox;
            UIControls_Dict["GDAX"].AvgPrice_NumCoins = GDAX_NumCoinsTextBox;
            UIControls_Dict["GDAX"].AvgPrice_Crypto = GDAX_CryptoComboBox;
            UIControls_Dict["GDAX"].AvgPrice = GDAX_AvgPrice_Label;

            // BFX
            UIControls_Dict["BFX"].Name = "BFX";
            UIControls_Dict["BFX"].dExchange_GB = BFX_GroupBox;
            UIControls_Dict["BFX"].XBT_Label = BFX_XBT_Label1;
            UIControls_Dict["BFX"].XBT_Price = BFX_XBT_Label2;
            UIControls_Dict["BFX"].XBT_Spread = BFX_XBT_Label3;
            UIControls_Dict["BFX"].XBT_PriceTT = BFX_XBT_PriceTT;
            UIControls_Dict["BFX"].ETH_Label = BFX_ETH_Label1;
            UIControls_Dict["BFX"].ETH_Price = BFX_ETH_Label2;
            UIControls_Dict["BFX"].ETH_Spread = BFX_ETH_Label3;
            UIControls_Dict["BFX"].ETH_PriceTT = BFX_ETH_PriceTT;
            UIControls_Dict["BFX"].BCH_Label = BFX_BCH_Label1;
            UIControls_Dict["BFX"].BCH_Price = BFX_BCH_Label2;
            UIControls_Dict["BFX"].BCH_Spread = BFX_BCH_Label3;
            UIControls_Dict["BFX"].BCH_PriceTT = BFX_BCH_PriceTT;
            UIControls_Dict["BFX"].LTC_Label = BFX_LTC_Label1;
            UIControls_Dict["BFX"].LTC_Price = BFX_LTC_Label2;
            UIControls_Dict["BFX"].LTC_Spread = BFX_LTC_Label3;
            UIControls_Dict["BFX"].LTC_PriceTT = BFX_LTC_PriceTT;
            UIControls_Dict["BFX"].AvgPrice_BuySell = BFX_BuySellComboBox;
            UIControls_Dict["BFX"].AvgPrice_NumCoins = BFX_NumCoinsTextBox;
            UIControls_Dict["BFX"].AvgPrice_Crypto = BFX_CryptoComboBox;
            UIControls_Dict["BFX"].AvgPrice = BFX_AvgPrice_Label;

            // CoinSpot
            UIControls_Dict["CSPT"].Name = "CSPT";
            UIControls_Dict["CSPT"].dExchange_GB = CSPT_GroupBox;
            UIControls_Dict["CSPT"].XBT_Label = CSPT_XBT_Label1;
            UIControls_Dict["CSPT"].XBT_Price = CSPT_XBT_Label2;
            UIControls_Dict["CSPT"].XBT_Spread = CSPT_XBT_Label3;
            UIControls_Dict["CSPT"].XBT_PriceTT = CSPT_XBT_PriceTT;
            UIControls_Dict["CSPT"].ETH_Label = CSPT_ETH_Label1;
            UIControls_Dict["CSPT"].ETH_Price = CSPT_ETH_Label2;
            UIControls_Dict["CSPT"].ETH_Spread = CSPT_ETH_Label3;
            UIControls_Dict["CSPT"].ETH_PriceTT = CSPT_ETH_PriceTT;
            UIControls_Dict["CSPT"].DOGE_Label = CSPT_DOGE_Label1;
            UIControls_Dict["CSPT"].DOGE_Price = CSPT_DOGE_Label2;
            UIControls_Dict["CSPT"].DOGE_Spread = CSPT_DOGE_Label3;
            UIControls_Dict["CSPT"].DOGE_PriceTT = CSPT_DOGE_PriceTT;
            UIControls_Dict["CSPT"].LTC_Label = CSPT_LTC_Label1;
            UIControls_Dict["CSPT"].LTC_Price = CSPT_LTC_Label2;
            UIControls_Dict["CSPT"].LTC_Spread = CSPT_LTC_Label3;
            UIControls_Dict["CSPT"].LTC_PriceTT = CSPT_LTC_PriceTT;
            UIControls_Dict["CSPT"].AvgPrice_BuySell = CSPT_BuySellComboBox;
            UIControls_Dict["CSPT"].AvgPrice_NumCoins = CSPT_NumCoinsTextBox;
            UIControls_Dict["CSPT"].AvgPrice_Crypto = CSPT_CryptoComboBox;
            UIControls_Dict["CSPT"].AvgPrice = CSPT_AvgPrice_Label;

            foreach (KeyValuePair<string, UIControls> uic in UIControls_Dict) {
                uic.Value.CreateControlDictionaries();  // builds the internal dictionaries so the controls themselves can be iterated over
                uic.Value.AvgPrice_BuySell.SelectedIndex = 0;  // force all the buy/sell drop downs to select buy (so can never be null)
            }
        }

        private Tuple<bool, string> Get(string uri) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = "IRTicker";

            try {
                using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using(Stream stream = response.GetResponseStream())
                using(StreamReader reader = new StreamReader(stream)) {
                    string result = reader.ReadToEnd();

                    // annoying
                    if (result.StartsWith("{\"success\":false")) {
                        return new Tuple<bool, string>(false, "BadRequest");
                    }
                    return new Tuple<bool, string>(true, result);
                }
            }
            catch(WebException e) {
                string returnStr = "";
                if(e.Response != null) {
                    using(WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Debug.Print("Error code: {0}", httpResponse.StatusCode);
                        using(Stream data = response.GetResponseStream())
                        using(var reader = new StreamReader(data)) {
                            returnStr = reader.ReadToEnd();
                            Debug.Print(returnStr);
                            returnStr = httpResponse.StatusCode.ToString();
                        }
                    }
                }
                //MessageBox.Show("Error connecting to URL: " + uri, "Network error", MessageBoxButtons.OK);
                return new Tuple<bool, string>(false, returnStr);
            }
        }

        // takes a website httpsResonse.StatusCode and returns a friendly string
        private string WebsiteError(string errorCode) {
            if (errorCode.Contains("429")) return "Rate limited";
            else if (errorCode.ToUpper().Contains("GATEWAYTIMEOU") || errorCode.ToUpper().Contains("SERVICEUNAVAILABLE")) return "API failure";
            else if (string.IsNullOrEmpty(errorCode)) return "Network error";
            else if (errorCode.ToUpper().Contains("BADREQUEST") || errorCode.ToUpper().Contains("NOTFOUND")) return "Bad request";
            else {
                MessageBox.Show("Unknown failure: " + errorCode, "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "??";
            }
        }

        private void FolderDialogButton_Click(object sender, EventArgs e) {
            DialogResult result = toolbarFolder.ShowDialog();
            if(result == DialogResult.OK) {
                cryptoDir = toolbarFolder.SelectedPath;
                folderDialogTextbox.Text = cryptoDir;
            }
        }

        // this grabs data from the API, creates a MarketSummary object, and pops it in the cryptoPairs dictionary
        private void ParseDCE_IR(string crypto, string fiat) {
            Tuple<bool, string> marketSummary = Get("https://api.independentreserve.com/Public/GetMarketSummary?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);
            if (!marketSummary.Item1) {
                DCEs["IR"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["IR"].NetworkAvailable = false;
            }
            else {
                DCEs["IR"].NetworkAvailable = true;
                DCE.MarketSummary mSummary = JsonConvert.DeserializeObject<DCE.MarketSummary>(marketSummary.Item2);
                DCEs["IR"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
            }
        }

        private void ParseDCE_BTCM(string crypto, string fiat) {
            Tuple<bool, string> marketSummary = Get("https://api.btcmarkets.net/market/" + (crypto == "XBT" ? "BTC" : crypto) + "/" + fiat + "/tick");
            if(!marketSummary.Item1) {
                DCEs["BTCM"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["BTCM"].NetworkAvailable = false;
            }
            else {
                DCEs["BTCM"].NetworkAvailable = true;
                DCE.MarketSummary_BTCM mSummary_BTCM = JsonConvert.DeserializeObject<DCE.MarketSummary_BTCM>(marketSummary.Item2);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();
                mSummary.CurrentHighestBidPrice = mSummary_BTCM.bestBid;
                mSummary.CurrentLowestOfferPrice = mSummary_BTCM.bestAsk;
                mSummary.LastPrice = mSummary_BTCM.lastPrice;
                mSummary.PrimaryCurrencyCode = crypto;
                mSummary.SecondaryCurrencyCode = fiat;
                mSummary.DayVolume = mSummary_BTCM.volume24h;

                DCEs["BTCM"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
            }
        }

        private void ParseDCE_GDAX(string crypto, string fiat) {
            Tuple<bool, string> marketSummary = Get("https://api.gdax.com/products/" + (crypto == "XBT" ? "BTC" : crypto) + "-" + fiat + "/ticker");
            if (!marketSummary.Item1) {
                DCEs["GDAX"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["GDAX"].NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_GDAX mSummary_GDAX = JsonConvert.DeserializeObject<DCE.MarketSummary_GDAX>(marketSummary.Item2);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                if(double.TryParse(mSummary_GDAX.price, out double temp)) {
                    mSummary.LastPrice = temp;
                }
                else Debug.Print("Could not convert GDAX price: " + mSummary_GDAX.price);

                if(double.TryParse(mSummary_GDAX.bid, out temp)) {
                    mSummary.CurrentHighestBidPrice = temp;
                }
                else Debug.Print("Could not convert GDAX price: " + mSummary_GDAX.bid);

                if(double.TryParse(mSummary_GDAX.ask, out temp)) {
                    mSummary.CurrentLowestOfferPrice = temp;
                }
                else Debug.Print("Could not convert GDAX price: " + mSummary_GDAX.ask);

                if(double.TryParse(mSummary_GDAX.volume, out temp)) {
                    mSummary.DayVolume = temp;
                }
                else Debug.Print("Could not convert GDAX price: " + mSummary_GDAX.volume);

                mSummary.PrimaryCurrencyCode = crypto;
                mSummary.SecondaryCurrencyCode = fiat;
                mSummary.CreatedTimestampUTC = mSummary_GDAX.time;

                DCEs["GDAX"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                DCEs["GDAX"].NetworkAvailable = true;
            }
        }

        private void ParseDCE_BFX(string crypto, string fiat) {

            string sendMessage = "{\"event\":\"subscribe\", \"channel\":\"ticker\", \"pair\":\"BTCUSD\"}";

            WebSocket ws = new WebSocket("wss://api.bitfinex.com/ws");

            ws.OnMessage += (sender, e) => 
                Debug.Print("Laputa says: " + e.Data);

            ws.OnOpen += (sender, e) => {
                Debug.Print("ws onopen");
                ws.Send(sendMessage);
            };

            ws.OnError += (sender, e) => {
                Debug.Print("ws onerror");
            };

            ws.OnClose += (sender, e) => {
                //Debug.Print("ws onclose");
            };

            ws.Connect();
            ws.Send(sendMessage);
            

            //ClientWebSocket wss = new WebSocket("wss://api.bitfinex.com/ws");
            






            Tuple<bool, string> marketSummary = Get("https://api.bitfinex.com/v1/pubticker/" + (crypto == "XBT" ? "BTC" : crypto) + fiat);
            if (!marketSummary.Item1) {
                DCEs["BFX"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["BFX"].NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_BFX mSummary_BFX = JsonConvert.DeserializeObject<DCE.MarketSummary_BFX>(marketSummary.Item2);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                if(double.TryParse(mSummary_BFX.last_price, out double temp)) {
                    mSummary.LastPrice = temp;
                }
                else Debug.Print("Could not convert BFX price: " + mSummary_BFX.last_price);

                if(double.TryParse(mSummary_BFX.bid, out temp)) {
                    mSummary.CurrentHighestBidPrice = temp;
                }
                else Debug.Print("Could not convert BFX price: " + mSummary_BFX.bid);

                if(double.TryParse(mSummary_BFX.ask, out temp)) {
                    mSummary.CurrentLowestOfferPrice = temp;
                }
                else Debug.Print("Could not convert BFX price: " + mSummary_BFX.ask);

                if(double.TryParse(mSummary_BFX.volume, out temp)) {
                    mSummary.DayVolume = temp;
                }
                else Debug.Print("Could not convert BFX price: " + mSummary_BFX.volume);

                if(double.TryParse(mSummary_BFX.low, out temp)) {
                    mSummary.DayLowestPrice = temp;
                }
                else Debug.Print("Could not convert BFX price: " + mSummary_BFX.ask);

                if(double.TryParse(mSummary_BFX.high, out temp)) {
                    mSummary.DayHighestPrice = temp;
                }
                else Debug.Print("Could not convert BFX price: " + mSummary_BFX.volume);

                mSummary.PrimaryCurrencyCode = crypto;
                mSummary.SecondaryCurrencyCode = fiat;
                mSummary.CreatedTimestampUTC = mSummary_BFX.timestamp;

                DCEs["BFX"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                DCEs["BFX"].NetworkAvailable = true;
            }
        }

        private void ParseDCE_CSPT(string fiat) {
            Tuple<bool, string> marketSummary = Get("https://www.coinspot.com.au/pubapi/latest");
            if (!marketSummary.Item1) {
                DCEs["CSPT"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["CSPT"].NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_CSPT mSummary_CSPT = JsonConvert.DeserializeObject<DCE.MarketSummary_CSPT>(marketSummary.Item2);

                if (mSummary_CSPT.status != "ok") {
                    DCEs["CSPT"].NetworkAvailable = false;
                    Debug.Print("CoinSpot API was alive, but didn't respond in a health  way, status: " + mSummary_CSPT.status);
                    return;
                }

                DCE.MarketSummary mSummary;

                mSummary_CSPT.prices.CreateCryptoList();
                foreach (DCE.Crypto_CSPT cryptoResponse in mSummary_CSPT.prices.cryptoList) {
                    mSummary = new DCE.MarketSummary();
                    if (double.TryParse(cryptoResponse.last, out double temp)) {
                        mSummary.LastPrice = temp;
                    }
                    else Debug.Print("Could not convert CSPT price: " + cryptoResponse.last);

                    if (double.TryParse(cryptoResponse.bid, out temp)) {
                        mSummary.CurrentHighestBidPrice = temp;
                    }
                    else Debug.Print("Could not convert CSPT price: " + cryptoResponse.bid);

                    if (double.TryParse(cryptoResponse.ask, out temp)) {
                        mSummary.CurrentLowestOfferPrice = temp;
                    }
                    else Debug.Print("Could not convert CSPT price: " + cryptoResponse.ask);

                    mSummary.PrimaryCurrencyCode = cryptoResponse.ticker;
                    mSummary.SecondaryCurrencyCode = fiat;

                    DCEs["CSPT"].CryptoPairsAdd(cryptoResponse.ticker + "-" + fiat, mSummary);
                }
                DCEs["CSPT"].NetworkAvailable = true;
            }
        }

        private void ParseFiat_Fixer(string baseSymbol, string symbols) {
            Tuple<bool, string> fxRates = Get("http://data.fixer.io/api/latest?access_key=3424408462ff94cfa5be1e61d92b6ca4&base=" + baseSymbol + "&symbols=" + symbols);
            if (!fxRates.Item1) {
                //OER_NetworkAvailable = false;
            }
            else {
                //OER_NetworkAvailable = true;
            }
        }

        private void ParseFiat_OER(string baseSymbol, string symbols) {
            Debug.Print("pulling fiat");
            Tuple<bool, string> fxRates = Get("https://openexchangerates.org/api/latest.json?app_id=33bde25e96a6447da4a54d490ca650f2&base=" + baseSymbol + "&symbols=" + symbols + "&prettyprint=false&show_alternative=false");
            if(!fxRates.Item1) {
                OER_NetworkAvailable = false;
            }
            else {
                OER_NetworkAvailable = true;
                fiatRates = JsonConvert.DeserializeObject<CE.MarketSummary_OER>(fxRates.Item2);
            }
        }

        // pulls from the /currencies API
        private string[] GetGDAXCurrencies() {
            Tuple<bool, string> currencies = Get("https://api.gdax.com/currencies");
            if (!currencies.Item1) {
                DCEs["GDAX"].CurrentDCEStatus = WebsiteError(currencies.Item2);
                DCEs["GDAX"].NetworkAvailable = false;
                return new string[] { "", "" };
            }
            else {
                DCEs["GDAX"].NetworkAvailable = true;
                List<DCE.currencies_GDAX> currencyList = JsonConvert.DeserializeObject<List<DCE.currencies_GDAX>>(currencies.Item2);

                StringBuilder fiatCurrencies = new StringBuilder();
                StringBuilder cryptoCurrencies = new StringBuilder();

                foreach (DCE.currencies_GDAX currencyObj in currencyList) {
                    if (double.TryParse(currencyObj.min_size, out double currencyMinSize)) {
                        if (currencyMinSize < 0.01) {  // this is a crypto
                            cryptoCurrencies.Append("\"" + (currencyObj.id == "BTC" ? "XBT" : currencyObj.id) + "\",");
                        }
                        else {  // this is a fiat currency
                            fiatCurrencies.Append("\"" + currencyObj.id + "\",");
                        }
                    }
                    else {
                        Debug.Print("Could not parse GDAX currencies... couldn't turn it into a double? - " + currencyObj.min_size);
                    }
                }
                string cryptoCurrencies2 = cryptoCurrencies.ToString();
                string fiatCurrencies2 = fiatCurrencies.ToString();

                // here we remove the trailing ',' if one exists... which it should.
                if (cryptoCurrencies2.EndsWith(",")) {
                    cryptoCurrencies2 = cryptoCurrencies2.Substring(0, cryptoCurrencies2.Length - 1);
                }
                if (fiatCurrencies2.EndsWith(",")) {
                    fiatCurrencies2 = fiatCurrencies2.Substring(0, fiatCurrencies2.Length - 1);
                }
                //Debug.Print("gdax crypto currencies: " + cryptoCurrencies2);
                //Debug.Print("gdax fiat currencies: " + fiatCurrencies2);
                return new string[] { cryptoCurrencies2, fiatCurrencies2 };
            }
        }

        private void GetGDAXProducts() {
            Tuple<bool, string> products = Get("https://api.gdax.com/products");
            if (!products.Item1) {
                DCEs["GDAX"].CurrentDCEStatus = WebsiteError(products.Item2);
                DCEs["GDAX"].NetworkAvailable = false;
            }
            else {
                List<DCE.products_GDAX> productList = JsonConvert.DeserializeObject<List<DCE.products_GDAX>>(products.Item2);

                Dictionary<string, DCE.products_GDAX> productDictionary = new Dictionary<string, DCE.products_GDAX>();

                // convert the list into a dictionary
                foreach(DCE.products_GDAX prod in productList) {
                    if(prod.id.StartsWith("BTC")) {  // messyyyyyy
                        prod.id = prod.id.Replace("BTC", "XBT");
                    }
                    productDictionary.Add(prod.id, prod);
                }

                DCEs["GDAX"].ExchangeProducts = productDictionary;
            }
        }

        private void GetBFXProducts() {
            Tuple<bool, string> products = Get("https://api.bitfinex.com/v1/symbols_details");
            if (!products.Item1) {
                DCEs["BFX"].CurrentDCEStatus = WebsiteError(products.Item2);
                DCEs["BFX"].NetworkAvailable = false;
            }
            else {
                List<DCE.products_BFX> productList = JsonConvert.DeserializeObject<List<DCE.products_BFX>>(products.Item2);
                Dictionary<string, DCE.products_GDAX> productDictionary = new Dictionary<string, DCE.products_GDAX>();

                // convert the list of producct_BFX's into a dictionary of product_GDAX's
                foreach (DCE.products_BFX prod in productList) {
                    if (prod.pair.StartsWith("btc")) {  // first make btc into xbt
                        prod.pair = prod.pair.Replace("btc", "XBT");
                    }

                    // next we need to do a manual conversion.
                    DCE.products_GDAX prod_gdax = new DCE.products_GDAX();
                    prod_gdax.id = (prod.pair.Insert(3, "-")).ToUpper();  // converts btcusd into BTC-USD
                    //Debug.Print("BFX prod_gdax id " + prod_gdax.id);
                    prod_gdax.base_min_size = prod.minimum_order_size;
                    prod_gdax.base_max_size = prod.maximum_order_size;
                    prod_gdax.margin_enabled = prod.margin;

                    productDictionary.Add(prod_gdax.id, prod_gdax);
                }
                DCEs["BFX"].ExchangeProducts = productDictionary;
                DCEs["BFX"].NetworkAvailable = true;
            }
        }

        private void GetIROrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Get("https://api.independentreserve.com/Public/GetOrderBook?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + DCEs["IR"].CurrentSecondaryCurrency);
            if (orderBookTpl.Item1) {
                DCE.OrderBook orderBook = JsonConvert.DeserializeObject<DCE.OrderBook>(orderBookTpl.Item2);
                DCEs["IR"].orderBooks[crypto + "-" + DCEs["IR"].CurrentSecondaryCurrency] = orderBook;
            }
        }

        private void GetBTCMOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Get("https://api.btcmarkets.net/market/" + (crypto == "XBT" ? "BTC" : crypto) + "/" + DCEs["BTCM"].CurrentSecondaryCurrency + "/orderbook");
            if (orderBookTpl.Item1) { 
                DCE.OrderBook_BTCM orderBook_BTCM = JsonConvert.DeserializeObject<DCE.OrderBook_BTCM>(orderBookTpl.Item2);

                // convert the BTCM order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = orderBook_BTCM.instrument;
                oBook.SecondaryCurrencyCode = orderBook_BTCM.currency;
                DateTimeOffset timeTemp = DateTimeOffset.FromUnixTimeSeconds(orderBook_BTCM.timestamp);  // convert from epoch
                oBook.CreatedTimestampUtc = timeTemp.UtcDateTime;

                foreach (List<double> ask in orderBook_BTCM.asks) {
                    oBook.SellOrders.Add(new DCE.Order("LimitSell", ask[0], ask[1]));
                }

                foreach (List<double> bid in orderBook_BTCM.bids) {
                    oBook.BuyOrders.Add(new DCE.Order("LimitBuy", bid[0], bid[1]));
                }
                DCEs["BTCM"].orderBooks[crypto + "-" + DCEs["BTCM"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void GetGDAXOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Get("https://api.gdax.com/products/" + (crypto == "XBT" ? "BTC" : crypto) + "-" + DCEs["GDAX"].CurrentSecondaryCurrency + "/book?level=" + (EnableGDAXLevel3_CheckBox.Checked ? "3" : "2"));
            if (orderBookTpl.Item1) {
                DCE.OrderBook_GDAX orderBook_GDAX = JsonConvert.DeserializeObject<DCE.OrderBook_GDAX>(orderBookTpl.Item2);

                // convert the GDAX order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = crypto;
                oBook.SecondaryCurrencyCode = DCEs["GDAX"].CurrentSecondaryCurrency;

                foreach (List<string> ask in orderBook_GDAX.asks) {
                    if (double.TryParse(ask[0], out double price)) {
                        if (double.TryParse(ask[1], out double volume)) {
                            oBook.SellOrders.Add(new DCE.Order("LimitSell", price, volume));
                        }
                        else MessageBox.Show("Could not convert GDAX order book ask volume to a double: " + ask[1], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert GDAX order book ask price to a double: " + ask[0], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (List<string> bid in orderBook_GDAX.bids) {
                    if (double.TryParse(bid[0], out double price)) {
                        if (double.TryParse(bid[1], out double volume)) {
                            oBook.BuyOrders.Add(new DCE.Order("LimitBuy", price, volume));
                        }
                        else MessageBox.Show("Could not convert GDAX order book bid volume to a double: " + bid[1], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert GDAX order book bid price to a double: " + bid[0], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DCEs["GDAX"].orderBooks[crypto + "-" + DCEs["GDAX"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void PopulateCryptoComboBox(string dExchange) {
            UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Clear();
            UIControls_Dict[dExchange].AvgPrice_Crypto.ResetText();
            UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Add("");  // add an empty option as the first one so it can be selected when we need to "reset"
            UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;
            foreach (string crypto in DCEs[dExchange].PrimaryCurrencyList) {
                if (DCEs[dExchange].cryptoPairs.ContainsKey(crypto + "-" + DCEs[dExchange].CurrentSecondaryCurrency) && // let's make sure the market summary exists
                    DCEs[dExchange].cryptoPairs[crypto + "-" + DCEs[dExchange].CurrentSecondaryCurrency].LastPrice != -1) {  // and it's not a fake entry
                    UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Add(crypto);
                }
            }
            if (UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Count < 1) {
                MessageBox.Show("Error - no primary currencies from " + dExchange + "?", "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = false;
            }
        }

        private void PollingThread_DoWork(object sender, DoWorkEventArgs e) {
            int loopCount = 0;  // we only want to ask the API about BCH/LTC much less, so we keep track of how many times we loop so we only call the LTC/BCH every 3rd loop maybe
            do {

                if (pollingThread.CancellationPending) {  // need to check here if cancelled.  we don't actulaly need to cancel here, but if we don't, we'll pull all the (correct) data from the
                    e.Cancel = true;  // API, then hit the same "if (pollingthread.cancellationpending)" if block below, and stop and start again anyway. This way we don't make a wasted API call.
                    Debug.Print("Poll cancelled, top!");
                    break;
                }

                pollingThread.ReportProgress(2);  // we need to lock the average price controls here so they user doesn't change them while the data is getting pulled

                Debug.Print("Begin API poll");


                ////// IR ///////
                if(!DCEs["IR"].HasStaticData) {  // only pull the currencies once per session as these are essentially static
                    Tuple<bool, string> primaryCurrencyCodesTpl = Get("https://api.independentreserve.com/Public/GetValidPrimaryCurrencyCodes");
                    if (!primaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(primaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].PrimaryCurrencyCodes = Utilities.TrimEnds(primaryCurrencyCodesTpl.Item2);
                    }

                    Tuple<bool, string> secondaryCurrencyCodesTpl = Get("https://api.independentreserve.com/Public/GetValidSecondaryCurrencyCodes");
                    if (!secondaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(secondaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].SecondaryCurrencyCodes = Utilities.TrimEnds(secondaryCurrencyCodesTpl.Item2);
                    }
                    if (DCEs["IR"].NetworkAvailable) DCEs["IR"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                }

                if (DCEs["IR"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["IR"].PrimaryCurrencyList) {
                        // if there's no crypto selected in the drop down or there's no number of coins entered, then just pull the market summary
                        if (loopCount == 0 || shitCoins.Contains(primaryCode)) {
                            ParseDCE_IR(primaryCode, DCEs["IR"].CurrentSecondaryCurrency);
                        }
                        if (DCEs["IR"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["IR"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                            GetIROrderBook(primaryCode);
                        }
                    }
                }
                else DCEs["IR"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try


                //////// BTC Markets /////////

                if (DCEs["BTCM"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["BTCM"].PrimaryCurrencyList) {
                        if (loopCount == 0 || shitCoins.Contains(primaryCode)) {
                            ParseDCE_BTCM(primaryCode, DCEs["BTCM"].CurrentSecondaryCurrency);
                        }

                        if (DCEs["BTCM"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["BTCM"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                            GetBTCMOrderBook(primaryCode);
                        }
                    }
                }
                else DCEs["BTCM"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try


                //////// GDAX ///////

                if (!DCEs["GDAX"].HasStaticData) {  // should only call this onec per session                    
                    string[] gdax_currencies = GetGDAXCurrencies();
                    GetGDAXProducts();
                    if(DCEs["GDAX"].NetworkAvailable) {
                        DCEs["GDAX"].PrimaryCurrencyCodes = gdax_currencies[0];
                        DCEs["GDAX"].SecondaryCurrencyCodes = gdax_currencies[1];
                        DCEs["GDAX"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                    }
                }

                if (DCEs["GDAX"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["GDAX"].PrimaryCurrencyList) {
                        if (DCEs["GDAX"].ExchangeProducts.ContainsKey(primaryCode + "-" + DCEs["GDAX"].CurrentSecondaryCurrency)) {
                            if (loopCount == 0 || shitCoins.Contains(primaryCode)) {
                                Debug.Print("loopCount = " + loopCount + " primaryCoin = " + (shitCoins.Contains(primaryCode) ? "shitcoin" : "legitCoin"));
                                ParseDCE_GDAX(primaryCode, DCEs["GDAX"].CurrentSecondaryCurrency);
                            }
                        }
                        if (DCEs["GDAX"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["GDAX"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                            Debug.Print("aww yea getting order book");
                            GetGDAXOrderBook(primaryCode);
                        }
                    }
                }
                else DCEs["GDAX"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try


                //////// BitFinex /////////

                if (!DCEs["BFX"].HasStaticData) {
                    GetBFXProducts();
                    if (DCEs["BFX"].NetworkAvailable) DCEs["BFX"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                }

                if (DCEs["BFX"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["BFX"].PrimaryCurrencyList) {
                        if (DCEs["BFX"].ExchangeProducts.ContainsKey(primaryCode + "-" + DCEs["BFX"].CurrentSecondaryCurrency)) {
                            if (loopCount == 0 || shitCoins.Contains(primaryCode)) {
                                ParseDCE_BFX(primaryCode, DCEs["BFX"].CurrentSecondaryCurrency);
                            }
                        }
                    }
                }
                else DCEs["BFX"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try


                //////// CoinSpot ////////
                if (DCEs["CSPT"].NetworkAvailable) {
                    ParseDCE_CSPT("AUD");  // the coinSpot parseDCE method is a bit different, no need to loop through pairs, there is only one endpoint we call and it has all the info on all the pairs.   This might sound cool, but it's actually shit.  CoinSpot API is shittttt
                }
                else DCEs["CSPT"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try


                if (pollingThread.CancellationPending) {  // this will be true if the user has changed the secondary currency.  we need to stop and start again, because it's possible that we have
                    e.Cancel = true;  // pulled the old secondary currency already from the API
                    Debug.Print("Poll cancelled, bottom!");
                    break;
                }


                //////// fiat rates /////////
                if (refreshFiat) {
                    ParseFiat_OER("USD", "AUD,NZD,EUR,USD");  // only run this once per session as we have limited fx API calls.
                    refreshFiat = false;
                }

                // OK we now have all the DCE and fiat rates info loaded.

                pollingThread.ReportProgress(1);

                if(int.TryParse(refreshFrequencyTextbox.Text, out int refreshInt)) {
                    System.Threading.Thread.Sleep(refreshInt * 1000);
                }
                else {
                    System.Windows.MessageBox.Show("couldn't parse the refresh time.. why?  text: " + refreshFrequencyTextbox.Text);
                    System.Threading.Thread.Sleep(10000);
                }

                loopCount++;
                if (loopCount >= shitCoinPollRate) loopCount = 0;  // reset it, it's time we poll the shit coins again

            } while(true);  // polling is lyfe
        }

        private void UpdateLabels(string dExchange) { 
            // first we run through all the labels and reset them.  The label writing process only writes to labes of pairs that exist, so we first need to set them back in case they don't exist
            foreach (KeyValuePair<string, System.Windows.Forms.Label> UICobj in UIControls_Dict[dExchange].Label_Dict) {
                if (UICobj.Key.EndsWith("_Price")) {
                    UICobj.Value.Text = "<no currency pair>";
                }
                else if (UICobj.Key.EndsWith("_Spread")) {
                    UICobj.Value.Text = "";
                }
            }

            // here we run through each available pair in the DCE object, and populate the corresponding labels with the info
            bool avgPriceSet = false;
            foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs[dExchange].cryptoPairs) {
                // i guess we need to filter out the wrong pairs, also don't try and update labels that are -1 (-1 means they're a fake entry)
                if (pairObj.Value.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency && pairObj.Value.LastPrice >= 0) {
                    string formatString = "### ##0.##";
                    if (pairObj.Value.LastPrice < 0.01) formatString = "0.#####";  // some coins are so shit, they're worth less than a cent.  Need different formatting for this.
                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].Text = pairObj.Value.LastPrice.ToString(formatString).Trim();
                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].ForeColor = Utilities.PriceColour(DCEs[dExchange].GetPriceList(pairObj.Key));

                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"].Text = "(Spread: " + pairObj.Value.spread.ToString(formatString) + ")";
                    //Debug.Print("ABOUT TO CHECK ORDER BOOK STUFF:");
                    //Debug.Print("---num coins = " + UIControls_Dict[dExchange].AvgPrice_NumCoins.Text + " avgprice_crypto = " + (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem == null ? "null" : UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem.ToString()));

                    // update average price label
                    if (!String.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text) && UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem != null &&  // need to check this before trying to evaluate it
                        UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem.ToString() == pairObj.Value.PrimaryCurrencyCode) {
                        UIControls_Dict[dExchange].AvgPrice.Text = DetermineAveragePrice(pairObj.Value, dExchange);
                        UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Black;
                        UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                        avgPriceSet = true;
                    }

                    // update tool tips.
                    UIControls_Dict[dExchange].ToolTip_Dict[pairObj.Value.PrimaryCurrencyCode + "_PriceTT"].SetToolTip(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"], "Best bid: " + pairObj.Value.CurrentHighestBidPrice + System.Environment.NewLine + "Best offer: " + pairObj.Value.CurrentLowestOfferPrice);
                }
                else Debug.Print("Pair don't exist, pairObj.Value.SecondaryCurrencyCode: " + pairObj.Value.SecondaryCurrencyCode);  // ETH seems to be the only pair displaying for coinspot.. why?  need to debug.
            }
            if (!avgPriceSet) UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Gray;  // any text there is now a poll old, so gray it out so the user knows it's stale.
            UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = true;  // we disable it if they change the fiat currency as we need to re-populate the crypto combo box first
        }

        private string DetermineAveragePrice(DCE.MarketSummary mSummary, string dExchange) {

            if (!DCEs[dExchange].orderBooks.ContainsKey(mSummary.pair)) return "Failed to pull order book from API";

            // work out the average and set it to the label
            List<DCE.Order> orderSide;
            if (UIControls_Dict[dExchange].AvgPrice_BuySell.SelectedItem.ToString() == "Buy") orderSide = DCEs[dExchange].orderBooks[mSummary.pair].SellOrders;
            else orderSide = DCEs[dExchange].orderBooks[mSummary.pair].BuyOrders;

            if (double.TryParse(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text, out double coins)) {

                double coinCounter = 0;  // we add to this counter until it reaches the numCoinsTextBox (coins) value
                double weightedAverage = 0;
                bool gracefulFinish = false;  // this only gets set to true if the order book has enough coins in it to handle the number of inputted coins.  If it doesn't (ie the foreach completes without us having counted the inputted coins), then we throw a warning message
                foreach (DCE.Order order in orderSide) {
                    coinCounter += order.Volume;
                    if (coinCounter > coins) {  // ok we are on the last value we need to look at.  need to truncate.
                        double usedCoinsInThisOrder = order.Volume - (coinCounter - coins);  // this is how many coins in this order would be required
                        weightedAverage += (usedCoinsInThisOrder / coins) * order.Price;
                        gracefulFinish = true;
                        break;  // we have finished filling the hypothetical order
                    }
                    else {  // this whole order is required
                        weightedAverage += (order.Volume / coins) * order.Price;
                    }
                }
                if (!gracefulFinish) {
                    //MessageBox.Show("You requested " + coins + " coins, but the order book's entire volume (that the API returned to us) had only " + coinCounter + " coins in it.  So, the displayed average price will be less than reality, but you probably fat fingered how many coins?", dExchange + "'s order book too small for that number of coins", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "Order book only has " + coinCounter.ToString("### ###.##").Trim() + " " + mSummary.PrimaryCurrencyCode;
                }
                return "Average price for " + mSummary.PrimaryCurrencyCode + ": " + weightedAverage.ToString("### ###.##").Trim();
            }
            else {
                MessageBox.Show("Could not convert num coins to a number.  how? num = " + UIControls_Dict[dExchange].AvgPrice_NumCoins.Text, "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "";
        }

        private void PollingThread_ReportProgress(object sender, ProgressChangedEventArgs e) {

            int reportType = e.ProgressPercentage;

            // 2 means we just want to lock the average coin price controls so it can't be change while we're pulling the data
            if (reportType == 2) {

                /*if (IR_CryptoComboBox.SelectedIndex != 0 && !string.IsNullOrEmpty(IR_NumCoinsTextBox.Text)) {
                    IR_CryptoComboBox.Enabled = IR_BuySellComboBox.Enabled = IR_NumCoinsTextBox.Enabled = false;
                }

                if (BTCM_CryptoComboBox.SelectedIndex != 0 && !string.IsNullOrEmpty(BTCM_NumCoinsTextBox.Text)) {
                    BTCM_CryptoComboBox.Enabled = BTCM_BuySellComboBox.Enabled = BTCM_NumCoinsTextBox.Enabled = false;
                }

                if (GDAX_CryptoComboBox.SelectedIndex != 0 && !string.IsNullOrEmpty(GDAX_NumCoinsTextBox.Text)) {
                    GDAX_CryptoComboBox.Enabled = GDAX_BuySellComboBox.Enabled = GDAX_NumCoinsTextBox.Enabled = false;
                }

                if (BFX_CryptoComboBox.SelectedIndex != 0 && !string.IsNullOrEmpty(BFX_NumCoinsTextBox.Text)) {
                    BFX_CryptoComboBox.Enabled = BFX_BuySellComboBox.Enabled = BFX_NumCoinsTextBox.Enabled = false;
                }

                if (BFX_CryptoComboBox.SelectedIndex != 0 && !string.IsNullOrEmpty(BFX_NumCoinsTextBox.Text)) {
                    BFX_CryptoComboBox.Enabled = BFX_BuySellComboBox.Enabled = BFX_NumCoinsTextBox.Enabled = false;
                }*/

                foreach (string dExchange in Exchanges) {
                    // if they have filled in the order book controls, then disable them while we work it out
                    if (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex != 0 && !string.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text)) {
                        UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = false;
                        UIControls_Dict[dExchange].AvgPrice_BuySell.Enabled = false;
                        UIControls_Dict[dExchange].AvgPrice_NumCoins.Enabled = false;
                    }
                }

                return;
            }

            LoadingPanel.Visible = false;

            // update the UI
            string secondaryCurrencyCode = "";

            /////////////////////////////////////
            ////////////     IR     /////////////
            /////////////////////////////////////

            // here we iterate through the exchanges and update their group boxes and labels
            foreach (string dExchange in Exchanges) {
                if (DCEs[dExchange].NetworkAvailable) {
                    if (DCEs[dExchange].ChangedSecondaryCurrency) {
                        PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                        DCEs[dExchange].ChangedSecondaryCurrency = false;
                    }

                    secondaryCurrencyCode = DCEs[dExchange].CurrentSecondaryCurrency;
                    UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Black;
                    UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + secondaryCurrencyCode + ")";

                    UpdateLabels(dExchange);
                }
                else APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
            }


            /*if (DCEs["IR"].NetworkAvailable) {
                if (DCEs["IR"].ChangedSecondaryCurrency) {
                    PopulateCryptoComboBox("IR");  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                    DCEs["IR"].ChangedSecondaryCurrency = false;
                }

                secondaryCurrencyCode = DCEs["IR"].CurrentSecondaryCurrency;
                //Debug.Print("secondary currency number is " + DCEs["IR"].chosenSecondaryCurrency + " and this is " + DCEs["IR"].currentSecondaryCurrency);
                IR_GroupBox.ForeColor = Color.Black;
                IR_GroupBox.Text = DCEs["IR"].FriendlyName + " (fiat pair: " + secondaryCurrencyCode + ")";

                UpdateLabels("IR");
            }
            else APIDown(IR_GroupBox, "IR");

            /////////////////////////////////////
            ////////////    BTCM    /////////////
            /////////////////////////////////////
            if (DCEs["BTCM"].NetworkAvailable) {
                if (BTCM_CryptoComboBox.Items.Count <= 1) PopulateCryptoComboBox("BTCM");
                secondaryCurrencyCode = DCEs["BTCM"].CurrentSecondaryCurrency;
                BTCM_GroupBox.ForeColor = Color.Black;
                BTCM_GroupBox.Text = DCEs["BTCM"].FriendlyName + " (fiat pair: " + secondaryCurrencyCode + ")";

                UpdateLabels("BTCM");
            }
            else APIDown(BTCM_GroupBox, "BTCM");

            /////////////////////////////////////
            ////////////    GDAX    /////////////
            /////////////////////////////////////
            if (DCEs["GDAX"].NetworkAvailable) {
                if (DCEs["GDAX"].ChangedSecondaryCurrency) {
                    PopulateCryptoComboBox("GDAX");  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                    DCEs["GDAX"].ChangedSecondaryCurrency = false;
                }
                secondaryCurrencyCode = DCEs["GDAX"].CurrentSecondaryCurrency;
                GDAX_GroupBox.ForeColor = Color.Black;
                GDAX_GroupBox.Text = DCEs["GDAX"].FriendlyName + " (fiat pair: " + secondaryCurrencyCode + ")";

                UpdateLabels("GDAX");
            }
            else APIDown(GDAX_GroupBox, "GDAX");

            /////////////////////////////////////
            ////////////    BFX     /////////////
            /////////////////////////////////////
            if (DCEs["BFX"].NetworkAvailable) {

                if (DCEs["BFX"].ChangedSecondaryCurrency) {
                    PopulateCryptoComboBox("BFX");  // need to re-populate this as it dynamically only populates the combobox with cryptos that the current fiat currency has a pair with
                    DCEs["BFX"].ChangedSecondaryCurrency = false;
                }
                secondaryCurrencyCode = DCEs["BFX"].CurrentSecondaryCurrency;
                BFX_GroupBox.ForeColor = Color.Black;
                BFX_GroupBox.Text = DCEs["BFX"].FriendlyName + " (fiat pair: " + secondaryCurrencyCode + ")";

                UpdateLabels("BFX");
            }
            else APIDown(BFX_GroupBox, "BFX");

            /////////////////////////////////////
            ////////////  CoinSpot  /////////////
            /////////////////////////////////////
            if (DCEs["CSPT"].NetworkAvailable) {

                secondaryCurrencyCode = DCEs["CSPT"].CurrentSecondaryCurrency;
                CSPT_GroupBox.ForeColor = Color.Black;
                BTCM_GroupBox.Text = DCEs["CSPT"].FriendlyName + " (fiat pair: " + secondaryCurrencyCode + ")";

                UpdateLabels("CSPT");
            }
            else APIDown(BFX_GroupBox, "CSPT");*/

            // we have updated all the prices, if the average price controls are disabled, we can enable them now
            IR_CryptoComboBox.Enabled = IR_BuySellComboBox.Enabled = IR_NumCoinsTextBox.Enabled = true;
            BTCM_CryptoComboBox.Enabled = BTCM_BuySellComboBox.Enabled = BTCM_NumCoinsTextBox.Enabled = true;
            GDAX_CryptoComboBox.Enabled = GDAX_BuySellComboBox.Enabled = GDAX_NumCoinsTextBox.Enabled = true;
            //BFX_CryptoComboBox.Enabled = BFX_BuySellComboBox.Enabled = BFX_NumCoinsTextBox.Enabled = true;  // we don't do BFX order book.
            //CSPT_CryptoComboBox.Enabled = CSPT_BuySellComboBox.Enabled = CSPT_NumCoinsTextBox.Enabled = true;  // we don't do CSPT order book.


            if (OER_NetworkAvailable) {
                PrintFiat();  // i outsourced updating the fiat UI we do it when loading for the first time, and also when the user clicks the fiat_groupBox.  it doesn't realy need to be done each poll as we only pull fiat once.. but meh
                if (fiatRefresh_checkBox.Checked) {
                    fiatRefresh_checkBox.Enabled = true;
                    fiatRefresh_checkBox.Text = "Tick to queue an update";
                    fiatRefresh_checkBox.Checked = false;
                }
            }


            if(!DCEs["IR"].NetworkAvailable) return;  // at this point everything else needs IR data.  no point in continuing if there is none.

            string tempXBTdir = cryptoDir + "XBT $" + DCEs["IR"].cryptoPairs["XBT-AUD"].LastPrice;
            string tempETHdir = cryptoDir + "ETH $" + DCEs["IR"].cryptoPairs["ETH-AUD"].LastPrice;
            string tempBCHdir = cryptoDir + "BCH $" + DCEs["IR"].cryptoPairs["BCH-AUD"].LastPrice;
            string tempLTCdir = cryptoDir + "LTC $" + DCEs["IR"].cryptoPairs["LTC-AUD"].LastPrice;

            bool XBTupdated = false;
            bool ETHupdated = false;
            bool BCHupdated = false;
            bool LTCupdated = false;

            try {
                string[] cryptoDirs = Directory.GetDirectories(cryptoDir);

                foreach(string dir in cryptoDirs) {
                    // don't bother if the value hasn't changed
                    if(dir.Equals(tempXBTdir, StringComparison.OrdinalIgnoreCase) ||
                        dir.Equals(tempETHdir, StringComparison.OrdinalIgnoreCase) ||
                        dir.Equals(tempBCHdir, StringComparison.OrdinalIgnoreCase) ||
                        dir.Equals(tempLTCdir, StringComparison.OrdinalIgnoreCase)) {
                        continue;
                    }

                    string dirName = new DirectoryInfo(dir).Name;
                    if(dirName.Length > 5) {  // make sure it's actually a legit crypto dir
                        if(dirName.Substring(0, 5).Equals("XBT $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the XBT folder, let's move it to the new value
                            Directory.Move(dir, tempXBTdir);
                            XBTupdated = true;
                        }
                        if(dirName.Substring(0, 5).Equals("ETH $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the ETH folder, let's move it to the new value
                            Directory.Move(dir, tempETHdir);
                            ETHupdated = true;
                        }
                        if(dirName.Substring(0, 5).Equals("BCH $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the BCH folder, let's move it to the new value
                            Directory.Move(dir, tempBCHdir);
                            BCHupdated = true;
                        }
                        if(dirName.Substring(0, 5).Equals("LTC $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the LTC folder, let's move it to the new value
                            Directory.Move(dir, tempLTCdir);
                            LTCupdated = true;
                        }
                    }
                }

                //  if w didn't come across existing folders to rename, create new ones i guess.
                if(!XBTupdated) { Directory.CreateDirectory(tempXBTdir); }
                if(!ETHupdated) { Directory.CreateDirectory(tempETHdir); }
                if(!BCHupdated) { Directory.CreateDirectory(tempBCHdir); }
                if(!LTCupdated) { Directory.CreateDirectory(tempLTCdir); }
            }
            catch(Exception ex) {
                // TODO: Handle the exception that has been thrown
                MessageBox.Show("renaming folders went bad: " + ex.ToString());
            }
        }

        private void PollingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if(e.Cancelled) {  // if it was cancelled, we start it up again.  The only reason it would be cancelled is if the user chooses a different secondary currency.
                pollingThread.RunWorkerAsync(); // we need to cancel to make sure we haven't already pulled the old currency from the API
            }
        }

        // when they close the app, rename the crypto dirs to blah - old.  this way if they user happens to check the toolbar thing they'll know they're not being updated anymore
        private void IRTicker_Closing(object sender, FormClosingEventArgs e) {
            try {
                string[] cryptoDirs = Directory.GetDirectories(cryptoDir);

                foreach(string dir in cryptoDirs) {
                    string dirName = new DirectoryInfo(dir).Name;

                    if(dirName.Length > 6) {
                        string lastSixCharsDir = dir.Substring(dir.Length - 7);

                        if(lastSixCharsDir.Equals(" - old", StringComparison.OrdinalIgnoreCase)) { continue; }  // if this dir already is suffixed with " - old", then move on
                    }

                    if (dirName.Length > 5) { 
                        if(dirName.Substring(0, 5).Equals("XBT $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the XBT folder, let's move it to the new value
                            Directory.Move(dir, dir + " - old");
                        }
                        if(dirName.Substring(0, 5).Equals("ETH $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the ETH folder, let's move it to the new value
                            Directory.Move(dir, dir + " - old");
                        }
                        if(dirName.Substring(0, 5).Equals("BCH $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the BCH folder, let's move it to the new value
                            Directory.Move(dir, dir + " - old");
                        }
                        if(dirName.Substring(0, 5).Equals("LTC $", StringComparison.OrdinalIgnoreCase)) {  // OK, we found the LTC folder, let's move it to the new value
                            Directory.Move(dir, dir + " - old");
                        }
                    }
                }
            }
            catch(Exception ex) {
                System.Windows.MessageBox.Show("couldn't rename to ' - old': " + ex.ToString());
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e) {
            Settings.Visible = true;
            Main.Visible = false;
        }

        private void SettingsOKButton_Click(object sender, EventArgs e) {
            if(int.TryParse(refreshFrequencyTextbox.Text, out int refreshInt)) {
                if(refreshInt >= minRefreshFrequency) {
                    Main.Visible = true;
                    Settings.Visible = false;
                }
                else {
                    MessageBox.Show("Sorry, minimum is " + minRefreshFrequency.ToString() + " seconds, or you'll piss off APIs and get blocked", "Too low!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    refreshFrequencyTextbox.Text = minRefreshFrequency.ToString();
                }
            }
            else {
                MessageBox.Show("Couldn't parse the refresh time?  weird.. show nick");
            }
        }

        private void GroupBox_Click(string dExchange) {
            DCEs[dExchange].NextSecondaryCurrency();
            UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Gray;
            UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";
            UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;
            UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = false;
            pollingThread.CancelAsync();
            ColourDCETags(Controls, dExchange);
            DCEs[dExchange].ChangedSecondaryCurrency = true;
        }

        private void IR_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["IR"].NetworkAvailable) GroupBox_Click("IR");
        }

        private void GDAX_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["GDAX"].NetworkAvailable) GroupBox_Click("GDAX");
        }

        private void BFX_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["BFX"].NetworkAvailable) GroupBox_Click("BFX");
        }

        private void Fiat_GroupBox_Click(object sender, EventArgs e) {
            fiatIsUSD = !fiatIsUSD;
            PrintFiat();
        }

        // this writes the fiat info to the UI
        private void PrintFiat() {
            if(fiatRates != null) {
                fiat_GroupBox.ForeColor = Color.Black;
                if(!fiatIsUSD) {  // it's USD, but we're changing it to AUD
                    fiat_GroupBox.Text = "Fiat rates (base: AUD" + (fiatInvert_checkBox.Checked ? ", inverted)": ")");
                    if(fiatInvert_checkBox.Checked) {
                        USD_Label2.Text = fiatRates.rates.AUD.ToString("0.#####");
                        NZD_Label2.Text = (1 / ((1 / fiatRates.rates.AUD) * fiatRates.rates.NZD)).ToString("0.#####");
                        EUR_Label2.Text = (1 / ((1 / fiatRates.rates.AUD) * fiatRates.rates.EUR)).ToString("0.#####");
                        AUD_Label2.Text = "1";
                    }
                    else {
                        USD_Label2.Text = (1 / fiatRates.rates.AUD).ToString("0.#####");
                        NZD_Label2.Text = ((1 / fiatRates.rates.AUD) * fiatRates.rates.NZD).ToString("0.#####");
                        EUR_Label2.Text = ((1 / fiatRates.rates.AUD) * fiatRates.rates.EUR).ToString("0.#####");
                        AUD_Label2.Text = "1";
                    }
                }
                else {  // we're changing it to USD base
                    fiat_GroupBox.Text = "Fiat rates (base: USD" + (fiatInvert_checkBox.Checked ? ", inverted)": ")");
                    if(fiatInvert_checkBox.Checked) {
                        AUD_Label2.Text = (1 / fiatRates.rates.AUD).ToString("0.#####");
                        NZD_Label2.Text = (1 / fiatRates.rates.NZD).ToString("0.#####");
                        EUR_Label2.Text = (1 / fiatRates.rates.EUR).ToString("0.#####");
                        USD_Label2.Text = (1 / fiatRates.rates.USD).ToString("0.#####");
                    }
                    else {
                        AUD_Label2.Text = fiatRates.rates.AUD.ToString("0.#####");
                        NZD_Label2.Text = fiatRates.rates.NZD.ToString("0.#####");
                        EUR_Label2.Text = fiatRates.rates.EUR.ToString("0.#####");
                        USD_Label2.Text = fiatRates.rates.USD.ToString("0.#####");
                    }
                }
            }
        }

        private void APIDown(System.Windows.Forms.GroupBox gb, string dExchange) {
            if (DCEs[dExchange].CurrentDCEStatus == "Online") {
                return;
            }
            else {
                gb.Text = DCEs[dExchange].FriendlyName + " - " + DCEs[dExchange].CurrentDCEStatus;
                gb.ForeColor = Color.Gray;

                // because we manually change the colour of the price labels, we need to manually change them here
                ColourDCETags(Controls, dExchange);
            }
        }

        private void ColourDCETags(System.Windows.Forms.Control.ControlCollection controls, string dExchange) {
            foreach (System.Windows.Forms.Control ctrl in controls) {
                if (ctrl.Tag != null)
                    if ((string)ctrl.Tag == dExchange) {
                        ctrl.ForeColor = Color.Gray;
                    }

                if (ctrl.HasChildren)
                    ColourDCETags(ctrl.Controls, dExchange); //Recursively check all children controls as well; ie groupboxes or tabpages
            }
        }

        private void Fiat_checkBox_CheckedChanged(object sender, EventArgs e) {
            if (fiatRefresh_checkBox.Checked) {
                refreshFiat = true;
                fiatRefresh_checkBox.Text = "FX will be updated next poll";
                fiatRefresh_checkBox.Enabled = false;
            }
        }

        private void FiatInvert_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.FiatInverse = fiatInvert_checkBox.Checked;
            Properties.Settings.Default.Save();
            PrintFiat();
        }

        private void RefreshFrequencyTextbox_TextChanged(object sender, EventArgs e) {
            Properties.Settings.Default.RefreshFreq = refreshFrequencyTextbox.Text;
            Properties.Settings.Default.Save();
        }


        // Every time the user changes the average price controls, we need to save the value so the worker thread can use them
        private void IR_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["IR"].CryptoCombo = IR_CryptoComboBox.SelectedItem.ToString();
        }

        private void IR_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["IR"].BuySell = IR_BuySellComboBox.SelectedItem.ToString();
            //Debug.Print("set by")
        }

        private void IR_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["IR"].NumCoinsStr = IR_NumCoinsTextBox.Text; 
        }

        private void BTCM_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BTCM"].BuySell = BTCM_BuySellComboBox.SelectedItem.ToString();
        }

        private void BTCM_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["BTCM"].NumCoinsStr = BTCM_NumCoinsTextBox.Text;
        }

        private void BTCM_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BTCM"].CryptoCombo = BTCM_CryptoComboBox.SelectedItem.ToString();
        }

        private void GDAX_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["GDAX"].BuySell = GDAX_BuySellComboBox.SelectedItem.ToString();
        }

        private void GDAX_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["GDAX"].NumCoinsStr = GDAX_NumCoinsTextBox.Text;
        }

        private void GDAX_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["GDAX"].CryptoCombo = GDAX_CryptoComboBox.SelectedItem.ToString();
            Debug.Print("just set gdax crypto combo: " + UIControls_Dict["GDAX"].AvgPrice_Crypto.SelectedItem.ToString());
        }
    }
}
