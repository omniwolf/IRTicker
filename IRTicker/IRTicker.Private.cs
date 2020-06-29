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

namespace IRTicker {
    // holds code to do with the IR Accounts panel
    partial class IRTicker {

        private string AccountSelectedCrypto = "XBT";
        private string OrderBookSide = "Bid";  //  maintains which side of the order book we show in the AccountOrders_listview
        private bool marketBaiterActive = false;
        IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBook;
        ConcurrentBag<Guid> openOrderGuids = new ConcurrentBag<Guid>();

        private void InitialiseAccountsPanel() {
            AccountOrderVolume_textbox.Enabled = true;
            AccountLimitPrice_textbox.Enabled = true;
            bulkSequentialAPICalls(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetAccounts, PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAddress, PrivateIREndPoints.UpdateOrderBook });
        }

        private void DrawIRAccounts(Dictionary<string, Account> irAccounts) {
            IRAccount_panel.Visible = true;
            Main.Visible = false;

            Label CurrentSecondaryCurrecyLabel = UIControls_Dict["IR"].Label_Dict[DCEs["IR"].CurrentSecondaryCurrency + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.DarkBlue;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

            Label SelectedCrypto = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
            SelectedCrypto.ForeColor = Color.DarkOrange;
            SelectedCrypto.Font = new Font(SelectedCrypto.Font.FontFamily, 14.25f, FontStyle.Bold);

            var mSummaries = DCEs["IR"].GetCryptoPairs();

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
        }

        // runs these network calls in order
        private async void bulkSequentialAPICalls(List<PrivateIREndPoints> endPoints, decimal volume = 0, decimal price = 0) {

            foreach (PrivateIREndPoints endP in endPoints) {
                if (endP == PrivateIREndPoints.GetAccounts) {
                    Task<Dictionary<string, Account>> irAccountsTask = new Task<Dictionary<string, Account>>(pIR.GetAccounts);
                    irAccountsTask.Start();
                    Dictionary<string, Account> irAccounts = await irAccountsTask;
                    if (irAccounts == null) {
                        Debug.Print(DateTime.Now + " - there was an error, closing the accounts page");
                        Main.Visible = true;
                        IRAccount_panel.Visible = false;
                        return;
                    }
                    DrawIRAccounts(irAccounts);
                }
                else if (endP == PrivateIREndPoints.GetAddress) {
                    DigitalCurrencyDepositAddress addressData = await pIR.GetDepositAddress(AccountSelectedCrypto);
                    drawDepositAddress(addressData);
                }
                else if (endP == PrivateIREndPoints.CheckAddress) {
                    string address = AccountWithdrawalAddress_label.Text;
                    DigitalCurrencyDepositAddress addressData;
                    try {
                        addressData = await pIR.CheckAddressNow(AccountSelectedCrypto, address);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (addressData != null) drawDepositAddress(addressData);
                }
                else if (endP == PrivateIREndPoints.GetOpenOrders) {
                    var openOrders = await pIR.GetOpenOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency);
                    drawOpenOrders(openOrders.Data);
                }
                else if (endP == PrivateIREndPoints.GetClosedOrders) {
                    Page<BankHistoryOrder> closedOrders = await pIR.GetClosedOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency);
                    drawClosedOrders(closedOrders.Data);
                }
                else if (endP == PrivateIREndPoints.PlaceMarketOrder) {
                    OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.MarketBid : OrderType.MarketOffer;

                    BankOrder orderResult;
                    try {
                        orderResult = await pIR.PlaceMarketOrder(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency, oType, volume);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Market order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIREndPoints.PlaceLimitOrder) {
                    OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.LimitBid : OrderType.LimitOffer;

                    BankOrder orderResult;
                    try {
                        orderResult = await pIR.PlaceLimitOrder(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency, oType, price, volume);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Limit order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIREndPoints.CancelOrder) {
                    string orderGuid = ((BankHistoryOrder)AccountOpenOrders_listview.SelectedItems[0].Tag).OrderGuid.ToString();
                    BankOrder cancelledOrder;
                    try { 
                    cancelledOrder = await pIR.CancelOrder(orderGuid);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Debug.Print("cancelled order status: " + cancelledOrder.Status.ToString());
                }
                else if (endP == PrivateIREndPoints.UpdateOrderBook) {
                    updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
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
            tt += "Date created: " + order.CreatedTimestampUtc.ToString() + Environment.NewLine;
            tt += "Volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Volume + Environment.NewLine;
            if (isOrderOpen) {
                tt += "Price: $ " + Utilities.FormatValue(order.Price.Value) + Environment.NewLine;
                tt += "Outstanding volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Outstanding + Environment.NewLine;
            }

            else {
                tt += "Avg price: $ " + Utilities.FormatValue(order.AvgPrice.Value) + Environment.NewLine;
                tt += "Notional value: $ " + Utilities.FormatValue(order.Value.Value) + Environment.NewLine;
                tt += "Fee: " + Utilities.FormatValue(order.FeePercent, 2, false) + "%" + Environment.NewLine;
            }
            tt += "Status: " + order.Status;
            return tt;
        }

        private void drawClosedOrders(IEnumerable<BankHistoryOrder> closedOrders) {
            AccountClosedOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " closed orders";
            AccountClosedOrders_listview.Items.Clear();
            foreach (BankHistoryOrder order in closedOrders) {
                AccountClosedOrders_listview.Items.Add(new ListViewItem(new string[] {
                    order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                    Utilities.FormatValue(order.Volume),
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
        }

        private void drawOpenOrders(IEnumerable<BankHistoryOrder> openOrders) {
            AccountOpenOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " open orders";
            AccountOpenOrders_listview.Items.Clear();
            openOrderGuids = new ConcurrentBag<Guid>();
            foreach (BankHistoryOrder order in openOrders) {
                if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) return;
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
                openOrderGuids.Add(order.OrderGuid);
            }
        }

        private void drawDepositAddress(DigitalCurrencyDepositAddress deposAddress) {
            AccountWithdrawalCrypto_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " deposit address";

            AccountWithdrawalAddress_label.Text = deposAddress.DepositAddress;

            string nextCheck;
            if (deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime() < DateTime.Now) nextCheck = "ASAP";
            else nextCheck = deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime().ToString();
            AccountWithdrawalNextCheck_label.Text = "Next check: " + nextCheck;

            AccountWithdrawalLastCheck_label.Text = "Last checked: " + deposAddress.LastCheckedTimestampUtc.Value.ToLocalTime().ToString(); ;

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

        private Tuple<decimal, List<string[]>> compileAccountOrderBook(string pair, string side, string oType, string volume) {

            List<string[]> accOrderListView = new List<string[]>();
            decimal estValue = 0;

            // here we grab the buy or sell order book, make a copy, and then sort it
            if (OrderBookSide == "Offer") {
                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item2.ToArray();  // because we're buying from the sell orders
                orderedBook = arrayBook.OrderBy(k => k.Key);
                //Debug.Print("--- Account picked the sell side, top order is: " + orderedBook.First().Key);
            }
            else {
                KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item1.ToArray();  // because we're selling to the buy orders
                orderedBook = arrayBook.OrderByDescending(k => k.Key);
                //Debug.Print("--- Account picked the buy side, top order is: " + orderedBook.First().Key);
            }

            int count = 1;
            decimal cumulativeVol = 0;
            decimal cumulativeValue = 0;
            decimal totalOrderValue = 0;
            decimal trackedOrderVolume = -1;

            // if we can parse the volume box, and it's a market order, let's work out the order value.  No need to track for limit order, can just do simple maths
            if (decimal.TryParse(volume, out decimal orderVolParsed) && oType == "Market") {
                if (orderVolParsed > 0) {
                    trackedOrderVolume = orderVolParsed;
                }
            }

            foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> pricePoint in orderedBook) {
                decimal totalVolume = 0;
                bool includesMyOrder = false;

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
                    
                    if (openOrderGuids.Contains(new Guid(order.Key))) {
                        includesMyOrder = true;
                    }
                }

                if (count < 10) {  // less than 6 we haven't finished populating the listview yet
                    cumulativeVol += totalVolume;
                    cumulativeValue += pricePoint.Key * totalVolume;
                    accOrderListView.Add(new string[] { count.ToString(), pricePoint.Key.ToString(), Utilities.FormatValue(totalVolume), Utilities.FormatValue(cumulativeVol), Utilities.FormatValue(cumulativeValue), (includesMyOrder ? "true" : "false") });
                    count++;
                }
                // this can be read like: "if we've finished populating the listview, but we still have more orders required 
                // to calculate our market order size, then keep looping
                if ((count > 9) && (trackedOrderVolume <= 0)) break;
            }

            if ((oType == "Market") && (trackedOrderVolume >= 0)) {
                if (trackedOrderVolume > 0) {
                    estValue = -1; //"Not enough depth!";
                }
                else {
                    estValue = totalOrderValue;
                }
            }
            // if it's a limit order, then the AccountEstOrderValue field is calculated manually (no need for OB), so here we need to make sure we don't clear it
            // this else is saying "if it's a market order, but we didn't engage trackedOrderVolume, then they probably have unparsable text in the vol box, so clear the estimate value label"
            else if (oType == "Market") estValue = -2; // ""
            return new Tuple<decimal, List<string[]>>(estValue, accOrderListView);
        }

        public async void updateAccountOrderBook(string pair) {

            if (AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency != pair) return;
            if (!DCEs["IR"].IR_OBs.ContainsKey(pair)) return;

            string oType = "";
            if (AccountOrderType_listbox.SelectedIndex == 0) oType = "Market";
            else if (AccountOrderType_listbox.SelectedIndex == 1) oType = "Limit";

            string volume = AccountOrderVolume_textbox.Text;

            Task<Tuple<decimal, List<string[]>>> AccountOrderUpdateTask = new Task<Tuple<decimal, List<string[]>>>(() => compileAccountOrderBook(pair, OrderBookSide, oType, volume));
            AccountOrderUpdateTask.Start();

            if (OrderBookSide == "Offer") {
                AccountOrders_listview.Columns[1].Text = "Offers";
            }
            else {
                AccountOrders_listview.Columns[1].Text = "Bids";
            }

            // here we run the task, and then await it, and parse the list

            Tuple<decimal, List<string[]>> AccountOrders = await AccountOrderUpdateTask;

            AccountOrders_listview.Items.Clear();

            foreach (string[] lvi in AccountOrders.Item2) {
                AccountOrders_listview.Items.Add(new ListViewItem(new string[] { lvi[0], Utilities.FormatValue(decimal.Parse(lvi[1]), 2), lvi[2], lvi[3], lvi[4] }));
                AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].SubItems[1].Tag = lvi[1];  // need to store the price in an unformatted (and therefore parseable) format
                if (lvi[5] == "true") {  // what a hack.  colourising any orders that are MINE
                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].ForeColor = Color.RoyalBlue;
                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.Yellow;
                }
            }
            if (AccountOrders.Item1 == -1) {
                AccountEstOrderValue_value.Text = "Not enough depth!";
            }
            else if (AccountOrders.Item1 == -2) {
                AccountEstOrderValue_value.Text = "";
            }
            else {  // leave it alone if a limit order
                if (AccountOrderType_listbox.SelectedIndex == 0) {
                    AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(AccountOrders.Item1);
                }
            }
        }

        private void cryptoClicked(Label clickedLabel) {
            if (!marketBaiterActive) {  // can't let the crypto change while we're baitin'
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
            }

            bulkSequentialAPICalls(new List<PrivateIREndPoints>() { 
                PrivateIREndPoints.GetAddress,PrivateIREndPoints.GetClosedOrders,
                PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });
        }

        private void IRAccountClose_button_Click(object sender, EventArgs e) {
            marketBaiterActive = false;
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
            bulkSequentialAPICalls(new List<PrivateIREndPoints>() { PrivateIREndPoints.CheckAddress });
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
            }
            else if (AccountOrderType_listbox.SelectedIndex == 2) {  // market baiter!
                AccountPlaceOrder_button.Text = "Start baitin'";
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value

                // switch the order book to the side we're dealing in
                SwitchOrderBookSide_button.Enabled = false;  // we're now monitoring this side, no changes allowed.
                if (AccountBuySell_listbox.SelectedIndex == 0) OrderBookSide = "Bid";
                else OrderBookSide = "Offer";
                updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
            }
            AccountPlaceOrder_button.Enabled = VolumePriceParseable();
        }

        private void AccountBuySell_listbox_Click(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex < 2) {  // if we're baitin', then don't change the button
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    AccountPlaceOrder_button.Text = "Buy now";
                    AccountOrders_listview.Columns[1].Text = "Offers";
                    OrderBookSide = "Offer";
                }
                else {
                    AccountPlaceOrder_button.Text = "Sell now";
                    AccountOrders_listview.Columns[1].Text = "Bids";
                    OrderBookSide = "Bid";
                }
            }
            if ((AccountOrderType_listbox.SelectedIndex > 0) &&  //  limit or bait
                decimal.TryParse(AccountLimitPrice_textbox.Text, out decimal ignore)) ValidateLimitOrder();
            updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
        }

        private bool VolumePriceParseable() {
            int orderType = AccountOrderType_listbox.SelectedIndex;
            string volume = AccountOrderVolume_textbox.Text;
            string price = AccountLimitPrice_textbox.Text;
            if (orderType == 0) {   // market, only care about volume
                if (decimal.TryParse(volume, out decimal orderVol)) {
                    if (orderVol > 0) return true;
                }
                return false;
            }
            else {  // limit order or market baiter, need to check both fields
                if (decimal.TryParse(price, out decimal orderPrice) && decimal.TryParse(volume, out decimal orderVol)) {
                    if ((orderVol > 0) && (orderPrice > 0)) return true;
                }
                return false;
            }
        }

        private void AccountOrderVolume_textbox_TextChanged(object sender, EventArgs e) {
            if (VolumePriceParseable()) {
                decimal volume = decimal.Parse(AccountOrderVolume_textbox.Text);
                if (AccountOrderType_listbox.SelectedIndex == 0) {
                    updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
                }
                else if (AccountOrderType_listbox.SelectedIndex == 1) {  // limit order
                    AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(
                        volume * decimal.Parse(AccountLimitPrice_textbox.Text));
                }
                AccountPlaceOrder_button.Enabled = true;
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
            }
        }

        private void AccountPlaceOrder_button_Click(object sender, EventArgs e) {
            string orderSide = "";
            if (AccountBuySell_listbox.SelectedIndex == 0) orderSide = "buy";
            else orderSide = "sell";

            DialogResult res = DialogResult.Cancel;
            if (AccountOrderType_listbox.SelectedIndex < 2) {
                res = MessageBox.Show("Placing " + orderSide + " order!" + Environment.NewLine + Environment.NewLine +
                    "Size of order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOrderVolume_textbox.Text + Environment.NewLine +
                    (AccountOrderType_listbox.SelectedIndex == 0 ? "" : AccountOrderType_listbox.SelectedIndex == 1 ? Utilities.FirstLetterToUpper(orderSide) + " price: $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine : "") +
                    "Estimated value of order: " + AccountEstOrderValue_value.Text,
                    "Confirm order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
            else if (AccountOrderType_listbox.SelectedIndex == 2) {  // market baiter
                if (marketBaiterActive) {
                    // cancel it
                    marketBaiterActive = false;
                }
                else {
                    res = MessageBox.Show("Start the market baiter strategy?" + Environment.NewLine + Environment.NewLine +
                        "This will create a " + orderSide + " order that will automatically move with the best order " +
                        "on the market, never going beyond $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine + Environment.NewLine +
                        "Size of moving order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOrderVolume_textbox.Text,
                        "Confirm market baiter order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }

            if (res == DialogResult.OK) {

                AccountPlaceOrder_button.Enabled = false;
                AccountOrderVolume_textbox.Enabled = false;
                AccountLimitPrice_textbox.Enabled = false;

                // no need to check if we can parse the volume value, we already checked in AccountOrderVolume_textbox_TextChanged
                if (AccountOrderType_listbox.SelectedIndex == 0) {
                    bulkSequentialAPICalls(new List<PrivateIREndPoints>() {
                    PrivateIREndPoints.PlaceMarketOrder, PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAccounts }, decimal.Parse(AccountOrderVolume_textbox.Text));
                }
                else if (AccountOrderType_listbox.SelectedIndex == 1)  {  // Limit order
                    bulkSequentialAPICalls(new List<PrivateIREndPoints>() {
                    PrivateIREndPoints.PlaceLimitOrder, PrivateIREndPoints.GetOpenOrders,
                    PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAccounts }, decimal.Parse(AccountOrderVolume_textbox.Text), decimal.Parse(AccountLimitPrice_textbox.Text));
                }
                else if (AccountOrderType_listbox.SelectedIndex == 2) {  // market baiter
                    // do something that starts the market baiter
                    marketBaiterActive = true;
                    AccountPlaceOrder_button.Text = "Stop market baiter and cancel order";
                    AccountBuySell_listbox.Enabled = false;
                    AccountOrderType_listbox.Enabled = false;
                    startMarketBaiter(decimal.Parse(AccountOrderVolume_textbox.Text), decimal.Parse(AccountLimitPrice_textbox.Text));
                }

                // need to disable the button until we have a result
                AccountPlaceOrder_button.Enabled = true;
                AccountOrderVolume_textbox.Enabled = true;
                AccountLimitPrice_textbox.Enabled = true;
            }
        }

        private async void startMarketBaiter(decimal volume, decimal limitPrice) {
            var firstPricepoint = orderedBook.First().Value;
            var firstOrder = firstPricepoint.ElementAt(0).Value;
            if (!firstOrder.OrderType.EndsWith(OrderBookSide)) {  // we had an order where the orderType was null ??? how? This means I have to check for null :/
                Debug.Print("MBAIT: wrong order book for market baiter?");
                AccountBuySell_listbox.Enabled = true;
                AccountOrderType_listbox.Enabled = true;
                AccountPlaceOrder_button.Text = "Start baitin'";
                return;                
            }
            Task marketBaiterLoopTask = new Task(() => marketBaiterLoop(volume, limitPrice));
            //marketBaiterLoopTask = marketBaiterLoop(volume, limitPrice);
            marketBaiterLoopTask.Start();
            Task.WaitAll(marketBaiterLoopTask);

            //if (marketBaiterLoopTask.IsCompleted) {
                AccountBuySell_listbox.Enabled = true;
                AccountOrderType_listbox.Enabled = true;
                AccountPlaceOrder_button.Text = "Start baitin'";
                bulkSequentialAPICalls(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAccounts, PrivateIREndPoints.UpdateOrderBook });
            //}
        }

        private async void marketBaiterLoop(decimal volume, decimal limitPrice) {
            string pair = AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency;
            string crypto = AccountSelectedCrypto;
            string fiat = DCEs["IR"].CurrentSecondaryCurrency;

            bool orderFilled = false;

            BankOrder placedOrder = null;
            decimal distanceFromTopOrder = DCEs["IR"].currencyFiatDivision[AccountSelectedCrypto] * 5;  // how far infront of the best order should we be?  will be different for different cryptos
            if (OrderBookSide == "Offer") distanceFromTopOrder = distanceFromTopOrder * -1;
                Debug.Print("MBAIT: distance from top: " + distanceFromTopOrder);
            while (marketBaiterActive) {

                //if ((orderedBook.First().Value).ElementAt(0).Value.OrderType.EndsWith(OrderBookSide)) {  // first make sure we have the right order book
                if (placedOrder == null) {  // no order.  let's create one.
                    Debug.Print(DateTime.Now + " - MBAIT: no bait guid, lets create it. Top order: " + orderedBook.First().Key);

                    decimal orderPrice;

                    // now we need to make sure this orderPrice is not bigger/smaller than the best offer/bid (ie turning the order into a market order)
                    orderPrice = orderedBook.First().Key + distanceFromTopOrder;
                    if (OrderBookSide == "Bid") {
                        Debug.Print("MBAIT: bid order price: " + orderPrice);
                        if (orderPrice > DCEs["IR"].IR_OBs[pair].Item2.Keys.Min()) {
                            Debug.Print("MBAIT: orderPrice (" + orderPrice + ") is greater than the lowest bid - " + DCEs["IR"].IR_OBs[pair].Item2.Keys.Min());
                            Thread.Sleep(10000);
                            continue;  // master while loop
                        }
                        else if (orderPrice > limitPrice) {
                            orderPrice = limitPrice;  // never go over the limitPrice
                            Debug.Print("MBAIT: order too high, limited to " + limitPrice);  
                        }
                    }
                    else {
                        Debug.Print("MBAIT: offer order price: " + orderPrice);
                        if (orderPrice < DCEs["IR"].IR_OBs[pair].Item1.Keys.Max()) {
                            Debug.Print("MBAIT: orderPrice (" + orderPrice + ") is less than the highest offer - " + DCEs["IR"].IR_OBs[pair].Item1.Keys.Max());
                            Thread.Sleep(10000);
                            continue;  // master while loop
                        }
                        else if (orderPrice < limitPrice) {
                            orderPrice = limitPrice;  // never go under the limitPrice
                            Debug.Print("MBAIT: order too low, limited to " + limitPrice); 
                        }
                    }
                    Debug.Print("MBAIT: placing order at " + orderPrice);
                    try {
                        placedOrder = await pIR.PlaceLimitOrder(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency,
                            (OrderBookSide == "Bid" ? OrderType.LimitBid : OrderType.LimitOffer), orderPrice, volume);
                    }
                    catch (Exception ex) {
                        Debug.Print("MBAIT: trid to create an order, but it failed: " + ex.Message);
                    }
                    updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { /*PrivateIREndPoints.PlaceLimitOrder,*/ PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });

                }
                else {  // an order is in play
                    // keeps track of how many pricePoint order dictionaries we have gone through.  the first is special - 
                    // if we find our order in the first it means we're still at the spread which is good.
                    int pricePointCount = 0;
                    bool foundOrder = false;

                    if (OrderBookSide == "Offer") {
                        KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item2.ToArray();  // because we're buying from the sell orders
                        orderedBook = arrayBook.OrderBy(k => k.Key);
                        //Debug.Print("--- Account picked the sell side, top order is: " + orderedBook.First().Key);
                    }
                    else {
                        KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] arrayBook = DCEs["IR"].IR_OBs[pair].Item1.ToArray();  // because we're selling to the buy orders
                        orderedBook = arrayBook.OrderByDescending(k => k.Key);
                        //Debug.Print("--- Account picked the buy side, top order is: " + orderedBook.First().Key);
                    }

                    foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> pricePoint in orderedBook) {
                        if (pricePoint.Value.ContainsKey(placedOrder.OrderGuid.ToString())) {
                            foundOrder = true;
                            if (pricePointCount > 0) {  // our order has been beaten by another. lez cancel and start again.  if == 0 then we're the top of the book, do nothing.
                                if (placedOrder.Price != limitPrice) {  // if we're at the limit price, just leave the order, do not cancel.
                                    Debug.Print("MBAIT: our order has been beaten.  cancelling it...");
                                    BankOrder bo = await pIR.CancelOrder(placedOrder.OrderGuid.ToString());
                                    if (bo.Status == OrderStatus.Cancelled) {
                                        Debug.Print("MBAIT: cancel order was successful");
                                        updateUIFromMarketBaiter(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });
                                        placedOrder = null;
                                    }
                                }
                                //else Debug.Print("MBAIT: our order is at the limit, just gonna leave it.  price: " + placedOrder.Price);
                            }
                            break;
                        }
                        pricePointCount++;
                    }
                    if (!foundOrder) {
                        Debug.Print("MBAIT: Our order doesn't exist in the OB, possibly filled? " + placedOrder.OrderGuid.ToString());
                        Page<BankHistoryOrder> bhos = await pIR.GetClosedOrders(crypto, fiat);
                        foreach (BankHistoryOrder bho in bhos.Data) {
                            if (bho.OrderGuid == placedOrder.OrderGuid) {
                                if (bho.Status == OrderStatus.Filled) {
                                    Debug.Print("MBAIT: our order got filled.  sweet.");
                                    orderFilled = true;
                                    break;  // closed orders loop
                                }
                            }
                        }
                        if (orderFilled) break;
                        Debug.Print("MBAIT: nope, order not filled.  maybe cancelled?  will recreate...");
                        placedOrder = null;
                    }
                }
                //Debug.Print("sleeping for " + (Properties.Settings.Default.UITimerFreq + 50).ToString());
                Thread.Sleep(Properties.Settings.Default.UITimerFreq + 50);  // refresh a bit slower than our OB updates, so any updates should be made before this loop tries to read them
            }

            if (!orderFilled && (placedOrder != null)) {
                Debug.Print("MBAIT: master loop finished, let's cancel the order if it still exists...");
                BankOrder bo;
                try {
                    bo = await pIR.CancelOrder(placedOrder.OrderGuid.ToString());
                }
                catch {
                    Debug.Print("MBAIT: couldn't cancel the order, I guess will try again?");
                    bo = new BankOrder() { Status = OrderStatus.Open };  // fake it for below if statement
                }
                if (bo.Status == OrderStatus.Cancelled) {
                    Debug.Print("MBAIT: cancel order was successful");
                    placedOrder = null;
                }
                else Debug.Print("MBAIT: couldn't cancel the order?? guid: " + bo.OrderGuid);
            }
            marketBaiterActive = false;
        }

        private void updateUIFromMarketBaiter(List<PrivateIREndPoints> endPoints) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                bulkSequentialAPICalls((List<PrivateIREndPoints>)o);
            }), endPoints);
        }

        // this method checks the limit price, and if it would make the order a market order, then highlight buttons and shit
        private void ValidateLimitOrder() {
            decimal price = decimal.Parse(AccountLimitPrice_textbox.Text);
            if (AccountOrders_listview.Items.Count > 0) {
                if (AccountBuySell_listbox.SelectedIndex == 0) {  // buy
                    if (price >= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountPlaceOrder_button.Text = (AccountOrderType_listbox.SelectedIndex == 1 ? "MARKET buy now" : "Start baitin'");
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = (AccountOrderType_listbox.SelectedIndex == 1 ? Color.Red : Color.Black);
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is higher than the lowest offer, this will be a market order!");
                    }
                    else {
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.Text = (AccountOrderType_listbox.SelectedIndex == 1 ? "Buy now" : "Start baitin'");
                        AccountPlaceOrder_button.ForeColor = Color.Black;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                    }
                }
                else {  // sell
                    if (price <= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountPlaceOrder_button.Text = (AccountOrderType_listbox.SelectedIndex == 1 ? "MARKET sell now" : "Start baitin'");
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = (AccountOrderType_listbox.SelectedIndex == 1 ? Color.Red : Color.Black);
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is lower than the higest bid, this will be a market order!");
                    }
                    else {
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.Text = (AccountOrderType_listbox.SelectedIndex == 1 ? "Sell now" : "Start baitin'");
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
                bulkSequentialAPICalls(new List<PrivateIREndPoints>() {
                    PrivateIREndPoints.CancelOrder, PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.UpdateOrderBook });
            }
        }

        private void SwitchOrderBookSide_button_Click(object sender, EventArgs e) {
            if (OrderBookSide == "Bid") OrderBookSide = "Offer";
            else OrderBookSide = "Bid";
            updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
        }

        enum PrivateIREndPoints {
            GetAccounts,
            GetAddress,
            GetOpenOrders,
            GetClosedOrders,
            CheckAddress,
            PlaceMarketOrder,
            PlaceLimitOrder,
            CancelOrder,
            UpdateOrderBook
        }
    }
}
