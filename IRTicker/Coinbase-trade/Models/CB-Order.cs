using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTicker.Coinbase_trade.Models {
    public class CB_Order {
        public string order_id { get; set; }
        public string id { get; set; }
        public string client_oid { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public decimal remaining_size { get; set; }
        public string product_id { get; set; }
        public string profile_id { get; set; }
        public string side { get; set; }
        [JsonProperty("type")]
        public string OrderType { get; set; }
        public string time_in_force { get; set; }
        public bool post_only { get; set; }
        public DateTime created_at { get; set; }
        public decimal fill_fees { get; set; }
        public decimal filled_size { get; set; }
        public decimal executed_value { get; set; }
        public string market_type { get; set; }
        public string status { get; set; }
        public bool settled { get; set; }
        public DateTime done_at { get; set; }
        public string done_reason { get; set; }
        public string funding_currency { get; set; }
        public string stp { get; set; }  // self trade protocol
        public bool isBaiter { get; set; } = false;

    }

    internal class CB_Order_matched {
        public string type { get; set; }
        public int trade_id { get; set; }
        public int sequence { get; set; }
        public string maker_order_id { get; set; }
        public string taker_order_id { get; set; }
        public DateTime time { get; set; }
        public string product_id { get; set; }
        public decimal size { get; set; }
        public decimal price { get; set; }
        public string side { get; set; }
        public string taker_user_id { get; set; }
        public string user_id { get; set; }
        public string taker_profile_id { get; set; }
        public string profile_id { get; set; }
        public decimal taker_fee_rate { get; set; }
        public string maker_user_id { get; set; }
        public string maker_profile_id { get; set; }
        public decimal maker_fee_rate { get; set; }
    }
}
