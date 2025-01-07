using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using IRTicker.Coinbase_trade.Models;


namespace IRTicker
{
    class CoinbaseClient
    {
        public static async Task<string> CB_get_accounts(string account_id = "", string APIKey = null, string APISecret = null, string PassPhrase = null) {
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            string endPoint = "accounts";
            if (!string.IsNullOrEmpty(account_id)) {
                endPoint += "/" + account_id;
            }

            var response = await CB_GET(APIKey, APISecret, PassPhrase, endPoint);
            return response.Body;
        }
        // gets trading pairs
        public static async Task<string> CB_get_pairs(string APIKey = null, string APISecret = null, string PassPhrase = null) {
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            var response = await CB_GET(APIKey, APISecret, PassPhrase, "products");
            return response.Body;
        }

        public static async Task<string> CB_cancel_order(string order_id, string APIKey = null, string APISecret = null, string PassPhrase = null) {
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            var response = await CB_DELETE(APIKey, APISecret, PassPhrase, "orders", order_id);
            return response;
        }

        public static async Task<string> CB_get_open_orders(string APIKey = null, string APISecret = null, string PassPhrase = null) {
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            var response = await CB_GET(APIKey, APISecret, PassPhrase, "orders");
            return response.Body;
        }

        public static async Task<string> CB_get_fills(string pair, string APIKey = null, string APISecret = null, string PassPhrase = null) {
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            var response = await CB_GET(APIKey, APISecret, PassPhrase, "fills?product_id=" + pair);
            return response.Body;
        }
        public static async Task<List<CB_Order>> CB_get_settled(string pair = "", DateTime? fromDate = null, string APIKey = null, string APISecret = null, string PassPhrase = null) {

            // Default to your stored credentials if none provided
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            // We'll store all results here
            var allOrders = new List<CB_Order>();

            // We'll be requesting in descending order by created_at
            // and using "CB-BEFORE" to fetch older pages if needed
            // https://docs.cdp.coinbase.com/exchange/docs/pagination

            string beforeCursor = null;
            bool keepPaging = true;

            while (keepPaging) {
                // Build the query string
                // e.g.: orders?status=done&sortedBy=created_at&sorting=desc&product_id=BTC-USD
                string endpoint = "orders?status=done&sortedBy=created_at&sorting=desc";

                if (!string.IsNullOrEmpty(pair)) {
                    endpoint += "&product_id=" + pair;
                }

                if (!string.IsNullOrEmpty(beforeCursor)) {
                    // This indicates we want the page of data that occurs before this cursor
                    endpoint += "&before=" + beforeCursor;
                }

                // Make the request
                CoinbaseResponse resp = await CB_GET(APIKey, APISecret, PassPhrase, endpoint);

                // Deserialize the response body
                // Coinbase returns an array of orders for this endpoint
                List<CB_Order> pageOrders = new List<CB_Order>();
                try {
                    pageOrders = JsonConvert.DeserializeObject<List<CB_Order>>(resp.Body)
                                        ?? new List<CB_Order>();
                }
                catch (Exception ex) {
                    Debug.Print("CB-trade - trying to deserialise the CB_GET body, but it failed: " + ex.Message);
                    break;
                }

                if (pageOrders.Count == 0) {
                    // No more orders to fetch, or we've reached the end
                    break;
                }

                // Add them to our overall list
                allOrders.AddRange(pageOrders);

                // 1) If we find an order older than fromDate, we can stop
                //    Because we sorted descending, that means everything after
                //    this is also older.
                // 2) We could also wait until we've fetched all pages, then do a final filter,
                //    but this short-circuits early.
                if (fromDate.HasValue) {  // if fromDate is null, we just finish here
                    DateTime oldestOnThisPage = pageOrders[pageOrders.Count - 1].created_at;
                    if (oldestOnThisPage < fromDate) {
                        // We have gone past the fromDate in descending order
                        keepPaging = false;
                        break;
                    }
                }
                else {
                    return allOrders;
                }

                // Otherwise, we must retrieve the "CB-BEFORE" header from this response
                // so we can get the next older page.
                // If there's no such header, it means no more pages exist.
                if (resp.Headers.TryGetValue("CB-BEFORE", out string nextBefore)) {
                    // If the header is empty, we can't proceed
                    if (string.IsNullOrWhiteSpace(nextBefore)) {
                        break;
                    }
                    beforeCursor = nextBefore;
                }
                else {
                    // No more pages
                    break;
                }
            }

            // At this point, `allOrders` contains all orders from "now" down to some older date,
            // possibly older than your fromDate. Let's filter out anything older than fromDate if you want:
            var finalList = allOrders
                .Where(o => o.created_at >= fromDate)
                .ToList();

            // Return the final chunk
            return finalList;
        }


