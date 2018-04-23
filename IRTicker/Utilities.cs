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
        public static string trimEnds(string value) {
            value = value.Remove(0, 1);
            return value.Remove(value.Length - 1, 1);
        }

        // this list will be sorted from earliest time to latest time
        public static Color priceColour(List<Tuple<DateTime, double>> priceList) {

            // if we don't have enough data, just go black.
            if (priceList == null) return Color.Black;
            if (priceList.Count < 5) return Color.Black;
            if (priceList.Last().Item1 - priceList.First().Item1 < TimeSpan.FromMinutes(5)) return Color.Black;

            foreach (Tuple<DateTime, double> pricePoint in priceList) {
                if (pricePoint.Item1 >= DateTime.Now - TimeSpan.FromMinutes(5)) {
                    double lastPrice = priceList.Last().Item2;
                    if(lastPrice > pricePoint.Item2 * 1.01) {
                        // colour dark green
                        return Color.Lime;
                    }
                    else if (lastPrice > pricePoint.Item2 * 1.005) {
                        // colour light green etc
                        return Color.PaleGreen;
                    }
                    else if (lastPrice < pricePoint.Item2 * 0.99) {
                        // colur red
                        return Color.Red;
                    }
                    else if (lastPrice < pricePoint.Item2 * 0.995) {
                        // colour light red
                        return Color.LightCoral;
                    }
                    else {  // anything between 99.5% and 100.5% is not much movement, so we say black.
                        return Color.Black;
                    }
                }
            }
            return Color.Black;
        }


    }
}
