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
using System.Threading.Tasks;

// todo:
// if the volume in IRAccounts is more than the availabe balance, maybe we can make the vol input field background red or something.  same on avg price window?
// blinkstick tasks, a non-null CTS is never returned from the method, are we doing this right?

namespace IRTicker {
    public partial class IRTicker : Form {
        private const String APP_ID = "Nikola.IRTicker";
        private const int minRefreshFrequency = 10;

        private Dictionary<string, DCE> DCEs;  // the master dictionary that holds all the data we pull from all the crypto APIs
        public Dictionary<string, UIControls> UIControls_Dict;
        private List<string> Exchanges;
        List<string> IRdExchanges = new List<string>() { "IR", "IRUSD", "IRSGD" };
        private List<string> shitCoins = new List<string>() { "BCH", "LTC", "XRP", "EOS", "OMG", "ZRX", "XLM", "BAT", "USDT", "DOT", "GRT", "AAVE", "YFI", "PMGT", "SNX", "COMP", "LINK", "ADA", "UNI", "MATIC", "DOGE", "ADA", "MANA", "SOL", "SAND" };  // we don't poll the shit coins as often to help with rate limiting
        private int shitCoinPollRate = 3; // this is how many polls we loop before we call shit coin APIs.  eg 3 means we only poll the shit coins once every 3 polls.
        private WebSocketsConnect wSocketConnect;
        private BlinkStick bStick, bStickETH, bStickUSDT, bStickXRP;
        private long lastBlock = 0;  // this holds the last block the app knows about
        private Task taskPulseBTC, taskPulseETH, taskPulseUSDT, taskPulseXRP;
        private CancellationTokenSource cTokenPulseBTC, cTokenPulseETH, cTokenPulseUSDT, cTokenPulseXRP;
        private Slack slackObj = new Slack();
        private DateTime lastCSVWrite = DateTime.Now;  // this holds the time we last saved the CSV file
        private decimal BTCfee = 0;  // holds the estimated BTC network fee for the next block in sats/byte
        private decimal ETHfee = 0;  // holds the estimated ETH network fee for the next block in gwei

        public ConcurrentDictionary<string, SpreadGraph> SpreadGraph_Dict = new ConcurrentDictionary<string, SpreadGraph>();  // needs to be public because it gets accessed from the graphs object

        PrivateIR pIR = new PrivateIR();
        public readonly SynchronizationContext synchronizationContext;  // use this to do UI stuff from the market baiter thread

        IRAccountsForm IRAF;

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

            //bStick = BlinkStick.FindFirst();
            var bSticks = BlinkStick.FindAll();

            foreach (BlinkStick bStick in bSticks) {
                Debug.Print("Found blinkStick: " + bSticks[0].Meta.Serial);
            }


            /*if (bSticks.Length == 4) {
                Debug.Print("bs1: " + bSticks[0].Meta.Serial);
                Debug.Print("bs2: " + bSticks[1].Meta.Serial);
                Debug.Print("bs3: " + bSticks[2].Meta.Serial);
                Debug.Print("bs4: " + bSticks[3].Meta.Serial);*/
            if (bSticks.Length > 0) {
                int i = 0;
                do {
                    switch (bSticks[i].Meta.Serial) {
                        case "BS032958-3.0":
                            bStickETH = bSticks[i];
                            break;
                        case "BS041767-3.0":
                            bStickUSDT = bSticks[i];
                            break;
                        case "BS028603-3.0":
                            bStick = bSticks[i];  // BTC
                            break;
                        case "BS041736-3.0":
                            bStickXRP = bSticks[i];
                            break;
                    }

                    if (bSticks[i].OpenDevice())  // activateee
                        bSticks[i].Blink("yellow", 1, 200);
                    else
                        Debug.Print("BlinkStick " + bSticks[i].Meta.Serial + " couldn't be accessed or opened");


                    i++;
                } while (i < bSticks.Length);
                //}

                if (bSticks.Length == 1) {
                    bStick = bSticks[0];  // if we only have 1 blink stick, then make it BTC
                    if (bStick != null && bStick.OpenDevice()) {
                        bStick.Blink("yellow", 1, 200);
                        bStick.Blink("blue", 1, 200);
                    }
                    else {
                        Debug.Print("Only 1 BlinkStick. Tried assigning it to BTC, but it couldn't be accessed or opened");
                    }
                }
            }

            if (refreshFrequencyTextbox.Text == "1") refreshFrequencyTextbox.Text = minRefreshFrequency.ToString();  // design time default is 1, we set to our actual min

            Exchanges = new List<string> {
                { "IR" },
                { "IRSGD" },
                { "IRUSD" },
                { "BTCM" },
                { "BAR" }
            };

            DCEs = new Dictionary<string, DCE> {

                // seed the DCEs dictionary with empty DCEs for the DCEs we will be interrogating
                { "IR", new DCE("IR", "Independent Reserve") },
                { "IRSGD", new DCE("IRSGD", "Independent Reserve (SGD)") },
                { "IRUSD", new DCE("IRUSD", "Independent Reserve (USD)") },
                { "BTCM", new DCE("BTCM", "BTC Markets") },
                { "BAR", new DCE("BAR", "Bitaroo") }
            };

            DCEs["IR"].BaseURL = "https://api.independentreserve.com";
            //DCEs["IR"].BaseURL = "https://dev.api.independentreserve.net";
            DCEs["IRSGD"].BaseURL = "https://api.independentreserve.com";
            DCEs["IRUSD"].BaseURL = "https://api.independentreserve.com";

            synchronizationContext = SynchronizationContext.Current;  // for the market baiter thread, see IRTicker.Private.cs

            // BTCM, BFX, and BAR have no APIs that let you download the currency pairs, so just set them manually
            // Actually I'm not sure about the above comment, i think some of them do?  But the main issue is most of them have
            // currencies that we don't want to deal with, so we set the currencies manually here.  IR we want all currencies, so
            // we use the API.  This is probably not really smart, as the UI is static, so when new currencies turn up IR breaks.  meh
            DCEs["IRSGD"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\",\"DOT\",\"XLM\",\"GRT\",\"ETC\",\"LINK\",\"USDC\",\"USDT\",\"UNI\",\"ADA\",\"MATIC\",\"DOGE\",\"SAND\",\"MANA\",\"SOL\"";
            DCEs["IRSGD"].SecondaryCurrencyCodes = "\"SGD\"";

            DCEs["IRUSD"].SecondaryCurrencyCodes = "\"USD\"";

            DCEs["BTCM"].PrimaryCurrencyCodes = "\"XBT\",\"ETH\",\"BCH\",\"LTC\",\"XRP\",\"OMG\",\"XLM\",\"BAT\",\"ETC\",\"LINK\",\"COMP\",\"USDT\",\"UNI\",\"SAND\",\"MANA\",\"USDC\",\"AAVE\"";
            DCEs["BTCM"].SecondaryCurrencyCodes = "\"AUD\"";
            DCEs["BTCM"].HasStaticData = false;  // want to set this to false so we run the subscribe code once.

            DCEs["BAR"].PrimaryCurrencyCodes = "\"XBT\"";
            DCEs["BAR"].SecondaryCurrencyCodes = "\"AUD\"";

            InitialiseUIControls();

            // initialise settings

            Settings.AutoScroll = false;
            Settings.HorizontalScroll.Enabled = false;
            Settings.HorizontalScroll.Visible = false;
            Settings.HorizontalScroll.Maximum = 0;
            Settings.VerticalScroll.Visible = false;
            Settings.AutoScroll = true;

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
            TelegramBotAPIToken_textBox.Text = Properties.Settings.Default.TelegramAPIToken;
            TGBot_Enable_checkBox.Checked = Properties.Settings.Default.TGBot_Enable;
            TelegramNewMessages_checkBox.Checked = Properties.Settings.Default.TelegramAllNewMessages;
            if (string.IsNullOrEmpty(Properties.Settings.Default.SlackNameFiatCurrency)) Properties.Settings.Default.SlackNameFiatCurrency = "AUD";

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

            if (Properties.Settings.Default.TGBot_Enable && !string.IsNullOrEmpty(Properties.Settings.Default.TelegramCode) && !string.IsNullOrEmpty(Properties.Settings.Default.TelegramAPIToken)) {
                try {
                    TGBot = new TelegramBot(Properties.Settings.Default.TelegramAPIToken, pIR, DCEs["IR"], this);
                }
                catch (Exception ex) {
                    MessageBox.Show("Error creating TelegramBot.  Maybe wrong API token?  Error mesage: " + Environment.NewLine + Environment.NewLine +
                        ex.Message, "Telegram Bot error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (TGBot != null) {
                        TGBot.StopBot();
                        TGBot = null;
                    }
                }
            }

            wSocketConnect = new WebSocketsConnect(DCEs, pollingThread, pIR);
            //LastPanel = Main;


            if (Properties.Settings.Default.ShowOB) obv.Show();

            VersionLabel.Text = "IR Ticker version " + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;

            pollingThread.RunWorkerAsync();
        }

