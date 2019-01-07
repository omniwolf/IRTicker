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

        public void UpdateOBs(ConcurrentDictionary<string, Tuple<ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>, ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>>> IR_OBs, string pair)
        {
            if (!IR_OBs.ContainsKey("XBT-AUD") || !IR_OBs.ContainsKey("ETH-AUD")) return;  // only try and update when we have a key..

            var Bids_Ordered = IR_OBs[pair].Item1.OrderByDescending(i => i.Key).ToArray();

            RichTextBox BidsTB = ((pair == "XBT-AUD") ? BidsTextBox : ETHBidsTextBox);
            RichTextBox OffersTB = ((pair == "XBT-AUD") ? OffersTextBox : ETHOffersTextBox);

            BidsTB.Clear();
            OffersTB.Clear();

            int count = 0;
            foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> bid in Bids_Ordered)
            {
                BidsTB.Text = BidsTB.Text + Environment.NewLine + bid.Key;
                count++;
                if (count > 20) break;
            }

            var Offers_Ordered = IR_OBs[pair].Item2.OrderBy(i => i.Key).ToArray();
            count = 0;
            foreach (KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> offer in Offers_Ordered)
            {
                OffersTB.Text = OffersTB.Text + Environment.NewLine + offer.Key;
                count++;
                if (count > 20) break;
            }
        }
    }
}
