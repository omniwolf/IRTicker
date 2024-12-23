using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using IndependentReserve.DotNetClientApi.Data;
using System.Globalization;

namespace IRTicker
{
    public partial class Balance : Form {
        DCE DCE_IR;
        PrivateIR pIR;

        Dictionary<string, TextBox> LoanDict = new Dictionary<string, TextBox>();
        Dictionary<string, TextBox> SlushDict = new Dictionary<string, TextBox>();
        Dictionary<string, Label> TotalBalDict = new Dictionary<string, Label>();
        Dictionary<string, Label> AvailsBalDict = new Dictionary<string, Label>();
        Dictionary<string, Label> OutByDict = new Dictionary<string, Label>();

        // outside string is the platform (eg "Independent Reserve", "B2C2", etc).  Inner string is crypto
        Dictionary<string, Dictionary<string, BalanceData>> masterBalanceDict = new Dictionary<string, Dictionary<string, BalanceData>>();

        // holds info about crypto prices that we refresh from time to time (eg when we re-draw dynamic rows, or click reload, or build slack text
        Dictionary<string, DCE.MarketSummary> CryptoPairs;

        // DCE_IR.PrimaryCurrencyList won't cut it - we need to include other currencies
        List<string> IRCurrencies_and_extras = new List<string>();

        // which other currencies?  These:
        List<string> Currencies_extras = new List<string>() { "SOL", "WETH", "WBTC", "CHI" };

        bool SaveLoanSlush = true;  // false if we're clearing stuff and don't want to make changes to the saved entries.  Taking bets on me having to remove this auto-save nonsense and build a save button again

        BalSettings balSetting_form;

        string SlackMessageTS = "";  // stores the most recent slack message identifier, we use this to thread the balance messages
        DateTime LastSlackThread = new DateTime(2000,1,1);  // last time we sent slack message.  year 2000 is as good as null.

        public Balance(DCE _dce_IR, PrivateIR _pIR) {
            InitializeComponent();

            DCE_IR = _dce_IR;
            pIR = _pIR;

            Platform_comboBox.SelectedIndex = 0;  // choose IR

            BuildUI();
        }

        enum Platform {
            IROTC,
            IROTCSG,
            B2C2,
            Coinbase,
            MetaMask,
            TrigonX
        }

        private async Task<Dictionary<string, Account>> pullAccounts(Platform platform) {
            // pull the balance
            switch (platform) {
                case Platform.IROTC:
                    return pIR.GetAccounts(Properties.Settings.Default.IROTCAPIKey, Properties.Settings.Default.IROTCAPISecret);
                    break;
                case Platform.IROTCSG:
                    return pIR.GetAccounts(Properties.Settings.Default.IROTCSGAPIKey, Properties.Settings.Default.IROTCSGAPISecret);
                    break;
            }
            Debug.Print("In Balance class, should never get here.  tried to pull IR settings for some thing we don't know about");
             return null;
        }

        // should only be called once!
        private async void BuildUI() {
            Task<Dictionary<string, DCE.MarketSummary>> cryptoPairsTask = GetCryptoPairs();
            int RowCount = 2;  // start at 2 because we don't want to start at 25 pixels, we want to start at 50 (because drop down and buttons at the top)
            int xLocation = 20;
            int yLocation = 50;

            // currency label
            Label irTempLabel_heading = new Label();
            irTempLabel_heading.AutoSize = true;
            irTempLabel_heading.Location = new Point(xLocation, yLocation);
            irTempLabel_heading.Font = new Font(irTempLabel_heading.Font.FontFamily, irTempLabel_heading.Font.Size, FontStyle.Bold);
            irTempLabel_heading.Text = "Currency";
            irTempLabel_heading.Name = "BalCurrencyHeading_label";
            Controls.Add(irTempLabel_heading);


            // Total balance labels
            Label irTempTotBalanceLabel_heading = new Label();
            irTempTotBalanceLabel_heading.AutoSize = true;
            irTempTotBalanceLabel_heading.Location = new Point(xLocation + 70, yLocation);
            irTempTotBalanceLabel_heading.Font = new Font(irTempTotBalanceLabel_heading.Font.FontFamily, irTempTotBalanceLabel_heading.Font.Size, FontStyle.Bold);
            irTempTotBalanceLabel_heading.Text = "Total balance";
            irTempTotBalanceLabel_heading.Name = "BalTotalBalanceHeading_label";
            Controls.Add(irTempTotBalanceLabel_heading);

            // Available balance labels
            Label irTempAvailsBalanceLabel_heading = new Label();
            irTempAvailsBalanceLabel_heading.AutoSize = true;
            irTempAvailsBalanceLabel_heading.Location = new Point(xLocation + 170, yLocation);
            irTempAvailsBalanceLabel_heading.Font = new Font(irTempAvailsBalanceLabel_heading.Font.FontFamily, irTempAvailsBalanceLabel_heading.Font.Size, FontStyle.Bold);
            irTempAvailsBalanceLabel_heading.Text = "Available balance";
            irTempAvailsBalanceLabel_heading.Name = "BalAvailableBalanceHeading_label";
            Controls.Add(irTempAvailsBalanceLabel_heading);

            // Loan input fields
            Label irTempLoan_textbox_heading = new Label();
            irTempLoan_textbox_heading.Size = new Size(70, 20);
            irTempLoan_textbox_heading.Location = new Point(xLocation + 300, yLocation);
            irTempLoan_textbox_heading.Font = new Font(irTempLoan_textbox_heading.Font.FontFamily, irTempLoan_textbox_heading.Font.Size, FontStyle.Bold);
            irTempLoan_textbox_heading.Name = "BalLoanHeading_label";
            irTempLoan_textbox_heading.Text = "Loan size";
            Controls.Add(irTempLoan_textbox_heading);

            // slush input fields
            Label irTempSlush_textbox_heading = new Label();
            irTempSlush_textbox_heading.Size = new Size(150, 20);
            irTempSlush_textbox_heading.Location = new Point(xLocation + 400, yLocation);
            irTempSlush_textbox_heading.Font = new Font(irTempSlush_textbox_heading.Font.FontFamily, irTempSlush_textbox_heading.Font.Size, FontStyle.Bold);
            irTempSlush_textbox_heading.Name = "BalSlushHeading_label";
            irTempSlush_textbox_heading.Text = "Slush size";
            Controls.Add(irTempSlush_textbox_heading);

            // out by input fields
            Label irTempOutBy_label_heading = new Label();
            irTempOutBy_label_heading.Size = new Size(70, 20);
            irTempOutBy_label_heading.Location = new Point(xLocation + 600, yLocation);
            irTempOutBy_label_heading.Font = new Font(irTempSlush_textbox_heading.Font.FontFamily, irTempSlush_textbox_heading.Font.Size, FontStyle.Bold);
            irTempOutBy_label_heading.Name = "BalOutByHeading_label";
            irTempOutBy_label_heading.Text = "Out by";
            Controls.Add(irTempOutBy_label_heading);

            RowCount++;

            RowCount = buildDynamicRows(DCE_IR.SecondaryCurrencyList, RowCount);

            // separator label
            Label irtempSeparator = new Label();
            irtempSeparator.AutoSize = true;
            irtempSeparator.Location = new Point(xLocation, RowCount * 25);
            irtempSeparator.Text = "___________________________________________________________________________________________________________________";
            irtempSeparator.Name = "BalSeparator_label";
            Controls.Add(irtempSeparator);

            RowCount += 2;
            CryptoPairs = await cryptoPairsTask;

            // initialise entire currency list, IR ones and extras we care about
            IRCurrencies_and_extras = DCE_IR.PrimaryCurrencyList.ToList();
            foreach (string extraCurr in Currencies_extras) {
                if (!DCE_IR.PrimaryCurrencyList.Contains(extraCurr)) {
                    IRCurrencies_and_extras.Add(extraCurr);
                }
            }

            buildDynamicRows(IRCurrencies_and_extras, RowCount);

            DrawIR(Platform.IROTC);  // always draw IR first, 
        }

