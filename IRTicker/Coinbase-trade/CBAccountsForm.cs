using IRTicker.Coinbase_trade.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

// TODO
// store the updated balances in the accounts dictionary
// put a button next to the balance so that we can buy/sell max
// hide price field when market order selected
// maybe colour the buy/sell box differently depending on what's selected?
// remember what extra columns i want for closed orders!!!!!!!!!!! :(  (sad face because I CAN'T REMEMBER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!)

namespace IRTicker {
    public partial class CBAccountsForm : Form {

        private CBWebSockets _client;
        private string current_product_id;
        private string spreadTicker = "\\";
        ConcurrentDictionary<string, CB_Order> openOrders = new ConcurrentDictionary<string, CB_Order>();

        bool baiter_Active = false;

        public CBAccountsForm() {
            InitializeComponent();

            // Need to start Coinbase websockets.
        }

        private async void CBAccountsForm_Load(object sender, EventArgs e) {
            // Replace with your actual credentials:
            string apiKey = Properties.Settings.Default.CoinbaseAPIKey;
            string apiSecret = Properties.Settings.Default.CoinbaseAPISecret;       // base64 encoded  // ?? this was in the chatgpt code.  is it fine as is?
            string apiPassphrase = Properties.Settings.Default.CoinbasePassPhrase;
            current_product_id = "USDT-USD";

            string[] currencies = current_product_id.Split('-');

            CB_currency1_label.Text = currencies[0] + "...";
            CB_currency2_label.Text = currencies[1] + "...";

            CB_order_type_listbox.SelectedIndex = 0;
            CB_order_side_listbox.SelectedIndex = 0;

            CB_closed_orders_label.Text = "Closed orders (" + current_product_id + "):";
            CB_closed_orders_listview.Columns[3].Text = "Value (" + currencies[1] + ")";

            CB_baiter_i_TT.SetToolTip(CB_baiter_i_image_panel, "The Market baiter option will put an order at the spread," + Environment.NewLine + "and move your order with the spread.  It will keep doing this" + Environment.NewLine + "until you either cancel the order, or the volume has been filled.");
            CB_general_TT.SetToolTip(CB_volume_per_min_label, "Volume traded per minute on the selected side. Eg if 'Buy' is" + Environment.NewLine + "selected, value here will be the volume of taker offers (sell" + Environment.NewLine + "orders that cross the spread) per minute." + Environment.NewLine + Environment.NewLine + "If the text is grey we don't have enough data for the rate." + Environment.NewLine + "Black text indicates we have enough data for a reliable rate.");

            CB_pair_comboBox.Text = current_product_id;  // default
            CB_pair_comboBox.Enabled = false; // don't let them try and change it while loading

            try {
                _client = await CBWebSockets.CreateAsync(apiKey, apiSecret, apiPassphrase);
            }
            catch (Exception ex) {
                //MessageBox.Show("Failed to connect to Coinbase API");  // we already put a message up at the actual fail, don't need it here too
                this.Close();
                return;
            }

            _client.OnOrderBookUpdated += UpdateOrderBookUI;
            _client.OnOpenOrdersUpdated += UpdateOpenOrdersUI;
            _client.OnClosedOrdersUpdated += UpdateClosedOrdersUI;
            _client.OnFailedToLoad += CloseForm;
            _client.OnProductsUpdated += UpdateProductsComboBox;
            _client.OnFinishNetworkTasks += EnableProductComboBox;
            _client.OnUpdatedPairBalance += UpdateBalance;
            _client.OnBaiterComplete += BaiterComplete;
            _client.OnBaiterStarted += BaiterStarted;
            _client.OnVolumePerMinUpdate += UpdateVolumePerMin;

            await _client.Start(current_product_id, true, false);  // first true - tells the method to download the products list as this is the first time.  second false - do not ignore sockets, we want to start them
        }

