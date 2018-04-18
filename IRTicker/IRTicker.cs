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

// todo:
// change the colour of the numbers - red if the price has generally dropped over the last 5 minutes (?), green if it's risen
// maybe align the spread text so it's not dependent on the size of the price text


namespace IRTicker {
    public partial class IRTicker : Form {
        private const String APP_ID = "Nikola.IRTicker";

        private Dictionary<string, DCE> DCEs;  // the master dictionary that holds all the data we pull from all the crypto APIs
        private CE.MarketSummary_OER fiatRates;  // as we only have one fiat source, just hold the market summary class directly
        private bool fiatIsUSD = true;  // to keep track of which fiat base is currently being shown.  I suppose we could inspect the groupbox text field.. but meh
        private bool IR_NetworkAvailable = true;  // these .._NetworkAvaiable vars are to track which APIs are up and which aren't.  If an API isn't up, we don't want to try pull it's data from the DCEs dictionary element, as there'll be nothing there
        private bool BTCM_NetworkAvailable = true;
        private bool GDAX_NetworkAvailable = true;
        private bool BFX_NetworkAvailable = true;
        private bool OER_NetworkAvailable = true;
        private bool refreshFiat = true;


        private string cryptoDir = "";

        public IRTicker() {
            InitializeComponent();

            //cryptoDir = @"C:\ntemp\Crypto\";
            cryptoDir = Path.Combine(System.IO.Path.GetTempPath(), @"Crypto\");
            //System.Windows.MessageBox.Show("crypto path: " + cryptoDir);

            if (!Directory.Exists(cryptoDir)) {
                Directory.CreateDirectory(cryptoDir);
            }

            folderDialogTextbox.Text = cryptoDir;

            // here we need to interrogate the existing cryptoDir, and feed the crypto folder names into the cryptoDirs dictionary
            // if there are no dirs in cryptoDir... what then?

            DCEs = new Dictionary<string, DCE>();

            // seed the DCEs dictionary with empty DCEs for the DCEs we will be interrogating
            DCEs.Add("IR", new DCE());
            DCEs.Add("BTCM", new DCE());
            DCEs.Add("GDAX", new DCE());
            DCEs.Add("BFX", new DCE());

            DCEs["BTCM"].primaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\"";
            DCEs["BTCM"].secondaryCurrencyCodes = "\"AUD\"";

            DCEs["BFX"].primaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\"";
            DCEs["BFX"].secondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            fiatInvert_checkBox.Checked = Properties.Settings.Default.FiatInverse;
            refreshFrequencyTextbox.Text = Properties.Settings.Default.RefreshFreq.ToString(); ;

            pollingThread.RunWorkerAsync();
        }

        public string Get(string uri) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = "IRTicker";

            try {
                using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using(Stream stream = response.GetResponseStream())
                using(StreamReader reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
            catch(WebException e) {
                if(e.Response != null) {
                    using(WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Debug.Print("Error code: {0}", httpResponse.StatusCode);
                        using(Stream data = response.GetResponseStream())
                        using(var reader = new StreamReader(data)) {
                            string text = reader.ReadToEnd();
                            Debug.Print(text);
                        }
                    }
                }
                MessageBox.Show("Error connecting to URL: " + uri, "Network error", MessageBoxButtons.OK);
                return "";
            }
        }

        private void folderDialogButton_Click(object sender, EventArgs e) {
            DialogResult result = toolbarFolder.ShowDialog();
            if(result == DialogResult.OK) {
                cryptoDir = toolbarFolder.SelectedPath;
                folderDialogTextbox.Text = cryptoDir;
            }
        }

        // this grabs data from the API, creates a MarketSummary object, and pops it in the cryptoPairs dictionary
        private void parseDCE_IR(string crypto, string fiat) {
            string marketSummary = Get("https://api.independentreserve.com/Public/GetMarketSummary?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);
            if(string.IsNullOrEmpty(marketSummary)) {
                IR_NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary mSummary = JsonConvert.DeserializeObject<DCE.MarketSummary>(marketSummary);

                if(DCEs["IR"].cryptoPairs.ContainsKey(crypto + "-" + fiat)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                    DCEs["IR"].cryptoPairs.Remove(crypto + "-" + fiat);
                }
                DCEs["IR"].cryptoPairs.Add(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
            }
        }

        private void parseDCE_BTCM(string crypto, string fiat) {
            string marketSummary = Get("https://api.btcmarkets.net/market/" + (crypto == "XBT" ? "BTC" : crypto) + "/" + fiat + "/tick");
            if(string.IsNullOrEmpty(marketSummary)) {
                BTCM_NetworkAvailable = false;
            }
            else {
                BTCM_NetworkAvailable = true;
                DCE.MarketSummary_BTCM mSummary_BTCM = JsonConvert.DeserializeObject<DCE.MarketSummary_BTCM>(marketSummary);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();
                mSummary.CurrentHighestBidPrice = mSummary_BTCM.bestBid;
                mSummary.CurrentLowestOfferPrice = mSummary_BTCM.bestAsk;
                mSummary.LastPrice = mSummary_BTCM.lastPrice;
                mSummary.PrimaryCurrencyCode = crypto;
                mSummary.SecondaryCurrencyCode = fiat;
                mSummary.DayVolume = mSummary_BTCM.volume24h;

                if(DCEs["BTCM"].cryptoPairs.ContainsKey(crypto + "-" + fiat)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                    DCEs["BTCM"].cryptoPairs.Remove(crypto + "-" + fiat);
                }
                DCEs["BTCM"].cryptoPairs.Add(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
            }
        }

        private void parseDCE_GDAX(string crypto, string fiat) {
            string marketSummary = Get("https://api.gdax.com/products/" + (crypto == "XBT" ? "BTC" : crypto) + "-" + fiat + "/ticker");
            if(string.IsNullOrEmpty(marketSummary)) {
                GDAX_NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_GDAX mSummary_GDAX = JsonConvert.DeserializeObject<DCE.MarketSummary_GDAX>(marketSummary);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                double temp = 0;
                if(double.TryParse(mSummary_GDAX.price, out temp)) {
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

                mSummary.CreatedTimestampUTC = mSummary_GDAX.time;

                if(DCEs["GDAX"].cryptoPairs.ContainsKey(crypto + "-" + fiat)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                    DCEs["GDAX"].cryptoPairs.Remove(crypto + "-" + fiat);
                }
                DCEs["GDAX"].cryptoPairs.Add(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
            }
        }

        private void parseDCE_BFX(string crypto, string fiat) {
            string marketSummary = Get("https://api.bitfinex.com/v1/pubticker/" + (crypto == "XBT" ? "BTC" : crypto) + fiat);
            if(string.IsNullOrEmpty(marketSummary)) {
                BFX_NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_BFX mSummary_BFX = JsonConvert.DeserializeObject<DCE.MarketSummary_BFX>(marketSummary);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                double temp = 0;
                if(double.TryParse(mSummary_BFX.last_price, out temp)) {
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

                mSummary.CreatedTimestampUTC = mSummary_BFX.timestamp;

                if(DCEs["BFX"].cryptoPairs.ContainsKey(crypto + "-" + fiat)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                    DCEs["BFX"].cryptoPairs.Remove(crypto + "-" + fiat);
                }
                DCEs["BFX"].cryptoPairs.Add(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
            }
        }

        private void parseFiat_Fixer(string baseSymbol, string symbols) {
            string fxRates = Get("http://data.fixer.io/api/latest?access_key=3424408462ff94cfa5be1e61d92b6ca4&base=" + baseSymbol + "&symbols=" + symbols);
            if (string.IsNullOrEmpty(fxRates)) {
                //OER_NetworkAvailable = false;
            }
            else {
                //OER_NetworkAvailable = true;
            }
        }

        private void parseFiat_OER(string baseSymbol, string symbols) {
            Debug.Print("pulling fiat");
            string fxRates = Get("https://openexchangerates.org/api/latest.json?app_id=33bde25e96a6447da4a54d490ca650f2&base=" + baseSymbol + "&symbols=" + symbols + "&prettyprint=false&show_alternative=false");
            if(string.IsNullOrEmpty(fxRates)) {
                OER_NetworkAvailable = false;
            }
            else {
                OER_NetworkAvailable = true;
                fiatRates = JsonConvert.DeserializeObject<CE.MarketSummary_OER>(fxRates);
            }
        }

        // pulls from the /currencies API
        private string[] getGDAXCurrencies() {
            string currencies = Get("https://api.gdax.com/currencies");
            if(string.IsNullOrEmpty(currencies)) {
                GDAX_NetworkAvailable = false;
                return new string[] { "", "" };
            }
            else {
                GDAX_NetworkAvailable = true;
                List<DCE.currencies_GDAX> currencyList = JsonConvert.DeserializeObject<List<DCE.currencies_GDAX>>(currencies);

                StringBuilder fiatCurrencies = new StringBuilder();
                StringBuilder cryptoCurrencies = new StringBuilder();

                foreach(DCE.currencies_GDAX currencyObj in currencyList) {
                    double currencyMinSize;
                    if(double.TryParse(currencyObj.min_size, out currencyMinSize)) {
                        if(currencyMinSize < 0.01) {  // this is a crypto
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
                if(cryptoCurrencies2.EndsWith(",")) {
                    cryptoCurrencies2 = cryptoCurrencies2.Substring(0, cryptoCurrencies2.Length - 1);
                }
                if(fiatCurrencies2.EndsWith(",")) {
                    fiatCurrencies2 = fiatCurrencies2.Substring(0, fiatCurrencies2.Length - 1);
                }
                //Debug.Print("gdax crypto currencies: " + cryptoCurrencies2);
                //Debug.Print("gdax fiat currencies: " + fiatCurrencies2);
                return new string[] { cryptoCurrencies2, fiatCurrencies2 };
            }
        }

        private void getGDAXProducts() {
            string products = Get("https://api.gdax.com/products");
            if(string.IsNullOrEmpty(products)) {
                GDAX_NetworkAvailable = false;
            }
            else {
                List<DCE.products_GDAX> productList = JsonConvert.DeserializeObject<List<DCE.products_GDAX>>(products);

                Dictionary<string, DCE.products_GDAX> productDictionary = new Dictionary<string, DCE.products_GDAX>();

                // convert the list into a dictionary
                foreach(DCE.products_GDAX prod in productList) {
                    if(prod.id.StartsWith("BTC")) {  // messyyyyyy
                        prod.id = prod.id.Replace("BTC", "XBT");
                    }
                    productDictionary.Add(prod.id, prod);
                }

                DCEs["GDAX"].exchangeProducts = productDictionary;
            }
        }

        private void getBFXProducts() {
            string products = Get("https://api.bitfinex.com/v1/symbols_details");
            if (string.IsNullOrEmpty(products)) {
                BFX_NetworkAvailable = false;
            }
            else {
                List<DCE.products_BFX> productList = JsonConvert.DeserializeObject<List<DCE.products_BFX>>(products);
                Dictionary<string, DCE.products_GDAX> productDictionary = new Dictionary<string, DCE.products_GDAX>();

                // convert the list of producct_BFX's into a dictionary of product_GDAX's
                foreach (DCE.products_BFX prod in productList) {
                    if (prod.pair.StartsWith("btc")) {  // first make btc into xbt
                        prod.pair = prod.pair.Replace("btc", "XBT");
                    }

                    // next we need to do a manual conversion.
                    DCE.products_GDAX prod_gdax = new DCE.products_GDAX();
                    prod_gdax.id = (prod.pair.Insert(3, "-")).ToUpper();  // converts btcusd into BTC-USD
                    Debug.Print("prod_gdax id " + prod_gdax.id);
                    prod_gdax.base_min_size = prod.minimum_order_size;
                    prod_gdax.base_max_size = prod.maximum_order_size;
                    prod_gdax.margin_enabled = prod.margin;

                    productDictionary.Add(prod_gdax.id, prod_gdax);
                }
                DCEs["BFX"].exchangeProducts = productDictionary;
            }
        }

        private void pollingThread_DoWork(object sender, DoWorkEventArgs e) {
            bool firstRun = true;
            do {

                if (pollingThread.CancellationPending) {  // need to check here if cancelled.  we don't actulaly need to cancel here, but if we don't, we'll pull all the (correct) data from the
                    e.Cancel = true;  // API, then hit the same "if (pollingthread.cancellationpending)" if block below, and stop and start again anyway. This way we don't make a wasted API call.
                    Debug.Print("Poll cancelled, top!");
                    break;
                }

                Debug.Print("Begin API poll");
                ////// IR ///////
                if(firstRun) {  // only pull the currencies once per session as these are essentially static
                    string primaryCurrencyCodesStr = Get("https://api.independentreserve.com/Public/GetValidPrimaryCurrencyCodes");
                    if(string.IsNullOrEmpty(primaryCurrencyCodesStr)) {
                        IR_NetworkAvailable = false;
                    }
                    else {
                        IR_NetworkAvailable = true;
                        DCEs["IR"].primaryCurrencyCodes = Utilities.trimEnds(primaryCurrencyCodesStr);
                    }

                    string secondaryCurrencyCodesStr = Get("https://api.independentreserve.com/Public/GetValidSecondaryCurrencyCodes");
                    if(string.IsNullOrEmpty(secondaryCurrencyCodesStr)) {
                        IR_NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].secondaryCurrencyCodes = Utilities.trimEnds(secondaryCurrencyCodesStr);
                    }
                }

                if(IR_NetworkAvailable) {
                    foreach(string primaryCode in DCEs["IR"].primaryCurrencyList) {
                        parseDCE_IR(primaryCode, DCEs["IR"].currentSecondaryCurrency);
                    }
                }

                //////// BTC Markets /////////

                foreach (string primaryCode in DCEs["BTCM"].primaryCurrencyList) {
                    parseDCE_BTCM(primaryCode, DCEs["BTCM"].currentSecondaryCurrency);
                }


                //////// GDAX ///////

                if(firstRun) {  // should only call this onec per session                    
                    string[] gdax_currencies = getGDAXCurrencies();
                    getGDAXProducts();
                    if(GDAX_NetworkAvailable) {
                        DCEs["GDAX"].primaryCurrencyCodes = gdax_currencies[0];
                        DCEs["GDAX"].secondaryCurrencyCodes = gdax_currencies[1];
                    }
                }

                if(GDAX_NetworkAvailable) {
                    foreach(string primaryCode in DCEs["GDAX"].primaryCurrencyList) {
                        if(DCEs["GDAX"].exchangeProducts.ContainsKey(primaryCode + "-" + DCEs["GDAX"].currentSecondaryCurrency)) {
                            parseDCE_GDAX(primaryCode, DCEs["GDAX"].currentSecondaryCurrency);
                        }
                        else {  // ok i guess we need to fake it.
                            DCE.MarketSummary mSummary = new DCE.MarketSummary();
                            mSummary.CreatedTimestampUTC = "";
                            mSummary.CurrentHighestBidPrice = 0;
                            mSummary.CurrentLowestOfferPrice = -1;  // so the spread is -1
                            mSummary.DayAvgPrice = 0;
                            mSummary.DayHighestPrice = 0;
                            mSummary.DayLowestPrice = 0;
                            mSummary.DayVolume = 0;
                            mSummary.DayVolumeInSecondaryCurrency = 0;
                            mSummary.LastPrice = -1;
                            mSummary.PrimaryCurrencyCode = primaryCode;
                            mSummary.SecondaryCurrencyCode = DCEs["GDAX"].currentSecondaryCurrency;

                            if(DCEs["GDAX"].cryptoPairs.ContainsKey(primaryCode + "-" + DCEs["GDAX"].currentSecondaryCurrency)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                                DCEs["GDAX"].cryptoPairs.Remove(primaryCode + "-" + DCEs["GDAX"].currentSecondaryCurrency);
                            }
                            DCEs["GDAX"].cryptoPairs.Add(primaryCode + "-" + DCEs["GDAX"].currentSecondaryCurrency, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                        }
                    }
                }


                //////// BitFinex /////////

                if(firstRun) {
                    getBFXProducts();
                }

                if (BFX_NetworkAvailable) {
                    foreach(string primaryCode in DCEs["BFX"].primaryCurrencyList) {
                        if(DCEs["BFX"].exchangeProducts.ContainsKey(primaryCode + "-" + DCEs["BFX"].currentSecondaryCurrency)) {
                            parseDCE_BFX(primaryCode, DCEs["BFX"].currentSecondaryCurrency);
                        }
                        else {  // ok i guess we need to fake it.
                            DCE.MarketSummary mSummary = new DCE.MarketSummary();
                            mSummary.CreatedTimestampUTC = "";
                            mSummary.CurrentHighestBidPrice = 0;
                            mSummary.CurrentLowestOfferPrice = -1;  // so the spread is -1
                            mSummary.DayAvgPrice = 0;
                            mSummary.DayHighestPrice = 0;
                            mSummary.DayLowestPrice = 0;
                            mSummary.DayVolume = 0;
                            mSummary.DayVolumeInSecondaryCurrency = 0;
                            mSummary.LastPrice = -1;
                            mSummary.PrimaryCurrencyCode = primaryCode;
                            mSummary.SecondaryCurrencyCode = DCEs["BFX"].currentSecondaryCurrency;

                            if(DCEs["BFX"].cryptoPairs.ContainsKey(primaryCode + "-" + DCEs["BFX"].currentSecondaryCurrency)) {  // we need to delete this entry if it exists because we're relpacing it with updated data
                                DCEs["BFX"].cryptoPairs.Remove(primaryCode + "-" + DCEs["BFX"].currentSecondaryCurrency);
                            }
                            DCEs["BFX"].cryptoPairs.Add(primaryCode + "-" + DCEs["BFX"].currentSecondaryCurrency, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                        }
                    }
                }


                if(pollingThread.CancellationPending) {  // this will be true if the user has changed the secondary currency.  we need to stop and start again, because it's possible that we have
                    e.Cancel = true;  // pulled the old secondary currency already from the API
                    Debug.Print("Poll cancelled, bottom!");
                    break;
                }


                //////// fiat rates /////////
                if (refreshFiat) {
                    parseFiat_OER("USD", "AUD,NZD,EUR,USD");  // only run this once per session as we have limited fx API calls.
                    refreshFiat = false;
                }

                // OK we now have all the DCE and fiat rates info loaded.

                pollingThread.ReportProgress(1);

                int refreshInt = 0;
                if(int.TryParse(refreshFrequencyTextbox.Text, out refreshInt)) {
                    System.Threading.Thread.Sleep(refreshInt * 1000);
                }
                else {
                    System.Windows.MessageBox.Show("couldn't parse the refresh time.. why?  text: " + refreshFrequencyTextbox.Text);
                    System.Threading.Thread.Sleep(10000);
                }

                // only set firstRun to false if we have managed to connect to all APIs.  If we haven't, we need to make sure we hit all APIs (as some are skipped when firstRun == false) before setting to false
                /*if (IR_NetworkAvailable && BTCM_NetworkAvailable && GDAX_NetworkAvailable)*/ firstRun = false;
                // some API calls we only do once, but without that data we cannot continue.  the second we have API issues, we need to keep calling these "call once" APIs until the API is back up and running, this way when it does come back we can be sure we have this vital data
                if(!IR_NetworkAvailable || !BTCM_NetworkAvailable || !GDAX_NetworkAvailable || !BFX_NetworkAvailable) firstRun = true;  // only problem with bunching all the APIs together like this: if one fails but another is fine, we'll be forever calling the good APIs "call once" end point.  this is wasteful and may get us blocked for being spammy

            } while(true);  // we never stop polling..
        }

        private void pollingThread_ReportProgress(object sender, ProgressChangedEventArgs e) {

            LoadingPanel.Visible = false;

            // update the UI
            string secondaryCurrencyCode = "";
            if(IR_NetworkAvailable) {
                secondaryCurrencyCode = DCEs["IR"].currentSecondaryCurrency;
                //Debug.Print("secondary currency number is " + DCEs["IR"].chosenSecondaryCurrency + " and this is " + DCEs["IR"].currentSecondaryCurrency);
                IR_GroupBox.ForeColor = Color.Black;
                IR_GroupBox.Text = "Independent Reserve (fiat pair: " + secondaryCurrencyCode + ")";
                IR_XBT_Label2.Text = DCEs["IR"].cryptoPairs["XBT-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["IR"].cryptoPairs["XBT-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                IR_ETH_Label2.Text = DCEs["IR"].cryptoPairs["ETH-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["IR"].cryptoPairs["ETH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                IR_BCH_Label2.Text = DCEs["IR"].cryptoPairs["BCH-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["IR"].cryptoPairs["BCH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                IR_LTC_Label2.Text = DCEs["IR"].cryptoPairs["LTC-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["IR"].cryptoPairs["LTC-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
            }
            else APIDown(IR_GroupBox);

            if(BTCM_NetworkAvailable) {
                secondaryCurrencyCode = DCEs["BTCM"].currentSecondaryCurrency;
                BTCM_GroupBox.ForeColor = Color.Black;
                BTCM_GroupBox.Text = "BTC Markets (fiat pair: " + secondaryCurrencyCode + ")";
                BTCM_XBT_Label2.Text = DCEs["BTCM"].cryptoPairs["XBT-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["BTCM"].cryptoPairs["XBT-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                BTCM_ETH_Label2.Text = DCEs["BTCM"].cryptoPairs["ETH-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["BTCM"].cryptoPairs["ETH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                BTCM_BCH_Label2.Text = DCEs["BTCM"].cryptoPairs["BCH-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["BTCM"].cryptoPairs["BCH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                BTCM_LTC_Label2.Text = DCEs["BTCM"].cryptoPairs["LTC-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["BTCM"].cryptoPairs["LTC-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                BTCM_XRP_Label2.Text = DCEs["BTCM"].cryptoPairs["XRP-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["BTCM"].cryptoPairs["XRP-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
            }

            if(GDAX_NetworkAvailable) {
                secondaryCurrencyCode = DCEs["GDAX"].currentSecondaryCurrency;
                GDAX_GroupBox.ForeColor = Color.Black;
                GDAX_GroupBox.Text = "GDAX (fiat pair: " + secondaryCurrencyCode + ")";

                // GDAX doesn't have all pairs, so we need to check first.  if we didn't do these ifs, the label would just be "-1" which is also fine.. but i already did the coding so might as well leave it
                if(DCEs["GDAX"].exchangeProducts.ContainsKey("XBT-" + secondaryCurrencyCode)) {
                    GDAX_XBT_Label2.Text = DCEs["GDAX"].cryptoPairs["XBT-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["GDAX"].cryptoPairs["XBT-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    GDAX_XBT_Label2.Text = "<no currency pair>";
                }

                if(DCEs["GDAX"].exchangeProducts.ContainsKey("ETH-" + secondaryCurrencyCode)) {
                    GDAX_ETH_Label2.Text = DCEs["GDAX"].cryptoPairs["ETH-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["GDAX"].cryptoPairs["ETH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    GDAX_ETH_Label2.Text = "<no currency pair>";
                }

                if(DCEs["GDAX"].exchangeProducts.ContainsKey("BCH-" + secondaryCurrencyCode)) {
                    GDAX_BCH_Label2.Text = DCEs["GDAX"].cryptoPairs["BCH-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["GDAX"].cryptoPairs["BCH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    GDAX_BCH_Label2.Text = "<no currency pair>";
                }

                if(DCEs["GDAX"].exchangeProducts.ContainsKey("LTC-" + secondaryCurrencyCode)) {
                    GDAX_LTC_Label2.Text = DCEs["GDAX"].cryptoPairs["LTC-" + secondaryCurrencyCode].LastPrice.ToString() + " (Spread: " + DCEs["GDAX"].cryptoPairs["LTC-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    GDAX_LTC_Label2.Text = "<no currency pair>";
                }
            }

            if(BFX_NetworkAvailable) {
                secondaryCurrencyCode = DCEs["BFX"].currentSecondaryCurrency;
                BFX_GroupBox.ForeColor = Color.Black;
                BFX_GroupBox.Text = "BitFinex (fiat pair: " + secondaryCurrencyCode + ")";

                // GDAX doesn't have all pairs, so we need to check first.  if we didn't do these ifs, the label would just be "-1" which is also fine.. but i already did the coding so might as well leave it
                if(DCEs["BFX"].exchangeProducts.ContainsKey("XBT-" + secondaryCurrencyCode)) {
                    BFX_XBT_Label2.Text = DCEs["BFX"].cryptoPairs["XBT-" + secondaryCurrencyCode].LastPrice.ToString("0.##") + " (Spread: " + DCEs["BFX"].cryptoPairs["XBT-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    BFX_XBT_Label2.Text = "<no currency pair>";
                }

                if(DCEs["BFX"].exchangeProducts.ContainsKey("ETH-" + secondaryCurrencyCode)) {
                    BFX_ETH_Label2.Text = DCEs["BFX"].cryptoPairs["ETH-" + secondaryCurrencyCode].LastPrice.ToString("0.##") + " (Spread: " + DCEs["BFX"].cryptoPairs["ETH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    BFX_ETH_Label2.Text = "<no currency pair>";
                }

                if(DCEs["BFX"].exchangeProducts.ContainsKey("BCH-" + secondaryCurrencyCode)) {
                    BFX_BCH_Label2.Text = DCEs["BFX"].cryptoPairs["BCH-" + secondaryCurrencyCode].LastPrice.ToString("0.##") + " (Spread: " + DCEs["BFX"].cryptoPairs["BCH-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    BFX_BCH_Label2.Text = "<no currency pair>";
                }

                if(DCEs["BFX"].exchangeProducts.ContainsKey("LTC-" + secondaryCurrencyCode)) {
                    BFX_LTC_Label2.Text = DCEs["BFX"].cryptoPairs["LTC-" + secondaryCurrencyCode].LastPrice.ToString("0.##") + " (Spread: " + DCEs["BFX"].cryptoPairs["LTC-" + secondaryCurrencyCode].spread.ToString("0.##") + ")";
                }
                else {
                    BFX_LTC_Label2.Text = "<no currency pair>";
                }
            }

            if (OER_NetworkAvailable) {
                printFiat();  // i outsourced updating the fiat UI we do it when loading for the first time, and also when the user clicks the fiat_groupBox.  it doesn't realy need to be done each poll as we only pull fiat once.. but meh
                if (fiatRefresh_checkBox.Checked) {
                    fiatRefresh_checkBox.Enabled = true;
                    fiatRefresh_checkBox.Text = "Tick to queue an update";
                    fiatRefresh_checkBox.Checked = false;
                }
            }


            if(!IR_NetworkAvailable) return;  // at this point everything else needs IR data.  no point in continuing if there is none.

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
                    //Debug.Print("dir: " + dir);
                    //Debug.Print("XBT: " + tempXBTdir);
                    //Debug.Print("ETH: " + tempETHdir);
                    //Debug.Print("BCH: " + tempBCHdir);
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

        private void pollingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if(e.Cancelled) {  // if it was cancelled, we start it up again.  The only reason it would be cancelled is if the user chooses a different secondary currency.
                pollingThread.RunWorkerAsync(); // we need to cancel to make sure we haven't already pulled the old currency from the API
            }
        }

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
            int refreshInt = 0;
            if(int.TryParse(refreshFrequencyTextbox.Text, out refreshInt)) {
                if(refreshInt >= 10) {
                    Main.Visible = true;
                    Settings.Visible = false;
                }
                else {
                    MessageBox.Show("Sorry, minimum is 10 seconds, or you'll piss off APIs and get blocked", "Too low!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    refreshFrequencyTextbox.Text = "10";
                }
            }
            else {
                MessageBox.Show("Couldn't parse the refresh time?  weird.. show nick");
            }
        }

        private void IR_GroupBox_Click(object sender, EventArgs e) {
            //Debug.Print("secondary currency is: " + DCEs["IR"].chosenSecondaryCurrency);
            DCEs["IR"].nextSecondaryCurrency();
            //Debug.Print("secondary currency is now " + DCEs["IR"].chosenSecondaryCurrency);

            IR_GroupBox.ForeColor = Color.Gray;
            IR_GroupBox.Text = "Independent Reserve (fiat pair: " + DCEs["IR"].currentSecondaryCurrency + ")";
            pollingThread.CancelAsync();
        }

        private void GDAX_GroupBox_Click(object sender, EventArgs e) {
            //Debug.Print("secondary currency is: " + DCEs["GDAX"].chosenSecondaryCurrency);
            DCEs["GDAX"].nextSecondaryCurrency();
            //Debug.Print("secondary currency is now " + DCEs["GDAX"].chosenSecondaryCurrency);

            GDAX_GroupBox.ForeColor = Color.Gray;
            GDAX_GroupBox.Text = "GDAX (fiat pair: " + DCEs["GDAX"].currentSecondaryCurrency + ")";
            pollingThread.CancelAsync();
        }

        private void BFX_GroupBox_Click(object sender, EventArgs e) {
            DCEs["BFX"].nextSecondaryCurrency();

            BFX_GroupBox.ForeColor = Color.Gray;
            BFX_GroupBox.Text = "BitFinex (fiat pair: " + DCEs["BFX"].currentSecondaryCurrency + ")";
            pollingThread.CancelAsync();
        }

        private void fiat_GroupBox_Click(object sender, EventArgs e) {
            fiatIsUSD = !fiatIsUSD;
            printFiat();
        }

        // this writes the fiat info to the UI
        private void printFiat() {
            if(fiatRates != null) {
                fiat_GroupBox.ForeColor = Color.Black;
                if(!fiatIsUSD) {  // it's USD, but we're changing it to AUD
                    fiat_GroupBox.Text = "Fiat rates (base: AUD)";
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
                    fiat_GroupBox.Text = "Fiat rates (base: USD)";
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

        private void APIDown(System.Windows.Forms.GroupBox gb) {
            if(!gb.Text.EndsWith("API down")) {
                gb.Text = gb.Text + " - API down";
                gb.ForeColor = Color.Gray;
            }
        }

        private void fiat_checkBox_CheckedChanged(object sender, EventArgs e) {
            if (fiatRefresh_checkBox.Checked) {
                refreshFiat = true;
                fiatRefresh_checkBox.Text = "FX will be updated next poll";
                fiatRefresh_checkBox.Enabled = false;
            }
        }

        private void fiatInvert_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.FiatInverse = fiatInvert_checkBox.Checked;
            Properties.Settings.Default.Save();
            printFiat();
        }

        private void refreshFrequencyTextbox_TextChanged(object sender, EventArgs e) {
            Properties.Settings.Default.RefreshFreq = refreshFrequencyTextbox.Text;
            Properties.Settings.Default.Save();
        }
    }
}