        // super manual function to push the UI controls into objects so we can read them programattically
        private void InitialiseUIControls() {

            UIControls_Dict = new Dictionary<string, UIControls> {

                // seed the UIControls_Dict dictionary with empty UIControls
                { "IR", new UIControls() },
                { "IRSGD", new UIControls() },
                { "IRUSD", new UIControls() },
                { "BTCM", new UIControls() },
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
            UIControls_Dict["IR"].MKR_Label = IR_MKR_Label1;
            UIControls_Dict["IR"].MKR_Price = IR_MKR_Label2;
            UIControls_Dict["IR"].MKR_Spread = IR_MKR_Label3;
            UIControls_Dict["IR"].ETC_Label = IR_ETC_Label1;
            UIControls_Dict["IR"].ETC_Price = IR_ETC_Label2;
            UIControls_Dict["IR"].ETC_Spread = IR_ETC_Label3;
            UIControls_Dict["IR"].USDT_Label = IR_USDT_Label1;
            UIControls_Dict["IR"].USDT_Price = IR_USDT_Label2;
            UIControls_Dict["IR"].USDT_Spread = IR_USDT_Label3;
            UIControls_Dict["IR"].DAI_Label = IR_DAI_Label1;
            UIControls_Dict["IR"].DAI_Price = IR_DAI_Label2;
            UIControls_Dict["IR"].DAI_Spread = IR_DAI_Label3;
            UIControls_Dict["IR"].LINK_Label = IR_LINK_Label1;
            UIControls_Dict["IR"].LINK_Price = IR_LINK_Label2;
            UIControls_Dict["IR"].LINK_Spread = IR_LINK_Label3;
            UIControls_Dict["IR"].USDC_Label = IR_USDC_Label1;
            UIControls_Dict["IR"].USDC_Price = IR_USDC_Label2;
            UIControls_Dict["IR"].USDC_Spread = IR_USDC_Label3;
            UIControls_Dict["IR"].COMP_Label = IR_COMP_Label1;
            UIControls_Dict["IR"].COMP_Price = IR_COMP_Label2;
            UIControls_Dict["IR"].COMP_Spread = IR_COMP_Label3;
            UIControls_Dict["IR"].SNX_Label = IR_SNX_Label1;
            UIControls_Dict["IR"].SNX_Price = IR_SNX_Label2;
            UIControls_Dict["IR"].SNX_Spread = IR_SNX_Label3;
            UIControls_Dict["IR"].PMGT_Label = IR_PMGT_Label1;
            UIControls_Dict["IR"].PMGT_Price = IR_PMGT_Label2;
            UIControls_Dict["IR"].PMGT_Spread = IR_PMGT_Label3;
            UIControls_Dict["IR"].YFI_Label = IR_YFI_Label1;
            UIControls_Dict["IR"].YFI_Price = IR_YFI_Label2;
            UIControls_Dict["IR"].YFI_Spread = IR_YFI_Label3;
            UIControls_Dict["IR"].AAVE_Label = IR_AAVE_Label1;
            UIControls_Dict["IR"].AAVE_Price = IR_AAVE_Label2;
            UIControls_Dict["IR"].AAVE_Spread = IR_AAVE_Label3;
            UIControls_Dict["IR"].DOT_Label = IR_DOT_Label1;
            UIControls_Dict["IR"].DOT_Price = IR_DOT_Label2;
            UIControls_Dict["IR"].DOT_Spread = IR_DOT_Label3;
            UIControls_Dict["IR"].GRT_Label = IR_GRT_Label1;
            UIControls_Dict["IR"].GRT_Price = IR_GRT_Label2;
            UIControls_Dict["IR"].GRT_Spread = IR_GRT_Label3;
            UIControls_Dict["IR"].UNI_Label = IR_UNI_Label1;
            UIControls_Dict["IR"].UNI_Price = IR_UNI_Label2;
            UIControls_Dict["IR"].UNI_Spread = IR_UNI_Label3;
            UIControls_Dict["IR"].ADA_Label = IR_ADA_Label1;
            UIControls_Dict["IR"].ADA_Price = IR_ADA_Label2;
            UIControls_Dict["IR"].ADA_Spread = IR_ADA_Label3;
            UIControls_Dict["IR"].DOGE_Label = IR_DOGE_Label1;
            UIControls_Dict["IR"].DOGE_Price = IR_DOGE_Label2;
            UIControls_Dict["IR"].DOGE_Spread = IR_DOGE_Label3;
            UIControls_Dict["IR"].MATIC_Label = IR_MATIC_Label1;
            UIControls_Dict["IR"].MATIC_Price = IR_MATIC_Label2;
            UIControls_Dict["IR"].MATIC_Spread = IR_MATIC_Label3;
            UIControls_Dict["IR"].MANA_Label = IR_MANA_Label1;
            UIControls_Dict["IR"].MANA_Price = IR_MANA_Label2;
            UIControls_Dict["IR"].MANA_Spread = IR_MANA_Label3;
            UIControls_Dict["IR"].SOL_Label = IR_SOL_Label1;
            UIControls_Dict["IR"].SOL_Price = IR_SOL_Label2;
            UIControls_Dict["IR"].SOL_Spread = IR_SOL_Label3;
            UIControls_Dict["IR"].SAND_Label = IR_SAND_Label1;
            UIControls_Dict["IR"].SAND_Price = IR_SAND_Label2;
            UIControls_Dict["IR"].SAND_Spread = IR_SAND_Label3;

            UIControls_Dict["IR"].AvgPrice_BuySell = IR_BuySellComboBox;
            UIControls_Dict["IR"].AvgPrice_NumCoins = IR_NumCoinsTextBox;
            UIControls_Dict["IR"].AvgPrice_Crypto = IR_CryptoComboBox;
            UIControls_Dict["IR"].AvgPrice_Currency = IR_CurrencyBox;
            UIControls_Dict["IR"].AvgPrice = IR_AvgPrice_Label;


            // IR SGD
            UIControls_Dict["IRSGD"].Name = "IRSGD";
            UIControls_Dict["IRSGD"].dExchange_GB = IRSGD_GroupBox;
            UIControls_Dict["IRSGD"].XBT_Label = IRSGD_XBT_Label1;
            UIControls_Dict["IRSGD"].XBT_Price = IRSGD_XBT_Label2;
            UIControls_Dict["IRSGD"].XBT_Spread = IRSGD_XBT_Label3;
            UIControls_Dict["IRSGD"].ETH_Label = IRSGD_ETH_Label1;
            UIControls_Dict["IRSGD"].ETH_Price = IRSGD_ETH_Label2;
            UIControls_Dict["IRSGD"].ETH_Spread = IRSGD_ETH_Label3;
            UIControls_Dict["IRSGD"].BCH_Label = IRSGD_BCH_Label1;
            UIControls_Dict["IRSGD"].BCH_Price = IRSGD_BCH_Label2;
            UIControls_Dict["IRSGD"].BCH_Spread = IRSGD_BCH_Label3;
            UIControls_Dict["IRSGD"].LTC_Label = IRSGD_LTC_Label1;
            UIControls_Dict["IRSGD"].LTC_Price = IRSGD_LTC_Label2;
            UIControls_Dict["IRSGD"].LTC_Spread = IRSGD_LTC_Label3;
            UIControls_Dict["IRSGD"].XRP_Label = IRSGD_XRP_Label1;
            UIControls_Dict["IRSGD"].XRP_Price = IRSGD_XRP_Label2;
            UIControls_Dict["IRSGD"].XRP_Spread = IRSGD_XRP_Label3;
            UIControls_Dict["IRSGD"].OMG_Label = IRSGD_OMG_Label1;
            UIControls_Dict["IRSGD"].OMG_Price = IRSGD_OMG_Label2;
            UIControls_Dict["IRSGD"].OMG_Spread = IRSGD_OMG_Label3;
            UIControls_Dict["IRSGD"].ZRX_Label = IRSGD_ZRX_Label1;
            UIControls_Dict["IRSGD"].ZRX_Price = IRSGD_ZRX_Label2;
            UIControls_Dict["IRSGD"].ZRX_Spread = IRSGD_ZRX_Label3;
            UIControls_Dict["IRSGD"].EOS_Label = IRSGD_EOS_Label1;
            UIControls_Dict["IRSGD"].EOS_Price = IRSGD_EOS_Label2;
            UIControls_Dict["IRSGD"].EOS_Spread = IRSGD_EOS_Label3;
            UIControls_Dict["IRSGD"].XLM_Label = IRSGD_XLM_Label1;
            UIControls_Dict["IRSGD"].XLM_Price = IRSGD_XLM_Label2;
            UIControls_Dict["IRSGD"].XLM_Spread = IRSGD_XLM_Label3;
            UIControls_Dict["IRSGD"].BAT_Label = IRSGD_BAT_Label1;
            UIControls_Dict["IRSGD"].BAT_Price = IRSGD_BAT_Label2;
            UIControls_Dict["IRSGD"].BAT_Spread = IRSGD_BAT_Label3;
            UIControls_Dict["IRSGD"].MKR_Label = IRSGD_MKR_Label1;
            UIControls_Dict["IRSGD"].MKR_Price = IRSGD_MKR_Label2;
            UIControls_Dict["IRSGD"].MKR_Spread = IRSGD_MKR_Label3;
            UIControls_Dict["IRSGD"].ETC_Label = IRSGD_ETC_Label1;
            UIControls_Dict["IRSGD"].ETC_Price = IRSGD_ETC_Label2;
            UIControls_Dict["IRSGD"].ETC_Spread = IRSGD_ETC_Label3;
            UIControls_Dict["IRSGD"].USDT_Label = IRSGD_USDT_Label1;
            UIControls_Dict["IRSGD"].USDT_Price = IRSGD_USDT_Label2;
            UIControls_Dict["IRSGD"].USDT_Spread = IRSGD_USDT_Label3;
            UIControls_Dict["IRSGD"].DAI_Label = IRSGD_DAI_Label1;
            UIControls_Dict["IRSGD"].DAI_Price = IRSGD_DAI_Label2;
            UIControls_Dict["IRSGD"].DAI_Spread = IRSGD_DAI_Label3;
            UIControls_Dict["IRSGD"].LINK_Label = IRSGD_LINK_Label1;
            UIControls_Dict["IRSGD"].LINK_Price = IRSGD_LINK_Label2;
            UIControls_Dict["IRSGD"].LINK_Spread = IRSGD_LINK_Label3;
            UIControls_Dict["IRSGD"].USDC_Label = IRSGD_USDC_Label1;
            UIControls_Dict["IRSGD"].USDC_Price = IRSGD_USDC_Label2;
            UIControls_Dict["IRSGD"].USDC_Spread = IRSGD_USDC_Label3;
            UIControls_Dict["IRSGD"].COMP_Label = IRSGD_COMP_Label1;
            UIControls_Dict["IRSGD"].COMP_Price = IRSGD_COMP_Label2;
            UIControls_Dict["IRSGD"].COMP_Spread = IRSGD_COMP_Label3;
            UIControls_Dict["IRSGD"].SNX_Label = IRSGD_SNX_Label1;
            UIControls_Dict["IRSGD"].SNX_Price = IRSGD_SNX_Label2;
            UIControls_Dict["IRSGD"].SNX_Spread = IRSGD_SNX_Label3;
            UIControls_Dict["IRSGD"].PMGT_Label = IRSGD_PMGT_Label1;
            UIControls_Dict["IRSGD"].PMGT_Price = IRSGD_PMGT_Label2;
            UIControls_Dict["IRSGD"].PMGT_Spread = IRSGD_PMGT_Label3;
            UIControls_Dict["IRSGD"].YFI_Label = IRSGD_YFI_Label1;
            UIControls_Dict["IRSGD"].YFI_Price = IRSGD_YFI_Label2;
            UIControls_Dict["IRSGD"].YFI_Spread = IRSGD_YFI_Label3;
            UIControls_Dict["IRSGD"].AAVE_Label = IRSGD_AAVE_Label1;
            UIControls_Dict["IRSGD"].AAVE_Price = IRSGD_AAVE_Label2;
            UIControls_Dict["IRSGD"].AAVE_Spread = IRSGD_AAVE_Label3;
            UIControls_Dict["IRSGD"].DOT_Label = IRSGD_DOT_Label1;
            UIControls_Dict["IRSGD"].DOT_Price = IRSGD_DOT_Label2;
            UIControls_Dict["IRSGD"].DOT_Spread = IRSGD_DOT_Label3;
            UIControls_Dict["IRSGD"].GRT_Label = IRSGD_GRT_Label1;
            UIControls_Dict["IRSGD"].GRT_Price = IRSGD_GRT_Label2;
            UIControls_Dict["IRSGD"].GRT_Spread = IRSGD_GRT_Label3;
            UIControls_Dict["IRSGD"].UNI_Label = IRSGD_UNI_Label1;
            UIControls_Dict["IRSGD"].UNI_Price = IRSGD_UNI_Label2;
            UIControls_Dict["IRSGD"].UNI_Spread = IRSGD_UNI_Label3;
            UIControls_Dict["IRSGD"].ADA_Label = IRSGD_ADA_Label1;
            UIControls_Dict["IRSGD"].ADA_Price = IRSGD_ADA_Label2;
            UIControls_Dict["IRSGD"].ADA_Spread = IRSGD_ADA_Label3;
            UIControls_Dict["IRSGD"].DOGE_Label = IRSGD_DOGE_Label1;
            UIControls_Dict["IRSGD"].DOGE_Price = IRSGD_DOGE_Label2;
            UIControls_Dict["IRSGD"].DOGE_Spread = IRSGD_DOGE_Label3;
            UIControls_Dict["IRSGD"].MATIC_Label = IRSGD_MATIC_Label1;
            UIControls_Dict["IRSGD"].MATIC_Price = IRSGD_MATIC_Label2;
            UIControls_Dict["IRSGD"].MATIC_Spread = IRSGD_MATIC_Label3;
            UIControls_Dict["IRSGD"].MANA_Label = IRSGD_MANA_Label1;
            UIControls_Dict["IRSGD"].MANA_Price = IRSGD_MANA_Label2;
            UIControls_Dict["IRSGD"].MANA_Spread = IRSGD_MANA_Label3;
            UIControls_Dict["IRSGD"].SOL_Label = IRSGD_SOL_Label1;
            UIControls_Dict["IRSGD"].SOL_Price = IRSGD_SOL_Label2;
            UIControls_Dict["IRSGD"].SOL_Spread = IRSGD_SOL_Label3;
            UIControls_Dict["IRSGD"].SAND_Label = IRSGD_SAND_Label1;
            UIControls_Dict["IRSGD"].SAND_Price = IRSGD_SAND_Label2;
            UIControls_Dict["IRSGD"].SAND_Spread = IRSGD_SAND_Label3;


            // IR USD
            UIControls_Dict["IRUSD"].Name = "IRUSD";
            UIControls_Dict["IRUSD"].dExchange_GB = IRUSD_GroupBox;
            UIControls_Dict["IRUSD"].XBT_Label = IRUSD_XBT_Label1;
            UIControls_Dict["IRUSD"].XBT_Price = IRUSD_XBT_Label2;
            UIControls_Dict["IRUSD"].XBT_Spread = IRUSD_XBT_Label3;
            UIControls_Dict["IRUSD"].ETH_Label = IRUSD_ETH_Label1;
            UIControls_Dict["IRUSD"].ETH_Price = IRUSD_ETH_Label2;
            UIControls_Dict["IRUSD"].ETH_Spread = IRUSD_ETH_Label3;
            UIControls_Dict["IRUSD"].BCH_Label = IRUSD_BCH_Label1;
            UIControls_Dict["IRUSD"].BCH_Price = IRUSD_BCH_Label2;
            UIControls_Dict["IRUSD"].BCH_Spread = IRUSD_BCH_Label3;
            UIControls_Dict["IRUSD"].LTC_Label = IRUSD_LTC_Label1;
            UIControls_Dict["IRUSD"].LTC_Price = IRUSD_LTC_Label2;
            UIControls_Dict["IRUSD"].LTC_Spread = IRUSD_LTC_Label3;
            UIControls_Dict["IRUSD"].XRP_Label = IRUSD_XRP_Label1;
            UIControls_Dict["IRUSD"].XRP_Price = IRUSD_XRP_Label2;
            UIControls_Dict["IRUSD"].XRP_Spread = IRUSD_XRP_Label3;
            UIControls_Dict["IRUSD"].OMG_Label = IRUSD_OMG_Label1;
            UIControls_Dict["IRUSD"].OMG_Price = IRUSD_OMG_Label2;
            UIControls_Dict["IRUSD"].OMG_Spread = IRUSD_OMG_Label3;
            UIControls_Dict["IRUSD"].ZRX_Label = IRUSD_ZRX_Label1;
            UIControls_Dict["IRUSD"].ZRX_Price = IRUSD_ZRX_Label2;
            UIControls_Dict["IRUSD"].ZRX_Spread = IRUSD_ZRX_Label3;
            UIControls_Dict["IRUSD"].EOS_Label = IRUSD_EOS_Label1;
            UIControls_Dict["IRUSD"].EOS_Price = IRUSD_EOS_Label2;
            UIControls_Dict["IRUSD"].EOS_Spread = IRUSD_EOS_Label3;
            UIControls_Dict["IRUSD"].XLM_Label = IRUSD_XLM_Label1;
            UIControls_Dict["IRUSD"].XLM_Price = IRUSD_XLM_Label2;
            UIControls_Dict["IRUSD"].XLM_Spread = IRUSD_XLM_Label3;
            UIControls_Dict["IRUSD"].BAT_Label = IRUSD_BAT_Label1;
            UIControls_Dict["IRUSD"].BAT_Price = IRUSD_BAT_Label2;
            UIControls_Dict["IRUSD"].BAT_Spread = IRUSD_BAT_Label3;
            UIControls_Dict["IRUSD"].MKR_Label = IRUSD_MKR_Label1;
            UIControls_Dict["IRUSD"].MKR_Price = IRUSD_MKR_Label2;
            UIControls_Dict["IRUSD"].MKR_Spread = IRUSD_MKR_Label3;
            UIControls_Dict["IRUSD"].ETC_Label = IRUSD_ETC_Label1;
            UIControls_Dict["IRUSD"].ETC_Price = IRUSD_ETC_Label2;
            UIControls_Dict["IRUSD"].ETC_Spread = IRUSD_ETC_Label3;
            UIControls_Dict["IRUSD"].USDT_Label = IRUSD_USDT_Label1;
            UIControls_Dict["IRUSD"].USDT_Price = IRUSD_USDT_Label2;
            UIControls_Dict["IRUSD"].USDT_Spread = IRUSD_USDT_Label3;
            UIControls_Dict["IRUSD"].DAI_Label = IRUSD_DAI_Label1;
            UIControls_Dict["IRUSD"].DAI_Price = IRUSD_DAI_Label2;
            UIControls_Dict["IRUSD"].DAI_Spread = IRUSD_DAI_Label3;
            UIControls_Dict["IRUSD"].LINK_Label = IRUSD_LINK_Label1;
            UIControls_Dict["IRUSD"].LINK_Price = IRUSD_LINK_Label2;
            UIControls_Dict["IRUSD"].LINK_Spread = IRUSD_LINK_Label3;
            UIControls_Dict["IRUSD"].USDC_Label = IRUSD_USDC_Label1;
            UIControls_Dict["IRUSD"].USDC_Price = IRUSD_USDC_Label2;
            UIControls_Dict["IRUSD"].USDC_Spread = IRUSD_USDC_Label3;
            UIControls_Dict["IRUSD"].COMP_Label = IRUSD_COMP_Label1;
            UIControls_Dict["IRUSD"].COMP_Price = IRUSD_COMP_Label2;
            UIControls_Dict["IRUSD"].COMP_Spread = IRUSD_COMP_Label3;
            UIControls_Dict["IRUSD"].SNX_Label = IRUSD_SNX_Label1;
            UIControls_Dict["IRUSD"].SNX_Price = IRUSD_SNX_Label2;
            UIControls_Dict["IRUSD"].SNX_Spread = IRUSD_SNX_Label3;
            UIControls_Dict["IRUSD"].PMGT_Label = IRUSD_PMGT_Label1;
            UIControls_Dict["IRUSD"].PMGT_Price = IRUSD_PMGT_Label2;
            UIControls_Dict["IRUSD"].PMGT_Spread = IRUSD_PMGT_Label3;
            UIControls_Dict["IRUSD"].YFI_Label = IRUSD_YFI_Label1;
            UIControls_Dict["IRUSD"].YFI_Price = IRUSD_YFI_Label2;
            UIControls_Dict["IRUSD"].YFI_Spread = IRUSD_YFI_Label3;
            UIControls_Dict["IRUSD"].AAVE_Label = IRUSD_AAVE_Label1;
            UIControls_Dict["IRUSD"].AAVE_Price = IRUSD_AAVE_Label2;
            UIControls_Dict["IRUSD"].AAVE_Spread = IRUSD_AAVE_Label3;
            UIControls_Dict["IRUSD"].DOT_Label = IRUSD_DOT_Label1;
            UIControls_Dict["IRUSD"].DOT_Price = IRUSD_DOT_Label2;
            UIControls_Dict["IRUSD"].DOT_Spread = IRUSD_DOT_Label3;
            UIControls_Dict["IRUSD"].GRT_Label = IRUSD_GRT_Label1;
            UIControls_Dict["IRUSD"].GRT_Price = IRUSD_GRT_Label2;
            UIControls_Dict["IRUSD"].GRT_Spread = IRUSD_GRT_Label3;
            UIControls_Dict["IRUSD"].UNI_Label = IRUSD_UNI_Label1;
            UIControls_Dict["IRUSD"].UNI_Price = IRUSD_UNI_Label2;
            UIControls_Dict["IRUSD"].UNI_Spread = IRUSD_UNI_Label3;
            UIControls_Dict["IRUSD"].ADA_Label = IRUSD_ADA_Label1;
            UIControls_Dict["IRUSD"].ADA_Price = IRUSD_ADA_Label2;
            UIControls_Dict["IRUSD"].ADA_Spread = IRUSD_ADA_Label3;
            UIControls_Dict["IRUSD"].DOGE_Label = IRUSD_DOGE_Label1;
            UIControls_Dict["IRUSD"].DOGE_Price = IRUSD_DOGE_Label2;
            UIControls_Dict["IRUSD"].DOGE_Spread = IRUSD_DOGE_Label3;
            UIControls_Dict["IRUSD"].MATIC_Label = IRUSD_MATIC_Label1;
            UIControls_Dict["IRUSD"].MATIC_Price = IRUSD_MATIC_Label2;
            UIControls_Dict["IRUSD"].MATIC_Spread = IRUSD_MATIC_Label3;
            UIControls_Dict["IRUSD"].MANA_Label = IRUSD_MANA_Label1;
            UIControls_Dict["IRUSD"].MANA_Price = IRUSD_MANA_Label2;
            UIControls_Dict["IRUSD"].MANA_Spread = IRUSD_MANA_Label3;
            UIControls_Dict["IRUSD"].SOL_Label = IRUSD_SOL_Label1;
            UIControls_Dict["IRUSD"].SOL_Price = IRUSD_SOL_Label2;
            UIControls_Dict["IRUSD"].SOL_Spread = IRUSD_SOL_Label3;
            UIControls_Dict["IRUSD"].SAND_Label = IRUSD_SAND_Label1;
            UIControls_Dict["IRUSD"].SAND_Price = IRUSD_SAND_Label2;
            UIControls_Dict["IRUSD"].SAND_Spread = IRUSD_SAND_Label3;


            // BTCM

            BTCM_panel.AutoScroll = false;
            BTCM_panel.HorizontalScroll.Enabled = false;
            BTCM_panel.HorizontalScroll.Visible = false;
            BTCM_panel.HorizontalScroll.Maximum = 0;
            BTCM_panel.VerticalScroll.Visible = false;
            BTCM_panel.AutoScroll = true;

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
            UIControls_Dict["BTCM"].ETC_Label = BTCM_ETC_Label1;
            UIControls_Dict["BTCM"].ETC_Price = BTCM_ETC_Label2;
            UIControls_Dict["BTCM"].ETC_Spread = BTCM_ETC_Label3;
            UIControls_Dict["BTCM"].LINK_Label = BTCM_LINK_Label1;
            UIControls_Dict["BTCM"].LINK_Price = BTCM_LINK_Label2;
            UIControls_Dict["BTCM"].LINK_Spread = BTCM_LINK_Label3;
            UIControls_Dict["BTCM"].COMP_Label = BTCM_COMP_Label1;
            UIControls_Dict["BTCM"].COMP_Price = BTCM_COMP_Label2;
            UIControls_Dict["BTCM"].COMP_Spread = BTCM_COMP_Label3;
            UIControls_Dict["BTCM"].USDT_Label = BTCM_USDT_Label1;
            UIControls_Dict["BTCM"].USDT_Price = BTCM_USDT_Label2;
            UIControls_Dict["BTCM"].USDT_Spread = BTCM_USDT_Label3;
            UIControls_Dict["BTCM"].USDC_Label = BTCM_USDC_Label1;
            UIControls_Dict["BTCM"].USDC_Price = BTCM_USDC_Label2;
            UIControls_Dict["BTCM"].USDC_Spread = BTCM_USDC_Label3;
            UIControls_Dict["BTCM"].UNI_Label = BTCM_UNI_Label1;
            UIControls_Dict["BTCM"].UNI_Price = BTCM_UNI_Label2;
            UIControls_Dict["BTCM"].UNI_Spread = BTCM_UNI_Label3;
            UIControls_Dict["BTCM"].SAND_Label = BTCM_SAND_Label1;
            UIControls_Dict["BTCM"].SAND_Price = BTCM_SAND_Label2;
            UIControls_Dict["BTCM"].SAND_Spread = BTCM_SAND_Label3;
            UIControls_Dict["BTCM"].MANA_Label = BTCM_MANA_Label1;
            UIControls_Dict["BTCM"].MANA_Price = BTCM_MANA_Label2;
            UIControls_Dict["BTCM"].MANA_Spread = BTCM_MANA_Label3;
            UIControls_Dict["BTCM"].AAVE_Label = BTCM_AAVE_Label1;
            UIControls_Dict["BTCM"].AAVE_Price = BTCM_AAVE_Label2;
            UIControls_Dict["BTCM"].AAVE_Spread = BTCM_AAVE_Label3;

            UIControls_Dict["BTCM"].AvgPrice_BuySell = BTCM_BuySellComboBox;
            UIControls_Dict["BTCM"].AvgPrice_NumCoins = BTCM_NumCoinsTextBox;
            UIControls_Dict["BTCM"].AvgPrice_Crypto = BTCM_CryptoComboBox;
            UIControls_Dict["BTCM"].AvgPrice_Currency = BTCM_CurrencyBox;
            UIControls_Dict["BTCM"].AvgPrice = BTCM_AvgPrice_Label;

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
                if (null != uic.Value.AvgPrice_Currency) {
                    uic.Value.AvgPrice_Currency.SelectedIndex = 1;
                }
            }
        }

