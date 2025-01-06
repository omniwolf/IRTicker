using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTicker.Coinbase_trade.Models {
    public class CB_Accounts {
        public string id { get; set; }
        public string currency { get; set; }
        public decimal balance { get; set; }
        public string hold { get; set; }
        public decimal available { get; set; }
        public string profile_id { get; set; }
        public bool trading_enabled { get; set; }
        public string pending_deposit { get; set; }
        public string display_name { get; set; }
    }
}