        private void UpdateVolumePerMin(decimal volumePerMin_bids, decimal volumePerMin_asks, bool volumeVelocityRunForOneMin) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateVolumePerMin(volumePerMin_bids, volumePerMin_asks, volumeVelocityRunForOneMin)));
                return;
            }

            if (CB_order_side_listbox.SelectedIndex == 0) { // 
                CB_volume_per_min_label.Text = "V/m: " + Utilities.FormatValue(volumePerMin_bids);  // the bids value is how many market sells are happening
            }
            else {
                CB_volume_per_min_label.Text = "V/m: " + Utilities.FormatValue(volumePerMin_asks);
            }

            if (volumeVelocityRunForOneMin) {
                CB_volume_per_min_label.ForeColor = Color.Black;
            }
            else {
                CB_volume_per_min_label.ForeColor = Color.Gray;
            }
        }

        private void BaiterStarted(string order_id) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => BaiterStarted(order_id)));
                return;
            }

            Debug.Print("CB-trade-baiter - trying to set baiter order blue.  Order_id: " + order_id + ", number of open orders: " + CB_open_orders_listview.Items.Count);
            foreach (ListViewItem lvi in CB_open_orders_listview.Items) {
                CB_Order order = (CB_Order)lvi.Tag;
                Debug.Print("-- CB-trade-baiter - looking at order id: " + order.order_id + ", id: " + order.id);
                if (order.order_id == order_id) {
                    lvi.ForeColor = Color.Blue;
                    order.isBaiter = true;
                }
            }
        }

        private void BaiterComplete() {

            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => BaiterComplete()));
                return;
            }

            baiter_Active = false;
            CB_orderbook_panel.BackColor = Color.DimGray;
        }

        private void UpdateBalance(CB_Accounts currency1, CB_Accounts currency2) {

            if ((null == currency1) || (null == currency2)) return;
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateBalance(currency1, currency2)));
                return;
            }

            CB_currency1_value.Text = Utilities.FormatValue(currency1.available);
            CB_currency1_value.Tag = currency1.available;

            CB_currency2_value.Text = Utilities.FormatValue(currency2.available);
            CB_currency2_value.Tag = currency2.available;

            CB_currency1_label.Text = currency1.currency;
            CB_currency2_label.Text = currency2.currency;
        }

        private void CloseForm() {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => CloseForm()));
                return;
            }

            this.Close();
        }

        private void EnableProductComboBox() {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => EnableProductComboBox()));
                return;
            }
            CB_pair_comboBox.Enabled = true;
        }

        // firstLoad = true, then we set USDT-USD by default.
        private void UpdateProductsComboBox(List<CB_Products> pairs) {

            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateProductsComboBox(pairs)));
                return;
            }

            CB_pair_comboBox.Items.Clear();
            int count = 0;
            int default_selection = 0;
            // now let's build the pairs drop down menu on the CB form
            foreach (CB_Products pair in pairs) {
                if (!pair.trading_disabled && pair.status == "online") { 
                    CB_pair_comboBox.Items.Add(new ComboBoxItem_Product(pair.id, pair));
                    if (pair.id == "USDT-USD") default_selection = count;
                    count++;
                }
            }

            // now let's select USDT-USD as we'll probably be trading this
            CB_pair_comboBox.SelectedIndex = default_selection;
        }

        private class ComboBoxItem_Product {
            public string DisplayText { get; set; }
            public CB_Products Pair { get; set; }

            public ComboBoxItem_Product(string displayText, CB_Products pair) {
                DisplayText = displayText;
                Pair = pair;
            }

            // Override ToString to display the text in the ComboBox
            public override string ToString() {
                return DisplayText;
            }
        }

        private void CBAccountsForm_FormClosing(object sender, FormClosingEventArgs e) {
            _client.Stop();
        }


        private void UpdateOpenOrdersUI(ConcurrentDictionary<string, CB_Order> openOrders) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateOpenOrdersUI(openOrders)));
                return;
            }

            CB_open_orders_listview.Items.Clear();

            foreach (var order in openOrders) {
                // if we don't have "remaining_size" but we do have a filled_size, then we can use this to calculate the remaining_size :|
                decimal remaining_vol = order.Value.size;
                if (order.Value.remaining_size > 0) {
                    remaining_vol = order.Value.remaining_size;
                }
                else if (order.Value.filled_size > 0) {
                    order.Value.remaining_size = order.Value.size - order.Value.filled_size;
                    /*if (decimal.TryParse(order.Value.size, out decimal size_d)) {
                        if (decimal.TryParse(order.Value.filled_size, out decimal filled_size_d)) {
                            remaining_vol = (size_d - filled_size_d).ToString();
                            order.Value.remaining_size = remaining_vol;
                        }
                    }*/
                }
                ListViewItem lvi = new ListViewItem(new string[] { order.Value.product_id, Utilities.FormatValue(order.Value.price, GetPriceDPs()), Utilities.FormatValue(order.Value.size, GetSizeDPs()), Utilities.FormatValue(remaining_vol, GetSizeDPs()) });
                lvi.ToolTipText = buildTTtext(order.Value);
                lvi.Tag = order.Value;  // so when we want to cancel an order by double clicking the item, this is how we get the order id
                if (order.Value.side == "buy") {
                    lvi.BackColor = Color.Thistle;
                }
                else {
                    lvi.BackColor = Color.PeachPuff;
                }

                if (order.Value.isBaiter) {
                    lvi.ForeColor = Color.Blue;
                }
                CB_open_orders_listview.Items.Add(lvi);
            }
        }

        private string buildTTtext(CB_Order order, string price = "") {
            string tt = "";

            if (string.IsNullOrEmpty(price)) {
                price = Utilities.FormatValue(order.price, GetPriceDPs());
            }

            string[] currencies = current_product_id.Split('-');

            tt += (order.OrderType + " " + order.side).ToUpper() + Environment.NewLine +
                "Result: " + order.done_reason + Environment.NewLine +
                "Price: " + currencies[1] + " " + price + Environment.NewLine +
                "Filled size: " + currencies[0] + " " + Utilities.FormatValue(order.filled_size, GetSizeDPs()) + Environment.NewLine +
                "Original size: " + currencies[0] + " " + Utilities.FormatValue(order.size, GetSizeDPs()) + Environment.NewLine +
                "Value of trade: " + currencies[1] + " " + Utilities.FormatValue(order.executed_value, GetPriceDPs()) + Environment.NewLine +
                "Created on: " + order.created_at.ToLocalTime() + Environment.NewLine +
                "Fees paid: " + currencies[1] + " " + Utilities.FormatValue(order.fill_fees);

            return tt;
        }

        private void UpdateClosedOrdersUI(List<CB_Order> closedOrders) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateClosedOrdersUI(closedOrders)));
                return;
            }

            CB_closed_orders_listview.Items.Clear();

            int count = 0;
            foreach (var order in closedOrders) {
                string price;
                // need to find ouht what happens here - is order.price null? or 0  (it's 0)
                if (order.price == 0) {  // closed orders seem to have 0 price, we have to calculate the average price manually...
                    price = Utilities.FormatValue((order.executed_value / order.filled_size), GetPriceDPs());
                }
                else {
                    price = Utilities.FormatValue(order.price, GetPriceDPs());
                }

                ListViewItem lvi = new ListViewItem(new string[] {
                    order.done_at.ToLocalTime().ToShortDateString(),
                    price,
                    Utilities.FormatValue(order.filled_size, GetSizeDPs()),
                    Utilities.FormatValue(order.executed_value),
                    Utilities.FormatValue(order.size, GetSizeDPs()),
                    Utilities.FormatValue(order.fill_fees)
                });

                lvi.UseItemStyleForSubItems = false;  // needed to allow us to colour individual cell item (subitem) text.  Consequence is we can't set formatting per row, must now be done on every subitem

                // let's see if we can colour the volume differently if the filled != size
                if (order.filled_size < order.size) {
                    lvi.SubItems[2].ForeColor = Color.Red;
                    lvi.SubItems[4].ForeColor = Color.Red;
                }

                lvi.Tag = order;  // just in case
                lvi.ToolTipText = buildTTtext(order, price);

                foreach (ListViewItem.ListViewSubItem subitem in lvi.SubItems) {
                    subitem.BackColor = (order.side == "buy" ? Color.Thistle : Color.PeachPuff);
                }                    
      
                CB_closed_orders_listview.Items.Add(lvi);
                count++;
                if (count > 50) break;
            }
        }

        // For use with the Utilities.FormatValue() sub - decimalPlaces argument
        private int GetPriceDPs() {
            // get the decimals we need
            if (CB_pair_comboBox.SelectedIndex < 0) return -1;  // i guess no selection?
            string quote_accuracy = ((ComboBoxItem_Product)CB_pair_comboBox.Items[CB_pair_comboBox.SelectedIndex]).Pair.quote_increment.ToString();
            int? quote_accuracy_dps = Utilities.countDecimalPrecision(quote_accuracy);
            int quote_accuracy_dps_final = -1;
            if (quote_accuracy_dps.HasValue) {
                quote_accuracy_dps_final = quote_accuracy_dps.Value;
            }

            return quote_accuracy_dps_final;
        }
        /// <summary>
        /// Get's the currently selected pair's volume decimal places.  Eg if you can
        /// trade to 0.0001, the return value will be 4
        /// </summary>
        /// <returns>int, number of decimal places</returns>
        private int GetSizeDPs() {
            // get the decimals we need
            if (CB_pair_comboBox.SelectedIndex < 0) return -1;  // no selection in the crypto pair dropdown yet
            string base_accuracy = ((ComboBoxItem_Product)CB_pair_comboBox.Items[CB_pair_comboBox.SelectedIndex]).Pair.base_increment.ToString();
            int? base_accuracy_dps = Utilities.countDecimalPrecision(base_accuracy);
            int base_accuracy_dps_final = -1;
            if (base_accuracy_dps.HasValue) {
                base_accuracy_dps_final = base_accuracy_dps.Value;
            }

            return base_accuracy_dps_final;
        }

        private void UpdateOrderBookUI(IEnumerable<CB_OrderBookEntry> bids, IEnumerable<CB_OrderBookEntry> asks, ConcurrentDictionary<string, CB_Order> openOrders) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateOrderBookUI(bids, asks, openOrders)));
                return;
            }

            CB_asks_listview.Items.Clear();
            CB_bids_listview.Items.Clear();

            int priceDPs = GetPriceDPs();

            // convert openOrders into this hashset so it's easier to lookup
            var orderPrices = new HashSet<decimal>(openOrders.Values.Select(order => order.price));

            int count = 0;
            foreach (var b in bids) {

                ListViewItem lvi = new ListViewItem(new string[] { Utilities.FormatValue(b.Price, priceDPs), Utilities.FormatValue(b.Size) });
                if (orderPrices.Contains(b.Price)) {  // colour for own orders
                    lvi.BackColor = Color.DarkBlue;
                }

                lvi.SubItems[0].Tag = b.Price;

                CB_bids_listview.Items.Add(lvi);
                count++;
                if (count > 8) break;
            }

            count = 0;
            foreach (var a in asks) {
                ListViewItem lvi = new ListViewItem(new string[] { Utilities.FormatValue(a.Price, priceDPs), Utilities.FormatValue(a.Size) });
                
                if (orderPrices.Contains(a.Price)) {  // colour for own orders
                    lvi.BackColor = Color.DarkBlue;
                }

                lvi.SubItems[0].Tag = a.Price;

                CB_asks_listview.Items.Insert(0, lvi);
                count++;
                if (count > 8) break;
            }

            // update the spread label
            decimal bestBid = bids.FirstOrDefault().Price;
            decimal bestAsk = asks.FirstOrDefault().Price;

            switch (spreadTicker) {
                case "\\":
                    spreadTicker = "-";
                    break;
                case "-":
                    spreadTicker = "/";
                    break;
                case "/":
                    spreadTicker = "|";
                    break;
                case "|":
                    spreadTicker = "\\";
                    break;
            }

            CB_spread_label.Text = "Spread: " + Utilities.FormatValue(bestAsk - bestBid)  + "  " + spreadTicker;
            if (baiter_Active) CB_spread_label.Text += "  MB active!";
        }

        private async void CB_open_orders_listview_DoubleClick(object sender, EventArgs e) {

            CB_Order order = (CB_Order)CB_open_orders_listview.Items[CB_open_orders_listview.SelectedItems[0].Index].Tag;

            DialogResult res;
            try {
                res = MessageBox.Show("Do you want to cancel this order?" + Environment.NewLine + Environment.NewLine +
                    "Date created: " + order.created_at + Environment.NewLine +
                    "Volume: " + order.product_id + " " + order.size + Environment.NewLine +
                    "Price: $ " + order.price + Environment.NewLine +
                    "Outstanding volume: " + order.product_id + " " + order.remaining_size,
                    "Cancel order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            catch (Exception ex) {
                Debug.Print("CB-trade - tried to cancel an order but there were no orders selected somehow, silently failing... error: " + ex.Message);
                return;
            }

            if (res == DialogResult.Yes) {
                if (CB_open_orders_listview.SelectedItems.Count == 0) return;

                // ok, let's actually cancel the order.
                bool response = await _client.CB_cancel_order(order.id, true);  // true - user cancelled

                if (response == true) {
                    Debug.Print("CB-trade - seems the cancel order was successful for id: " + order.id);
                }
                else {
                    Debug.Print("CB-trade - cancel order failed for order id: " + order.id);
                    Debug.Print("-- CB-trade - response: " + response.ToString());
                    MessageBox.Show("Failed to cancel the order for some reason...");
                }
            }
        }

        private async void CB_pair_comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.Print("CB-trade - selected Index changed for pair drop down!");
            if (current_product_id == CB_pair_comboBox.Text) return;  // already selected
            current_product_id = CB_pair_comboBox.Text;

            CB_pair_comboBox.Enabled = false;
            // now we clear all listView controls and rebuild
            _client.Stop();
            CB_open_orders_listview.Items.Clear();
            CB_closed_orders_listview.Items.Clear();
            CB_asks_listview.Items.Clear();
            CB_bids_listview.Items.Clear();

            // loading!
            CB_open_orders_listview.Items.Add(new ListViewItem(new string[] { "Loading..."} ));
            CB_closed_orders_listview.Items.Add(new ListViewItem(new string[] { "Loading..." }));
            CB_asks_listview.Items.Add(new ListViewItem(new string[] { "Loading..." }));
            CB_bids_listview.Items.Add(new ListViewItem(new string[] { "Loading..." }));

            string[] currencies = current_product_id.Split('-');


            CB_currency1_label.Text = currencies[0] + "...";
            CB_currency2_label.Text = currencies[1] + "...";
            CB_currency1_value.Text = "...";
            CB_currency2_value.Text = "...";

            CB_closed_orders_label.Text = "Closed orders (" + current_product_id + "):";
            CB_closed_orders_listview.Columns[3].Text = "Value (" + currencies[1] + ")";

            //await Task.Delay(1000);
            await _client.Start(CB_pair_comboBox.Text, false, false);  // first false - do not download products list as we should already have it. second - do NOT ignore sockets, we want to reload them
        }

        private async void CB_place_order_button_Click(object sender, EventArgs e) {

            CB_place_order_button.Enabled = false;
            string side = (CB_order_side_listbox.SelectedIndex == 0 ? "buy" : "sell");

            decimal volume;
            // all orders need volume, let's make sure it's legit.
            if (!string.IsNullOrEmpty(CB_volume_textbox.Text)) {
                if (decimal.TryParse(CB_volume_textbox.Text, out decimal _volume)) {
                    volume = _volume;
                }
                else {
                    MessageBox.Show("volume not a number?");
                    CB_place_order_button.Enabled = true;
                    return;
                }
            }
            else {
                MessageBox.Show("volume empty?");
                CB_place_order_button.Enabled = true;
                return;
            }

            switch (CB_order_type_listbox.SelectedIndex) {
                case 0:  // limit order
                    if (!string.IsNullOrEmpty(CB_price_textbox.Text)) {
                        if (decimal.TryParse(CB_price_textbox.Text, out decimal price)) {

                            if ((volume > 0) && (price > 0)) {
                                var response = await _client.CB_place_order(current_product_id, side, price.ToString(), volume.ToString(), "limit", false);

                                if (!string.IsNullOrEmpty(response.ErrorMessage)) {
                                    MessageBox.Show("Error when placing order: " + Environment.NewLine + Environment.NewLine +  response.ErrorMessage);
                                }
                            }
                            else {
                                MessageBox.Show("volume or price < 0?");
                            }
                        }
                        else {
                            MessageBox.Show("price not a number?");
                        }
                    }
                    else {
                        MessageBox.Show("price empty?");
                    }
                    break;

                case 1:  // market order
                    if (volume > 0) {
                        var response = await _client.CB_place_order(current_product_id, side, "", volume.ToString(), "market", false);
                        if (!string.IsNullOrEmpty(response.ErrorMessage)) {
                            MessageBox.Show("Error when placing order: " + Environment.NewLine + Environment.NewLine + response.ErrorMessage);
                        }
                    }
                    else {
                        MessageBox.Show("volume < 0?");
                    }
                    break;

                case 2:  // market bait!
                        
                    if (baiter_Active) {
                        MessageBox.Show("Baiter already running, cannot run it twice");
                        CB_place_order_button.Enabled = true;
                        return;
                    }

                    if (volume > 0) {
                        baiter_Active = await _client.CB_start_baiter(current_product_id, side, volume);

                        if (baiter_Active) {
                            CB_orderbook_panel.BackColor = Color.DarkBlue;
                        }
                        else {
                            MessageBox.Show("Failed to start market baiter");
                        }
                    }
                    else {
                        MessageBox.Show("volume < 0?");
                    }
                    break;
            }
            CB_place_order_button.Enabled = true;
        }

        private void CB_asks_listview_Click(object sender, EventArgs e) {

            if (((ListView)sender).SelectedItems.Count == 0) return;
            CB_price_textbox.Text = ((ListView)sender).SelectedItems[0].Text;
        }

        private void CB_bids_listview_Click(object sender, EventArgs e) {
            if (((ListView)sender).SelectedItems.Count == 0) return;
            CB_price_textbox.Text = ((ListView)sender).SelectedItems[0].Text;
        }

        // hide price in market and market baiter options
        private void CB_order_type_listbox_SelectedIndexChanged(object sender, EventArgs e) {
            switch (CB_order_type_listbox.SelectedIndex) {
                case 0:
                    CB_price_label.Visible = true;
                    CB_price_textbox.Visible = true;
                    CB_price_spread_button.Visible = true;
                    break;

                case 1:
                case 2:
                    CB_price_label.Visible = false;
                    CB_price_textbox.Visible = false;
                    CB_price_spread_button.Visible = false;
                    break;
            }
        }

        private void CB_order_side_listbox_SelectedIndexChanged(object sender, EventArgs e) {
            if (CB_order_side_listbox.SelectedIndex == 0) {

            }
            else {

            }
        }

        private async void CB_refresh_button_Click(object sender, EventArgs e) {
            CB_pair_comboBox.Enabled = false;
            CB_open_orders_listview.Items.Clear();
            CB_closed_orders_listview.Items.Clear();
            CB_open_orders_listview.Items.Add(new ListViewItem(new string[] { "Loading..." }));
            CB_closed_orders_listview.Items.Add(new ListViewItem(new string[] { "Loading..." }));
            string[] currencies = current_product_id.Split('-');


            CB_currency1_label.Text = currencies[0] + "...";
            CB_currency2_label.Text = currencies[1] + "...";
            CB_currency1_value.Text = "...";
            CB_currency2_value.Text = "...";

            await _client.Start(current_product_id, false, true);
        }

        private void CB_AccAvgPrice_button_Click(object sender, EventArgs e) {
            var _AccAvgPrice = new AccAvgPrice(null, null, null, _client, enableAutoUpdate: true, crypto: current_product_id, fiat: "", direction: CB_order_side_listbox.SelectedIndex);
            _AccAvgPrice.Show();
        }

        private void CB_volume_max_button_Click(object sender, EventArgs e) {
            CB_volume_textbox.Text = ((decimal)CB_currency1_value.Tag).ToString();
        }

        private void CB_price_spread_button_Click(object sender, EventArgs e) {
            if (CB_order_side_listbox.SelectedIndex == 0) {  // buy
                var lvis = CB_bids_listview.Items;
                CB_price_textbox.Text = ((decimal)lvis[0].SubItems[0].Tag).ToString();  // the first subItem is the price.  The tag on the price contains a decial - the price (not in pretty string format)
            }
            else {  // sell
                var lvis = CB_asks_listview.Items;
                CB_price_textbox.Text = ((decimal)lvis[lvis.Count - 1].SubItems[0].Tag).ToString();
            }
        }
    }
}
