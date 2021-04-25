using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IndependentReserve.DotNetClientApi.Data;
using System.Diagnostics;

namespace IRTicker
{
    public partial class AccAvgPrice : Form
    {
        private PrivateIR pIR;
        private DCE dce;
        private IRTicker IRT;
        private string AvgPriceResult = "";  // holds the unformatted version of the average price
        private string TotalCryptoDealt = "";  // holds the unformatted version of the total crypto dealt
        private string TotalFiatDealt = "";  // holds the unformatted version of the total fiat dealt

        public AccAvgPrice(DCE _DCE, PrivateIR _pIR, IRTicker _IRT, bool enableAutoUpdate = false, string crypto = "", int direction = 0) {
            InitializeComponent();
            dce = _DCE;
            pIR = _pIR;
            IRT = _IRT;

            // initialise the controls
            Utilities.PopulateCryptoComboBox(dce, AccAvgPrice_Crypto_ComboBox);
            PopulateFiatComboBox();
            AccAvgPrice_Start_DTPicker.Value = DateTime.Now;
            AccAvgPrice_End_DTPicker.Value = DateTime.Now + TimeSpan.FromHours(24);
            AccAvgPrice_BuySell_ComboBox.SelectedIndex = direction;  // 0 = buy, 1 = sell
            AccAvgPrice_AutoUpdate_CheckBox.Checked = enableAutoUpdate;
            AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex = 1;  // auto select "Crypto" - it's the only option i actually use
            if (!string.IsNullOrEmpty(crypto)) {
                if (crypto == "XBT") crypto = "BTC";
                if (AccAvgPrice_Crypto_ComboBox.Items.Contains(crypto)) AccAvgPrice_Crypto_ComboBox.SelectedItem = crypto;
                else Debug.Print("Can't find the crypto to set the crypto combobox: " + crypto);
            }
        }