        // side is "buy" or "sell
        // type is "limit" or "market" or "stop"
        // time in force is assumed GTC
        public static async Task<string> CB_post_order(string pair, string side, string price, string size, string type, bool post_only = false, string client_uid = "", string APIKey = null, string APISecret = null, string PassPhrase = null) {
            if (APIKey == null || APISecret == null || PassPhrase == null) {
                APIKey = Properties.Settings.Default.CoinbaseAPIKey;
                APISecret = Properties.Settings.Default.CoinbaseAPISecret;
                PassPhrase = Properties.Settings.Default.CoinbasePassPhrase;
            }

            string jsonBody;
            if (type == "limit") {
                var orderData = new
                {
                    product_id = pair,
                    side,
                    price,
                    size,
                    type,
                    time_in_force = "GTC",
                    client_uid,
                    post_only
                };

                jsonBody = JsonConvert.SerializeObject(orderData);
            }
            else if (type == "market") {
                var orderData = new
                {
                    product_id = pair,
                    side,
                    size,
                    type,
                };

                jsonBody = JsonConvert.SerializeObject(orderData);
            }
            else {
                return null;
            }

            var response = await CB_POST(APIKey, APISecret, PassPhrase, "orders", jsonBody);
            return response;
        }

        // for use in CB_GET, like a tuple I guess so we can return the body and headers in one object
        public class CoinbaseResponse {
            public string Body { get; set; }
            public Dictionary<string, string> Headers { get; set; }

            public CoinbaseResponse() {
                Body = string.Empty;
                Headers = new Dictionary<string, string>();
            }
        }


        // does a basic GET request on the coinbase exchange 
        // endPoint can be "orders" or whatever the first thing after the / is
        // arg can be another folder deeper, eg /{order_id} in /orders/{order_id}
        private static async Task<CoinbaseResponse> CB_GET(
            string APIKey,
            string APISecret,
            string PassPhrase,
            string endPoint,
            string arg = "") {
            HttpClient httpClient = new HttpClient();

            // Build the request (assuming BuildHTTPRequest is the same method you use in CB_GET)
            var httpRequestMessage = BuildHTTPRequest(
                "https://api.exchange.coinbase.com",
                "/" + endPoint + (string.IsNullOrEmpty(arg) ? "" : "/" + arg),
                "",
                HttpMethod.Get,
                APIKey,
                APISecret,
                PassPhrase
            );

            if (httpRequestMessage == null) {
                return new CoinbaseResponse { Body = "" };
            }

            try {
                // Send the request
                HttpResponseMessage httpResponseMessage = await httpClient
                    .SendAsync(httpRequestMessage)
                    .ConfigureAwait(false);

                // Capture the body
                string body = await httpResponseMessage.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                // Prepare our CoinbaseResponse
                var response = new CoinbaseResponse
                {
                    Body = body
                };

                // Capture headers (like CB-BEFORE, CB-AFTER, etc.)
                foreach (var header in httpResponseMessage.Headers) {
                    // If a header has multiple values, join them into a single comma-separated string
                    response.Headers[header.Key] = string.Join(",", header.Value);
                }

                return response;
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now +
                    " - Caught exception in Coinbase class when performing a GET (/" + endPoint +
                    "), error: " + ex.Message);

                // Return an empty response or handle error as needed
                return new CoinbaseResponse { Body = "" };
            }
        }


        private static async Task<string> CB_DELETE(string APIKey, string APISecret, string PassPhrase, string endPoint, string arg = "") {
            HttpClient httpClient = new HttpClient();

            // Build the HTTP request message
            var httpRequestMessage = BuildHTTPRequest(
                "https://api.exchange.coinbase.com",
                "/" + endPoint + (string.IsNullOrEmpty(arg) ? "" : "/" + arg),
                "",
                HttpMethod.Delete,
                APIKey,
                APISecret,
                PassPhrase
            );

            if (httpRequestMessage == null) return "";

            HttpResponseMessage httpResponseMessage;
            try {
                // Send the DELETE request
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
                string res = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                return res;
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - Caught exception in Coinbase class when making a DELETE (/" + endPoint + "), error: " + ex.Message);
            }
            return "";
        }

