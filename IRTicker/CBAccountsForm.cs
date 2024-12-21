using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            _client.Start();
        }

        private void CBAccountsForm_FormClosing(object sender, FormClosingEventArgs e) {
            _client.Stop();
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
    }
}
