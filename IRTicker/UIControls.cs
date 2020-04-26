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
        public Label EOS_Label { get; set; }
        public Label OMG_Label { get; set; }
        public Label ZRX_Label { get; set; }
        public Label PLA_Label { get; set; }
        public Label XLM_Label { get; set; }
        public Label BAT_Label { get; set; }
        public Label REP_Label { get; set; }
        public Label GNT_Label { get; set; }
        public Label ETC_Label { get; set; }
        public Label USDT_Label { get; set; }
        public Label BSV_Label { get; set; }

        public Label XBT_Price { get; set; }
        public Label ETH_Price { get; set; }
        public Label BCH_Price { get; set; }
        public Label LTC_Price { get; set; }
        public Label XRP_Price { get; set; }
        public Label EOS_Price { get; set; }
        public Label OMG_Price { get; set; }
        public Label ZRX_Price { get; set; }
        public Label PLA_Price { get; set; }
        public Label XLM_Price { get; set; }
        public Label BAT_Price { get; set; }
        public Label REP_Price { get; set; }
        public Label GNT_Price { get; set; }
        public Label ETC_Price { get; set; }
        public Label USDT_Price { get; set; }
        public Label BSV_Price { get; set; }

        public Label XBT_Spread { get; set; }
        public Label ETH_Spread { get; set; }
        public Label BCH_Spread { get; set; }
        public Label LTC_Spread { get; set; }
        public Label XRP_Spread { get; set; }
        public Label EOS_Spread { get; set; }
        public Label OMG_Spread { get; set; }
        public Label ZRX_Spread { get; set; }
        public Label PLA_Spread { get; set; }
        public Label XLM_Spread { get; set; }
        public Label BAT_Spread { get; set; }
        public Label REP_Spread { get; set; }
        public Label GNT_Spread { get; set; }
        public Label ETC_Spread { get; set; }
        public Label USDT_Spread { get; set; }
        public Label BSV_Spread { get; set; }

        public ComboBox AvgPrice_BuySell { get; set; }
        public MaskedTextBox AvgPrice_NumCoins { get; set; }
        public ComboBox AvgPrice_Crypto { get; set; }
        public ComboBox AvgPrice_Currency { get; set; }  // whether or not the coins they're entering are crypto or fiat

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
            if (XLM_Label != null) Label_Dict.Add("XLM_Label", XLM_Label);
            if (BAT_Label != null) Label_Dict.Add("BAT_Label", BAT_Label);
            if (REP_Label != null) Label_Dict.Add("REP_Label", REP_Label);
            if (GNT_Label != null) Label_Dict.Add("GNT_Label", GNT_Label);
            if (ETC_Label != null) Label_Dict.Add("ETC_Label", ETC_Label);
            if (USDT_Label != null) Label_Dict.Add("USDT_Label", USDT_Label);
            if (BSV_Label != null) Label_Dict.Add("BSV_Label", BSV_Label);

            Label_Dict.Add("XBT_Price", XBT_Price);
            Label_Dict.Add("ETH_Price", ETH_Price);
            Label_Dict.Add("LTC_Price", LTC_Price);
            if (XRP_Price != null) Label_Dict.Add("XRP_Price", XRP_Price);
            if (BCH_Price != null) Label_Dict.Add("BCH_Price", BCH_Price);
            if (EOS_Price != null) Label_Dict.Add("EOS_Price", EOS_Price);
            if (OMG_Price != null) Label_Dict.Add("OMG_Price", OMG_Price);
            if (ZRX_Price != null) Label_Dict.Add("ZRX_Price", ZRX_Price);
            if (PLA_Price != null) Label_Dict.Add("PLA_Price", PLA_Price);
            if (XLM_Price != null) Label_Dict.Add("XLM_Price", XLM_Price);
            if (BAT_Price != null) Label_Dict.Add("BAT_Price", BAT_Price);
            if (REP_Price != null) Label_Dict.Add("REP_Price", REP_Price);
            if (GNT_Price != null) Label_Dict.Add("GNT_Price", GNT_Price);
            if (ETC_Price != null) Label_Dict.Add("ETC_Price", ETC_Price);
            if (USDT_Price != null) Label_Dict.Add("USDT_Price", USDT_Price);
            if (BSV_Price != null) Label_Dict.Add("BSV_Price", BSV_Price);

            Label_Dict.Add("XBT_Spread", XBT_Spread);
            Label_Dict.Add("ETH_Spread", ETH_Spread);
            Label_Dict.Add("LTC_Spread", LTC_Spread);
            if (XRP_Spread != null) Label_Dict.Add("XRP_Spread", XRP_Spread);
            if (BCH_Spread != null) Label_Dict.Add("BCH_Spread", BCH_Spread);
            if (EOS_Spread != null) Label_Dict.Add("EOS_Spread", EOS_Spread);
            if (OMG_Spread != null) Label_Dict.Add("OMG_Spread", OMG_Spread);
            if (ZRX_Spread != null) Label_Dict.Add("ZRX_Spread", ZRX_Spread);
            if (PLA_Spread != null) Label_Dict.Add("PLA_Spread", PLA_Spread);
            if (XLM_Spread != null) Label_Dict.Add("XLM_Spread", XLM_Spread);
            if (BAT_Spread != null) Label_Dict.Add("BAT_Spread", BAT_Spread);
            if (REP_Spread != null) Label_Dict.Add("REP_Spread", REP_Spread);
            if (GNT_Spread != null) Label_Dict.Add("GNT_Spread", GNT_Spread);
            if (ETC_Spread != null) Label_Dict.Add("ETC_Spread", ETC_Spread);
            if (USDT_Spread != null) Label_Dict.Add("USDT_Spread", USDT_Spread);
            if (BSV_Spread != null) Label_Dict.Add("BSV_Spread", BSV_Spread);
        }
    }
}