        // this builds the dynamic row controls.  Will be sent secondary currency list then primary currency list. Returns how many rows were created (eg RowCount)
        private int buildDynamicRows(List<string> Currencies, int RowCount) {
            foreach (string curr in Currencies) {

                int yLocation = RowCount * 25;  // this should move the thingo down by 25 pixels each time
                int xLocation = 20;

                // fiat label
                Label irTempLabel = new Label();
                irTempLabel.AutoSize = true;
                irTempLabel.Location = new Point(xLocation, yLocation);
                irTempLabel.Text = curr;
                irTempLabel.Name = "BalCurrency" + curr + "_label";
                Controls.Add(irTempLabel);

                // Total balance labels
                Label irTempTotBalanceLabel = new Label();
                irTempTotBalanceLabel.AutoSize = true;
                irTempTotBalanceLabel.Location = new Point(xLocation + 70, yLocation);
                TotalBalDict.Add(curr, irTempTotBalanceLabel);
                irTempTotBalanceLabel.Name = "BalTotalBalance" + curr + "_label";
                Controls.Add(irTempTotBalanceLabel);

                // Available balance labels
                Label irTempAvailsBalanceLabel = new Label();
                irTempAvailsBalanceLabel.AutoSize = true;
                irTempAvailsBalanceLabel.Location = new Point(xLocation + 170, yLocation);
                AvailsBalDict.Add(curr, irTempAvailsBalanceLabel);
                irTempAvailsBalanceLabel.Name = "BalAvailableBalance" + curr + "_label";
                Controls.Add(irTempAvailsBalanceLabel);

                // Loan input fields
                TextBox irTempLoan_textbox = new TextBox();
                irTempLoan_textbox.Size = new Size(70, 20);
                irTempLoan_textbox.Location = new Point(xLocation + 300, yLocation);
                irTempLoan_textbox.Name = "BalLoan" + curr + "_textbox";
                irTempLoan_textbox.TextChanged += LoanSlush_TextChanged;
                LoanDict.Add(curr, irTempLoan_textbox);
                Controls.Add(irTempLoan_textbox);

                // slush input fields
                TextBox irTempSlush_textbox = new TextBox();
                irTempSlush_textbox.Size = new Size(150, 20);
                irTempSlush_textbox.Location = new Point(xLocation + 400, yLocation);
                irTempSlush_textbox.Name = "BalSlush" + curr + "_textbox";
                irTempSlush_textbox.TextChanged += LoanSlush_TextChanged;
                SlushDict.Add(curr, irTempSlush_textbox);
                Controls.Add(irTempSlush_textbox);

                // Out by labels
                Label irTempOutBy_label = new Label();
                irTempOutBy_label.AutoSize = true;
                irTempOutBy_label.Location = new Point(xLocation + 600, yLocation);

                irTempOutBy_label.Name = "BalOutBy" + curr + "_label";
                Controls.Add(irTempOutBy_label);
                OutByDict.Add(curr, irTempOutBy_label);

                RowCount++;
            }

            return RowCount;
        }

        private async void DrawTrigonX() {
            string platformName = "TrigonX";
            if (string.IsNullOrEmpty(Properties.Settings.Default.TrigonXToken)) {
                balSetting_form = new BalSettings();
                balSetting_form.Show();
                Platform_comboBox.SelectedIndex = 0;
                Platform_comboBox_SelectedIndexChanged(null, null);  // revert back to IR
                return;
            }

            /// Will return something like this on a good day:
            /// 
            /// {"ADA":"0","USD":"0.0045115155","ETH":"0","UST":"-0.0068","XRP":"0.000001","AUD":"-0.006411","BTC":"0","BCH":"0","DOT":"0","BNB":"0","CAD":"0","CHF":"0","CNH":"0","DOG":"0","EOS":"0","ETC":"0","EUR":"0","GBP":"0","ICP":"0","JPY":"0","KSM":"0","LNK":"0","LTC":"0","MXN":"0","NZD":"0","SGD":"0","TRX":"0","UNI":"0","USC":"0","XAU":"0","XLM":"0","XMR":"0","XTZ":"0","ZEC":"0"}
            Task<string> res = Utilities.GetWebData("https://trading.trigonx.com/otc/api/customer/", "Token " + Properties.Settings.Default.TrigonXToken);

            if (!masterBalanceDict.ContainsKey(platformName))
                masterBalanceDict.Add(platformName, new Dictionary<string, BalanceData>());

            masterBalanceDict[platformName].Clear();

            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(IRCurrencies_and_extras);
            LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded_TrigonX, platformName);

