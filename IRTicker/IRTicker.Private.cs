using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Concurrent;
using System.Net;

namespace IRTicker {
    // holds code to do with the IR Accounts panel
    partial class IRTicker {

        private string AccountSelectedCrypto = "XBT";
        private Task updateOBTask;
        private bool IRAccountsButtonJustClicked = true;  // true if the use has just clicked the IR Accounts button.  If true and GetAccounts fails, then we close the IR Accounts panel and head back to the Main panel.  If false and GetAccounts fails, we just do it silently

        private void InitialiseAccountsPanel() {
            AccountOrderVolume_textbox.Enabled = true;
            AccountLimitPrice_textbox.Enabled = true;
            IRAccount_panel.Visible = true;
            Main.Visible = false;
            IRAccountsButtonJustClicked = true;
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { 
                PrivateIR.PrivateIREndPoints.GetAccounts, 
                PrivateIR.PrivateIREndPoints.GetOpenOrders, 
                PrivateIR.PrivateIREndPoints.GetClosedOrders, 
                PrivateIR.PrivateIREndPoints.GetAddress, 
                PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        private void DrawIRAccounts(Dictionary<string, Account> irAccounts) {

            synchronizationContext.Post(new SendOrPostCallback(o => {
                Dictionary<string, Account> _irAccounts = (Dictionary<string, Account>)o;
                Label CurrentSecondaryCurrecyLabel = UIControls_Dict["IR"].Label_Dict[DCEs["IR"].CurrentSecondaryCurrency + "_Account_Label"];
                CurrentSecondaryCurrecyLabel.ForeColor = Color.DarkBlue;
                CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

                Label SelectedCrypto = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                SelectedCrypto.ForeColor = Color.DarkOrange;
                SelectedCrypto.Font = new Font(SelectedCrypto.Font.FontFamily, 14.25f, FontStyle.Bold);

                var mSummaries = DCEs["IR"].GetCryptoPairs();

                foreach (KeyValuePair<string, Account> acc in _irAccounts) {
                    if (UIControls_Dict["IR"].Label_Dict.ContainsKey(acc.Key + "_Account_Total")) {
                        UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Total"].Text =
                            Utilities.FormatValue(acc.Value.AvailableBalance);
                        IRTickerTT_generic.SetToolTip(UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Total"], acc.Value.AvailableBalance.ToString());
                    }
                    else {
                        Debug.Print(DateTime.Now + " new currency?? - " + acc.Key);
                    }

                    if (UIControls_Dict["IR"].Label_Dict.ContainsKey(acc.Key + "_Account_Value") && mSummaries.ContainsKey(acc.Key + "-" + DCEs["IR"].CurrentSecondaryCurrency)) {
                        UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Value"].Text =
                            Utilities.FormatValue(acc.Value.AvailableBalance * mSummaries[acc.Key + "-" + DCEs["IR"].CurrentSecondaryCurrency].CurrentHighestBidPrice);
                    }
                    else {
                        //Debug.Print(DateTime.Now + " new currency (value)?? - " + acc.Key);
                    }
                }
            }), irAccounts);
        }

        // runs these network calls in order
        // this method should only be called from the UI because it can surface messageboxes
        // eg baiter and telegram should never use this.
        private void bulkSequentialAPICalls(List<PrivateIR.PrivateIREndPoints> endPoints, decimal volume = 0, decimal price = 0, string orderGuid = "") {

            foreach (PrivateIR.PrivateIREndPoints endP in endPoints) {
                if (endP == PrivateIR.PrivateIREndPoints.GetAccounts) {

                    Dictionary<string, Account> irAccounts;
                    try {
                        irAccounts = pIR.GetAccounts();
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " - couldn't pull getAccounts pIR because: " + ex.Message);
                        irAccounts = null;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error - GetAccounts", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //Debug.Print("PIR: gotACcounts");
                    if ((irAccounts == null) && IRAccountsButtonJustClicked) {
                        Debug.Print(DateTime.Now + " - there was an error, closing the accounts page");
                        synchronizationContext.Post(new SendOrPostCallback(o => {
                            Main.Visible = true;
                            IRAccount_panel.Visible = false;
                        }), null);
                        return;  // close the IRAccounts panel
                    }
                    else if ((irAccounts == null) && !IRAccountsButtonJustClicked) continue;
                    DrawIRAccounts(irAccounts);
                    IRAccountsButtonJustClicked = false;  // we have now run this successfully once after opening the panel, can set this to false.
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetAddress) {

                    DigitalCurrencyDepositAddress addressData;
                    try {
                        addressData = pIR.GetDepositAddress(AccountSelectedCrypto);
                    }
                    catch (Exception ex) {
                        Debug.Print(DateTime.Now + " - failed to call GetDepositAddress properly: " + ex.Message);
                        continue;
                    }
                    drawDepositAddress(addressData);
                }
                else if (endP == PrivateIR.PrivateIREndPoints.CheckAddress) {
                    string address = AccountWithdrawalAddress_label.Text;
                    DigitalCurrencyDepositAddress addressData;
                    try {
                        addressData = pIR.CheckAddressNow(AccountSelectedCrypto, address);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error - CheckAddress", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (addressData != null) drawDepositAddress(addressData);
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetOpenOrders) {
                    try {
                        var openOrders = pIR.GetOpenOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency);
                        drawOpenOrders(openOrders.Data);
                    }
                    catch (Exception ex) {
                        showBalloon("Failed to get open orders", "Error: " + ex.Message);
                        Debug.Print(DateTime.Now + " - GetOpenOrders failed with: " + ex.Message);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetClosedOrders) {
                    Page<BankHistoryOrder> closedOrders;
                    try {
                        closedOrders = pIR.GetClosedOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency);
                    }
                    catch (Exception ex) {
                        Debug.Print("Bulk method: couldn't pull closed orders: " + ex.Message);
                        continue;
                    }
                    if (closedOrders != null) {
                        drawClosedOrders(closedOrders.Data);
                    }
                }
                // need to be more robust, and pull multiple pages if necessary
                else if (endP == PrivateIR.PrivateIREndPoints.PlaceMarketOrder) {
                    //OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.MarketBid : OrderType.MarketOffer;

                    BankOrder orderResult;
                    try {
                        orderResult = pIR.PlaceMarketOrder(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency, null, -1);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error - Market order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Market order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.PlaceLimitOrder) {
                    //OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.LimitBid : OrderType.LimitOffer;

                    BankOrder orderResult;
                    try {
                        orderResult = pIR.PlaceLimitOrder(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency, null, -1, -1);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error - Limit order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Limit order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.CancelOrder) {
                    BankOrder cancelledOrder;
                    try {
                        cancelledOrder = pIR.CancelOrder(orderGuid);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error - Cancel order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Debug.Print("cancelled order status: " + cancelledOrder.Status.ToString());
                }
                else if (endP == PrivateIR.PrivateIREndPoints.UpdateOrderBook) {
                    updateOBTask = Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency));
                }
            }
        }

        private string buildOrderTT(BankHistoryOrder order, bool isOrderOpen) {
            string tt = "";
            switch (order.OrderType) {
                case OrderType.LimitBid:
                    tt += "Limit bid order" + Environment.NewLine + Environment.NewLine;
                    break;
                case OrderType.LimitOffer:
                    tt += "Limit offer order" + Environment.NewLine + Environment.NewLine;
                    break;
                case OrderType.MarketBid:
                    tt += "Market bid order" + Environment.NewLine + Environment.NewLine;
                    break;
                case OrderType.MarketOffer:
                    tt += "Market offer order" + Environment.NewLine + Environment.NewLine;
                    break;
            }
            tt += "Date created: " + order.CreatedTimestampUtc.ToLocalTime().ToString() + Environment.NewLine;

            decimal vol = order.Volume;
            if (order.Outstanding.HasValue && (order.Outstanding.Value > 0)) {

            }

            if (isOrderOpen) {
                tt += "Original volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Original.Volume + Environment.NewLine;
                tt += "Price: $ " + Utilities.FormatValue(order.Price.Value) + Environment.NewLine;
                if (order.Outstanding.HasValue && (order.Outstanding.Value > 0)) {
                    tt += "Outstanding volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Outstanding.Value + Environment.NewLine;
                }
            }

            else {
                tt += "Volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Volume + Environment.NewLine;
                tt += "Avg price: $ " + Utilities.FormatValue(order.AvgPrice.Value) + Environment.NewLine;
                tt += "Notional value: $ " + Utilities.FormatValue(order.Value.Value) + Environment.NewLine;
                tt += "Fee: " + Utilities.FormatValue(order.FeePercent, 2, false) + "%" + Environment.NewLine;
            }
            tt += "Status: " + order.Status;
            return tt;
        }

        private void drawClosedOrders(IEnumerable<BankHistoryOrder> closedOrders) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                IEnumerable<BankHistoryOrder> _closedOrders = (IEnumerable<BankHistoryOrder>)o;
                AccountClosedOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " closed orders";
                AccountClosedOrders_listview.Items.Clear();
                foreach (BankHistoryOrder order in _closedOrders) {
                    if ((order.Status != OrderStatus.Filled) && (order.Status != OrderStatus.PartiallyFilledAndCancelled) && (order.Status != OrderStatus.PartiallyFilledAndFailed)) continue;
                    decimal vol = order.Volume;
                    if (order.Outstanding.HasValue && order.Outstanding.Value > 0) {
                        vol = order.Volume - order.Outstanding.Value;
                    }
                    AccountClosedOrders_listview.Items.Add(new ListViewItem(new string[] {
                    order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                    Utilities.FormatValue(vol),
                    Utilities.FormatValue(order.AvgPrice.Value, 2),
                    Utilities.FormatValue(order.Value.Value)}));
                    AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].ToolTipText = buildOrderTT(order, false);
                    if (order.OrderType == OrderType.LimitBid || order.OrderType == OrderType.MarketBid) {
                        AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].BackColor = Color.Thistle;
                    }
                    else {
                        AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].BackColor = Color.PeachPuff;
                    }
                    AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].Tag = order;
                }
            }), closedOrders);
        }

        public void drawOpenOrders(IEnumerable<BankHistoryOrder> openOrders) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                IEnumerable<BankHistoryOrder> _openOrders = (IEnumerable<BankHistoryOrder>)o;
                AccountOpenOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " open orders";
                AccountOpenOrders_listview.Items.Clear();

                foreach (BankHistoryOrder order in _openOrders) {
                    if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) continue;
                    AccountOpenOrders_listview.Items.Add(new ListViewItem(new string[] {
                    order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                    Utilities.FormatValue(order.Volume),
                    Utilities.FormatValue(order.Price.Value, 2),
                    Utilities.FormatValue(order.Outstanding.Value)}));
                    AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].ToolTipText = buildOrderTT(order, true);
                    if (order.OrderType == OrderType.LimitBid) {
                        AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].BackColor = Color.Thistle;
                    }
                    else {
                        AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].BackColor = Color.PeachPuff;
                    }
                    AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].Tag = order;
                }
            }), openOrders);
        }

        private void drawDepositAddress(DigitalCurrencyDepositAddress deposAddress) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                DigitalCurrencyDepositAddress _deposAddress = (DigitalCurrencyDepositAddress)o;
                AccountWithdrawalCrypto_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " deposit address";

                AccountWithdrawalAddress_label.Text = _deposAddress.DepositAddress;

                string nextCheck;
                if (_deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime() < DateTime.Now) nextCheck = "ASAP";
                else nextCheck = _deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime().ToString();
                AccountWithdrawalNextCheck_label.Text = "Next check: " + nextCheck;

                AccountWithdrawalLastCheck_label.Text = "Last checked: " + _deposAddress.LastCheckedTimestampUtc.Value.ToLocalTime().ToString(); ;

                if (string.IsNullOrEmpty(_deposAddress.Tag)) {
                    AccountWithdrawalTag_label.Visible = false;
                    AccountWithdrawalTag_value.Text = "";
                }
                else {
                    AccountWithdrawalTag_label.Visible = true;
                    AccountWithdrawalTag_value.Text = _deposAddress.Tag;
                }
            }), deposAddress);
        }

        private void setCurrencyValues(string crypto, decimal price) {
            if (pIR.accounts.ContainsKey(crypto)) {
                UIControls_Dict["IR"].Label_Dict[crypto + "_Account_Value"].Text = Utilities.FormatValue(price * pIR.accounts[crypto].AvailableBalance);
            }
        }


        public void drawAccountOrderBook(Tuple<decimal, List<string[]>> accountOrders, string pair) {

            synchronizationContext.Post(new SendOrPostCallback(o => {

                Tuple<decimal, List<string[]>> _accountOrders = (Tuple<decimal, List<string[]>>)o;

                if (AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency != pair) return;
                if (!DCEs["IR"].IR_OBs.ContainsKey(pair)) return;

                string volume = AccountOrderVolume_textbox.Text;

                AccountOrders_listview.Items.Clear();

                foreach (string[] lvi in _accountOrders.Item2) {
                    Tuple<string, string> pairTup = Utilities.SplitPair(pair);
                    AccountOrders_listview.Items.Add(new ListViewItem(new string[] { lvi[0], Utilities.FormatValue(decimal.Parse(lvi[1]), DCEs["IR"].currencyFiatDivision[pairTup.Item1], false), lvi[2], lvi[3], lvi[4] }));
                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].SubItems[1].Tag = lvi[1];  // need to store the price in an unformatted (and therefore parseable) format
                    if (lvi[5] == "true") {  // what a hack.  colourising any orders that are MINE
                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].ForeColor = Color.RoyalBlue;
                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.Yellow;
                    }
                }
                if (_accountOrders.Item1 == -1) {
                    AccountEstOrderValue_value.Text = "Not enough depth!";
                }
                else if (_accountOrders.Item1 == -2) {
                    AccountEstOrderValue_value.Text = "";
                }
                else {  // leave it alone if a limit order
                    if (AccountOrderType_listbox.SelectedIndex == 0) {
                        AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(_accountOrders.Item1);
                    }
                }

            }), accountOrders);

        }

        private void cryptoClicked(Label clickedLabel) {
            if (!pIR.marketBaiterActive) {  // can't let the crypto change while we're baitin'
              
                Label oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                oldLabel.ForeColor = Color.Black;
                oldLabel.Font = new Font(oldLabel.Font.FontFamily, 14.25f, FontStyle.Regular);

                oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
                oldLabel.ForeColor = Color.FromArgb(64, 64, 64);

                oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
                oldLabel.ForeColor = Color.FromArgb(64, 64, 64);


                AccountSelectedCrypto = clickedLabel.Text.Substring(0, clickedLabel.Text.IndexOf(':'));

                AccountOpenOrders_label.Text = AccountSelectedCrypto + " open orders";
                AccountOpenOrders_listview.Items.Clear();
                AccountOpenOrders_listview.Items.Add(new ListViewItem("Loading..."));

                AccountClosedOrders_label.Text = AccountSelectedCrypto + " closed orders";
                AccountClosedOrders_listview.Items.Clear();
                AccountClosedOrders_listview.Items.Add(new ListViewItem("Loading..."));

                if (AccountSelectedCrypto == "BTC") AccountSelectedCrypto = "XBT";
                pIR.Crypto = AccountSelectedCrypto;

                Label newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                newLabel.ForeColor = Color.DarkOrange;
                newLabel.Font = new Font(newLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

                newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
                newLabel.ForeColor = Color.DarkOrange;

                newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
                newLabel.ForeColor = Color.DarkOrange;
            }

            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { 
                PrivateIR.PrivateIREndPoints.GetAddress,PrivateIR.PrivateIREndPoints.GetClosedOrders,
                PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetAccounts, PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        private void IRAccountClose_button_Click(object sender, EventArgs e) {
            Main.Visible = true;  // this has to be above the UpdateLabel() call, because UpdateLabels() exits if main is invisible.
            // we stopped the UI from updating when the IR Accounts screen was showing, so let's update all the pairs now that we're closing the ACcounts page
            foreach (string dExchange in Exchanges) {
                UpdateLabels(dExchange);
            }

            LastPanel = Main;
            IRAccount_panel.Visible = false;

            Label CurrentSecondaryCurrecyLabel = UIControls_Dict["IR"].Label_Dict[DCEs["IR"].CurrentSecondaryCurrency + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.Black;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Regular);
        }

        private void AccountWithdrawalAddress_label_Click(object sender, EventArgs e) {
            Label address = (Label)sender;
            Clipboard.SetText(address.Text);
        }

        private void AccountWithdrawalNextCheck_label_Click(object sender, EventArgs e) {
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.CheckAddress }));
        }

        private void Account_label_Click(object sender, EventArgs e) {
            cryptoClicked((Label)sender);
        }

        // market order, limit order, market baiter list box
        private void AcccountOrderType_listbox_SelectedIndexChanged(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex == 1) {
                SwitchOrderBookSide_button.Enabled = true;
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
                pIR.OrderTypeStr = "Limit";
            }
            else if (AccountOrderType_listbox.SelectedIndex == 0) {
                SwitchOrderBookSide_button.Enabled = true;
                AccountLimitPrice_label.Visible = false;
                AccountLimitPrice_textbox.Visible = false;
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    AccountPlaceOrder_button.Text = "Buy now";
                }
                else {
                    AccountPlaceOrder_button.Text = "Sell now";
                }
                AccountLimitPrice_label.ForeColor = Color.Black;
                AccountPlaceOrder_button.ForeColor = Color.Black;
                IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                if (pIR != null) pIR.OrderTypeStr = "Market";
            }
            else if (AccountOrderType_listbox.SelectedIndex == 2) {  // market baiter!
                AccountPlaceOrder_button.Text = "Start baitin'";
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
                pIR.OrderTypeStr = "Limit";  // we can now place limit orders while baitin'

                // switch the order book to the side we're dealing in
                //SwitchOrderBookSide_button.Enabled = false;  // we're now monitoring this side, no changes allowed.
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    pIR.OrderBookSide = "Bid";
                    AccountOrders_listview.Columns[1].Text = "Bids";
                    AccountOrders_listview.BackColor = Color.Thistle;
                }
                else {
                    pIR.OrderBookSide = "Offer";
                    AccountOrders_listview.Columns[1].Text = "Offers";
                    AccountOrders_listview.BackColor = Color.PeachPuff;
                }
                pIR.OrderTypeStr = "Limit";
                Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
                //updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
            }
            AccountPlaceOrder_button.Enabled = VolumePriceParseable();
        }

        private void AccountBuySell_listbox_Click(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex < 2) {  // if we're baitin', then don't change the button
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    AccountPlaceOrder_button.Text = "Buy now";
                    AccountOrders_listview.Columns[1].Text = "Offers";
                    pIR.OrderBookSide = "Offer";
                    pIR.BuySell = "Buy";
                    AccountOrders_listview.BackColor = Color.PeachPuff;
                }
                else {
                    AccountPlaceOrder_button.Text = "Sell now";
                    AccountOrders_listview.Columns[1].Text = "Bids";
                    pIR.OrderBookSide = "Bid";
                    pIR.BuySell = "Sell";
                    AccountOrders_listview.BackColor = Color.Thistle;
                }
            }
            else {  // baitin'
                    // switch the order book to the side we're dealing in
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    pIR.OrderBookSide = "Bid";
                    AccountOrders_listview.Columns[1].Text = "Bids";
                    pIR.BuySell = "Buy";
                    AccountOrders_listview.BackColor = Color.Thistle;
                }
                else {
                    pIR.OrderBookSide = "Offer";
                    AccountOrders_listview.Columns[1].Text = "Offers";
                    pIR.BuySell = "Sell";
                    AccountOrders_listview.BackColor = Color.PeachPuff;
                }
                if (pIR.marketBaiterActive) {
                    if (AccountBuySell_listbox.SelectedIndex == 0) {
                        AccountPlaceOrder_button.Text = "Buy now";
                    }
                    else {
                        AccountPlaceOrder_button.Text = "Sell now";
                    }
                }
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
            }
            if ((AccountOrderType_listbox.SelectedIndex > 0) &&  //  limit or bait
                decimal.TryParse(AccountLimitPrice_textbox.Text, out decimal ignore)) ValidateLimitOrder();
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        private bool VolumePriceParseable() {
            int orderType = AccountOrderType_listbox.SelectedIndex;
            string volume = AccountOrderVolume_textbox.Text;
            string price = AccountLimitPrice_textbox.Text;
            if (orderType == 0) {   // market, only care about volume
                if (decimal.TryParse(volume, out decimal orderVol)) {
                    if (orderVol > 0) {
                        pIR.Volume = orderVol;
                        return true;
                    }
                }
                if (pIR != null) pIR.Volume = 0;
                return false;
            }
            else {  // limit order or market baiter, need to check both fields
                if (decimal.TryParse(price, out decimal orderPrice) && decimal.TryParse(volume, out decimal orderVol)) {
                    if ((orderVol > 0) && (orderPrice > 0)) {
                        pIR.Volume = orderVol;
                        pIR.LimitPrice = orderPrice;
                        return true;
                    }
                }
                pIR.Volume = pIR.LimitPrice = 0;
                return false;
            }
        }

        private void AccountOrderVolume_textbox_TextChanged(object sender, EventArgs e) {
            if (VolumePriceParseable()) {
                if (AccountOrderType_listbox.SelectedIndex == 0) {
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
                }
                else /*if (AccountOrderType_listbox.SelectedIndex == 1)*/ {  // limit order
                    AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(
                        decimal.Parse(AccountOrderVolume_textbox.Text) * decimal.Parse(AccountLimitPrice_textbox.Text));
                }
                AccountPlaceOrder_button.Enabled = true;
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
            }
        }

        private void StopBaitin_button_Click(object sender, EventArgs e) {
            if (!pIR.marketBaiterActive) return;  // this button should only be able to be clicked if we're baitin'
            pIR.marketBaiterActive = false;
            AccountPlaceOrder_button.Size = new Size(294, 39);
            StopBaitin_button.Visible = false;
            StopBaitin_button.Enabled = false;
        }

        private async void AccountPlaceOrder_button_Click(object sender, EventArgs e) {
            string orderSide = "";
            if (AccountBuySell_listbox.SelectedIndex == 0) orderSide = "buy";
            else orderSide = "sell";

            int oType = AccountOrderType_listbox.SelectedIndex;
            if (pIR.marketBaiterActive) oType = 1;  // if they click this button while baitin', then we treat it like a limit order

            DialogResult res = DialogResult.Cancel;
            if ((oType < 2) || pIR.marketBaiterActive) {  // assume limit order if we're baitin' and user tries to place a new order
                res = MessageBox.Show("Placing " + orderSide + " order!" + Environment.NewLine + Environment.NewLine +
                    "Size of order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOrderVolume_textbox.Text + Environment.NewLine +
                    (oType == 0 ? "" : oType == 1 ? Utilities.FirstLetterToUpper(orderSide) + " price: $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine : "") +
                    "Estimated value of order: " + Utilities.FormatValue(decimal.Parse(AccountEstOrderValue_value.Text), 2, false),
                    "Confirm order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
            else if ((oType == 2) && !pIR.marketBaiterActive) {  // start market baiter

                res = MessageBox.Show("Start the market baiter strategy?" + Environment.NewLine + Environment.NewLine +
                    "This will create a " + orderSide + " order that will automatically move with the best order " +
                    "on the market, never going beyond $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine + Environment.NewLine +
                    "Size of moving order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + Utilities.FormatValue(decimal.Parse(AccountOrderVolume_textbox.Text), -1, false),
                    "Confirm market baiter order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }

            if (res == DialogResult.OK) {

                AccountPlaceOrder_button.Enabled = false;
                AccountOrderVolume_textbox.Enabled = false;
                AccountLimitPrice_textbox.Enabled = false;
                if (oType == 2) {
                    AccountPlaceOrder_button.Size = new Size(170, 39);
                    StopBaitin_button.Enabled = true;
                    StopBaitin_button.Visible = true;
                }


                // no need to check if we can parse the volume value, we already checked in AccountOrderVolume_textbox_TextChanged
                if (oType == 0) {
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                    PrivateIR.PrivateIREndPoints.PlaceMarketOrder, PrivateIR.PrivateIREndPoints.GetClosedOrders, PrivateIR.PrivateIREndPoints.GetAccounts }, decimal.Parse(AccountOrderVolume_textbox.Text)));
                }
                else if (oType == 1)  {  // Limit order
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                    PrivateIR.PrivateIREndPoints.PlaceLimitOrder, PrivateIR.PrivateIREndPoints.GetOpenOrders,
                    PrivateIR.PrivateIREndPoints.GetClosedOrders, PrivateIR.PrivateIREndPoints.GetAccounts }, decimal.Parse(AccountOrderVolume_textbox.Text), decimal.Parse(AccountLimitPrice_textbox.Text)));
                }
                else if (oType == 2) {  // market baiter
                    // do something that starts the market baiter
                    if (AccountBuySell_listbox.SelectedIndex == 0) pIR.BaiterBookSide = "Bid";
                    else pIR.BaiterBookSide = "Offer";
                    pIR.marketBaiterActive = true;
                    //AccountPlaceOrder_button.Text = "Stop market baiter and cancel order";
                    AccountBuySell_listbox_Click(null, null); // simulate changing the buy/sell so we set the button name corretly
                    ValidateLimitOrder();
                    Text = "IR Ticker - Market Baiter Running...";  // this is the form title bar
                    AccountBuySell_listbox.Enabled = false;
                    AccountOrderType_listbox.Enabled = false;
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));  // build the baiterBook
                    await updateOBTask;  // the idea here is to await the completion of the pIR.compileAccountOrderBookAsync(...) method
                    startMarketBaiter(decimal.Parse(AccountOrderVolume_textbox.Text), decimal.Parse(AccountLimitPrice_textbox.Text));
                }

                // need to disable the button until we have a result
                AccountPlaceOrder_button.Enabled = true;
                AccountOrderVolume_textbox.Enabled = true;
                AccountLimitPrice_textbox.Enabled = true;
            }
        }

        private async void startMarketBaiter(decimal volume, decimal limitPrice) {

            await Task.Run(() => pIR.marketBaiterLoopAsync(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency, volume, limitPrice));

            ValidateLimitOrder();
            AccountBuySell_listbox.Enabled = true;
            AccountOrderType_listbox.Enabled = true;
            AccountPlaceOrder_button.Text = "Start baitin'";
            AccountPlaceOrder_button.Size = new Size(294, 39);
            StopBaitin_button.Visible = false;
            StopBaitin_button.Enabled = false;
            Text = "IR Ticker";
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetClosedOrders, PrivateIR.PrivateIREndPoints.GetAccounts, PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        //public void updateUIFromMarketBaiter(List<PrivateIR.PrivateIREndPoints> endPoints) {
            //synchronizationContext.Post(new SendOrPostCallback(o => {
       //         bulkSequentialAPICalls(/*(List<PrivateIR.PrivateIREndPoints>)o*/endPoints);  // we are in the market baiter htread here, stay here
            //}), endPoints);
        //}

        public void notificationFromMarketBaiter(Tuple<string, string> notifText, bool sendToTelegram = false) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                Tuple<string, string> notif = (Tuple<string, string>)o;
                showBalloon(notif.Item1, notif.Item2);
            }), notifText);

            if (sendToTelegram  && (TGBot != null)) {
                synchronizationContext.Post(new SendOrPostCallback(o => {
                    Tuple<string, string> notif = (Tuple<string, string>)o;
                    TGBot.SendMessage("*" + notif.Item1 + "*" + Environment.NewLine + "  " + notif.Item2);
                }), notifText);
            }
        }

        // this method checks the limit price, and if it would make the order a market order, then highlight buttons and shit
        // can only be called if AccountOrderType_listbox.SelectedIndex is 1 or 2 (limit or bait)
        private void ValidateLimitOrder() {
            //if (pIR.marketBaiterActive) return;  // we don't want to really look at anything if baitin'
            decimal price = decimal.Parse(AccountLimitPrice_textbox.Text);
            if (AccountOrders_listview.Items.Count > 0) {  // only continue if we have orders in the OB
                if (AccountBuySell_listbox.SelectedIndex == 0) {  // buy
                    if (price >= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountPlaceOrder_button.Text = "MARKET buy now";
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is higher than the lowest offer, this will be a market order!");
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) {
                            AccountPlaceOrder_button.Text = "Start baitin'";
                            AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Black;
                            IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                        }
                    }
                    else {
                        AccountPlaceOrder_button.Text = "Buy now";
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) { // actually ..
                            AccountPlaceOrder_button.Text = "Start baitin'";
                        }
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.ForeColor = Color.Black;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                    }
                }
                else {  // sell
                    if (price <= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountPlaceOrder_button.Text = "MARKET sell now";
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is lower than the higest bid, this will be a market order!");
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) {
                            AccountPlaceOrder_button.Text = "Start baitin'";
                            AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Black;
                            IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                        }
                    }
                    else {
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.Text = "Sell now";
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) { // actually ..
                            AccountPlaceOrder_button.Text = "Start baitin'";
                        }
                        AccountPlaceOrder_button.ForeColor = Color.Black;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                    }
                }
            }
        }

        private void AccountLimitPrice_textbox_TextChanged(object sender, EventArgs e) {
            if (VolumePriceParseable()) {
                AccountPlaceOrder_button.Enabled = true;
                ValidateLimitOrder();
                AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(
                    decimal.Parse(AccountOrderVolume_textbox.Text) * decimal.Parse(AccountLimitPrice_textbox.Text));
            }
            // if VolumePriceParseable() not true, but we can parse the price field on it's own, then we can still colour some UI elements
            else if (decimal.TryParse(AccountLimitPrice_textbox.Text, out decimal volume)) {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
                ValidateLimitOrder();
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
            }
        }

        private void AccountOpenOrders_listview_DoubleClick(object sender, EventArgs e) {
            DialogResult res = MessageBox.Show("Do you want to cancel this order?" + Environment.NewLine + Environment.NewLine +
                "Date created: " + AccountOpenOrders_listview.SelectedItems[0].SubItems[0].Text + Environment.NewLine +
                "Volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOpenOrders_listview.SelectedItems[0].SubItems[1].Text + Environment.NewLine +
                "Price: $ " + AccountOpenOrders_listview.SelectedItems[0].SubItems[2].Text + Environment.NewLine +
                "Outstanding volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOpenOrders_listview.SelectedItems[0].SubItems[3].Text,
                "Cancel order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes) {
                if (AccountOpenOrders_listview.SelectedItems.Count == 0) return;
                string orderGuid = ((BankHistoryOrder)AccountOpenOrders_listview.SelectedItems[0].Tag).OrderGuid.ToString();
                Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                    PrivateIR.PrivateIREndPoints.CancelOrder, PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetAccounts, PrivateIR.PrivateIREndPoints.UpdateOrderBook }, 0, 0, orderGuid));
            }
        }

        private void SwitchOrderBookSide_button_Click(object sender, EventArgs e) {
            if (pIR.OrderBookSide == "Bid") {
                pIR.OrderBookSide = "Offer";
                AccountOrders_listview.Columns[1].Text = "Offers";
                AccountOrders_listview.BackColor = Color.PeachPuff;
            }
            else {
                pIR.OrderBookSide = "Bid";
                AccountOrders_listview.Columns[1].Text = "Bids";
                AccountOrders_listview.BackColor = Color.Thistle;
            }

            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        private void showBalloon(string title, string body) {

            IRT_notification.Visible = true;

            if (title != null) {
                IRT_notification.BalloonTipTitle = title;
            }

            if (body != null) {
                IRT_notification.BalloonTipText = body;
            }

            IRT_notification.ShowBalloonTip(10000);
        }

        private void AccountOrderVolume_textbox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Tab) {
                AccountLimitPrice_textbox.SelectAll();
            }
        }

        private void AccountOrderVolume_label_DoubleClick(object sender, EventArgs e) {
            AccountOrderVolume_textbox.Text = pIR.accounts[AccountSelectedCrypto].AvailableBalance.ToString();
        }
    }
}
