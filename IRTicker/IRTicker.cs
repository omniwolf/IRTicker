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
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;


// todo:


namespace IRTicker {
    public partial class IRTicker : Form {
        private const String APP_ID = "Nikola.IRTicker";
        private const int minRefreshFrequency = 10;

        private Dictionary<string, DCE> DCEs;  // the master dictionary that holds all the data we pull from all the crypto APIs
        private CE.MarketSummary_OER fiatRates;  // as we only have one fiat source, just hold the market summary class directly
        private bool fiatIsUSD = true;  // to keep track of which fiat base is currently being shown.  I suppose we could inspect the groupbox text field.. but meh
        private bool OER_NetworkAvailable = true;  // should move this into the CE class i guess
        private bool refreshFiat = true;  // this too?
        private Dictionary<string, UIControls> UIControls_Dict;
        private List<string> Exchanges;
        private string cryptoDir = "";
        private List<string> shitCoins = new List<string>() { "BCH", "LTC", "XRP", "DOGE" };  // we don't poll the shit coins as often to help with rate limiting
        private int shitCoinPollRate = 3; // this is how many polls we loop before we call shit coin APIs.  eg 3 means we only poll the shit coins once every 3 polls.
        private WebSocketsConnect wSocketConnect;

        public ConcurrentDictionary<string, SpreadGraph> SpreadGraph_Dict = new ConcurrentDictionary<string, SpreadGraph>();  // needs to be public because it gets accessed from the graphs object

