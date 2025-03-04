﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

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

        /// <summary>
        /// takes a decimal and returns a beautiful string.  eg input: 144223.443332, output: 144 223
        /// input 0.3434332, output 0.34343
        /// input 0.34343 and 2, output 0.34
        /// a value between 10 and 1000 will automatically get 2 decimal places, this is strict
        /// a value over 1000 will get no decimal places, this is strict
        /// a value under 10 will be default get 5 decimal places, but this is configurable using maxDecimalPlaces.
        /// decimalsForLargeNumbersOnly = if set to true (default), then we only apply custom `decimalPlaces` arguement IF the value is greater than 1000, `decimalPlaces` is ignored for numbers under 1000.
        /// decimalsForLargeNumbersOnly = if set to false, then the `decimalPlaces` arguement is used for ALL numbers sent to this method.
        /// </summary>
        /// <param name="val">the value we're formatting</param>
        /// <param name="decimalPlaces">max decimal places to return.  Defaults to 5, this is only for limiting if you want less than 5 where the value is less than 10</param>
        /// <param name="decimalsForLargeNumbersOnly">if the number is over 1000, apply custom "decimalPlaces", otherwise if the price is less than 1000 just use standard (2 dp for > 10, 5 dp under 10)</param>
        /// <returns>a beautiful string</returns>
        public static string FormatValue(decimal val, int decimalPlaces = -1, bool decimalsForLargeNumbersOnly = true) {

            bool positiveInput = true;  // track if this sub receives a negative or positive "val" value.  If negative, make it positive for calculations, then flip it at the end
            string formatString = "0";
            if (val < 0) {
                val = val * -1;
                positiveInput = false;
            }

            string suffix = "";

            // default settings
            if (val < 10) formatString = "0.00###";
            if (val < 0.01M) formatString = "0.00######";
            if (val >= 10) formatString = "##0.00";
            if (val >= 1000) formatString = "### ### ##0";
            if (val >= 1_000_000_000) {
                val = Math.Round(val / 1_000_000_000, 3);  // _ is just a visual separator - the compiler ignores
                formatString = "0.###";
                suffix = "B";
            }

            // -1 means the method was not sent any decimalPlaces value
            if (decimalPlaces == -1) return (positiveInput ? val.ToString(formatString).Trim() : "-" + val.ToString(formatString).Trim()) + suffix;

            // we get here if we have a decimalPlaces value, and the decimalsForLargeNumbersOnly is true.
            // the idea is that even if we have a custom decimalPlaces value, we ignore it if the number is LESS THAN 1000
            if (decimalsForLargeNumbersOnly && (val < 1000)) return (positiveInput ? val.ToString(formatString).Trim() : "-" + val.ToString(formatString).Trim());

            // custom decimal places
            if (val < 10) formatString = "0";
            if (val >= 10) formatString = "##0";
            if (val >= 1000) formatString = "### ### ##0";
            if (val >= 1000000000) {
                val = Math.Round(val / 1_000_000_000, 3);
                formatString = "0.###";
            }


            int loopCounter = 0;
            if (decimalPlaces > 0) formatString += ".";
            while (loopCounter < decimalPlaces) {
                if (loopCounter < 2) formatString += "0";
                else formatString += "#";
                loopCounter++;
            }
            return (positiveInput ? val.ToString(formatString).Trim() : "-" + val.ToString(formatString).Trim());
        }

        /// <summary>
        /// takes a decimal and returns a beautiful string.  eg input: 144223.443332, output: 144 223
        /// input 0.3434332, output 0.34343
        /// input 0.34343 and 2, output 0.34
        /// a value between 10 and 1000 will automatically get 2 decimal places, this is strict
        /// a value over 1000 will get no decimal places, this is strict
        /// a value under 10 will be default get 5 decimal places, but this is configurable using maxDecimalPlaces.
        /// decimalsForLargeNumbersOnly = if set to true (default), then we only apply custom `decimalPlaces` arguement IF the value is greater than 1000, `decimalPlaces` is ignored for numbers under 1000.
        /// decimalsForLargeNumbersOnly = if set to false, then the `decimalPlaces` arguement is used for ALL numbers sent to this method.
        /// </summary>
        /// <param name="val">the value we're formatting</param>
        /// <param name="decimalPlaces">max decimal places to return.  Defaults to 5, this is only for limiting if you want less than 5 where the value is less than 10</param>
        /// <param name="decimalsForLargeNumbersOnly">if the number is over 1000, apply custom "decimalPlaces", otherwise if the price is less than 1000 just use standard (2 dp for > 10, 5 dp under 10)</param>
        /// <returns>a beautiful string</returns>
        public static string FormatValue(string val_str, int decimalPlaces = -1, bool decimalsForLargeNumbersOnly = true) {

            // first, we must convert the string to a decimal
            if (!decimal.TryParse(val_str, out decimal val)) {
                return "N/A";
            }

            bool positiveInput = true;  // track if this sub receives a negative or positive "val" value.  If negative, make it positive for calculations, then flip it at the end
            string formatString = "0";
            if (val < 0) {
                val = val * -1;
                positiveInput = false;
            }

            string suffix = "";

            // default settings
            if (val < 10) formatString = "0.00###";
            if (val < 0.01M) formatString = "0.00######";
            if (val >= 10) formatString = "##0.00";
            if (val >= 1000) formatString = "### ### ##0";
            if (val >= 1_000_000_000) {
                val = Math.Round(val / 1_000_000_000, 3);  // _ is just a visual separator - the compiler ignores
                formatString = "0.###";
                suffix = "B";
            }

            // -1 means the method was not sent any decimalPlaces value
            if (decimalPlaces == -1) return (positiveInput ? val.ToString(formatString).Trim() : "-" + val.ToString(formatString).Trim()) + suffix;

            // we get here if we have a decimalPlaces value, and the decimalsForLargeNumbersOnly is true.
            // the idea is that even if we have a custom decimalPlaces value, we ignore it if the number is LESS THAN 1000
            if (decimalsForLargeNumbersOnly && (val < 1000)) return (positiveInput ? val.ToString(formatString).Trim() : "-" + val.ToString(formatString).Trim());

            // custom decimal places
            if (val < 10) formatString = "0";
            if (val >= 10) formatString = "##0";
            if (val >= 1000) formatString = "### ### ##0";
            if (val >= 1000000000) {
                val = Math.Round(val / 1_000_000_000, 3);
                formatString = "0.###";
            }


            int loopCounter = 0;
            if (decimalPlaces > 0) formatString += ".";
            while (loopCounter < decimalPlaces) {
                if (loopCounter < 2) formatString += "0";
                else formatString += "#";
                loopCounter++;
            }
            return (positiveInput ? val.ToString(formatString).Trim() : "-" + val.ToString(formatString).Trim());
        }

        // this list will be sorted from earliest time to latest time
        public static Color PriceColour(List<Tuple<DateTime, decimal>> priceList) {

            //var priceList = priceListSource.ToList();
            if (priceList.Count <= 1) {  // if there's 1 price, well we can't compare that with anything, so just bail.  if there's no prices, well shit you know you gotta bail
                //Debug.Print("PriceColour function was sent an empty list.  bailing.");
                return Color.Black;
            }

            var priceListFirst = priceList.FirstOrDefault();
            var priceListLast = priceList.LastOrDefault();

            if (priceListLast == null) return Color.Black;
            //else throw new Exception("null pricelistlast");
            if (priceListFirst == null) return Color.Black;
                //else throw new Exception("null pricelistfirst");
            /*}
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - caught an exception for pricelist: " + ex.ToString());
                return Color.Black;
            }
            */

            //if (priceList == null || priceList.Count == 0 || PriceListLast == null || PriceListFirst == null || PriceListLastItem1 == null || PriceListFirstItem1 == null) return Color.Black;
            if (priceList.Count < 5) return Color.Black;
            if (priceListLast.Item1 - priceListFirst.Item1 < TimeSpan.FromMinutes(5)) return Color.Black;


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

            return Color.Black;
        }

        /// <summary>
        /// takes a pair with a - separater like XBT-AUD, and returns a tuple (string, string) of each currency.  item1 in the tuple is what's before the '-', item2 is what's after.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static Tuple<string, string> SplitPair(string pair) {

            // if it doesn't contain a dash, or the dash is the first or last char...
            if (!pair.Contains("-") || (pair.IndexOf("-") == 0) || (pair.Length - 1 <= pair.IndexOf("-"))) {
                return new Tuple<string, string>("", "");
            }

            string primary = pair.Substring(0, pair.IndexOf('-'));
            string secondary = pair.Substring(pair.IndexOf('-') + 1, pair.Length - pair.IndexOf('-') - 1);
            return new Tuple<string, string>(primary, secondary);
        }

        /// <summary>
        /// takes - separated pair (eg "USDT-AUD") and swaps the crypto part out with the provided crypto string
        /// </summary>
        /// <param name="pair">the pair to manipulate</param>
        /// <param name="newCrypto">the new crypto to insert into the pair</param>
        /// <returns>the new pair string with the new crypto</returns>
        public static string replaceCrypto(string pair, string newCrypto) {
            return newCrypto + pair.Substring(pair.IndexOf('-'), pair.Length - 1);
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
                    else if (string.IsNullOrEmpty(result)) {
                        return new Tuple<bool, string>(false, "EmptyResponse");
                    }
                    else if (result.StartsWith("Internal Server Error")) {
                        return new Tuple<bool, string>(false, "Internal Server Error");
                    }

                    return new Tuple<bool, string>(true, result);
                }
            }
            catch (WebException e) {
                string returnStr = "";
                
                if (e.Response != null) {
                    using (WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Debug.Print("Fail for: " + uri);
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
            catch (Exception e) {
                Debug.Print(DateTime.Now + " -- GET FAILED! exception: " + e.Message);
                return new Tuple<bool, string>(false, e.Message);
            }
        }

        /// <summary>
        /// Will make the first letter capitalised, and all other letters lower case
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLetterToUpper(string str) {
            if (str == null) return null;

            str = str.ToLower();
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static void PopulateCryptoComboBox(DCE _DCE, ComboBox cBox) {

            if (cBox == null) return;  // eg coinspot 

            cBox.Items.Clear();
            cBox.ResetText();
            cBox.Items.Add("");  // add an empty option as the first one so it can be selected when we need to "reset"
            cBox.SelectedIndex = 0;

            foreach (string pair in _DCE.UsablePairs()) {
                Tuple<string, string> splitPair = SplitPair(pair);  // splits "XBT-AUD" into a tuple ("XBT","AUD")
                if (splitPair.Item2 == _DCE.CurrentSecondaryCurrency) {
                    cBox.Items.Add(splitPair.Item1 == "XBT" ? "BTC" : splitPair.Item1);
                }
            }

            if (cBox.Items.Count < 1) {
                MessageBox.Show("Error - no primary currencies from " + _DCE.FriendlyName + "?", "Show this to Nick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cBox.Enabled = false;
            }
        }
        public static decimal Truncate(decimal d, byte decimals) {
            decimal r = Math.Round(d, decimals);

            if (d > 0 && r > d) {
                return r - new decimal(1, 0, 0, false, decimals);
            }
            else if (d < 0 && r < d) {
                return r + new decimal(1, 0, 0, false, decimals);
            }
            return r;
        }

        public static string GetOrdinalSuffix(int num) {
            string number = num.ToString();
            if (number.EndsWith("11")) return "th";
            if (number.EndsWith("12")) return "th";
            if (number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
        }

        /// <summary>
        /// Just does a web request where there is 1 header - Authorization.  
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetWebData(string uri, string AuthorizationToken = "") {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //request.UserAgent = "IRTicker";

            if (!string.IsNullOrEmpty(AuthorizationToken)) request.Headers["Authorization"] = AuthorizationToken;  // both trigon and B2C2 use the same thing, i guess this is coinroutes

            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) {
                    string result = reader.ReadToEnd();

                    return result;
                }
            }
            catch (WebException e) {
                string returnStr = "";

                if (e.Response != null) {
                    using (WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Debug.Print("Fail for: " + uri);
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
                //return new Tuple<bool, string>(false, returnStr);
            }
            catch (Exception e) {
                Debug.Print(DateTime.Now + " -- GET FAILED! exception: " + e.Message);
                //return new Tuple<bool, string>(false, e.Message);
            }
            return "";
        }

        // builds the combo box items in the trade drop down menu on the main panel
        public static void buildTradingVenueItems (ComboBox combo) {

            combo.Items.Clear();
            combo.Items.Add("Trade");  // always
            
            if (!string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPubKey) &&
                !string.IsNullOrEmpty(Properties.Settings.Default.IRAPIPrivKey)) {

                combo.Items.Add("IR");
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.CoinbaseAPIKey) &&
                !string.IsNullOrEmpty(Properties.Settings.Default.CoinbasePassPhrase) &&
                !string.IsNullOrEmpty(Properties.Settings.Default.CoinbaseAPISecret)) {

                combo.Items.Add("Coinbase");
            }

            if (combo.Items.Count == 1) {
                combo.Items.Add("No trade venues available");
            }
        }

        public static int? countDecimalPrecision(decimal number) {
            // Remove trailing zeros
            number = decimal.Parse(number.ToString().TrimEnd('0'));

            // Get the string representation of the number
            string[] parts = number.ToString().Split('.');
            if (parts.Length == 2) // If there's a decimal part
            {
                return parts[1].Length; // Length of the decimal part
            }
            return null; // No decimal part
        }

        public static int? countDecimalPrecision(string number) {
            // Remove trailing zeros

            // Get the string representation of the number
            string[] parts = number.ToString().Split('.');
            if (parts.Length == 2) // If there's a decimal part
            {
                return parts[1].Length; // Length of the decimal part
            }
            return null; // No decimal part
        }
    }
}
