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


namespace IRTicker
{
    public partial class Balance : Form
    {
        DCE DCE_IR;
        PrivateIR pIR;
        Dictionary<string, IndependentReserve.DotNetClientApi.Data.Account> pAccounts;
        Dictionary<string, TextBox> LoanDict = new Dictionary<string, TextBox>();
        Dictionary<string, TextBox> SlushDict = new Dictionary<string, TextBox>();
        Dictionary<string, Label> TotalBalDict = new Dictionary<string, Label>();
        Dictionary<string, Label> AvailsBalDict = new Dictionary<string, Label>();
        Dictionary<string, Label> OutByDict = new Dictionary<string, Label>();

        bool SaveLoanSlush = true;  // false if we're clearing stuff and don't want to make changes to the saved entries.  Taking bets on me having to remove this auto-save nonsense and build a save button again

        public Balance(DCE _dce_IR, PrivateIR _pIR) {
            InitializeComponent();

            DCE_IR = _dce_IR;
            pIR = _pIR;

            Platform_comboBox.SelectedIndex = 0;  // choose IR

            BuildUI();
        }

        private async Task pullAccounts() {
            // pull the balance
             pAccounts = pIR.GetAccounts();
        }

        // should only be called once!
        private void BuildUI() {

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


        private void DrawDynamicRows(List<string> Currencies, Dictionary<string, Tuple<string, string>> LoanSlushDict, bool crypto) {

            foreach (string curr in Currencies) {
                if (pAccounts.ContainsKey(curr)) {
                    if (TotalBalDict.ContainsKey(curr)) {
                        TotalBalDict[curr].Text = Utilities.FormatValue(pAccounts[curr].TotalBalance, 8, false);
                    }
                    if (AvailsBalDict.ContainsKey(curr)) {
                        AvailsBalDict[curr].Text = Utilities.FormatValue(pAccounts[curr].AvailableBalance, 8, false);
                    }
                }

                if (LoanSlushDict.ContainsKey(curr)) {
                    if (LoanDict.ContainsKey(curr)) {
                        LoanDict[curr].Text = LoanSlushDict[curr].Item1;
                    }
                    if (SlushDict.ContainsKey(curr)) {
                        SlushDict[curr].Text = LoanSlushDict[curr].Item2;
                    }
                }

                if (OutByDict.ContainsKey(curr)) {

                    decimal LoanDec = 0;
                    decimal SlushDec = 0;
                    if (decimal.TryParse(LoanDict[curr].Text, out decimal _LoanDec)) LoanDec = _LoanDec;
                    if (decimal.TryParse(SlushDict[curr].Text, out decimal _SlushDec)) SlushDec = _SlushDec;
                    decimal outby = pAccounts[curr].TotalBalance - LoanDec - SlushDec;

                    // colour the out by text to give a quick idea where we're at
                    if (outby > (LoanDec + SlushDec) * 0.01M) OutByDict[curr].ForeColor = Color.Red;  // if outby is greater than 1% of the loan+slush, then we alert
                    else if (outby == 0) OutByDict[curr].ForeColor = Color.Green;
                    else if (outby < ((0 - (LoanDec + SlushDec)) * 0.01M)) {  // if the outby is less than 1% of the loan+slush...
                        if (crypto && (LoanDec == 0) && outby < ((0 - SlushDec) * 0.5M)) OutByDict[curr].ForeColor = Color.Purple;  // if there's no loan, and this is a crypto, then we can highlight when we think slush should be topped up (eg when slush in < 50% what I have specified it should be
                        else OutByDict[curr].ForeColor = Color.Red;  // if it's not slikely to be just a slush running low issue (which happens naturally because we lose crypto when the system charges us for withdrawal fees), then let's alert
                    }
                    else OutByDict[curr].ForeColor = Color.Black;
                    OutByDict[curr].Text = Utilities.FormatValue(outby);
                }
            }
        }

        /// <summary>
        /// Will return something like this on a good day:
        /// 
        /// {"ADA":"0","USD":"0.0045115155","ETH":"0","UST":"-0.0068","XRP":"0.000001","AUD":"-0.006411","BTC":"0","BCH":"0","DOT":"0","BNB":"0","CAD":"0","CHF":"0","CNH":"0","DOG":"0","EOS":"0","ETC":"0","EUR":"0","GBP":"0","ICP":"0","JPY":"0","KSM":"0","LNK":"0","LTC":"0","MXN":"0","NZD":"0","SGD":"0","TRX":"0","UNI":"0","USC":"0","XAU":"0","XLM":"0","XMR":"0","XTZ":"0","ZEC":"0"}
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetB2C2() {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.b2c2.net/balance/");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = "IRTicker";
            request.Headers["Authorization"] = "Token 90a3c5c102c7faa91a66a7ee18433f8270a87e7d";

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
            Task<string> res = GetB2C2();
            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(DCE_IR.PrimaryCurrencyList);
            Dictionary<string, Tuple<string, string>> LoanSlushDict = LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded_B2C2);

            string B2C2res = await res;
            Dictionary<string, decimal> B2C2Balances = parseB2C2Response(B2C2res);
            DrawDynamicRows_B2C2(DCE_IR.SecondaryCurrencyList, B2C2Balances, LoanSlushDict, false);
            DrawDynamicRows_B2C2(DCE_IR.PrimaryCurrencyList, B2C2Balances, LoanSlushDict, true);
        }

