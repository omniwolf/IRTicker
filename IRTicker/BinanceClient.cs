using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRTicker {
    internal class BinanceClient {

        private const string BaseUrl = "https://api.binance.com";

        /*static async Task Main(string APIkey, string APIsecret)
        {
            var result = await GetSpotBalancesAsync(APIkey, APIsecret);
            var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.WriteLine(json);
        }*/


        /**         * Retrieves spot account balances from Binance API.
         *
         * @param APIkey Your Binance API key.
         * @param APIsecret Your Binance API secret.
         * @param recvWindowMs Optional. The number of milliseconds after timestamp the request is valid for. Default is 5000ms.
         * @return An object containing account type, update time, and a list of non-zero balances.
         * @throws HttpRequestException if the API request fails.
         * @throws InvalidOperationException if the response cannot be deserialized.
         */
        public async Task<string> GetSpotBalancesAsync(string APIkey, string APIsecret, long recvWindowMs = 5000)
        {
            using (var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) })
            {
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                http.DefaultRequestHeaders.Add("X-MBX-APIKEY", APIkey);

                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var query = $"timestamp={timestamp}&recvWindow={recvWindowMs}";
                var signature = Sign(query, APIsecret);
                var url = $"{BaseUrl}/api/v3/account?{query}&signature={signature}";

                using (var resp = await http.GetAsync(url))
                {
                    var body = await resp.Content.ReadAsStringAsync();

                    if (!resp.IsSuccessStatusCode) {
                        //throw new HttpRequestException($"Binance error {(int)resp.StatusCode}: {body}");
                        Debug.Print("Binance GetSpotBalancesAsync error {0}: {1}", (int)resp.StatusCode, body);
                        return null;
                    }

                    /*var acct = JsonConvert.DeserializeObject<AccountResponse>(body);
                    if (acct == null)
                        throw new InvalidOperationException("Failed to deserialize Binance account response.");


                    var nonZero = acct.Balances
                        .Where(b => ParseDec(b.Free) > 0m || ParseDec(b.Locked) > 0m)
                        .Select(b => new { b.Asset, b.Free, b.Locked })
                        .ToList();

                    return new
                    {
                        acct.AccountType,
                        acct.UpdateTime,
                        balances = nonZero
                    };*/

                    return body;
                }
            }
        }

        string Sign(string data, string secret)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            using (var hmac = new HMACSHA256(key)) {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
