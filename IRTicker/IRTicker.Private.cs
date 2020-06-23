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
        private string OrderBookSide = "Bids";  //  maintains which side of the order book we show in the AccountOrders_listview

        private void InitialiseAccountsPanel() {
            AccountOrderVolume_textbox.Enabled = true;
            AccountLimitPrice_textbox.Enabled = true;
            bulkSequentialAPICalls(new List<PrivateIREndPoints>() { PrivateIREndPoints.GetAccounts, PrivateIREndPoints.GetOpenOrders, PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAddress });
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
        private async void bulkSequentialAPICalls(List<PrivateIREndPoints> endPoints) {

            foreach (PrivateIREndPoints endP in endPoints) {
                if (endP == PrivateIREndPoints.GetAccounts) {
                    Task<Dictionary<string, Account>> irAccountsTask = new Task<Dictionary<string, Account>>(pIR.GetAccounts);
                    irAccountsTask.Start();
                    Dictionary<string, Account> irAccounts = await irAccountsTask;
                    DrawIRAccounts(irAccounts);
                }
                else if (endP == PrivateIREndPoints.GetAddress) {
                    Task<DigitalCurrencyDepositAddress> updateDepositAddressTask = new Task<DigitalCurrencyDepositAddress>(() => pIR.GetDepositAddress(AccountSelectedCrypto));
                    updateDepositAddressTask.Start();
                    DigitalCurrencyDepositAddress addressData = await updateDepositAddressTask;
                    drawDepositAddress(addressData);
                }
                else if (endP == PrivateIREndPoints.CheckAddress) {
                    string address = AccountWithdrawalAddress_label.Text;
                    Task<DigitalCurrencyDepositAddress> CheckAddressTask = new Task<DigitalCurrencyDepositAddress>(() => pIR.CheckAddressNow(AccountSelectedCrypto, address));
                    CheckAddressTask.Start();
                    DigitalCurrencyDepositAddress addressData = await CheckAddressTask;
                    drawDepositAddress(addressData);
                }
                else if (endP == PrivateIREndPoints.GetOpenOrders) {

                    Task<Page<BankHistoryOrder>> updateOpenOrdersTask = new Task<Page<BankHistoryOrder>>(() => pIR.GetOpenOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency));
                    updateOpenOrdersTask.Start();
                    Page<BankHistoryOrder> openOrders = await updateOpenOrdersTask;
                    drawOpenOrders(openOrders.Data);
                }
                else if (endP == PrivateIREndPoints.GetClosedOrders) {
                    Task<Page<BankHistoryOrder>> updateClosedOrdersTask = new Task<Page<BankHistoryOrder>>(() => pIR.GetClosedOrders(AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency));
                    updateClosedOrdersTask.Start();
                    Page<BankHistoryOrder> closedOrders = await updateClosedOrdersTask;
                    drawClosedOrders(closedOrders.Data);
                }
                else if (endP == PrivateIREndPoints.PlaceMarketOrder) {
                    BankOrder orderResult;

                    decimal volume = decimal.Parse(AccountOrderVolume_textbox.Text);
                    OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.MarketBid : OrderType.MarketOffer;
                    Task<BankOrder> orderResultTask = new Task<BankOrder>(() => pIR.PlaceMarketOrder(AccountSelectedCrypto,
                        DCEs["IR"].CurrentSecondaryCurrency,
                        oType,
                        volume));
                    orderResultTask.Start();
                    orderResult = await orderResultTask;
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Market order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIREndPoints.PlaceLimitOrder) {
                    BankOrder orderResult;

                    decimal volume = decimal.Parse(AccountOrderVolume_textbox.Text);
                    decimal price = decimal.Parse(AccountLimitPrice_textbox.Text);
                    OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.LimitBid : OrderType.LimitOffer;
                    Task<BankOrder> orderResultTask = new Task<BankOrder>(() => pIR.PlaceLimitOrder(AccountSelectedCrypto,
                        DCEs["IR"].CurrentSecondaryCurrency,
                        oType,
                        price,
                        volume));
                    orderResultTask.Start();
                    orderResult = await orderResultTask;
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Limit order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIREndPoints.CancelOrder) {
                    string orderGuid = ((BankOrder)AccountOpenOrders_listview.SelectedItems[0].Tag).OrderGuid.ToString();
                    Task<BankOrder> cancelOrderTask = new Task<BankOrder>(() => pIR.CancelOrder(orderGuid));
                    cancelOrderTask.Start();
                    BankOrder cancelledOrder = await cancelOrderTask;
                    Debug.Print("cancelled order status: " + cancelledOrder.Status.ToString());
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
                    Utilities.FormatValue(order.AvgPrice.Value),
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
            foreach (BankHistoryOrder order in openOrders) {
                if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) return;
                AccountOpenOrders_listview.Items.Add(new ListViewItem(new string[] {
                    order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                    Utilities.FormatValue(order.Volume),
                    Utilities.FormatValue(order.Price.Value),
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
            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBook;
            if (OrderBookSide == "Offers") {
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
                    cumulativeValue += pricePoint.Key * totalVolume;
                    accOrderListView.Add(new string[] { count.ToString(), pricePoint.Key.ToString(), Utilities.FormatValue(totalVolume), Utilities.FormatValue(cumulativeVol), Utilities.FormatValue(cumulativeValue) });
                    count++;
                }
                // this can be read like: "if we've finished populating the listview, but we still have more orders required 
                // to calculate our market order size, then keep looping
                if ((count > 5) && (trackedOrderVolume <= 0)) break;
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

            if (OrderBookSide == "Offers") {
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

            bulkSequentialAPICalls(new List<PrivateIREndPoints>() { 
                PrivateIREndPoints.GetAddress,PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetOpenOrders });
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
            bulkSequentialAPICalls(new List<PrivateIREndPoints>() { PrivateIREndPoints.CheckAddress });
        }

        private void Account_label_Click(object sender, EventArgs e) {
            cryptoClicked((Label)sender);
        }

        private void AcccountOrderType_listbox_SelectedIndexChanged(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex == 1) {
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
            }
            else if (AccountOrderType_listbox.SelectedIndex == 0) {
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
            AccountPlaceOrder_button.Enabled = VolumePriceParseable();
        }

        private void AccountBuySell_listbox_Click(object sender, EventArgs e) {
            if (AccountBuySell_listbox.SelectedIndex == 0) {
                AccountPlaceOrder_button.Text = "Buy now";
                AccountOrders_listview.Columns[1].Text = "Offers";
                OrderBookSide = "Offers";
            }
            else {
                AccountPlaceOrder_button.Text = "Sell now";
                AccountOrders_listview.Columns[1].Text = "Bids";
                OrderBookSide = "Bids";
            }
            if (AccountOrderType_listbox.SelectedIndex == 1) ValidateLimitOrder();
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
            else {  // limit order, need to check both fields
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

            DialogResult res = MessageBox.Show("Placing " + orderSide + " order!" + Environment.NewLine + Environment.NewLine +
                "Size of order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOrderVolume_textbox.Text + Environment.NewLine +
                (AccountOrderType_listbox.SelectedIndex == 0 ? "" : AccountOrderType_listbox.SelectedIndex == 1 ? Utilities.FirstLetterToUpper(orderSide) + " price: $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine : "") +
                "Estimated value of order: " + AccountEstOrderValue_value.Text,
                "Confirm order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

            if (res == DialogResult.OK) {

                AccountPlaceOrder_button.Enabled = false;
                AccountOrderVolume_textbox.Enabled = false;
                AccountLimitPrice_textbox.Enabled = false;

                // no need to check if we can parse the volume value, we already checked in AccountOrderVolume_textbox_TextChanged
                if (AccountOrderType_listbox.SelectedIndex == 0) {
                    bulkSequentialAPICalls(new List<PrivateIREndPoints>() {
                    PrivateIREndPoints.PlaceMarketOrder, PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAccounts });
                }
                else if (AccountOrderType_listbox.SelectedIndex == 1)  {  // Limit order
                    bulkSequentialAPICalls(new List<PrivateIREndPoints>() {
                    PrivateIREndPoints.PlaceLimitOrder, PrivateIREndPoints.GetOpenOrders,
                    PrivateIREndPoints.GetClosedOrders, PrivateIREndPoints.GetAccounts });
                }

                // need to disable the button until we have a result
                AccountPlaceOrder_button.Enabled = true;
                AccountOrderVolume_textbox.Enabled = true;
                AccountLimitPrice_textbox.Enabled = true;
            }
        }

        // this method checks the limit price, and if it would make the order a market order, then highlight buttons and shit
        private void ValidateLimitOrder() {
            decimal price = decimal.Parse(AccountLimitPrice_textbox.Text);
            if (AccountOrders_listview.Items.Count > 0) {
                if (AccountBuySell_listbox.SelectedIndex == 0) {  // buy
                    if (price >= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountLimitPrice_label.ForeColor = Color.Red;
                        AccountPlaceOrder_button.Text = "MARKET buy now";
                        AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is higher than the lowest offer, this will be a market order!");
                    }
                    else {
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.Text = "Buy now";
                        AccountPlaceOrder_button.ForeColor = Color.Black;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                    }
                }
                else {  // sell
                    if (price <= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountLimitPrice_label.ForeColor = Color.Red;
                        AccountPlaceOrder_button.Text = "MARKET sell now";
                        AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is lower than the higest bid, this will be a market order!");
                    }
                    else {
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.Text = "Sell now";
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
                    PrivateIREndPoints.CancelOrder, PrivateIREndPoints.GetOpenOrders });
            }
        }

        private void SwitchOrderBookSide_button_Click(object sender, EventArgs e) {
            if (OrderBookSide == "Bids") OrderBookSide = "Offers";
            else OrderBookSide = "Bids";
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
            CancelOrder
        }
    }
}