        private void DrawDynamicRows_B2C2(List<string> Currencies, Dictionary<string, decimal> B2C2Balances, Dictionary<string, Tuple<string, string>> LoanSlushDict, bool crypto) {
            foreach (string curr in Currencies) {
                if (B2C2Balances.ContainsKey(curr)) {
                    if (TotalBalDict.ContainsKey(curr)) {
                        TotalBalDict[curr].Text = Utilities.FormatValue(B2C2Balances[curr], 8, false);
                    }
                }

                if (LoanSlushDict.ContainsKey(curr)) {
                    if (LoanDict.ContainsKey(curr)) {
                        LoanDict[curr].Text = LoanSlushDict[curr].Item1;
                    }
                    if (SlushDict.ContainsKey(curr)) {
                        SlushDict[curr].Text = LoanSlushDict[curr].Item2;
                    }
                }

                if (OutByDict.ContainsKey(curr) && B2C2Balances.ContainsKey(curr)) {

                    decimal LoanDec = 0;
                    decimal SlushDec = 0;
                    if (decimal.TryParse(LoanDict[curr].Text, out decimal _LoanDec)) LoanDec = _LoanDec;
                    if (decimal.TryParse(SlushDict[curr].Text, out decimal _SlushDec)) SlushDec = _SlushDec;
                    decimal outby = B2C2Balances[curr] - LoanDec - SlushDec;

                    // colour the out by text to give a quick idea where we're at
                    if (outby > (LoanDec + SlushDec) * 0.01M) OutByDict[curr].ForeColor = Color.Red;  // if outby is greater than 1% of the loan+slush, then we alert
                    else if (outby == 0) OutByDict[curr].ForeColor = Color.Green;
                    else if (outby < ((0 - (LoanDec + SlushDec)) * 0.01M)) {  // if the outby is less than 1% of the loan+slush...
                        OutByDict[curr].ForeColor = Color.Red;  // if it's not slikely to be just a slush running low issue (which happens naturally because we lose crypto when the system charges us for withdrawal fees), then let's alert
                    }
                    else OutByDict[curr].ForeColor = Color.Black;

                    if (!crypto && ((outby < 0.02M) && (outby > -0.02M))) OutByDict[curr].ForeColor = Color.Black;  // within 2c is fine

                    OutByDict[curr].Text = Utilities.FormatValue(outby);
                }
            }
        }