        public IRTicker() {
            InitializeComponent();

            if (refreshFrequencyTextbox.Text == "1") refreshFrequencyTextbox.Text = minRefreshFrequency.ToString();  // design time default is 1, we set to our actual min

            if (string.IsNullOrEmpty(Properties.Settings.Default.ToolbarFolder)) {
                cryptoDir = Path.Combine(System.IO.Path.GetTempPath(), @"Crypto\");
            }
            else {
                cryptoDir = Properties.Settings.Default.ToolbarFolder;
            }


            if (!Directory.Exists(cryptoDir)) {
                Directory.CreateDirectory(cryptoDir);
            }

            Exchanges = new List<string> {
                { "IR" },
                { "BTCM" },
                { "GDAX" },
                { "BFX" },
                { "CSPT" }
            };

            DCEs = new Dictionary<string, DCE> {

                // seed the DCEs dictionary with empty DCEs for the DCEs we will be interrogating
                { "IR", new DCE("IR", "Independent Reserve") },
                { "BTCM", new DCE("BTCM", "BTC Markets") },
                { "GDAX", new DCE("GDAX", "Coinbase Pro") },
                { "BFX", new DCE("BFX", "BitFinex") },
                { "CSPT", new DCE("CSPT", "CoinSpot") }
            };

            // BTCM, BFX, and CSPT have no APIs that let you download the currency pairs, so just set them manually
            DCEs["BTCM"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\"";
            DCEs["BTCM"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["BTCM"].HasStaticData = true;  // we don't poll for static data, so just say we have it.

            DCEs["BFX"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\"";
            DCEs["BFX"].SecondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            DCEs["CSPT"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"DOGE\",\"LTC\"";
            DCEs["CSPT"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["CSPT"].HasStaticData = true;  // we don't poll for static data, so just say we have it.

            wSocketConnect = new WebSocketsConnect(DCEs, pollingThread);

            InitialiseUIControls();

            // if they have somehow set it below 20 secs, force back to 20.
            if (int.TryParse(Properties.Settings.Default.RefreshFreq, out int freq)) {
                if (freq < minRefreshFrequency) Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            }
            else Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            Properties.Settings.Default.Save();

            fiatInvert_checkBox.Checked = Properties.Settings.Default.FiatInverse;
            refreshFrequencyTextbox.Text = Properties.Settings.Default.RefreshFreq.ToString();
            folderDialogTextbox.Text = cryptoDir;
            EnableGDAXLevel3_CheckBox.Checked = Properties.Settings.Default.FullGDAXOB;
            ExportFull_Checkbox.Checked = Properties.Settings.Default.ExportFull;
            ExportSummarised_Checkbox.Checked = Properties.Settings.Default.ExportSummarised;

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
            UIControls_Dict["IR"].AvgPriceTT = IR_AvgPriceTT;

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
            UIControls_Dict["BTCM"].AvgPriceTT = BTCM_AvgPriceTT;

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
            UIControls_Dict["GDAX"].AvgPriceTT = GDAX_AvgPriceTT;

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
            UIControls_Dict["BFX"].XRP_Label = BFX_XRP_Label1;
            UIControls_Dict["BFX"].XRP_Price = BFX_XRP_Label2;
            UIControls_Dict["BFX"].XRP_Spread = BFX_XRP_Label3;
            UIControls_Dict["BFX"].XRP_PriceTT = BFX_XRP_PriceTT;
            UIControls_Dict["BFX"].AvgPrice_BuySell = BFX_BuySellComboBox;
            UIControls_Dict["BFX"].AvgPrice_NumCoins = BFX_NumCoinsTextBox;
            UIControls_Dict["BFX"].AvgPrice_Crypto = BFX_CryptoComboBox;
            UIControls_Dict["BFX"].AvgPrice = BFX_AvgPrice_Label;
            UIControls_Dict["BFX"].AvgPriceTT = BFX_AvgPriceTT;

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
            UIControls_Dict["CSPT"].AvgPriceTT = CSPT_AvgPriceTT;

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
            else if (errorCode.ToUpper().Contains("GATEWAYTIMEOUT") || errorCode.ToUpper().Contains("SERVICEUNAVAILABLE") || errorCode.ToUpper().Contains("BADGATEWAY")) return "API failure";
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
                folderDialogTextbox.Text = Properties.Settings.Default.ToolbarFolder = cryptoDir;
                Properties.Settings.Default.Save();
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
                DCEs["IR"].CurrentDCEStatus = "Online";
            }
        }

        private void ParseDCE_BTCM(string crypto, string fiat) {
            Tuple<bool, string> marketSummary = Get("https://api.btcmarkets.net/market/" + (crypto == "XBT" ? "BTC" : crypto) + "/" + fiat + "/tick");
            if(!marketSummary.Item1) {
                DCEs["BTCM"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["BTCM"].NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_BTCM mSummary_BTCM = JsonConvert.DeserializeObject<DCE.MarketSummary_BTCM>(marketSummary.Item2);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();
                mSummary.CurrentHighestBidPrice = mSummary_BTCM.bestBid;
                mSummary.CurrentLowestOfferPrice = mSummary_BTCM.bestAsk;
                mSummary.LastPrice = mSummary_BTCM.lastPrice;
                mSummary.PrimaryCurrencyCode = crypto;
                mSummary.SecondaryCurrencyCode = fiat;
                mSummary.DayVolume = mSummary_BTCM.volume24h;

                DCEs["BTCM"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs

                DCEs["BTCM"].CurrentDCEStatus = "Online";
                DCEs["BTCM"].NetworkAvailable = true;
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
                    Debug.Print("CoinSpot API was alive, but didn't respond in a healthy way, status: " + mSummary_CSPT.status);
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
                DCEs["CSPT"].CurrentDCEStatus = "Online";
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
            Tuple<bool, string> currencies = Get("https://api.pro.coinbase.com/currencies");
            if (!currencies.Item1) {
                DCEs["GDAX"].CurrentDCEStatus = WebsiteError(currencies.Item2);
                DCEs["GDAX"].NetworkAvailable = false;
                return new string[] { "", "" };
            }
            else {
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
                DCEs["GDAX"].NetworkAvailable = true;
                DCEs["GDAX"].CurrentDCEStatus = "Online";
                return new string[] { cryptoCurrencies2, fiatCurrencies2 };
            }
        }

        private void GetGDAXProducts() {
            Tuple<bool, string> products = Get("https://api.pro.coinbase.com/products");
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

                DCEs["GDAX"].HasStaticData = true;
                DCEs["GDAX"].CurrentDCEStatus = "Online";
            }
        }

        private void GetBFXProducts() {

            Tuple<bool, string> products = Get("https://api.bitfinex.com/v1/symbols_details");
            if (!products.Item1) {
                DCEs["BFX"].CurrentDCEStatus = WebsiteError(products.Item2);
                //DCEs["BFX"].NetworkAvailable = false;
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
                //DCEs["BFX"].NetworkAvailable = true;
                DCEs["BFX"].HasStaticData = true;
                DCEs["BFX"].CurrentDCEStatus = "Online";
                SubscribeTickerSocket("BFX");
            }
        }

        public void SubscribeTickerSocket(string dExchange) {
            // subscribe to all the pairs
            foreach (string secondaryCode in DCEs[dExchange].SecondaryCurrencyList) {
                foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                    if (DCEs[dExchange].ExchangeProducts.ContainsKey(primaryCode + "-" + secondaryCode)) {
                        wSocketConnect.WebSocket_Subscribe(dExchange, primaryCode, secondaryCode);
                    }
                }
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
            Tuple<bool, string> orderBookTpl = Get("https://api.pro.coinbase.com/products/" + (crypto == "XBT" ? "BTC" : crypto) + "-" + DCEs["GDAX"].CurrentSecondaryCurrency + "/book?level=" + (EnableGDAXLevel3_CheckBox.Checked ? "3" : "2"));
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

        private void GetBFXOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Get("https://api.bitfinex.com/v1/book/" + (crypto == "XBT" ? "BTC" : crypto) + DCEs["BFX"].CurrentSecondaryCurrency + "?limit_bids=200&limit_asks=200");
            if (orderBookTpl.Item1) {
                DCE.OrderBook_BFX orderBook_BFX = JsonConvert.DeserializeObject<DCE.OrderBook_BFX>(orderBookTpl.Item2);

                // convert the  order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = crypto;
                oBook.SecondaryCurrencyCode = DCEs["BFX"].CurrentSecondaryCurrency;

                foreach (DCE.BidAsk_BFX ask in orderBook_BFX.asks) {
                    if (double.TryParse(ask.price, out double price)) {
                        if (double.TryParse(ask.amount, out double volume)) {
                            oBook.SellOrders.Add(new DCE.Order("LimitSell", price, volume));
                        }
                        else MessageBox.Show("Could not convert BFX order book ask volume to a double: " + ask.amount, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert BFX order book ask price to a double: " + ask.price, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (DCE.BidAsk_BFX bid in orderBook_BFX.bids) {
                    if (double.TryParse(bid.price, out double price)) {
                        if (double.TryParse(bid.amount, out double volume)) {
                            oBook.BuyOrders.Add(new DCE.Order("LimitBuy", price, volume));
                        }
                        else MessageBox.Show("Could not convert BFX order book bid volume to a double: " + bid.amount, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert BFX order book bid price to a double: " + bid.price, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DCEs["BFX"].orderBooks[crypto + "-" + DCEs["BFX"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void PopulateCryptoComboBox(string dExchange) {
            UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Clear();
            UIControls_Dict[dExchange].AvgPrice_Crypto.ResetText();
            UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Add("");  // add an empty option as the first one so it can be selected when we need to "reset"
            UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;

            foreach (string pair in DCEs[dExchange].UsablePairs()) {
                Tuple<string, string> splitPair = Utilities.SplitPair(pair);  // splits "XBT-AUD" into a tuple ("XBT","AUD")
                if (splitPair.Item2 == DCEs[dExchange].CurrentSecondaryCurrency) {
                    UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Add(splitPair.Item1);
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

                //Debug.Print("Begin API poll");


                ////// IR ///////
                if(!DCEs["IR"].HasStaticData) {  // only pull the currencies once per session as these are essentially static
                    Tuple<bool, string> primaryCurrencyCodesTpl = Get("https://api.independentreserve.com/Public/GetValidPrimaryCurrencyCodes");
                    if (!primaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(primaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].CurrentDCEStatus = "Online";
                        DCEs["IR"].PrimaryCurrencyCodes = Utilities.TrimEnds(primaryCurrencyCodesTpl.Item2);
                    }

                    Tuple<bool, string> secondaryCurrencyCodesTpl = Get("https://api.independentreserve.com/Public/GetValidSecondaryCurrencyCodes");
                    if (!secondaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(secondaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].CurrentDCEStatus = "Online";
                        DCEs["IR"].SecondaryCurrencyCodes = Utilities.TrimEnds(secondaryCurrencyCodesTpl.Item2);
                    }
                    if (DCEs["IR"].NetworkAvailable) {
                        DCEs["IR"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                        Dictionary<string, DCE.products_GDAX> productDictionary_IR = new Dictionary<string, DCE.products_GDAX>();
                        foreach (string crypto in DCEs["IR"].PrimaryCurrencyList) {
                            foreach (string fiat in DCEs["IR"].SecondaryCurrencyList) {
                                productDictionary_IR.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                            }
                        }
                        DCEs["IR"].ExchangeProducts = productDictionary_IR;
                    }
                }

                if (DCEs["IR"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["IR"].PrimaryCurrencyList) {
                        // if there's no crypto selected in the drop down or there's no number of coins entered, then just pull the market summary
                        if (loopCount == 0 || !shitCoins.Contains(primaryCode)) {
                            ParseDCE_IR(primaryCode, DCEs["IR"].CurrentSecondaryCurrency);
                        }
                        if (DCEs["IR"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["IR"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                            GetIROrderBook(primaryCode);
                        }
                    }
                }
                else DCEs["IR"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try


                //////// BTC Markets /////////

                Dictionary<string, DCE.products_GDAX> productDictionary_BTCM = new Dictionary<string, DCE.products_GDAX>();
                foreach (string crypto in DCEs["BTCM"].PrimaryCurrencyList) {
                    foreach (string fiat in DCEs["BTCM"].SecondaryCurrencyList) {
                        productDictionary_BTCM.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                    }
                }
                DCEs["BTCM"].ExchangeProducts = productDictionary_BTCM;

                if (DCEs["BTCM"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["BTCM"].PrimaryCurrencyList) {
                        if (loopCount == 0 || !shitCoins.Contains(primaryCode)) {
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
                    string[] gdax_currencies = GetGDAXCurrencies();  // this is dangerous.  Currently GDAX only supports the cryptos we show, but if it suddenly supports a new one we don't, then it'll be part of the primaryCurrencyList.  
                    GetGDAXProducts();
                    if (DCEs["GDAX"].HasStaticData) {
                        DCEs["GDAX"].PrimaryCurrencyCodes = gdax_currencies[0];
                        DCEs["GDAX"].SecondaryCurrencyCodes = gdax_currencies[1];
                        Debug.Print("calling gdax sockets sub");
                        SubscribeTickerSocket("GDAX");
                        pollingThread.ReportProgress(44);
                    }
                }
                else DCEs["GDAX"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try

                if (loopCount == 0) {
                    if (wSocketConnect.IsSocketAlive("BFX")) { } //Debug.Print("GDAX");
                    else {
                        Debug.Print("BFX ded, reconnecting");
                        wSocketConnect.WebSocket_Reconnect("BFX");
                    }
                    if (wSocketConnect.IsSocketAlive("GDAX")) { } //Debug.Print("GDAX");
                    else {
                        Debug.Print("GDAX ded, reconnecting");
                        wSocketConnect.WebSocket_Reconnect("GDAX");
                    }
                }


                //////// BitFinex /////////

                if (!DCEs["BFX"].HasStaticData) {
                    GetBFXProducts();  // pulls bfx pairs, and starts the websockets connection
                    pollingThread.ReportProgress(54);  // populate crypto drop down
                    //if (DCEs["BFX"].NetworkAvailable) DCEs["BFX"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                }


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

                if (DCEs["GDAX"].CryptoCombo != "" && !string.IsNullOrEmpty(DCEs["GDAX"].NumCoinsStr) && DCEs["GDAX"].HasStaticData) {  // we have a crypto selected and coins entered, let's get the order book for them
                    pollingThread.ReportProgress(2);  // OK let's lock the fields down
                    GetGDAXOrderBook(DCEs["GDAX"].CryptoCombo);
                    pollingThread.ReportProgress(43);  // display order book
                }

                if (DCEs["BFX"].CryptoCombo != "" && !string.IsNullOrEmpty(DCEs["BFX"].NumCoinsStr) && DCEs["BFX"].HasStaticData) {  // we have a crypto selected and coins entered, let's get the order book for them
                    pollingThread.ReportProgress(2);  // OK let's lock the fields down
                    GetBFXOrderBook(DCEs["BFX"].CryptoCombo);
                    pollingThread.ReportProgress(53);  // display order book
                }

                if (int.TryParse(refreshFrequencyTextbox.Text, out int refreshInt)) {
                    System.Threading.Thread.Sleep(refreshInt * 1000);
                }
                else {
                    System.Windows.MessageBox.Show("couldn't parse the refresh time.. why?  text: " + refreshFrequencyTextbox.Text);
                    System.Threading.Thread.Sleep(10000);
                }

                loopCount++;
                if (loopCount >= shitCoinPollRate) {
                    loopCount = 0;  // reset it, it's time we poll the shit coins again
                    if (Properties.Settings.Default.ExportFull) WriteSpreadHistory();  // OK it's been 30 secs, let's write what we have
                    if (Properties.Settings.Default.ExportSummarised) WriteSpreadHistoryCompressed();
                }

            } while(true);  // polling is lyfe
        }

        private void UpdateLabels(string dExchange) {
            // get the copy of the cryptoPairs dictionary.  this is an expensive operation, so do it up here before we reset the labels
            Dictionary<string, DCE.MarketSummary> cPairs = DCEs[dExchange].GetCryptoPairs();

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
            //bool avgPriceSet = false;
            foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in cPairs) {
                // i guess we need to filter out the wrong pairs, also don't try and update labels that are -1 (-1 means they're a fake entry)
                if (pairObj.Value.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency && pairObj.Value.LastPrice >= 0) {
                    string formatString = "### ##0.##";
                    if (pairObj.Value.LastPrice < 0.01 || pairObj.Value.spread < 0.01) formatString = "0.#####";  // some coins are so shit, they're worth less than a cent.  Need different formatting for this.
                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].Text = pairObj.Value.LastPrice.ToString(formatString).Trim();
                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].ForeColor = Utilities.PriceColour(DCEs[dExchange].GetPriceList(pairObj.Key));

                    // if there's a colour, make the font bold.  otherwise not bold.
                    if (UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].ForeColor != Color.Black) {
                        UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].Font = new Font(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].Font, FontStyle.Bold);
                    }
                    else {
                        UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].Font = new Font(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"].Font, FontStyle.Regular);
                    }

                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"].Text = "(Spread: " + pairObj.Value.spread.ToString(formatString) + ")";

                    // update tool tips.
                    UIControls_Dict[dExchange].ToolTip_Dict[pairObj.Value.PrimaryCurrencyCode + "_PriceTT"].SetToolTip(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"], "Best bid: " + pairObj.Value.CurrentHighestBidPrice + System.Environment.NewLine + "Best offer: " + pairObj.Value.CurrentLowestOfferPrice);
                }
                else Debug.Print("Pair don't exist, pairObj.Value.SecondaryCurrencyCode: " + pairObj.Value.SecondaryCurrencyCode);
            }

            if (!String.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text) && UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex != 0) {  // need to check this before trying to evaluate it
                UIControls_Dict[dExchange].AvgPrice.Text = DetermineAveragePrice(DCEs[dExchange].CryptoCombo, DCEs[dExchange].CurrentSecondaryCurrency, dExchange);
                UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
            }
            else UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Gray;  // any text there is now a poll old, so gray it out so the user knows it's stale.

            UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = true;  // we disable it if they change the fiat currency as we need to re-populate the crypto combo box first
        }

        // Updates labels, but just a specific pair (used for websockets because we get each pair separartely)
        private void UpdateLabels_Pair(string dExchange, string crypto, string fiat) {
            // first we reset the labels.  The label writing process only writes to labels of pairs that exist, so we first need to set them back in case they don't exist

            DCE.MarketSummary mSummary = DCEs[dExchange].GetCryptoPairs()[crypto + "-" + fiat];

            // i guess we need to filter out the wrong pairs, also don't try and update labels that are -1 (-1 means they're a fake entry)
            if (mSummary.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency && mSummary.LastPrice >= 0) {

                // we have a legit pair we're about to update.  if the groupBox is grey, let's black it.
                UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Black;
                UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";

                string formatString = "### ##0.##";
                if (mSummary.LastPrice < 0.01 || mSummary.spread < 0.01) formatString = "0.#####";  // some coins are so shit, they're worth less than a cent.  Need different formatting for this.  ORRR the spread is so amazingly small we need more decimal places
                UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].Text = mSummary.LastPrice.ToString(formatString).Trim();
                UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].ForeColor = Utilities.PriceColour(DCEs[dExchange].GetPriceList(crypto + "-" + fiat));

                // if there's a colour, make the font bold.  otherwise not bold.
                if (UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].ForeColor != Color.Black) {
                    UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].Font = new Font(UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].Font, FontStyle.Bold);
                }
                else {
                    UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].Font = new Font(UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"].Font, FontStyle.Regular);
                }

                UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"].Text = "(Spread: " + mSummary.spread.ToString(formatString) + ")";
                //Debug.Print("ABOUT TO CHECK ORDER BOOK STUFF:");
                //Debug.Print("---num coins = " + UIControls_Dict[dExchange].AvgPrice_NumCoins.Text + " avgprice_crypto = " + (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem == null ? "null" : UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem.ToString()));

                if (DCEs[dExchange].ChangedSecondaryCurrency) { 
                    PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                    DCEs[dExchange].ChangedSecondaryCurrency = false;
                }

                // update tool tips.
                UIControls_Dict[dExchange].ToolTip_Dict[mSummary.PrimaryCurrencyCode + "_PriceTT"].SetToolTip(UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"], "Best bid: " + mSummary.CurrentHighestBidPrice + System.Environment.NewLine + "Best offer: " + mSummary.CurrentLowestOfferPrice);
            }
            else Debug.Print("Pair2 don't exist, pairObj.Value.SecondaryCurrencyCode: " + mSummary.SecondaryCurrencyCode);
        }

        private string DetermineAveragePrice(string crypto, string fiat, string dExchange) {

            string pair = crypto + "-" + fiat;

            if (!DCEs[dExchange].orderBooks.ContainsKey(pair)) return "Failed to pull order book from API";

            // work out the average and set it to the label
            List<DCE.Order> orderSide;
            if (UIControls_Dict[dExchange].AvgPrice_BuySell.SelectedItem.ToString() == "Buy") orderSide = DCEs[dExchange].orderBooks[pair].SellOrders;
            else orderSide = DCEs[dExchange].orderBooks[pair].BuyOrders;

            if (double.TryParse(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text, out double coins)) {

                double coinCounter = 0;  // we add to this counter until it reaches the numCoinsTextBox (coins) value
                double weightedAverage = 0;
                double totalCost = 0;
                int orderCount = 0;
                bool gracefulFinish = false;  // this only gets set to true if the order book has enough coins in it to handle the number of inputted coins.  If it doesn't (ie the foreach completes without us having counted the inputted coins), then we throw a warning message
                foreach (DCE.Order order in orderSide) {
                    orderCount++;
                    coinCounter += order.Volume;
                    if (coinCounter > coins) {  // ok we are on the last value we need to look at.  need to truncate.
                        double usedCoinsInThisOrder = order.Volume - (coinCounter - coins);  // this is how many coins in this order would be required
                        totalCost += usedCoinsInThisOrder * order.Price;
                        weightedAverage += (usedCoinsInThisOrder / coins) * order.Price;
                        gracefulFinish = true;
                        string tTip = "Max price paid: " + order.Price.ToString("### ##0.##") + System.Environment.NewLine + "Orders required to fill: " + orderCount + System.Environment.NewLine + "Total fiat cost: " + totalCost.ToString("### ### ##0.##");
                        UIControls_Dict[dExchange].AvgPriceTT.SetToolTip(UIControls_Dict[dExchange].AvgPrice, tTip);
                        break;  // we have finished filling the hypothetical order
                    }
                    else {  // this whole order is required
                        weightedAverage += (order.Volume / coins) * order.Price;
                        totalCost += order.Volume * order.Price;
                    }
                }
                if (!gracefulFinish) {
                    //MessageBox.Show("You requested " + coins + " coins, but the order book's entire volume (that the API returned to us) had only " + coinCounter + " coins in it.  So, the displayed average price will be less than reality, but you probably fat fingered how many coins?", dExchange + "'s order book too small for that number of coins", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "Order book only has " + coinCounter.ToString("### ###.##").Trim() + " " + crypto;
                }
                DCEs[dExchange].RemoveOrderBook(pair);  // need to remove once we've used - there's the possibility that the next orderbook API pull fails, then the code will just use the existing order book
                return "Average price for " + crypto + ": " + weightedAverage.ToString("### ###.##").Trim();
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
                foreach (string dExchange in Exchanges) {
                    // if they have filled in the order book controls, then disable them while we work it out
                    if (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex > 0 && !string.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text)) {
                        UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = false;
                        UIControls_Dict[dExchange].AvgPrice_BuySell.Enabled = false;
                        UIControls_Dict[dExchange].AvgPrice_NumCoins.Enabled = false;
                    }
                }
                return;
            }

            else if (reportType == 41) {  // update GDAX
                DCE.MarketSummary mSummary = (DCE.MarketSummary)e.UserState;
                UpdateLabels_Pair("GDAX", mSummary.PrimaryCurrencyCode, mSummary.SecondaryCurrencyCode);
                return;
            }
            else if (reportType == 42) {  // 42 is error in the response or API.   either way, we disconnect and start again.
                string dExchange = (string)e.UserState;
                APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
                return;
            }
            else if (reportType == 43) {  // 43 is order book stuff for gdax
                UIControls_Dict["GDAX"].AvgPrice.Text = DetermineAveragePrice(DCEs["GDAX"].CryptoCombo, DCEs["GDAX"].CurrentSecondaryCurrency, "GDAX");
                UIControls_Dict["GDAX"].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict["GDAX"].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                GDAX_CryptoComboBox.Enabled = GDAX_BuySellComboBox.Enabled = GDAX_NumCoinsTextBox.Enabled = true;
                return;
            }
            else if (reportType == 44) {  // should only be called once per session - if we don't do this the crypto combo box is empty until we change secondary currencies
                PopulateCryptoComboBox("GDAX");
                return;
            }
            if (reportType == 51) {  // 51 is BFX update labels
                DCE.MarketSummary mSummary = (DCE.MarketSummary)e.UserState;
                UpdateLabels_Pair("BFX", mSummary.PrimaryCurrencyCode, mSummary.SecondaryCurrencyCode);
                return;
            }
            else if (reportType == 53) {  // 53 is order book stuff for bfx
                UIControls_Dict["BFX"].AvgPrice.Text = DetermineAveragePrice(DCEs["BFX"].CryptoCombo, DCEs["BFX"].CurrentSecondaryCurrency, "BFX");
                UIControls_Dict["BFX"].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict["BFX"].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                BFX_CryptoComboBox.Enabled = BFX_BuySellComboBox.Enabled = BFX_NumCoinsTextBox.Enabled = true;
                return;
            }
            else if (reportType == 54) {  // should only be called once per session - if we don't do this the crypto combo box is empty until we change secondary currencies
                PopulateCryptoComboBox("BFX");
                return;
            }


            // update the UI

            // here we iterate through the exchanges and update their group boxes and labels

            foreach (string dExchange in Exchanges) {
                if (dExchange == "BFX" || dExchange == "GDAX") {  // for sockets we don't update labels or change colours.  that happens on demand.
                    if (DCEs[dExchange].HasStaticData && DCEs[dExchange].ChangedSecondaryCurrency) {
                        PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                        DCEs[dExchange].ChangedSecondaryCurrency = false;
                    }
                    else UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Gray;  // any text there is now a poll old, so gray it out so the user knows it's stale.

                    if (!DCEs[dExchange].HasStaticData) APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
                    continue;
                }
                if (DCEs[dExchange].NetworkAvailable) {
                    if (DCEs[dExchange].ChangedSecondaryCurrency) {
                        PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                        DCEs[dExchange].ChangedSecondaryCurrency = false;
                    }

                    UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Black;
                    UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";

                    UpdateLabels(dExchange);
                }
                else APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
            }

            // we have updated all the prices, if the average price controls are disabled, we can enable them now
            IR_CryptoComboBox.Enabled = IR_BuySellComboBox.Enabled = IR_NumCoinsTextBox.Enabled = true;
            BTCM_CryptoComboBox.Enabled = BTCM_BuySellComboBox.Enabled = BTCM_NumCoinsTextBox.Enabled = true;
            //CSPT_CryptoComboBox.Enabled = CSPT_BuySellComboBox.Enabled = CSPT_NumCoinsTextBox.Enabled = true;  // we don't do CSPT order book.


            if (OER_NetworkAvailable) {
                PrintFiat();  // i outsourced updating the fiat UI we do it when loading for the first time, and also when the user clicks the fiat_groupBox.  it doesn't realy need to be done each poll as we only pull fiat once.. but meh
                if (fiatRefresh_checkBox.Checked) {
                    fiatRefresh_checkBox.Enabled = true;
                    fiatRefresh_checkBox.Text = "Tick to queue an update";
                    fiatRefresh_checkBox.Checked = false;
                }
            }

            LoadingPanel.Visible = false;  // OK, all UI data is written, let's remove the loading panel.

            if (!DCEs["IR"].NetworkAvailable) return;  // at this point everything else needs IR data.  no point in continuing if there is none.

            Dictionary<string, DCE.MarketSummary> cPairs = DCEs["IR"].GetCryptoPairs();

            string tempXBTdir = cryptoDir + "XBT $" + cPairs["XBT-AUD"].LastPrice;
            string tempETHdir = cryptoDir + "ETH $" + cPairs["ETH-AUD"].LastPrice;
            string tempBCHdir = cryptoDir + "BCH $" + cPairs["BCH-AUD"].LastPrice;
            string tempLTCdir = cryptoDir + "LTC $" + cPairs["LTC-AUD"].LastPrice;

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

            foreach (KeyValuePair<string, SpreadGraph> sGraph in SpreadGraph_Dict) sGraph.Value.Redraw();  // update the graph
        }

        private void PollingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if(e.Cancelled) {  // if it was cancelled, we start it up again.  The only reason it would be cancelled is if the user chooses a different secondary currency.
                pollingThread.RunWorkerAsync(); // we need to cancel to make sure we haven't already pulled the old currency from the API
            }
            else {
                Debug.Print("POLL stopped!! why?? " + e.Result + " " + e.Error + " " + e.ToString());
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

            if (UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Count > 0) UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset the selection to blank

            Utilities.ColourDCETags(Controls, dExchange);
            DCEs[dExchange].ChangedSecondaryCurrency = true;
        }

        private void IR_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["IR"].NetworkAvailable) {
                GroupBox_Click("IR");
                UIControls_Dict["IR"].AvgPrice_Crypto.Enabled = false;  // disable it while we work out the new available cryptos
                pollingThread.CancelAsync();  // cancel the poll so we don't try and download data for the wrong base currency
            }
        }

        private void GDAX_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["GDAX"].HasStaticData) {
                GroupBox_Click("GDAX");
                UIControls_Dict["GDAX"].dExchange_GB.ForeColor = Color.Black;
                // need to force a label update, otherwise they'll stay grey <no currency> until the next update comes through
                foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs["GDAX"].GetCryptoPairs()) {
                    UpdateLabels_Pair("GDAX", pairObj.Value.PrimaryCurrencyCode, pairObj.Value.SecondaryCurrencyCode);
                }

                // we have a new fiat currency.  if there are any pairs not available, update the UI.
                foreach (string crypto in DCEs["GDAX"].PrimaryCurrencyList) {
                    if (!DCEs["GDAX"].ExchangeProducts.ContainsKey(crypto + "-" + DCEs["GDAX"].CurrentSecondaryCurrency)) {
                        UIControls_Dict["GDAX"].Label_Dict[crypto + "_Price"].Text = "<no currency pair>";
                        UIControls_Dict["GDAX"].Label_Dict[crypto + "_Spread"].Text = "";
                    }
                }
                PopulateCryptoComboBox("GDAX");
            }
        }

        private void BFX_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["BFX"].HasStaticData) {
                GroupBox_Click("BFX");
                UIControls_Dict["BFX"].dExchange_GB.ForeColor = Color.Black;
                // need to force a label update, otherwise they'll stay grey <no currency> until the next update comes through
                foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs["BFX"].GetCryptoPairs()) {
                    UpdateLabels_Pair("BFX", pairObj.Value.PrimaryCurrencyCode, pairObj.Value.SecondaryCurrencyCode);
                }

                // we have a new fiat currency.  if there are any pairs not available, update the UI.
                foreach (string crypto in DCEs["BFX"].PrimaryCurrencyList) {
                    if (!DCEs["BFX"].ExchangeProducts.ContainsKey(crypto + "-" + DCEs["BFX"].CurrentSecondaryCurrency)) {
                        UIControls_Dict["BFX"].Label_Dict[crypto + "_Price"].Text = "<no currency pair>";
                        UIControls_Dict["BFX"].Label_Dict[crypto + "_Spread"].Text = "";
                    }
                }
                PopulateCryptoComboBox("BFX");
            }
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
                Utilities.ColourDCETags(Controls, dExchange);
            }
        }

        private void WriteSpreadHistory() {

            StreamWriter dataWriter;
            string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\IRTicker spread history data\\";
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);  // create it if it doesn't exist

            try {
                foreach (KeyValuePair<string, DCE> Exchange in DCEs) {  // spin through all the exchanges
                    foreach (KeyValuePair<string, List<DataPoint>> spreadHistory in Exchange.Value.GetSpreadHistoryCSV()) {  // spin through all the pairs of this exchange
                        dataWriter = new StreamWriter(dataFolder + Exchange.Value.CodeName + "-" + spreadHistory.Key + ".csv", append: true);
                        foreach (DataPoint dp in spreadHistory.Value) {  // spin through all the NEW data points in this pair
                            dataWriter.WriteLine(string.Join(",", dp.XValue.ToString(), dp.YValues[0].ToString()));
                        }
                        dataWriter.Close();
                    }
                }
            }
            catch (Exception ex) {
                Debug.Print("Error writing to file: " + ex.ToString());
            }
        }

        // this one only writes the spread as it sees it every 30 seconds or so.. to reduce the CSV file size.
        private void WriteSpreadHistoryCompressed() {

            string baseFolder = "G:\\My Drive\\IR\\IRTicker\\Spread history data\\";
            if (!Directory.Exists(baseFolder)) {
                Debug.Print("Cannot write spread history info - base folder not accessible or doesn't exist");
                return;
            }

            string dataFolder = baseFolder + Environment.UserName + "\\";
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);  // create it if it doesn't exist
            StreamWriter dataWriter;
            try {
                foreach (KeyValuePair<string, DCE> Exchange in DCEs) {  // spin through all the exchanges
                    foreach (KeyValuePair<string, DCE.MarketSummary> spreadHistory in Exchange.Value.GetCryptoPairs()) {  // spin through all the pairs of this exchange
                        dataWriter = new StreamWriter(dataFolder + Exchange.Value.CodeName + "-" + spreadHistory.Key + " - compressed.csv", append: true);
                        dataWriter.WriteLine(string.Join(",", DateTime.Now.ToOADate(), spreadHistory.Value.spread));
                        dataWriter.Close();
                    }
                }
            }
            catch (Exception ex) {
                Debug.Print("Error writing to file: " + ex.ToString());
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
            IR_AvgPrice_Label.Text = "";
        }

        private void IR_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["IR"].NumCoinsStr = IR_NumCoinsTextBox.Text;
            IR_AvgPrice_Label.Text = "";
        }

