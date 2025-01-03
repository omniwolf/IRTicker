using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTicker.Coinbase_trade.Models {
    internal class CB_Products {
        public string id { get; set; }
        public string base_currency { get; set; }
        public string quote_currency { get; set; }
        public decimal quote_increment { get; set; }
        public decimal base_increment { get; set; }
        public string display_name { get; set; }
        public string min_market_funds { get; set; }
        public bool margin_enabled { get; set; }
        public bool post_only { get; set; }
        public bool limit_only { get; set; }
        public bool cancel_only { get; set; }
        public string status { get; set; }
        public string status_message { get; set; }
        public bool trading_disabled { get; set; }
        public bool fx_stablecoin { get; set; }
        public string max_slippage_percentage { get; set; }
        public bool auction_mode { get; set; }
        public string high_bid_limit_percentage { get; set; }
    }
}
