﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;
using System.Diagnostics;

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

            if (priceList.Count <= 1) {  // if there's 1 price, well we can't compare that with anything, so just bail.  if there's no prices, well shit you know you gotta bail
                //Debug.Print("PriceColour function was sent an empty list.  bailing.");
                return Color.Black;
            }

            Tuple<DateTime, decimal> PriceListLast ;
            Tuple<DateTime, decimal> PriceListFirst;
            DateTime PriceListLastItem1;
            DateTime PriceListFirstItem1;

            try {
                // if we don't have enough data, just go black.
                PriceListLast = priceList.LastOrDefault();
                PriceListFirst = priceList.FirstOrDefault();
                PriceListLastItem1 = PriceListLast.Item1;
                PriceListFirstItem1 = PriceListFirst.Item1;
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - caught an exception for pricelist: " + ex.ToString());
                return Color.Black;
            }


            if (priceList == null || priceList.Count == 0 || PriceListLast == null || PriceListFirst == null || PriceListLastItem1 == null || PriceListFirstItem1 == null) return Color.Black;
            if (priceList.Count < 5) return Color.Black;
            if (PriceListLast.Item1 - PriceListFirst.Item1 < TimeSpan.FromMinutes(5)) return Color.Black;

            lock (priceList) {
                foreach (Tuple<DateTime, decimal> pricePoint in priceList) {
                    if (pricePoint.Item1 >= DateTime.Now - TimeSpan.FromMinutes(5)) {
                        decimal lastPrice = priceList.LastOrDefault().Item2;
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

        public static Tuple<bool, string> Get(string uri) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = "IRTicker";

            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) {
                    string result = reader.ReadToEnd();

                    // annoying
                    if (result.StartsWith("{\"success\":false")) {
                        return new Tuple<bool, string>(false, "BadRequest");
                    }
                    return new Tuple<bool, string>(true, result);
                }
            }
            catch (WebException e) {
                string returnStr = "";
                if (e.Response != null) {
                    using (WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Debug.Print("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data)) {
                            returnStr = reader.ReadToEnd();
                            Debug.Print(returnStr);
                            returnStr = httpResponse.StatusCode.ToString();
                        }
                    }
                }
                //MessageBox.Show("Error connecting to URL: " + uri, "Network error", MessageBoxButtons.OK);
                return new Tuple<bool, string>(false, returnStr);
            }
        }
    }
}