        private CancellationTokenSource setStickColourAsync(BlinkStick _bStick, CancellationTokenSource cTokenSrc, ref Task pulseTask, decimal IRvol, decimal BTCMvol) {
            //IRBTCvol = 98;
            //BTCMBTCvol = 99;
            if (_bStick != null && _bStick.OpenDevice()) {

                RgbColor currentColour;// = RgbColor.FromString("FF802B");  // an orange - if we see the cubes go this colour then there's probably something wrong
                try {
                    if (IRvol > (BTCMvol * 2)) {
                        if ((pulseTask == null) || pulseTask.IsCompleted) {
                            cTokenSrc = new CancellationTokenSource();
                            CancellationToken cTok = cTokenSrc.Token;
                            currentColour = RgbColor.FromString("#0079FF");
                            pulseTask = Task.Run(() => BWPulseforDubVol(currentColour, _bStick, cTok), cTok);
                            //Debug.Print(DateTime.Now + " -- BS -- started the IR GOOOOOD thread");
                        }
                    }
                    else if ((IRvol * 2) < BTCMvol) {
                        if ((pulseTask == null) || pulseTask.IsCompleted) {
                            cTokenSrc = new CancellationTokenSource();
                            CancellationToken cTok = cTokenSrc.Token;
                            currentColour = RgbColor.FromString("#00FF00");
                            pulseTask = Task.Run(() => BWPulseforDubVol(currentColour, _bStick, cTok), cTok);

                            //Debug.Print(DateTime.Now + " -- BS -- started the IR BAAAD thread");
                        }
                    }
                    else if (IRvol > (BTCMvol * 1.05M)) {  // more than 5%, IR winning
                        //Debug.Print(DateTime.Now + " -- BS -- IR winning");
                        if ((pulseTask != null) && !pulseTask.IsCompleted && (cTokenSrc != null)) cTokenSrc.Cancel();
                        else if ((pulseTask == null) || pulseTask.IsCompleted) {
                            currentColour = RgbColor.FromString("#3176BC");
                            _bStick.Morph(currentColour);
                        }
                        //cTokenSrc = null;  // don't think we actually want/need to do this.  we should just let it get set to null in the catch naturally.
                    }
                    else if ((IRvol <= (BTCMvol * 1.05M)) && (IRvol >= (BTCMvol * 0.95M))) {
                        //Debug.Print(DateTime.Now + " -- BS -- trying to go white");
                        if ((pulseTask != null) && !pulseTask.IsCompleted && (cTokenSrc != null)) cTokenSrc.Cancel();  // cancelling the task will set the colour to the pulsing colour.  don't bother trying to set to white, we can do that next round.
                        else {  // else there are no running tasks dub vol tasks, so we can change to white and start pulsing.  I know it's a dub vol task because the dub vol task is the only one that sets a cancellation token.
                                currentColour = RgbColor.FromString("#C19E6E");
                                _bStick.Morph(currentColour);
                            if (pulseTask == null || pulseTask.IsCompleted)  // only try and start the white pulse if there isn't already something happening.  
                                pulseTask = Task.Run(() => BlinkStickWhitePulseAsync(_bStick, RgbColor.FromString((BTCMvol > IRvol ? "#42953A" : "#B6CBE1"))));
                        }
                    }
                    else if (IRvol < BTCMvol * 0.95M) {
                        //Debug.Print(DateTime.Now + " -- BS -- BTCM is winning");
                        if ((pulseTask != null) && !pulseTask.IsCompleted && (cTokenSrc != null)) {
                            cTokenSrc.Cancel();
                            //pulseTask.
                        }
                        else if ((pulseTask == null) || pulseTask.IsCompleted) {
                            currentColour = RgbColor.FromString("#00A607");
                            _bStick.Morph(currentColour);
                        }

                        //cTokenSrc = null;  // don't think we actually want/need to do this.  we should just let it get set to null in the catch naturally.
                    }
                }
                catch (OperationCanceledException ex) {
                    try {
                        _bStick.Morph("red");  // pulsing is over, now we morph to the pulsing colour (assuming we haven't jumped phases to white or the other colour)
                    }
                    catch (Exception ex2) {
                        throw new Exception("Final morph failed after cancelling the dub vol method/thread - " + ex2.Message);
                    }

                    Debug.Print(DateTime.Now + " -- BS -- exited the dub volume loop as expected, message: " + ex.Message);
                    cTokenSrc.Dispose();
                    cTokenSrc = null;
                }
                catch (Exception ex) {
                    Debug.Print(DateTime.Now + " -- BS -- caught an exception: " + ex.Message);
                }
            }
            return cTokenSrc;
        }

        private void BlinkStickWhitePulseAsync(BlinkStick _bStick, RgbColor col) {
            int pulseLength = 200;
            //Debug.Print(DateTime.Now + " - BS - white thread should pulse a colour");

            if (_bStick != null && bStick.OpenDevice()) {
                try {
                    //bStick.Pulse(col, 1, pulseLength, 50);
                    _bStick.Morph(col, pulseLength);
                    _bStick.Morph("#C19E6E", pulseLength);
                }
                catch (Exception ex) {
                    Debug.Print(DateTime.Now + " -- BS -- caught an exception in white thread: " + ex.Message);
                }
            }
        }

        private Task BWPulseforDubVol(RgbColor col, BlinkStick _bStick, CancellationToken cToken) {

            int pulseLength = 700;
            //int repeats = 15000 / pulseLength;

            //bStick.Pulse(col, repeats, pulseLength, 50);

            do {

                if (_bStick != null && bStick.OpenDevice()) {
                    try {
                        _bStick.Pulse(col, 1, pulseLength, 50);
                        //Thread.Sleep(pulseLength);
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " -- BS -- caught an exception in BW for pulsing: " + ex.Message);
                    }
                }
                if (cToken.IsCancellationRequested) {
                    cToken.ThrowIfCancellationRequested();  // this should only be called if we didn't throw when trying to morph just above.  I hope
                }
            } while (true);
        }