            string TrigonXres = await res;
            if (string.IsNullOrEmpty(TrigonXres)) {
                TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull TrigonX data";
            }
            else {
                Dictionary<string, Account> TrigonXBalances = parseTrigonXResponse(TrigonXres);
                DrawDynamicRows(DCE_IR.SecondaryCurrencyList, TrigonXBalances, false, platformName);

                DrawDynamicRows(IRCurrencies_and_extras, TrigonXBalances, true, platformName);
            }
        }

        private Dictionary<string, Account> parseTrigonXResponse(string jsonResp) {
            Dictionary<string, Account> Balances = new Dictionary<string, Account>();
            List<TrigonXResponse> TrigonDeserialised;
            try {
                TrigonDeserialised = JsonConvert.DeserializeObject<List<TrigonXResponse>>(jsonResp);
            }
            catch (Exception ex) {
                Debug.Print("Could not parse Trigon response: " + ex.Message);
                return Balances;
            }

            foreach (KeyValuePair<string, double> bal in TrigonDeserialised.FirstOrDefault().balances) {
                string currency = bal.Key;
                switch (currency) {
                    case "BTC":
                        currency = "XBT";
                        break;
                }
                if (IRCurrencies_and_extras.Contains(currency) || DCE_IR.SecondaryCurrencyList.Contains(currency)) {
                    Account tempAcc = new Account();

                    if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currency.ToLower()), out CurrencyCode currencyEnum)) {
                        tempAcc.CurrencyCode = currencyEnum;
                    }
                    else Debug.Print("Cannot parse " + currency + " in TrigonX for the ticker");

                    tempAcc.TotalBalance = Convert.ToDecimal(bal.Value);

