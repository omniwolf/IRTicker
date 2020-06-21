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

        public PrivateIR(string APIKey, string APISecret) {

            //string API_Key = "67a60129-033e-429b-a46a-3f0395334e19";

            //string API_Secret = "a031caf6c67440819cf2a15f0fbe9784";

            if (string.IsNullOrEmpty(APIKey) || string.IsNullOrEmpty(APISecret)) {
                Debug.Print(DateTime.Now + " cannot do private IR stuff, missing API key(s)");
                return;
            }

            IRcreds = new ApiCredential(APIKey, APISecret);

            var IRconf = new ApiConfig {
                BaseUrl = "https://api.independentreserve.com/",
                Credential = IRcreds
            };
            IRclient = Client.Create(IRconf);
        }

        public Dictionary<string, Account> GetAccounts() {

            accounts = IRclient.GetAccounts().ToDictionary(x => x.CurrencyCode.ToString().ToUpper(), x => x);
            return accounts;
        }

        public DigitalCurrencyDepositAddress GetDepositAddress(string crypto) {
            return IRclient.GetDigitalCurrencyDepositAddress(convertCryptoStrToCryptoEnum(crypto));
        }

        //
        public DigitalCurrencyDepositAddress CheckAddressNow(string crypto, string address) {
            DigitalCurrencyDepositAddress result;
            try {
                result = IRclient.SynchDigitalCurrencyDepositAddressWithBlockchain(address, convertCryptoStrToCryptoEnum(crypto));
            }
            catch (Exception ex) {
                MessageBox.Show("API error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = null;
            }

            return result;
        }

        public BankOrder PlaceMarketOrder(string crypto, string fiat, string orderType, decimal volume) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);
            Enum.TryParse(orderType, out OrderType enumOrderType);

            BankOrder orderResult;

            try {
                orderResult = IRclient.PlaceMarketOrder(enumCrypto, enumFiat, enumOrderType, volume);
            }
            catch (Exception ex) {
                MessageBox.Show("API error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                orderResult = null;
            }
            return orderResult;
        }

        public Page<BankHistoryOrder> GetOpenOrders(string crypto, string fiat) {

            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            return IRclient.GetOpenOrders(enumCrypto,enumFiat,1, 6);
        }

        public Page<BankHistoryOrder> GetClosedOrders(string crypto, string fiat) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            return IRclient.GetClosedFilledOrders(enumCrypto, enumFiat, 1, 6);
        }

        private CurrencyCode convertCryptoStrToCryptoEnum(string crypto) {
            Enum.TryParse(Utilities.FirstLetterToUpper(crypto), out CurrencyCode enumCrypto);
            return enumCrypto;
        }
    }
}
