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
using System.Collections.Concurrent;

namespace IRTicker {
    class PrivateIR {

        //private static readonly HttpClient client = new HttpClient();
        private Client IRclient;
        public Dictionary<string, Account> accounts = new Dictionary<string, Account>();
        private ApiCredential IRcreds;
        private IRTicker IRT;
        private ConcurrentQueue<IRClientData> IRQueue = new ConcurrentQueue<IRClientData>();
        private bool isDequeuing = false;

        public PrivateIR(string _BaseURL, string APIKey, string APISecret, IRTicker _IRT) {

            IRT = _IRT;

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
        public Task<DigitalCurrencyDepositAddress> CheckAddressNow(CurrencyCode crypto, string address) {
            Task<DigitalCurrencyDepositAddress> result;
            try {
                result = IRclient.SynchDigitalCurrencyDepositAddressWithBlockchainAsync(address, crypto);
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

            return IRclient.GetOpenOrdersAsync(enumCrypto, enumFiat, 1, 7);
        }

        public Task<Page<BankHistoryOrder>> GetClosedOrders(string crypto, string fiat) {
            CurrencyCode enumCrypto = convertCryptoStrToCryptoEnum(crypto);
            CurrencyCode enumFiat = convertCryptoStrToCryptoEnum(fiat);

            return IRclient.GetClosedFilledOrdersAsync(enumCrypto, enumFiat, 1, 7);
        }

        public Task<BankOrder> CancelOrder(Guid guid) {
            try {
                return IRclient.CancelOrderAsync(guid);
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

        public class IRClientData {
            public IRTicker.PrivateIREndPoints EndPoint { get; set; }
            public decimal LimitPrice { get; set; }
            public decimal Volume { get; set; }
            public CurrencyCode Crypto { get; set; }
            public CurrencyCode Fiat { get; set; }
            public int PageNum { get; set; }
            public int PageSize { get; set; }
            public OrderType orderType { get; set; }
            public string CryptoAddress { get; set; }
            public Guid guid { get; set; }
        }

        public void Enqueue(IRClientData IRdata) {
            IRQueue.Enqueue(IRdata);
            if (!isDequeuing) {
                isDequeuing = true;
                Dequeue();
            }
        }

        public void Enqueue(List<IRClientData> IRdataList) {
            foreach (IRClientData dat in IRdataList) {
                IRQueue.Enqueue(dat);
            }
            if (!isDequeuing) {
                isDequeuing = true;
                Dequeue();
            }
        }

        private async void Dequeue() {
            while (IRQueue.Count > 0) {
                if (IRQueue.TryDequeue(out IRClientData data)) {
                    switch (data.EndPoint) {
                        case IRTicker.PrivateIREndPoints.CancelOrder:
                            BankOrder bo = await CancelOrder(data.guid);
                            break;
                        case IRTicker.PrivateIREndPoints.CheckAddress:
                            await CheckAddressNow(data.Crypto, data.CryptoAddress);
                            break;
                        case IRTicker.PrivateIREndPoints.GetAccounts:
                            // this one hase a result.  should capture it and then call the draw func
                    }
                }
            }
            isDequeuing = false;
        }
    }
}
