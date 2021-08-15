using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
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
        private int oldDealSizeCurrencySelected = 1;  // remembers which dealsize currency (eg crypto or fiat) is selected for when we force crypto and disable the control if they choose more than one fiat currency, and then deselect to just 1 currency and we need to remember which option they had selected bofer
        private ConcurrentDictionary<string, Tuple<Button, bool>> fiatCurrenciesSelected = new ConcurrentDictionary<string, Tuple<Button, bool>>();

        public AccAvgPrice(DCE _DCE, PrivateIR _pIR, IRTicker _IRT, bool enableAutoUpdate = false, string crypto = "", string fiat = "AUD", int direction = 0) {
            InitializeComponent();
            dce = _DCE;
            pIR = _pIR;
            IRT = _IRT;

            // initialise the controls
            Utilities.PopulateCryptoComboBox(dce, AccAvgPrice_Crypto_ComboBox);
            //PopulateFiatComboBox();
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


            fiatCurrenciesSelected.TryAdd("AUD", new Tuple<Button, bool>(AccAvgPrice_FiatAUD_button, false));
            fiatCurrenciesSelected.TryAdd("USD", new Tuple<Button, bool>(AccAvgPrice_FiatUSD_button, false));
            fiatCurrenciesSelected.TryAdd("NZD", new Tuple<Button, bool>(AccAvgPrice_FiatNZD_button, false));
            fiatCurrenciesSelected.TryAdd("SGD", new Tuple<Button, bool>(AccAvgPrice_FiatSGD_button, false));

            // simulate clicking on the button to make it selected by default
            AccAvgPrice_Fiat_button_click(fiatCurrenciesSelected[fiat].Item1, null);

        }

        // I guess to do this fully properly I should run this every time they choose a crypto to ensure that the crypto-fiat pair exists (ie check it against dce.usablepairs())
        // but i'm almost certain we'll only use this for IR, so let's not worry right now.  Just "statically" create it with the knowledge that every crypto works with every fiat
        /*private void PopulateFiatComboBox() {
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
        }*/

        private async void AccAvgPrice_Go_Button_Click(object sender, EventArgs e) {
            if (AccAvgPrice_Crypto_ComboBox.SelectedIndex == 0) {
                AccAvgPrice_Status_Label.Text = "Choose a crypto and fiat please";
                return;
            }
            if (AccAvgPrice_End_DTPicker.Value < AccAvgPrice_Start_DTPicker.Value) {
                AccAvgPrice_Status_Label.Text = "Last Order time is before First Order time!";
                return;
            }

            bool atLeastOneCurrencySelected = false;
            foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                if (fiatButton.Value.Item2) {
                    atLeastOneCurrencySelected = true;
                    break;
                }
            }
            if (!atLeastOneCurrencySelected) {
                AccAvgPrice_Status_Label.Text = "Choose a crypto and fiat please";
                return;
            }

            AccAvgPrice_Status_Label.Text = "Pulling closed orders...";
            AccAvgPrice_Crypto_ComboBox.Enabled = false;

            foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                fiatButton.Value.Item1.Enabled = false;
            }

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
            Page<BankHistoryOrder> ultimateBHO = new Page<BankHistoryOrder>();
            List<BankHistoryOrder> allOrders = new List<BankHistoryOrder>();
            int fiatCurrenciesSelectedCount = 0;
            try {
                // now we need to grab all the orders for each fiat currency selected and concatenate them into one, then stuff it back in the Page<> object and send it to get calculated
                foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                    if (fiatButton.Value.Item2) {
                        fiatCurrenciesSelectedCount++;
                        Task<Page<BankHistoryOrder>> cOrdersTask = new Task<Page<BankHistoryOrder>>(() => pIR.GetClosedOrders(crypto, fiatButton.Key));
                        cOrdersTask.Start();
                        ultimateBHO = await cOrdersTask;

                        foreach (BankHistoryOrder order in ultimateBHO.Data) {  // bad pattern here.  we should check that ultimateBHO is null, but basically if it is null, we want to do everything in the catch block... so just let it fail and get caught
                            allOrders.Add(order);
                        }
                    }
                }
                ultimateBHO.Data = allOrders;
                CalculateAvgPrice(ultimateBHO, (fiatCurrenciesSelectedCount > 1));
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - failed to pull closed orders when clicking the average price go button.  error: " + ex.Message);
                AccAvgPrice_Status_Label.Text = "Failed to pull closed orders, please try again.";

                // pulling closed orders failed, lets re-enable the controls
                AccAvgPrice_Crypto_ComboBox.Enabled = true;

                foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                    fiatButton.Value.Item1.Enabled = true;
                }

                AccAvgPrice_Start_DTPicker.Enabled = true;
                AccAvgPrice_End_DTPicker.Enabled = true;
            }
        }

        public void CalculateAvgPrice(Page<BankHistoryOrder> cOrders, bool isMultipleFiatCurrenciesSelected) {

            int count = 0;
            decimal totalCryptoDealt = 0;
            decimal totalValue = 0;

            foreach (BankHistoryOrder cOrder in cOrders.Data) {
                // first we make sure the order isn't still open
                if ((cOrder.Status == OrderStatus.Filled) || (cOrder.Status == OrderStatus.PartiallyFilledAndCancelled) || (cOrder.Status == OrderStatus.PartiallyFilled) ||
                    (cOrder.Status == OrderStatus.PartiallyFilledAndExpired) || (cOrder.Status == OrderStatus.PartiallyFilledAndFailed)) {
                    // then we make sure it's within the time period
                    /*Debug.Print(" -- order timestamp: " + cOrder.CreatedTimestampUtc);
                    Debug.Print("-- DTPicker value: " + AccAvgPrice_Start_DTPicker.Value + ", UTC val: " + AccAvgPrice_Start_DTPicker.Value.ToUniversalTime());*/
                    if ((cOrder.CreatedTimestampUtc >= AccAvgPrice_Start_DTPicker.Value.ToUniversalTime()) && (cOrder.CreatedTimestampUtc <= AccAvgPrice_End_DTPicker.Value.ToUniversalTime())) {
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
                                // this isn't accurate enough.  Need to use exact trade numbers
                                totalValue += cOrder.Value.Value;
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

                    decimal tempRes = Math.Round((totalValue / totalCryptoDealt), dce.currencyDecimalPlaces[crypto].Item2, MidpointRounding.AwayFromZero);
                    AvgPriceResult = tempRes.ToString();

                    if (isMultipleFiatCurrenciesSelected) {
                        AccAvgPrice_Result_TextBox.Text = "N/A";
                        AccAvgPrice_TotalFiat_TextBox.Text = "N/A";
                    }
                    else {
                        AccAvgPrice_Result_TextBox.Text = Utilities.FormatValue(tempRes, dce.currencyDecimalPlaces[crypto].Item2, false);
                        AccAvgPrice_TotalFiat_TextBox.Text = Utilities.FormatValue(totalValue, 2, false);
                    }

                    AccAvgPrice_CopyAvg_Button.Enabled = true;
                    AccAvgPrice_TotalCrypto_TextBox.Text = Utilities.FormatValue(totalCryptoDealt, 8, false);
                    TotalCryptoDealt = totalCryptoDealt.ToString();
                    AccAvgPrice_CopyCrypto_Button.Enabled = true;
                    TotalFiatDealt = totalValue.ToString();
                    AccAvgPrice_CopyFiat_Button.Enabled = true;

                    if (!string.IsNullOrEmpty(AccAvgPrice_DealSize_TextBox.Text) && (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex > 0)) {  // if the user has entered a deal size and chose a currency
                        if (decimal.TryParse(AccAvgPrice_DealSize_TextBox.Text, out decimal dealSize)) {
                            // deal size user entry is good
                            if (dealSize > 0) {
                                decimal dealtSoFar = (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 1 ? totalCryptoDealt : totalValue);
                                int decimals = dce.currencyDecimalPlaces[crypto].Item1;  // crypto vol should go to 8 dp

                                if (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 2) decimals = 2;  // fiat just do 2

                                // colour the remaining box
                                if ((dealSize - dealtSoFar) < 0) AccAvgPrice_RemainingToDeal_TextBox.BackColor = Color.MistyRose;
                                else if ((dealSize - dealtSoFar == 0)) AccAvgPrice_RemainingToDeal_TextBox.BackColor = Color.PaleGreen;
                                else AccAvgPrice_RemainingToDeal_TextBox.BackColor = Color.LightGoldenrodYellow;

                                AccAvgPrice_RemainingToDeal_TextBox.Text = Utilities.FormatValue((dealSize - dealtSoFar), decimals, false);
                                AccAvgPrice_RemainingToDeal_TextBox.Tag = dealSize - dealtSoFar;  // put the raw unformatted number in the tag, so we can copy this if they double click the label

                                // ok we need to work out the fiat currency selected.  We should never need this if more than one is selected..
                                int fiatCount = 0;
                                string fiatSelected = "";
                                foreach (KeyValuePair<string, Tuple<Button, bool>> fiatSelectedKVP2 in fiatCurrenciesSelected) {
                                    if (fiatSelectedKVP2.Value.Item2) {
                                        fiatCount++;
                                        fiatSelected = fiatSelectedKVP2.Key;
                                    }
                                }
                                if ((fiatCount > 1) && (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 2)) Debug.Print("We somehow have multiple fiats selected.  BAD");

                                AccAvgPrice_RemaingToDealCurrency_Label.Text = (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 1 ? AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString() : fiatSelected);
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

            foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                fiatButton.Value.Item1.Enabled = true;
            }

            AccAvgPrice_Start_DTPicker.Enabled = true;
            AccAvgPrice_End_DTPicker.Enabled = true;

        }

        private void AccAvgPrice_Copy_Button_Click(object sender, EventArgs e) {
            Clipboard.SetText(AvgPriceResult);
        }

        // cOrders.Data must have at least one element, this method assumes this..
        public async void UpdatePrice(Page<BankHistoryOrder> cOrders) {

            // off the bat - if the fiat currency isn't my main currency, then bail.  we assume here that there is at least one element in the Data container
            // not anymore, can have more than one fiat selected
            //if (cOrders.Data.ElementAt(0).SecondaryCurrencyCode.ToString().ToUpper() != dce.CurrentSecondaryCurrency) return;

            string cOrdersFiat = cOrders.Data.ElementAt(0).SecondaryCurrencyCode.ToString().ToUpper();

            int fiatSelectedCount = 0;
            bool sentFiatIsSelected = false;
            foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                if (fiatButton.Value.Item2) {
                    fiatSelectedCount++;
                    if (fiatButton.Key == cOrdersFiat) sentFiatIsSelected = true;
                }
            }

            if (!sentFiatIsSelected) return;  // the secondary currency of the sent orders is not in our list, so don't process it

            if ((AccAvgPrice_AutoUpdate_CheckBox.Checked) && (AccAvgPrice_Crypto_ComboBox.SelectedIndex > 0) && (fiatSelectedCount > 0)) {
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

                // OK.  if we have multiple fiat currencies selected, then we need to grab 



                if (crypto == AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString()) {

                    // OK.  the sent orders are the correct crypto, but what if the user has selected multiple fiat currencies?  If they have we need to pull the closed
                    // orders for the other currencies and concatenate.  No need to pull the currently selected fiat because we already have been sent it
                    // to get here we MUST be seeing orders from the currently selected  (in the main panel) fiat currency, because we filtered out the others at the 
                    // top of this sub.

                    //Page<BankHistoryOrder> ultimateBHO = new Page<BankHistoryOrder>();
                    List<BankHistoryOrder> allOrders = new List<BankHistoryOrder>(cOrders.Data);
                    int selectedFiatCurrencies = 0;
                    try {
                        // now we need to grab all the orders for each fiat currency selected and concatenate them into one, then stuff it back in the Page<> object and send it to get calculated
                        foreach (KeyValuePair<string, Tuple<Button, bool>> fiatButton in fiatCurrenciesSelected) {
                            if (fiatButton.Value.Item2) {
                                selectedFiatCurrencies++;
                                if (fiatButton.Key != cOrdersFiat) {  // don't bother pulling orders for the fiat orders we were sent.  Only other fiats that are selected
                                    Task<Page<BankHistoryOrder>> cOrdersTask = new Task<Page<BankHistoryOrder>>(() => pIR.GetClosedOrders(crypto, fiatButton.Key));
                                    cOrdersTask.Start();
                                    Page<BankHistoryOrder> secondaryFiatOrders = await cOrdersTask;

                                    foreach (BankHistoryOrder order in secondaryFiatOrders.Data) {
                                        allOrders.Add(order);
                                    }
                                }
                            }
                        }
                        cOrders.Data = allOrders;
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " - failed to pull closed orders when requested in UpdatePrice() sub.  error: " + ex.Message);
                        AccAvgPrice_Status_Label.Text = "Failed to pull closed orders, please try again.";
                        return;
                    }


                    CalculateAvgPrice(cOrders, (selectedFiatCurrencies > 1));
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

        private void AccAvgPrice_Start_DTPicker_ValueChanged(object sender, EventArgs e) {
            // make a note of the start date, so we only pull orders from this date onwards to reduce our closedOrders cost.  
            // this way when pulling closed orders we only pull what's required because we know how far back we have to look
            pIR.earliestClosedOrderRequired = AccAvgPrice_Start_DTPicker.Value;
        }

        private void AccAvgPrice_FormClosing(object sender, FormClosingEventArgs e) {
            pIR.earliestClosedOrderRequired = null;  // no more start date, just grab the 7 newest closed orders
        }

        private void AccAvgPrice_Crypto_ComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            pIR.AvgPriceSelectedCrypto = (AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString() == "BTC" ? "XBT" : AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString());
            AccAvgPrice_DealSize_TextBox_TextChanged(null, null);  // simulate text change to validate and adjust
        }

        private void AccAvgPrice_Fiat_button_click(object sender, EventArgs e) {
            Button fiatButton = (Button)sender;  // this is the button the user clicked
            foreach (KeyValuePair<string, Tuple<Button, bool>> fiatSelectedKVP in fiatCurrenciesSelected) {
                if (fiatButton == fiatSelectedKVP.Value.Item1) {  // if this is the button the user clicked...
                    // if it's true, make it false.  if false, make it true.  need to colour them too
                    if (fiatSelectedKVP.Value.Item2) { // it's true (selected), let's set it to false
                        fiatCurrenciesSelected[fiatSelectedKVP.Key] = new Tuple<Button, bool>(fiatSelectedKVP.Value.Item1, false);
                        fiatSelectedKVP.Value.Item1.ForeColor = Color.Black;
                        fiatSelectedKVP.Value.Item1.BackColor = Color.White;
                    }
                    else {  // it's false, let's choose this fiat

                        fiatCurrenciesSelected[fiatSelectedKVP.Key] = new Tuple<Button, bool>(fiatSelectedKVP.Value.Item1, true);
                        fiatSelectedKVP.Value.Item1.ForeColor = Color.White;
                        fiatSelectedKVP.Value.Item1.BackColor = Color.RoyalBlue;
                    }
                    break;
                }
            }
            // now we need to disable the dealsizecurrency control and force Crypto as the selection if there is more than 1 fiat selected
            int fiatCount = 0;
            foreach (KeyValuePair<string, Tuple<Button, bool>> fiatSelectedKVP2 in fiatCurrenciesSelected) {
                if (fiatSelectedKVP2.Value.Item2) fiatCount++;
            }
            // i think this bit doesn't work.  The fiat/crypto drop down menu doesn't enable/disable as expected, need to fix
            if (fiatCount > 1) {
                oldDealSizeCurrencySelected = AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex;
                AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex = 1;
                AccAvgPrice_DealSizeCurrency_ComboBox.Enabled = false;
            }
            else {
                if (AccAvgPrice_DealSizeCurrency_ComboBox.Enabled == false) {
                    AccAvgPrice_DealSizeCurrency_ComboBox.Enabled = true;
                    AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex = oldDealSizeCurrencySelected;
                }
            }
            pIR.fiatCurrenciesSelected = fiatCurrenciesSelected;
            //AccAvgPrice_Go_Button_Click(null, null);  // simulate a click on the button
        }

        private void AccAvgPrice_DealSizeCurrency_ComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndex == 1) {
                AccAvgPrice_SendRemainingToVolumeField_button.Enabled = true;
                AccAvgPrice_CopyFiat_Button.Enabled = true;
            }
            else {
                AccAvgPrice_SendRemainingToVolumeField_button.Enabled = false;
                AccAvgPrice_CopyFiat_Button.Enabled = false;
            }
        }

        private void AccAvgPrice_DealSize_TextBox_TextChanged(object sender, EventArgs e) {
            if (AccAvgPrice_Crypto_ComboBox.SelectedIndex == 0) return;
            // let's truncate and validate
            string crypto = AccAvgPrice_Crypto_ComboBox.SelectedItem.ToString();
            decimal adjustedVol = 0;
            if (crypto == "BTC") crypto = "XBT";
            if (decimal.TryParse(AccAvgPrice_DealSize_TextBox.Text, out decimal dealSize)) {
                if (dealSize > 0) {
                    int mantissaLen = BitConverter.GetBytes(decimal.GetBits(dealSize)[3])[2];
                    if (mantissaLen > dce.currencyDecimalPlaces[crypto].Item1) {
                        adjustedVol = Utilities.Truncate(dealSize, (byte)(dce.currencyDecimalPlaces[crypto].Item1));
                        AccAvgPrice_DealSize_TextBox.Text = adjustedVol.ToString();
                        AccAvgPrice_DealSize_TextBox.SelectionStart = AccAvgPrice_DealSize_TextBox.Text.Length;
                        AccAvgPrice_DealSize_TextBox.SelectionLength = 0;
                    }
                }

            }
        }
    }
}
