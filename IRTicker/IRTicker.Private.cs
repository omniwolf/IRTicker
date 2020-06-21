using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Concurrent;

namespace IRTicker {
    // holds code to do with the IR Accounts panel
    partial class IRTicker {

        private string AccountSelectedCrypto = "XBT";

        private async void DrawIRAccounts() {
            Task<Dictionary<string, Account>> irAccountsTask = new Task<Dictionary<string, Account>>(pIR.GetAccounts);
            irAccountsTask.Start();

            IRAccount_panel.Visible = true;
            Main.Visible = false;

            Label CurrentSecondaryCurrecyLabel = UIControls_Dict["IR"].Label_Dict[DCEs["IR"].CurrentSecondaryCurrency + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.DarkBlue;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

            Label SelectedCrypto = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
            SelectedCrypto.ForeColor = Color.DarkOrange;
            SelectedCrypto.Font = new Font(SelectedCrypto.Font.FontFamily, 14.25f, FontStyle.Bold);

            var mSummaries = DCEs["IR"].GetCryptoPairs();

            Dictionary<string, Account> irAccounts = await irAccountsTask;

            foreach (KeyValuePair<string, Account> acc in irAccounts) {
                if (UIControls_Dict["IR"].Label_Dict.ContainsKey(acc.Key + "_Account_Total")) {
                    UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Total"].Text = 
                        Utilities.FormatValue(acc.Value.AvailableBalance);
                }
                else {
                    Debug.Print(DateTime.Now + " new currency?? - " + acc.Key);
                }

                if (UIControls_Dict["IR"].Label_Dict.ContainsKey(acc.Key + "_Account_Value") && mSummaries.ContainsKey(acc.Key + "-" + DCEs["IR"].CurrentSecondaryCurrency)) {
                    UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Value"].Text =
                        Utilities.FormatValue(acc.Value.AvailableBalance * mSummaries[acc.Key + "-" + DCEs["IR"].CurrentSecondaryCurrency].CurrentHighestBidPrice);
                }
                else {
                    Debug.Print(DateTime.Now + " new currency (value)?? - " + acc.Key);
                }
            }
            updateDepositAddress();

            Task<Page<BankHistoryOrder>> IRAccountClosedOrders = new Task<Page<BankHistoryOrder>>(() => pIR.GetClosedOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency));
            IRAccountClosedOrders.Start();

            Page<BankHistoryOrder> closedOrders = await IRAccountClosedOrders;
            drawClosedOrders(closedOrders.Data);

            /*Task<Page<BankHistoryOrder>> IRAccountOpenOrders = new Task<Page<BankHistoryOrder>>(() => pIR.GetOpenOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency));
            IRAccountOpenOrders.Start();

            Page<BankHistoryOrder> openOrders = await IRAccountOpenOrders;
            drawOpenOrders(openOrders.Data);*/

        }

        private void drawClosedOrders(IEnumerable<BankHistoryOrder> closedOrders) {
            AccountClosedOrders_listview.Items.Clear();
            foreach (BankHistoryOrder order in closedOrders) {
                AccountClosedOrders_listview.Items.Add(new ListViewItem(new string[] {
                    order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                    Utilities.FormatValue(order.Volume),
                    Utilities.FormatValue(order.AvgPrice.Value),
                    Utilities.FormatValue(order.Value.Value)}));
                AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].ToolTipText = order.CreatedTimestampUtc.ToLocalTime().ToString();
            }
        }

