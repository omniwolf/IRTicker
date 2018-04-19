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



        public static Color priceColour(List<Tuple<DateTime, double>> priceList) {
            foreach (Tuple<DateTime, double> pricePoint in priceList) {
                if (pricePoint.Item1 >= DateTime.Now - TimeSpan.FromMinutes(5)) {
                    Tuple<DateTime, double> lastPrice = priceList.Last();
                    if(lastPrice.Item2 > pricePoint.Item2 * 1.01) {
                        // colour dark green
                    }
                    else if (lastPrice.Item2 > pricePoint.Item2 * 1.005) {
                        // colour light green etc
                    }
                }
            }
        }


    }
}