        // I guess to do this fully properly I should run this every time they choose a crypto to ensure that the crypto-fiat pair exists (ie check it against dce.usablepairs())
        // but i'm almost certain we'll only use this for IR, so let's not worry right now.  Just "statically" create it with the knowledge that every crypto works with every fiat
        private void PopulateFiatComboBox() {
            AccAvgPrice_Fiat_ComboBox.Items.Clear();
            AccAvgPrice_Fiat_ComboBox.ResetText();
            AccAvgPrice_Fiat_ComboBox.Items.Add("");
            AccAvgPrice_Fiat_ComboBox.SelectedIndex = 0;

            int AUDselector = 0;
            int loopCount = 1;  // start at 1 because we add a blank enty above

            foreach (string fiat in dce.SecondaryCurrencyList) {
                AccAvgPrice_Fiat_ComboBox.Items.Add(fiat);
                if (fiat.ToUpper() == "AUD") AUDselector = loopCount;
            }

            if (AccAvgPrice_Fiat_ComboBox.Items.Count < 1) {
                MessageBox.Show("Error - no fiat currencies to select??", "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AccAvgPrice_Go_Button.Enabled = false;
            }
            else {
                AccAvgPrice_Fiat_ComboBox.SelectedIndex = AUDselector;  // make AUD default
            }
        }

        private async void AccAvgPrice_Go_Button_Click(object sender, EventArgs e) {
            if ((AccAvgPrice_Crypto_ComboBox.SelectedIndex == 0) || (AccAvgPrice_Fiat_ComboBox.SelectedIndex == 0)) {
                AccAvgPrice_Status_Label.Text = "Choose a crypto and fiat please";
                return;
            }

            AccAvgPrice_Status_Label.Text = "Pulling closed orders...";
            AccAvgPrice_Crypto_ComboBox.Enabled = false;
            AccAvgPrice_Fiat_ComboBox.Enabled = false;
            AccAvgPrice_Start_DTPicker.Enabled = false;
            AccAvgPrice_End_DTPicker.Enabled = false;

            // blank out the previous results
            AccAvgPrice_Result_TextBox.Text = "";
            AccAvgPrice_TotalCrypto_TextBox.Text = "";
            AccAvgPrice_TotalFiat_TextBox.Text = "";
            AvgPriceResult = "";
            TotalCryptoDealt = "";
            TotalFiatDealt = "";
            AccAvgPrice_RemainingToDeal_TextBox.BackColor = SystemColors.Control;
            AccAvgPrice_RemainingToDeal_TextBox.Text = "";
            AccAvgPrice_RemaingToDealCurrency_Label.Text = "";
            

            string crypto = AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString();
            string fiat = AccAvgPrice_Fiat_ComboBox.SelectedItem.ToString();

            Task<Page<BankHistoryOrder>> cOrdersTask = new Task<Page<BankHistoryOrder>>(() => pIR.GetClosedOrders(crypto, fiat));
            cOrdersTask.Start();
            CalculateAvgPrice(await cOrdersTask);
        }

        public void CalculateAvgPrice(Page<BankHistoryOrder> cOrders) {

            int count = 0;
            decimal totalCryptoDealt = 0;
            decimal totalValue = 0;

            foreach (BankHistoryOrder cOrder in cOrders.Data) {
                // first we make sure the order isn't still open
                if ((cOrder.Status == OrderStatus.Filled) || (cOrder.Status == OrderStatus.PartiallyFilledAndCancelled) ||
                    (cOrder.Status == OrderStatus.PartiallyFilledAndExpired) || (cOrder.Status == OrderStatus.PartiallyFilledAndFailed)) {
                    // then we make sure it's within the time period
                    if ((cOrder.CreatedTimestampUtc > AccAvgPrice_Start_DTPicker.Value.ToUniversalTime()) && (cOrder.CreatedTimestampUtc < AccAvgPrice_End_DTPicker.Value.ToUniversalTime())) {
                        // then we makse sure it's a buy or sell as specified
                        if (((AccAvgPrice_BuySell_ComboBox.SelectedIndex == 0) && ((cOrder.OrderType == OrderType.LimitBid) || (cOrder.OrderType == OrderType.MarketBid))) ||
                            ((AccAvgPrice_BuySell_ComboBox.SelectedIndex == 1) && ((cOrder.OrderType == OrderType.LimitOffer) || (cOrder.OrderType == OrderType.MarketOffer))) ||
                            (AccAvgPrice_BuySell_ComboBox.SelectedIndex == 2)) {
                            // make sure the order has valid values
                            if (cOrder.Value.HasValue && cOrder.AvgPrice.HasValue) {
                                decimal vol = cOrder.Volume;
                                if (cOrder.Outstanding.HasValue) {
                                    vol = vol - cOrder.Outstanding.Value;
                                }
                                totalCryptoDealt += vol;
                                totalValue += cOrder.AvgPrice.Value * vol;  // we can't just use the .Value property because it has a resolution of 2 decimal places which isn't enough.  So we use AvgPrice and vol to work backwards what the real Value is
                                count++;
                            }
                        }
                    }
                }
            }

            AccAvgPrice_Status_Label.Text = "Found " + count + (count == 1 ? " order that matches" : " orders that match") + " this criteria";

            if (count > 0) {

                if ((totalCryptoDealt == 0) || (totalValue == 0)) {
                    AccAvgPrice_Result_TextBox.Text = "Error: totalValue: " + totalValue + " totalCryptoDealt: " + totalCryptoDealt;
                }
                else {
                    string crypto = (AccAvgPrice_Crypto_ComboBox.SelectedItem == "BTC" ? "XBT" : AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString());

                    decimal tempRes = Math.Round((totalValue / totalCryptoDealt), dce.currencyFiatDivision[crypto], MidpointRounding.AwayFromZero);
                    AvgPriceResult = tempRes.ToString();
                    AccAvgPrice_Result_TextBox.Text = Utilities.FormatValue(tempRes, dce.currencyFiatDivision[crypto], false);
                    AccAvgPrice_CopyAvg_Button.Enabled = true;
                    AccAvgPrice_TotalCrypto_TextBox.Text = Utilities.FormatValue(totalCryptoDealt, 8, false);
                    TotalCryptoDealt = totalCryptoDealt.ToString();
                    AccAvgPrice_CopyCrypto_Button.Enabled = true;
                    AccAvgPrice_TotalFiat_TextBox.Text = Utilities.FormatValue(totalValue, 2, false);
                    TotalFiatDealt = totalValue.ToString();
                    AccAvgPrice_CopyFiat_Button.Enabled = true;

                    if (!string.IsNullOrEmpty(AccAvgPrice_DealSize_TextBox.Text) && (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex > 0)) {  // if the user has entered a deal size and chose a currency
                        if (decimal.TryParse(AccAvgPrice_DealSize_TextBox.Text, out decimal dealSize)) {
                            // deal size user entry is good
                            if (dealSize > 0) {
                                decimal dealtSoFar = (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 1 ? totalCryptoDealt : totalValue);
                                int decimals = 8;  // crypto vol should go to 8 dp
                                if (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 2) decimals = 2;  // fiat just do 2

                                // colour the remaining box
                                if ((dealSize - dealtSoFar) < 0) AccAvgPrice_RemainingToDeal_TextBox.BackColor = Color.MistyRose;
                                else if ((dealSize - dealtSoFar == 0)) AccAvgPrice_RemainingToDeal_TextBox.BackColor = Color.PaleGreen;
                                else AccAvgPrice_RemainingToDeal_TextBox.BackColor = Color.LightGoldenrodYellow;

                                AccAvgPrice_RemainingToDeal_TextBox.Text = Utilities.FormatValue((dealSize - dealtSoFar), decimals, false);
                                AccAvgPrice_RemainingToDeal_TextBox.Tag = dealSize - dealtSoFar;  // put the raw unformatted number in the tag, so we can copy this if they double click the label
                                AccAvgPrice_RemaingToDealCurrency_Label.Text = (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 1 ? AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString() : AccAvgPrice_Fiat_ComboBox.SelectedItem.ToString());
                            }
                            else AccAvgPrice_RemainingToDeal_TextBox.Text = "Must be > 0";
                        }
                        else AccAvgPrice_RemainingToDeal_TextBox.Text = "Bad deal size";
                    }
                    else {  // don't have enough info to calculate remaining deal size, so blank it out
                        AccAvgPrice_RemainingToDeal_TextBox.BackColor = SystemColors.Control;
                        AccAvgPrice_RemainingToDeal_TextBox.Text = "";
                    }
                }
            }
            else {
                AccAvgPrice_Result_TextBox.Text = "";
                AvgPriceResult = "";
                AccAvgPrice_CopyAvg_Button.Enabled = false;
                AccAvgPrice_TotalCrypto_TextBox.Text = "";
                AccAvgPrice_CopyCrypto_Button.Enabled = false;
                AccAvgPrice_TotalFiat_TextBox.Text = "";
                AccAvgPrice_CopyFiat_Button.Enabled = false;
                AccAvgPrice_RemainingToDeal_TextBox.Text = "";
                AccAvgPrice_RemainingToDeal_TextBox.BackColor = SystemColors.Control;
                AccAvgPrice_RemaingToDealCurrency_Label.Text = AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString();

            }

            AccAvgPrice_Crypto_ComboBox.Enabled = true;
            AccAvgPrice_Fiat_ComboBox.Enabled = true;
            AccAvgPrice_Start_DTPicker.Enabled = true;
            AccAvgPrice_End_DTPicker.Enabled = true;

        }

        private void AccAvgPrice_Copy_Button_Click(object sender, EventArgs e) {
            Clipboard.SetText(AvgPriceResult);
        }

        public void UpdatePrice(Page<BankHistoryOrder> cOrders) {
            if ((AccAvgPrice_AutoUpdate_CheckBox.Checked) && (AccAvgPrice_Crypto_ComboBox.SelectedIndex > 0) && (AccAvgPrice_Fiat_ComboBox.SelectedIndex > 0)) {
                string crypto = cOrders.Data.ElementAt(0).PrimaryCurrencyCode.ToString().ToUpper();

                switch (crypto) {
                    case "XBT":
                        crypto = "BTC";
                        break;
                    case "UST":
                        crypto = "USDT";
                        break;

                }

                //if (crypto == "XBT") crypto = "BTC";

                if ((crypto == AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString()) &&
                    (cOrders.Data.ElementAt(0).SecondaryCurrencyCode.ToString().ToUpper() == AccAvgPrice_Fiat_ComboBox.SelectedItem.ToString())) {

                    CalculateAvgPrice(cOrders);
                }
            }
        }

        private void AccAvgPrice_CopyCrypto_Button_Click(object sender, EventArgs e) {
            Clipboard.SetText(TotalCryptoDealt);
        }

        private void AccAvgPrice_CopyFiat_Button_Click(object sender, EventArgs e) {
            Clipboard.SetText(TotalFiatDealt);
        }

        private void AccAvgPrice_Start_Label_DoubleClick(object sender, EventArgs e) {
            AccAvgPrice_Start_DTPicker.Value = DateTime.Now;
        }

        private void AccAvgPrice_End_Label_DoubleClick(object sender, EventArgs e) {
            AccAvgPrice_End_DTPicker.Value = DateTime.Now + TimeSpan.FromDays(1);
        }

        private void AccAvgPrice_SendRemainingToVolumeField_button_Click(object sender, EventArgs e) {
            if ((null != AccAvgPrice_RemainingToDeal_TextBox.Tag) && (!string.IsNullOrEmpty(AccAvgPrice_RemainingToDeal_TextBox.Tag.ToString()))) {
                IRT.IRAccount_FillVolumeField(AccAvgPrice_RemainingToDeal_TextBox.Tag.ToString());
            }
        }
    }
}
