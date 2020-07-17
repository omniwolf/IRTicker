using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;
using BlinkStickDotNet;
using System.Runtime.InteropServices;
using System.Reactive.Threading.Tasks;
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

        PrivateIR pIR = new PrivateIR();
        public readonly SynchronizationContext synchronizationContext;  // use this to do UI stuff from the market baiter thread

        TelegramBot TGBot = null;

        OBview obv = new OBview();

        public IRTicker() {
            InitializeComponent();

            Debug.Print("");
            Debug.Print("----------------");
            Debug.Print("IR TICKER BEGINS");
            Debug.Print("----------------");
            Debug.Print("");

            // populate Session started labels
            SessionStartedAbs_label.Text = DateTime.Now.ToString("g");

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
                { "BAR" }
            };

            DCEs = new Dictionary<string, DCE> {

                // seed the DCEs dictionary with empty DCEs for the DCEs we will be interrogating
                { "IR", new DCE("IR", "Independent Reserve") },
                { "BTCM", new DCE("BTCM", "BTC Markets") },
                { "GDAX", new DCE("GDAX", "Coinbase Pro") },
                { "BFX", new DCE("BFX", "BitFinex") },
                { "BAR", new DCE("BAR", "Bitaroo") }
            };

            DCEs["IR"].BaseURL = "https://api.independentreserve.com";
            //DCEs["IR"].BaseURL = "https://dev.api.independentreserve.net";

            synchronizationContext = SynchronizationContext.Current;  // for the market baiter thread, see IRTicker.Private.cs

            // BTCM, BFX, and BAR have no APIs that let you download the currency pairs, so just set them manually
            // Actually I'm not sure about the above comment, i think some of them do?  But the main issue is most of them have
            // currencies that we don't want to deal with, so we set the currencies manually here.  IR we want all currencies, so
            // we use the API.  This is probably not really smart, as the UI is static, so when new currencies turn up IR breaks.  meh
            DCEs["BTCM"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\",\"OMG\",\"XLM\",\"BAT\",\"GNT\",\"ETC\",\"BSV\",\"LINK\"";
            DCEs["BTCM"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["BTCM"].HasStaticData = false;  // want to set this to false so we run the subscribe code once.

            DCEs["BFX"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\",\"OMG\",\"ZRX\",\"EOS\",\"XLM\",\"BAT\",\"REP\",\"GNT\",\"ETC\",\"BSV\",\"USDT\"";
            DCEs["BFX"].SecondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            DCEs["GDAX"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"ZRX\",\"XRP\",\"XLM\",\"REP\",\"ETC\",\"LINK\"";
            DCEs["GDAX"].SecondaryCurrencyCodes = "\"USD\",\"EUR\",\"GBP\"";

            DCEs["BAR"].PrimaryCurrencyCodes = "\"XBT\"";
            DCEs["BAR"].SecondaryCurrencyCodes = "\"AUD\"";

            InitialiseUIControls();

            // initialise settings

            // if they have somehow set it below 20 secs, force back to 20... or 10
            if (int.TryParse(Properties.Settings.Default.RefreshFreq, out int freq)) {
                if (freq < minRefreshFrequency) Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            }
            else Properties.Settings.Default.RefreshFreq = minRefreshFrequency.ToString();
            Properties.Settings.Default.Save();

            refreshFrequencyTextbox.Text = Properties.Settings.Default.RefreshFreq.ToString();
            EnableGDAXLevel3_CheckBox.Checked = Properties.Settings.Default.FullGDAXOB;

            ExportSummarised_Checkbox.Checked = Properties.Settings.Default.ExportSummarised;
            spreadHistoryCustomFolderValue_Textbox.Text = Properties.Settings.Default.SpreadHistoryCustomFolder;
            if (Properties.Settings.Default.ExportSummarised) spreadHistoryCustomFolderValue_Textbox.Enabled = true;
            else spreadHistoryCustomFolderValue_Textbox.Enabled = false;

            Slack_checkBox.Checked = Properties.Settings.Default.Slack;
            flashForm_checkBox.Checked = Properties.Settings.Default.FlashForm;
            slackToken_textBox.Text = Properties.Settings.Default.SlackToken;
            slackDefaultNameTextBox.Text = Properties.Settings.Default.SlackDefaultName;
            slackNameChangeCheckBox.Checked = Properties.Settings.Default.SlackNameChange;
            OB_checkBox.Checked = Properties.Settings.Default.ShowOB;
            UITimerFreq_maskedTextBox.Text = Properties.Settings.Default.UITimerFreq.ToString();
            NegativeSpread_checkBox.Checked = Properties.Settings.Default.NegativeSpread;
            TelegramCode_textBox.Text = Properties.Settings.Default.TelegramCode;
            if (string.IsNullOrEmpty(Properties.Settings.Default.SlackNameCurrency)) Properties.Settings.Default.SlackNameCurrency = "AUD";

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

            if (!string.IsNullOrEmpty(Properties.Settings.Default.TelegramCode)) {
                TGBot = new TelegramBot(pIR, DCEs["IR"]);
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey) || string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey)) {
                IRAccount_button.Enabled = false;
                pIR = null;
            }
            else {
                pIR.PrivateIR_init(DCEs["IR"].BaseURL, Properties.Settings.Default.IRAPIPubKey, Properties.Settings.Default.IRAPIPrivKey, this, DCEs["IR"], TGBot);
            }

            wSocketConnect = new WebSocketsConnect(DCEs, pollingThread, pIR);



            if (Properties.Settings.Default.ShowOB) obv.Show();

            VersionLabel.Text = "IR Ticker version " + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;

            AccountBuySell_listbox.SelectedIndex = 0;
            AccountOrderType_listbox.SelectedIndex = 0;

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
                { "BAR", new UIControls() }
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
            UIControls_Dict["IR"].PMGT_Label = IR_PMGT_Label1;
            UIControls_Dict["IR"].PMGT_Price = IR_PMGT_Label2;
            UIControls_Dict["IR"].PMGT_Spread = IR_PMGT_Label3;
            UIControls_Dict["IR"].LINK_Label = IR_LINK_Label1;
            UIControls_Dict["IR"].LINK_Price = IR_LINK_Label2;
            UIControls_Dict["IR"].LINK_Spread = IR_LINK_Label3;
            UIControls_Dict["IR"].AvgPrice_BuySell = IR_BuySellComboBox;
            UIControls_Dict["IR"].AvgPrice_NumCoins = IR_NumCoinsTextBox;
            UIControls_Dict["IR"].AvgPrice_Crypto = IR_CryptoComboBox;
            UIControls_Dict["IR"].AvgPrice_Currency = IR_CurrencyBox;
            UIControls_Dict["IR"].AvgPrice = IR_AvgPrice_Label;
            UIControls_Dict["IR"].Account_XBT_Label = AccountXBT_label;
            UIControls_Dict["IR"].Account_XBT_Value = AccountXBT_value;
            UIControls_Dict["IR"].Account_ETH_Value = AccountETH_value;
            UIControls_Dict["IR"].Account_ETH_Label = AccountETH_label;
            UIControls_Dict["IR"].Account_XRP_Value = AccountXRP_value;
            UIControls_Dict["IR"].Account_XRP_Label = AccountXRP_label;
            UIControls_Dict["IR"].Account_BCH_Value = AccountBCH_value;
            UIControls_Dict["IR"].Account_BCH_Label = AccountBCH_label;
            UIControls_Dict["IR"].Account_BSV_Value = AccountBSV_value;
            UIControls_Dict["IR"].Account_BSV_Label = AccountBSV_label;
            UIControls_Dict["IR"].Account_USDT_Value = AccountUSDT_value;
            UIControls_Dict["IR"].Account_USDT_Label = AccountUSDT_label;
            UIControls_Dict["IR"].Account_LTC_Value = AccountLTC_value;
            UIControls_Dict["IR"].Account_LTC_Label = AccountLTC_label;
            UIControls_Dict["IR"].Account_EOS_Value = AccountEOS_value;
            UIControls_Dict["IR"].Account_EOS_Label = AccountEOS_label;
            UIControls_Dict["IR"].Account_XLM_Value = AccountXLM_value;
            UIControls_Dict["IR"].Account_XLM_Label = AccountXLM_label;
            UIControls_Dict["IR"].Account_ETC_Value = AccountETC_value;
            UIControls_Dict["IR"].Account_ETC_Label = AccountETC_label;
            UIControls_Dict["IR"].Account_BAT_Value = AccountBAT_value;
            UIControls_Dict["IR"].Account_BAT_Label = AccountBAT_label;
            UIControls_Dict["IR"].Account_OMG_Value = AccountOMG_value;
            UIControls_Dict["IR"].Account_OMG_Label = AccountOMG_label;
            UIControls_Dict["IR"].Account_REP_Value = AccountREP_value;
            UIControls_Dict["IR"].Account_REP_Label = AccountREP_label;
            UIControls_Dict["IR"].Account_ZRX_Value = AccountZRX_value;
            UIControls_Dict["IR"].Account_ZRX_Label = AccountZRX_label;
            UIControls_Dict["IR"].Account_GNT_Value = AccountGNT_value;
            UIControls_Dict["IR"].Account_GNT_Label = AccountGNT_label;
            UIControls_Dict["IR"].Account_PMGT_Value = AccountPMGT_value;
            UIControls_Dict["IR"].Account_PMGT_Label = AccountPMGT_label;
            UIControls_Dict["IR"].Account_LINK_Value = AccountLINK_value;
            UIControls_Dict["IR"].Account_LINK_Label = AccountLINK_label;
            UIControls_Dict["IR"].Account_XBT_Total = AccountXBT_total;
            UIControls_Dict["IR"].Account_ETH_Total = AccountETH_total;
            UIControls_Dict["IR"].Account_XRP_Total = AccountXRP_total;
            UIControls_Dict["IR"].Account_BCH_Total = AccountBCH_total;
            UIControls_Dict["IR"].Account_BSV_Total = AccountBSV_total;
            UIControls_Dict["IR"].Account_USDT_Total = AccountUSDT_total;
            UIControls_Dict["IR"].Account_LTC_Total = AccountLTC_total;
            UIControls_Dict["IR"].Account_EOS_Total = AccountEOS_total;
            UIControls_Dict["IR"].Account_XLM_Total = AccountXLM_total;
            UIControls_Dict["IR"].Account_ETC_Total = AccountETC_total;
            UIControls_Dict["IR"].Account_BAT_Total = AccountBAT_total;
            UIControls_Dict["IR"].Account_OMG_Total = AccountOMG_total;
            UIControls_Dict["IR"].Account_REP_Total = AccountREP_total;
            UIControls_Dict["IR"].Account_ZRX_Total = AccountZRX_total;
            UIControls_Dict["IR"].Account_GNT_Total = AccountGNT_total;
            UIControls_Dict["IR"].Account_PMGT_Total = AccountPMGT_total;
            UIControls_Dict["IR"].Account_LINK_Total = AccountLINK_total;
            UIControls_Dict["IR"].Account_AUD_Total = AccountAUD_total;
            UIControls_Dict["IR"].Account_AUD_Label = AccountAUD_label;
            UIControls_Dict["IR"].Account_NZD_Total = AccountNZD_total;
            UIControls_Dict["IR"].Account_NZD_Label = AccountNZD_label;
            UIControls_Dict["IR"].Account_USD_Total = AccountUSD_total;
            UIControls_Dict["IR"].Account_USD_Label = AccountUSD_label;

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
            UIControls_Dict["BTCM"].LINK_Label = BTCM_LINK_Label1;
            UIControls_Dict["BTCM"].LINK_Price = BTCM_LINK_Label2;
            UIControls_Dict["BTCM"].LINK_Spread = BTCM_LINK_Label3;
            UIControls_Dict["BTCM"].AvgPrice_BuySell = BTCM_BuySellComboBox;
            UIControls_Dict["BTCM"].AvgPrice_NumCoins = BTCM_NumCoinsTextBox;
            UIControls_Dict["BTCM"].AvgPrice_Crypto = BTCM_CryptoComboBox;
            UIControls_Dict["BTCM"].AvgPrice_Currency = BTCM_CurrencyBox;
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
            UIControls_Dict["GDAX"].LINK_Label = GDAX_LINK_Label1;
            UIControls_Dict["GDAX"].LINK_Price = GDAX_LINK_Label2;
            UIControls_Dict["GDAX"].LINK_Spread = GDAX_LINK_Label3;
            UIControls_Dict["GDAX"].AvgPrice_BuySell = GDAX_BuySellComboBox;
            UIControls_Dict["GDAX"].AvgPrice_NumCoins = GDAX_NumCoinsTextBox;
            UIControls_Dict["GDAX"].AvgPrice_Crypto = GDAX_CryptoComboBox;
            UIControls_Dict["GDAX"].AvgPrice_Currency = GDAX_CurrencyBox;
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
            UIControls_Dict["BFX"].AvgPrice_Currency = BFX_CurrencyBox;
            UIControls_Dict["BFX"].AvgPrice = BFX_AvgPrice_Label;

            // Bitaroo
            UIControls_Dict["BAR"].Name = "BAR";
            UIControls_Dict["BAR"].dExchange_GB = BAR_GroupBox;
            UIControls_Dict["BAR"].XBT_Label = BAR_XBT_Label1;
            UIControls_Dict["BAR"].XBT_Price = BAR_XBT_Label2;
            UIControls_Dict["BAR"].XBT_Spread = BAR_XBT_Label3;
            UIControls_Dict["BAR"].AvgPrice_BuySell = BAR_BuySellComboBox;
            UIControls_Dict["BAR"].AvgPrice_NumCoins = BAR_NumCoinsTextBox;
            UIControls_Dict["BAR"].AvgPrice_Crypto = BAR_CryptoComboBox;
            UIControls_Dict["BAR"].AvgPrice_Currency = BAR_CurrencyBox;  // will require a lot of changes, basically meaning all other exchanges will be able to do this.  a job for later
            UIControls_Dict["BAR"].AvgPrice = BAR_AvgPrice_Label;

            foreach (KeyValuePair<string, UIControls> uic in UIControls_Dict) {
                uic.Value.CreateControlDictionaries();  // builds the internal dictionaries so the controls themselves can be iterated over
                if (uic.Value.AvgPrice != null) {
                    uic.Value.AvgPrice_BuySell.SelectedIndex = 0;  // force all the buy/sell drop downs to select buy (so can never be null)
                }
                uic.Value.AvgPrice_Currency.SelectedIndex = 1;
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

        private void setSlackStatus(decimal IRBTCvol, decimal BTCMBTCvol, bool disable = false) {
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

                if (disable) {
                    slackObj.setStatus("", ":crying_cat_face:", 1, "");  // slack has been disabled.  set the name back to normal and the emoji to crying cat for 1 second
                    return;
                }

                name = Properties.Settings.Default.SlackDefaultName;
                if (!DCEs["IR"].socketsAlive || !DCEs["IR"].NetworkAvailable || IRBTCvol < 0) {
                    slackObj.setStatus("", ":exclamation:", 120, name + " - IR API down");
                    return;
                }
                else if (!DCEs["BTCM"].socketsAlive || !DCEs["BTCM"].NetworkAvailable || BTCMBTCvol < 0) {
                    slackObj.setStatus("", ":face_with_rolling_eyes:", 120, name + " - BTCM API down");
                    return;
                }

                //string tempName = UIControls_Dict["IR"].Label_Dict["XBT_Price"].Text;
                Dictionary<string, DCE.MarketSummary> cPairs = DCEs["IR"].GetCryptoPairs();
                if (cPairs.ContainsKey("XBT-" + Properties.Settings.Default.SlackNameCurrency)) {
                    decimal bid = cPairs["XBT-" + Properties.Settings.Default.SlackNameCurrency].CurrentHighestBidPrice;
                    decimal offer = cPairs["XBT-" + Properties.Settings.Default.SlackNameCurrency].CurrentLowestOfferPrice;
                    string midPoint = Utilities.FormatValue(Math.Round(((bid + offer) / 2), 0));

                    //string tempName = ((cPairs["XBT-" + Properties.Settings.Default.SlackNameCurrency].CurrentLowestOfferPrice - cPairs["XBT-" + Properties.Settings.Default.SlackNameCurrency].CurrentHighestBidPrice) / 2).ToString();

                    //if (tempName.Length >= 3) tempName = tempName.Substring(0, tempName.Length - 3);  // remove decimal places from the price
                    name += " - " + Properties.Settings.Default.SlackNameCurrency + " " + midPoint;
                }
            }
            //Debug.Print("slack name is: " + name);

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
        private void ParseDCE_IR(string crypto, string fiat, bool updateLabels) {
            Tuple<bool, string> marketSummary = Utilities.Get(DCEs["IR"].BaseURL + "/Public/GetMarketSummary?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);
            if (!marketSummary.Item1) {
                DCEs["IR"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["IR"].NetworkAvailable = false;
            }
            else {
                DCEs["IR"].NetworkAvailable = true;
                DCE.MarketSummary mSummary;
                try {
                    mSummary = JsonConvert.DeserializeObject<DCE.MarketSummary>(marketSummary.Item2);
                }
                catch {
                    Debug.Print(DateTime.Now + " - IR - bad REST result: " + marketSummary.Item2);
                    DCEs["IR"].NetworkAvailable = false;
                    return;
                }

                // This bit is for a) volume (we don't get vol from websockets), and b) if there have been no orders to establish a spread, then the price and spread
                // stay at 0.  This is 
                Dictionary<string, DCE.MarketSummary> cPairs = DCEs["IR"].GetCryptoPairs();
                if (cPairs.ContainsKey(mSummary.pair) && cPairs[mSummary.pair].spread != 0) {  // logic here is if the spread is not 0, then don't send spread info, as what we have is better
                //if (crypto == "XBT") {  // don't want to overwrite the spread orders as they're probably out of date
                    mSummary.CurrentHighestBidPrice = 0;  // sending cryptoPairsAdd a 0 bid and offer will mean the previous best bid and offer remain
                    mSummary.CurrentLowestOfferPrice = 0;
                }
                //}
                mSummary.CreatedTimestampUTC = "";
                DCEs["IR"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                DCEs["IR"].CurrentDCEStatus = "Online";
                // don't update the labels if we are pulling a different fiat than the one we're showing (eg for Slack name)
                if (updateLabels && (DCEs["IR"].CurrentSecondaryCurrency == fiat)) pollingThread.ReportProgress(21, mSummary);
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
                mSummary.DayVolumeXbt = mSummary_BTCM.volume24h;
                mSummary.DayHighestPrice = mSummary_BTCM.low24h;
                mSummary.DayLowestPrice = mSummary_BTCM.high24h;

                DCEs["BTCM"].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs

                DCEs["BTCM"].CurrentDCEStatus = "Online";
                DCEs["BTCM"].NetworkAvailable = true;
                pollingThread.ReportProgress(31, mSummary);
            }
        }

        private void ParseDCE_BAR() {
            Tuple<bool, string> marketSummary = Utilities.Get("https://api.bitaroo.com.au/trade/market-data/btcaud");
            if (!marketSummary.Item1) {
                DCEs["BAR"].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                DCEs["BAR"].NetworkAvailable = false;
            }
            else {
                DCE.MarketSummary_BAR mSummary_BAR = JsonConvert.DeserializeObject<DCE.MarketSummary_BAR>(marketSummary.Item2);

                DCE.MarketSummary mSummary = new DCE.MarketSummary();

                if ((mSummary_BAR.orderBook.buy.Count > 0) && decimal.TryParse(mSummary_BAR.orderBook.buy.FirstOrDefault().price, out decimal bid)) {
                    mSummary.CurrentHighestBidPrice = bid;
                }
                else Debug.Print("could not convert BAR bid: " + mSummary_BAR.orderBook.buy.FirstOrDefault().price);

                if ((mSummary_BAR.orderBook.sell.Count > 0) && decimal.TryParse(mSummary_BAR.orderBook.sell.FirstOrDefault().price, out decimal offer)) {
                    mSummary.CurrentLowestOfferPrice = offer;
                }
                else Debug.Print("could not convert BAR offer: " + mSummary_BAR.orderBook.sell.FirstOrDefault().price);

                if (decimal.TryParse(mSummary_BAR.dailyStats.high, out decimal high)) {
                    mSummary.DayHighestPrice = high;
                }
                else Debug.Print("could not convert BAR highest price: " + mSummary_BAR.dailyStats.high);

                if (decimal.TryParse(mSummary_BAR.dailyStats.low, out decimal low)) {
                    mSummary.DayLowestPrice = low;
                }
                else Debug.Print("could not convert BAR lowest price: " + mSummary_BAR.dailyStats.low);

                if (decimal.TryParse(mSummary_BAR.dailyStats.volDst, out decimal vol)) {
                    mSummary.DayVolumeXbt = vol;
                }
                else Debug.Print("could not convert BAR vol: " + mSummary_BAR.dailyStats.volDst);

                if (decimal.TryParse(mSummary_BAR.dailyStats.volSrc, out decimal volFiat)) {
                    mSummary.DayVolumeInSecondaryCurrency = volFiat;
                }
                else Debug.Print("could not convert BAR vol in fiat: " + mSummary_BAR.dailyStats.volSrc);

                if (decimal.TryParse(mSummary_BAR.dailyStats.lastPrice, out decimal lastPrice)) {
                    mSummary.LastPrice = lastPrice;
                }
                else Debug.Print("could not convert BAR last price: " + mSummary_BAR.dailyStats.lastPrice);

                if (mSummary_BAR.pairSymbol == "btcaud") mSummary.pair = "XBT-AUD";  // this will populate both primary and secondary currencies for this mSummary obj
                else Debug.Print("Bitaroo not sending btcaud as the pair?? - " + mSummary_BAR.pairSymbol);


                DCEs["BAR"].CryptoPairsAdd(mSummary.pair, mSummary);

                ParseOrderBook_BAR(mSummary_BAR.orderBook, mSummary.pair);  // the REST response sends the OB as well, so may as well parse it.
                
                DCEs["BAR"].NetworkAvailable = true;
                DCEs["BAR"].CurrentDCEStatus = "Online";
            }
        }

        private void ParseOrderBook_BAR (DCE.OrderBook_BAR OB_BAR, string pair) {

            // every single entry is a string, and I don't wan to do a decimal.tryParse() around every call, so just do a try on the whole thing
            try {
                if (!DCEs["BAR"].orderBooks.ContainsKey(pair)) {
                    DCEs["BAR"].orderBooks[pair] = new DCE.OrderBook();
                    DCEs["BAR"].orderBooks[pair].PrimaryCurrencyCode = Utilities.SplitPair(pair).Item1;
                    DCEs["BAR"].orderBooks[pair].SecondaryCurrencyCode = Utilities.SplitPair(pair).Item2;
                    DCEs["BAR"].orderBooks[pair].CreatedTimestampUtc = DateTime.Now;
                }
                else {  // the OB_BAR object contains a complete copy of the order book, so we clear what we have and repace with this.
                    DCEs["BAR"].orderBooks[pair].BuyOrders.Clear();
                    DCEs["BAR"].orderBooks[pair].SellOrders.Clear();
                }

                DCE.OrderBook OB = DCEs["BAR"].orderBooks[pair];
                foreach (DCE.Buy_BAR buyOrder in OB_BAR.buy) {
                    OB.BuyOrders.Add(new DCE.Order("LimitBid", decimal.Parse(buyOrder.price), decimal.Parse(buyOrder.amount), "1"));
                }
                foreach (DCE.Sell_BAR sellOrder in OB_BAR.sell) {
                    OB.SellOrders.Add(new DCE.Order("LimitOffer", decimal.Parse(sellOrder.price), decimal.Parse(sellOrder.amount), "1"));
                }
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - BAR failed to parse the " + pair + " order book, probably one of the strings couldn't be turned into a decimal. error: " + ex.Message);
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
            }
        }

        public void SubscribeTickerSocket(string dExchange) {
            // subscribe to all the pairs
            // don't do it this way anymore
            /*List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();

            if (true) { // dExchange == "IR") {
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
            }*/
            wSocketConnect.subscribe_unsubscribe_new(dExchange, true);  // this sub is now kinda useless
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
                    UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Add(splitPair.Item1 == "XBT" ? "BTC" : splitPair.Item1);
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

                foreach (string dExchange in Exchanges) {
                    pollingThread.ReportProgress(2, dExchange);  // we need to lock the average price controls here so they user doesn't change them while the data is getting pulled
                }

                //Debug.Print("Begin API poll");


                ////// IR ///////
                if(!DCEs["IR"].HasStaticData) {  // only pull the currencies once per session as these are essentially static
                    Tuple<bool, string> primaryCurrencyCodesTpl = Utilities.Get(DCEs["IR"].BaseURL + "/Public/GetValidPrimaryCurrencyCodes");
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

                    Tuple<bool, string> secondaryCurrencyCodesTpl = Utilities.Get(DCEs["IR"].BaseURL + "/Public/GetValidSecondaryCurrencyCodes");
                    if (!secondaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = WebsiteError(secondaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = true;
                        DCEs["IR"].CurrentDCEStatus = "Online";
                        DCEs["IR"].SecondaryCurrencyCodes = Utilities.TrimEnds(secondaryCurrencyCodesTpl.Item2);
                        //DCEs["IR"].SecondaryCurrencyCodes = "\"AUD\"";

                        pollingThread.ReportProgress(28, "IR");  // populate slack name currency combobox in settnigs
                    }
                    if (DCEs["IR"].NetworkAvailable) {
                        DCEs["IR"].HasStaticData = true;  // we got here with the network up?  then we got the static data!
                        Dictionary<string, DCE.products_GDAX> productDictionary_IR = new Dictionary<string, DCE.products_GDAX>();
                        foreach (string crypto in DCEs["IR"].PrimaryCurrencyList) {
                            foreach (string fiat in DCEs["IR"].SecondaryCurrencyList) {
                                productDictionary_IR.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                                DCEs["IR"].negSpreadCount[crypto + "-" + fiat] = 0;  // init

                                //create OB objects ready to be filled.  we only do this once here, and never delete them.  neverrrrr
                                DCEs["IR"].InitialiseOrderBookDicts_IR(crypto, fiat);
                                if (DCEs["IR"].CurrentSecondaryCurrency == fiat) ParseDCE_IR(crypto, fiat, true);  // initial data pull and display
                            }
                        }
                        /*DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "AUD");
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "USD");
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "NZD");*/

                        IRCurrencies.IRCurrenciesRoot CurrencyRoot;
                        using (StreamReader r = new StreamReader("IRCurrencyAttributes.txt")) {
                            string json = r.ReadToEnd();
                            CurrencyRoot = JsonConvert.DeserializeObject<IRCurrencies.IRCurrenciesRoot>(json);
                        }

                        //Dictionary<string, DCE.MarketSummary> cryptos = DCEs["IR"].GetCryptoPairs();
                        foreach (string currency in DCEs["IR"].PrimaryCurrencyList) {
                            switch (currency) {
                                case "XBT":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Xbt.FiatPriceDecimalPlaces)).ToString()));
                                    break;
                                case "ETH":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Eth.FiatPriceDecimalPlaces)).ToString()));
                                    break;
                                case "XRP":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Xrp.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "BCH":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Bch.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "BSV":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Bsv.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "USDT":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Ust.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "LTC":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Ltc.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "EOS":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Eos.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "XLM":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Xlm.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "ETC":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Etc.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "BAT":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Bat.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "OMG":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Omg.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "REP":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Rep.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "ZRX":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Zrx.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "GNT":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Gnt.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "PMGT":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Pmg.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                                case "LINK":
                                    DCEs["IR"].currencyFiatDivision.Add(currency, decimal.Parse((Math.Pow(0.1, CurrencyRoot.Lnk.FiatPriceDecimalPlaces)).ToString("0.#######################################")));
                                    break;
                            }

                            
                        }

                        DCEs["IR"].ExchangeProducts = productDictionary_IR;

                        wSocketConnect.Reinit_sockets("IR");  // this will setup all the necessary dictionaries
                        SubscribeTickerSocket("IR");
                        pollingThread.ReportProgress(14, "IR");
                    }
                    else {
                        pollingThread.ReportProgress(12, "IR");
                    }
                }

                // still need to run this to get volume data (and all coins except BTC)
                foreach (string primaryCode in DCEs["IR"].PrimaryCurrencyList) {
                    // if there's no crypto selected in the drop down or there's no number of coins entered, then just pull the market summary
                    if (loopCount == 0 || !shitCoins.Contains(primaryCode)) {
                        ParseDCE_IR(primaryCode, DCEs["IR"].CurrentSecondaryCurrency, false);
                    }
                    
                    //if (DCEs["IR"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["IR"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                    //GetIROrderBook(primaryCode, );
                    //}
                }

                // need to pull this other fiat currency market summary data if our chose slack currency is not the one we're looking at (and the slack stuff is enabled)
                if ((Properties.Settings.Default.SlackNameCurrency != DCEs["IR"].CurrentSecondaryCurrency) && 
                    Properties.Settings.Default.Slack && Properties.Settings.Default.SlackNameChange) {

                    ParseDCE_IR("XBT", Properties.Settings.Default.SlackNameCurrency, false);
                }

                // let's get all the closed orders and notify the user if there are new ones
                foreach (string crypto in DCEs["IR"].PrimaryCurrencyList) {
                    foreach (string fiat in DCEs["IR"].SecondaryCurrencyList) {
                        if (pIR != null) {
                            pIR.GetClosedOrders(crypto, fiat).Wait();
                            /*if (cOrders != null) {

                                synchronizationContext.Post(new SendOrPostCallback(o => {

                                    drawClosedOrders((Task<Page<BankHistoyOrder>>)o.Result).Data);
                                }), cOrders);*/
                        }

                    }
                }

                //pIR.GetClosedOrders("XBT", "AUD");


                // let's check the IR spread.  Cycle through all the "_Spread" labels

                if (Properties.Settings.Default.NegativeSpread) {
                    Dictionary<string, DCE.MarketSummary> mSummaries = DCEs["IR"].GetCryptoPairs();
                    foreach (var mSummary in mSummaries) {
                        if (mSummary.Value.SecondaryCurrencyCode == DCEs["IR"].CurrentSecondaryCurrency) {
                            if (mSummary.Value.CurrentLowestOfferPrice <= mSummary.Value.CurrentHighestBidPrice) {
                                if (!DCEs["IR"].positiveSpread[mSummary.Value.pair]) {  // already been negative for this pair :(
                                    Debug.Print(DateTime.Now + " - negative pair detected (" + mSummary.Value.pair + ").  let's unsub resub");
                                    // do something..
                                    pollingThread.ReportProgress(29, mSummary.Value.pair);  // update UI to show another spread fail
                                    wSocketConnect.WebSocket_Resubscribe("IR", mSummary.Value.PrimaryCurrencyCode);
                                }
                                else {
                                    // spread was positive last time, set the signal and wait for the next rotation
                                    DCEs["IR"].positiveSpread[mSummary.Value.pair] = false;
                                    Debug.Print(DateTime.Now + " - Negave pair (" + mSummary.Value.pair + ") signaled, waiting...");
                                }
                            }
                            else {
                                DCEs["IR"].positiveSpread[mSummary.Value.pair] = true;  // this pair is OK.  Doesn't mean it's right, it's just not DEFINITELY wrong.
                                //Debug.Print("Negative spread check all good for " + mSummary.Value.pair);
                            }
                        }
                    }
                }


                //////// BTC Markets /////////


                foreach (string primaryCode in DCEs["BTCM"].PrimaryCurrencyList) {

                    if (DCEs["BTCM"].CryptoCombo == primaryCode && !string.IsNullOrEmpty(DCEs["BTCM"].NumCoinsStr)) {  // we have a crypto selected and coins entered, let's get the order book for them
                        GetBTCMOrderBook(primaryCode);
                    }
                }

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
                    pollingThread.ReportProgress(14, "BTCM");
                    DCEs["BTCM"].HasStaticData = true;
                }


                //////// GDAX ///////

                if (!DCEs["GDAX"].HasStaticData) {
                    GetGDAXProducts();
                    Debug.Print("calling gdax sockets sub");
                    SubscribeTickerSocket("GDAX");
                    pollingThread.ReportProgress(14, "GDAX");
                }

                //////// BitFinex /////////

                if (!DCEs["BFX"].HasStaticData) {
                    GetBFXProducts();  // pulls bfx pairs, and starts the websockets connection
                    pollingThread.ReportProgress(14, "BFX");  // populate crypto drop down
                    SubscribeTickerSocket("BFX");
                }

                //////// Bitaroo ////////
                ///

                if (!DCEs["BAR"].HasStaticData) {
                    DCEs["BAR"].HasStaticData = true;  // BAR has no static data APIs, so just say we have it.
                    DCEs["BAR"].socketsAlive = true;  // Bitaroo has no sockets, so just leave this as true so everything is happy.
                    Dictionary<string, DCE.products_GDAX> productDictionary_BAR = new Dictionary<string, DCE.products_GDAX>();
                    productDictionary_BAR.Add("XBT-AUD", new DCE.products_GDAX("XBT-AUD"));
                    DCEs["BAR"].ExchangeProducts = productDictionary_BAR;  // manually create an ExchangeProduct for this exchange so we save spread data
                    pollingThread.ReportProgress(14, "BAR");
                }
                
                ParseDCE_BAR();  // the bitaroo parseDCE method is a bit different, no need to loop through pairs, there is only one endpoint we call and it has all the info on all the pairs.   This might sound cool, but it's actually shit.  CoinSpot API is shittttt
                

                if (pollingThread.CancellationPending) {  // this will be true if the user has changed the secondary currency.  we need to stop and start again, because it's possible that we have
                    e.Cancel = true;  // pulled the old secondary currency already from the API
                    Debug.Print("Poll cancelled, middle!");
                    break;  // will break out of our big ol' loop
                }

                // do a loop through the exchanges and do some common stuff
                foreach (string dExchange in Exchanges) {
                    if (dExchange == "BAR") continue;
                    // the heartbeat is initialised as the year 2000, so if it's this year we know it must be just starting up, no need to worry
                    if ((DCEs[dExchange].HeartBeat + TimeSpan.FromSeconds(100) < DateTime.Now) && DCEs[dExchange].HeartBeat.Year != 2000) {
                        // we haven't received a heartbeat in 100 seconds..
                        Debug.Print(DateTime.Now + " - " + dExchange + " - haven't received any messages via sockets in 100 seconds.  reconnecting..");
                        DCEs[dExchange].socketsAlive = false;
                        DCEs[dExchange].socketsReset = true;
                        pollingThread.ReportProgress(12, dExchange);
                    }

                    // separate this because it's possible to hit this code where the socketsreset == true for some other reason that heartbeat
                    if (DCEs[dExchange].socketsReset) {
                        // just in case sockets is still broken, let's grab some REST data
                        foreach (string primaryCode in DCEs[dExchange].PrimaryCurrencyList) {
                            switch (dExchange) {
                                case "IR":
                                    ParseDCE_IR(primaryCode, DCEs[dExchange].CurrentSecondaryCurrency, true);
                                    break;
                                case "BTCM":
                                    ParseDCE_BTCM(primaryCode, DCEs[dExchange].CurrentSecondaryCurrency);
                                    break;
                            }
                        }
                        DCEs[dExchange].socketsReset = false;
                        // ok we need to reset the socket.
                        Debug.Print(DateTime.Now + " - " + dExchange + " - REST data pulled, now restarting sockets from backgroundWorker");
                        wSocketConnect.WebSocket_Reconnect(dExchange);
                    }

                    if (loopCount == 0) {
                        if (!wSocketConnect.IsSocketAlive(dExchange)) {
                            Debug.Print(dExchange + " ded, reconnecting");
                            wSocketConnect.WebSocket_Reconnect(dExchange);
                        }
                    }
                }


                //////// fiat rates /////////
                if (refreshFiat) {
                    ParseFiat_OER("USD", "AUD,NZD,EUR,USD,SGD");  // only run this once per session as we have limited fx API calls.
                    refreshFiat = false;
                }

                // OK we now have all the DCE and fiat rates info loaded.

                pollingThread.ReportProgress(1);
                /*  we do a loop now
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
                }*/

                foreach (string dExchange in Exchanges) {
                    string cCombo = DCEs[dExchange].CryptoCombo;
                    if (cCombo == "BTC") cCombo = "XBT";
                    if (cCombo != "" && !string.IsNullOrEmpty(DCEs[dExchange].NumCoinsStr) && DCEs[dExchange].HasStaticData) {  // we have a crypto selected and coins entered, let's get the order book for them
                        pollingThread.ReportProgress(2, dExchange);  // OK let's lock the fields down
                        switch (dExchange) {
                            case "BTCM":
                                GetBTCMOrderBook(cCombo);
                                break;
                            case "GDAX":
                                GetGDAXOrderBook(cCombo);
                                break;
                            case "BFX":
                                GetBFXOrderBook(cCombo);
                                break;
                        }
                        pollingThread.ReportProgress(13, dExchange);  // display order book
                    }
                }

                loopCount++;
                if (loopCount >= shitCoinPollRate) {
                    loopCount = 0;  // reset it, it's time we poll the shit coins again
                }

                if (Properties.Settings.Default.ExportSummarised && (lastCSVWrite + TimeSpan.FromHours(1) < DateTime.Now )) {
                    WriteSpreadHistoryCompressed();
                    lastCSVWrite = DateTime.Now;  // whether or not we write to the CSV, don't try again for another hour.  Setting this AFTER we call the writeSpreadHistoryCompressed() sub so worst case we miss a couple of datapoints rather than duplicate them.
                }

                if (int.TryParse(refreshFrequencyTextbox.Text, out int refreshInt)) {
                    Thread.Sleep(refreshInt * 1000);
                }
                else {
                    System.Windows.MessageBox.Show("couldn't parse the refresh time.. why?  text: " + refreshFrequencyTextbox.Text);
                    Thread.Sleep(10000);
                }

            } while(true);  // polling is lyfe
        }

        private void UpdateLabels(string dExchange) {
            if (!Main.Visible) return;  // no point drawing to the UI if we can't see anything
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

                    decimal midPoint = (pairObj.Value.CurrentHighestBidPrice + pairObj.Value.CurrentLowestOfferPrice) / 2;

                    // we use this price label so often and it's so much text to access it, i want to just create a quick variable to make the code easier to read
                    System.Windows.Forms.Label tempPrice = UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Price"];
                    tempPrice.Text = Utilities.FormatValue(midPoint);
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

                    UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"].Text = Utilities.FormatValue(pairObj.Value.spread) + ((pairObj.Value.DayVolumeXbt == 0) ? " / 0" : " / " + Utilities.FormatValue(pairObj.Value.DayVolumeXbt));

                    // update tool tips.
                    IRTickerTT_spread.SetToolTip(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"], "Best bid: " + Utilities.FormatValue(pairObj.Value.CurrentHighestBidPrice) + System.Environment.NewLine + "Best offer: " + Utilities.FormatValue(pairObj.Value.CurrentLowestOfferPrice));
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
            if (!Main.Visible) return;  // no point drawing to the UI if we can't see anything
            // first we reset the labels.  The label writing process only writes to labels of pairs that exist, so we first need to set them back in case they don't exist

            //DCE.MarketSummary mSummary = DCEs[dExchange].GetCryptoPairs()[crypto + "-" + fiat];

            // i guess we need to filter out the wrong pairs, also don't try and update labels that are -1 (-1 means they're a fake entry)
            if (mSummary.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency && mSummary.LastPrice >= 0) {

                // we have a legit pair we're about to update.  if the groupBox is grey, let's black it.
                GroupBoxAndLabelColourActive(dExchange);
                UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";

                decimal midPoint = (mSummary.CurrentHighestBidPrice + mSummary.CurrentLowestOfferPrice) / 2;  // we don't use last price anymore, instead the midpoint of the spread

                System.Windows.Forms.Label tempPrice = UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"];

                tempPrice.Text = Utilities.FormatValue(midPoint);
                tempPrice.ForeColor = Utilities.PriceColour(DCEs[dExchange].GetPriceList(mSummary.pair));
                // don't do this anymore, because we buffer and hold event, the buffer will often legitimately have events in it, so this isn't (anmymore) an indication that there's a nonce issue
                // if we're experiencing nonce errors for this pair, make it gray.
                //if ((DCEs[dExchange].orderBuffer_IR.ContainsKey(mSummary.pair.ToUpper())) && DCEs[dExchange].orderBuffer_IR[mSummary.pair.ToUpper()].Count > 0) {
                //    tempPrice.ForeColor = Color.Gray;
                //}

                // lets update IRACCOUNTS
                if (IRAccount_panel.Visible && dExchange == "IR") {
                    setCurrencyValues(mSummary.PrimaryCurrencyCode.ToUpper(), mSummary.CurrentHighestBidPrice);
                }


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

                UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"].Text = Utilities.FormatValue(mSummary.spread) + ((mSummary.DayVolumeXbt == 0) ? " / 0" : " / " + Utilities.FormatValue(mSummary.DayVolumeXbt));
                //Debug.Print("ABOUT TO CHECK ORDER BOOK STUFF:");
                //Debug.Print("---num coins = " + UIControls_Dict[dExchange].AvgPrice_NumCoins.Text + " avgprice_crypto = " + (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem == null ? "null" : UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedItem.ToString()));

                if (DCEs[dExchange].ChangedSecondaryCurrency) { 
                    PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                    DCEs[dExchange].ChangedSecondaryCurrency = false;
                }

                // update tool tips.
                IRTickerTT_spread.SetToolTip(UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"], "Best bid: " + Utilities.FormatValue(mSummary.CurrentHighestBidPrice) + System.Environment.NewLine + "Best offer: " + Utilities.FormatValue(mSummary.CurrentLowestOfferPrice));
            }
            else Debug.Print("Pair2 don't exist, pairObj.Value.SecondaryCurrencyCode: " + mSummary.SecondaryCurrencyCode);
        }

        // this works out what the average price of a market order on the OB would be for IR.  We needed to separate this logic from the other exchanges 
        // because IR's order book is represented very differently (and used constantly)
        private string DetermineAveragePrice_IR(string crypto, string fiat, string currency) {
            bool fiatSelected = currency.ToLower() == "fiat";  // if the crypto value is the same as the fiat one, it means that the selected crypto was fiat (ie they chose AUD from the drop avg price dropdown)
            crypto = (crypto == "BTC" ? "XBT" : crypto.ToUpper());
            fiat = fiat.ToUpper();
            string pair = crypto + "-" + fiat;

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
                            IRTickerTT_avgPrice.SetToolTip(UIControls_Dict["IR"].AvgPrice, tTip);
                            
                            return "Average price for " + crypto + ": " + (fiatSelected ? "$" : "") + Utilities.FormatValue(weightedAverage);  // we have finished filling the hypothetical order
                        }
                        else {  // this whole sub order is required, factor it in and then loop
                            weightedAverage += ((subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price : 1)) / coins) * subOrder.Value.Price;
                            totalCost += subOrder.Value.Volume * (fiatSelected ? 1 : subOrder.Value.Price);  // if fiat is selected, totalCost var represents total coins
                            Debug.Print("--- totalCost is now " + totalCost + " and was increased by " + subOrder.Value.Volume * (fiatSelected ? 1 : subOrder.Value.Price));
                        }
                    }
                }
                IRTickerTT_avgPrice.SetToolTip(UIControls_Dict["IR"].AvgPrice, buildAvgPriceTooltip(orderSide, fiatSelected, orderedBook.Last().Key, orderCount, totalCost, crypto));
                return "Order book only has " + (fiatSelected ? "$" : crypto) + " " + Utilities.FormatValue(coinCounter);
            }
            else {
                MessageBox.Show("Please just enter a real number (decimal places are fine).", "Avg price calc for Independent Reserve", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UIControls_Dict["IR"].AvgPrice_NumCoins.Text = "";
            }
            return "";
        }

        // builds the string in the tool tip.  Depending on what 
        private string buildAvgPriceTooltip(string buySell, bool fiatSelected, decimal price, int orderCount, decimal finalAmount, string crypto) {

            StringBuilder AvgPrice_TTStr = new StringBuilder();
            AvgPrice_TTStr.AppendLine((buySell == "Buy" ? "Max" : "Min") + " price paid: " + Utilities.FormatValue(price));
            AvgPrice_TTStr.AppendLine("Orders required to fill: " + orderCount);
            AvgPrice_TTStr.Append("Notional ");

            if (fiatSelected && buySell == "Buy") {
                AvgPrice_TTStr.Append("coins bought: " + (crypto == "XBT" ? "BTC" : crypto) + " ");
            }
            else if (fiatSelected && buySell == "Sell") {
                AvgPrice_TTStr.Append("coins sold: " + (crypto == "XBT" ? "BTC" : crypto) + " ");
            }
            else if (!fiatSelected && buySell == "Buy") {
                AvgPrice_TTStr.Append("fiat cost: $");
            }
            else if (!fiatSelected && buySell == "Sell") {
                AvgPrice_TTStr.Append("fiat received: $");
            }

            AvgPrice_TTStr.Append(Utilities.FormatValue(finalAmount));
            return AvgPrice_TTStr.ToString();
        }

        private string DetermineAveragePrice(string crypto, string fiat, string dExchange) { 

            crypto = (crypto == "BTC" ? "XBT" : crypto.ToUpper());  // gotta convert it back to XBT
            fiat = fiat.ToUpper();
            string pair = crypto + "-" + fiat;
            // if fiat we need to multiply by the current price constantly... or you know.. whatever man.  don't let fiat rule your life
            bool isFiat = UIControls_Dict[dExchange].AvgPrice_Currency.Text.ToUpper() == "FIAT";

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
                decimal totalCost = 0;  // for crypto this is the total cost/profit in fiat, for fiat this is the total coins bought/sold
                int orderCount = 0;
                bool gracefulFinish = false;  // this only gets set to true if the order book has enough coins in it to handle the number of inputted coins.  If it doesn't (ie the foreach completes without us having counted the inputted coins), then we throw a warning message
                foreach (DCE.Order order in orderBook) {
                    orderCount++;
                    coinCounter += order.Volume * (isFiat ? order.Price : 1);
                    if (coinCounter > coins) {  // ok we are on the last value we need to look at.  need to truncate.
                        decimal usedCoinsInThisOrder = (order.Volume * (isFiat ? order.Price : 1)) - (coinCounter - coins);  // this is how many coins in this order would be required
                        totalCost += usedCoinsInThisOrder * (isFiat ? 1 / order.Price : order.Price);
                        weightedAverage += (usedCoinsInThisOrder / coins) * order.Price;
                        gracefulFinish = true;
                        //string tTip = (orderSide == "Buy" ? "Max" : "Min") + " price paid: " + Utilities.FormatValue(order.Price) + System.Environment.NewLine + "Orders required to fill: " + orderCount + System.Environment.NewLine + "Total fiat cost: " + Utilities.FormatValue(totalCost);
                        IRTickerTT_avgPrice.SetToolTip(UIControls_Dict[dExchange].AvgPrice, buildAvgPriceTooltip(orderSide, isFiat, order.Price, orderCount, totalCost, crypto));
                        break;  // we have finished filling the hypothetical order
                    }
                    else {  // this whole order is required
                        decimal test = order.Volume * (isFiat ? order.Price : 1);
                        decimal test2 = test / coins;
                        weightedAverage += ((order.Volume * (isFiat ? order.Price : 1)) / coins) * order.Price;
                        totalCost += order.Volume * (isFiat ? 1 : order.Price);
                    }
                }
                if (!gracefulFinish) {
                    IRTickerTT_avgPrice.SetToolTip(UIControls_Dict[dExchange].AvgPrice, buildAvgPriceTooltip(orderSide, isFiat, orderBook.Last().Price, orderCount, totalCost, crypto));
                    //MessageBox.Show("You requested " + coins + " coins, but the order book's entire volume (that the API returned to us) had only " + coinCounter + " coins in it.  So, the displayed average price will be less than reality, but you probably fat fingered how many coins?", dExchange + "'s order book too small for that number of coins", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "Order book only has " + (isFiat ? "$" : crypto) + " " + Utilities.FormatValue(coinCounter);
                }
                DCEs[dExchange].RemoveOrderBook(pair);  // need to remove once we've used - there's the possibility that the next orderbook API pull fails, then the code will just use the existing order book
                return "Average price for " + crypto + ": " + Utilities.FormatValue(weightedAverage);
            }
            else {
                MessageBox.Show("Please just enter a real number (decimal places are fine).", "Avg price calc for " + DCEs[dExchange].FriendlyName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UIControls_Dict[dExchange].AvgPrice_NumCoins.Text = "";
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
                string dExchange = (string)e.UserState;
                if (UIControls_Dict[dExchange].AvgPrice == null) return; // none of these fields exist for coinspot
                // if they have filled in the order book controls, then disable them while we work it out
                if (UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex > 0 && !string.IsNullOrEmpty(UIControls_Dict[dExchange].AvgPrice_NumCoins.Text)) {
                    UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = false;
                    UIControls_Dict[dExchange].AvgPrice_BuySell.Enabled = false;
                    UIControls_Dict[dExchange].AvgPrice_NumCoins.Enabled = false;
                }
                return;
            }

            if (reportType == 12) {  // 12 is error in the response or API.   either way, we disconnect and start again.
                string dExchange = (string)e.UserState;
                APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
                return;
            }

            if (reportType == 13) {  // order book stuff for avg price thingos
                string dExchange = (string)e.UserState;
                if (dExchange == "IR") {  // unfortunately IR's OB is stored quite differently due to our requirements to use it for working out the current spread over web sockts.  We need a different method
                    UIControls_Dict[dExchange].AvgPrice.Text = DetermineAveragePrice_IR(DCEs[dExchange].CryptoCombo, DCEs[dExchange].CurrentSecondaryCurrency, DCEs[dExchange].CurrencyCombo);
                }
                else {
                    UIControls_Dict[dExchange].AvgPrice.Text = DetermineAveragePrice(DCEs[dExchange].CryptoCombo, DCEs[dExchange].CurrentSecondaryCurrency, dExchange);
                }
                UIControls_Dict[dExchange].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                UIControls_Dict[dExchange].AvgPrice_Crypto.Enabled = true;
                UIControls_Dict[dExchange].AvgPrice_BuySell.Enabled = true;
                UIControls_Dict[dExchange].AvgPrice_NumCoins.Enabled = true;
                return;
            }
            else if (reportType == 14) {  // should only be called once per session - if we don't do this the crypto combo box is empty until we change secondary currencies
                string dExchange = (string)e.UserState;
                PopulateCryptoComboBox(dExchange);
                return;
            }

            /*if (reportType == 20) {
                if (IRAccount_panel.Visible || marketBaiterActive) {
                    string pair = (string)e.UserState;
                    pIR.updateAccountOrderBook(pair);
                }
                return;
            }*/

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

            if (reportType == 28) {  // populate slack name currency combobox in settings

                // now we populate the slack name currency combobox in settings
                foreach (string irFiat in DCEs["IR"].SecondaryCurrencyList) {
                    if (SlackNameCurrency_comboBox.Items.Contains(irFiat)) continue;
                    SlackNameCurrency_comboBox.Items.Add(irFiat);
                }

                SlackNameCurrency_comboBox.Enabled = true;  // have to enable it to change the value :/
                //Debug.Print("properties slack name currency: " + Properties.Settings.Default.SlackNameCurrency + "find string index: " + SlackNameCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameCurrency));
                if (SlackNameCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameCurrency) > -1)
                    SlackNameCurrency_comboBox.SelectedIndex = SlackNameCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameCurrency);

                // after setting the default value, if this control shouldn't be enabled, disable it.
                if (!Properties.Settings.Default.Slack || !Properties.Settings.Default.SlackNameChange || (SlackNameCurrency_comboBox.Items.Count == 0))
                    SlackNameCurrency_comboBox.Enabled = false;

                return;
            }

            if (reportType == 29) {  // updates the IR currency label to include the number of negative spread fail events we've had for this currency
                // do something..
                string pair = (string)e.UserState;
                Tuple<string, string> pairTup = Utilities.SplitPair(pair);
                DCEs["IR"].negSpreadCount[pair]++;
                UIControls_Dict["IR"].Label_Dict[pairTup.Item1 + "_Label"].Text = (pairTup.Item1 == "XBT" ? "BTC" : pairTup.Item1) + " (" + DCEs["IR"].negSpreadCount[pair] + "):";
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

            if (reportType == 63) {  // 63 is order book stuff for bar
                UIControls_Dict["BAR"].AvgPrice.Text = DetermineAveragePrice(DCEs["BAR"].CryptoCombo, DCEs["BAR"].CurrentSecondaryCurrency, "BAR");
                UIControls_Dict["BAR"].AvgPrice.ForeColor = Color.Black;
                UIControls_Dict["BAR"].AvgPrice_Crypto.SelectedIndex = 0;  // reset this so we don't pull the order book every time.
                BAR_CryptoComboBox.Enabled = BAR_BuySellComboBox.Enabled = BAR_NumCoinsTextBox.Enabled = true;
                return;
            }
            else if (reportType == 64) {  // should only be called once per session - if we don't do this the crypto combo box is empty until we change secondary currencies
                PopulateCryptoComboBox("BAR");
                return;
            }

            Dictionary<string, DCE.MarketSummary> IRpairs = DCEs["IR"].GetCryptoPairs();
            Dictionary<string, DCE.MarketSummary> BTCMpairs = DCEs["BTCM"].GetCryptoPairs();
            decimal IRvol = -1, BTCMvol = -1;
            if (IRpairs.ContainsKey("XBT-AUD") && BTCMpairs.ContainsKey("XBT-AUD")) {
                IRvol = IRpairs["XBT-AUD"].DayVolumeXbt; ;
                BTCMvol = BTCMpairs["XBT-AUD"].DayVolumeXbt; 
                
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
                    if (!DCEs[dExchange].socketsAlive) {  // website might be up, this should never be reached.  only exchang to get here should be BAR, and it's sockets will by definition always be "
                        Debug.Print(DateTime.Now + " - 2 setting sockets down, we are in the main reportProgress and socktsAlive is false - " + dExchange);
                        UIControls_Dict[dExchange].dExchange_GB.Text += " - sockets down";
                    }

                    UpdateLabels(dExchange);
                }
                else if (DCEs[dExchange].NetworkAvailable) {  // if we have network but not online, we probably have REST data to send to the UI, so do it.
                    Debug.Print(DateTime.Now + " - updating UI even though the exchange (" + dExchange + ") isn't in an 'online' state");
                    UpdateLabels(dExchange);
                }
                else APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
            }

            // we have updated all the prices, if the average price controls are disabled, we can enable them now
            IR_CryptoComboBox.Enabled = IR_BuySellComboBox.Enabled = IR_NumCoinsTextBox.Enabled = true;
            BTCM_CryptoComboBox.Enabled = BTCM_BuySellComboBox.Enabled = BTCM_NumCoinsTextBox.Enabled = true;

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
            if (e.Cancelled) {  // if it was cancelled, we start it up again.  The only reason it would be cancelled is if the user chooses a different secondary currency.
                Debug.Print(DateTime.Now + " - Poll was cancelled, now restarting...");
                pollingThread.RunWorkerAsync(); // we need to cancel to make sure we haven't already pulled the old currency from the API
            }
            else {
                Debug.Print(DateTime.Now + " - POLL stopped!! why?? " + e.Result + " " + e.Error + " " + e.ToString());
            }
        }

        // when they close the app, rename the crypto dirs to blah - old.  this way if they user happens to check the toolbar thing they'll know they're not being updated anymore
        private void IRTicker_Closing(object sender, FormClosingEventArgs e) {

            if (marketBaiterActive) {
                DialogResult result = MessageBox.Show("Market Baiter still active, you should cancel it before closing the app or the order will stay on the order book!" + Environment.NewLine + Environment.NewLine +
                    "Are you sure you want to close IRTicker?",
                    "Trading bot still going", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No) {
                    e.Cancel = true;
                    return;
                }
            }
           
           // turn off the blink stick.
           if (bStick != null) {
                if (bStick.OpenDevice()) {
                    BlinkStickBW.CancelAsync();
                    bStick.TurnOff();
                }
           }

            IRAccount_panel.Visible = false;

           if (Properties.Settings.Default.Slack && (Properties.Settings.Default.SlackToken != "")) {
                slackObj.setStatus("", "");
           }
            wSocketConnect.IR_Disconnect();  // let's see if this stops the occasional crash on exit
            wSocketConnect.stopUITimerThread();  // needed otherwise the app never actually closes
        }

        private void SettingsButton_Click(object sender, EventArgs e) {
            // update session started relative label
            TimeSpan session = DateTime.Now - DateTime.Parse(SessionStartedAbs_label.Text);
            SessionStartedRel_label.Text = session.ToString("%d") + " day(s), " + session.ToString("%h") + " hour(s), " + session.ToString("%m") + " min(s)";

            populateIRAPIKeysSettings();  // populate the api keys drop down.

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

                    if (!IRAccount_button.Enabled && 
                        !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey) &&
                        !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey)) {

                        pIR.PrivateIR_init(DCEs["IR"].BaseURL, Properties.Settings.Default.IRAPIPubKey, Properties.Settings.Default.IRAPIPrivKey, this, DCEs["IR"], TGBot);
                        IRAccount_button.Enabled = true;
                    }
                    else if (string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey) || string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey)) {
                        IRAccount_button.Enabled = false;
                    }

                    if (!string.IsNullOrEmpty(TelegramCode_textBox.Text)) {

                        Properties.Settings.Default.TelegramCode = TelegramCode_textBox.Text;
                        if (TGBot == null) {
                            TGBot = new TelegramBot(pIR, DCEs["IR"]);
                            pIR.setTGBot(TGBot);
                        }
                    }
                    else {
                        TGBot = null;  // hopefully this will dispose of the bot and it will responding..
                        pIR.setTGBot(null);
                        Properties.Settings.Default.TelegramCode = "";
                        Properties.Settings.Default.TelegramChatID = 0;
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
                case "BAR":
                    fColour = Color.DarkTurquoise;
                    break;
                default:
                    fColour = Color.Black;
                    break;
            }

            UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Black;

            foreach (KeyValuePair<string, System.Windows.Forms.Label> UICobj in UIControls_Dict[dExchange].Label_Dict) {
                if ((string)UICobj.Value.Tag == "DCECryptoLabel") {
                    UICobj.Value.ForeColor = fColour;
                }
            }
        }

        private void GroupBox_Click(string dExchange) {
            string oldFiat = DCEs[dExchange].CurrentSecondaryCurrency;
            DCEs[dExchange].NextSecondaryCurrency();
            wSocketConnect.WebSocket_Resubscribe(dExchange, "none", oldFiat, DCEs[dExchange].CurrentSecondaryCurrency);

            ParseExchangeThreadStarter(dExchange);  // here we start a quick thread pull volume data for IR
            UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Gray;
            UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + DCEs[dExchange].CurrentSecondaryCurrency + ")";

            if (UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Count > 0) UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset the selection to blank

            Utilities.ColourDCETags(Controls, dExchange);
            DCEs[dExchange].ChangedSecondaryCurrency = true;
        }

        // this starts the thread.  I return the thread, but in reality I don't do anything with it, so it's just discarded.  Maybe I should make sure the thread isn't hanging...
        public Thread ParseExchangeThreadStarter(string dExchange) {
            var t = new Thread(() => ParseExchangeThreadWorker(dExchange));
            t.Start();
            return t;
        }

        private void ParseExchangeThreadWorker(string dExchange) {
            switch (dExchange) {
                case "IR":
                    foreach (string crypto in DCEs[dExchange].PrimaryCurrencyList) {
                        ParseDCE_IR(crypto, DCEs[dExchange].CurrentSecondaryCurrency, false);
                    }
                    break;
            }
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

            //string baseFolder = "G:\\My Drive\\IR\\IRTicker\\Spread history data\\";
            string baseFolder = Properties.Settings.Default.SpreadHistoryCustomFolder;
            if (!Directory.Exists(baseFolder)) {
                Debug.Print("Cannot write spread history info - base folder not accessible or doesn't exist");
                return;
            }
            Debug.Print(DateTime.Now + " - CSV write: " + baseFolder + " exists, let's do it.");
            string dataFolder = baseFolder + "\\" + Environment.UserName + "\\";
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

        private void BAR_BuySellComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BAR"].BuySell = BAR_BuySellComboBox.SelectedItem.ToString();
            BAR_AvgPrice_Label.Text = "";
        }

        private void BAR_NumCoinsTextBox_TextChanged(object sender, EventArgs e) {
            DCEs["BAR"].NumCoinsStr = BAR_NumCoinsTextBox.Text;
            BAR_AvgPrice_Label.Text = "";
        }

        private void BAR_CryptoComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            DCEs["BAR"].CryptoCombo = BAR_CryptoComboBox.SelectedItem.ToString();
        }

        private void BAR_CryptoComboBox_DropDown(object sender, EventArgs e) {
            BAR_AvgPrice_Label.Text = "";
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
            SpreadGraph SGForm = new SpreadGraph(DCEs["BAR"], "XBT-" + DCEs["BAR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BAR-XBT-" + DCEs["BAR"].CurrentSecondaryCurrency, SGForm);
        }

        private void Help_Button_Click(object sender, EventArgs e) {
            Help helpForm = new Help(this);
            helpForm.Show();
            Help_Button.Enabled = false;
        }

        private void ExportSummarised_Checkbox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.ExportSummarised = ExportSummarised_Checkbox.Checked;
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.ExportSummarised) spreadHistoryCustomFolderValue_Textbox.Enabled = true;
            else spreadHistoryCustomFolderValue_Textbox.Enabled = false;
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

        private void IR_Reset_Button_Click(object sender, EventArgs e) {
            wSocketConnect.IR_Disconnect();
            DCEs["IR"].CurrentDCEStatus = "Resetting...";
            Debug.Print(DateTime.Now + " - IR reset button clicked");
            APIDown(UIControls_Dict["IR"].dExchange_GB, "IR");
            //DCEs["IR"].socketsReset = true;
        }

        private void BlinkStickBW_DoWork(object sender, DoWorkEventArgs e) {
            RgbColor col = (RgbColor)e.Argument;

            int pulseLength = 700;
            //int repeats = 15000 / pulseLength;

            //bStick.Pulse(col, repeats, pulseLength, 50);

            do {
                if (BlinkStickBW.CancellationPending == true) {
                    break;
                }
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

            decimal IRvol = IRpairs["XBT-AUD"].DayVolumeXbt;
            decimal BTCMvol = BTCMpairs["XBT-AUD"].DayVolumeXbt;
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
                if (Properties.Settings.Default.SlackNameChange && (SlackNameCurrency_comboBox.Items.Count > 0)) SlackNameCurrency_comboBox.Enabled = true;
            }
            else {
                slackDefaultNameTextBox.Enabled = false;
                slackNameChangeCheckBox.Enabled = false;
                slackToken_textBox.Enabled = false;
                setSlackStatus(0, 0, true);  // reset the slack name to the default
                SlackNameCurrency_comboBox.Enabled = false;
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

            if (Properties.Settings.Default.SlackNameChange && (SlackNameCurrency_comboBox.Items.Count > 0)) SlackNameCurrency_comboBox.Enabled = true;
            else SlackNameCurrency_comboBox.Enabled = false;
        }

        private void NegativeSpread_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.NegativeSpread = NegativeSpread_checkBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void SlackNameCurrency_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            Properties.Settings.Default.SlackNameCurrency = (string)SlackNameCurrency_comboBox.SelectedItem;
            Properties.Settings.Default.Save();
        }

        private void spreadHistoryCustomFolderValue_Textbox_Click(object sender, EventArgs e) {
            if (Properties.Settings.Default.ExportSummarised) {  // only do something if we're actually exporting
                DialogResult result = spreadHistory_FolderDialog.ShowDialog();
                if (result == DialogResult.OK) {
                    Properties.Settings.Default.SpreadHistoryCustomFolder = spreadHistory_FolderDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                    spreadHistoryCustomFolderValue_Textbox.Text = Properties.Settings.Default.SpreadHistoryCustomFolder;
                }
            }
        }

        private void IRAccount_button_Click(object sender, EventArgs e) {
            InitialiseAccountsPanel();
        }

        private void EditKeys_button_Click(object sender, EventArgs e) {
            Form EditKeys = new AccountAPIKeys();
            EditKeys.ShowDialog();

            populateIRAPIKeysSettings();
        }

        private void populateIRAPIKeysSettings() { 

            APIKeys_comboBox.Items.Clear();
                        
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly1) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey1) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey1)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly1, Properties.Settings.Default.IRAPIPubKey1, Properties.Settings.Default.IRAPIPrivKey1);
                APIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly2) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey2) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey2)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly2, Properties.Settings.Default.IRAPIPubKey2, Properties.Settings.Default.IRAPIPrivKey2);
                APIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly3) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey3) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey3)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly3, Properties.Settings.Default.IRAPIPubKey3, Properties.Settings.Default.IRAPIPrivKey3);
                APIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly4) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey4) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey4)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly4, Properties.Settings.Default.IRAPIPubKey4, Properties.Settings.Default.IRAPIPrivKey4);
                APIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly5) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey5) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey5)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly5, Properties.Settings.Default.IRAPIPubKey5, Properties.Settings.Default.IRAPIPrivKey5);
                APIKeys_comboBox.Items.Add(grp);
            }

            bool foundKey = false;
            foreach (AccountAPIKeys.APIKeyGroup chosenKey in APIKeys_comboBox.Items) {
                if (chosenKey.friendlyName == Properties.Settings.Default.APIFriendly) {
                    //Select this one somehow..
                    APIKeys_comboBox.SelectedItem = chosenKey;
                    foundKey = true;
                }
            }
            if (!foundKey) {
                Properties.Settings.Default.APIFriendly = "";
                Properties.Settings.Default.IRAPIPubKey = "";
                Properties.Settings.Default.IRAPIPrivKey = "";
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup("", "", "");
                APIKeys_comboBox.Items.Add(grp);
                APIKeys_comboBox.SelectedItem = grp;
                Properties.Settings.Default.Save();
            }
        }

        private void APIKeys_comboBox_SelectedIndexChanged(object sender, EventArgs e) {

            if (Properties.Settings.Default.APIFriendly != ((AccountAPIKeys.APIKeyGroup)APIKeys_comboBox.SelectedItem).friendlyName) {
                // a new key has been chosen, let's reset the closed orders.
            }

            Properties.Settings.Default.APIFriendly = ((AccountAPIKeys.APIKeyGroup)APIKeys_comboBox.SelectedItem).friendlyName;
            Properties.Settings.Default.IRAPIPubKey = ((AccountAPIKeys.APIKeyGroup)APIKeys_comboBox.SelectedItem).pubKey;
            Properties.Settings.Default.IRAPIPrivKey = ((AccountAPIKeys.APIKeyGroup)APIKeys_comboBox.SelectedItem).privKey;

            pIR.PrivateIR_init(DCEs["IR"].BaseURL, Properties.Settings.Default.IRAPIPubKey, Properties.Settings.Default.IRAPIPrivKey, this, DCEs["IR"], TGBot);
        }
    }
}
