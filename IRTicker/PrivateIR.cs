using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace IRTicker {
    class PrivateIR {

        //private static readonly HttpClient client = new HttpClient();
        private Client IRclient;
        public Dictionary<string, Account> accounts = new Dictionary<string, Account>();
        private ApiCredential IRcreds;

        public PrivateIR(string _BaseURL, string APIKey, string APISecret) {

            //string API_Key = "67a60129-033e-429b-a46a-3f0395334e19";

            //string API_Secret = "a031caf6c67440819cf2a15f0fbe9784";

            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(APISecret)) {
                Debug.Print(DateTime.Now + " cannot do private IR stuff, missing API key(s)");
                return;
            }

            IRcreds = new ApiCredential(APIKey, APISecret);

            var IRconf = new ApiConfig {
                BaseUrl = _BaseURL,
                Credential = IRcreds
            };
            IRclient = Client.Create(IRconf);
        }

        public Dictionary<string, Account> GetAccounts() {
            try {
                accounts = IRclient.GetAccounts().ToDictionary(x => x.CurrencyCode.ToString().ToUpper(), x => x);
                return accounts;
            }
            catch (Exception ex) {
                MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                    ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public Task<DigitalCurrencyDepositAddress> GetDepositAddress(string crypto) {
            return IRclient.GetDigitalCurrencyDepositAddressAsync(convertCryptoStrToCryptoEnum(crypto));
        }

        //
        public Task<DigitalCurrencyDepositAddress> CheckAddressNow(string crypto, string address) {
            Task<DigitalCurrencyDepositAddress> result;
            try {
                result = IRclient.SynchDigitalCurrencyDepositAddressWithBlockchainAsync(address, convertCryptoStrToCryptoEnum(crypto));
            }
            catch (Exception ex) {
                MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                    ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return result;
        }

        public Task<BankOrder> PlaceLimitOrder(string crypto, string fiat, OrderType orderType, decimal price, decimal volume) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            Task<BankOrder> orderResult;

            try {
                orderResult = IRclient.PlaceLimitOrderAsync(enumCrypto, enumFiat, orderType, price, volume);
            }
            catch (Exception ex) {
                MessageBox.Show("API error: " + ex.InnerException.Message, "Limit Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                orderResult = null;
            }
            return orderResult;
        }

        public Task<BankOrder> PlaceMarketOrder(string crypto, string fiat, OrderType orderType, decimal volume) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            Task<BankOrder> orderResult;

            try {
                orderResult = IRclient.PlaceMarketOrderAsync(enumCrypto, enumFiat, orderType, volume);
            }
            catch (Exception ex) {
                MessageBox.Show("API error: " + ex.InnerException.Message, "Market Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                orderResult = null;
            }
            return orderResult;
        }

        public Task<Page<BankHistoryOrder>> GetOpenOrders(string crypto, string fiat) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            return IRclient.GetOpenOrdersAsync(enumCrypto,enumFiat,1, 6);
        }

        public Task<Page<BankHistoryOrder>> GetClosedOrders(string crypto, string fiat) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            return IRclient.GetClosedFilledOrdersAsync(enumCrypto, enumFiat, 1, 6);
        }

        public Task<BankOrder> CancelOrder(string guid) {
            try {
                return IRclient.CancelOrderAsync(new Guid(guid));
            }
            catch (Exception ex) {
                MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                    ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private CurrencyCode convertCryptoStrToCryptoEnum(string crypto) {
            Enum.TryParse(Utilities.FirstLetterToUpper(crypto), out CurrencyCode enumCrypto);
            return enumCrypto;
        }
    }
}
