using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;


namespace IRTicker
{
    public partial class OBview : Form
    {
        // maybe later keep track of the old IR_OBs object and compare order guids, and if the vol changes then highlight it?
        public OBview()
        {
            InitializeComponent();
        }

//        public void UpdateOBs(KeyValuePair<string, Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>>>[] IR_OBs, string pair)
        public void UpdateOBs(KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] buySide, 
            KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] sellSide, string pair) {

            if (buySide.Length == 0 || sellSide.Length == 0) return;

            RichTextBox BidsTB = ((pair == "XBT-AUD") ? BidsTextBox : ETHBidsTextBox);
            RichTextBox OffersTB = ((pair == "XBT-AUD") ? OffersTextBox : ETHOffersTextBox);

            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> Buy_OrderBook = buySide.OrderByDescending(k => k.Key);
            IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> Sell_OrderBook = sellSide.OrderBy(j => j.Key);

            if (pair == "XBT-AUD") {
                BidTopGuid_InputBox.Text = "";  // blank it first
                ConcurrentDictionary<string, DCE.OrderBook_IR> topPrice = Buy_OrderBook.FirstOrDefault().Value;  // get the top price
                foreach(KeyValuePair<string, DCE.OrderBook_IR> price in topPrice) {
                    BidTopGuid_InputBox.Text += price.Value.OrderGuid + ", ";  // append each order at this top price
                }

                OfferTopGuid_InputBox.Text = "";  // blank it first
                topPrice = Sell_OrderBook.FirstOrDefault().Value;  // get the top price
                foreach (KeyValuePair<string, DCE.OrderBook_IR> price in topPrice) {
                    OfferTopGuid_InputBox.Text += price.Value.OrderGuid + ", ";  // append each order at this top price
                }
            }

            WriteOBView(Buy_OrderBook, BidsTB);
            WriteOBView(Sell_OrderBook, OffersTB);
        }

        private void WriteOBView(IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> OrderBook, RichTextBox RTB) {
            RTB.Clear();
            decimal RunningVol = 0;
            int count = 0;
            foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> PriceLevel in OrderBook) {
                decimal IndividualVol = 0;
                foreach (var order in PriceLevel.Value) {
                    if (PriceLevel.Key == 0) continue;
                    IndividualVol += order.Value.Volume;
                }
                RunningVol += IndividualVol;
                RTB.Text += PriceLevel.Key.ToString("####0.00") + "  |  " + IndividualVol.ToString("#######0.00000000") + "  |  " + RunningVol.ToString("#######0.00000000") + Environment.NewLine;
                count++;
                if (count > 10) break;
            }
        }
    }
}
