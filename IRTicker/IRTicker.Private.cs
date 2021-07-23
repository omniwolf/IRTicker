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
        private AccAvgPrice _AccAvgPrice = null;

        private void InitialiseAccountsPanel() {
            AccountOrderVolume_textbox.Enabled = true;
            AccountLimitPrice_textbox.Enabled = true;
            Settings.Visible = false;
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
                CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 12f, FontStyle.Bold);

                Label SelectedCrypto = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                SelectedCrypto.ForeColor = Color.DarkOrange;
                SelectedCrypto.Font = new Font(SelectedCrypto.Font.FontFamily, 12f, FontStyle.Bold);

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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        Debug.Print(DateTime.Now + " - couldn't pull getAccounts pIR because: " + errorMsg);
                        irAccounts = null;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - GetAccounts", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        Debug.Print(DateTime.Now + " - failed to call GetDepositAddress properly: " + errorMsg);
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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - CheckAddress", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        Debug.Print("Bulk method: couldn't pull closed orders: " + errorMsg);
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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - Market order", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - Limit order", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - Cancel order", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void drawClosedOrders(IEnumerable<BankHistoryOrder> closedOrders) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                IEnumerable<BankHistoryOrder> _closedOrders = (IEnumerable<BankHistoryOrder>)o;

                if (_closedOrders == null) {
                    AccountClosedOrders_listview.Items.Clear();
                    AccountClosedOrders_listview.Items.Add(new System.Windows.Forms.ListViewItem("Loading..."));
                }
                else {
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
                    Utilities.FormatValue(vol, 8, false),
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
            }), closedOrders);
        }

        public void drawOpenOrders(IEnumerable<BankHistoryOrder> openOrders) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                IEnumerable<BankHistoryOrder> _openOrders = (IEnumerable<BankHistoryOrder>)o;

                if (_openOrders == null) {
                    AccountOpenOrders_listview.Items.Clear();
                    AccountOpenOrders_listview.Items.Add(new System.Windows.Forms.ListViewItem("Loading..."));
                }
                else {
                    AccountOpenOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " open orders";
                    AccountOpenOrders_listview.Items.Clear();

                    int loopCount = 1;
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
                        loopCount++;
                        if (loopCount > 7) break;  // we only need to draw 7 items
                    }
                }
            }), openOrders);
        }

        private void drawDepositAddress(DigitalCurrencyDepositAddress deposAddress) {
            if (deposAddress == null) {  // construct an empty address object and draw blanks, this is used when our data is old and we need to show nothing until the new data is sent
                deposAddress = new DigitalCurrencyDepositAddress();
                deposAddress.DepositAddress = "";
                deposAddress.NextUpdateTimestampUtc = null;
                deposAddress.LastCheckedTimestampUtc = null;

            }
            synchronizationContext.Post(new SendOrPostCallback(o => {
                DigitalCurrencyDepositAddress _deposAddress = (DigitalCurrencyDepositAddress)o;
                AccountWithdrawalCrypto_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " deposit address";

                AccountWithdrawalAddress_label.Text = _deposAddress.DepositAddress;

                string nextCheck = "";
                if (_deposAddress.NextUpdateTimestampUtc != null) {
                    if (_deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime() < DateTime.Now) nextCheck = "ASAP";
                    else nextCheck = _deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime().ToString();
                }
                AccountWithdrawalNextCheck_label.Text = "Next check: " + nextCheck;

                string lastChecked = "";
                if (_deposAddress.LastCheckedTimestampUtc != null) lastChecked = _deposAddress.LastCheckedTimestampUtc.Value.ToLocalTime().ToString();
                AccountWithdrawalLastCheck_label.Text = "Last checked: " + lastChecked;

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

        // how is this accountORders.item2 string array made up?
        // count, pricePoint (not formatted), totalVolume, cumulativeVol (not formatted), cumulativeValue, includesMyOrder 
        public void drawAccountOrderBook(Tuple<decimal, List<decimal[]>> accountOrders, string pair) {

            synchronizationContext.Post(new SendOrPostCallback(o => {

                Tuple<decimal, List<decimal[]>> _accountOrders = (Tuple<decimal, List<decimal[]>>)o;

                if (AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency != pair) return;
                if (!DCEs["IR"].IR_OBs.ContainsKey(pair)) return;

                AccountOrders_listview.Items.Clear();

                bool cumVolumeReached = false;  // We need to track the cum volume on the orders.  When we find a row that has higher cumulative volume than the form volume value, we need to highlight this one, but no further ones.  use this flag to show we no longer need to highlight orders

                foreach (decimal[] lvi in _accountOrders.Item2) {
                    Tuple<string, string> pairTup = Utilities.SplitPair(pair);
                    AccountOrders_listview.Items.Add(new ListViewItem(new string[] { lvi[0].ToString(), Utilities.FormatValue(lvi[1], DCEs["IR"].currencyFiatDivision[pairTup.Item1], false), Utilities.FormatValue(lvi[2], 8, false), Utilities.FormatValue(lvi[3]), Utilities.FormatValue(lvi[4]) }));
                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].SubItems[1].Tag = lvi[1];  // need to store the price in an unformatted (and therefore parseable) format

                    // if limit order or baiter, and can parse vol and limit price, and order book is showing the opposite side (ie if we're selling, and the OB is showing bids)
                    // if cumVol >= formVol then highlight

                    Tuple<bool, decimal, decimal> volPriceTup = VolumePriceParseable();

                    if (volPriceTup.Item1) {
                        if (((AccountBuySell_listbox.SelectedIndex == 0) && (pIR.OrderBookSide == "Offer")) ||
                            ((AccountBuySell_listbox.SelectedIndex == 1) && (pIR.OrderBookSide == "Bid"))) {
                            if (lvi[3] < volPriceTup.Item2) {
                                if (AccountOrderType_listbox.SelectedIndex > 0) {  // if we are have a limit or bait order type chosen, let's also stop highlighting at the price value
                                    if ((AccountBuySell_listbox.SelectedIndex == 0) && (lvi[1] <= volPriceTup.Item3) ||
                                        ((AccountBuySell_listbox.SelectedIndex == 1) && (lvi[1] >= volPriceTup.Item3))) {
                                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                                    }
                                    // if the price is beyond our limit price, then colour a different colour to signify that this is the price level the user
                                    // would need to enter if they wanted to fill this volume
                                    else {
                                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleVioletRed;
                                    }
                                }
                                else AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;  // else it's a market order, just highlight if the vol is good
                            }
                            else if (!cumVolumeReached) {  // we need to highlight one more row, as this will be the final order we'll eat into to fulfill our above order
                                if (AccountOrderType_listbox.SelectedIndex == 0) {  // if it's a market order, just colour it.  don't try and compare limit prices
                                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                                }
                                else if ((AccountBuySell_listbox.SelectedIndex == 0) && (lvi[1] <= volPriceTup.Item3) ||
                                ((AccountBuySell_listbox.SelectedIndex == 1) && (lvi[1] >= volPriceTup.Item3))) {
                                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                                }
                                // if the price is beyond our limit price, then colour a different colour to signify that this is the price level the user
                                // would need to enter if they wanted to fill this volume
                                else {
                                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleVioletRed;
                                }
                                cumVolumeReached = true;
                            }
                        }
                    }
                    else if (volPriceTup.Item3 >= 0) {  // vol not parsable, but price is.  let's colour some rows
                        if ((AccountBuySell_listbox.SelectedIndex == 0) && (lvi[1] <= volPriceTup.Item3) ||
                        ((AccountBuySell_listbox.SelectedIndex == 1) && (lvi[1] >= volPriceTup.Item3))) {
                            AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                        }
                    }
                    if (lvi[5] == 1) {  // what a hack.  colourising any orders that are MINE
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
                oldLabel.Font = new Font(oldLabel.Font.FontFamily, 12f, FontStyle.Bold);

                oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
                oldLabel.ForeColor = Color.FromArgb(64, 64, 64);

                oldLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
                oldLabel.ForeColor = Color.FromArgb(64, 64, 64);


                AccountSelectedCrypto = clickedLabel.Text.Substring(0, clickedLabel.Text.IndexOf(':'));

                AccountOpenOrders_label.Text = AccountSelectedCrypto + " open orders";
                drawOpenOrders(null);

                AccountClosedOrders_label.Text = AccountSelectedCrypto + " closed orders";
                drawClosedOrders(null);

                drawDepositAddress(null);

                if (AccountSelectedCrypto == "BTC") AccountSelectedCrypto = "XBT";
                pIR.SelectedCrypto = AccountSelectedCrypto;  // inform the pIR object what our selected crypto is

                Label newLabel = UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                newLabel.ForeColor = Color.DarkOrange;
                newLabel.Font = new Font(newLabel.Font.FontFamily, 12f, FontStyle.Bold);

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
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 12f, FontStyle.Regular);
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
            if ((null == pIR) || !IRAccount_panel.Visible) return;  // this sub seems to get called when the app opens.. 
            if (AccountOrderType_listbox.SelectedIndex == 1) {
                SwitchOrderBookSide_button.Enabled = true;
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
                pIR.OrderTypeStr = "Limit";
                AccountSwitchOrderBook(false);
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
                AccountSwitchOrderBook(false);
            }
            else if (AccountOrderType_listbox.SelectedIndex == 2) {  // market baiter!
                AccountPlaceOrder_button.Text = "Start baitin'";
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
                pIR.OrderTypeStr = "Limit";  // we can now place limit orders while baitin'

                // switch the order book to the side we're dealing in
                //SwitchOrderBookSide_button.Enabled = false;  // we're now monitoring this side, no changes allowed.
                AccountSwitchOrderBook(true);  // switches the OB shown
                pIR.OrderTypeStr = "Limit";
                //updateAccountOrderBook(AccountSelectedCrypto + "-" + DCEs["IR"].CurrentSecondaryCurrency);
            }
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));

            AccountPlaceOrder_button.Enabled = VolumePriceParseable().Item1;
        }

        // if they chose Market Baiter, then we do the opposite - a buy will show bids and a sell will show offers
        // if they clicked market or limit, then we show the other order book, ie if they have buy selected, then we show offers, and if the have sell selected then we show bids
        private void AccountSwitchOrderBook(bool switchToLimit) {
            int BuySellIndex = AccountBuySell_listbox.SelectedIndex;
            if (switchToLimit) {
                if (BuySellIndex == 0) BuySellIndex = 1;
                else BuySellIndex = 0;
            }
            if (BuySellIndex == 1) {
                pIR.OrderBookSide = "Bid";
                AccountOrders_listview.Columns[1].Text = "Bids";
                AccountOrders_listview.BackColor = Color.Thistle;
            }
            else {
                pIR.OrderBookSide = "Offer";
                AccountOrders_listview.Columns[1].Text = "Offers";
                AccountOrders_listview.BackColor = Color.PeachPuff;
            }
        }

        private void AccountBuySell_listbox_Click(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex < 2) {  // if we're baitin', then don't change the button
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    AccountPlaceOrder_button.Text = "Buy now";
                    pIR.BuySell = "Buy";
                }
                else {
                    AccountPlaceOrder_button.Text = "Sell now";
                    pIR.BuySell = "Sell";
                }
                AccountSwitchOrderBook(false);
            }
            else {  // baitin'
                    // switch the order book to the side we're dealing in
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    pIR.BuySell = "Buy";
                }
                else {
                    pIR.BuySell = "Sell";
                }
                AccountSwitchOrderBook(true);

                if (pIR.marketBaiterActive) {
                    if (AccountBuySell_listbox.SelectedIndex == 0) {
                        AccountPlaceOrder_button.Text = "Buy now";
                    }
                    else {
                        AccountPlaceOrder_button.Text = "Sell now";
                    }
                }
                //Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
            }
            if ((AccountOrderType_listbox.SelectedIndex > 0) &&  //  limit or bait
                decimal.TryParse(AccountLimitPrice_textbox.Text, out decimal ignore)) ValidateLimitOrder();
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        /// <summary>
        /// checks if the vol and price are parsable
        /// </summary>
        /// <returns>item1 is the bool, item2 is volume, item3 is the price</returns>
        private Tuple<bool, decimal, decimal> VolumePriceParseable() {
            int orderType = AccountOrderType_listbox.SelectedIndex;
            string volume = AccountOrderVolume_textbox.Text;
            string price = AccountLimitPrice_textbox.Text;
            if (orderType == 0) {   // market, only care about volume
                if (decimal.TryParse(volume, out decimal orderVol)) {
                    if (orderVol > 0) {
                        pIR.Volume = orderVol;
                        return new Tuple<bool, decimal, decimal>(true, orderVol, -1);
                    }
                }
                if (pIR != null) pIR.Volume = 0;
                return new Tuple<bool, decimal, decimal>(false, -1, -1);
            }
            else {  // limit order or market baiter, need to check both fields
                decimal orderPrice = -1;
                decimal orderVol = -1;
                bool canParsePrice = false;
                bool canParseVol = false;
                if (decimal.TryParse(price, out decimal _orderPrice)) {
                    orderPrice = _orderPrice;
                    canParsePrice = true;
                }
                else orderPrice = -1;
                if (decimal.TryParse(volume, out decimal _orderVol)) {
                    orderVol = _orderVol;
                    canParseVol = true;
                }
                else orderVol = -1;
                if (canParseVol && canParsePrice && (orderVol > 0) && (orderPrice >= 0)) {
                    pIR.Volume = orderVol;
                    pIR.LimitPrice = orderPrice;
                    return new Tuple<bool, decimal, decimal>(true, orderVol, orderPrice);
                }
                
                pIR.Volume = pIR.LimitPrice = 0;
                return new Tuple<bool, decimal, decimal>(false, orderVol, orderPrice);
            }
        }

        private void AccountOrderVolume_textbox_TextChanged(object sender, EventArgs e) {
            Tuple<bool, decimal, decimal> volPriceTup = VolumePriceParseable();

            if (volPriceTup.Item1) {
                if (AccountOrderType_listbox.SelectedIndex > 0) {  // limit or bait
                    AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(
                        volPriceTup.Item2 * volPriceTup.Item3);
                }
                AccountPlaceOrder_button.Enabled = true;
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
            }
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
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
                    "Estimated value of order: " + AccountEstOrderValue_value.Text,
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

                    // now ask if they want to start the avg price thingo
                    if ((_AccAvgPrice == null) || !Application.OpenForms.OfType<AccAvgPrice>().Any()) {
                        res = MessageBox.Show("Start recording the average order price?" + Environment.NewLine + Environment.NewLine +
                            "This will open up the Average Price Calculator window and enable the auto update setting",
                            "Average Price Calculator", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (res == DialogResult.Yes) {
                            _AccAvgPrice = new AccAvgPrice(DCEs["IR"], pIR, this, true, AccountSelectedCrypto, DCEs["IR"].CurrentSecondaryCurrency, AccountBuySell_listbox.SelectedIndex);
                            _AccAvgPrice.Show();
                        }
                    }

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
            //if (pIR.marketBaiterActive) return;  // we don't want to really look at anything if baitin'  // actually... we can now place limit orders while baitin'
            decimal price = decimal.Parse(AccountLimitPrice_textbox.Text);  // why no tryParse?  the only way this gets called really is if the price has been validated as a number, or it's the result of clicking the place order button, which is only clickable if the vol/price are validated.  so we should be safe here...
            if (AccountOrders_listview.Items.Count > 0) {  // only continue if we have orders in the OB
                if (AccountBuySell_listbox.SelectedIndex == 0) {  // buy
                    if (price >= decimal.Parse(AccountOrders_listview.Items[0].SubItems[1].Tag.ToString())) {
                        AccountPlaceOrder_button.Text = "Possible MARKET buy";
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is higher than the lowest offer, this will be a market order!");
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) {
                            AccountPlaceOrder_button.Text = "Start baitin'";
                            AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Black;
                            IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                        }
                    }
                    else {
                        AccountPlaceOrder_button.Text = "Place limit buy";
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
                        AccountPlaceOrder_button.Text = "Possible MARKET sell";
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
                        AccountPlaceOrder_button.Text = "Place limit sell";
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
            Tuple<bool, decimal, decimal> volPriceTup = VolumePriceParseable();

            if (volPriceTup.Item1) {
                AccountPlaceOrder_button.Enabled = true;
                ValidateLimitOrder();
                AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(
                    volPriceTup.Item2 * volPriceTup.Item3);
            }
            // if VolumePriceParseable() not true, but we can parse the price field on it's own, then we can still colour some UI elements
            else if (volPriceTup.Item3 >= 0) {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
                ValidateLimitOrder();
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
            }
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.UpdateOrderBook }));
        }

        private void AccountOpenOrders_listview_DoubleClick(object sender, EventArgs e) {
            DialogResult res;
            try {
                res = MessageBox.Show("Do you want to cancel this order?" + Environment.NewLine + Environment.NewLine +
                    "Date created: " + AccountOpenOrders_listview.SelectedItems[0].SubItems[0].Text + Environment.NewLine +
                    "Volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOpenOrders_listview.SelectedItems[0].SubItems[1].Text + Environment.NewLine +
                    "Price: $ " + AccountOpenOrders_listview.SelectedItems[0].SubItems[2].Text + Environment.NewLine +
                    "Outstanding volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOpenOrders_listview.SelectedItems[0].SubItems[3].Text,
                    "Cancel order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            catch (Exception ex) {
                Debug.Print("tried to cancel an order but there were no orders selected somehow, silently failing...");
                return;
            }

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
        private void AccountLimitPrice_textbox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Tab) {
                AccountLimitPrice_textbox.SelectAll();
            }
        }

        private void AccountOrderVolume_label_DoubleClick(object sender, EventArgs e) {
            AccountOrderVolume_textbox.Text = pIR.accounts[AccountSelectedCrypto].AvailableBalance.ToString();
        }
        private void IRAccount_AvgPrice_Button_Click(object sender, EventArgs e) {
            if ((_AccAvgPrice == null) || (!Application.OpenForms.OfType<AccAvgPrice>().Any())) {
                _AccAvgPrice = new AccAvgPrice(DCEs["IR"], pIR, this);
                _AccAvgPrice.Show();
            }
            else _AccAvgPrice.Focus();
        }

        // I had a crash here once, can't reproduce it.  instance not set to an object or something, but I couldn't see what was wrong.
        // maybe i should check that cOrders isn't somehow null?
        public void SignalAveragePriceUpdate(Page<BankHistoryOrder> cOrders) {
            synchronizationContext.Post(new SendOrPostCallback(o => {
                if (_AccAvgPrice != null) _AccAvgPrice.UpdatePrice((Page<BankHistoryOrder>)o);
            }), cOrders);
            
        }

        public void IRAccount_FillVolumeField(string vol) {
            AccountOrderVolume_textbox.Text = vol;
        }
    }
}
