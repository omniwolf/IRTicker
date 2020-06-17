using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace IRTicker {
    class PrivateIR {

        private static readonly HttpClient client = new HttpClient();


        public PrivateIR() {
            Task blah = GetAccounts();
        }

        async public Task GetAccounts() {


            // https://stackoverflow.com/questions/36575195/hmac-sha256-hash-computation-in-c-sharp
            string API_Key = "67a60129 - 033e-429b - a46a - 3f0395334e19";

            string API_Secret = "a031caf6c67440819cf2a15f0fbe9784";

            string url = "https://api.independentreserve.com/Private/GetAccounts";

            DateTime foo = DateTime.UtcNow;
            long nonce = ((DateTimeOffset)foo).ToUnixTimeSeconds();

            List<string> sigParams = new List<string>();
            sigParams.Add(url);
            sigParams.Add("apiKey=" + API_Key);
            sigParams.Add("nonce=" + nonce);

            string preSig = "";

            // build comma separated list of parameters for the signature
            foreach (string param in sigParams) {
                if (preSig.Length > 0) preSig += ",";
                preSig += param;
            }

            var signatureBytes = Encoding.UTF8.GetBytes(preSig);
            var encodedSecret = Encoding.UTF8.GetBytes(API_Secret);

            string hashString;
            using (var hmac = new HMACSHA256(encodedSecret)) {
                var hash = hmac.ComputeHash(signatureBytes);
                hashString = Convert.ToBase64String(hash);
            }




            var values = new Dictionary<string, string>
                {
                    {"apiKey", API_Key },
                    {"nonce", nonce.ToString() },
                    {"signature", hashString }
                };

            var content = new FormUrlEncodedContent(values);

            content.Headers.ContentType.MediaType = "application/json";
            var response = await client.PostAsync(url, content);

            var responseString = await response.Content.ReadAsStringAsync();

            //HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            //httpRequest.Content = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
            //httpRequest.Headers.Authorization = new AuthenticationHeaderValue("CitrixAuth", "H4sIAAAAAAAEAK1X2Y6jyBL9lZLnEbnZt1J");

            //var response = await client.SendAsync(httpRequest);
        }

    }
}
