﻿using System;
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


namespace IRTicker
{
    public partial class Balance : Form
    {
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

        bool SaveLoanSlush = true;  // false if we're clearing stuff and don't want to make changes to the saved entries.  Taking bets on me having to remove this auto-save nonsense and build a save button again

        BalSettings balSetting_form;

        public Balance(DCE _dce_IR, PrivateIR _pIR) {
            InitializeComponent();

            DCE_IR = _dce_IR;
            pIR = _pIR;

            Platform_comboBox.SelectedIndex = 0;  // choose IR

            BuildUI();
        }

        private async Task<Dictionary<string, Account>> pullAccounts() {
            // pull the balance
             return pIR.GetAccounts();
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
            buildDynamicRows(DCE_IR.PrimaryCurrencyList, RowCount);

            DrawIR();  // always draw IR first, 
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

        // this sub assumes that the masterBalanceDict[][].loan and .slush are populated - eg the LoanSlushDecode() sub has been called
        /*private void DrawDynamicRows_IR(List<string> Currencies, bool crypto, string platformName) {

            foreach (string curr in Currencies) {
                if (!masterBalanceDict[platformName].ContainsKey(curr))
                    masterBalanceDict[platformName].Add(curr, new BalanceData() { isActive = true } );  // at this point we should already have the loan and slush data in here
                else masterBalanceDict[platformName][curr].isActive = true;  // all currencies are active for IR, just set this to true

                if (pAccounts.ContainsKey(curr)) {
                    if (TotalBalDict.ContainsKey(curr)) {
                        TotalBalDict[curr].Text = Utilities.FormatValue(pAccounts[curr].TotalBalance, 8, false);
                        masterBalanceDict[platformName][curr].TotalBalance = pAccounts[curr].TotalBalance;
                    }
                    if (AvailsBalDict.ContainsKey(curr)) {
                        AvailsBalDict[curr].Text = Utilities.FormatValue(pAccounts[curr].AvailableBalance, 8, false);
                        masterBalanceDict[platformName][curr].AvailableBalance = pAccounts[curr].AvailableBalance;
                    }
                }

                if (masterBalanceDict[platformName].ContainsKey(curr)) {
                    if (LoanDict.ContainsKey(curr)) {
                        LoanDict[curr].Text = masterBalanceDict[platformName][curr].LoanStr;
                    }
                    if (SlushDict.ContainsKey(curr)) {
                        SlushDict[curr].Text = masterBalanceDict[platformName][curr].SlushStr;
                    }
                }

                if (OutByDict.ContainsKey(curr)) {

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
                    decimal outby = pAccounts[curr].TotalBalance - LoanDec - SlushDec;

                    // colour the out by text to give a quick idea where we're at
                    OutByDict[curr].ForeColor = DetermineOutByColour(curr, outby, SlushDec);
                    
                    OutByDict[curr].Text = Utilities.FormatValue(outby);
                    masterBalanceDict[platformName][curr].OutBy = outby;
                }
            }
        }*/

        /// <summary>
        /// Will return something like this on a good day:
        /// 
        /// {"ADA":"0","USD":"0.0045115155","ETH":"0","UST":"-0.0068","XRP":"0.000001","AUD":"-0.006411","BTC":"0","BCH":"0","DOT":"0","BNB":"0","CAD":"0","CHF":"0","CNH":"0","DOG":"0","EOS":"0","ETC":"0","EUR":"0","GBP":"0","ICP":"0","JPY":"0","KSM":"0","LNK":"0","LTC":"0","MXN":"0","NZD":"0","SGD":"0","TRX":"0","UNI":"0","USC":"0","XAU":"0","XLM":"0","XMR":"0","XTZ":"0","ZEC":"0"}
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetB2C2(string token) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.b2c2.net/balance/");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = "IRTicker";
            request.Headers["Authorization"] = "Token " + token;

            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) {
                    string result = reader.ReadToEnd();

                    return result;
                }
            }
            catch (WebException e) {
                string returnStr = "";

                if (e.Response != null) {
                    using (WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Debug.Print("Fail for: " + uri);
                        Debug.Print("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data)) {
                            returnStr = reader.ReadToEnd();
                            Debug.Print(returnStr);
                            returnStr = httpResponse.StatusCode.ToString();
                        }
                    }
                }
                //MessageBox.Show("Error connecting to URL: " + uri, "Network error", MessageBoxButtons.OK);
                //return new Tuple<bool, string>(false, returnStr);
            }
            catch (Exception e) {
                Debug.Print(DateTime.Now + " -- GET FAILED! exception: " + e.Message);
                //return new Tuple<bool, string>(false, e.Message);
            }
            return "";
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
            Task<string> res = GetB2C2(Properties.Settings.Default.B2C2Token);

