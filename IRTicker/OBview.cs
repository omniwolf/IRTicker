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

        public void UpdateOBs(KeyValuePair<string, Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>>>[] IR_OBs, string pair)
        {
            RichTextBox BidsTB = ((pair == "XBT-AUD") ? BidsTextBox : ETHBidsTextBox);
            RichTextBox OffersTB = ((pair == "XBT-AUD") ? OffersTextBox : ETHOffersTextBox);

            for (int i = 0; i < IR_OBs.Count(); i++) {

                if (IR_OBs[i].Key == pair) {
                    //if (!IR_OBs.ContainsKey("XBT-AUD") || !IR_OBs.ContainsKey("ETH-AUD")) return;  // only try and update when we have a key..
                    WriteOBView(IR_OBs[i].Value.Item1.OrderByDescending(k => k.Key), BidsTB);

                    //if (!IR_OBs.ContainsKey("XBT-AUD") || !IR_OBs.ContainsKey("ETH-AUD")) return;  // only try and update when we have a key..
                    WriteOBView(IR_OBs[i].Value.Item2.OrderBy(k => k.Key), OffersTB);
                }
            }
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
