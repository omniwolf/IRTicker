using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;
using BlinkStickDotNet;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private List<string> shitCoins = new List<string>() { "BCH", "LTC", "XRP", "EOS", "OMG", "ZRX", "XLM", "BAT", "REP", "GNT", "BSV", "USDT" };  // we don't poll the shit coins as often to help with rate limiting
        private int shitCoinPollRate = 3; // this is how many polls we loop before we call shit coin APIs.  eg 3 means we only poll the shit coins once every 3 polls.
        private WebSocketsConnect wSocketConnect;
        private BlinkStick bStick;
        private Slack slackObj = new Slack();
        private DateTime lastCSVWrite = DateTime.Now;  // this holds the time we last saved the CSV file

        public ConcurrentDictionary<string, SpreadGraph> SpreadGraph_Dict = new ConcurrentDictionary<string, SpreadGraph>();  // needs to be public because it gets accessed from the graphs object

        OBview obv = new OBview();

        public IRTicker() {
            InitializeComponent();

            Debug.Print("");
            Debug.Print("----------------");
            Debug.Print("IR TICKER BEGINS");
            Debug.Print("----------------");
            Debug.Print("");

            bStick = BlinkStick.FindFirst();

            if (bStick != null && bStick.OpenDevice()) {
                bStick.Blink("yellow",1,200);
                bStick.Blink("blue",1,200);
                //bStick.Pulse("purple", 1, 300, 50);
                //bStick.Pulse(RgbColor.FromRgb(69, 114, 69), 20, 700, 50);
                //BlinkStickBW.RunWorkerAsync(argument: RgbColor.FromRgb(69, 114, 69));
            }
            else {
                Debug.Print("BlinkStick couldn't be accessed or opened");
            }

            if (refreshFrequencyTextbox.Text == "1") refreshFrequencyTextbox.Text = minRefreshFrequency.ToString();  // design time default is 1, we set to our actual min

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

            DCEs["IR"].CurrencyCombo = "fiat";
            IR_CurrencyBox.SelectedIndex = 1;

            // BTCM, BFX, and CSPT have no APIs that let you download the currency pairs, so just set them manually
            // Actually I'm not sure about the above comment, i think some of them do?  But the main issue is most of them have
            // currencies that we don't want to deal with, so we set the currencies manually here.  IR we want all currencies, so
            // we use the API.  This is probably not really smart, as the UI is static, so when new currencies turn up IR breaks.  meh
            DCEs["BTCM"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\",\"OMG\",\"XLM\",\"BAT\",\"GNT\",\"ETC\",\"BSV\"";
            DCEs["BTCM"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["BTCM"].HasStaticData = false;  // want to set this to false so we run the subscribe code once.

            DCEs["BFX"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\",\"OMG\",\"ZRX\",\"EOS\",\"XLM\",\"BAT\",\"REP\",\"GNT\",\"ETC\",\"BSV\",\"USDT\"";
            DCEs["BFX"].SecondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            DCEs["GDAX"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"ZRX\",\"XRP\",\"XLM\",\"REP\",\"ETC\"";
            DCEs["GDAX"].SecondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            DCEs["CSPT"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"EOS\",\"LTC\",\"XRP\"";
            DCEs["CSPT"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["CSPT"].HasStaticData = true;  // we don't poll for static data, so just say we have it.
            DCEs["CSPT"].socketsAlive = true;  // coinspot has no sockets, so just leave this as true so everything is happy.

            wSocketConnect = new WebSocketsConnect(DCEs, pollingThread);

            InitialiseUIControls();

            // if they have somehow set it below 20 secs, force back to 20... or 10
            if (int.TryParse(Properties.Settings.Default.RefreshFreq, out int freq)) {
                if (freq < minRefreshFrequency) Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            }
            else Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            Properties.Settings.Default.Save();

            refreshFrequencyTextbox.Text = Properties.Settings.Default.RefreshFreq.ToString();
            EnableGDAXLevel3_CheckBox.Checked = Properties.Settings.Default.FullGDAXOB;
            ExportSummarised_Checkbox.Checked = Properties.Settings.Default.ExportSummarised;
            Slack_checkBox.Checked = Properties.Settings.Default.Slack;
            flashForm_checkBox.Checked = Properties.Settings.Default.FlashForm;
            slackToken_textBox.Text = Properties.Settings.Default.SlackToken;
            slackDefaultNameTextBox.Text = Properties.Settings.Default.SlackDefaultName;
            slackNameChangeCheckBox.Checked = Properties.Settings.Default.SlackNameChange;
            OB_checkBox.Checked = Properties.Settings.Default.ShowOB;
            UITimerFreq_maskedTextBox.Text = Properties.Settings.Default.UITimerFreq.ToString();

            if (Slack_checkBox.Checked) {
                slackDefaultNameTextBox.Enabled = true;
                slackNameChangeCheckBox.Enabled = true;
                slackToken_textBox.Enabled = true;
            }
            else {
                slackDefaultNameTextBox.Enabled = false;
                slackNameChangeCheckBox.Enabled = false;
                slackToken_textBox.Enabled = false;
            }

            if (Properties.Settings.Default.ShowOB) obv.Show();

            VersionLabel.Text = "IR Ticker version " + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;

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
            UIControls_Dict["IR"].ETH_Label = IR_ETH_Label1;
            UIControls_Dict["IR"].ETH_Price = IR_ETH_Label2;
            UIControls_Dict["IR"].ETH_Spread = IR_ETH_Label3;
            UIControls_Dict["IR"].BCH_Label = IR_BCH_Label1;
            UIControls_Dict["IR"].BCH_Price = IR_BCH_Label2;
            UIControls_Dict["IR"].BCH_Spread = IR_BCH_Label3;
            UIControls_Dict["IR"].LTC_Label = IR_LTC_Label1;
            UIControls_Dict["IR"].LTC_Price = IR_LTC_Label2;
            UIControls_Dict["IR"].LTC_Spread = IR_LTC_Label3;
            UIControls_Dict["IR"].XRP_Label = IR_XRP_Label1;
            UIControls_Dict["IR"].XRP_Price = IR_XRP_Label2;
            UIControls_Dict["IR"].XRP_Spread = IR_XRP_Label3;
            UIControls_Dict["IR"].OMG_Label = IR_OMG_Label1;
            UIControls_Dict["IR"].OMG_Price = IR_OMG_Label2;
            UIControls_Dict["IR"].OMG_Spread = IR_OMG_Label3;
            UIControls_Dict["IR"].ZRX_Label = IR_ZRX_Label1;
            UIControls_Dict["IR"].ZRX_Price = IR_ZRX_Label2;
            UIControls_Dict["IR"].ZRX_Spread = IR_ZRX_Label3;
            UIControls_Dict["IR"].EOS_Label = IR_EOS_Label1;
            UIControls_Dict["IR"].EOS_Price = IR_EOS_Label2;
            UIControls_Dict["IR"].EOS_Spread = IR_EOS_Label3;
            UIControls_Dict["IR"].XLM_Label = IR_XLM_Label1;
            UIControls_Dict["IR"].XLM_Price = IR_XLM_Label2;
            UIControls_Dict["IR"].XLM_Spread = IR_XLM_Label3;
            UIControls_Dict["IR"].BAT_Label = IR_BAT_Label1;
            UIControls_Dict["IR"].BAT_Price = IR_BAT_Label2;
            UIControls_Dict["IR"].BAT_Spread = IR_BAT_Label3;
            UIControls_Dict["IR"].REP_Label = IR_REP_Label1;
            UIControls_Dict["IR"].REP_Price = IR_REP_Label2;
            UIControls_Dict["IR"].REP_Spread = IR_REP_Label3;
            UIControls_Dict["IR"].GNT_Label = IR_GNT_Label1;
            UIControls_Dict["IR"].GNT_Price = IR_GNT_Label2;
            UIControls_Dict["IR"].GNT_Spread = IR_GNT_Label3;
            UIControls_Dict["IR"].ETC_Label = IR_ETC_Label1;
            UIControls_Dict["IR"].ETC_Price = IR_ETC_Label2;
            UIControls_Dict["IR"].ETC_Spread = IR_ETC_Label3;
            UIControls_Dict["IR"].USDT_Label = IR_USDT_Label1;
            UIControls_Dict["IR"].USDT_Price = IR_USDT_Label2;
            UIControls_Dict["IR"].USDT_Spread = IR_USDT_Label3;
            UIControls_Dict["IR"].BSV_Label = IR_BSV_Label1;
            UIControls_Dict["IR"].BSV_Price = IR_BSV_Label2;
            UIControls_Dict["IR"].BSV_Spread = IR_BSV_Label3;
            UIControls_Dict["IR"].AvgPrice_BuySell = IR_BuySellComboBox;
            UIControls_Dict["IR"].AvgPrice_NumCoins = IR_NumCoinsTextBox;
            UIControls_Dict["IR"].AvgPrice_Crypto = IR_CryptoComboBox;
            UIControls_Dict["IR"].AvgPrice_Currency = IR_CryptoComboBox;
            UIControls_Dict["IR"].AvgPrice = IR_AvgPrice_Label;

            // BTCM
            UIControls_Dict["BTCM"].Name = "BTCM";
            UIControls_Dict["BTCM"].dExchange_GB = BTCM_GroupBox;
            UIControls_Dict["BTCM"].XBT_Label = BTCM_XBT_Label1;
            UIControls_Dict["BTCM"].XBT_Price = BTCM_XBT_Label2;
            UIControls_Dict["BTCM"].XBT_Spread = BTCM_XBT_Label3;
            UIControls_Dict["BTCM"].ETH_Label = BTCM_ETH_Label1;
            UIControls_Dict["BTCM"].ETH_Price = BTCM_ETH_Label2;
            UIControls_Dict["BTCM"].ETH_Spread = BTCM_ETH_Label3;
            UIControls_Dict["BTCM"].BCH_Label = BTCM_BCH_Label1;
            UIControls_Dict["BTCM"].BCH_Price = BTCM_BCH_Label2;
            UIControls_Dict["BTCM"].BCH_Spread = BTCM_BCH_Label3;
            UIControls_Dict["BTCM"].LTC_Label = BTCM_LTC_Label1;
            UIControls_Dict["BTCM"].LTC_Price = BTCM_LTC_Label2;
            UIControls_Dict["BTCM"].LTC_Spread = BTCM_LTC_Label3;
            UIControls_Dict["BTCM"].XRP_Label = BTCM_XRP_Label1;
            UIControls_Dict["BTCM"].XRP_Price = BTCM_XRP_Label2;
            UIControls_Dict["BTCM"].XRP_Spread = BTCM_XRP_Label3;
            UIControls_Dict["BTCM"].OMG_Label = BTCM_OMG_Label1;
            UIControls_Dict["BTCM"].OMG_Price = BTCM_OMG_Label2;
            UIControls_Dict["BTCM"].OMG_Spread = BTCM_OMG_Label3;
            UIControls_Dict["BTCM"].XLM_Label = BTCM_XLM_Label1;
            UIControls_Dict["BTCM"].XLM_Price = BTCM_XLM_Label2;
            UIControls_Dict["BTCM"].XLM_Spread = BTCM_XLM_Label3;
            UIControls_Dict["BTCM"].BAT_Label = BTCM_BAT_Label1;
            UIControls_Dict["BTCM"].BAT_Price = BTCM_BAT_Label2;
            UIControls_Dict["BTCM"].BAT_Spread = BTCM_BAT_Label3;
            UIControls_Dict["BTCM"].GNT_Label = BTCM_GNT_Label1;
            UIControls_Dict["BTCM"].GNT_Price = BTCM_GNT_Label2;
            UIControls_Dict["BTCM"].GNT_Spread = BTCM_GNT_Label3;
            UIControls_Dict["BTCM"].ETC_Label = BTCM_ETC_Label1;
            UIControls_Dict["BTCM"].ETC_Price = BTCM_ETC_Label2;
            UIControls_Dict["BTCM"].ETC_Spread = BTCM_ETC_Label3;
            UIControls_Dict["BTCM"].BSV_Label = BTCM_BSV_Label1;
            UIControls_Dict["BTCM"].BSV_Price = BTCM_BSV_Label2;
            UIControls_Dict["BTCM"].BSV_Spread = BTCM_BSV_Label3;
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
            UIControls_Dict["GDAX"].ETH_Label = GDAX_ETH_Label1;
            UIControls_Dict["GDAX"].ETH_Price = GDAX_ETH_Label2;
            UIControls_Dict["GDAX"].ETH_Spread = GDAX_ETH_Label3;
            UIControls_Dict["GDAX"].BCH_Label = GDAX_BCH_Label1;
            UIControls_Dict["GDAX"].BCH_Price = GDAX_BCH_Label2;
            UIControls_Dict["GDAX"].BCH_Spread = GDAX_BCH_Label3;
            UIControls_Dict["GDAX"].LTC_Label = GDAX_LTC_Label1;
            UIControls_Dict["GDAX"].LTC_Price = GDAX_LTC_Label2;
            UIControls_Dict["GDAX"].LTC_Spread = GDAX_LTC_Label3;
            UIControls_Dict["GDAX"].ZRX_Label = GDAX_ZRX_Label1;
            UIControls_Dict["GDAX"].ZRX_Price = GDAX_ZRX_Label2;
            UIControls_Dict["GDAX"].ZRX_Spread = GDAX_ZRX_Label3;
            UIControls_Dict["GDAX"].XRP_Label = GDAX_XRP_Label1;
            UIControls_Dict["GDAX"].XRP_Price = GDAX_XRP_Label2;
            UIControls_Dict["GDAX"].XRP_Spread = GDAX_XRP_Label3;
            UIControls_Dict["GDAX"].XLM_Label = GDAX_XLM_Label1;
            UIControls_Dict["GDAX"].XLM_Price = GDAX_XLM_Label2;
            UIControls_Dict["GDAX"].XLM_Spread = GDAX_XLM_Label3;
            UIControls_Dict["GDAX"].REP_Label = GDAX_REP_Label1;
            UIControls_Dict["GDAX"].REP_Price = GDAX_REP_Label2;
            UIControls_Dict["GDAX"].REP_Spread = GDAX_REP_Label3;
            UIControls_Dict["GDAX"].ETC_Label = GDAX_ETC_Label1;
            UIControls_Dict["GDAX"].ETC_Price = GDAX_ETC_Label2;
            UIControls_Dict["GDAX"].ETC_Spread = GDAX_ETC_Label3;
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
            UIControls_Dict["BFX"].ETH_Label = BFX_ETH_Label1;
            UIControls_Dict["BFX"].ETH_Price = BFX_ETH_Label2;
            UIControls_Dict["BFX"].ETH_Spread = BFX_ETH_Label3;
            UIControls_Dict["BFX"].BCH_Label = BFX_BCH_Label1;
            UIControls_Dict["BFX"].BCH_Price = BFX_BCH_Label2;
            UIControls_Dict["BFX"].BCH_Spread = BFX_BCH_Label3;
            UIControls_Dict["BFX"].LTC_Label = BFX_LTC_Label1;
            UIControls_Dict["BFX"].LTC_Price = BFX_LTC_Label2;
            UIControls_Dict["BFX"].LTC_Spread = BFX_LTC_Label3;
            UIControls_Dict["BFX"].XRP_Label = BFX_XRP_Label1;
            UIControls_Dict["BFX"].XRP_Price = BFX_XRP_Label2;
            UIControls_Dict["BFX"].XRP_Spread = BFX_XRP_Label3;
            UIControls_Dict["BFX"].OMG_Label = BFX_OMG_Label1;
            UIControls_Dict["BFX"].OMG_Price = BFX_OMG_Label2;
            UIControls_Dict["BFX"].OMG_Spread = BFX_OMG_Label3;
            UIControls_Dict["BFX"].ZRX_Label = BFX_ZRX_Label1;
            UIControls_Dict["BFX"].ZRX_Price = BFX_ZRX_Label2;
            UIControls_Dict["BFX"].ZRX_Spread = BFX_ZRX_Label3;
            UIControls_Dict["BFX"].EOS_Label = BFX_EOS_Label1;
            UIControls_Dict["BFX"].EOS_Price = BFX_EOS_Label2;
            UIControls_Dict["BFX"].EOS_Spread = BFX_EOS_Label3;
            UIControls_Dict["BFX"].XLM_Label = BFX_XLM_Label1;
            UIControls_Dict["BFX"].XLM_Price = BFX_XLM_Label2;
            UIControls_Dict["BFX"].XLM_Spread = BFX_XLM_Label3;
            UIControls_Dict["BFX"].BAT_Label = BFX_BAT_Label1;
            UIControls_Dict["BFX"].BAT_Price = BFX_BAT_Label2;
            UIControls_Dict["BFX"].BAT_Spread = BFX_BAT_Label3;
            UIControls_Dict["BFX"].REP_Label = BFX_REP_Label1;
            UIControls_Dict["BFX"].REP_Price = BFX_REP_Label2;
            UIControls_Dict["BFX"].REP_Spread = BFX_REP_Label3;
            UIControls_Dict["BFX"].GNT_Label = BFX_GNT_Label1;
            UIControls_Dict["BFX"].GNT_Price = BFX_GNT_Label2;
            UIControls_Dict["BFX"].GNT_Spread = BFX_GNT_Label3;
            UIControls_Dict["BFX"].ETC_Label = BFX_ETC_Label1;
            UIControls_Dict["BFX"].ETC_Price = BFX_ETC_Label2;
            UIControls_Dict["BFX"].ETC_Spread = BFX_ETC_Label3;
            UIControls_Dict["BFX"].USDT_Label = BFX_USDT_Label1;
            UIControls_Dict["BFX"].USDT_Price = BFX_USDT_Label2;
            UIControls_Dict["BFX"].USDT_Spread = BFX_USDT_Label3;
            UIControls_Dict["BFX"].BSV_Label = BFX_BSV_Label1;
            UIControls_Dict["BFX"].BSV_Price = BFX_BSV_Label2;
            UIControls_Dict["BFX"].BSV_Spread = BFX_BSV_Label3;
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
            UIControls_Dict["CSPT"].ETH_Label = CSPT_ETH_Label1;
            UIControls_Dict["CSPT"].ETH_Price = CSPT_ETH_Label2;
            UIControls_Dict["CSPT"].ETH_Spread = CSPT_ETH_Label3;
            UIControls_Dict["CSPT"].EOS_Label = CSPT_EOS_Label1;
            UIControls_Dict["CSPT"].EOS_Price = CSPT_EOS_Label2;
            UIControls_Dict["CSPT"].EOS_Spread = CSPT_EOS_Label3;
            UIControls_Dict["CSPT"].LTC_Label = CSPT_LTC_Label1;
            UIControls_Dict["CSPT"].LTC_Price = CSPT_LTC_Label2;
            UIControls_Dict["CSPT"].LTC_Spread = CSPT_LTC_Label3;
            UIControls_Dict["CSPT"].XRP_Label = CSPT_XRP_Label1;
            UIControls_Dict["CSPT"].XRP_Price = CSPT_XRP_Label2;
            UIControls_Dict["CSPT"].XRP_Spread = CSPT_XRP_Label3;

            foreach (KeyValuePair<string, UIControls> uic in UIControls_Dict) {
                uic.Value.CreateControlDictionaries();  // builds the internal dictionaries so the controls themselves can be iterated over
                if (uic.Value.AvgPrice != null) {
                    uic.Value.AvgPrice_BuySell.SelectedIndex = 0;  // force all the buy/sell drop downs to select buy (so can never be null)
                }
            }
        }

        private void setStickColour(decimal IRBTCvol, decimal BTCMBTCvol) {
            //IRBTCvol = 98;
            //BTCMBTCvol = 99;
            if (bStick != null && bStick.OpenDevice()) {
                //RgbColor col = new RgbColor();
                //RgbColor.FromRgb(69, 114, 69);
                if (BlinkStickBW.IsBusy) {
                    BlinkStickBW.CancelAsync();
                    //Debug.Print(DateTime.Now + " -- BS -- cancelling the BlinkStickBW thread...");
                }

                try {
                    if (IRBTCvol > BTCMBTCvol * 2) {
                        if (!BlinkStickBW.IsBusy) {
                            BlinkStickBW.RunWorkerAsync(RgbColor.FromString("#0079FF"));
                            //Debug.Print(DateTime.Now + " -- BS -- started the IR GOOOOOD thread");
                        }
                    }
                    else if (IRBTCvol * 2 < BTCMBTCvol) {
                        if (!BlinkStickBW.IsBusy) {
                            BlinkStickBW.RunWorkerAsync(RgbColor.FromString("#00FF00"));
                            //Debug.Print(DateTime.Now + " -- BS -- started the IR BAAAD thread");
                        }
                    }
                    else if (IRBTCvol > BTCMBTCvol + 5) {
                        //Debug.Print(DateTime.Now + " -- BS -- IR winning");
                        bStick.Morph("#3176BC");
                    }
                    else if ((IRBTCvol <= BTCMBTCvol + 5) && (IRBTCvol >= BTCMBTCvol - 5)) {
                        //Debug.Print(DateTime.Now + " -- BS -- trying to go white");
                        bStick.Morph("#C19E6E");
                        if (!BlinkStickWhite_Thread.IsBusy) BlinkStickWhite_Thread.RunWorkerAsync(RgbColor.FromString((BTCMBTCvol > IRBTCvol ? "#42953A" : "#B6CBE1")));
                    }
                    else if (IRBTCvol < BTCMBTCvol - 5) {
                        //Debug.Print(DateTime.Now + " -- BS -- BTCM is winning");
                        bStick.Morph("#00A607");
                    }
                }
                catch (Exception ex) {
                    Debug.Print(DateTime.Now + " -- BS -- caught an exception: " + ex.Message);
                }
            }
        }

        private void setSlackStatus(decimal IRBTCvol, decimal BTCMBTCvol) {
            // now we set slack stuff
            /*if (IRBTCvol > (BTCMBTCvol + 5)) {  // IR is winning :D
                slackObj.setStatus("", ":large_blue_diamond:", 120);
            }
            else if (BTCMBTCvol > (IRBTCvol + 5)) {  // BTCM is winning :<
                slackObj.setStatus("", ":green_book:", 120);
            }
            else {  // pretty even - white :|
                slackObj.setStatus("", ":white_square:", 120);
            }*/

            string name = "";

            if (Properties.Settings.Default.SlackNameChange && (Properties.Settings.Default.SlackDefaultName != string.Empty)) {

                name = Properties.Settings.Default.SlackDefaultName;
                if (!DCEs["IR"].socketsAlive || !DCEs["IR"].NetworkAvailable || IRBTCvol < 0) {
                    slackObj.setStatus("", ":exclamation:", 120, name + " - IR API down");
                    return;
                }
                else if (!DCEs["BTCM"].socketsAlive || !DCEs["BTCM"].NetworkAvailable || BTCMBTCvol < 0) {
                    slackObj.setStatus("", ":face_with_rolling_eyes:", 120, name + " - BTCM API down");
                    return;
                }

                string tempName = UIControls_Dict["IR"].Label_Dict["XBT_Price"].Text;
                tempName = tempName.Substring(0, tempName.Length - 3);  // remove decimal places from the price
                name += " - AUD " + tempName;
            }
            Debug.Print("slack name is: " + name);

            if (IRBTCvol < 0 || BTCMBTCvol < 0) {
                slackObj.setStatus("", ":question:", 120, name);
            }
            else if (IRBTCvol > BTCMBTCvol * 2) {
                slackObj.setStatus("", ":danbizan:", 120, name);
            }
            else if (IRBTCvol * 2 < BTCMBTCvol) {
                slackObj.setStatus("", ":sob:", 120, name);
            }
            else if (IRBTCvol > BTCMBTCvol + 5) {
                slackObj.setStatus("", ":sunglasses:", 120, name);
            }
            else if ((IRBTCvol <= BTCMBTCvol + 5) && (IRBTCvol >= BTCMBTCvol - 5)) {
                slackObj.setStatus("", ":neutral_face:", 120, name);
            }
            else if (IRBTCvol < BTCMBTCvol - 5) {
                slackObj.setStatus("", ":slightly_frowning_face:", 120, name);
            }
        }

        // takes a website httpsResonse.StatusCode and returns a friendly string
        private string WebsiteError(string errorCode) {
            if (errorCode.Contains("429")) return "Rate limited";
            else if (errorCode.ToUpper().Contains("GATEWAYTIMEOUT") || errorCode.ToUpper().Contains("SERVICEUNAVAILABLE") || errorCode.ToUpper().Contains("CONFLICT") || errorCode.ToUpper().Contains("522") || errorCode.ToUpper().Contains("BADGATEWAY")) return "API failure";
            else if (string.IsNullOrEmpty(errorCode)) return "Network error";
            else if (errorCode.ToUpper().Contains("BADREQUEST") || errorCode.ToUpper().Contains("NOTFOUND")) return "Bad request";
            else {
                MessageBox.Show("Unknown failure: " + errorCode, "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "??";
            }
        }

        // this grabs data from the API, creates a MarketSummary object, and pops it in the cryptoPairs dictionary
        private void ParseDCE_IR(string crypto, string fiat) {
            Tuple<bool, string> marketSummary = Utilities.Get("https://api.independentreserve.com/Public/GetMarketSummary?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);
            if (!marketSummary.Item1) {
                DCEs["IR"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["IR"].NetworkAvailable = false;
            }
            else {
                DCEs["IR"].NetworkAvailable = true;
                DCE.MarketSummary mSummary = JsonConvert.DeserializeObject<DCE.MarketSummary>(marketSummary.Item2);

                // This bit is for a) volume (we don't get vol from websockets), and b) if there have been no orders to establish a spread, then the price and spread
                // stay at 0.  This is 
                //Dictionary<string, DCE.MarketSummary> cPairs = DCEs["IR"].GetCryptoPairs();
                //if (cPairs.ContainsKey(mSummary.pair) && cPairs[mSummary.pair].spread == 0) { 
                if (crypto == "XBT") {  // don't want to overwrite the spread orders as they're probably out of date
                    mSummary.CurrentHighestBidPrice = 0;
                    mSummary.CurrentLowestOfferPrice = 0;
                }
                //}
                mSummary.CreatedTimestampUTC = "";
                DCEs["IR"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                DCEs["IR"].CurrentDCEStatus = "Online";
                pollingThread.ReportProgress(21, mSummary);
            }
        }

        private void ParseDCE_BTCM(string crypto, string fiat) {
            string BTCM_crypto = crypto;
            if (crypto == "XBT") BTCM_crypto = "BTC";
            //if (crypto == "BCH") BTCM_crypto = "BCH";
            // https://api.btcmarkets.net/v3/markets/BCH-AUD/ticker
            Tuple<bool, string> marketSummary = Utilities.Get("https://api.btcmarkets.net/v3/markets/" + BTCM_crypto + "-" + fiat + "/ticker");
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
                mSummary.DayHighestPrice = mSummary_BTCM.low24h;
                mSummary.DayLowestPrice = mSummary_BTCM.high24h;

                DCEs["BTCM"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs

                DCEs["BTCM"].CurrentDCEStatus = "Online";
                DCEs["BTCM"].NetworkAvailable = true;
            }
        }

        private void ParseDCE_CSPT(string fiat) {
            Tuple<bool, string> marketSummary = Utilities.Get("https://www.coinspot.com.au/pubapi/latest");
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
                    if (decimal.TryParse(cryptoResponse.last, out decimal temp)) {
                        mSummary.LastPrice = temp;
                    }
                    else Debug.Print("Could not convert CSPT price: " + cryptoResponse.last);

                    if (decimal.TryParse(cryptoResponse.bid, out temp)) {
                        mSummary.CurrentHighestBidPrice = temp;
                    }
                    else Debug.Print("Could not convert CSPT price: " + cryptoResponse.bid);

                    if (decimal.TryParse(cryptoResponse.ask, out temp)) {
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

        private void ParseFiat_OER(string baseSymbol, string symbols) {
            Debug.Print("pulling fiat");
            Tuple<bool, string> fxRates = Utilities.Get("https://openexchangerates.org/api/latest.json?app_id=33bde25e96a6447da4a54d490ca650f2&base=" + baseSymbol + "&symbols=" + symbols + "&prettyprint=false&show_alternative=false");
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
            Tuple<bool, string> currencies = Utilities.Get("https://api.pro.coinbase.com/currencies");
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
            Tuple<bool, string> products = Utilities.Get("https://api.pro.coinbase.com/products");
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

            Tuple<bool, string> products = Utilities.Get("https://api.bitfinex.com/v1/symbols_details");
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
                    if (prod.pair.StartsWith("bab")) {
                        prod.pair = prod.pair.Replace("bab", "BCH");
                    }
                    if (prod.pair.StartsWith("ust")) prod.pair = prod.pair.Replace("ust", "USDT");

                    // next we need to do a manual conversion.
                    DCE.products_GDAX prod_gdax = new DCE.products_GDAX();
                    prod_gdax.id = (prod.pair.Insert(prod.pair.Length - 3, "-")).ToUpper();  // converts btcusd into BTC-USD
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
            List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();

            if (dExchange == "IR") {
                pairList.Add(new Tuple<string, string>("XBT", "AUD"));
                pairList.Add(new Tuple<string, string>("XBT", "USD"));
                pairList.Add(new Tuple<string, string>("XBT", "NZD"));
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
            wSocketConnect.WebSocket_Subscribe(dExchange, pairList);
        }

        private void GetBTCMOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Utilities.Get("https://api.btcmarkets.net/market/" + (crypto == "XBT" ? "BTC" : crypto) + "/" + DCEs["BTCM"].CurrentSecondaryCurrency + "/orderbook");
            if (orderBookTpl.Item1) { 
                DCE.OrderBook_BTCM orderBook_BTCM = JsonConvert.DeserializeObject<DCE.OrderBook_BTCM>(orderBookTpl.Item2);

                // convert the BTCM order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = orderBook_BTCM.instrument;
                oBook.SecondaryCurrencyCode = orderBook_BTCM.currency;
                DateTimeOffset timeTemp = DateTimeOffset.FromUnixTimeSeconds(orderBook_BTCM.timestamp);  // convert from epoch
                oBook.CreatedTimestampUtc = timeTemp.UtcDateTime;

                foreach (List<decimal> ask in orderBook_BTCM.asks) {
                    oBook.SellOrders.Add(new DCE.Order("LimitSell", ask[0], ask[1], ""));
                }

                foreach (List<decimal> bid in orderBook_BTCM.bids) {
                    oBook.BuyOrders.Add(new DCE.Order("LimitBuy", bid[0], bid[1], ""));
                }
                DCEs["BTCM"].orderBooks[crypto + "-" + DCEs["BTCM"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void GetGDAXOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Utilities.Get("https://api.pro.coinbase.com/products/" + (crypto == "XBT" ? "BTC" : crypto) + "-" + DCEs["GDAX"].CurrentSecondaryCurrency + "/book?level=" + (EnableGDAXLevel3_CheckBox.Checked ? "3" : "2"));
            if (orderBookTpl.Item1) {
                DCE.OrderBook_GDAX orderBook_GDAX = JsonConvert.DeserializeObject<DCE.OrderBook_GDAX>(orderBookTpl.Item2);

                // convert the GDAX order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = crypto;
                oBook.SecondaryCurrencyCode = DCEs["GDAX"].CurrentSecondaryCurrency;

                foreach (List<string> ask in orderBook_GDAX.asks) {
                    if (decimal.TryParse(ask[0], out decimal price)) {
                        if (decimal.TryParse(ask[1], out decimal volume)) {
                            oBook.SellOrders.Add(new DCE.Order("LimitSell", price, volume, ask[2]));
                        }
                        else MessageBox.Show("Could not convert GDAX order book ask volume to a double: " + ask[1], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert GDAX order book ask price to a double: " + ask[0], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (List<string> bid in orderBook_GDAX.bids) {
                    if (decimal.TryParse(bid[0], out decimal price)) {
                        if (decimal.TryParse(bid[1], out decimal volume)) {
                            oBook.BuyOrders.Add(new DCE.Order("LimitBuy", price, volume, bid[2]));
                        }
                        else MessageBox.Show("Could not convert GDAX order book bid volume to a double: " + bid[1], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert GDAX order book bid price to a double: " + bid[0], "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DCEs["GDAX"].orderBooks[crypto + "-" + DCEs["GDAX"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void GetBFXOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Utilities.Get("https://api.bitfinex.com/v1/book/" + (crypto == "XBT" ? "BTC" : crypto == "BCH" ? "BAB" : crypto == "USDT" ? "UST" : crypto) + DCEs["BFX"].CurrentSecondaryCurrency + "?limit_bids=200&limit_asks=200");
            if (orderBookTpl.Item1) {
                DCE.OrderBook_BFX orderBook_BFX = JsonConvert.DeserializeObject<DCE.OrderBook_BFX>(orderBookTpl.Item2);

                // convert the  order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = crypto;
                oBook.SecondaryCurrencyCode = DCEs["BFX"].CurrentSecondaryCurrency;

                foreach (DCE.BidAsk_BFX ask in orderBook_BFX.asks) {
                    if (decimal.TryParse(ask.price, out decimal price)) {
                        if (decimal.TryParse(ask.amount, out decimal volume)) {
                            oBook.SellOrders.Add(new DCE.Order("LimitSell", price, volume, ""));
                        }
                        else MessageBox.Show("Could not convert BFX order book ask volume to a double: " + ask.amount, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert BFX order book ask price to a double: " + ask.price, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (DCE.BidAsk_BFX bid in orderBook_BFX.bids) {
                    if (decimal.TryParse(bid.price, out decimal price)) {
                        if (decimal.TryParse(bid.amount, out decimal volume)) {
                            oBook.BuyOrders.Add(new DCE.Order("LimitBuy", price, volume, ""));
                        }
                        else MessageBox.Show("Could not convert BFX order book bid volume to a double: " + bid.amount, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Could not convert BFX order book bid price to a double: " + bid.price, "Show Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DCEs["BFX"].orderBooks[crypto + "-" + DCEs["BFX"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void PopulateCryptoComboBox(string dExchange) {

            if (UIControls_Dict[dExchange].AvgPrice == null) return;  // eg coinspot 

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
                    Tuple<bool, string> primaryCurrencyCodesTpl = Utilities.Get("https://api.independentreserve.com/Public/GetValidPrimaryCurrencyCodes");
                    if (!primaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(primaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].CurrentDCEStatus = "Online";
                        DCEs["IR"].PrimaryCurrencyCodes = Utilities.TrimEnds(primaryCurrencyCodesTpl.Item2);
                        //DCEs["IR"].PrimaryCurrencyCodes = "\"XBT\"";
                    }

                    Tuple<bool, string> secondaryCurrencyCodesTpl = Utilities.Get("https://api.independentreserve.com/Public/GetValidSecondaryCurrencyCodes");
                    if (!secondaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(secondaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].CurrentDCEStatus = "Online";
                        DCEs["IR"].SecondaryCurrencyCodes = Utilities.TrimEnds(secondaryCurrencyCodesTpl.Item2);
                        //DCEs["IR"].SecondaryCurrencyCodes = "\"AUD\"";
                    }
                    if (DCEs["IR"].NetworkAvailable) {
                        DCEs["IR"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                        Dictionary<string, DCE.products_GDAX> productDictionary_IR = new Dictionary<string, DCE.products_GDAX>();
                        foreach (string crypto in DCEs["IR"].PrimaryCurrencyList) {
                            foreach (string fiat in DCEs["IR"].SecondaryCurrencyList) {
                                productDictionary_IR.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));

                                //create OB objects ready to be filled.  we only do this once here, and never delete them.  neverrrrr
                                //DCEs["IR"].InitialiseOrderBookDicts_IR(crypto, fiat);  // commented as we only want ot do BTC atm
                            }
                        }
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "AUD");
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "USD");
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "NZD");
                        DCEs["IR"].ExchangeProducts = productDictionary_IR;
                        
                        SubscribeTickerSocket("IR");
                    }
                    else {
                        pollingThread.ReportProgress(12, "IR");
                    }
                }

                // still need to run this to get volume data (and all coins except BTC)
                if (DCEs["IR"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["IR"].PrimaryCurrencyList) {
                        // if there's no crypto selected in the drop down or there's no number of coins entered, then just pull the market summary
                        if (loopCount == 0 || !shitCoins.Contains(primaryCode)) {
                            ParseDCE_IR(primaryCode, DCEs["IR"].CurrentSecondaryCurrency);
                        }
                        //if (DCEs["IR"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["IR"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                            //GetIROrderBook(primaryCode, );
                        //}
                    }
                }
                else DCEs["IR"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try

                // the heartbeat is initialised as the year 2000, so if it's this year we know it must be just starting up, no need to worry
                if ((DCEs["IR"].HeartBeat + TimeSpan.FromSeconds(100) < DateTime.Now) && DCEs["IR"].HeartBeat.Year != 2000) {
                    // we haven't received a heartbeat in 80 seconds..
                    Debug.Print(DateTime.Now + " IR - haven't received any messages via sockets in 100 seconds.  reconnecting..");
                    DCEs["IR"].socketsReset = true;
                }

                // separate this because it's possible to hit this code where the socketsreset == true for some other reason that heartbeat
                if (DCEs["IR"].socketsReset) {
                    DCEs["IR"].socketsReset = false;
                    // ok we need to reset the socket.
                    Debug.Print(DateTime.Now + " IR - restarting sockets from backgroundWorker");
                    wSocketConnect.WebSocket_Reconnect("IR");
                }


                //////// BTC Markets /////////

                if (DCEs["BTCM"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["BTCM"].PrimaryCurrencyList) {

                        if (DCEs["BTCM"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["BTCM"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                            GetBTCMOrderBook(primaryCode);
                        }
                    }
                }
                else DCEs["BTCM"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try

                // everything in here only happens once per session
                if (!DCEs["BTCM"].HasStaticData) {

                    Dictionary<string, DCE.products_GDAX> productDictionary_BTCM = new Dictionary<string, DCE.products_GDAX>();
                    foreach (string crypto in DCEs["BTCM"].PrimaryCurrencyList) {
                        foreach (string fiat in DCEs["BTCM"].SecondaryCurrencyList) {
                            productDictionary_BTCM.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                        }
                    }
                    DCEs["BTCM"].ExchangeProducts = productDictionary_BTCM;

                    // we do one connection to the REST API because it can take some time for sockets to populate all the pairs.
                    foreach (string primaryCode in DCEs["BTCM"].PrimaryCurrencyList) {
                        ParseDCE_BTCM(primaryCode, DCEs["BTCM"].CurrentSecondaryCurrency);
                    }
                    
                    SubscribeTickerSocket("BTCM");
                    pollingThread.ReportProgress(34);
                    DCEs["BTCM"].HasStaticData = true;
                }

                // the heartbeat is initialised as the year 2000, so if it's this year we know it must be just starting up, no need to worry
                if ((DCEs["BTCM"].HeartBeat + TimeSpan.FromSeconds(100) < DateTime.Now) && DCEs["BTCM"].HeartBeat.Year != 2000) {
                    // we haven't received a heartbeat in 10 seconds..
                    Debug.Print(DateTime.Now + " BTCMv2 - haven't received any messages via sockets in 100 seconds.  reconnecting..");
                    DCEs["BTCM"].socketsAlive = false;
                    DCEs["BTCM"].socketsReset = true;
                }

                // separate this because it's possible to hit this code where the socketsreset == true for some other reason that heartbeat
                if (DCEs["BTCM"].socketsReset) {
                    // just in case sockets is still broken, let's grab some REST data
                    foreach (string primaryCode in DCEs["BTCM"].PrimaryCurrencyList) {
                        ParseDCE_BTCM(primaryCode, DCEs["BTCM"].CurrentSecondaryCurrency);
                    }
                    DCEs["BTCM"].socketsReset = false;
                    // ok we need to reset the socket.
                    Debug.Print(DateTime.Now + " BTCMv2 - REST data pulled, now restarting sockets from backgroundWorker");
                   wSocketConnect.WebSocket_Reconnect("BTCM");
                }


                //////// GDAX ///////

                if (!DCEs["GDAX"].HasStaticData) {  // should only call this onec per session
                    GetGDAXProducts();
                    if (DCEs["GDAX"].HasStaticData) {
                        Debug.Print("calling gdax sockets sub");
                        SubscribeTickerSocket("GDAX");
                        pollingThread.ReportProgress(44);
                    }
                }
                else DCEs["GDAX"].NetworkAvailable = true;  // set to true here so on the next poll we make an attempt on the parseDCE method.  If it fails, we set to false and skip the next try

                if (loopCount == 0) {
                    if (!wSocketConnect.IsSocketAlive("BFX")) { 
                        Debug.Print("BFX ded, reconnecting");
                        wSocketConnect.WebSocket_Reconnect("BFX");
                    }
                    if (!wSocketConnect.IsSocketAlive("GDAX")) { 
                        Debug.Print("GDAX ded, reconnecting");
                        wSocketConnect.WebSocket_Reconnect("GDAX");
                    }
                    if (!wSocketConnect.IsSocketAlive("IR")) {  // do i need to add some logic here to make sure we're not currently in the reconnect process if this code happens to get hit when we are reconnecting?
                        Debug.Print("IR ded, reconnecting at next poll");
                        pollingThread.ReportProgress(12, "IR");
                        DCEs["IR"].socketsReset = true;
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
                    ParseFiat_OER("USD", "AUD,NZD,EUR,USD,SGD");  // only run this once per session as we have limited fx API calls.
                    refreshFiat = false;
                }

                // OK we now have all the DCE and fiat rates info loaded.

                pollingThread.ReportProgress(1);

                if (DCEs["IR"].CryptoCombo != "" && !string.IsNullOrEmpty(DCEs["IR"].NumCoinsStr) && DCEs["IR"].HasStaticData) {  // we have a crypto selected and coins entered, let's get the order book for them
                    pollingThread.ReportProgress(2);  // OK let's lock the fields down
                    pollingThread.ReportProgress(23);  // display order book
                }

                if (DCEs["BTCM"].CryptoCombo != "" && !string.IsNullOrEmpty(DCEs["BTCM"].NumCoinsStr) && DCEs["BTCM"].HasStaticData) {  // we have a crypto selected and coins entered, let's get the order book for them
                    pollingThread.ReportProgress(2);  // OK let's lock the fields down
                    GetBTCMOrderBook(DCEs["BTCM"].CryptoCombo);
                    pollingThread.ReportProgress(33);  // display order book
                }

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
                }

                if (Properties.Settings.Default.ExportSummarised && (lastCSVWrite + TimeSpan.FromHours(1) < DateTime.Now )) {
                    WriteSpreadHistoryCompressed();
                    lastCSVWrite = DateTime.Now;  // whether or not we write to the CSV, don't try again for another hour.  Setting this AFTER we call the writeSpreadHistoryCompressed() sub so worst case we miss a couple of datapoints rather than duplicate them.
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
                    string formatString = "### ##0.00";
                    string formatStringSpread = "### ##0.00";
                    string formatStringVol = "##0.00";
                    if (pairObj.Value.LastPrice < 10) formatString = "0.00###";  // some coins are so shit, they're worth less than a cent.  Need different formatting for this.  ORRR the spread is so amazingly small we need more decimal places
                    if (pairObj.Value.spread < 10) formatStringSpread = "0.00###";
                    if (pairObj.Value.DayVolume >= 1000) formatStringVol = "### ##0.00";
                    if (pairObj.Value.DayVolume >= 1000000) formatStringVol = "### ### ##0.00";
                    decimal midPoint = (pairObj.Value.CurrentHighestBidPrice + pairObj.Value.CurrentLowestOfferPrice) / 2;

                    // we use this price label so often and it's so much text to access it, i want to just create a quick variable to make the code easier to read
                    System.Windows.Forms.Label tempPrice = UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"];
                    tempPrice.Text = midPoint.ToString(formatString).Trim();
                    tempPrice.ForeColor = Utilities.PriceColour(DCEs[dExchange].GetPriceList(pairObj.Key));

                    // if there's a colour, make the font bigger.  otherwise not bigger.
                    if (tempPrice.ForeColor != Color.Black) {
                        tempPrice.Font = new Font(tempPrice.Font.FontFamily, 10f, FontStyle.Bold);

                        // this next bit is crazy.  When we change the size of the text, it seems to drop down a couple of pixels.  I don't know why, the label just looks lower
                        // so to fix it I push the label up 2 pixels.  But I need to keep track of whether I have already pushed the label up or not, so I use the tag property
                        if (!tempPrice.Tag.ToString().Contains("emphasised")) {
                            tempPrice.Location = new Point(tempPrice.Location.X, tempPrice.Location.Y - 3);
                            if (!tempPrice.Tag.ToString().EndsWith(",")) tempPrice.Tag = tempPrice.Tag + ",";
                            tempPrice.Tag = tempPrice.Tag + "emphasised";
                        }
                    }
                    else {
                        tempPrice.Font = new Font(tempPrice.Font.FontFamily, 8.25f, FontStyle.Bold);

                        if (tempPrice.Tag.ToString().Contains("emphasised")) {  // coming down off a high, if we were emphasised, but we're now not, we need to drop the label 2 pixels
                            tempPrice.Location = new Point(tempPrice.Location.X, tempPrice.Location.Y + 3);
                            tempPrice.Tag = tempPrice.Tag.ToString().Replace("emphasised", "");
                        }
                    }

                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"].Text = pairObj.Value.spread.ToString(formatStringSpread) + ((pairObj.Value.DayVolume == 0) ? "" : " / " + pairObj.Value.DayVolume.ToString(formatStringVol));

                    // update tool tips.
                    IRTickerTT.SetToolTip(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"], "Best bid: " + pairObj.Value.CurrentHighestBidPrice.ToString(formatString) + System.Environment.NewLine + "Best offer: " + pairObj.Value.CurrentLowestOfferPrice.ToString(formatString));
                }
                //else Debug.Print("Pair don't exist, pairObj.Value.SecondaryCurrencyCode: " + pairObj.Value.SecondaryCurrencyCode);
            }

            // have commented this out as all exchanges with an order book should be done through reportProgress 23, 33, 43, etc
            /*if (!String.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text) && UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex != 0) {  // need to check this before trying to evaluate it
                UIControls_Dict[dExchange].AvgPrice.Text = DetermineAveragePrice(DCEs[dExchange].CryptoCombo, DCEs[dExchange].CurrentSecondaryCurrency, dExchange);
                UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
            }*/
            /*else*/
            if (UIControls_Dict[dExchange].AvgPrice != null) {
                UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Gray;  // any text there is now a poll old, so gray it out so the user knows it's stale.
                UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = true;  // we disable it if they change the fiat currency as we need to re-populate the crypto combo box first
            }
        }

        // Updates labels, but just a specific pair (used for websockets because we get each pair separartely)
        private void UpdateLabels_Pair(string dExchange, DCE.MarketSummary mSummary) {
            // first we reset the labels.  The label writing process only writes to labels of pairs that exist, so we first need to set them back in case they don't exist

            //DCE.MarketSummary mSummary = DCEs[dExchange].GetCryptoPairs()[crypto + "-" + fiat];

            // i guess we need to filter out the wrong pairs, also don't try and update labels that are -1 (-1 means they're a fake entry)
            if (mSummary.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency && mSummary.LastPrice >= 0) {

                // we have a legit pair we're about to update.  if the groupBox is grey, let's black it.
                GroupBoxAndLabelColourActive(dExchange);
                UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";

                string formatString = "### ##0.00";
                string formatStringSpread = "### ##0.00";
                string formatStringVol = "##0.00";

                decimal midPoint = (mSummary.CurrentHighestBidPrice + mSummary.CurrentLowestOfferPrice) / 2;  // we don't use last price anymore, instead the midpoint of the spread

                if (midPoint < 10) formatString = "0.00###";  // some coins are so shit, they're worth less than a cent.  Need different formatting for this.  ORRR the spread is so amazingly small we need more decimal places
                if (mSummary.spread < 10) formatStringSpread = "0.00###";
                if (mSummary.DayVolume >= 1000) formatStringVol = "### ##0.00";
                if (mSummary.DayVolume >= 1000000) formatStringVol = "### ### ##0.00";


                System.Windows.Forms.Label tempPrice = UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"];

                tempPrice.Text = midPoint.ToString(formatString).Trim();
                tempPrice.ForeColor = Utilities.PriceColour(DCEs[dExchange].GetPriceList(mSummary.pair));
                // don't do this anymore, because we buffer and hold event, the buffer will often legitimately have events in it, so this isn't (anmymore) an indication that there's a nonce issue
                // if we're experiencing nonce errors for this pair, make it gray.
                //if ((DCEs[dExchange].orderBuffer_IR.ContainsKey(mSummary.pair.ToUpper())) && DCEs[dExchange].orderBuffer_IR[mSummary.pair.ToUpper()].Count > 0) {
                //    tempPrice.ForeColor = Color.Gray;
                //}


                // if there's a colour, make the font bigger.  otherwise not bigger.
                if (tempPrice.ForeColor != Color.Black) {
                    tempPrice.Font = new Font(tempPrice.Font.FontFamily, 10f, FontStyle.Bold);

                    // this next bit is crazy.  When we change the size of the text, it seems to drop down a couple of pixels.  I don't know why, the label just looks lower
                    // so to fix it I push the label up 2 pixels.  But I need to keep track of whether I have already pushed the label up or not, so I use the tag property
                    if (!tempPrice.Tag.ToString().Contains("emphasised")) {
                        tempPrice.Location = new Point(tempPrice.Location.X, tempPrice.Location.Y - 3);
                        if (!tempPrice.Tag.ToString().EndsWith(",")) tempPrice.Tag = tempPrice.Tag + ",";
                        tempPrice.Tag = tempPrice.Tag + "emphasised";
                    }
                }
                else {
                    tempPrice.Font = new Font(tempPrice.Font.FontFamily, 8.25f, FontStyle.Bold);

                    if (tempPrice.Tag.ToString().Contains("emphasised")) {
                        tempPrice.Location = new Point(tempPrice.Location.X, tempPrice.Location.Y + 3);
                        tempPrice.Tag = tempPrice.Tag.ToString().Replace("emphasised", "");
                    }
                }

                UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"].Text = mSummary.spread.ToString(formatStringSpread) + ((mSummary.DayVolume == 0) ? "" : " / " + mSummary.DayVolume.ToString(formatStringVol));
                //Debug.Print("ABOUT TO CHECK ORDER BOOK STUFF:");
                //Debug.Print("---num coins = " + UIControls_Dict[dExchange].AvgPrice_NumCoins.Text + " avgprice_crypto = " + (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem == null ? "null" : UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem.ToString()));

                if (DCEs[dExchange].ChangedSecondaryCurrency) { 
                    PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                    DCEs[dExchange].ChangedSecondaryCurrency = false;
                }

                // update tool tips.
                IRTickerTT.SetToolTip(UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"], "Best bid: " + mSummary.CurrentHighestBidPrice.ToString(formatString) + System.Environment.NewLine + "Best offer: " + mSummary.CurrentLowestOfferPrice.ToString(formatString));
            }
            else Debug.Print("Pair2 don't exist, pairObj.Value.SecondaryCurrencyCode: " + mSummary.SecondaryCurrencyCode);
        }

        // this works out what the average price of a market order on the OB would be for IR.  We needed to separate this logic from the other exchanges 
        // because IR's order book is represented very differently (and used constantly)
        private string DetermineAveragePrice_IR(string crypto, string fiat, string currency) {
            crypto = crypto.ToUpper();
            fiat = fiat.ToUpper();
            string pair = crypto + "-" + fiat;
            bool fiatSelected = (currency == "fiat" ? true : false);  // if the crypto value is the same as the fiat one, it means that the selected crypto was fiat (ie they chose AUD from the drop avg price dropdown)

            if (!DCEs["IR"].IR_OBs.ContainsKey(pair)) return "No order book for the " + pair + " pair";
            string orderSide = "Buy";
            if (UIControls_Dict["IR"].AvgPrice_BuySell.SelectedItem.ToString() == "Sell") orderSide = "Sell";

            // here we grab the buy or sell order book, make a copy, and then sort it
            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBook;
            if (orderSide == "Buy") {
                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item2.ToArray();  // because we're buying from the sell orders
                orderedBook = arrayBook.OrderBy(k => k.Key);
                Debug.Print("--- picked the sell side, top order is: " + orderedBook.First().Key);
            }
            else {
                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item1.ToArray();  // because we're selling to the buy orders
                orderedBook = arrayBook.OrderByDescending(k => k.Key);
                Debug.Print("--- picked the buy side, top order is: " + orderedBook.First().Key);
            }

            if (decimal.TryParse(UIControls_Dict["IR"].AvgPrice_NumCoins.Text, out decimal coins)) {  // grab the number of coins we want to buy/sell from the DCE object
                Debug.Print("--- you typed in " + coins + " coins");
                decimal coinCounter = 0;  // we add to this counter until it reaches the numCoinsTextBox (coins) value
                decimal weightedAverage = 0;
                decimal totalCost = 0;  // this will be used to tally fiat if a crypto is selected, and crypto if fiat is selected
                int orderCount = 0;
                foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> pricePoint in orderedBook) {
                    Debug.Print("--- first outside price point (" + pricePoint.Key + "), there are " + pricePoint.Value.Count + " orders at this price to pick through");
                    foreach (KeyValuePair<string, DCE.OrderBook_IR> subOrder in pricePoint.Value) {
                        orderCount++;
                        Debug.Print("--- looking at order " + orderCount);
                        coinCounter += subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price : 1);  // if they have selected a fiat currency, we need to multiply by the price
                        Debug.Print("--- coinCounter is " + coinCounter + " and has just been increased by " + subOrder.Value.Volume + " (coins: " + coins + ")");
                        if (coinCounter > coins) {
                            decimal usedCoinsInThisOrder = (subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price : 1)) - (coinCounter - coins);  // this is how many coins in this order would be required
                            Debug.Print("--- we're in the final tally up if, we will use " + usedCoinsInThisOrder + "coins from this order");
                            totalCost += usedCoinsInThisOrder * (fiatSelected ? (1 / subOrder.Value.Price) : subOrder.Value.Price);
                            Debug.Print("--- total cost is finally " + totalCost + " and the final increase was " + usedCoinsInThisOrder / (fiatSelected ? subOrder.Value.Price : 1));
                            weightedAverage += (usedCoinsInThisOrder / coins) * subOrder.Value.Price;
                            string tTip = buildAvgPriceTooltip(orderSide, fiatSelected, subOrder.Value.Price, orderCount, totalCost, crypto);
                            IRTickerTT.SetToolTip(UIControls_Dict["IR"].AvgPrice, tTip);
                            return "Average price for " + crypto + ": " + (fiatSelected ? "$" : "") + weightedAverage.ToString("### ##0.##").Trim();  // we have finished filling the hypothetical order
                        }
                        else {  // this whole sub order is required
                            weightedAverage += ((subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price : 1)) / coins) * subOrder.Value.Price;
                            totalCost += subOrder.Value.Volume * (fiatSelected ? 1 : subOrder.Value.Price);  // if fiat is selected, totalCost var represents total coins
                            Debug.Print("--- totalCost is now " + totalCost + " and was increased by " + subOrder.Value.Volume * (fiatSelected ? 1 : subOrder.Value.Price));
                        }
                    }
                }
                return "Order book only has " + (fiatSelected ? "$" : "") + coinCounter.ToString("### ##0.##").Trim() + " " + crypto;
            }
            else {
                MessageBox.Show("Could not convert num coins to a number.  how? num = " + UIControls_Dict["IR"].AvgPrice_NumCoins.Text, "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "";
        }

        // builds the string in the tool tip.  Depending on what 
        private string buildAvgPriceTooltip(string buySell, bool fiatSelected, decimal price, int orderCount, decimal finalAmount, string crypto) {

            StringBuilder AvgPrice_TTStr = new StringBuilder();
            AvgPrice_TTStr.AppendLine((buySell == "Buy" ? "Max" : "Min") + " price paid: " + price.ToString("### ##0.##"));
            AvgPrice_TTStr.AppendLine("Orders required to fill: " + orderCount);
            AvgPrice_TTStr.Append("Notional ");

            if (fiatSelected && buySell == "Buy") {
                AvgPrice_TTStr.Append("coins bought: " + (crypto == "XBT" ? "BTC" : crypto) + " ");
            }
            else if (fiatSelected && buySell == "Sell") {
                AvgPrice_TTStr.Append("coins sold: " + (crypto == "XBT" ? "BTC" : crypto) + " ");
            }
            else if (!fiatSelected && buySell == "Buy") {
                AvgPrice_TTStr.Append("fiat cost: $ ");
            }
            else if (!fiatSelected && buySell == "Sell") {
                AvgPrice_TTStr.Append("fiat received: $ ");
            }

            string formatString = "##0.##";
            if (finalAmount > 999) formatString = "### ##0.##";
            if (finalAmount > 999999) formatString = "### ### ##0.##";

            AvgPrice_TTStr.Append(finalAmount.ToString(formatString));
            return AvgPrice_TTStr.ToString();
        }

        private string DetermineAveragePrice(string crypto, string fiat, string dExchange) { 

            crypto = crypto.ToUpper();
            fiat = fiat.ToUpper();
            string pair = crypto + "-" + fiat;

            if (!DCEs[dExchange].orderBooks.ContainsKey(pair)) return "Failed to pull order book from API";

            // work out the average and set it to the label
            List<DCE.Order> orderBook;
            string orderSide = "Buy";
            if (UIControls_Dict[dExchange].AvgPrice_BuySell.SelectedItem.ToString() == "Sell") orderSide = "Sell";

            if (orderSide == "Buy") orderBook = DCEs[dExchange].orderBooks[pair].SellOrders;  // because we're buying from the sell orders
            else orderBook = DCEs[dExchange].orderBooks[pair].BuyOrders;  // selling to the buy orders

            if (decimal.TryParse(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text, out decimal coins)) {

                decimal coinCounter = 0;  // we add to this counter until it reaches the numCoinsTextBox (coins) value
                decimal weightedAverage = 0;
                decimal totalCost = 0;
                int orderCount = 0;
                bool gracefulFinish = false;  // this only gets set to true if the order book has enough coins in it to handle the number of inputted coins.  If it doesn't (ie the foreach completes without us having counted the inputted coins), then we throw a warning message
                foreach (DCE.Order order in orderBook) {
                    orderCount++;
                    coinCounter += order.Volume;
                    if (coinCounter > coins) {  // ok we are on the last value we need to look at.  need to truncate.
                        decimal usedCoinsInThisOrder = order.Volume - (coinCounter - coins);  // this is how many coins in this order would be required
                        totalCost += usedCoinsInThisOrder * order.Price;
                        weightedAverage += (usedCoinsInThisOrder / coins) * order.Price;
                        gracefulFinish = true;
                        string tTip = (orderSide == "Buy" ? "Max" : "Min") + " price paid: " + order.Price.ToString("### ##0.##") + System.Environment.NewLine + "Orders required to fill: " + orderCount + System.Environment.NewLine + "Total fiat cost: " + totalCost.ToString("### ### ##0.##");
                        IRTickerTT.SetToolTip(UIControls_Dict[dExchange].AvgPrice, tTip);
                        break;  // we have finished filling the hypothetical order
                    }
                    else {  // this whole order is required
                        weightedAverage += (order.Volume / coins) * order.Price;
                        totalCost += order.Volume * order.Price;
                    }
                }
                if (!gracefulFinish) {
                    //MessageBox.Show("You requested " + coins + " coins, but the order book's entire volume (that the API returned to us) had only " + coinCounter + " coins in it.  So, the displayed average price will be less than reality, but you probably fat fingered how many coins?", dExchange + "'s order book too small for that number of coins", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "Order book only has " + coinCounter.ToString("### ##0.##").Trim() + " " + crypto;
                }
                DCEs[dExchange].RemoveOrderBook(pair);  // need to remove once we've used - there's the possibility that the next orderbook API pull fails, then the code will just use the existing order book
                return "Average price for " + crypto + ": " + weightedAverage.ToString("### ##0.##").Trim();
            }
            else {
                MessageBox.Show("Could not convert num coins to a number.  how? num = " + UIControls_Dict[dExchange].AvgPrice_NumCoins.Text, "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "";
        }

        private void OBProgressNext(bool negativeSpread) {
            switch (obv.OBProgress.Text) {
                case "--":
                    obv.OBProgress.Text = "\\";
                    break;
                case "\\":
                    obv.OBProgress.Text = "|";
                    break;
                case "|":
                    obv.OBProgress.Text = "/";
                    break;
                case "/":
                    obv.OBProgress.Text = "--";
                    break;
                default:
                    obv.OBProgress.Text = "--";
                    break;
            }
            if (negativeSpread) {
                obv.OBProgress.ForeColor = Color.Red;
            }
            else {
                obv.OBProgress.ForeColor = Color.Black;
            }
        }

        private void PollingThread_ReportProgress(object sender, ProgressChangedEventArgs e) {

            int reportType = e.ProgressPercentage;

            // 2 means we just want to lock the average coin price controls so it can't be change while we're pulling the data
            if (reportType == 2) {
                foreach (string dExchange in Exchanges) {
                    if (UIControls_Dict[dExchange].AvgPrice == null) continue; // none of these fields exist for coinspot
                    // if they have filled in the order book controls, then disable them while we work it out
                    if (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex > 0 && !string.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text)) {
                        UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = false;
                        UIControls_Dict[dExchange].AvgPrice_BuySell.Enabled = false;
                        UIControls_Dict[dExchange].AvgPrice_NumCoins.Enabled = false;
                    }
                }
                return;
            }

            else if (reportType == 12) {  // 12 is error in the response or API.   either way, we disconnect and start again.
                string dExchange = (string)e.UserState;
                APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
                return;
            }

            if (reportType == 21) {  // 21 is IR update labels
                DCE.MarketSummary mSummary = (DCE.MarketSummary)e.UserState;
                UpdateLabels_Pair("IR", mSummary);
                //return;  // we want to go to report type 25 next to update OBView
                reportType = 25;
            }
            else if (reportType == 23) {  // 23 is order book stuff for ir - not currently working. (or required?)
                UIControls_Dict["IR"].AvgPrice.Text = DetermineAveragePrice_IR(DCEs["IR"].CryptoCombo, DCEs["IR"].CurrentSecondaryCurrency, DCEs["IR"].CurrencyCombo);
                UIControls_Dict["IR"].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict["IR"].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                IR_CryptoComboBox.Enabled = IR_BuySellComboBox.Enabled = IR_NumCoinsTextBox.Enabled = true;
                return;
            }
            else if (reportType == 24) {  // should only be called once per session - if we don't do this the crypto combo box is empty until we change secondary currencies
                PopulateCryptoComboBox("IR");
                return;
            }
            if (reportType == 25) {  // 25 is for updating ob view
                DCE.MarketSummary mSummary = (DCE.MarketSummary)e.UserState;
                if (mSummary.pair == "XBT-AUD" && obv.Visible) { // || mSummary.pair == "--ETH-AUD") {
                    OBProgressNext(mSummary.spread < 0);
                    KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] buySide = DCEs["IR"].IR_OBs[mSummary.pair].Item1.ToArray();
                    KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] sellSide = DCEs["IR"].IR_OBs[mSummary.pair].Item2.ToArray();
                    obv.UpdateOBs(buySide, sellSide, mSummary.pair.ToUpper());  // update the debug window
                }
                return;
            }
            if (reportType == 26) {  // 26 is to flash the window
                FlashWindowEx(this.FindForm());
                return;
            }
            if (reportType == 27) {  // 27 is we need to update the price colour because of nonce
                Tuple<bool, string> nonceIssue = (Tuple<bool, string>)e.UserState;  // bool is for if an issue exists, the string is the pair it exists (or not) on
                System.Windows.Forms.Label tempPrice = UIControls_Dict["IR"].Label_Dict[nonceIssue.Item2 + "_Price"];
                if (nonceIssue.Item1) {  // we have a nonce issue, set to grey
                    tempPrice.ForeColor = Color.Gray;
                }
                else {  // set back to whatever colour it should be
                    tempPrice.ForeColor = Utilities.PriceColour(DCEs["IR"].GetPriceList(nonceIssue.Item2));
                }
                return;
            }

            else if (reportType == 31) {  // update BTCM
                DCE.MarketSummary mSummary = (DCE.MarketSummary)e.UserState;

                UpdateLabels_Pair("BTCM", mSummary);
                return;
            }
            else if (reportType == 33) {  // 33 is order book stuff for btcm
                UIControls_Dict["BTCM"].AvgPrice.Text = DetermineAveragePrice(DCEs["BTCM"].CryptoCombo, DCEs["BTCM"].CurrentSecondaryCurrency, "BTCM");
                UIControls_Dict["BTCM"].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict["BTCM"].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                BTCM_CryptoComboBox.Enabled = BTCM_BuySellComboBox.Enabled = BTCM_NumCoinsTextBox.Enabled = true;
                return;
            }
            else if (reportType == 34) {  // should only be called once per session - if we don't do this the crypto combo box is empty until we change secondary currencies
                PopulateCryptoComboBox("BTCM");
                UpdateLabels("BTCM");  // we have pulled the BTCM data from the REST API, so let's display it.
                return;
            }

            else if (reportType == 41) {  // update GDAX
                DCE.MarketSummary mSummary = (DCE.MarketSummary)e.UserState;
                UpdateLabels_Pair("GDAX", mSummary);
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
                UpdateLabels_Pair("BFX", mSummary);
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

            Dictionary<string, DCE.MarketSummary> IRpairs = DCEs["IR"].GetCryptoPairs();
            Dictionary<string, DCE.MarketSummary> BTCMpairs = DCEs["BTCM"].GetCryptoPairs();
            decimal IRvol = -1, BTCMvol = -1;
            if (IRpairs.ContainsKey("XBT-AUD") && BTCMpairs.ContainsKey("XBT-AUD")) {
                IRvol = IRpairs["XBT-AUD"].DayVolume; ;
                BTCMvol = BTCMpairs["XBT-AUD"].DayVolume; 
                
                if (bStick == null) bStick = BlinkStick.FindFirst();
                if (bStick != null && bStick.OpenDevice()) {
                    // update blink stick
                    setStickColour(IRvol, BTCMvol);
                }
            }

            if (Properties.Settings.Default.Slack && (Properties.Settings.Default.SlackToken != "")) {
                setSlackStatus(IRvol, BTCMvol);
            }

            // update the UI

            // here we iterate through the exchanges and update their group boxes and labels

            foreach (string dExchange in Exchanges) {
                if (dExchange == "BFX" || dExchange == "GDAX" || dExchange == "BTCM" || dExchange == "IR") {  // for sockets we don't update labels or change colours.  that happens on demand.
                    // i don't think we need this code.  we shouldn't set this to false, it's set to false in the updatelabels_pairs(...) code
                    /*if (DCEs[dExchange].HasStaticData && DCEs[dExchange].ChangedSecondaryCurrency) {
                        // don't think we need to do this next line - we do it in the groupbox_click() sub
                        //PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                        DCEs[dExchange].ChangedSecondaryCurrency = false;
                    }*/
                    /*else*/ UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Gray;  // any text there is now a poll old, so gray it out so the user knows it's stale.

                    if (!DCEs[dExchange].socketsAlive) {  // this should happen if REST is up but sockets are down.  if REST is also down we wouldn't get here i hope.
                        Debug.Print(DateTime.Now + " - 1 setting sockets down, we are in the main reportProgress and socktsAlive is false - " + dExchange);
                        UpdateLabels(dExchange);
                        UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")" + " - sockets down";
                    }

                    if (!DCEs[dExchange].HasStaticData) APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
                    continue;
                }
                if (DCEs[dExchange].NetworkAvailable && DCEs[dExchange].CurrentDCEStatus == "Online") {
                    if (DCEs[dExchange].ChangedSecondaryCurrency) {
                        PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                        DCEs[dExchange].ChangedSecondaryCurrency = false;
                    }

                    GroupBoxAndLabelColourActive(dExchange);

                    UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";
                    if (!DCEs[dExchange].socketsAlive) {  // website might be up, 
                        Debug.Print(DateTime.Now + " - 2 setting sockets down, we are in the main reportProgress and socktsAlive is false - " + dExchange);
                        UIControls_Dict[dExchange].dExchange_GB.Text += " - sockets down";
                    }

                    UpdateLabels(dExchange);
                }
                else if (DCEs[dExchange].NetworkAvailable) {  // if we have network but not online, we probably have REST data to send to the UI, so do it.
                    Debug.Print(DateTime.Now + " - updating UI even though the exchange isn't in an 'online' state");
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
           
           // turn off the blink stick.
           if (bStick != null) {
                if (bStick.OpenDevice()) {
                    bStick.TurnOff();
                }
           }

           if (Properties.Settings.Default.Slack && (Properties.Settings.Default.SlackToken != "")) {
                slackObj.setStatus("", "");
           }
            wSocketConnect.stopUITimerThread();  // needed otherwise the app never actually closes
        }

        private void SettingsButton_Click(object sender, EventArgs e) {
            Settings.Visible = true;
            Main.Visible = false;
        }

        private void SettingsOKButton_Click(object sender, EventArgs e) {
            if(int.TryParse(refreshFrequencyTextbox.Text, out int refreshInt)) {
                if(refreshInt >= minRefreshFrequency) {
                    if (slackNameChangeCheckBox.Checked && slackDefaultNameTextBox.Text == string.Empty) {
                        MessageBox.Show("If you want the Slack name integration, please enter your default display name", "Need a display name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if(slackDefaultNameTextBox.Text == string.Empty && Properties.Settings.Default.SlackDefaultName != string.Empty) {  // if they're trying to delete the name, let's quickly set their slack name back to the default before we erase it.  This will remove the emoji until the next cycle, but oh well
                        slackObj.setStatus("", "", 120, Properties.Settings.Default.SlackDefaultName);
                    }
                    if (Slack_checkBox.Checked && slackToken_textBox.Text == string.Empty) {
                        MessageBox.Show("If you want Slack integration, please enter your xoxp token.  Ask Nick for details.", "Need a slack token!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Properties.Settings.Default.SlackToken = slackToken_textBox.Text;
                    Properties.Settings.Default.SlackDefaultName = slackDefaultNameTextBox.Text;
                    if (Int32.TryParse(UITimerFreq_maskedTextBox.Text, out int freq)) Properties.Settings.Default.UITimerFreq = freq;
                    else {
                        Debug.Print("ERROR: couldn't save the ui timer freq as it couldn't be converted to an int? - " + UITimerFreq_maskedTextBox.Text);
                        UITimerFreq_maskedTextBox.Text = Properties.Settings.Default.UITimerFreq.ToString();  // set it back to the last save value
                    }
                    Properties.Settings.Default.Save();
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

        // call this sub when the group box has good data and we need to make sure it's not greyed out.  this will
        // set the group box to black and the crypto labels to their exchange colour
        private void GroupBoxAndLabelColourActive(string dExchange) {
            Color fColour;
            switch (dExchange) {
                case "IR":
                    fColour = Color.RoyalBlue;
                    break;
                case "BTCM":
                    fColour = Color.OliveDrab;
                    break;
                case "GDAX":
                    fColour = Color.DodgerBlue;
                    break;
                case "BFX":
                    fColour = Color.DarkGreen;
                    break;
                case "CSPT":
                    fColour = Color.DarkTurquoise;
                    break;
                default:
                    fColour = Color.Black;
                    break;
            }

            UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Black;

            foreach (KeyValuePair<string, System.Windows.Forms.Label> UICobj in UIControls_Dict[dExchange].Label_Dict) {
                if (UICobj.Key.EndsWith("_Label")) {
                    UICobj.Value.ForeColor = fColour;
                }
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
            if (DCEs["IR"].HasStaticData) {
                GroupBox_Click("IR");
                GroupBoxAndLabelColourActive("IR");
                // need to force a label update, otherwise they'll stay grey <no currency> until the next update comes through
                foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs["IR"].GetCryptoPairs()) {
                    if (pairObj.Value.SecondaryCurrencyCode == DCEs["IR"].CurrentSecondaryCurrency) {
                        UpdateLabels_Pair("IR", pairObj.Value);
                    }
                }

                PopulateCryptoComboBox("IR");
            }
        }

        private void GDAX_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["GDAX"].HasStaticData) {
                GroupBox_Click("GDAX");
                GroupBoxAndLabelColourActive("GDAX");
                // need to force a label update, otherwise they'll stay grey <no currency> until the next update comes through
                foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs["GDAX"].GetCryptoPairs()) {
                    if (pairObj.Value.SecondaryCurrencyCode == DCEs["GDAX"].CurrentSecondaryCurrency) {
                        UpdateLabels_Pair("GDAX", pairObj.Value);
                    }
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
                GroupBoxAndLabelColourActive("BFX");
                // need to force a label update, otherwise they'll stay grey <no currency> until the next update comes through
                foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs["BFX"].GetCryptoPairs()) {
                    if (pairObj.Value.SecondaryCurrencyCode == DCEs["BFX"].CurrentSecondaryCurrency) {
                        UpdateLabels_Pair("BFX", pairObj.Value);
                    }
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
                    fiat_GroupBox.Text = "Fiat rates (base: AUD)";

                    USD_Label2.Text = fiatRates.rates.AUD.ToString("0.#####") + "  /  " + (1 / fiatRates.rates.AUD).ToString("0.#####");
                    NZD_Label2.Text = (1 / ((1 / fiatRates.rates.AUD) * fiatRates.rates.NZD)).ToString("0.#####") + "  /  " + ((1 / fiatRates.rates.AUD) * fiatRates.rates.NZD).ToString("0.#####");
                    EUR_Label2.Text = (1 / ((1 / fiatRates.rates.AUD) * fiatRates.rates.EUR)).ToString("0.#####") + "  /  " + ((1 / fiatRates.rates.AUD) * fiatRates.rates.EUR).ToString("0.#####");
                    SGD_Label2.Text = (1 / ((1 / fiatRates.rates.AUD) * fiatRates.rates.SGD)).ToString("0.#####") + "  /  " + ((1 / fiatRates.rates.AUD) * fiatRates.rates.SGD).ToString("0.#####");
                    AUD_Label2.Text = "1" + "  /  " + "1";
                }
                else {  // we're changing it to USD base
                    fiat_GroupBox.Text = "Fiat rates (base: USD)";

                    AUD_Label2.Text = (1 / fiatRates.rates.AUD).ToString("0.#####") + "  /  " + fiatRates.rates.AUD.ToString("0.#####");
                    NZD_Label2.Text = (1 / fiatRates.rates.NZD).ToString("0.#####") + "  /  " + fiatRates.rates.NZD.ToString("0.#####");
                    EUR_Label2.Text = (1 / fiatRates.rates.EUR).ToString("0.#####") + "  /  " + fiatRates.rates.EUR.ToString("0.#####");
                    USD_Label2.Text = (1 / fiatRates.rates.USD).ToString("0.#####") + "  /  " + fiatRates.rates.USD.ToString("0.#####");
                    SGD_Label2.Text = (1 / fiatRates.rates.SGD).ToString("0.#####") + "  /  " + fiatRates.rates.SGD.ToString("0.#####");
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

        /*private void WriteSpreadHistory() {

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
        }*/

        // this one only writes the spread as it sees it every 30 seconds or so.. to reduce the CSV file size.
        private void WriteSpreadHistoryCompressed() {

            string baseFolder = "G:\\My Drive\\IR\\IRTicker\\Spread history data\\";
            if (!Directory.Exists(baseFolder)) {
                Debug.Print("Cannot write spread history info - base folder not accessible or doesn't exist");
                return;
            }
            Debug.Print(DateTime.Now + " - CSV write: G drive folder exists, let's do it.");
            string dataFolder = baseFolder + Environment.UserName + "\\";
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);  // create it if it doesn't exist
            StreamWriter dataWriter;
            try {
                foreach (KeyValuePair<string, DCE> Exchange in DCEs) {  // spin through all the exchanges
                    ConcurrentDictionary<string, List<DataPoint>> spreadHistory = Exchange.Value.GetSpreadHistory();  // DataPoint: OADate, spread
                    foreach (string pair in Exchange.Value.UsablePairs()) {  // spin through all the pairs of this exchange
                        if (spreadHistory.ContainsKey(pair)) {

                            double totalSpread = 0;
                            int avgDivider = 0;

                            foreach (DataPoint dp in spreadHistory[pair]) {  // average out the last hour
                                //Debug.Print("Xval: " + dp.XValue + ", current oadate: " + DateTime.Now.ToOADate() + ", 1 hour ago: " + (DateTime.Now.ToOADate() - 0.041666666));
                                if (dp.XValue > (DateTime.Now.ToOADate() - 0.041666666)) {  // 0.0416666666666 is 1 hour in OADate format.  we average out the last hour
                                    totalSpread += dp.YValues[0];
                                    avgDivider++;
                                }
                            }

                            if (avgDivider > 0) {
                                totalSpread = totalSpread / avgDivider;  // just a bit of variable reuse going on here.

                                Debug.Print("CSV write: " + Exchange.Value.CodeName + " (" + pair + "), " + avgDivider + " datapoints averaged to a spread of $" + totalSpread);
                                dataWriter = new StreamWriter(dataFolder + Exchange.Value.CodeName + "-" + pair + " - compressed.csv", append: true);
                                dataWriter.WriteLine(string.Join(",", DateTime.Now.ToOADate(), totalSpread));
                                dataWriter.Close();
                            }
                        }
                    }
                }
                Debug.Print("Write CSV: done.");
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

        private void IR_CurrencyBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["IR"].CurrencyCombo = IR_CurrencyBox.SelectedItem.ToString();
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

        private void IR_XRP_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "XRP-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-XRP-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
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

        private void ExportSummarised_Checkbox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.ExportSummarised = ExportSummarised_Checkbox.Checked;
            Properties.Settings.Default.Save();
        }

        // this sub sets the cursor position to the far right when control enters the num coins masked text box
        // if the cursor is anywhere else, it limits the amount of characters you can type in the box.  really annoying.
        private void PositionCursorInMaskedTextBox(MaskedTextBox mtb) {
            if (mtb == null) return;

            int pos = mtb.SelectionStart;

            if (pos > mtb.Text.Length)
                pos = mtb.Text.Length;

            BeginInvoke((MethodInvoker)delegate () { mtb.Select(mtb.Text.Length, 0); });
        }

        private void IR_NumCoinsTextBox_Enter(object sender, EventArgs e) {
            PositionCursorInMaskedTextBox((MaskedTextBox)sender);
        }

        private void BTCM_NumCoinsTextBox_Enter(object sender, EventArgs e) {
            PositionCursorInMaskedTextBox((MaskedTextBox)sender);
        }

        private void GDAX_NumCoinsTextBox_Enter(object sender, EventArgs e) {
            PositionCursorInMaskedTextBox((MaskedTextBox)sender);
        }

        private void BFX_NumCoinsTextBox_Enter(object sender, EventArgs e) {
            PositionCursorInMaskedTextBox((MaskedTextBox)sender);
        }

        private void IR_ZRX_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "ZRX-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-ZRX-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }

        private void IR_OMG_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "OMG-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-OMG-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }

        private void BTCM_OMG_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BTCM"], "OMG-" + DCEs["BTCM"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BTCM-OMG-" + DCEs["BTCM"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_OMG_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "OMG-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-OMG-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void BFX_ZRX_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BFX"], "ZRX-" + DCEs["BFX"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BFX-ZRX-" + DCEs["BFX"].CurrentSecondaryCurrency, SGForm);
        }

        private void CSPT_XRP_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["CSPT"], "XRP-" + DCEs["CSPT"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("CSPT-XRP-" + DCEs["CSPT"].CurrentSecondaryCurrency, SGForm);
        }

        private void IR_Reset_Button_Click(object sender, EventArgs e) {
            wSocketConnect.IR_Disconnect();
            DCEs["IR"].CurrentDCEStatus = "Resetting...";
            Debug.Print(DateTime.Now + " - IR reset button clicked");
            APIDown(UIControls_Dict["IR"].dExchange_GB, "IR");
            DCEs["IR"].socketsReset = true;
        }

        private void BlinkStickBW_DoWork(object sender, DoWorkEventArgs e) {
            RgbColor col = (RgbColor)e.Argument;

            int pulseLength = 700;
            //int repeats = 15000 / pulseLength;

            //bStick.Pulse(col, repeats, pulseLength, 50);

            do {
                if (bStick != null && bStick.OpenDevice()) {
                    try {
                        bStick.Pulse(col, 1, pulseLength, 50);
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " -- BS -- caught an exception in BW: " + ex.Message);
                    }
                }
                if (BlinkStickBW.CancellationPending == true) {
                    break;
                }
            } while (true);
        }

        private void BlinkStickBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            // update blink stick
            Dictionary<string, DCE.MarketSummary> IRpairs = DCEs["IR"].GetCryptoPairs();
            Dictionary<string, DCE.MarketSummary> BTCMpairs = DCEs["BTCM"].GetCryptoPairs();

            decimal IRvol = IRpairs["XBT-AUD"].DayVolume;
            decimal BTCMvol = BTCMpairs["XBT-AUD"].DayVolume;
            //Debug.Print("hoping for FALSE here - isBusy for blink is: " + BlinkStickBW.IsBusy);

            setStickColour(IRvol, BTCMvol);
        }

        private void BlinkStickWhite_Thread_DoWork(object sender, DoWorkEventArgs e) {

            RgbColor col = (RgbColor)e.Argument;

            int pulseLength = 200;
            //Debug.Print(DateTime.Now + " - BS - white thread should pulse a colour");

            if (bStick != null && bStick.OpenDevice()) {
                try {
                    //bStick.Pulse(col, 1, pulseLength, 50);
                    bStick.Morph(col, pulseLength);
                    bStick.Morph("#C19E6E", pulseLength);
                }
                catch (Exception ex) {
                    Debug.Print(DateTime.Now + " -- BS -- caught an exception in white thread: " + ex.Message);
                }
            }
        }

        // To support flashing.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;

        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        // Do the flashing - this does not involve a raincoat.
        public static bool FlashWindowEx(Form form) {
            IntPtr hWnd = form.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }

        private void Slack_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.Slack = Slack_checkBox.Checked;
            if (Slack_checkBox.Checked) {
                slackDefaultNameTextBox.Enabled = true;
                slackNameChangeCheckBox.Enabled = true;
                slackToken_textBox.Enabled = true;
            }
            else {
                slackDefaultNameTextBox.Enabled = false;
                slackNameChangeCheckBox.Enabled = false;
                slackToken_textBox.Enabled = false;
            }
            Properties.Settings.Default.Save();
        }

        private void flashForm_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.FlashForm = flashForm_checkBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void OB_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.ShowOB = OB_checkBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void slackNameChangeCheckBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.SlackNameChange = slackNameChangeCheckBox.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.SlackNameChange && slackDefaultNameTextBox.Text == string.Empty) {
                MessageBox.Show("If you leave the name blank here and the app has already changed your name, then the app won't know what to change it back to and your display name will be blank (meaning your display name will default to your real name).  I recommend you leave your preferred display name in here", "No name?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
