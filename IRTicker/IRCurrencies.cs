using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTicker {
    class IRCurrencies {


        public class IRCurrenciesRoot {
            public Aud Aud { get; set; }
            public Usd Usd { get; set; }
            public Nzd Nzd { get; set; }
            public Xbt Xbt { get; set; }
            public Eth Eth { get; set; }
            public Xrp Xrp { get; set; }
            public Bch Bch { get; set; }
            public Bsv Bsv { get; set; }
            public Ust Ust { get; set; }
            public Ltc Ltc { get; set; }
            public Eos Eos { get; set; }
            public Xlm Xlm { get; set; }
            public Etc Etc { get; set; }
            public Bat Bat { get; set; }
            public Omg Omg { get; set; }
            public Rep Rep { get; set; }
            public Zrx Zrx { get; set; }
            public Gnt Gnt { get; set; }
            public Pla Pla { get; set; }
            public Pmg Pmg { get; set; }
            public Lnk Lnk { get; set; }
        }

        public class Aud {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string RefNoPrefix { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
        }

        public class Usd {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string RefNoPrefix { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
        }

        public class Nzd {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string RefNoPrefix { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
        }

        public class Xbt {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Eth {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Xrp {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Bch {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Bsv {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Ust {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }
     
        public class Ltc {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Eos {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public string ClonesDepositAddress { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Xlm {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public string ClonesDepositAddress { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Etc {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public string ClonesDepositAddress { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Bat {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Tokencontract {
            public string HostCurrency { get; set; }
            public string CurrencyCode { get; set; }
            public string ContractAddress { get; set; }
            public int DecimalPlaces { get; set; }
        }

        public class Omg {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Rep {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Zrx {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Gnt {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Pla {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Pmg {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }

        public class Lnk {
            public string Code { get; set; }
            public Tokencontract TokenContract { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public string CurrencyType { get; set; }
            public string[] Entities { get; set; }
            public int SortOrder { get; set; }
            public string Family { get; set; }
            public string DepositType { get; set; }
            public int FiatPriceDecimalPlaces { get; set; }
            public string FiatPriceStringFormat { get; set; }
            public bool AlwaysEnforceMinimumWithdrawal { get; set; }
            public bool IsDelisted { get; set; }
            public string UrlProtocol { get; set; }
        }
    }
}
