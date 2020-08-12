using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace IRTicker {
    public partial class IRCurrencies
    {
        [JsonProperty("Currencies")]
        public Currency[] Currencies { get; set; }
    }

    public partial class Currency
    {
        [JsonProperty("Ir.Common.Attributes.CurrencyConfiguration")]
        public IrCommonAttributesCurrencyConfiguration IrCommonAttributesCurrencyConfiguration { get; set; }
    }

    public partial class IrCommonAttributesCurrencyConfiguration
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Ticker")]
        public string Ticker { get; set; }

        [JsonProperty("CurrencyType")]
        public CurrencyType CurrencyType { get; set; }

        [JsonProperty("Entities")]
        public Entity[] Entities { get; set; }

        [JsonProperty("SortOrder")]
        public int SortOrder { get; set; }

        [JsonProperty("RefNoPrefix", NullValueHandling = NullValueHandling.Ignore)]
        public string RefNoPrefix { get; set; }

        [JsonProperty("Family")]
        public string Family { get; set; }

        [JsonProperty("DepositType")]
        public DepositType DepositType { get; set; }

        [JsonProperty("FiatPriceDecimalPlaces")]
        public int FiatPriceDecimalPlaces { get; set; }

        [JsonProperty("FiatPriceStringFormat")]
        public string FiatPriceStringFormat { get; set; }

        [JsonProperty("AlwaysEnforceMinimumWithdrawal")]
        public bool AlwaysEnforceMinimumWithdrawal { get; set; }

        [JsonProperty("IsDelisted")]
        public bool IsDelisted { get; set; }

        [JsonProperty("UrlProtocol", NullValueHandling = NullValueHandling.Ignore)]
        public string UrlProtocol { get; set; }

        [JsonProperty("TokenContract", NullValueHandling = NullValueHandling.Ignore)]
        public TokenContract TokenContract { get; set; }

        [JsonProperty("ClonesDepositAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string ClonesDepositAddress { get; set; }
    }

    public partial class TokenContract
    {
        [JsonProperty("HostCurrency")]
        public HostCurrency HostCurrency { get; set; }

        [JsonProperty("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("ContractAddress")]
        public string ContractAddress { get; set; }

        [JsonProperty("DecimalPlaces")]
        public long DecimalPlaces { get; set; }
    }

    public enum CurrencyType { Primary, Secondary };

    public enum DepositType { Address, AddressAndTag, Unknown };

    public enum Entity { Au, Sg };

    public enum HostCurrency { Eth };
}
