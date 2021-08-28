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


namespace IRTicker
{
    class CoinbaseClient
    {
        private string ApiKey { get; set; }
        private string UnsignedSignature { get; set; }
        private string PassPhrase { get; set; }

        public async Task<string> GetAccounts(string inAPIKey, string inAPISecret, string inPassPhrase) {

            HttpClient httpClient = new HttpClient();

            ApiKey = inAPIKey;
            UnsignedSignature = inAPISecret;
            PassPhrase = inPassPhrase;

            var httpRequestMessage = BuildHTTPRequest("https://api.exchange.coinbase.com", "/accounts", "", HttpMethod.Get);
            if (null == httpRequestMessage) return "";
            HttpResponseMessage httpResponseMessage;
            try {
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
                string res =  await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                return res;
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - Caught exception in Coinbase class when doing network things: " + ex.Message);
            }
            return "";
        }

        private HttpRequestMessage BuildHTTPRequest(string apiUri, string requestUri, string contentBody, HttpMethod httpMethod) {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(apiUri), requestUri)) {
                Content = contentBody == string.Empty
                    ? null
                    : new StringContent(contentBody, Encoding.UTF8, "application/json")
            };
            double timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            if (string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(UnsignedSignature) || string.IsNullOrEmpty(PassPhrase)) {
                return null;
            }

            string signedSignature = ComputeSignature(httpMethod, UnsignedSignature, timeStamp, requestUri, contentBody);

            AddHeaders(requestMessage, signedSignature, timeStamp, true);

            return requestMessage;
        }


        private void AddHeaders(HttpRequestMessage httpRequestMessage, string signedSignature, double timeStamp, bool includeAuthentication) {

            httpRequestMessage.Headers.Add("User-Agent", "IRTicker");

            if (!includeAuthentication) {
                return;
            }

            httpRequestMessage.Headers.Add("CB-ACCESS-KEY", ApiKey);
            httpRequestMessage.Headers.Add("CB-ACCESS-TIMESTAMP", timeStamp.ToString("F0", CultureInfo.InvariantCulture));
            httpRequestMessage.Headers.Add("CB-ACCESS-SIGN", signedSignature);
            httpRequestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", PassPhrase);
        }

        /*public void Authenticator(string apiKey, string unsignedSignature, string passphrase) {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(unsignedSignature) ||
                string.IsNullOrEmpty(passphrase)) {
                throw new ArgumentException(
                    $"{nameof(Authenticator)} requires parameters {nameof(apiKey)}, {nameof(unsignedSignature)} and {nameof(passphrase)} to be populated.");
            }

            ApiKey = apiKey;
            UnsignedSignature = unsignedSignature;
            PassPhrase = passphrase;
        }*/

        private string ComputeSignature(
            HttpMethod httpMethod,
            string secret,
            double timestamp,
            string requestUri,
            string contentBody = "") {

            var convertedString = Convert.FromBase64String(secret);
            var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() + requestUri + contentBody;
            return HashString(prehash, convertedString);
        }

        private string HashString(string str, byte[] secret) {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var hmaccsha = new HMACSHA256(secret)) {
                return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
            }
        }
    }
}
