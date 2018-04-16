using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IRTicker {
    class CE {
        public class MarketSummary_Fixer {
            public bool success { get; set; }
            public double timestamp { get; set; }
            [JsonProperty("base")]  // need to do this because "base" is a c# keyword.  can't natively use it as a property.  so we use "Base", and let newtonsoft know we're referring to "base"
            public string Base { get; set; }
            public string date { get; set; }
            public Rates_Fixer rates { get; set; }
        }

        public class Rates_Fixer {
            public double GBP { get; set; }
            public double JPY { get; set; }
            public double EUR { get; set; }
            public double AUD { get; set; }
            public double NZD { get; set; }
        }

        public class MarketSummary_OER {
            public string disclaimer { get; set; }
            public string license { get; set; }
            public double timestamp { get; set; }
            [JsonProperty("base")]  // need to do this because "base" is a c# keyword.  can't natively use it as a property.  so we use "Base", and let newtonsoft know we're referring to "base"
            public string Base { get; set; }
            public Rates_OER rates { get; set; }
        }

        public class Rates_OER {
            public double GBP { get; set; }
            public double JPY { get; set; }
            public double EUR { get; set; }
            public double AUD { get; set; }
            public double NZD { get; set; }
            public double USD { get; set; }
        }
    }
    
}