            if (!masterBalanceDict.ContainsKey(platformName))
                masterBalanceDict.Add(platformName, new Dictionary<string, BalanceData>());

            masterBalanceDict[platformName].Clear();

            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(DCE_IR.PrimaryCurrencyList);
            LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded_B2C2, platformName);

            string B2C2res = await res;
            if (string.IsNullOrEmpty(B2C2res)) {
                TotalBalDict.FirstOrDefault().Value.Text = "Failed to pull B2C2 data";
            }
            else {
                Dictionary<string, Account> B2C2Balances = parseB2C2Response(B2C2res);
                DrawDynamicRows_NonIR(DCE_IR.SecondaryCurrencyList, B2C2Balances, false, platformName);
                DrawDynamicRows_NonIR(DCE_IR.PrimaryCurrencyList, B2C2Balances, true, platformName);
            }
        }


        private void DrawDynamicRows_NonIR(List<string> Currencies, Dictionary<string, Account> Balances, bool crypto, string platformName) {
            foreach (string curr in Currencies) {
                if (!masterBalanceDict[platformName].ContainsKey(curr))
                    masterBalanceDict[platformName].Add(curr, new BalanceData());  // at this point we should already have the loan and slush data in here
                if (Balances.ContainsKey(curr)) {
                    if (TotalBalDict.ContainsKey(curr)) {
                        TotalBalDict[curr].Text = Utilities.FormatValue(Balances[curr].TotalBalance, 8, false);
                        masterBalanceDict[platformName][curr].TotalBalance = Balances[curr].TotalBalance;
                    }
                    if (AvailsBalDict.ContainsKey(curr)) {
                        AvailsBalDict[curr].Text = Utilities.FormatValue(Balances[curr].AvailableBalance, 8, false);
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

                    OutByDict[curr].Text = Utilities.FormatValue(outby);
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

                if (DCE_IR.SecondaryCurrencyList.Contains(normalisedCurrency) || DCE_IR.PrimaryCurrencyList.Contains(normalisedCurrency)) {
                    if (decimal.TryParse(currPair[1], out decimal balance)) {
                        tempAccount.TotalBalance = balance;
                    }
                    else Debug.Print("Cannot parse " + currPair[1] + " in B2C2 for the balance");
                    if (Enum.TryParse(currPair[0], out CurrencyCode currencyEnum)) {
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

        private async void DrawIR() {

            Task<Dictionary<string, Account>> meeTask = pullAccounts();
            string platformName = "Independent Reserve";

            if (!masterBalanceDict.ContainsKey(platformName)) 
                masterBalanceDict.Add(platformName, new Dictionary<string, BalanceData>());

            masterBalanceDict[platformName].Clear();

            // let's draw some controls
            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(DCE_IR.PrimaryCurrencyList);

            // populate numbas
            LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded, platformName);

            Dictionary<string, Account> pAccounts = await meeTask;  // the idea here is we pause until this task is done

            DrawDynamicRows_NonIR(DCE_IR.SecondaryCurrencyList, pAccounts, false, platformName);
            DrawDynamicRows_NonIR(DCE_IR.PrimaryCurrencyList, pAccounts, true, platformName);
        }

        private void LoanSlush_TextChanged(object sender, EventArgs e) {
            if (!SaveLoanSlush) return;  // we're reloading things, don't make changes.
            // now we encode the slush and loan data into a string and save it
            string loanSlushEncoded = "";

            foreach (string fiat in DCE_IR.SecondaryCurrencyList) {
                loanSlushEncoded += fiat + ";" + LoanDict[fiat].Text + ";" + SlushDict[fiat].Text + "?";
            }

            foreach (string crypto in DCE_IR.PrimaryCurrencyList) {
                loanSlushEncoded += crypto + ";" + LoanDict[crypto].Text + ";" + SlushDict[crypto].Text + "?";
            }

            /*if (closing) {
                if (Properties.Settings.Default.LoanSlushEncoded != loanSlushEncoded) {
                    var res = MessageBox.Show("Save your loan and slush values?", "Unsaved changes!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No) return;
                }
            }*/

            switch (Platform_comboBox.SelectedItem.ToString()) {
                case "Independent Reserve":
                    Properties.Settings.Default.LoanSlushEncoded = loanSlushEncoded;
                    break;
                case "B2C2":
                    Properties.Settings.Default.LoanSlushEncoded_B2C2 = loanSlushEncoded;
                    break;
                case "Coinbase":
                    Properties.Settings.Default.LoanSlushEncoded_Coinbase = loanSlushEncoded;
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
                if ((DCE_IR.PrimaryCurrencyList.Contains(currencySplit[0]) || DCE_IR.SecondaryCurrencyList.Contains(currencySplit[0])) && 
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

        // copies to clipboard some text that can be pasted into slack
        // refresh
        private void CopyForSlack(string platformName) {
            if (masterBalanceDict.ContainsKey(platformName)) {

                Dictionary<string, BalanceData> BalancesDict = masterBalanceDict[platformName];
                switch (platformName) {
                    case "Independent Reserve": platformName = ":ir:";
                        break;
                    case "TrigonX": platformName = ":trigon:";
                        break;
                    default:
                        platformName = "`" + platformName + "`";
                        break;
                }
                bool reachedCryptoYet = false;  // hack to let put in an empty line in between cryptos and fiat.  Only will work if fiat is all on the top as we cycle through the dictionary
                string slackString = "*Platform:* " + platformName + Environment.NewLine + Environment.NewLine;
                foreach (KeyValuePair<string, BalanceData> currencyData in BalancesDict) {
                    if (!BalancesDict[currencyData.Key].isActive) continue;  // don't print if this isn't a valid currency for this platform
                    string currency = currencyData.Key;
                    switch (currency) {  // some cryptos have emojis, let's use them
                        case "XBT": currency = ":xbt:";
                            break;
                        case "ETH": currency = ":eth:";
                            break;
                        case "USDT": currency = ":usdt:";
                            break;
                        case "USDC": currency = ":usdc:";
                            break;
                        case "BCH": currency = ":bch:";
                            break;
                        case "AUD": currency = ":flag-au:";
                            break;
                        case "USD": currency = ":flag-us:";
                            break;
                        case "NZD": currency = ":flag-nz:";
                            break;
                        case "SGD": currency = ":flag-sg:";
                            break;
                        case "DOGE": currency = ":dog:";
                            break;
                        case "UNI": currency = ":unicorn_face:";
                            break;
                        case "BAT": currency = ":bat:";
                            break;
                        case "PMGT": currency = ":golden:";
                            break;
                    }

                    if (DCE_IR.PrimaryCurrencyList.Contains(currencyData.Key) && !reachedCryptoYet) {
                        slackString += Environment.NewLine;
                        reachedCryptoYet = true;
                    }

                    slackString += currency + "  Total balance: " + Utilities.FormatValue(BalancesDict[currencyData.Key].TotalBalance) + "  |  " +
                        "Loan: " + Utilities.FormatValue(BalancesDict[currencyData.Key].Loan) + "  |  " +
                        "Slush: " + Utilities.FormatValue(BalancesDict[currencyData.Key].Slush) + "  |  " +
                        "Out by: " + Utilities.FormatValue(BalancesDict[currencyData.Key].OutBy);

                    Color OutByAlertColour = DetermineOutByColour(currencyData.Key, BalancesDict[currencyData.Key].OutBy, BalancesDict[currencyData.Key].Slush);

                    if (OutByAlertColour == Color.Black) slackString += " :ok:";
                    else if (OutByAlertColour == Color.Green) slackString += " :white_tick:";
                    else if (OutByAlertColour == Color.Red) slackString += " :exclamation:";
                    else if (OutByAlertColour == Color.Purple) slackString += " :male-detective:";
                    else slackString += " :warning:";

                    slackString += Environment.NewLine;
                }
                //Debug.Print("copy for slack: " + slackString);
                Clipboard.SetText(slackString);
            }
        }

        private Color DetermineOutByColour(string currency, decimal OutBy, decimal slush) {
            if (null != CryptoPairs) {
                string pair = currency + "-" + DCE_IR.CurrentSecondaryCurrency;
                bool isCrypto = DCE_IR.PrimaryCurrencyList.Contains(currency);

                decimal value = 0;
                if (CryptoPairs.ContainsKey(pair)) {
                    value = OutBy * (CryptoPairs[pair].CurrentLowestOfferPrice - CryptoPairs[pair].CurrentHighestBidPrice);
                }
                else if (!isCrypto) value = OutBy;
                else return Color.Orange;

                if (isCrypto && (slush > 0) && (OutBy < 0)) {  // for crypto we're OK if there's slush and the crypto outBy is negative up to 50% of the slush - this is expected due to withdrawal fees
                    if (OutBy < (slush * -0.5M)) return Color.Purple;  // be alert, not alarmed
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
            CoinbaseClient cbc = new CoinbaseClient();

            if (string.IsNullOrEmpty(Properties.Settings.Default.CoinbaseAPIKey) || 
                string.IsNullOrEmpty(Properties.Settings.Default.CoinbaseAPISecret) || 
                string.IsNullOrEmpty(Properties.Settings.Default.CoinbasePassPhrase)) {

                balSetting_form = new BalSettings();
                balSetting_form.Show();
                Platform_comboBox.SelectedIndex = 0;
                Platform_comboBox_SelectedIndexChanged(null, null);  // revert back to IR
                return;
            }

            Task<string> cbResponseTask = cbc.GetAccounts(Properties.Settings.Default.CoinbaseAPIKey, Properties.Settings.Default.CoinbaseAPISecret, Properties.Settings.Default.CoinbasePassPhrase);

            if (!masterBalanceDict.ContainsKey("Coinbase"))
                masterBalanceDict.Add("Coinbase", new Dictionary<string, BalanceData>());

            masterBalanceDict["Coinbase"].Clear();

            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(DCE_IR.PrimaryCurrencyList);
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
                    DrawDynamicRows_NonIR(DCE_IR.SecondaryCurrencyList, CoinbaseBalances, false, "Coinbase");
                    DrawDynamicRows_NonIR(DCE_IR.PrimaryCurrencyList, CoinbaseBalances, true, "Coinbase");
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
                return null;
            }

            foreach (CoinbaseAccountResponse currency in responseJson) {
                if (DCE_IR.SecondaryCurrencyList.Contains(currency.currency) || DCE_IR.PrimaryCurrencyList.Contains(currency.currency)) {
                    if (decimal.TryParse(currency.balance, out decimal balance)) {

                       Account tempAccount = new Account();
                        if (Enum.TryParse(currency.currency, out CurrencyCode IRCurrency)) {
                            tempAccount.CurrencyCode = IRCurrency;
                        }
                        else Debug.Print("Could not parse " + currency.currency + " in Coinbase to an IR currency");
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

                        CoinbaseBalances.Add(currency.currency, tempAccount);
                    }
                }
            }
            return CoinbaseBalances;
        }


        private async void Platform_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            // platform hub
            CryptoPairs = await GetCryptoPairs();
            switch (Platform_comboBox.SelectedItem.ToString()) {
                case "Independent Reserve":
                    SaveLoanSlush = false;
                    DrawIR();
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


        private void BalCopyForSlack_button_Click(object sender, EventArgs e) {
            CopyForSlack(Platform_comboBox.SelectedItem.ToString());
        }

        private void BalSettings_button_Click(object sender, EventArgs e) {
            balSetting_form = new BalSettings();
            balSetting_form.Show();
        }
    }
}