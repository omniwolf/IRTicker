using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRTicker {
    class UIControls {

        public Dictionary<string, Label> Label_Dict;
        public Dictionary<string, ToolTip> ToolTip_Dict;

        public string Name { get; set; }  // internal code representing the exchange, eg "IR", "BTCM", etc

        public GroupBox dExchange_GB { get; set; }

        public Label XBT_Label { get; set; }
        public Label ETH_Label { get; set; }
        public Label BCH_Label { get; set; }
        public Label LTC_Label { get; set; }
        public Label XRP_Label { get; set; }
        public Label EOS_Label { get; set; }
        public Label OMG_Label { get; set; }
        public Label ZRX_Label { get; set; }
        public Label PLA_Label { get; set; }

        public Label XBT_Price { get; set; }
        public Label ETH_Price { get; set; }
        public Label BCH_Price { get; set; }
        public Label LTC_Price { get; set; }
        public Label XRP_Price { get; set; }
        public Label EOS_Price { get; set; }
        public Label OMG_Price { get; set; }
        public Label ZRX_Price { get; set; }
        public Label PLA_Price { get; set; }

        public ToolTip XBT_PriceTT { get; set; }
        public ToolTip ETH_PriceTT { get; set; }
        public ToolTip BCH_PriceTT { get; set; }
        public ToolTip LTC_PriceTT { get; set; }
        public ToolTip XRP_PriceTT { get; set; }
        public ToolTip EOS_PriceTT { get; set; }
        public ToolTip OMG_PriceTT { get; set; }
        public ToolTip ZRX_PriceTT { get; set; }
        public ToolTip PLA_PriceTT { get; set; }

        public ToolTip AvgPriceTT { get; set; }

        public Label XBT_Spread { get; set; }
        public Label ETH_Spread { get; set; }
        public Label BCH_Spread { get; set; }
        public Label LTC_Spread { get; set; }
        public Label XRP_Spread { get; set; }
        public Label EOS_Spread { get; set; }
        public Label OMG_Spread { get; set; }
        public Label ZRX_Spread { get; set; }
        public Label PLA_Spread { get; set; }

        public ComboBox AvgPrice_BuySell { get; set; }
        public MaskedTextBox AvgPrice_NumCoins { get; set; }
        public ComboBox AvgPrice_Crypto { get; set; }

        public Label AvgPrice { get; set; }


        public void CreateControlDictionaries() {

            // Labels
            Label_Dict = new Dictionary<string, Label>();
            Label_Dict.Add("XBT_Label", XBT_Label);
            Label_Dict.Add("ETH_Label", ETH_Label);
            Label_Dict.Add("LTC_Label", LTC_Label);
            if (XRP_Label != null) Label_Dict.Add("XRP_Label", XRP_Label);
            if (BCH_Label != null) Label_Dict.Add("BCH_Label", BCH_Label);
            if (EOS_Label != null) Label_Dict.Add("EOS_Label", EOS_Label);
            if (OMG_Label != null) Label_Dict.Add("OMG_Label", OMG_Label);
            if (ZRX_Label != null) Label_Dict.Add("ZRX_Label", ZRX_Label);
            if (PLA_Label != null) Label_Dict.Add("PLA_Label", PLA_Label);

            Label_Dict.Add("XBT_Price", XBT_Price);
            Label_Dict.Add("ETH_Price", ETH_Price);
            Label_Dict.Add("LTC_Price", LTC_Price);
            if (XRP_Price != null) Label_Dict.Add("XRP_Price", XRP_Price);
            if (BCH_Price != null) Label_Dict.Add("BCH_Price", BCH_Price);
            if (EOS_Price != null) Label_Dict.Add("EOS_Price", EOS_Price);
            if (OMG_Price != null) Label_Dict.Add("OMG_Price", OMG_Price);
            if (ZRX_Price != null) Label_Dict.Add("ZRX_Price", ZRX_Price);
            if (PLA_Price != null) Label_Dict.Add("PLA_Price", PLA_Price);

            Label_Dict.Add("XBT_Spread", XBT_Spread);
            Label_Dict.Add("ETH_Spread", ETH_Spread);
            Label_Dict.Add("LTC_Spread", LTC_Spread);
            if (XRP_Spread != null) Label_Dict.Add("XRP_Spread", XRP_Spread);
            if (BCH_Spread != null) Label_Dict.Add("BCH_Spread", BCH_Spread);
            if (EOS_Spread != null) Label_Dict.Add("EOS_Spread", EOS_Spread);
            if (OMG_Spread != null) Label_Dict.Add("OMG_Spread", OMG_Spread);
            if (ZRX_Spread != null) Label_Dict.Add("ZRX_Spread", ZRX_Spread);
            if (PLA_Spread != null) Label_Dict.Add("PLA_Spread", PLA_Spread);

            // ToolTips
            ToolTip_Dict = new Dictionary<string, ToolTip>();
            if (XBT_PriceTT != null) ToolTip_Dict.Add("XBT_PriceTT", XBT_PriceTT);
            if (ETH_PriceTT != null) ToolTip_Dict.Add("ETH_PriceTT", ETH_PriceTT);
            if (BCH_PriceTT != null) ToolTip_Dict.Add("BCH_PriceTT", BCH_PriceTT);
            if (LTC_PriceTT != null) ToolTip_Dict.Add("LTC_PriceTT", LTC_PriceTT);
            if (XRP_PriceTT != null) ToolTip_Dict.Add("XRP_PriceTT", XRP_PriceTT);
            if (EOS_PriceTT != null) ToolTip_Dict.Add("EOS_PriceTT", EOS_PriceTT);
            if (OMG_PriceTT != null) ToolTip_Dict.Add("OMG_PriceTT", OMG_PriceTT);
            if (ZRX_PriceTT != null) ToolTip_Dict.Add("ZRX_PriceTT", ZRX_PriceTT);
            if (PLA_PriceTT != null) ToolTip_Dict.Add("PLA_PriceTT", PLA_PriceTT);

            if (AvgPriceTT != null) ToolTip_Dict.Add("AvgPriceTT", AvgPriceTT);
        }
    }
}