        // parsing something like this:
        // {"ADA":"0","USD":"0.0045115155","ETH":"0","UST":"-0.0068","XRP":"0.000001","AUD":"-0.006411","BTC":"0","BCH":"0","DOT":"0","BNB":"0","CAD":"0","CHF":"0","CNH":"0","DOG":"0","EOS":"0","ETC":"0","EUR":"0","GBP":"0","ICP":"0","JPY":"0","KSM":"0","LNK":"0","LTC":"0","MXN":"0","NZD":"0","SGD":"0","TRX":"0","UNI":"0","USC":"0","XAU":"0","XLM":"0","XMR":"0","XTZ":"0","ZEC":"0"}
        private Dictionary<string, decimal> parseB2C2Response(string resp) {
            resp = resp.Replace("{", "");
            resp = resp.Replace("}", "");
            resp = resp.Replace("\"", "");
            resp = resp.Trim();

            Dictionary<string, decimal> B2C2Balances = new Dictionary<string, decimal>();

            string[] respArray = resp.Split(',');

            foreach (string currency in respArray) {
                string[] currPair = currency.Split(':');
                string normalisedCurrency = currPair[0];
                if (normalisedCurrency == "BTC") normalisedCurrency = "XBT";
                else if (normalisedCurrency == "UST") normalisedCurrency = "USDT";
                else if (normalisedCurrency == "DOG") normalisedCurrency = "DOGE";
                else if (normalisedCurrency == "LNK") normalisedCurrency = "LINK";
                else if (normalisedCurrency == "USC") normalisedCurrency = "USDC";

                if (DCE_IR.SecondaryCurrencyList.Contains(normalisedCurrency) || DCE_IR.PrimaryCurrencyList.Contains(normalisedCurrency)) {
                    if (decimal.TryParse(currPair[1], out decimal balance)) {
                        B2C2Balances.Add(normalisedCurrency, balance);
                    }
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

        private async void DrawIR() {
            // let's draw some controls

            Task meeTask = pullAccounts();
            ClearDynamicRows(DCE_IR.SecondaryCurrencyList);
            ClearDynamicRows(DCE_IR.PrimaryCurrencyList);
            // populate numbas
            Dictionary<string, Tuple<string, string>> LoanSlushDict = LoanSlushDecode(Properties.Settings.Default.LoanSlushEncoded);

            await meeTask;

            DrawDynamicRows(DCE_IR.SecondaryCurrencyList, LoanSlushDict, false);
            DrawDynamicRows(DCE_IR.PrimaryCurrencyList, LoanSlushDict, true);
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
            }

            Properties.Settings.Default.Save();
        }

        // takes the input, expecting in a format:
        // <crypto>;<loan value>;<slush value>?...  and repeats
        // will decode this and populate the loan and slush text boxes with the data
        private Dictionary<string, Tuple<string, string>> LoanSlushDecode(string EncodedLoanSlush) {

            Dictionary<string, Tuple<string, string>> result = new Dictionary<string, Tuple<string, string>>();

            string[] loanSlushCurrencies = EncodedLoanSlush.Split('?');
            if (loanSlushCurrencies.Length < 1) return result;
            foreach (string loanSlushCurrency in loanSlushCurrencies) {
                string[] currencySplit = loanSlushCurrency.Split(';');
                if (currencySplit.Length != 3) continue;

                // if it's a legit currency, and we have loan and slush textbox controls for it, then...
                if ((DCE_IR.PrimaryCurrencyList.Contains(currencySplit[0]) || DCE_IR.SecondaryCurrencyList.Contains(currencySplit[0])) && 
                    LoanDict.ContainsKey(currencySplit[0]) && SlushDict.ContainsKey(currencySplit[0])) {

                    result.Add(currencySplit[0], new Tuple<string, string>(currencySplit[1], currencySplit[2]));
                }
            }
            return result;
        }

        private void BalReload_button_Click(object sender, EventArgs e) {
            Platform_comboBox_SelectedIndexChanged(null, null);
        }

        // copies to clipboard some text that can be pasted into slack
        private void CopyForSlack() {

        }

        private void Platform_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            // platform hub

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
            }
        }
    }
}
