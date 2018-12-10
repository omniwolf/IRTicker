using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IRTicker {
    class Utilities {
        /// <summary>
        /// just removes literally one character from either end
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimEnds(string value) {
            value = value.Remove(0, 1);
            return value.Remove(value.Length - 1, 1);
        }

        // this list will be sorted from earliest time to latest time
        public static Color PriceColour(List<Tuple<DateTime, decimal>> priceList) {

            // if we don't have enough data, just go black.
            if (priceList == null) return Color.Black;
            if (priceList.Count < 5) return Color.Black;
            if (priceList.Last().Item1 - priceList.First().Item1 < TimeSpan.FromMinutes(5)) return Color.Black;

            lock (priceList) {
                foreach (Tuple<DateTime, decimal> pricePoint in priceList) {
                    if (pricePoint.Item1 >= DateTime.Now - TimeSpan.FromMinutes(5)) {
                        decimal lastPrice = priceList.Last().Item2;
                        if (lastPrice > pricePoint.Item2 * 1.01m) {
                            // colour dark green
                            //return Color.Lime;
                            return Color.Lime;
                        }
                        else if (lastPrice > pricePoint.Item2 * 1.005m) {
                            // colour light green etc
                            //return Color.PaleGreen;
                            return Color.MediumAquamarine;
                        }
                        else if (lastPrice < pricePoint.Item2 * 0.99m) {
                            // colur red
                            return Color.Red;
                        }
                        else if (lastPrice < pricePoint.Item2 * 0.995m) {
                            // colour light red
                            return Color.LightCoral;
                        }
                        else {  // anything between 99.5% and 100.5% is not much movement, so we say black.
                            return Color.Black;
                        }
                    }
                }
            }
            return Color.Black;
        }

        /// <summary>
        /// takes a pair with a - separater like XBT-AUD, and returns a tuple (string, string) of each currency.  item1 in the tuple is what's before the '-', item2 is what's after.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static Tuple<string, string> SplitPair(string pair) {
            string primary = pair.Substring(0, pair.IndexOf('-'));
            string secondary = pair.Substring(pair.IndexOf('-') + 1, pair.Length - pair.IndexOf('-') - 1);
            return new Tuple<string, string>(primary, secondary);
        }

        public static void ColourDCETags(System.Windows.Forms.Control.ControlCollection controls, string dExchange) {
            foreach (System.Windows.Forms.Control ctrl in controls) {
                if (ctrl.Tag != null)
                    if (ctrl.Tag.ToString().Contains(dExchange)) {
                        ctrl.ForeColor = Color.Gray;
                    }

                if (ctrl.HasChildren)
                    ColourDCETags(ctrl.Controls, dExchange); //Recursively check all children controls as well; ie groupboxes or tabpages
            }
        }
    }
}
