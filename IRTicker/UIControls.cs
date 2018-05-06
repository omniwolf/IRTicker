using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRTicker {
    class UIControls {

        public Dictionary<string, Label> Label_Dict;

        public string Name { get; set; }  // internal code representing the exchange, eg "IR", "BTCM", etc

        public GroupBox dExchange_GB { get; set; }

        public Label XBT_Label { get; set; }
        public Label ETH_Label { get; set; }
        public Label BCH_Label { get; set; }
        public Label LTC_Label { get; set; }
        public Label XRP_Label { get; set; }

        public Label XBT_Price { get; set; }
        public Label ETH_Price { get; set; }
        public Label BCH_Price { get; set; }
        public Label LTC_Price { get; set; }
        public Label XRP_Price { get; set; }

        public Label XBT_Spread { get; set; }
        public Label ETH_Spread { get; set; }
        public Label BCH_Spread { get; set; }
        public Label LTC_Spread { get; set; }
        public Label XRP_Spread { get; set; }

        public ComboBox AvgPrice_BuySell { get; set; }
        public MaskedTextBox AvgPrice_NumCoins { get; set; }
        public ComboBox AvgPrice_Crypto { get; set; }

        public Label AvgPrice { get; set; }


        public void createControlDictionaries() {
            Label_Dict = new Dictionary<string, Label>();
            Label_Dict.Add("XBT_Label", XBT_Label);
            Label_Dict.Add("ETH_Label", ETH_Label);
            Label_Dict.Add("BCH_Label", BCH_Label);
            Label_Dict.Add("LTC_Label", LTC_Label);
            if (XRP_Label != null) Label_Dict.Add("XRP_Label", XRP_Label);

            Label_Dict.Add("XBT_Price", XBT_Price);
            Label_Dict.Add("ETH_Price", ETH_Price);
            Label_Dict.Add("BCH_Price", BCH_Price);
            Label_Dict.Add("LTC_Price", LTC_Price);
            if (XRP_Price != null) Label_Dict.Add("XRP_Price", XRP_Price);

            Label_Dict.Add("XBT_Spread", XBT_Spread);
            Label_Dict.Add("ETH_Spread", ETH_Spread);
            Label_Dict.Add("BCH_Spread", BCH_Spread);
            Label_Dict.Add("LTC_Spread", LTC_Spread);
            if (XRP_Spread != null) Label_Dict.Add("XRP_Spread", XRP_Spread);
        }
    }
}