                    tempAcc.AccountStatus = AccountStatus.Active;
                    Balances.Add(currency, tempAcc);
                }
            }
            return Balances;
        }


        private async void DrawB2C2() {
            string platformName = "B2C2";
            if (string.IsNullOrEmpty(Properties.Settings.Default.B2C2Token)) {
                balSetting_form = new BalSettings();
                balSetting_form.Show();
                Platform_comboBox.SelectedIndex = 0;
                Platform_comboBox_SelectedIndexChanged(null, null);  // revert back to IR
                return;
            }

            /// Will return something like this on a good day:
            /// 
            /// {"ADA":"0","USD":"0.0045115155","ETH":"0","UST":"-0.0068","XRP":"0.000001","AUD":"-0.006411","BTC":"0","BCH":"0","DOT":"0","BNB":"0","CAD":"0","CHF":"0","CNH":"0","DOG":"0","EOS":"0","ETC":"0","EUR":"0","GBP":"0","ICP":"0","JPY":"0","KSM":"0","LNK":"0","LTC":"0","MXN":"0","NZD":"0","SGD":"0","TRX":"0","UNI":"0","USC":"0","XAU":"0","XLM":"0","XMR":"0","XTZ":"0","ZEC":"0"}
            Task<string> res = Utilities.GetWebData("https://api.b2c2.net/balance/", "Token " + Properties.Settings.Default.B2C2Token);

            if (!masterBalanceDict.ContainsKey(platformName))
                masterBalanceDict.Add(platformName, new Dictionary<string, BalanceData>());

            masterBalanceDict[platformName].Clear();

            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(IRCurrencies_and_extras);
            LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded_B2C2, platformName);

            string B2C2res = await res;
            if (string.IsNullOrEmpty(B2C2res)) {
                TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull B2C2 data";
            }
            else {
                Dictionary<string, Account> B2C2Balances = parseB2C2Response(B2C2res);
                DrawDynamicRows(DCE_IR.SecondaryCurrencyList, B2C2Balances, false, platformName);
                DrawDynamicRows(IRCurrencies_and_extras, B2C2Balances, true, platformName);
            }
        }

        private void DrawDynamicRows(List<string> Currencies, Dictionary<string, Account> Balances, bool crypto, string platformName) {
            foreach (string curr in Currencies) {
                if (!masterBalanceDict[platformName].ContainsKey(curr))
                    masterBalanceDict[platformName].Add(curr, new BalanceData());  // at this point we should already have the loan and slush data in here
                if (Balances.ContainsKey(curr)) {
                    if (TotalBalDict.ContainsKey(curr)) {
                        TotalBalDict[curr].Text = Utilities.FormatValue(Balances[curr].TotalBalance, (crypto ? 8 : 2), false);
                        masterBalanceDict[platformName][curr].TotalBalance = Balances[curr].TotalBalance;
                    }
                    if (AvailsBalDict.ContainsKey(curr)) {
                        AvailsBalDict[curr].Text = Utilities.FormatValue(Balances[curr].AvailableBalance, (crypto ? 8 : 2), false);
                        masterBalanceDict[platformName][curr].AvailableBalance = Balances[curr].AvailableBalance;
                    }
                    masterBalanceDict[platformName][curr].isActive = true;
                }

                if (LoanDict.ContainsKey(curr)) {
                    LoanDict[curr].Text = masterBalanceDict[platformName][curr].LoanStr;
                }
                if (SlushDict.ContainsKey(curr)) {
                    SlushDict[curr].Text = masterBalanceDict[platformName][curr].SlushStr;
                }

                if (OutByDict.ContainsKey(curr) && Balances.ContainsKey(curr)) {

                    decimal LoanDec = 0;
                    decimal SlushDec = 0;
                    if (decimal.TryParse(LoanDict[curr].Text, out decimal _LoanDec)) {
                        LoanDec = _LoanDec;
                        masterBalanceDict[platformName][curr].Loan = LoanDec;
                    }
                    if (decimal.TryParse(SlushDict[curr].Text, out decimal _SlushDec)) {
                        SlushDec = _SlushDec;
                        masterBalanceDict[platformName][curr].Slush = SlushDec;
                    }
                    decimal outby = Balances[curr].TotalBalance - LoanDec - SlushDec;

                    // colour the out by text to give a quick idea where we're at
                    OutByDict[curr].ForeColor = DetermineOutByColour(curr, outby, SlushDec);

                    if (crypto) OutByDict[curr].Text = Utilities.FormatValue(outby);
                    else OutByDict[curr].Text = Utilities.FormatValue(outby, 2, false);
                    masterBalanceDict[platformName][curr].OutBy = outby;
                }
            }
        }

        // parsing something like this:
        // {"ADA":"0","USD":"0.0045115155","ETH":"0","UST":"-0.0068","XRP":"0.000001","AUD":"-0.006411","BTC":"0","BCH":"0","DOT":"0","BNB":"0","CAD":"0","CHF":"0","CNH":"0","DOG":"0","EOS":"0","ETC":"0","EUR":"0","GBP":"0","ICP":"0","JPY":"0","KSM":"0","LNK":"0","LTC":"0","MXN":"0","NZD":"0","SGD":"0","TRX":"0","UNI":"0","USC":"0","XAU":"0","XLM":"0","XMR":"0","XTZ":"0","ZEC":"0"}
        private Dictionary<string, Account> parseB2C2Response(string resp) {
            resp = resp.Replace("{", "");
            resp = resp.Replace("}", "");
            resp = resp.Replace("\"", "");
            resp = resp.Trim();

            Dictionary<string, Account> B2C2Balances = new Dictionary<string, Account>();

            string[] respArray = resp.Split(',');

            foreach (string currency in respArray) {
                Account tempAccount = new Account();

                string[] currPair = currency.Split(':');
                string normalisedCurrency = currPair[0];

                if (normalisedCurrency == "BTC") normalisedCurrency = "XBT";
                else if (normalisedCurrency == "UST") normalisedCurrency = "USDT";
                else if (normalisedCurrency == "DOG") normalisedCurrency = "DOGE";
                else if (normalisedCurrency == "LNK") normalisedCurrency = "LINK";
                else if (normalisedCurrency == "USC") normalisedCurrency = "USDC";

                if (DCE_IR.SecondaryCurrencyList.Contains(normalisedCurrency) || IRCurrencies_and_extras.Contains(normalisedCurrency)) {
                    if (decimal.TryParse(currPair[1], out decimal balance)) {
                        tempAccount.TotalBalance = balance;
                    }
                    else Debug.Print("Cannot parse " + currPair[1] + " in B2C2 for the balance");
                    if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currPair[0].ToLower()), out CurrencyCode currencyEnum)) {
                        tempAccount.CurrencyCode = currencyEnum;
                    }
                    else Debug.Print("Cannot parse " + currPair[0] + " in B2C2 for the ticker");
                    tempAccount.AccountStatus = AccountStatus.Active;


                    B2C2Balances.Add(normalisedCurrency, tempAccount);
                }
            }
            return B2C2Balances;
        }

        private void ClearDynamicRows(List<string> Currencies) {
            foreach (string curr in Currencies) {
                if (TotalBalDict.ContainsKey(curr)) {
                    TotalBalDict[curr].Text = "";
                }
                if (AvailsBalDict.ContainsKey(curr)) {
                    AvailsBalDict[curr].Text = "";
                }

                if (LoanDict.ContainsKey(curr)) {
                    LoanDict[curr].Text = "";
                }
                if (SlushDict.ContainsKey(curr)) {
                    SlushDict[curr].Text = "";
                }

                if (OutByDict.ContainsKey(curr)) {
                    OutByDict[curr].Text = "";
                }
            }
        }

        private async Task<Dictionary<string, DCE.MarketSummary>> GetCryptoPairs() {
            return DCE_IR.GetCryptoPairs();
        }

        private async void DrawIR(Platform platform) {

            Task<Dictionary<string, Account>> meeTask = pullAccounts(platform);
            string platformName = "Independent Reserve " + platform.ToString();

            if (!masterBalanceDict.ContainsKey(platformName)) 
                masterBalanceDict.Add(platformName, new Dictionary<string, BalanceData>());

            masterBalanceDict[platformName].Clear();

            // let's draw some controls
            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(IRCurrencies_and_extras);

            // populate numbas
            switch (platform) {
                case Platform.IROTC:
                    LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded, platformName);
                    break;
                case Platform.IROTCSG:
                    LoanSlushDecode(Properties.Settings.Default.LoanSlushEncodedIROTCSG, platformName);
                    break;
            }

            Dictionary<string, Account> pAccounts = await meeTask;  // the idea here is we pause until this task is done

            DrawDynamicRows(DCE_IR.SecondaryCurrencyList, pAccounts, false, platformName);
            DrawDynamicRows(IRCurrencies_and_extras, pAccounts, true, platformName);
        }

        private void LoanSlush_TextChanged(object sender, EventArgs e) {
            if (!SaveLoanSlush) return;  // we're reloading things, don't make changes.
            // now we encode the slush and loan data into a string and save it
            string loanSlushEncoded = "";

            foreach (string fiat in DCE_IR.SecondaryCurrencyList) {
                loanSlushEncoded += fiat + ";" + LoanDict[fiat].Text + ";" + SlushDict[fiat].Text + "?";
            }

            foreach (string crypto in IRCurrencies_and_extras) {
                loanSlushEncoded += crypto + ";" + LoanDict[crypto].Text + ";" + SlushDict[crypto].Text + "?";
            }

            /*if (closing) {
                if (Properties.Settings.Default.LoanSlushEncoded != loanSlushEncoded) {
                    var res = MessageBox.Show("Save your loan and slush values?", "Unsaved changes!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No) return;
                }
            }*/

            switch (Platform_comboBox.SelectedItem.ToString()) {
                case "Independent Reserve IROTC":
                    Properties.Settings.Default.LoanSlushEncoded = loanSlushEncoded;
                    break;
                case "Independent Reserve IROTCSG":
                    Properties.Settings.Default.LoanSlushEncodedIROTCSG = loanSlushEncoded;
                    break;
                case "B2C2":
                    Properties.Settings.Default.LoanSlushEncoded_B2C2 = loanSlushEncoded;
                    break;
                case "Coinbase":
                    Properties.Settings.Default.LoanSlushEncoded_Coinbase = loanSlushEncoded;
                    break;
                case "IROTC MetaMask":
                    Properties.Settings.Default.LoanSlushEncoded_IROTCMetaMask = loanSlushEncoded;
                    break;
                case "TrigonX":
                    Properties.Settings.Default.LoanSlushEncoded_TrigonX = loanSlushEncoded;
                    break;
            }

            Properties.Settings.Default.Save();
        }

        // takes the input, expecting in a format:
        // <crypto>;<loan value>;<slush value>?...  and repeats
        // will decode this and populate the loan and slush text boxes with the data
        private void LoanSlushDecode(string EncodedLoanSlush, string platformName) {

            //Dictionary<string, Tuple<string, string>> result = new Dictionary<string, Tuple<string, string>>();

            string[] loanSlushCurrencies = EncodedLoanSlush.Split('?');
            if (loanSlushCurrencies.Length < 1) return;
            foreach (string loanSlushCurrency in loanSlushCurrencies) {
                string[] currencySplit = loanSlushCurrency.Split(';');
                if (currencySplit.Length != 3) continue;
                if (!masterBalanceDict[platformName].ContainsKey(currencySplit[0])) 
                    masterBalanceDict[platformName].Add(currencySplit[0], new BalanceData());


                // if it's a legit currency, and we have loan and slush textbox controls for it, then...
                if ((IRCurrencies_and_extras.Contains(currencySplit[0]) || DCE_IR.SecondaryCurrencyList.Contains(currencySplit[0])) && 
                    LoanDict.ContainsKey(currencySplit[0]) && SlushDict.ContainsKey(currencySplit[0])) {
                    masterBalanceDict[platformName][currencySplit[0]].LoanStr = currencySplit[1];
                    masterBalanceDict[platformName][currencySplit[0]].SlushStr = currencySplit[2];
                    //result.Add(currencySplit[0], new Tuple<string, string>(currencySplit[1], currencySplit[2]));
                }
            }
            return;
        }

        private void BalReload_button_Click(object sender, EventArgs e) {
            Platform_comboBox_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformName"></param>
        /// <returns>Item1 = string for slack, Item2 = string for .txt file</returns>
        private Tuple<string, string> GenerateBalanceStrings(string platformName) {
            if (masterBalanceDict.ContainsKey(platformName)) {

                Dictionary<string, BalanceData> BalancesDict = masterBalanceDict[platformName];
                string slackPlatformName = platformName;
                switch (slackPlatformName) {
                    case "Independent Reserve":
                        slackPlatformName = ":ir:";
                        break;
                    case "TrigonX":
                        slackPlatformName = ":trigon:";
                        break;
                    default:
                        slackPlatformName = "`" + slackPlatformName + "`";
                        break;
                }
                bool reachedCryptoYet = false;  // hack to let put in an empty line in between cryptos and fiat.  Only will work if fiat is all on the top as we cycle through the dictionary
                string slackString = "*Platform:* " + slackPlatformName + Environment.NewLine + Environment.NewLine;
                string TXTString = "Platform: " + platformName + Environment.NewLine + Environment.NewLine;
                foreach (KeyValuePair<string, BalanceData> currencyData in BalancesDict) {
                    if (!BalancesDict[currencyData.Key].isActive) continue;  // don't print if this isn't a valid currency for this platform
                    string currency = currencyData.Key;
                    switch (currency) {  // some cryptos have emojis, let's use them
                        case "XBT": currency = ":xbt:   ";
                            break;
                        case "ETH": currency = ":eth:   ";
                            break;
                        case "USDT": currency = ":usdt:   ";
                            break;
                        case "USDC": currency = ":usdc:   ";
                            break;
                        case "BCH": currency = ":bch:   ";
                            break;
                        case "AUD": currency = ":flag-au:   ";
                            break;
                        case "USD": currency = ":flag-us:   ";
                            break;
                        case "NZD": currency = ":flag-nz:   ";
                            break;
                        case "SGD": currency = ":flag-sg:   ";
                            break;
                        case "DOGE": currency = ":dog:   ";
                            break;
                        case "UNI": currency = ":unicorn_face:   ";
                            break;
                        case "BAT": currency = ":bat:   ";
                            break;
                        case "PMGT": currency = ":golden:   ";
                            break;
                        default:
                            if (currency.Length == 3) currency += " ";
                            break;
                    }

                    if (IRCurrencies_and_extras.Contains(currencyData.Key) && !reachedCryptoYet) {
                        slackString += Environment.NewLine;
                        reachedCryptoYet = true;
                    }

                    slackString += currency + "  Total balance: " + Utilities.FormatValue(BalancesDict[currencyData.Key].TotalBalance) + "  ";

                    TXTString += currencyData.Key + "\tTotal balance: " + Utilities.FormatValue(BalancesDict[currencyData.Key].TotalBalance) + "\t|\t" +
                        "Loan: " + Utilities.FormatValue(BalancesDict[currencyData.Key].Loan) + "\t|\t" +
                        "Slush: " + Utilities.FormatValue(BalancesDict[currencyData.Key].Slush) + "\t|\t" +
                        "Out by: " + Utilities.FormatValue(BalancesDict[currencyData.Key].OutBy);

                    Color OutByAlertColour = DetermineOutByColour(currencyData.Key, BalancesDict[currencyData.Key].OutBy, BalancesDict[currencyData.Key].Slush);

                    if (OutByAlertColour == Color.Black) slackString += "\t:ok:";
                    else if (OutByAlertColour == Color.Green) slackString += "\t:white_check_mark:";
                    else if (OutByAlertColour == Color.Red) slackString += "\t:exclamation:";
                    else if (OutByAlertColour == Color.Purple) slackString += "\t:male-detective:";
                    else if (OutByAlertColour == Color.DarkBlue) slackString += "\t:man-shrugging:";
                    else slackString += " :warning:";

                    if (OutByAlertColour == Color.Black) TXTString += "\tOK";
                    else if (OutByAlertColour == Color.Green) TXTString += "\tPerfect";
                    else if (OutByAlertColour == Color.Red) TXTString += "\t!! Warning";
                    else if (OutByAlertColour == Color.Purple) TXTString += "\tSlush needs topping up";
                    else if (OutByAlertColour == Color.DarkBlue) TXTString += "\tUnsupported token";
                    else TXTString += "\tUnknown?";

                    slackString += Environment.NewLine;
                    TXTString += Environment.NewLine;
                }
                //Debug.Print("copy for slack: " + slackString);
                Clipboard.SetText(slackString);
                TXTString += Environment.NewLine + Environment.NewLine;
                return new Tuple<string, string>(slackString, TXTString);
            }
            return new Tuple<string, string>("", "");
        }

        private Color DetermineOutByColour(string currency, decimal OutBy, decimal slush) {
            if (null != CryptoPairs) {

                // some cryptos (eg wrapped cryptos) will have the same price as their unwrapped counterparts, or close enough for our purposes.  So "unwrap" them for this sub
                string pairCurrency = currency;
                if (pairCurrency == "WETH") pairCurrency = "ETH";
                if (pairCurrency == "WBTC") pairCurrency = "XBT";
                string pair = pairCurrency + "-" + DCE_IR.CurrentSecondaryCurrency;
                
                bool isCrypto = IRCurrencies_and_extras.Contains(currency);

                decimal value = 0;
                if (CryptoPairs.ContainsKey(pair)) {
                    value = OutBy * ((CryptoPairs[pair].CurrentLowestOfferPrice + CryptoPairs[pair].CurrentHighestBidPrice) / 2);
                }
                else if (!isCrypto) value = OutBy;
                else {
                    if (OutBy == 0) return Color.Green;  // even if we don't know the value of the token, if we're out by 0, then we're all good.
                    else return Color.DarkBlue;
                }

                if (isCrypto && (slush > 0) && (OutBy < 0)) {  // for crypto we're OK if there's slush and the crypto outBy is negative up to 50% of the slush - this is expected due to withdrawal fees
                    if (OutBy < (slush * -1)) return Color.Red;
                    else if ((slush + OutBy) < (slush / 2)) {  // (slush + OutBy) is the amount we've eaten into the slush by. is it less than half the slush? if so, warn as slush is getting low
                    //else if (OutBy < (slush * -0.5M)) {
                        return Color.Purple;  // be alert, not alarmed
                    }
                    else return Color.Black;  // we're down a bit, but have more than half the slush still, so just chill.  no need for red/purple
                }
                else {  // it's not crypto, or slush is 0
                    if (value == 0) return Color.Green;
                    else if ((value > 1) || (value < -1)) return Color.Red;
                    else return Color.Black;
                }
            }
            return Color.Orange;  // error
        }

        private async void DrawCoinbase() {
            //CoinbaseClient cbc = new CoinbaseClient();  // static class now

            if (string.IsNullOrEmpty(Properties.Settings.Default.CoinbaseAPIKey) || 
                string.IsNullOrEmpty(Properties.Settings.Default.CoinbaseAPISecret) || 
                string.IsNullOrEmpty(Properties.Settings.Default.CoinbasePassPhrase)) {

                balSetting_form = new BalSettings();
                balSetting_form.Show();
                Platform_comboBox.SelectedIndex = 0;
                Platform_comboBox_SelectedIndexChanged(null, null);  // revert back to IR
                return;
            }

            Task<string> cbResponseTask = CoinbaseClient.CB_get_accounts();

            if (!masterBalanceDict.ContainsKey("Coinbase"))
                masterBalanceDict.Add("Coinbase", new Dictionary<string, BalanceData>());

            masterBalanceDict["Coinbase"].Clear();

            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(IRCurrencies_and_extras);
            LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded_Coinbase, "Coinbase");

            string CoinbaseRes = await cbResponseTask;

            // now to parse the response.
            if (string.IsNullOrEmpty(CoinbaseRes)) {
                TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull Coinbase data";
            }
            else {
                Dictionary<string, Account> CoinbaseBalances = ParseCoinbaseResponse(CoinbaseRes);
                if (null == CoinbaseBalances) TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull Coinbase data";
                else {
                    DrawDynamicRows(DCE_IR.SecondaryCurrencyList, CoinbaseBalances, false, "Coinbase");
                    DrawDynamicRows(IRCurrencies_and_extras, CoinbaseBalances, true, "Coinbase");
                }
            }
        }

        /// <summary>
        /// response should look like this:
        /// [{"id":"8a650510-0c3e-4fb5-8b22-e59ce34b043f","currency":"1INCH","balance":"0.0000000000000000","hold":"0.0000000000000000","available":"0","profile_id":"c0c22bcb-83ef-4b48-a56b-8d59b9a83e47","trading_enabled":true},
        /// {"id":"c4d92d4e-4063-40c2-880e-4ac8b4959a27","currency":"AAVE","balance":"0.0000000000000000","hold":"0.0000000000000000","available":"0","profile_id":"c0c22bcb-83ef-4b48-a56b-8d59b9a83e47","trading_enabled":true},
        /// </summary>
        /// <param name="response"></param>
        private Dictionary<string, Account> ParseCoinbaseResponse(string response) {

            Dictionary<string, Account> CoinbaseBalances = new Dictionary<string, Account>();
            List<CoinbaseAccountResponse> responseJson;
            try {
                responseJson = JsonConvert.DeserializeObject<List<CoinbaseAccountResponse>>(response);
            }
            catch (Exception ex) {
                Debug.Print("Failed to parse Coinbase web response: " + ex.Message);
                Debug.Print("Response: " + response);
                return null;
            }

            foreach (CoinbaseAccountResponse currency in responseJson) {
                string curr = currency.currency;
                if (curr == "BTC") curr = "XBT";
                if (DCE_IR.SecondaryCurrencyList.Contains(curr) || IRCurrencies_and_extras.Contains(curr)) {
                    if (decimal.TryParse(currency.balance, out decimal balance)) {

                       Account tempAccount = new Account();
                        if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(curr.ToLower()), out CurrencyCode IRCurrency)) {
                            tempAccount.CurrencyCode = IRCurrency;
                        }
                        else Debug.Print("Could not parse " + curr + " in Coinbase to an IR currency");
                        if (decimal.TryParse(currency.available, out decimal parseAvailable)) {
                            tempAccount.AvailableBalance = parseAvailable;
                        }
                        else Debug.Print("Could not parse " + currency.available + " in Coinbase to Available balance decimal");
                        if (decimal.TryParse(currency.balance, out decimal parseBalance)) {
                            tempAccount.TotalBalance = parseBalance;
                        }
                        else Debug.Print("Could not parse " + currency.balance + " in Coinbase to (total) balance decimal");
                        tempAccount.AccountGuid = new Guid(currency.id);
                        if (currency.trading_enabled) tempAccount.AccountStatus = AccountStatus.Active;
                        else tempAccount.AccountStatus = AccountStatus.Inactive;

                        CoinbaseBalances.Add(curr, tempAccount);
                    }
                }
            }
            return CoinbaseBalances;
        }

        private async void DrawMetaMask() {
            string platformName = "IROTC MetaMask";
            if (string.IsNullOrEmpty(Properties.Settings.Default.ETHWalletAddress)) {
                balSetting_form = new BalSettings();
                balSetting_form.Show();
                Platform_comboBox.SelectedIndex = 0;
                Platform_comboBox_SelectedIndexChanged(null, null);  // revert back to IR
                return;
            }
            string uri = "https://api.ethplorer.io/getAddressInfo/" + Properties.Settings.Default.ETHWalletAddress + "?apiKey=freekey";
            Task<string> responseTask = Utilities.GetWebData(uri);

            if (!masterBalanceDict.ContainsKey(platformName))
                masterBalanceDict.Add(platformName, new Dictionary<string, BalanceData>());

            masterBalanceDict[platformName].Clear();

            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(IRCurrencies_and_extras);
            LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded_IROTCMetaMask, platformName);

            string MMRes = await responseTask;

            // now to parse the response.
            if (string.IsNullOrEmpty(MMRes)) {
                TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull ETH wallet data";
            }
            else {
                Dictionary<string, Account> ETHBalances = ParseETHWalletResponse(MMRes);
                if (null == ETHBalances) TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull ETH wallet data";
                else {
                    DrawDynamicRows(DCE_IR.SecondaryCurrencyList, ETHBalances, false, platformName);
                    DrawDynamicRows(IRCurrencies_and_extras, ETHBalances, true, platformName);
                }
            }

        }

        private Dictionary<string, Account> ParseETHWalletResponse(string response) {

            Dictionary<string, Account> ETHBalances = new Dictionary<string, Account>();
            ETHWallet jsonETHBalances;
            try {
                jsonETHBalances = JsonConvert.DeserializeObject<ETHWallet>(response);
            }
            catch (Exception ex) {
                Debug.Print("failed to parse eth wallet data: " + ex.Message);
                return null;
            }

            // first grab the ETH balance
            Account ethAccount = new Account();
            ethAccount.CurrencyCode = CurrencyCode.Eth;
            ethAccount.AccountStatus = AccountStatus.Active;
            ethAccount.TotalBalance = Convert.ToDecimal(jsonETHBalances.ETH.balance);
            ETHBalances.Add("ETH", ethAccount);

            foreach (Token tok in jsonETHBalances.tokens) {
                Account tokAccount = new Account();
                if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tok.tokenInfo.symbol.ToLower()), out CurrencyCode currencyCode)) {
                    tokAccount.CurrencyCode = currencyCode;
                }
                else Debug.Print("Couldn't parse " + tok.tokenInfo.symbol + " in ETHWallet for " + tok.tokenInfo.symbol);

                if (long.TryParse(tok.tokenInfo.decimals, out long decimalsLong)) {
                    double bal = tok.balance / Math.Pow(10, decimalsLong);
                    tokAccount.TotalBalance = Convert.ToDecimal(bal);
                }
                else Debug.Print("Couldn't parse " + tok.tokenInfo.decimals + " in ETHWallet for " + tok.tokenInfo.symbol);

                tokAccount.AccountStatus = AccountStatus.Active;
                if (null != tok.tokenInfo.symbol) {
                    if (!ETHBalances.ContainsKey(tok.tokenInfo.symbol)) {
                        ETHBalances.Add(tok.tokenInfo.symbol, tokAccount);
                    }
                    else {
                        Debug.Print("hmm looks like we have 2 tokens with the same symbol in this ETH wallet: " + tok.tokenInfo.symbol + ".  Will ignore duplicates");
                    }
                }
            }

            return ETHBalances;
        }

        private async void BalCopyForSlack_button_Click(object sender, EventArgs e) {
            Tuple<string, string> txtTuple = GenerateBalanceStrings(Platform_comboBox.SelectedItem.ToString());
            string platform = Platform_comboBox.SelectedItem.ToString();
            string platformURLEncoded = (Platform_comboBox.SelectedItem.ToString()).Replace(" ", "%20");

            if (string.IsNullOrEmpty(Properties.Settings.Default.SlackBotToken) ||
                string.IsNullOrEmpty(Properties.Settings.Default.SlackBotChannel)) {
                balSetting_form = new BalSettings();
                balSetting_form.Show();
                return;
            }

            //Slack slack = new Slack();

            if (LastSlackThread.Date < DateTime.Now.Date) {  // start a new thread
                var ParentMessage = new Slack.SlackMessage {
                    channel = Properties.Settings.Default.SlackBotChannel,
                    text = "Balance check :thread:",
                    icon_url = "https://s3-ap-southeast-2.amazonaws.com/independentreserve/media/IRTicker/IRTicker-avatar2.png"
                };
                Slack.SlackMessageResponse SlackResponse = await Slack.SendMessageAsync(Properties.Settings.Default.SlackBotToken, ParentMessage, "https://slack.com/api/chat.postMessage");
                if (null == SlackResponse) {
                    Debug.Print("Failed to send Slack thread message");
                    return;
                }
                SlackMessageTS = SlackResponse.ts;
                LastSlackThread = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(SlackMessageTS)) {
                var smsg = new Slack.SlackMessage {
                    channel = Properties.Settings.Default.SlackBotChannel,
                    text = txtTuple.Item1,
                    icon_url = "https://s3-ap-southeast-2.amazonaws.com/independentreserve/media/IRTicker/" + platformURLEncoded + ".png",
                    thread_ts = SlackMessageTS
                };
                await Slack.SendMessageAsync(Properties.Settings.Default.SlackBotToken, smsg, "https://slack.com/api/chat.postMessage");
            }

            DialogResult SaveToGDriveRes = MessageBox.Show("Save results to G drive?  This will overwrite any previously saved data for today" + Environment.NewLine + Environment.NewLine +
                Properties.Settings.Default.GDriveFolder_BalSettings, "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (SaveToGDriveRes == DialogResult.Yes) {

                if (string.IsNullOrEmpty(Properties.Settings.Default.GDriveFolder_BalSettings)) {  // make sure we have GDrive settings

                    balSetting_form = new BalSettings();
                    balSetting_form.Show();
                    return;
                }

                string Filename = Properties.Settings.Default.GDriveFolder_BalSettings + "\\OTC-balances-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                // need to open the file, find out if this platform has been written yet.  If not we append, if it has, we start again?
                // might be good to have a "check file" button that checks to make sure all platforms have bee written

                if (File.Exists(Filename)) {
                    string readText = File.ReadAllText(Filename);
                    if (readText.Contains(platform)) {  // Today's file already has this platform, so we start again, even if it means overwriting all other platform results
                        try {
                            File.WriteAllText(Filename, txtTuple.Item2);
                        }
                        catch (Exception ex) {
                            Debug.Print("Couldn't write file.. " + ex.Message);
                            MessageBox.Show("Failed to write gdrive file to: " + Filename);
                        }
                    }
                    else {  // it doesn't contain this platfrom, so we append
                        try {
                            File.AppendAllText(Filename, txtTuple.Item2, Encoding.UTF8);  // this doesn't append, it just overwrites??
                        }
                        catch (Exception ex) {
                            Debug.Print("Couldn't append file.. " + ex.Message);
                            MessageBox.Show("Failed to append to gdrive file: " + Filename);
                        }
                    }
                }
                else {  // else file not there, so let's create it
                    try {
                        File.WriteAllText(Filename, txtTuple.Item2);
                    }
                    catch (Exception ex) {
                        Debug.Print("Couldn't write file.. " + ex.Message);
                        MessageBox.Show("Failed to write gdrive file to: " + Filename);
                    }
                }
            }
        }

        private async void Platform_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            // platform hub
            CryptoPairs = await GetCryptoPairs();
            switch (Platform_comboBox.SelectedItem.ToString()) {
                case "Independent Reserve IROTC":
                    SaveLoanSlush = false;
                    DrawIR(Platform.IROTC);
                    SaveLoanSlush = true;
                    break;

                case "Independent Reserve IROTCSG":
                    SaveLoanSlush = false;
                    DrawIR(Platform.IROTCSG);
                    SaveLoanSlush = true;
                    break;

                case "B2C2":
                    SaveLoanSlush = false;
                    DrawB2C2();
                    SaveLoanSlush = true;
                    break;

                case "Coinbase":
                    SaveLoanSlush = false;
                    DrawCoinbase();
                    SaveLoanSlush = true;
                    break;

                case "TrigonX":
                    SaveLoanSlush = false;
                    DrawTrigonX();
                    SaveLoanSlush = true;
                    break;

                case "IROTC MetaMask":
                    SaveLoanSlush = false;
                    DrawMetaMask();
                    SaveLoanSlush = true;
                    break;
            }
        }

        private class BalanceData
        {
            public string Currency { get; set; }
            public decimal TotalBalance { get; set; }
            public decimal AvailableBalance { get; set; }
            public string LoanStr { get; set; }  // record as string when we pull out of the Settings object, convert to decimal later
            public string SlushStr { get; set; }
            public decimal OutBy { get; set; }
            public decimal Loan { get; set; }
            public decimal Slush { get; set; }
            public bool isActive { get; set; }
        }
        private class CoinbaseAccountResponse
        {
            public string id { get; set; }
            public string currency { get; set; }
            public string balance { get; set; }
            public string hold { get; set; }
            public string available { get; set; }
            public string profile_id { get; set; }
            public bool trading_enabled { get; set; }
        }

        public class Price
        {
            public double rate { get; set; }
            public double diff { get; set; }
            public double diff7d { get; set; }
            public int ts { get; set; }
            public double marketCapUsd { get; set; }
            public double availableSupply { get; set; }
            public double volume24h { get; set; }
            public double diff30d { get; set; }
            public double volDiff1 { get; set; }
            public double volDiff7 { get; set; }
            public double volDiff30 { get; set; }
            //public string currency { get; set; }
        }

        public class ETH
        {
            public Price price { get; set; }
            public double balance { get; set; }
            public string rawBalance { get; set; }
        }

        public class TokenInfo
        {
            public string address { get; set; }
            public string name { get; set; }
            public string decimals { get; set; }
            public string symbol { get; set; }
            public string totalSupply { get; set; }
            public string owner { get; set; }
            public int lastUpdated { get; set; }
            public int slot { get; set; }
            public int issuancesCount { get; set; }
            public int holdersCount { get; set; }
            public string image { get; set; }
            public string website { get; set; }
            public string telegram { get; set; }
            public string twitter { get; set; }
            public string coingecko { get; set; }
            public int ethTransfersCount { get; set; }
            public object price { get; set; }
            public List<string> publicTags { get; set; }
            public string facebook { get; set; }
            public string reddit { get; set; }
            public string description { get; set; }
            public string links { get; set; }
            public string storageTotalSupply { get; set; }
        }

        public class JsonExponentialConverter : JsonConverter
        {
            public override bool CanRead { get { return true; } }
            public override bool CanConvert(Type objectType) {
                return true;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                serializer.Serialize(writer, value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                long amount = 0;
                if (long.TryParse(reader.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount)) {
                    return amount;
                }
                return amount;
            }
        }

        public class Token
        {
            public TokenInfo tokenInfo { get; set; }
            [JsonConverter(typeof(JsonExponentialConverter))]
            public long balance { get; set; }
            public int totalIn { get; set; }
            public int totalOut { get; set; }
            public string rawBalance { get; set; }
        }

        public class ETHWallet
        {
            public string address { get; set; }
            public ETH ETH { get; set; }
            public int countTxs { get; set; }
            public List<Token> tokens { get; set; }
        }

        public class Pair
        {
            public int id { get; set; }
            public string slug { get; set; }
        }

        public class Balances
        {
            public double ADA { get; set; }
            public double AUD { get; set; }
            public double BTC { get; set; }
            public double DOT { get; set; }
            public double EOS { get; set; }
            public double ETH { get; set; }
            public double LINK { get; set; }
            public double LTC { get; set; }
            public double USD { get; set; }
            public double USDC { get; set; }
            public double USDT { get; set; }
            public double XRP { get; set; }

            [JsonProperty("Total Equity")]
            public double TotalEquity { get; set; }

            [JsonProperty("Credit Used")]
            public double CreditUsed { get; set; }

            [JsonProperty("Credit Limit")]
            public double CreditLimit { get; set; }
        }

        public class TrigonXResponse
        {
            public List<Pair> pairs { get; set; }
            public string credit_limit { get; set; }
            public string credit_used { get; set; }
            public string discretion_bps { get; set; }
            public Dictionary<string, double> balances { get; set; }
            //public Balances balances { get; set; }
            public string name { get; set; }
            public object credit_limit_since { get; set; }
            public object customer_template { get; set; }
            public string internal_account { get; set; }
        }

        private void BalSettings_button_Click(object sender, EventArgs e) {
            balSetting_form = new BalSettings();
            balSetting_form.Show();
        }
    }

    /*public class JsonExponentialConverter : JsonConverter
    {
        public override bool CanRead { get { return true; } }
        public override bool CanConvert(Type objectType) {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            long amount = 0;
            if (long.TryParse(reader.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount)) {
                return amount;
            }
            return amount;
        }
    }*/
}
