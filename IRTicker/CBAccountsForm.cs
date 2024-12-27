using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IRTicker.Balance;
using static IRTicker.CBWebSockets;

// TODO
// colour open and closed buy and sell differently
// show pair in open orders, or maybe filter out only that currency? hmm
// show price for market orders in closed orders
// store the updated balances in the accounts dictionary

namespace IRTicker {
    public partial class CBAccountsForm : Form {

        private CBWebSockets _client;
        private string current_product_id;
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

            CB_pair_comboBox.Text = current_product_id;  // default

            _client = new CBWebSockets(apiKey, apiSecret, apiPassphrase);
            _client.OnOrderBookUpdated += UpdateOrderBookUI;
            _client.OnOpenOrdersUpdated += UpdateOpenOrdersUI;
            _client.OnClosedOrdersUpdated += UpdateClosedOrdersUI;
            _client.OnFailedToLoad += CloseForm;
            _client.OnProductsUpdated += UpdateProductsComboBox;
            _client.OnFinishNetworkTasks += EnableProductComboBox;
            _client.OnUpdatedPairBalance += UpdateBalance;

            await _client.Start(current_product_id);

            string[] currencies = current_product_id.Split('-');

            CB_currency1_label.Text = currencies[0] + "...";
            CB_currency2_label.Text = currencies[1] + "...";

            CB_order_type_listbox.SelectedIndex = 0;
            CB_order_side_listbox.SelectedIndex = 0;
        }