        private void drawOpenOrders(IEnumerable<BankHistoryOrder> openOrders) {
            AccountOpenOrders_listview.Items.Clear();
            foreach (BankHistoryOrder order in openOrders) {
                if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) return;
                AccountOpenOrders_listview.Items.Add(new ListViewItem(new string[] {
                    order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                    Utilities.FormatValue(order.Volume),
                    Utilities.FormatValue(order.Price.Value),
                    Utilities.FormatValue(order.Outstanding.Value)}));
                AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].ToolTipText = order.CreatedTimestampUtc.ToLocalTime().ToString();
            }
        }

        private async void updateDepositAddress() {
            Task<DigitalCurrencyDepositAddress> deposAddressTask = new Task<DigitalCurrencyDepositAddress>(() => pIR.GetDepositAddress(AccountSelectedCrypto));
            deposAddressTask.Start();

            AccountWithdrawalCrypto_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " deposit address";

            DigitalCurrencyDepositAddress deposAddress = await deposAddressTask;
            AccountWithdrawalAddress_label.Text = deposAddress.DepositAddress;
            AccountWithdrawalNextCheck_label.Text = "Next check: " + deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime().ToString();
            AccountWithdrawalLastCheck_label.Text = "Last checked: " + deposAddress.LastCheckedTimestampUtc.Value.ToLocalTime().ToString();

            if (string.IsNullOrEmpty(deposAddress.Tag)) {
                AccountWithdrawalTag_label.Visible = false;
                AccountWithdrawalTag_value.Text = "";
            }
            else {
                AccountWithdrawalTag_label.Visible = true;
                AccountWithdrawalTag_value.Text = deposAddress.Tag;
            }
        }

        private void setCurrencyValues(string crypto, decimal price) {
            if (pIR.accounts.ContainsKey(crypto)) {
                UIControls_Dict["IR"].Label_Dict[crypto + "_Account_Value"].Text = Utilities.FormatValue(price * pIR.accounts[crypto].AvailableBalance);
            }
        }

        public void updateAccountOrderBook(string pair) {

            if (AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency != pair) return;
            if (!DCEs["IR"].IR_OBs.ContainsKey(pair)) return;

            // here we grab the buy or sell order book, make a copy, and then sort it
            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBook;
            if (AccountBuySell_listbox.SelectedIndex == 0) {
                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item2.ToArray();  // because we're buying from the sell orders
                orderedBook = arrayBook.OrderBy(k => k.Key);
                //Debug.Print("--- Account picked the sell side, top order is: " + orderedBook.First().Key);
            }
            else {
                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item1.ToArray();  // because we're selling to the buy orders
                orderedBook = arrayBook.OrderByDescending(k => k.Key);
                //Debug.Print("--- Account picked the buy side, top order is: " + orderedBook.First().Key);
            }

            AccountOrders_listview.Items.Clear();
            int count = 1;
            decimal cumulativeVol = 0;
            decimal totalOrderValue = 0;
            decimal trackedOrderVolume = -1;

            // if we can parse the volume box, and it's a market order, let's work out the order value
            if (decimal.TryParse(AccountOrderVolume_textbox.Text, out decimal orderVolParsed) && AccountOrderType_listbox.SelectedIndex == 0) {
                if (orderVolParsed > 0) {
                    trackedOrderVolume = orderVolParsed;
                }
            }

            foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> pricePoint in orderedBook) {
                decimal totalVolume = 0;

                foreach (KeyValuePair<string, DCE.OrderBook_IR> order in pricePoint.Value) {
                    totalVolume += order.Value.Volume;
                    if (trackedOrderVolume != -1) {  // only bother working out this stuff we have a real Tracked value
                        if (trackedOrderVolume > order.Value.Volume) {
                            totalOrderValue += (order.Value.Volume * pricePoint.Key);
                            trackedOrderVolume -= order.Value.Volume;
                        }
                        else {  // ok, this is the last order to fill our proposed order
                            totalOrderValue += (trackedOrderVolume * pricePoint.Key);
                            trackedOrderVolume = 0;  // no more counting
                        }
                    }
                }

                if (count < 6) {  // less than 6 we haven't finished populating the listview yet
                    cumulativeVol += totalVolume;
                    AccountOrders_listview.Items.Add(new ListViewItem(new string[] { count.ToString(), Utilities.FormatValue(pricePoint.Key), Utilities.FormatValue(totalVolume), Utilities.FormatValue(cumulativeVol) }));
                    count++;
                }
                // this can be read like: "if we've finished populating the listview, but we still have more orders required 
                // to calculate our market order size, then keep looping
                if ((count > 5) && (trackedOrderVolume <= 0)) break;
            }

            if ((AccountOrderType_listbox.SelectedIndex == 0) && (trackedOrderVolume >= 0)) {
                if (trackedOrderVolume > 0) {
                    AccountEstOrderValue_value.Text = "Not enough depth!";
                }
                else {
                    AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(totalOrderValue);
                }
            }
            else AccountEstOrderValue_value.Text = "";
        }

        private void cryptoClicked(Label clickedLabel) {
            Label oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
            oldLabel.ForeColor = Color.Black;
            oldLabel.Font = new Font(oldLabel.Font.FontFamily, 14.25f, FontStyle.Regular);
            
            oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
            oldLabel.ForeColor = Color.FromArgb(64, 64, 64);

            oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
            oldLabel.ForeColor = Color.FromArgb(64, 64, 64);


            AccountSelectedCrypto = clickedLabel.Text.Substring(0, clickedLabel.Text.IndexOf(':'));
            if (AccountSelectedCrypto == "BTC") AccountSelectedCrypto = "XBT";

            Label newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
            newLabel.ForeColor = Color.DarkOrange;
            newLabel.Font = new Font(newLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

            newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
            newLabel.ForeColor = Color.DarkOrange;

            newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
            newLabel.ForeColor = Color.DarkOrange;

            updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);

            updateDepositAddress();
        }

        private void IRAccountClose_button_Click(object sender, EventArgs e) {
            IRAccount_panel.Visible = false;
            Main.Visible = true;
            Label CurrentSecondaryCurrecyLabel = UIControls_Dict["IR"].Label_Dict[DCEs["IR"].CurrentSecondaryCurrency + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.Black;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Regular);
        }

        private void AccountWithdrawalAddress_label_Click(object sender, EventArgs e) {
            Label address = (Label)sender;
            Clipboard.SetText(address.Text);
        }

        private void AccountWithdrawalNextCheck_label_Click(object sender, EventArgs e) {
            Label address = (Label)sender;
            pIR.CheckAddressNow(AccountSelectedCrypto, address.Text);
        }

        private void Account_label_Click(object sender, EventArgs e) {
            cryptoClicked((Label)sender);
        }

        private void AcccountOrderType_listbox_SelectedIndexChanged(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex == 1) {
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountEstOrderValue_value.Text = "";
                AccountPlaceOrder_button.Enabled = false;  // we're not ready for this yet
            }
            else {
                AccountLimitPrice_label.Visible = false;
                AccountLimitPrice_textbox.Visible = false;
                updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
                AccountPlaceOrder_button.Enabled = true;  // can only do market orders
            }
        }

        private void AccountBuySell_listbox_Click(object sender, EventArgs e) {
            if (AccountBuySell_listbox.SelectedIndex == 0) {
                AccountPlaceOrder_button.Text = "Buy now";
            }
            else {
                AccountPlaceOrder_button.Text = "Sell now";
            }
        }

        private void AccountOrderVolume_textbox_TextChanged(object sender, EventArgs e) {
            if (decimal.TryParse(AccountOrderVolume_textbox.Text, out decimal orderVol)) {
                if (orderVol > 0) updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
                AccountPlaceOrder_button.Enabled = true;
            }
            else AccountPlaceOrder_button.Enabled = false;
        }

        private async void AccountPlaceOrder_button_Click(object sender, EventArgs e) {
            // need to also check for limit orders..
            Task<BankOrder> orderResultTask;
            BankOrder orderResult;

            // no need to check if we can parse the volume value, we already checked in AccountOrderVolume_textbox_TextChanged
            if (AccountOrderType_listbox.SelectedIndex == 0) {

                orderResultTask = new Task<BankOrder>(() => pIR.PlaceMarketOrder(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency,
                    (AccountBuySell_listbox.SelectedIndex == 0 ? "MarketBuy" : "MarketSell"),
                    decimal.Parse(AccountOrderVolume_textbox.Text)));
                orderResultTask.Start();
                orderResult = await orderResultTask;
            }
            else {  // Limit order
                orderResult = null;
            }


            if (orderResult != null) {
                if (orderResult.Status == OrderStatus.Failed) {
                    MessageBox.Show("Order failed?", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // need to disable the button until we have a result

        }
    }
}