        public static async Task<string> CB_POST(string APIKey, string APISecret, string PassPhrase, string endPoint, string jsonBody) {
            HttpClient httpClient = new HttpClient();

            // Build the HTTP request message
            var httpRequestMessage = BuildHTTPRequest(
                "https://api.exchange.coinbase.com",
                "/" + endPoint,
                jsonBody,
                HttpMethod.Post,
                APIKey,
                APISecret,
                PassPhrase
            );
            if (httpRequestMessage == null) return "";

            HttpResponseMessage httpResponseMessage;
            try {
                // Send the POST request
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
                string response = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response;
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - Caught exception in Coinbase class when making a POST (/" + endPoint + "), error: " + ex.Message);
            }
            return "";
        }



        /// AUTHENTICATION HELPERS

        private static HttpRequestMessage BuildHTTPRequest(string apiUri, string requestUri, string contentBody, HttpMethod httpMethod, string APIKey, string APISecret, string PassPhrase) {

            var requestMessage = new HttpRequestMessage(httpMethod, new Uri(new Uri(apiUri), requestUri))
            {
                Content = contentBody == string.Empty
                    ? null
                    : new StringContent(contentBody, Encoding.UTF8, "application/json")
            };

            double timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;  // epoch time

            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(APISecret) || string.IsNullOrEmpty(PassPhrase)) {
                return null;
            }

            string signedSignature = ComputeSignature(httpMethod, APISecret, timeStamp, requestUri, contentBody);
            //string signedSig2 = GenerateSignature(timeStamp.ToString("F0", CultureInfo.InvariantCulture), httpMethod.ToString().ToUpper(), requestUri, "", APISecret);

            AddHeaders(requestMessage, signedSignature, timeStamp, APIKey, PassPhrase);  // requestMessage is manipulated by this method

            return requestMessage;
        }


        private static void AddHeaders(HttpRequestMessage httpRequestMessage, string signedSignature, double timeStamp, string APIkey, string PassPhrase) {

            httpRequestMessage.Headers.Add("User-Agent", "IRTicker");

            // CB - ACCESS - KEY The api key as a string.
            // CB - ACCESS - SIGN The base64-encoded signature(see Signing a Message).
            // CB - ACCESS - TIMESTAMP A timestamp for your request.
            // CB - ACCESS - PASSPHRASE The passphrase you specified when creating the API key.
            httpRequestMessage.Headers.Add("CB-ACCESS-KEY", APIkey);
            httpRequestMessage.Headers.Add("CB-ACCESS-TIMESTAMP", timeStamp.ToString("F0", CultureInfo.InvariantCulture));
            httpRequestMessage.Headers.Add("CB-ACCESS-SIGN", signedSignature);
            httpRequestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", PassPhrase);
        }


        // From Coinbase API docs - https://docs.exchange.coinbase.com/#signing-a-message
        // The CB-ACCESS-SIGN header is generated by creating a sha256 HMAC using the base64-decoded secret key on the prehash 
        // string timestamp + method + requestPath + body (where + represents string concatenation) and base64-encode the output. 
        // The timestamp value is the same as the CB-ACCESS-TIMESTAMP header.
        private static string ComputeSignature(
            HttpMethod httpMethod,
            string secret,
            double timestamp,
            string requestUri,
            string contentBody = "") {

            var convertedString = Convert.FromBase64String(secret);
            var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() + requestUri + contentBody;
            return HashString(prehash, convertedString);
        }

        // ... magic
        public static string HashString(string str, byte[] secret) {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var hmaccsha = new HMACSHA256(secret)) {
                return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
            }
        }

        // i think these next three functions i was trying to figure out why cb wasn't working, but it was a typo in another file.  I think i can delete.

        /*static string ByteToHexString(byte[] bytes) {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++) {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
            }
            return new string(c);
        }*/

        /* public static string GenerateSignature(string timestamp, string method, string url, string body, string appSecret) {
             return GetHMACInHex(appSecret, timestamp + method + url + body);
         }*/

        /*internal static string GetHMACInHex(string key, string data) {
            var hmacKey = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA256(hmacKey)) {
                var sig = hmac.ComputeHash(dataBytes);
                return ByteToHexString(sig);
            }
        }*/
    }
}