        private void setSlackStatus(bool disable = false) {
            // now we set slack stuff

            long status_emoji_duration = 120;  // seconds

            if (string.IsNullOrEmpty(Properties.Settings.Default.SlackNameEmojiCrypto) || string.IsNullOrEmpty(Properties.Settings.Default.SlackNameFiatCurrency)) return;
            string crypto = (Properties.Settings.Default.SlackNameEmojiCrypto == "BTC" ? "XBT" : Properties.Settings.Default.SlackNameEmojiCrypto);
            string pair = crypto + "-" + DCEs["IR"].CurrentSecondaryCurrency;

            Dictionary<string, DCE.MarketSummary> IRpairs = DCEs["IR"].GetCryptoPairs();
            Dictionary<string, DCE.MarketSummary> BTCMpairs = DCEs["BTCM"].GetCryptoPairs();
            decimal IRvol = -1, BTCMvol = -1;
            if (IRpairs.ContainsKey(pair)) IRvol = IRpairs[pair].DayVolumeXbt;
            if (BTCMpairs.ContainsKey(crypto + "-AUD")) BTCMvol = BTCMpairs[crypto + "-AUD"].DayVolumeXbt;


            string name = "";

            if (Properties.Settings.Default.SlackNameChange && (Properties.Settings.Default.SlackDefaultName != string.Empty)) {

                if (disable) {
                    slackObj.setStatus("", ":crying_cat_face:", 1, "");  // slack has been disabled.  set the name back to normal and the emoji to crying cat for 1 second
                    return;
                }

                name = Properties.Settings.Default.SlackDefaultName;

                // can't continue if we somehow don't have crypto and fiat chosen for slack

                //string tempName = UIControls_Dict["IR"].Label_Dict["XBT_Price"].Text;
                Dictionary<string, DCE.MarketSummary> cPairs = DCEs["IR"].GetCryptoPairs();
                if (cPairs.ContainsKey(crypto + "-" + Properties.Settings.Default.SlackNameFiatCurrency)) {
                    decimal bid = cPairs[crypto + "-" + Properties.Settings.Default.SlackNameFiatCurrency].CurrentHighestBidPrice;
                    decimal offer = cPairs[crypto + "-" + Properties.Settings.Default.SlackNameFiatCurrency].CurrentLowestOfferPrice;
                    //string midPoint = Utilities.FormatValue(Math.Round(((bid + offer) / 2), 0), 5);
                    string midPoint = Utilities.FormatValue(((bid + offer) / 2));

                    //string tempName = ((cPairs["XBT-" + Properties.Settings.Default.SlackNameCurrency].CurrentLowestOfferPrice - cPairs["XBT-" + Properties.Settings.Default.SlackNameCurrency].CurrentHighestBidPrice) / 2).ToString();

                    //if (tempName.Length >= 3) tempName = tempName.Substring(0, tempName.Length - 3);  // remove decimal places from the price
                    name += " - " + Properties.Settings.Default.SlackNameFiatCurrency + " " + midPoint;
                }
            }
            //Debug.Print("slack name is: " + name);

            if (!DCEs["IR"].NetworkAvailable) {
                slackObj.setStatus("", ":exclamation:", status_emoji_duration, name + " - IR API down");
                return;
            }
            else if (!DCEs["IR"].socketsAlive) {
                slackObj.setStatus("", ":exclamation:", status_emoji_duration, name + " - IR WSS down");
                return;
            }
            else if (!DCEs["BTCM"].socketsAlive || !DCEs["BTCM"].NetworkAvailable) {
                slackObj.setStatus("", ":man-shrugging:", status_emoji_duration, name);  // should change this to still show the price, and not bother saying "BTCM API down" 
                if (TGBot != null) TGBot.BTCMemoji = "🤷‍";
                return;
            }


            if (IRvol < 0) {
                slackObj.setStatus("", ":question:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "";
            }
            else if (BTCMvol < 0) {
                slackObj.setStatus("", ":man-shrugging:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "🤷‍";
            }
            else if (IRvol > BTCMvol * 2) {
                slackObj.setStatus("", ":danbizan:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "🥳";
            }
            else if (IRvol * 2 < BTCMvol) {
                slackObj.setStatus("", ":sob:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "😭";
            }
            else if (IRvol > BTCMvol * 1.05M) {
                slackObj.setStatus("", ":sunglasses:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "😎";
            }
            else if ((IRvol <= BTCMvol * 1.05M) && (IRvol >= BTCMvol * 0.95M)) {
                slackObj.setStatus("", ":neutral_face:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "😐";
            }
            else if (IRvol < BTCMvol * 0.95M) {
                slackObj.setStatus("", ":slightly_frowning_face:", status_emoji_duration, name);
                if (TGBot != null) TGBot.BTCMemoji = "🙁";
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
        public void ParseDCE_IR(string crypto, string fiat, bool updateLabels, bool updateSpread = false) {
            Tuple<bool, string> marketSummary = Utilities.Get(DCEs["IR"].BaseURL + "/Public/GetMarketSummary?primaryCurrencyCode=" + crypto + "&secondaryCurrencyCode=" + fiat);
            if (!marketSummary.Item1) {
                foreach (string dExchange in IRdExchanges) {
                    DCEs[dExchange].CurrentDCEStatus = WebsiteError(marketSummary.Item2);
                    DCEs[dExchange].NetworkAvailable = false;
                    return;
                }
            }
            else {
                foreach (string dExchange in IRdExchanges) DCEs[dExchange].NetworkAvailable = true;
                DCE.MarketSummary mSummary;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                try {
                    mSummary = JsonConvert.DeserializeObject<DCE.MarketSummary>(marketSummary.Item2, settings);
                }
                catch {
                    Debug.Print(DateTime.Now + " - IR - bad REST result: " + marketSummary.Item2);
                    DCEs["IR"].NetworkAvailable = false;
                    if (fiat == "USD") {
                        DCEs["IRUSD"].NetworkAvailable = false;
                    }
                    if (fiat == "SGD") {
                        DCEs["IRSGD"].NetworkAvailable = false;
                    }
                    return;
                }

                foreach (string dExchange in IRdExchanges) {
                    // This bit is for a) volume (we don't get vol from websockets), and b) if there have been no orders to establish a spread, then the price and spread
                    // stay at 0.  This is 
                    Dictionary<string, DCE.MarketSummary> cPairs = DCEs[dExchange].GetCryptoPairs();
                    if (cPairs.ContainsKey(mSummary.pair) && (cPairs[mSummary.pair].spread != 0) && !updateSpread) {  // logic here is if the spread is not 0, then don't send spread info, as what we have is better
                    //if (crypto == "XBT") {  // don't want to overwrite the spread orders as they're probably out of date
                        mSummary.CurrentHighestBidPrice = 0;  // sending cryptoPairsAdd a 0 bid and offer will mean the previous best bid and offer remain
                        mSummary.CurrentLowestOfferPrice = 0;
                    }
                    //}
                    mSummary.CreatedTimestampUTC = "";
                    //if (DCEs[dExchange].CurrentSecondaryCurrency == fiat) {
                        DCEs[dExchange].CryptoPairsAdd(crypto + "-" + fiat, mSummary);  // this cryptoPairs dictionary holds a list of all the DCE's trading pairs
                        if (updateLabels) pollingThread.ReportProgress(21, mSummary);  // don't update the labels if we are pulling a different fiat than the one we're showing (eg for Slack name)

                    //}
                    DCEs[dExchange].CurrentDCEStatus = "Online";
                }
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
                DCE.MarketSummary_BTCM mSummary_BTCM;
                try {
                    mSummary_BTCM = JsonConvert.DeserializeObject<DCE.MarketSummary_BTCM>(marketSummary.Item2);
                }
                catch {
                    Debug.Print(DateTime.Now + " - BTCM - bad REST result: " + marketSummary.Item2);
                    DCEs["BTCM"].NetworkAvailable = false;
                    return;
                }

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
                DCE.MarketSummary_BAR mSummary_BAR;
                try {
                    mSummary_BAR = JsonConvert.DeserializeObject<DCE.MarketSummary_BAR>(marketSummary.Item2);
                }
                catch {
                    Debug.Print(DateTime.Now + " - BAR - bad REST result: " + marketSummary.Item2);
                    DCEs["BAR"].NetworkAvailable = false;
                    return;
                }
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

        public void SubscribeTickerSocket(string dExchange) {
            // subscribe to all the pairs
            wSocketConnect.subscribe_unsubscribe_new(dExchange, true);  // this sub is now kinda useless
        }

        private void GetBTCMOrderBook(string crypto) {
            Tuple<bool, string> orderBookTpl = Utilities.Get("https://api.btcmarkets.net/market/" + (crypto == "XBT" ? "BTC" : crypto) + "/" + DCEs["BTCM"].CurrentSecondaryCurrency + "/orderbook");
            if (orderBookTpl.Item1) {
                DCE.OrderBook_BTCM orderBook_BTCM;
                try {
                    orderBook_BTCM = JsonConvert.DeserializeObject<DCE.OrderBook_BTCM>(orderBookTpl.Item2);
                }
                catch (Exception ex) {
                    Debug.Print(DateTime.Now + " - BTCM - bad REST result for order book: " + ex.Message);
                    DCEs["BTCM"].NetworkAvailable = false;
                    return;
                }

                // convert the BTCM order book into the IR format
                DCE.OrderBook oBook = new DCE.OrderBook();
                oBook.PrimaryCurrencyCode = orderBook_BTCM.instrument;
                oBook.SecondaryCurrencyCode = orderBook_BTCM.currency;
                DateTimeOffset timeTemp = DateTimeOffset.FromUnixTimeSeconds(orderBook_BTCM.timestamp);  // convert from epoch
                oBook.CreatedTimestampUtc = timeTemp.UtcDateTime;

                foreach (List<decimal> ask in orderBook_BTCM.asks) {
                    oBook.SellOrders.Add(new DCE.Order("LimitSell",ask[0], ask[1], ""));
                }

                foreach (List<decimal> bid in orderBook_BTCM.bids) {
                    oBook.BuyOrders.Add(new DCE.Order("LimitBuy", bid[0], bid[1], ""));
                }
                DCEs["BTCM"].orderBooks[crypto + "-" + DCEs["BTCM"].CurrentSecondaryCurrency] = oBook;
            }
        }

        private void PopulateCryptoComboBox(string dExchange) {

            Utilities.PopulateCryptoComboBox(DCEs[dExchange], UIControls_Dict[dExchange].AvgPrice_Crypto);
            return;
        }

        private void PollingThread_DoWork(object sender, DoWorkEventArgs e) {
            try {
                PollingThread_MeatAndPotates(sender, e);
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - BACKGROUND WORKER FAILED:");
                Debug.Print(ex.Message);
                if (null != ex.InnerException) {
                    Debug.Print("inner exception: " + ex.InnerException.Message);
                }
            }
        }            

        private void PollingThread_MeatAndPotates(object sender, DoWorkEventArgs e) {
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
                if (!DCEs["IR"].NetworkAvailable) DCEs["IR"].HasStaticData = false;  // if we have a network outage, let's start again.  jesus I contradict this the very next line
                if (!DCEs["IR"].HasStaticData) {  // only pull the currencies once per session as these are essentially static
                    Tuple<bool, string> primaryCurrencyCodesTpl = Utilities.Get(DCEs["IR"].BaseURL + "/Public/GetValidPrimaryCurrencyCodes");
                    if (!primaryCurrencyCodesTpl.Item1) {
                        DCEs["IR"].CurrentDCEStatus = DCEs["IRUSD"].CurrentDCEStatus = DCEs["IRSGD"].CurrentDCEStatus = WebsiteError(primaryCurrencyCodesTpl.Item2);
                        DCEs["IR"].NetworkAvailable = DCEs["IRUSD"].NetworkAvailable = DCEs["IRSGD"].NetworkAvailable = false;
                    }
                    else {
                        DCEs["IR"].NetworkAvailable = DCEs["IRUSD"].NetworkAvailable = DCEs["IRSGD"].NetworkAvailable = true;
                        DCEs["IR"].CurrentDCEStatus = DCEs["IRUSD"].CurrentDCEStatus = DCEs["IRSGD"].CurrentDCEStatus = "Online";
                        DCEs["IR"].PrimaryCurrencyCodes = DCEs["IRUSD"].PrimaryCurrencyCodes = Utilities.TrimEnds(primaryCurrencyCodesTpl.Item2);  // no SGD - they have their own list of currencies
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

                    }
                    if (DCEs["IR"].NetworkAvailable) {
                        DCEs["IR"].HasStaticData = DCEs["IRUSD"].HasStaticData = DCEs["IRSGD"].HasStaticData = true;  // we got here with the network up?  then we got the static data!  and probs for USD and SGD
                        Dictionary<string, DCE.products_GDAX> productDictionary_IR = new Dictionary<string, DCE.products_GDAX>();
                        Dictionary<string, DCE.products_GDAX> productDictionary_IRUSD = new Dictionary<string, DCE.products_GDAX>();
                        Dictionary<string, DCE.products_GDAX> productDictionary_IRSGD = new Dictionary<string, DCE.products_GDAX>();
                        foreach (string crypto in DCEs["IR"].PrimaryCurrencyList) {
                            foreach (string fiat in DCEs["IR"].SecondaryCurrencyList) {
                                productDictionary_IR.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                                DCEs["IR"].negSpreadCount[crypto + "-" + fiat] = 0;  // init

                                if (fiat == "USD") {
                                    productDictionary_IRUSD.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                                    DCEs["IRUSD"].negSpreadCount[crypto + "-" + fiat] = 0;  // init
                                    DCEs["IRUSD"].InitialiseOrderBookDicts_IR(crypto, fiat);
                                    ParseDCE_IR(crypto, fiat, true);
                                }

                                else if ((fiat == "SGD") && DCEs["IRSGD"].PrimaryCurrencyList.Contains(crypto)) {  // extra check about crypto because SGD doesn't pair with all cryptos
                                    productDictionary_IRSGD.Add(crypto + "-" + fiat, new DCE.products_GDAX(crypto + "-" + fiat));
                                    DCEs["IRSGD"].negSpreadCount[crypto + "-" + fiat] = 0;  // init
                                    DCEs["IRSGD"].InitialiseOrderBookDicts_IR(crypto, fiat);
                                    ParseDCE_IR(crypto, fiat, true);
                                }

                                //create OB objects ready to be filled.  we only do this once here, and never delete them.  neverrrrr
                                DCEs["IR"].InitialiseOrderBookDicts_IR(crypto, fiat);
                                if (DCEs["IR"].CurrentSecondaryCurrency == fiat) ParseDCE_IR(crypto, fiat, true);  // initial data pull and display
                            }
                        }

                        // now that we have the currencies, lets grab all closedorders and put into notifiedOrders
                        // we do this in the privateIR_init() sub now
                        // if (null != pIR) pIR.populateClosedOrders();

                        if (string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey) || string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey)) {
                            IRAccount_button.Enabled = false;
                            pIR = null;
                        }
                        else {
                            pIR.PrivateIR_init(Properties.Settings.Default.IRAPIPubKey, Properties.Settings.Default.IRAPIPrivKey, IRAF, DCEs["IR"], TGBot);
                            //int friendlyNameLen = Properties.Settings.Default.APIFriendly.Length;
                            //if (friendlyNameLen > 20) friendlyNameLen = 20;

                            //if (null != IRAF) IRAF.UpdateAccountNameButton(Properties.Settings.Default.APIFriendly.Substring(0, friendlyNameLen) + (friendlyNameLen != Properties.Settings.Default.APIFriendly.Length ? "..." : ""));
                        }

                        /*DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "AUD");
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "USD");
                        DCEs["IR"].InitialiseOrderBookDicts_IR("XBT", "NZD");*/

                        if (DCEs["IR"].currencyDecimalPlaces.Count() > 0) DCEs["IR"].currencyDecimalPlaces.Clear();  // if we reset due to network outage, then clear this before 
                        if (DCEs["IRUSD"].currencyDecimalPlaces.Count() > 0) DCEs["IRUSD"].currencyDecimalPlaces.Clear();  // if we reset due to network outage, then clear this before 
                        if (DCEs["IRSGD"].currencyDecimalPlaces.Count() > 0) DCEs["IRSGD"].currencyDecimalPlaces.Clear();  // if we reset due to network outage, then clear this before 

                        if (!File.Exists("cryptoDPs.csv")) {
                            MessageBox.Show("cryptoDPs.csv can't be found in the root application folder.  Grab it from Resources folder if you can, or ask Nick.  Can re-generate this file by running the CryptoDecimalPlaces-scrape.ps1 script.  App will close now.",
                                "Error: can't find decimal places file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                            return;
                        }
                        try {
                            using (StreamReader r = new StreamReader("cryptoDPs.csv")) {
                                //string json = r.ReadToEnd();
                                //CurrencyRoot = JsonConvert.DeserializeObject<IRCurrencies>(json);
                                string line;
                                while ((line = r.ReadLine()) != null) {
                                    if (string.IsNullOrEmpty(line)) break;
                                    string[] details = line.Split(',');
                                    int vol = int.Parse(details[1]);
                                    int fiat = int.Parse(details[2]);
                                    if ((vol > 126) || (vol < -126) || (fiat > 126) || (fiat < -126)) throw new Exception("volume or fiat not within -126 and 126");  // we have to convert this to a byte, which must be within these values
                                    DCEs["IR"].currencyDecimalPlaces.Add(details[0].ToUpper(), new Tuple<int, int>(int.Parse(details[1]), int.Parse(details[2])));
                                    DCEs["IRUSD"].currencyDecimalPlaces.Add(details[0].ToUpper(), new Tuple<int, int>(int.Parse(details[1]), int.Parse(details[2])));
                                    DCEs["IRSGD"].currencyDecimalPlaces.Add(details[0].ToUpper(), new Tuple<int, int>(int.Parse(details[1]), int.Parse(details[2])));
                                }
                            }
                        }
                        catch (Exception ex) {
                            MessageBox.Show("cryptoDPs.csv can't be read, can't parse the lines for ints maybe?  Error: " + ex.Message + " App will close now.",
                                "Error: can't read decimal places file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }

                        DCEs["IR"].ExchangeProducts = productDictionary_IR;
                        DCEs["IRUSD"].ExchangeProducts = productDictionary_IRUSD;
                        DCEs["IRSGD"].ExchangeProducts = productDictionary_IRSGD;

                        pollingThread.ReportProgress(28, "IR");  // populate slack name currency comboboxes in settnigs


                        wSocketConnect.Reinit_sockets("IR");  // this will setup all the necessary dictionaries
                        wSocketConnect.Reinit_sockets("IRUSD");  // this will setup all the necessary dictionaries
                        wSocketConnect.Reinit_sockets("IRSGD");  // this will setup all the necessary dictionaries

                        SubscribeTickerSocket("IR");
                        SubscribeTickerSocket("IRUSD");
                        SubscribeTickerSocket("IRSGD");

                        pollingThread.ReportProgress(14, "IR");
                    }
                    else {  // network is down.  we should report it and try again?
                        pollingThread.ReportProgress(12, "IR");
                        pollingThread.ReportProgress(12, "IRUSD");
                        pollingThread.ReportProgress(12, "IRSGD");

                        DCEs["IR"].HasStaticData = DCEs["IRUSD"].HasStaticData = DCEs["IRSGD"].HasStaticData = false;  // if we go offline, let's start again.
                    }
                }

                // still need to run this to get volume data (and all coins except BTC)
                // only need vol data for the main IR groupBox, SGD and USD don't need to show it (it's the same)
                if (DCEs["IR"].HasStaticData && DCEs["IR"].NetworkAvailable) {
                    foreach (string primaryCode in DCEs["IR"].PrimaryCurrencyList) {
                        // if there's no crypto selected in the drop down or there's no number of coins entered, then just pull the market summary
                        if (loopCount == 0 || !shitCoins.Contains(primaryCode)) {
                            ParseDCE_IR(primaryCode, DCEs["IR"].CurrentSecondaryCurrency, false);
                        }
                        if ((null != IRAF) && !IRAF.IsDisposed && (null != pIR)) {
                            foreach (string fiat in DCEs["IR"].SecondaryCurrencyList) {
                                try {
                                    var cOrders = pIR.GetClosedOrders(primaryCode, fiat);  // grab the closed orders on a schedule, this way we will know if an order has been filled and can alert.
                                    if ((null != IRAF) && (primaryCode == IRAF.AccountSelectedCrypto) && (fiat == DCEs["IR"].CurrentSecondaryCurrency) && (null != cOrders)) 
                                        IRAF.drawClosedOrders(cOrders.Data);
                                }
                                catch (Exception ex) {
                                    string errorMsg = ex.Message;
                                    if (ex.InnerException != null) {
                                        errorMsg = ex.InnerException.Message;
                                    }
                                    Debug.Print(DateTime.Now + " - In BGW thread loop, trying to pull closed orders, but it failed (" + primaryCode + "-" + fiat + "): " + errorMsg);
                                }
                                // if i want to get fancy i can call reportProgress and drawClosedOrders()...

                                // once a cycle let's give the IR Accounts order book a nudge.. there might be new orders that we haven't seen.
                                if ((null != IRAF) && !IRAF.IsDisposed) pIR.compileAccountOrderBookAsync(primaryCode + "-" + fiat);
                            }
                        }
                    }

                    // need to pull this other fiat currency market summary data if our chosen slack currency is not the one we're looking at (and the slack stuff is enabled)
                    if ((Properties.Settings.Default.SlackNameFiatCurrency != DCEs["IR"].CurrentSecondaryCurrency) &&
                        Properties.Settings.Default.Slack && Properties.Settings.Default.SlackNameChange) {
                        string slackCrypto = Properties.Settings.Default.SlackNameEmojiCrypto;
                        if (slackCrypto == "BTC") slackCrypto = "XBT";
                        if (DCEs["IR"].PrimaryCurrencyList.Contains(slackCrypto)) {  // if the chosen crypto is a real one
                            ParseDCE_IR(slackCrypto, Properties.Settings.Default.SlackNameFiatCurrency, updateLabels: false, updateSpread: true);  // we force a spread update only if the slack secondary currency is different to the current secondary currency
                        }
                    }


                    // let's check the IR spread.  Cycle through all the "_Spread" labels
                    // go through them one at a time, if one is good (ie returns false), then check the next one.  Only want to reset the sockets once.
                    if (Properties.Settings.Default.NegativeSpread) {
                        if (!CheckNegativeSpread("IR"))
                            if (!CheckNegativeSpread("IRUSD"))
                                CheckNegativeSpread("IRSGD");
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
                                case "IRUSD":
                                case "IRSGD":
                                    ParseDCE_IR(primaryCode, DCEs[dExchange].CurrentSecondaryCurrency, updateLabels: true);
                                    break;
                                case "BTCM":
                                    ParseDCE_BTCM(primaryCode, DCEs[dExchange].CurrentSecondaryCurrency);
                                    break;
                            }
                        }

                        // for the reset, let's just do the IR exchange.  they all hang off the same socket connection, so they should all fail at the same time.  I don't want it reconnecting 3 times
                        if ((dExchange != "IRUSD") && (dExchange != "IRSGD")) {
                            DCEs[dExchange].socketsReset = false;
                            // ok we need to reset the socket.
                            Debug.Print(DateTime.Now + " - " + dExchange + " - REST data pulled, now restarting sockets from backgroundWorker");
                            wSocketConnect.WebSocket_Reconnect(dExchange);
                        }
                    }

                    if (loopCount == 0) {
                        if (!wSocketConnect.IsSocketAlive(dExchange)) {  // isSocketAlive is always true for IR and it's brethren.. for better or worse.
                            Debug.Print(dExchange + " ded, reconnecting");
                            wSocketConnect.WebSocket_Reconnect(dExchange);
                        }
                    }
                }

                // OK we now have all the DCE and fiat rates info loaded.

                pollingThread.ReportProgress(1);

                // grab BTC and ETH fees
                Tuple<bool, string> feeTup = Utilities.Get("https://mempool.space/api/v1/fees/recommended");
                if (feeTup.Item1) {
                    mempoolSpace mspaceRecommended;

                    try {
                        mspaceRecommended = JsonConvert.DeserializeObject<mempoolSpace>(feeTup.Item2);
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " - mempool.space - bad REST result: " + ex.Message);
                        DCEs["BAR"].NetworkAvailable = false;
                        return;
                    }
                    BTCfee = mspaceRecommended.fastestFee;
                }
                else BTCfee = -1;  // tells the system to signal on the "Last updated" label

                feeTup = Utilities.Get("https://api.etherscan.io/api?module=gastracker&action=gasoracle&apikey=VGGBNCFAISMZAQKC1SDN92WDI1WF75KDKP");
                if (feeTup.Item1) {
                    EtherscanRoot ESData;
                    try {
                        ESData = JsonConvert.DeserializeObject<EtherscanRoot>(feeTup.Item2);
                        if (decimal.TryParse(ESData.result.FastGasPrice, out decimal res)) {
                            ETHfee = res;
                        }
                        else ETHfee = -1;
                    }
                    catch (Exception ex) {
                        Debug.Print("Etherscan gas fee error: " + ex.ToString());
                        ETHfee = -1;
                    }

                }
                else ETHfee = -1;

                /*feeTup = Utilities.Get("https://data-api.defipulse.com/api/v1/egs/api/ethgasAPI.json?api-key=17888172119617872db6baf33644a66c6e0b4354f25e03cf974986aedfa2");
                if (feeTup.Item1) {
                    EGSRoot EGSData = JsonConvert.DeserializeObject<EGSRoot>(feeTup.Item2);
                    ETHfee = EGSData.fast / 10;
                }
                else ETHfee = -1;*/


                // lets grab the latest BTC block
                Tuple<bool, string> latestBlockTup = Utilities.Get("https://blockchain.info/latestblock");
                if (latestBlockTup.Item1) {
                    BlockHeight bHeight;
                    try {
                        bHeight = JsonConvert.DeserializeObject<BlockHeight>(latestBlockTup.Item2);

                        if (lastBlock == 0) lastBlock = bHeight.Height;  // we haven' found a block before, just set it and move on
                        else if (lastBlock != bHeight.Height) {
                            if (bStick != null && bStick.OpenDevice()) {
                                try {
                                    bStick.Morph("purple");
                                }
                                catch (Exception ex) {
                                    Debug.Print(DateTime.Now + " -- BS -- caught an exception in block height: " + ex.Message);
                                }
                            }
                            lastBlock = bHeight.Height;
                            pollingThread.ReportProgress(15);
                            Debug.Print(DateTime.Now + " - we have a new BTC block: " + lastBlock);
                        }
                        //Debug.Print("current block is: " + lastBlock);
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " - blockchain.info - bad REST result: " + ex.Message);
                        //return;
                    }
                }
                else {
                    Debug.Print("couldn't pull the block height data? error: " + latestBlockTup.Item2);
                }

                // Time to blink some sticks
                Dictionary<string, DCE.MarketSummary> IRpairs = DCEs["IR"].GetCryptoPairs();
                Dictionary<string, DCE.MarketSummary> BTCMpairs = DCEs["BTCM"].GetCryptoPairs();
                decimal IRvol_BTC = -1, BTCMvol_BTC = -1, IRvol_ETH = -1, BTCMvol_ETH = -1, IRvol_USDT = -1, BTCMvol_USDT = -1, IRvol_XRP = -1, BTCMvol_XRP = -1;
                string currentSecondary = DCEs["IR"].CurrentSecondaryCurrency;
                if (IRpairs.ContainsKey("XBT-" + currentSecondary) && BTCMpairs.ContainsKey("XBT-AUD") &&
                    IRpairs.ContainsKey("ETH-" + currentSecondary) && BTCMpairs.ContainsKey("ETH-AUD") &&
                    IRpairs.ContainsKey("USDT-" + currentSecondary) && BTCMpairs.ContainsKey("USDT-AUD") &&
                    IRpairs.ContainsKey("XRP-" + currentSecondary) && BTCMpairs.ContainsKey("XRP-AUD")) {
                    IRvol_BTC = IRpairs["XBT-" + currentSecondary].DayVolumeXbt; ;
                    BTCMvol_BTC = BTCMpairs["XBT-AUD"].DayVolumeXbt;
                    IRvol_ETH = IRpairs["ETH-" + currentSecondary].DayVolumeXbt; ;
                    BTCMvol_ETH = BTCMpairs["ETH-AUD"].DayVolumeXbt;
                    IRvol_USDT = IRpairs["USDT-" + currentSecondary].DayVolumeXbt; ;
                    BTCMvol_USDT = BTCMpairs["USDT-AUD"].DayVolumeXbt;
                    IRvol_XRP = IRpairs["XRP-" + currentSecondary].DayVolumeXbt; ;
                    BTCMvol_XRP = BTCMpairs["XRP-AUD"].DayVolumeXbt;


                    if ((bStick == null) || (bStickETH == null) || (bStickUSDT == null) || (bStickXRP == null)) {
                        var bSticks = BlinkStick.FindAll();
                        if (bSticks.Length > 1) {

                            int i = 0;
                            do {
                                switch (bSticks[i].Meta.Serial) {
                                    case "BS032958-3.0":
                                        bStickETH = bSticks[i];
                                        break;
                                    case "BS041767-3.0":
                                        bStickUSDT = bSticks[i];
                                        break;
                                    case "BS028603-3.0":
                                        bStick = bSticks[i];  // BTC
                                        break;
                                    case "BS041736-3.0":
                                        bStickXRP = bSticks[i];
                                        break;
                                }

                                i++;
                            } while (i < bSticks.Length);

                        }
                        else if (bSticks.Length == 1) bStick = bSticks[0];  // if we only have 1 blink stick, then make it BTC
                    }
                    if (bStick != null && bStick.OpenDevice()) {
                        cTokenPulseBTC = setStickColourAsync(bStick, cTokenPulseBTC, ref taskPulseBTC, IRvol_BTC, BTCMvol_BTC);
                    }
                    if (bStickETH != null && bStickETH.OpenDevice()) {
                        cTokenPulseETH = setStickColourAsync(bStickETH, cTokenPulseETH, ref taskPulseETH, IRvol_ETH, BTCMvol_ETH);
                    }
                    if (bStickUSDT != null && bStickUSDT.OpenDevice()) {
                        cTokenPulseUSDT = setStickColourAsync(bStickUSDT, cTokenPulseUSDT, ref taskPulseUSDT, IRvol_USDT, BTCMvol_USDT);
                    }
                    if (bStickXRP != null && bStickXRP.OpenDevice()) {
                        cTokenPulseXRP = setStickColourAsync(bStickXRP, cTokenPulseXRP, ref taskPulseXRP, IRvol_XRP, BTCMvol_XRP);
                    }
                }

                // loopCount // - let's only update this every 3rd time, stop my slack phone app from restarting as often
                if (Properties.Settings.Default.Slack && (Properties.Settings.Default.SlackToken != "") /*&& (loopCount == 0)*/) {
                    setSlackStatus();
                }

                foreach (string dExchange in Exchanges) {
                    string cCombo = DCEs[dExchange].CryptoCombo;
                    if (cCombo == "BTC") cCombo = "XBT";
                    if (cCombo != "" && !string.IsNullOrEmpty(DCEs[dExchange].NumCoinsStr) && DCEs[dExchange].HasStaticData) {  // we have a crypto selected and coins entered, let's get the order book for them
                        pollingThread.ReportProgress(2, dExchange);  // OK let's lock the fields down
                        switch (dExchange) {
                            case "BTCM":
                                GetBTCMOrderBook(cCombo);
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

        // this isn't going to work anymore - need to call WebSocket_Resubscribe differently... resubscribing to IRSGD for example won't do anything, need to do it to IR first
        private bool CheckNegativeSpread(string dExchange) {
            Dictionary<string, DCE.MarketSummary> mSummaries = DCEs[dExchange].GetCryptoPairs();
            foreach (var mSummary in mSummaries) {
                if (mSummary.Value.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency) {
                    if (mSummary.Value.CurrentLowestOfferPrice <= mSummary.Value.CurrentHighestBidPrice) {
                        if (!DCEs[dExchange].positiveSpread[mSummary.Value.pair]) {  // already been negative for this pair :(
                            Debug.Print(DateTime.Now + " - negative pair detected (" + mSummary.Value.pair + ") for " + dExchange + ". let's unsub resub");
                            // do something..
                            pollingThread.ReportProgress(29, new Tuple<string, string>(dExchange, mSummary.Value.pair));  // update UI to show another spread fail
                            wSocketConnect.WebSocket_Resubscribe(dExchange, mSummary.Value.PrimaryCurrencyCode);
                            return true;
                        }
                        else {
                            // spread was positive last time, set the signal and wait for the next rotation
                            DCEs[dExchange].positiveSpread[mSummary.Value.pair] = false;
                            Debug.Print(DateTime.Now + " - Negave pair (" + mSummary.Value.pair + ") for " + dExchange + " signaled, waiting...");
                        }
                    }
                    else {
                        DCEs[dExchange].positiveSpread[mSummary.Value.pair] = true;  // this pair is OK.  Doesn't mean it's right, it's just not DEFINITELY wrong.
                                                                                //Debug.Print("Negative spread check all good for " + mSummary.Value.pair);
                    }
                }
            }
            return false;  // all g, no need to take action
        }

        private void UpdateLabels(string dExchange) {
            if (!Main.Visible && !LoadingPanel.Visible) return;  // no point drawing to the UI if we can't see anything
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

                    Color priceCol = Utilities.PriceColour(DCEs[dExchange].GetPriceList(pairObj.Key));
                    if (tempPrice.ForeColor != priceCol) tempPrice.ForeColor = priceCol;

                    // if there's a colour, make the font bigger.  otherwise not bigger.
                    // nah not doing this anymore.
                    /*if (tempPrice.ForeColor != Color.Black) {
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
                    }*/

                    string vol = "";
                    if ((dExchange == "IRUSD") || (dExchange == "IRSGD")) vol = "";  // no vol for these exchanges, the vol is in the main groupBox
                    else {
                        if (pairObj.Value.DayVolumeXbt == 0) vol = " / 0";
                        else if (pairObj.Value.DayVolumeXbt < 0) vol = " / ?";
                        else vol = " / " + Utilities.FormatValue(pairObj.Value.DayVolumeXbt);
                    }

                        UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"].Text = Utilities.FormatValue(pairObj.Value.spread) + vol;

                    // update tool tips.
                    string spreadTT = "Best bid: " + Utilities.FormatValue(pairObj.Value.CurrentHighestBidPrice) + System.Environment.NewLine +
                        "Best offer: " + Utilities.FormatValue(pairObj.Value.CurrentLowestOfferPrice) + System.Environment.NewLine +
                        "Spread: " + Utilities.FormatValue(((pairObj.Value.CurrentLowestOfferPrice - pairObj.Value.CurrentHighestBidPrice) / midPoint * 100), 2, false) + "%";

                    IRTickerTT_spread.SetToolTip(UIControls_Dict[dExchange].Label_Dict[pairObj.Value.PrimaryCurrencyCode + "_Spread"], spreadTT);
                        
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
            if (!Main.Visible && !LoadingPanel.Visible) return;  // no point drawing to the UI if we can't see anything
            if (!UIControls_Dict[dExchange].Label_Dict.ContainsKey(mSummary.PrimaryCurrencyCode + "_Price")) return;  // we haven't added this crypto to the UI yet
            // first we reset the labels.  The label writing process only writes to labels of pairs that exist, so we first need to set them back in case they don't exist

            //DCE.MarketSummary mSummary = DCEs[dExchange].GetCryptoPairs()[crypto + "-" + fiat];

            // i guess we need to filter out the wrong pairs, also don't try and update labels that are -1 (-1 means they're a fake entry)
            if ((mSummary.LastPrice >= 0) &&
                ((mSummary.SecondaryCurrencyCode == DCEs[dExchange].CurrentSecondaryCurrency)/* ||  // if it's the chosen currency, or it's IR and (SGD or USD) - we always show these now
                ((dExchange == "IRUSD") && (mSummary.SecondaryCurrencyCode == "USD")) ||
                ((dExchange == "IRSGD") && (mSummary.SecondaryCurrencyCode == "SGD"))*/)) {

                // we have a legit pair we're about to update.  if the groupBox is grey, let's black it.
                if (UIControls_Dict[dExchange].dExchange_GB.ForeColor != Color.Black) GroupBoxAndLabelColourActive(dExchange);
                if (UIControls_Dict[dExchange].dExchange_GB.Text != DCEs[dExchange].FriendlyName + " (fiat pair: " + mSummary.SecondaryCurrencyCode + ")") 
                    UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: " + mSummary.SecondaryCurrencyCode + ")";

                decimal midPoint = (mSummary.CurrentHighestBidPrice + mSummary.CurrentLowestOfferPrice) / 2;  // we don't use last price anymore, instead the midpoint of the spread

                System.Windows.Forms.Label tempPrice = UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Price"];

                tempPrice.Text = Utilities.FormatValue(midPoint);

                // lets update IRACCOUNTS
                if ((null != IRAF) && !IRAF.IsDisposed && dExchange == "IR") {
                    IRAF.setCurrencyValues(mSummary.PrimaryCurrencyCode.ToUpper(), mSummary.CurrentHighestBidPrice);
                }

                // want to check we're not updating a colour to the same thing - minimise UI updates here
                Color priceCol = Utilities.PriceColour(DCEs[dExchange].GetPriceList(mSummary.pair));
                if (tempPrice.ForeColor != priceCol) tempPrice.ForeColor = priceCol;

                // if there's a colour, make the font bigger.  otherwise not bigger.
                // new idea, let's not mess around with the size.  
                /*if (tempPrice.ForeColor != Color.Black) {
                    tempPrice.Font = new Font(tempPrice.Font.FontFamily, 10f, FontStyle.Bold);

                    // this next bit is crazy.  When we change the size of the text, it seems to drop down a couple of pixels.  I don't know why, the label just looks lower
                    // so to fix it I push the label up 2 pixels.  But I need to keep track of whether I have already pushed the label up or not, so I use the tag property
                    if (!tempPrice.Tag.ToString().Contains("emphasised")) {  // leave the location alone - we're not changing the size anymore
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
                }*/

                string vol;
                if ((dExchange == "IRUSD") || (dExchange == "IRSGD")) vol = "";  // no vol for these exchanges, the vol is in the main groupBox
                else {
                    if (mSummary.DayVolumeXbt == 0) vol = " / 0";
                    else if (mSummary.DayVolumeXbt < 0) vol = " / ?";
                    else vol = " / " + Utilities.FormatValue(mSummary.DayVolumeXbt);
                }

                UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"].Text = Utilities.FormatValue(mSummary.spread) + vol;
                if (DCEs[dExchange].ChangedSecondaryCurrency) { 
                    PopulateCryptoComboBox(dExchange);  // need to re-populate this as it dynamically only populates the comboxbox with cryptos that the current fiat currency has a pair with
                    DCEs[dExchange].ChangedSecondaryCurrency = false;
                }

                // update tool tips.
                if (midPoint != 0) {
                    string spreadTT = "Best bid: " + Utilities.FormatValue(mSummary.CurrentHighestBidPrice) + System.Environment.NewLine +
                        "Best offer: " + Utilities.FormatValue(mSummary.CurrentLowestOfferPrice) + System.Environment.NewLine +
                        "Spread: " + Utilities.FormatValue(((mSummary.spread) / midPoint * 100), 2, false) + "%";
                    IRTickerTT_spread.SetToolTip(UIControls_Dict[dExchange].Label_Dict[mSummary.PrimaryCurrencyCode + "_Spread"], spreadTT);
                }
            }
            else Debug.Print("Pair2 don't exist, pairObj.Value.SecondaryCurrencyCode: " + mSummary.SecondaryCurrencyCode);
        }

        // this works out what the average price of a market order on the OB would be for IR.  We needed to separate this logic from the other exchanges 
        // because IR's order book is represented very differently (and used constantly)
        // IRSGD and IRSGD has no average price controls, so we can hard code IR here.
        private string DetermineAveragePrice_IR(string crypto, string fiat, string currency) {
            bool fiatSelected = currency.ToLower() == "fiat";  // if the crypto value is the same as the fiat one, it means that the selected crypto was fiat (ie they chose AUD from the drop avg price dropdown)
            crypto = (crypto == "BTC" ? "XBT" : crypto.ToUpper());
            fiat = fiat.ToUpper();
            string pair = (crypto + "-" + fiat).ToUpper();

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
                        coinCounter += subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price[fiat] : 1);  // if they have selected a fiat currency, we need to multiply by the price
                        Debug.Print("--- coinCounter is " + coinCounter + " and has just been increased by " + subOrder.Value.Volume + " (coins: " + coins + ")");
                        if (coinCounter > coins) {
                            decimal usedCoinsInThisOrder = (subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price[fiat] : 1)) - (coinCounter - coins);  // this is how many coins in this order would be required
                            Debug.Print("--- we're in the final tally up if, we will use " + usedCoinsInThisOrder + "coins from this order");
                            totalCost += usedCoinsInThisOrder * (fiatSelected ? (1 / subOrder.Value.Price[fiat]) : subOrder.Value.Price[fiat]);
                            Debug.Print("--- total cost is finally " + totalCost + " and the final increase was " + usedCoinsInThisOrder / (fiatSelected ? subOrder.Value.Price[fiat] : 1));
                            weightedAverage += (usedCoinsInThisOrder / coins) * subOrder.Value.Price[fiat];
                            string tTip = buildAvgPriceTooltip(orderSide, fiatSelected, subOrder.Value.Price[fiat], orderCount, totalCost, crypto);
                            IRTickerTT_avgPrice.SetToolTip(UIControls_Dict["IR"].AvgPrice, tTip);
                            return "Average price for " + crypto + ": " + (fiatSelected ? "$" : "") + Utilities.FormatValue(weightedAverage);  // we have finished filling the hypothetical order
                        }
                        else {  // this whole sub order is required, factor it in and then loop
                            weightedAverage += ((subOrder.Value.Volume * (fiatSelected ? subOrder.Value.Price[fiat] : 1)) / coins) * subOrder.Value.Price[fiat];
                            totalCost += subOrder.Value.Volume * (fiatSelected ? 1 : subOrder.Value.Price[fiat]);  // if fiat is selected, totalCost var represents total coins
                            Debug.Print("--- totalCost is now " + totalCost + " and was increased by " + subOrder.Value.Volume * (fiatSelected ? 1 : subOrder.Value.Price[fiat]));
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
            else if (reportType == 15) {  // update last block mined time
                BTC_LastBlock_Time_value.Text = DateTime.Now.ToString("t");
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
                switch (mSummary.SecondaryCurrencyCode) {
                    case "SGD":
                        UpdateLabels_Pair("IRSGD", mSummary);
                        break;
                    case "USD":
                        UpdateLabels_Pair("IRUSD", mSummary);
                        break;
                }

                if (mSummary.SecondaryCurrencyCode == DCEs["IR"].CurrentSecondaryCurrency) UpdateLabels_Pair("IR", mSummary);

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
                Tuple<bool, string, string> nonceIssue = (Tuple<bool, string, string>)e.UserState;  // bool is for if an issue exists, the string is the pair it exists (or not) on, second string is exchange
                if (UIControls_Dict[nonceIssue.Item3].Label_Dict.ContainsKey(nonceIssue.Item2)) {
                    System.Windows.Forms.Label tempPrice = UIControls_Dict[nonceIssue.Item3].Label_Dict[nonceIssue.Item2 + "_Price"];
                    if (nonceIssue.Item1)
                    {  // we have a nonce issue, set to grey
                        tempPrice.ForeColor = Color.Gray;
                    }
                    else
                    {  // set back to whatever colour it should be
                        tempPrice.ForeColor = Utilities.PriceColour(DCEs[nonceIssue.Item3].GetPriceList(nonceIssue.Item2));
                    }
                }
                return;
            }

            if (reportType == 28) {  // populate slack name currency combobox in settings

                // now we populate the slack name currency combobox in settings
                foreach (string irFiat in DCEs["IR"].SecondaryCurrencyList) {
                    if (SlackNameFiatCurrency_comboBox.Items.Contains(irFiat)) continue;
                    SlackNameFiatCurrency_comboBox.Items.Add(irFiat);
                }

                SlackNameFiatCurrency_comboBox.Enabled = true;  // have to enable it to change the value :/
                //Debug.Print("properties slack name currency: " + Properties.Settings.Default.SlackNameCurrency + "find string index: " + SlackNameCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameCurrency));
                if (SlackNameFiatCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameFiatCurrency) > -1)
                    SlackNameFiatCurrency_comboBox.SelectedIndex = SlackNameFiatCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameFiatCurrency);

                // after setting the default value, if this control shouldn't be enabled, disable it.
                if (!Properties.Settings.Default.Slack || !Properties.Settings.Default.SlackNameChange || (SlackNameFiatCurrency_comboBox.Items.Count == 0))
                    SlackNameFiatCurrency_comboBox.Enabled = false;


                // now we populate the crypto box
                foreach (string pair in DCEs["IR"].UsablePairs()) {
                    Tuple<string, string> splitPair = Utilities.SplitPair(pair);  // splits "XBT-AUD" into a tuple ("XBT","AUD")
                    if (splitPair.Item2 == DCEs["IR"].CurrentSecondaryCurrency) {
                        SlackNameEmojiCrypto_comboBox.Items.Add(splitPair.Item1 == "XBT" ? "BTC" : splitPair.Item1);
                    }
                }

                SlackNameEmojiCrypto_comboBox.Enabled = true;  // have to enable it to change the value :/
                //Debug.Print("properties slack name currency: " + Properties.Settings.Default.SlackNameCurrency + "find string index: " + SlackNameCurrency_comboBox.FindStringExact(Properties.Settings.Default.SlackNameCurrency));
                if (SlackNameEmojiCrypto_comboBox.FindStringExact(Properties.Settings.Default.SlackNameEmojiCrypto) > -1)
                    SlackNameEmojiCrypto_comboBox.SelectedIndex = SlackNameEmojiCrypto_comboBox.FindStringExact(Properties.Settings.Default.SlackNameEmojiCrypto);

                // after setting the default value, if this control shouldn't be enabled, disable it.
                if (!Properties.Settings.Default.Slack || !Properties.Settings.Default.SlackNameChange || (SlackNameEmojiCrypto_comboBox.Items.Count == 0))
                    SlackNameEmojiCrypto_comboBox.Enabled = false;


                return;
            }

            if (reportType == 29) {  // updates the IR currency label to include the number of negative spread fail events we've had for this currency
                // do something..
                Tuple<string, string> dExchangePair = (Tuple<string, string>)e.UserState;  // dExchange, pair  (eg "IRSGD", "BTC-SGD")
                Tuple<string, string> pairTup = Utilities.SplitPair(dExchangePair.Item2);
                DCEs[dExchangePair.Item1].negSpreadCount[dExchangePair.Item2]++;
                UIControls_Dict[dExchangePair.Item1].Label_Dict[pairTup.Item1 + "_Label"].Text = (pairTup.Item1 == "XBT" ? "BTC" : pairTup.Item1) + " (" + DCEs[dExchangePair.Item1].negSpreadCount[dExchangePair.Item2] + "):";
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



            // update the UI

            // here we iterate through the exchanges and update their group boxes and labels

            foreach (string dExchange in Exchanges) {
                if (dExchange == "BTCM" || dExchange == "IR") {  // for sockets we don't update labels or change colours.  that happens on demand.
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

            // If the account panel is open, let's update some sub panels (open orders, closed orders, etc)
            if (null != IRAF) Task.Run(() => UpdateIRAccountPanel());  // don't wait up

            // we have updated all the prices, if the average price controls are disabled, we can enable them now
            IR_CryptoComboBox.Enabled = IR_BuySellComboBox.Enabled = IR_NumCoinsTextBox.Enabled = true;
            BTCM_CryptoComboBox.Enabled = BTCM_BuySellComboBox.Enabled = BTCM_NumCoinsTextBox.Enabled = true;

            // update crypto fees

            cryptoFees_groupBox.ForeColor = Color.Black;

            string cryptoFees_LastUpdated_temp = "";
            if (BTCfee > 0) {
                cryptoFees_BTC_value.ForeColor = Color.Black;
                cryptoFees_BTC_value.Text = BTCfee + " sats/byte";
                IRTickerTT_generic.SetToolTip(cryptoFees_BTC_value, "");  // fees API is good, no TT required
            }
            else {
                cryptoFees_BTC_value.ForeColor = Color.Gray;
                cryptoFees_LastUpdated_temp = "BTC fail.  ";
                if (string.IsNullOrEmpty(IRTickerTT_generic.GetToolTip(cryptoFees_BTC_value))) {  // if this tooltip already has data in it, it means we don't want to change it because the timestamp needs to stay static until we next successfully update the fees
                    IRTickerTT_generic.SetToolTip(cryptoFees_BTC_value, "Last successful update: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            }

            if (ETHfee > 0) {
                cryptoFees_ETH_value.ForeColor = Color.Black;
                cryptoFees_ETH_value.Text = ETHfee + " gwei";
                IRTickerTT_generic.SetToolTip(cryptoFees_ETH_value, "");  // fees API is good, no TT required
            }
            else {
                cryptoFees_ETH_value.ForeColor = Color.Gray;
                cryptoFees_LastUpdated_temp += "ETH fail.";
                if (string.IsNullOrEmpty(IRTickerTT_generic.GetToolTip(cryptoFees_ETH_value))) {  // if this tooltip already has data in it, it means we don't want to change it because the timestamp needs to stay static until we next successfully update the fees
                    IRTickerTT_generic.SetToolTip(cryptoFees_ETH_value, "Last successful update: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            }

            // am i getting too tricky here?  the addition of these 2 will be -2 if they have both failed, in which case we don't care about the last updated time.  If only one has failed, we still want the updated time for the successful one.
            if (BTCfee + ETHfee > -2) {
                cryptoFees_LastUpdated_temp += DateTime.Now.ToString("HH:mm:ss");
            }

            cryptoFees_LastUpdated_value.Text = cryptoFees_LastUpdated_temp;

            if ((reportType == 1) && LoadingPanel.Visible) {
                Main.Visible = true;
                LoadingPanel.Visible = false;  // OK, all UI data is written, let's remove the loading panel.
            }

            foreach (KeyValuePair<string, SpreadGraph> sGraph in SpreadGraph_Dict) sGraph.Value.Redraw();  // update the graph
        }

        // this is only called if IRAF is not null
        private void UpdateIRAccountPanel() {
            try {
                var openOs = pIR.GetOpenOrders(IRAF.AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency);
                IRAF.drawOpenOrders(openOs.Data);
                //var closedOs = pIR.GetClosedOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency);  // shouldn't need to call this, we call it from DoWork already
                //drawClosedOrders(closedOs.Data);
                var accs = pIR.GetAccounts();
                IRAF.DrawIRAccounts(accs);
            }
            catch (Exception ex) {
                string errorMsg = ex.Message;
                if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                Debug.Print(DateTime.Now + " - Trying to do a sneaky account update, but we had a failure: " + errorMsg);
            }
        }

        private void PollingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled) {  // if it was cancelled, we start it up again.  The only reason it would be cancelled is if the user chooses a different secondary currency.
                Debug.Print(DateTime.Now + " - Poll was cancelled, now restarting...");

            }
            else {
                if (null == e.Result) {
                    Debug.Print(DateTime.Now + " - POLL stopped??? why?  e.Result object was null");
                }
                else {
                    Debug.Print(DateTime.Now + " - POLL stopped!! why?? " + e.Result + " " + e.Error + " " + e.Error.Message.ToString());
                }
            }

            // regardless, I guess we need to start it up again
            pollingThread.Dispose();
            pollingThread = null;
            pollingThread = new BackgroundWorker();
            pollingThread.WorkerReportsProgress = true;
            pollingThread.WorkerSupportsCancellation = true;
            pollingThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PollingThread_DoWork);
            pollingThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.PollingThread_ReportProgress);
            pollingThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PollingThread_RunWorkerCompleted);
            pollingThread.RunWorkerAsync(); // we need to cancel to make sure we haven't already pulled the old currency from the API
        }

        // when they close the app, rename the crypto dirs to blah - old.  this way if they user happens to check the toolbar thing they'll know they're not being updated anymore
        private void IRTicker_Closing(object sender, FormClosingEventArgs e) {

            if (pIR != null && pIR.marketBaiterActive) {
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
                    if (cTokenPulseBTC != null) cTokenPulseBTC.Cancel();
                    bStick.TurnOff();
                }
            }
            if (bStickETH != null) {
                if (bStickETH.OpenDevice()) {
                    if (cTokenPulseETH != null) cTokenPulseETH.Cancel();
                    bStickETH.TurnOff();
                }
            }
            if (bStickUSDT != null) {
                if (bStickUSDT.OpenDevice()) {
                    if (cTokenPulseUSDT != null) cTokenPulseUSDT.Cancel();
                    bStickUSDT.TurnOff();
                }
            }
            if (bStickXRP != null) {
                if (bStickXRP.OpenDevice()) {
                    if (cTokenPulseXRP != null) cTokenPulseXRP.Cancel();
                    bStickXRP.TurnOff();
                }
            }

            //IRAccount_panel.Visible = false;

            if (Properties.Settings.Default.Slack && (Properties.Settings.Default.SlackToken != "")) {
                slackObj.setStatus("", "");
           }
            wSocketConnect.IR_Disconnect();  // let's see if this stops the occasional crash on exit
            wSocketConnect.stopUITimerThread();  // needed otherwise the app never actually closes
        }

        public void SettingsButton_Click(object sender, EventArgs e) {
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

                        IRAccount_button.Enabled = true;

                        pIR = new PrivateIR();
                        pIR.PrivateIR_init(Properties.Settings.Default.IRAPIPubKey, Properties.Settings.Default.IRAPIPrivKey, IRAF, DCEs["IR"], TGBot);
                        //Task.Run(() => pIR.populateClosedOrders());
                    }

                    if (string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey) || string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey)) {
                        IRAccount_button.Enabled = false;
                        pIR = null;
                    }


                    // if we have a tg code, a tg api token, and pIR isn't null, it means we can start the tgbot
                    if (TGBot_Enable_checkBox.Checked && !string.IsNullOrEmpty(TelegramCode_textBox.Text) && !string.IsNullOrEmpty(TelegramBotAPIToken_textBox.Text) && (pIR != null)) {

                        if (TGBot == null) {
                            try {

                                TGBot = new TelegramBot(TelegramBotAPIToken_textBox.Text, pIR, DCEs["IR"], this);
                            }
                            catch (Exception ex) {
                                MessageBox.Show("Error creating TelegramBot.  Maybe wrong API token?  Error mesage: " + Environment.NewLine + Environment.NewLine +
                                    ex.Message);
                                if (TGBot != null) {
                                    TGBot.StopBot();
                                    TGBot = null;
                                }
                            }
                            pIR.setTGBot(TGBot);
                        }
                        else {
                            if (Properties.Settings.Default.TelegramAPIToken != TelegramBotAPIToken_textBox.Text) {
                                TGBot.NewClient(TelegramBotAPIToken_textBox.Text);  // changes the bot api token
                            }
                        }
                        Properties.Settings.Default.TelegramCode = TelegramCode_textBox.Text;
                        Properties.Settings.Default.TelegramAPIToken = TelegramBotAPIToken_textBox.Text;

                    }
                    else {  // we can't start tgbot, so kill it if it's running
                        if (TGBot != null) {
                            try {
                                TGBot.StopBot();
                            }
                            catch (Exception ex) {
                                Debug.Print("Tried to stop TGBot but failed: " + ex.Message);
                            }
                            TGBot = null;  // hopefully this will dispose of the bot and it will responding..
                        }
                        if (pIR != null) pIR.setTGBot(null);
                        Properties.Settings.Default.TelegramCode = "";
                        Properties.Settings.Default.TelegramChatID = 0;
                    }

                    Properties.Settings.Default.TelegramAllNewMessages = TelegramNewMessages_checkBox.Checked;
                    Properties.Settings.Default.TGBot_Enable = TGBot_Enable_checkBox.Checked;
                    Properties.Settings.Default.TelegramAPIToken = TelegramBotAPIToken_textBox.Text;

                    Properties.Settings.Default.Save();
                    /*try {  // don't need this anymore, we do it in InitialiseAccountsPanel() sub below
                        if (pIR != null) System.Threading.Tasks.Task.Run(() => pIR.GetAccounts());
                    }
                    catch (Exception ex) {
                        Debug.Print("Tried to getAccounts on closing the settings screen, but it failed.  oh well: " + ex.Message);
                    }*/
                    Main.Visible = true;
                    Settings.Visible = false;
                    if ((null != IRAF) && !IRAF.IsDisposed) {
                        IRAF.InitialiseAccountsPanel();  // seeing if this helps the tg spam
                        IRAF.drawClosedOrders(null);
                        IRAF.drawOpenOrders(null);
                        IRAF.drawDepositAddress(null);  // blanks out the deposit address pane
                    }
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
                case "IRUSD":
                    fColour = Color.RoyalBlue;
                    break;
                case "IRSGD":
                    fColour = Color.RoyalBlue;
                    break;
                case "BTCM":
                    fColour = Color.OliveDrab;
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

        private void GroupBox_Click(string dExchange, string fiat = "") {
            string oldFiat = DCEs[dExchange].CurrentSecondaryCurrency;

            if (string.IsNullOrEmpty(fiat)) {
                DCEs[dExchange].NextSecondaryCurrency();
                if ((null != IRAF) && !IRAF.IsDisposed) {  // if the IRAccounts form is open, let's update it
                    System.Windows.Forms.Label labelToSelect = UIControls_Dict["IR"].Label_Dict[DCEs[dExchange].CurrentSecondaryCurrency + "_Account_Label"];
                    IRAF.fiatClicked(labelToSelect, oldFiat);
                }

            }
            else {
                if (DCEs["IR"].CurrentSecondaryCurrency != fiat) DCEs["IR"].CurrentSecondaryCurrency = fiat;
                else return;  // if they click the fiat label that is already the current fiat, then do nothing
            }

            UIControls_Dict[dExchange].dExchange_GB.Text = DCEs[dExchange].FriendlyName + " (fiat pair: updating...)";

            wSocketConnect.WebSocket_Resubscribe(dExchange, "none", oldFiat, DCEs[dExchange].CurrentSecondaryCurrency);

            UIControls_Dict[dExchange].dExchange_GB.ForeColor = Color.Gray;

            if (UIControls_Dict[dExchange].AvgPrice_Crypto.Items.Count > 0) UIControls_Dict[dExchange].AvgPrice_Crypto.SelectedIndex = 0;  // reset the selection to blank

            Utilities.ColourDCETags(Controls, dExchange);
            DCEs[dExchange].ChangedSecondaryCurrency = true;
        }

        private void ParseExchangeThreadWorker(string dExchange) {
            switch (dExchange) {
                case "IR":
                    foreach (string crypto in DCEs[dExchange].PrimaryCurrencyList) {
                        ParseDCE_IR(crypto, DCEs[dExchange].CurrentSecondaryCurrency, updateLabels: false);
                    }
                    break;
            }
        }

        public async void IR_GroupBox_Click(object sender, EventArgs e) {
            if (DCEs["IR"].HasStaticData && !pIR.marketBaiterActive) {  // can't let the secondary currency change if market baiter is running, too dangerous

                // sender will be of type Label if the user clicked the fiat label in the IR Account page
                string currency = "";
                if (sender.GetType() == typeof(System.Windows.Forms.Label)) { 
                    currency = ((System.Windows.Forms.Label)sender).Text;
                    currency = currency.Replace(":", "");  // the labels will usually be "AUD:" so we need to get rid of the colon
                }

                GroupBox_Click("IR", currency);
                GroupBoxAndLabelColourActive("IR");

                await Task.Run(() => ParseExchangeThreadWorker("IR"));  // here we start a quick thread pull volume data for IR
                // need to force a label update, otherwise they'll stay grey <no currency> until the next update comes through
                foreach (KeyValuePair<string, DCE.MarketSummary> pairObj in DCEs["IR"].GetCryptoPairs()) {
                    if (pairObj.Value.SecondaryCurrencyCode == DCEs["IR"].CurrentSecondaryCurrency) {
                        UpdateLabels_Pair("IR", pairObj.Value);
                    }
                }

                PopulateCryptoComboBox("IR");
                UIControls_Dict["IR"].dExchange_GB.Text = DCEs["IR"].FriendlyName + " (fiat pair: " + DCEs["IR"].CurrentSecondaryCurrency + ")";
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

        // this one only writes the spread as it sees it every 30 seconds or so.. to reduce the CSV file size.
        private void WriteSpreadHistoryCompressed()
        {

            //string baseFolder = "G:\\My Drive\\IR\\IRTicker\\Spread history data\\";
            string baseFolder = Properties.Settings.Default.SpreadHistoryCustomFolder;
            if (!Directory.Exists(baseFolder))
            {
                Debug.Print("Cannot write spread history info - base folder not accessible or doesn't exist");
                return;
            }
            Debug.Print(DateTime.Now + " - CSV write: " + baseFolder + " exists, let's do it.");
            string dataFolder = baseFolder + "\\" + Environment.UserName + "\\";
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);  // create it if it doesn't exist
            StreamWriter dataWriter;
            try
            {
                foreach (KeyValuePair<string, DCE> Exchange in DCEs)
                {  // spin through all the exchanges
                    if (!Exchange.Value.HasStaticData) continue;
                    ConcurrentDictionary<string, List<DataPoint>> spreadHistory = Exchange.Value.GetSpreadHistory();  // DataPoint: OADate, spread
                    foreach (string pair in Exchange.Value.UsablePairs())
                    {  // spin through all the pairs of this exchange
                        if (spreadHistory.ContainsKey(pair))
                        {

                            double totalSpread = 0;
                            int avgDivider = 0;

                            foreach (DataPoint dp in spreadHistory[pair])
                            {  // average out the last hour
                                //Debug.Print("Xval: " + dp.XValue + ", current oadate: " + DateTime.Now.ToOADate() + ", 1 hour ago: " + (DateTime.Now.ToOADate() - 0.041666666));
                                if (dp.XValue > (DateTime.Now.ToOADate() - 0.041666666))
                                {  // 0.0416666666666 is 1 hour in OADate format.  we average out the last hour
                                    totalSpread += dp.YValues[0];
                                    avgDivider++;
                                }
                            }

                            if (avgDivider > 0)
                            {
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
            catch (Exception ex)
            {
                Debug.Print("Error writing to file: " + ex.ToString());
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

        // actually BAR label..
        private void CSPT_XBT_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["BAR"], "XBT-" + DCEs["BAR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("BAR-XBT-" + DCEs["BAR"].CurrentSecondaryCurrency, SGForm);
        }

        private void IR_YFI_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "YFI-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-YFI-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
        }
        private void IR_LINK_Label3_MouseDoubleClick(object sender, MouseEventArgs e) {
            SpreadGraph SGForm = new SpreadGraph(DCEs["IR"], "LINK-" + DCEs["IR"].CurrentSecondaryCurrency, this);
            SGForm.Show();
            SpreadGraph_Dict.TryAdd("IR-LINK-" + DCEs["IR"].CurrentSecondaryCurrency, SGForm);
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

        private void IR_Reset_Button_Click(object sender, EventArgs e) {
            wSocketConnect.IR_Disconnect();
            List<string> dExchanges = new List<string>() { "IR", "IRUSD", "IRSGD" };
            foreach (string dExchange in dExchanges) {
                DCEs[dExchange].CurrentDCEStatus = "Resetting...";
                APIDown(UIControls_Dict[dExchange].dExchange_GB, dExchange);
            }
            Debug.Print(DateTime.Now + " - IR (+SGD, USD) reset button clicked");
            Debug.Print("IR (+USD, SGD) websocket connecting....");
            wSocketConnect.WebSocket_Reconnect("IR");  // using "IR" here - it resets the whole sockets and reconnects to all 3 currencies
        }


        // To support window form flashing.
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
                if (Properties.Settings.Default.SlackNameChange && (SlackNameFiatCurrency_comboBox.Items.Count > 0)) SlackNameFiatCurrency_comboBox.Enabled = true;
                if (Properties.Settings.Default.SlackNameChange && (SlackNameEmojiCrypto_comboBox.Items.Count > 0)) SlackNameEmojiCrypto_comboBox.Enabled = true;
            }

            else {
                slackDefaultNameTextBox.Enabled = false;
                slackNameChangeCheckBox.Enabled = false;
                slackToken_textBox.Enabled = false;
                setSlackStatus(true);  // reset the slack name to the default
                SlackNameFiatCurrency_comboBox.Enabled = false;
                SlackNameEmojiCrypto_comboBox.Enabled = false;
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

            if (Properties.Settings.Default.SlackNameChange && (SlackNameFiatCurrency_comboBox.Items.Count > 0)) SlackNameFiatCurrency_comboBox.Enabled = true;
            else SlackNameFiatCurrency_comboBox.Enabled = false;

            if (Properties.Settings.Default.SlackNameChange && (SlackNameEmojiCrypto_comboBox.Items.Count > 0)) SlackNameEmojiCrypto_comboBox.Enabled = true;
            else SlackNameEmojiCrypto_comboBox.Enabled = false;
        }

        private void NegativeSpread_checkBox_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.NegativeSpread = NegativeSpread_checkBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void SlackNameCurrency_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            Properties.Settings.Default.SlackNameFiatCurrency = (string)SlackNameFiatCurrency_comboBox.SelectedItem;
            Properties.Settings.Default.Save();
        }

        private void SlackNameEmojiCrypto_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            Properties.Settings.Default.SlackNameEmojiCrypto = (string)SlackNameEmojiCrypto_comboBox.SelectedItem;
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
            if ((null == IRAF) || IRAF.IsDisposed) {
                IRAF = new IRAccountsForm(this, pIR, DCEs["IR"], TGBot);
                IRAF.Show();
            }
            else {
                IRAF.BringToFront();
                IRAF.Show();
                IRAF.Activate();
            }            
        }

        private void EditKeys_button_Click(object sender, EventArgs e) {
            Form EditKeys = new AccountAPIKeys();
            EditKeys.ShowDialog();

            populateIRAPIKeysSettings();
        }

        // if we get sent the IRAF form object, then use it.
        public void populateIRAPIKeysSettings(IRAccountsForm currentIRAF = null) {

            bool IRAFavailable = false;
            IRAccountsForm _IRAF;

            if (currentIRAF != null) {
                _IRAF = currentIRAF;
            }
            else {
                _IRAF = IRAF;
            }
            if ((null != _IRAF) && !_IRAF.IsDisposed) IRAFavailable = true;
            

            APIKeys_comboBox.Items.Clear();
            if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.Items.Clear();
                        
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly1) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey1) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey1)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly1, Properties.Settings.Default.IRAPIPubKey1, Properties.Settings.Default.IRAPIPrivKey1);
                APIKeys_comboBox.Items.Add(grp);
                if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly2) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey2) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey2)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly2, Properties.Settings.Default.IRAPIPubKey2, Properties.Settings.Default.IRAPIPrivKey2);
                APIKeys_comboBox.Items.Add(grp);
                if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly3) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey3) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey3)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly3, Properties.Settings.Default.IRAPIPubKey3, Properties.Settings.Default.IRAPIPrivKey3);
                APIKeys_comboBox.Items.Add(grp);
                if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.Items.Add(grp);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly4) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey4) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey4)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly4, Properties.Settings.Default.IRAPIPubKey4, Properties.Settings.Default.IRAPIPrivKey4);
                APIKeys_comboBox.Items.Add(grp);
                if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.Items.Add(grp);

            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.APIFriendly5) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey5) && !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey5)) {
                AccountAPIKeys.APIKeyGroup grp = new AccountAPIKeys.APIKeyGroup(Properties.Settings.Default.APIFriendly5, Properties.Settings.Default.IRAPIPubKey5, Properties.Settings.Default.IRAPIPrivKey5);
                APIKeys_comboBox.Items.Add(grp);
                if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.Items.Add(grp);

            }

            bool foundKey = false;
            foreach (AccountAPIKeys.APIKeyGroup chosenKey in APIKeys_comboBox.Items) {
                if (chosenKey.friendlyName == Properties.Settings.Default.APIFriendly) {
                    //Select this one somehow..
                    APIKeys_comboBox.SelectedItem = chosenKey;
                    if (IRAFavailable) _IRAF.AccountAPIKeys_comboBox.SelectedItem = chosenKey;  // the Items collections in each combobox control is idental (see above), so no need to iterate through both
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
                if (IRAFavailable) {
                    _IRAF.AccountAPIKeys_comboBox.Items.Add(grp);
                    _IRAF.AccountAPIKeys_comboBox.SelectedItem = grp;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void APIKeys_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            pIRAccountChanged((System.Windows.Forms.ComboBox)sender);
        }

        public void pIRAccountChanged(System.Windows.Forms.ComboBox cb) { 

            if (Properties.Settings.Default.APIFriendly != ((AccountAPIKeys.APIKeyGroup)cb.SelectedItem).friendlyName) {

                // a new key has been chosen, let's reset the closed orders.
                Debug.Print(DateTime.Now + " - API key has been changed from " + Properties.Settings.Default.APIFriendly + " to " + ((AccountAPIKeys.APIKeyGroup)cb.SelectedItem).friendlyName + ", about to clear the closedOrdersFirstRun ductionary...");

                Properties.Settings.Default.APIFriendly = ((AccountAPIKeys.APIKeyGroup)cb.SelectedItem).friendlyName;
                Properties.Settings.Default.IRAPIPubKey = ((AccountAPIKeys.APIKeyGroup)cb.SelectedItem).pubKey;
                Properties.Settings.Default.IRAPIPrivKey = ((AccountAPIKeys.APIKeyGroup)cb.SelectedItem).privKey;
                Properties.Settings.Default.Save();

                //int friendlyNameLen = Properties.Settings.Default.APIFriendly.Length;
                //if (friendlyNameLen > 20) friendlyNameLen = 20;

                //if ((null != IRAF) && !IRAF.IsDisposed) IRAF.UpdateAccountNameButton(Properties.Settings.Default.APIFriendly.Substring(0, friendlyNameLen) + (friendlyNameLen != Properties.Settings.Default.APIFriendly.Length ? "..." : ""));

                updatePIRAccountComboBox(Properties.Settings.Default.APIFriendly);  // ensures both API key comboboxes get updated

                if (TGBot != null) {  // need to clear the TGBot dictionaries first before we re-initialise pIR object as privateIR_init will alert TGBot to API key's closed orders
                    TGBot.closedOrdersFirstRun = new ConcurrentDictionary<string, bool>();
                    TGBot.notifiedOrders = new ConcurrentDictionary<string, List<Guid>>();
                    Debug.Print(DateTime.Now + " - closedOrdersFirstRun has been cleared.  There should be no old orders reported.  Size of dict now: " + TGBot.closedOrdersFirstRun.Count);
                    Debug.Print(DateTime.Now + " - notifiedOrders has been cleared.  There should be no old orders reported.  Size of dict now: " + TGBot.notifiedOrders.Count);
                }
                if (pIR != null) {
                    pIR.APIKeyHasChanged();
                    pIR.PrivateIR_init(Properties.Settings.Default.IRAPIPubKey, Properties.Settings.Default.IRAPIPrivKey, IRAF, DCEs["IR"], TGBot);
                }
            }
        }

        // the idea here is we have 2 comboboxes that display the private IR API key.  We need to keep them in sync.  so when
        // we change the key on one, we check both and update the one that's wrong.   hmm how to do this without triggering a change...
        private void updatePIRAccountComboBox(string APIKeyName) {
            // need to first disable the event handler, cahnge the box, then re-enable the event handler like this:
            // combo.SelectedIndexChanged -= EventHandler<SelectedIndexChangedEventArgs> SomeEventHandler;
            //combo.SelectedIndex = 0;
            //combo.SelectedIndexChanged += EventHandler < SelectedIndexChangedEventArgs > SomeEventHandler;

            // First check the IRTicker (settings panel) combobox
            if (APIKeys_comboBox.SelectedItem.ToString() != APIKeyName) {  // if the currently selected item in this combobox is not correct, then cycle through the available options and select the correct one

                APIKeys_comboBox.SelectedIndexChanged -= new EventHandler (APIKeys_comboBox_SelectedIndexChanged);  // we're about to change the selected item, but we don't want to trigger the code, so remove the handler
                foreach (var item in APIKeys_comboBox.Items) {
                    if (item.ToString() == APIKeyName) {
                        APIKeys_comboBox.SelectedItem = item;
                    }
                }
                APIKeys_comboBox.SelectedIndexChanged += new EventHandler(APIKeys_comboBox_SelectedIndexChanged);  // OK, change done - re-register the handler
            }

            // Next check the IR Accounts Form combobox
            if ((null != IRAF) && !IRAF.IsDisposed) {
                if (IRAF.AccountAPIKeys_comboBox.SelectedItem.ToString() != APIKeyName) {  // if the currently selected item in this combobox is not correct, then cycle through the available options and select the correct one

                    IRAF.AccountAPIKeys_comboBox.SelectedIndexChanged -= new EventHandler(IRAF.AccountAPIKeys_comboBox_SelectedIndexChanged);  // we're about to change the selected item, but we don't want to trigger the code, so remove the handler
                    foreach (var item in IRAF.AccountAPIKeys_comboBox.Items) {
                        if (item.ToString() == APIKeyName) {
                            IRAF.AccountAPIKeys_comboBox.SelectedItem = item;
                        }
                    }
                    IRAF.AccountAPIKeys_comboBox.SelectedIndexChanged += new EventHandler(IRAF.AccountAPIKeys_comboBox_SelectedIndexChanged);  // OK, change done - re-register the handler
                }
            }

        }

        private void TGReset_button_Click(object sender, EventArgs e) {
            if (TGBot != null) {
                TGBot.SendMessage("Un-authenticating...");
                TGBot.ResetBot();
                showBalloon("TelegramBot", "TelegramBot has been un-authenticated.");
            }
            else {
                showBalloon("TelegramBot", "No action taken - the TelegramBot is not currently authenticated");
            }
        }

        public void showBalloon(string title, string body) {

            IRT_notification.Visible = true;

            if (title != null) {
                IRT_notification.BalloonTipTitle = title;
            }

            if (body != null) {
                IRT_notification.BalloonTipText = body;
            }

            IRT_notification.ShowBalloonTip(10000);
        }

        private void AccountName_button_Click(object sender, EventArgs e) {
            //LastPanel = IRAccount_panel;
            populateIRAPIKeysSettings();  // populate the api keys drop down.
            BringToFront();  // should make the main form focused
            Settings.Visible = true;
        }

        public partial class BlockHeight
        {
            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("time")]
            public long Time { get; set; }

            [JsonProperty("block_index")]
            public long BlockIndex { get; set; }

            [JsonProperty("height")]
            public long Height { get; set; }

            [JsonProperty("txIndexes")]
            public object[] TxIndexes { get; set; }
        }
        public class mempoolSpace {
            public int fastestFee { get; set; }
            public int halfHourFee { get; set; }
            public int hourFee { get; set; }
            public int minimumFee { get; set; }
        }

        public class GasPriceRange
        {
            public int _4 { get; set; }
            public int _6 { get; set; }
            public int _8 { get; set; }
            public int _10 { get; set; }
            public int _20 { get; set; }
            public int _30 { get; set; }
            public int _40 { get; set; }
            public int _50 { get; set; }
            public int _60 { get; set; }
            public int _70 { get; set; }
            public int _80 { get; set; }
            public int _90 { get; set; }
            public int _100 { get; set; }
            public int _110 { get; set; }
            public int _120 { get; set; }
            public int _130 { get; set; }
            public int _140 { get; set; }
            public int _150 { get; set; }
            public int _160 { get; set; }
            public int _170 { get; set; }
            public int _180 { get; set; }
            public int _190 { get; set; }
            public int _200 { get; set; }
            public int _220 { get; set; }
            public int _240 { get; set; }
            public int _260 { get; set; }
            public int _280 { get; set; }
            public int _300 { get; set; }
            public int _320 { get; set; }
            public int _340 { get; set; }
            public int _360 { get; set; }
            public int _380 { get; set; }
            public int _400 { get; set; }
            public int _420 { get; set; }
            public int _440 { get; set; }
            public int _460 { get; set; }
            public int _480 { get; set; }
            public int _500 { get; set; }
            public int _520 { get; set; }
            public int _540 { get; set; }
            public int _560 { get; set; }
            public int _580 { get; set; }
            public int _600 { get; set; }
            public int _620 { get; set; }
            public int _640 { get; set; }
            public int _660 { get; set; }
            public int _680 { get; set; }
            public double _700 { get; set; }
            public double _710 { get; set; }
            public double _720 { get; set; }
            public double _740 { get; set; }
            public double _760 { get; set; }
            public double _780 { get; set; }
            public double _800 { get; set; }
            public double _820 { get; set; }
            public double _840 { get; set; }
            public int _860 { get; set; }
            public double _880 { get; set; }
            public double _900 { get; set; }
            public double _920 { get; set; }
            public double _940 { get; set; }
            public double _960 { get; set; }
            public double _980 { get; set; }
            public double _1000 { get; set; }
            public double _1020 { get; set; }
            public double _1040 { get; set; }
            public double _1060 { get; set; }
            public double _1080 { get; set; }
            public double _1100 { get; set; }
            public double _1120 { get; set; }
        }

        public class ESResult {
            public string LastBlock { get; set; }
            public string SafeGasPrice { get; set; }
            public string ProposeGasPrice { get; set; }
            public string FastGasPrice { get; set; }
            public string suggestBaseFee { get; set; }
            public string gasUsedRatio { get; set; }
        }

        public class EtherscanRoot {
            public string status { get; set; }
            public string message { get; set; }
            public ESResult result { get; set; }
        }

        public class EGSRoot
        {
            public int fast { get; set; }
            public int fastest { get; set; }
            public int safeLow { get; set; }
            public int average { get; set; }
            public double block_time { get; set; }
            public int blockNum { get; set; }
            public double speed { get; set; }
            public double safeLowWait { get; set; }
            public double avgWait { get; set; }
            public double fastWait { get; set; }
            public double fastestWait { get; set; }
            public GasPriceRange gasPriceRange { get; set; }
        }
        private void Balance_button_Click(object sender, EventArgs e) {
            if (null != pIR) {
                Balance balForm = new Balance(DCEs["IR"], pIR);  // sending the IR DCE - it's really just to build the rows of cryptos, doesn't need any specific data
                balForm.Show();
            }
            else MessageBox.Show("IR API config not setup, go to Settings and do it first", "API not configured", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