        private void IR_CryptoComboBox_DropDown(object sender, EventArgs e) {
            IR_AvgPrice_Label.Text = "";
        }

        private void BTCM_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BTCM"].BuySell = BTCM_BuySellComboBox.SelectedItem.ToString();
            BTCM_AvgPrice_Label.Text = "";
        }

        private void BTCM_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["BTCM"].NumCoinsStr = BTCM_NumCoinsTextBox.Text;
            BTCM_AvgPrice_Label.Text = "";
        }

        private void BTCM_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BTCM"].CryptoCombo = BTCM_CryptoComboBox.SelectedItem.ToString();
        }

        private void BTCM_CryptoComboBox_DropDown(object sender, EventArgs e) {
            BTCM_AvgPrice_Label.Text = "";
        }

        private void GDAX_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["GDAX"].BuySell = GDAX_BuySellComboBox.SelectedItem.ToString();
            GDAX_AvgPrice_Label.Text = "";
        }

        private void GDAX_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["GDAX"].NumCoinsStr = GDAX_NumCoinsTextBox.Text;
            GDAX_AvgPrice_Label.Text = "";
        }

        private void GDAX_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["GDAX"].CryptoCombo = GDAX_CryptoComboBox.SelectedItem.ToString();
        }

        private void GDAX_CryptoComboBox_DropDown(object sender, EventArgs e) {
            GDAX_AvgPrice_Label.Text = "";
        }

        private void BFX_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BFX"].BuySell = BFX_BuySellComboBox.SelectedItem.ToString();
            BFX_AvgPrice_Label.Text = "";
        }

        private void BFX_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["BFX"].NumCoinsStr = BFX_NumCoinsTextBox.Text;
            BFX_AvgPrice_Label.Text = "";
        }

        private void BFX_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BFX"].CryptoCombo = BFX_CryptoComboBox.SelectedItem.ToString();
        }

        private void BFX_CryptoComboBox_DropDown(object sender, EventArgs e) {
            BFX_AvgPrice_Label.Text = "";
        }

        private void IR_XBT_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "XBT-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-XBT-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }

        private void GDAX_XBT_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["GDAX"], "XBT-" + DCEs["GDAX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("GDAX-XBT-" + DCEs["GDAX"].CurrentSecondaryCurrency, SGForm);
        }

        private void IR_ETH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "ETH-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-ETH-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }

        private void IR_BCH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "BCH-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-BCH-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }

        private void IR_LTC_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "LTC-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-LTC-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }

        private void BTCM_XBT_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BTCM"], "XBT-" + DCEs["BTCM"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BTCM-XBT-" + DCEs["BTCM"].CurrentSecondaryCurrency, SGForm);
        }

        private void BTCM_ETH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BTCM"], "ETH-" + DCEs["BTCM"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BTCM-ETH-" + DCEs["BTCM"].CurrentSecondaryCurrency, SGForm);
        }

        private void BTCM_BCH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BTCM"], "BCH-" + DCEs["BTCM"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BTCM-BCH-" + DCEs["BTCM"].CurrentSecondaryCurrency, SGForm);
        }

        private void BTCM_LTC_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BTCM"], "LTC-" + DCEs["BTCM"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BTCM-LTC-" + DCEs["BTCM"].CurrentSecondaryCurrency, SGForm);
        }

        private void BTCM_XRP_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BTCM"], "XRP-" + DCEs["BTCM"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BTCM-XRP-" + DCEs["BTCM"].CurrentSecondaryCurrency, SGForm);
        }

        private void GDAX_ETH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["GDAX"], "ETH-" + DCEs["GDAX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("GDAX-ETH-" + DCEs["GDAX"].CurrentSecondaryCurrency, SGForm);
        }

        private void GDAX_BCH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["GDAX"], "BCH-" + DCEs["GDAX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("GDAX-BCH-" + DCEs["GDAX"].CurrentSecondaryCurrency, SGForm);
        }

        private void GDAX_LTC_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["GDAX"], "LTC-" + DCEs["GDAX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("GDAX-LTC-" + DCEs["GDAX"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_XBT_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "XBT-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-XBT-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_ETH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "ETH-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-ETH-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_BCH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "BCH-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-BCH-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_LTC_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "LTC-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-LTC-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_XRP_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "XRP-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-XRP-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void CSPT_XBT_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["CSPT"], "XBT-" + DCEs["CSPT"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("CSPT-XBT-" + DCEs["CSPT"].CurrentSecondaryCurrency, SGForm);
        }

        private void CSPT_ETH_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["CSPT"], "ETH-" + DCEs["CSPT"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("CSPT-ETH-" + DCEs["CSPT"].CurrentSecondaryCurrency, SGForm);
        }

        private void CSPT_DOGE_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["CSPT"], "DOGE-" + DCEs["CSPT"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("CSPT-DOGE-" + DCEs["CSPT"].CurrentSecondaryCurrency, SGForm);
        }

        private void CSPT_LTC_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["CSPT"], "LTC-" + DCEs["CSPT"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("CSPT-LTC-" + DCEs["CSPT"].CurrentSecondaryCurrency, SGForm);
        }

        private void Help_Button_Click(object sender, EventArgs e) {
            Help helpForm = new Help(this);
            helpForm.Show();
            Help_Button.Enabled = false;
        }

        private void ExportFull_Checkbox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.ExportFull = ExportFull_Checkbox.Checked;
            Properties.Settings.Default.Save();
        }

        private void ExportSummarised_Checkbox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.ExportSummarised = ExportSummarised_Checkbox.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