        private void UpdateBalance(CB_Accounts currency1, CB_Accounts currency2) {

            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateBalance(currency1, currency2)));
                return;
            }

            // first we convert the strings to decimals... before converting back to pretty strings :/

            if (decimal.TryParse(currency1.available, out decimal c1_dec)) {
                try {
                    CB_currency1_value.Text = Utilities.FormatValue(c1_dec);
                }
                catch (Exception ex) {
                    Debug.Print("CB-trade - couldn't format new balance number");
                }

                if (decimal.TryParse(currency2.available, out decimal c2_dec)) {
                    CB_currency2_value.Text = Utilities.FormatValue(c2_dec);
                }
                else {
                    Debug.Print("CB-trade - can't convert " + currency2.currency + " to decimal?  trying to convert: " + currency2.balance);
                }
            }
            else {
                Debug.Print("CB-trade - can't convert " + currency1.currency + " to decimal?  trying to convert: " + currency1.balance);
            }

            CB_currency1_label.Text = currency1.currency;
            CB_currency2_label.Text = currency2.currency;
        }

        private void CloseForm() {
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
        private void UpdateProductsComboBox(List<Products> pairs) {

            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateProductsComboBox(pairs)));
                return;
            }

            CB_pair_comboBox.Items.Clear();
            int count = 0;
            int default_selection = 0;
            // now let's build the pairs drop down menu on the CB form
            foreach (Products pair in pairs) {
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
            public Products Pair { get; set; }

            public ComboBoxItem_Product(string displayText, Products pair) {
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


        private void UpdateOpenOrdersUI(ConcurrentDictionary<string, Order> openOrders) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateOpenOrdersUI(openOrders)));
                return;
            }

            CB_open_orders_listview.Items.Clear();

            foreach (var order in openOrders) {
                // if we don't have "remaining_size" but we do have a filled_size, then we can use this to calculate the remaining_size :|
                string remaining_vol = order.Value.size;
                if (!string.IsNullOrEmpty(order.Value.remaining_size)) {
                    remaining_vol = order.Value.remaining_size;
                }
                else if (!string.IsNullOrEmpty(order.Value.filled_size)) {
                    if (decimal.TryParse(order.Value.size, out decimal size_d)) {
                        if (decimal.TryParse(order.Value.filled_size, out decimal filled_size_d)) {
                            remaining_vol = (size_d - filled_size_d).ToString();
                            order.Value.remaining_size = remaining_vol;
                        }
                    }
                }
                ListViewItem lvi = new ListViewItem(new string[] { order.Value.created_at.ToShortDateString(), order.Value.price, order.Value.size, remaining_vol });
                lvi.Tag = order.Value;  // so when we want to cancel an order by double clicking the item, this is how we get the order id
                if (order.Value.side == "buy") {
                    lvi.BackColor = Color.Thistle;
                }
                else {
                    lvi.BackColor = Color.PeachPuff;
                }
                CB_open_orders_listview.Items.Add(lvi);
            }
        }

        private void UpdateClosedOrdersUI(List<Order> openOrders) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateClosedOrdersUI(openOrders)));
                return;
            }

            CB_closed_orders_listview.Items.Clear();

            int count = 0;
            foreach (var order in openOrders) {

                ListViewItem lvi = new ListViewItem(new string[] { order.done_at.ToShortDateString(), order.price, order.size, order.executed_value });
                lvi.Tag = order;  // just in case

                if (order.side == "buy") {
                    lvi.BackColor = Color.Thistle;
                }
                else {
                    lvi.BackColor = Color.PeachPuff;
                }
                CB_closed_orders_listview.Items.Add(lvi);
                count++;
                if (count > 4) break;
            }
        }


        private void UpdateOrderBookUI(IEnumerable<OrderBookEntry> bids, IEnumerable<OrderBookEntry> asks) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateOrderBookUI(bids, asks)));
                return;
            }

            CB_asks_listview.Items.Clear();
            CB_bids_listview.Items.Clear();

            int count = 0;
            foreach (var b in bids) {
                CB_bids_listview.Items.Add(new ListViewItem(new string[] { Utilities.FormatValue(b.Price), Utilities.FormatValue(b.Size) }));
                count++;
                if (count > 10) break;
            }

            count = 0;
            foreach (var a in asks) {
                CB_asks_listview.Items.Insert(0, new ListViewItem(new string[] { Utilities.FormatValue(a.Price), Utilities.FormatValue(a.Size) }));
                count++;
                if (count > 10) break;
            }
        }

        private async void CB_open_orders_listview_DoubleClick(object sender, EventArgs e) {

            Order order = (Order)CB_open_orders_listview.Items[CB_open_orders_listview.SelectedItems[0].Index].Tag;

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
                var response = await CoinbaseClient.CB_cancel_order(order.id);

                if (response != null) {
                    if (response == "\"" + order.id + "\"") {
                        Debug.Print("CB-trade - seems the cancel order was successful for id: " + order.id);
                    }
                    else {
                        Debug.Print("CB-trade - cancel order failed for order id: " + order.id);
                        Debug.Print("-- CB-trade - response: " + response.ToString());
                    }
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

            //await Task.Delay(1000);
            await _client.Start(CB_pair_comboBox.Text);
        }

        private async void CB_place_order_button_Click(object sender, EventArgs e) {
            string side = (CB_order_side_listbox.SelectedIndex == 0 ? "buy" : "sell");
            
            string type = "";
            switch (CB_order_type_listbox.SelectedIndex) {
                case 0:
                    type = "limit";
                    break;
                case 1:
                    type = "market";
                    break;
                case 3:
                    // market bait!
                    break;
            }

            if (string.IsNullOrEmpty(type)) {
                return;
            }

            // check volume
            if (!string.IsNullOrEmpty(CB_volume_textbox.Text)) {
                if (decimal.TryParse(CB_volume_textbox.Text, out decimal volume)) {
                    if (type == "limit") {
                        if (!string.IsNullOrEmpty(CB_price_textbox.Text)) {
                            if (decimal.TryParse(CB_price_textbox.Text, out decimal price)) {

                                if ((volume > 0) && (price > 0)) {
                                    var response = await CoinbaseClient.CB_post_order(current_product_id, side, price.ToString(), volume.ToString(), type);
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
                    }
                    else if (type == "market") {
                        if (volume > 0) {
                            var response = await CoinbaseClient.CB_post_order(current_product_id, side, "", volume.ToString(), type);
                        }
                        else {
                            MessageBox.Show("volume < 0?");
                        }
                    }
                }
                else {
                    MessageBox.Show("volume not a number?");
                }
            }
            else {
                MessageBox.Show("volume empty?");
            }
        }
    }
}
