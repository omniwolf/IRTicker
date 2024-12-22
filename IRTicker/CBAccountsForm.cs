using IndependentReserve.DotNetClientApi.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IRTicker.CBWebSockets;

namespace IRTicker {
    public partial class CBAccountsForm : Form {

        private CBWebSockets _client;
        public CBAccountsForm() {
            InitializeComponent();

            // Need to start Coinbase websockets.
        }

        private void CBAccountsForm_Load(object sender, EventArgs e) {
            // Replace with your actual credentials:
            string apiKey = Properties.Settings.Default.CoinbaseAPIKey;
            string apiSecret = Properties.Settings.Default.CoinbaseAPISecret;       // base64 encoded  // ?? this was in the chatgpt code.  is it fine as is?
            string apiPassphrase = Properties.Settings.Default.CoinbasePassPhrase;

            _client = new CBWebSockets("USDT-USD", apiKey, apiSecret, apiPassphrase);
            _client.OnOrderBookUpdated += UpdateOrderBookUI;
            _client.OnOpenOrdersUpdated += UpdateOpenOrdersUI;
            _client.OnClosedOrdersUpdated += UpdateClosedOrdersUI;
            _client.Start();
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
                CB_open_orders_listview.Items.Add(lvi);
            }
        }

        private void UpdateClosedOrdersUI(ConcurrentDictionary<string, Order> openOrders) {
            if (this.InvokeRequired) {  // if we're not on the UI thread, call again from the UI thread
                this.BeginInvoke((Action)(() => UpdateClosedOrdersUI(openOrders)));
                return;
            }

            CB_closed_orders_listview.Items.Clear();

            int count = 0;
            foreach (var order in openOrders) {

                ListViewItem lvi = new ListViewItem(new string[] { order.Value.done_at.ToShortDateString(), order.Value.price, order.Value.size, order.Value.executed_value });
                lvi.Tag = order.Value;  // just in case
                CB_closed_orders_listview.Items.Add(lvi);
                count++;
                if (count > 5) break;
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

                CB_bids_listview.Items.Add(new ListViewItem(new string[] { b.Price.ToString(), b.Size.ToString() }));
                count++;
                if (count > 10) break;
            }

            count = 0;
            foreach (var a in asks) {
                CB_asks_listview.Items.Insert(0, new ListViewItem(new string[] { a.Price.ToString(), a.Size.ToString() }));
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
    }
}
