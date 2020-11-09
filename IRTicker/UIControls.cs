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
        public Label XLM_Label { get; set; }
        public Label BAT_Label { get; set; }
        public Label MKR_Label { get; set; }
        public Label GNT_Label { get; set; }
        public Label ETC_Label { get; set; }
        public Label USDT_Label { get; set; }
        public Label BSV_Label { get; set; }
        public Label DAI_Label { get; set; }
        public Label LINK_Label { get; set; }
        public Label USDC_Label { get; set; }
        public Label COMP_Label { get; set; }
        public Label SNX_Label { get; set; }
        public Label PMGT_Label { get; set; }
        public Label YFI_Label { get; set; }
        public Label AAVE_Label { get; set; }
        public Label KNC_Label { get; set; }

        public Label XBT_Price { get; set; }
        public Label ETH_Price { get; set; }
        public Label BCH_Price { get; set; }
        public Label LTC_Price { get; set; }
        public Label XRP_Price { get; set; }
        public Label EOS_Price { get; set; }
        public Label OMG_Price { get; set; }
        public Label ZRX_Price { get; set; }
        public Label XLM_Price { get; set; }
        public Label BAT_Price { get; set; }
        public Label MKR_Price { get; set; }
        public Label GNT_Price { get; set; }
        public Label ETC_Price { get; set; }
        public Label USDT_Price { get; set; }
        public Label BSV_Price { get; set; }
        public Label DAI_Price { get; set; }
        public Label LINK_Price { get; set; }
        public Label USDC_Price { get; set; }
        public Label COMP_Price { get; set; }
        public Label SNX_Price { get; set; }
        public Label PMGT_Price { get; set; }
        public Label YFI_Price { get; set; }
        public Label AAVE_Price { get; set; }
        public Label KNC_Price { get; set; }

        public Label XBT_Spread { get; set; }
        public Label ETH_Spread { get; set; }
        public Label BCH_Spread { get; set; }
        public Label LTC_Spread { get; set; }
        public Label XRP_Spread { get; set; }
        public Label EOS_Spread { get; set; }
        public Label OMG_Spread { get; set; }
        public Label ZRX_Spread { get; set; }
        public Label XLM_Spread { get; set; }
        public Label BAT_Spread { get; set; }
        public Label MKR_Spread { get; set; }
        public Label GNT_Spread { get; set; }
        public Label ETC_Spread { get; set; }
        public Label USDT_Spread { get; set; }
        public Label BSV_Spread { get; set; }
        public Label DAI_Spread { get; set; }
        public Label LINK_Spread { get; set; }
        public Label USDC_Spread { get; set; }
        public Label COMP_Spread { get; set; }
        public Label SNX_Spread { get; set; }
        public Label PMGT_Spread { get; set; }
        public Label YFI_Spread { get; set; }
        public Label AAVE_Spread { get; set; }
        public Label KNC_Spread { get; set; }

        public ComboBox AvgPrice_BuySell { get; set; }
        public MaskedTextBox AvgPrice_NumCoins { get; set; }
        public ComboBox AvgPrice_Crypto { get; set; }
        public ComboBox AvgPrice_Currency { get; set; }  // whether or not the coins they're entering are crypto or fiat

        public Label AvgPrice { get; set; }



        // IR account stuff, private API

        public Label Account_XBT_Label { get; set; }
        public Label Account_ETH_Label { get; set; }
        public Label Account_XRP_Label { get; set; }
        public Label Account_BCH_Label { get; set; }
        public Label Account_BSV_Label { get; set; }
        public Label Account_USDT_Label { get; set; }
        public Label Account_LTC_Label { get; set; }
        public Label Account_EOS_Label { get; set; }
        public Label Account_XLM_Label { get; set; }
        public Label Account_ETC_Label { get; set; }
        public Label Account_BAT_Label { get; set; }
        public Label Account_OMG_Label { get; set; }
        public Label Account_MKR_Label { get; set; }
        public Label Account_ZRX_Label { get; set; }
        public Label Account_GNT_Label { get; set; }
        public Label Account_DAI_Label { get; set; }
        public Label Account_LINK_Label { get; set; }
        public Label Account_USDC_Label { get; set; }
        public Label Account_AUD_Label { get; set; }
        public Label Account_NZD_Label { get; set; }
        public Label Account_USD_Label { get; set; }
        public Label Account_COMP_Label { get; set; }
        public Label Account_SNX_Label { get; set; }
        public Label Account_PMGT_Label { get; set; }
        public Label Account_YFI_Label { get; set; }
        public Label Account_AAVE_Label { get; set; }
        public Label Account_KNC_Label { get; set; }

        public Label Account_XBT_Value { get; set; }
        public Label Account_ETH_Value { get; set; }
        public Label Account_XRP_Value { get; set; }
        public Label Account_BCH_Value { get; set; }
        public Label Account_BSV_Value { get; set; }
        public Label Account_USDT_Value { get; set; }
        public Label Account_LTC_Value { get; set; }
        public Label Account_EOS_Value { get; set; }
        public Label Account_XLM_Value { get; set; }
        public Label Account_ETC_Value { get; set; }
        public Label Account_BAT_Value { get; set; }
        public Label Account_OMG_Value { get; set; }
        public Label Account_MKR_Value { get; set; }
        public Label Account_ZRX_Value { get; set; }
        public Label Account_GNT_Value { get; set; }
        public Label Account_DAI_Value { get; set; }
        public Label Account_LINK_Value { get; set; }
        public Label Account_USDC_Value { get; set; }
        public Label Account_COMP_Value { get; set; }
        public Label Account_SNX_Value { get; set; }
        public Label Account_PMGT_Value { get; set; }
        public Label Account_YFI_Value { get; set; }
        public Label Account_AAVE_Value { get; set; }
        public Label Account_KNC_Value { get; set; }

        public Label Account_XBT_Total { get; set; }
        public Label Account_ETH_Total { get; set; }
        public Label Account_XRP_Total { get; set; }
        public Label Account_BCH_Total { get; set; }
        public Label Account_BSV_Total { get; set; }
        public Label Account_USDT_Total { get; set; }
        public Label Account_LTC_Total { get; set; }
        public Label Account_EOS_Total { get; set; }
        public Label Account_XLM_Total { get; set; }
        public Label Account_ETC_Total { get; set; }
        public Label Account_BAT_Total { get; set; }
        public Label Account_OMG_Total { get; set; }
        public Label Account_MKR_Total { get; set; }
        public Label Account_ZRX_Total { get; set; }
        public Label Account_GNT_Total { get; set; }
        public Label Account_DAI_Total { get; set; }
        public Label Account_LINK_Total { get; set; }
        public Label Account_USDC_Total { get; set; }
        public Label Account_AUD_Total { get; set; }
        public Label Account_NZD_Total { get; set; }
        public Label Account_USD_Total { get; set; }
        public Label Account_COMP_Total { get; set; }
        public Label Account_SNX_Total { get; set; }
        public Label Account_PMGT_Total { get; set; }
        public Label Account_YFI_Total { get; set; }
        public Label Account_AAVE_Total { get; set; }
        public Label Account_KNC_Total { get; set; }



        public void CreateControlDictionaries() {

            // Labels
            Label_Dict = new Dictionary<string, Label>();
            if (XBT_Label != null) Label_Dict.Add("XBT_Label", XBT_Label);
            if (ETH_Label != null) Label_Dict.Add("ETH_Label", ETH_Label);
            if (LTC_Label != null) Label_Dict.Add("LTC_Label", LTC_Label);
            if (XRP_Label != null) Label_Dict.Add("XRP_Label", XRP_Label);
            if (BCH_Label != null) Label_Dict.Add("BCH_Label", BCH_Label);
            if (EOS_Label != null) Label_Dict.Add("EOS_Label", EOS_Label);
            if (OMG_Label != null) Label_Dict.Add("OMG_Label", OMG_Label);
            if (ZRX_Label != null) Label_Dict.Add("ZRX_Label", ZRX_Label);
            if (XLM_Label != null) Label_Dict.Add("XLM_Label", XLM_Label);
            if (BAT_Label != null) Label_Dict.Add("BAT_Label", BAT_Label);
            if (MKR_Label != null) Label_Dict.Add("MKR_Label", MKR_Label);
            if (GNT_Label != null) Label_Dict.Add("GNT_Label", GNT_Label);
            if (ETC_Label != null) Label_Dict.Add("ETC_Label", ETC_Label);
            if (USDT_Label != null) Label_Dict.Add("USDT_Label", USDT_Label);
            if (BSV_Label != null) Label_Dict.Add("BSV_Label", BSV_Label);
            if (DAI_Label != null) Label_Dict.Add("DAI_Label", DAI_Label);
            if (LINK_Label != null) Label_Dict.Add("LINK_Label", LINK_Label);
            if (USDC_Label != null) Label_Dict.Add("USDC_Label", USDC_Label);
            if (COMP_Label != null) Label_Dict.Add("COMP_Label", COMP_Label);
            if (SNX_Label != null) Label_Dict.Add("SNX_Label", SNX_Label);
            if (PMGT_Label != null) Label_Dict.Add("PMGT_Label", PMGT_Label);
            if (YFI_Label != null) Label_Dict.Add("YFI_Label", YFI_Label);
            if (AAVE_Label != null) Label_Dict.Add("AAVE_Label", AAVE_Label);
            if (KNC_Label != null) Label_Dict.Add("KNC_Label", KNC_Label);

            if (XBT_Price != null) Label_Dict.Add("XBT_Price", XBT_Price);
            if (ETH_Price != null) Label_Dict.Add("ETH_Price", ETH_Price);
            if (LTC_Price != null) Label_Dict.Add("LTC_Price", LTC_Price);
            if (XRP_Price != null) Label_Dict.Add("XRP_Price", XRP_Price);
            if (BCH_Price != null) Label_Dict.Add("BCH_Price", BCH_Price);
            if (EOS_Price != null) Label_Dict.Add("EOS_Price", EOS_Price);
            if (OMG_Price != null) Label_Dict.Add("OMG_Price", OMG_Price);
            if (ZRX_Price != null) Label_Dict.Add("ZRX_Price", ZRX_Price);
            if (XLM_Price != null) Label_Dict.Add("XLM_Price", XLM_Price);
            if (BAT_Price != null) Label_Dict.Add("BAT_Price", BAT_Price);
            if (MKR_Price != null) Label_Dict.Add("MKR_Price", MKR_Price);
            if (GNT_Price != null) Label_Dict.Add("GNT_Price", GNT_Price);
            if (ETC_Price != null) Label_Dict.Add("ETC_Price", ETC_Price);
            if (USDT_Price != null) Label_Dict.Add("USDT_Price", USDT_Price);
            if (BSV_Price != null) Label_Dict.Add("BSV_Price", BSV_Price);
            if (DAI_Price != null) Label_Dict.Add("DAI_Price", DAI_Price);
            if (LINK_Price != null) Label_Dict.Add("LINK_Price", LINK_Price);
            if (USDC_Price != null) Label_Dict.Add("USDC_Price", USDC_Price);
            if (COMP_Price != null) Label_Dict.Add("COMP_Price", COMP_Price);
            if (SNX_Price != null) Label_Dict.Add("SNX_Price", SNX_Price);
            if (PMGT_Price != null) Label_Dict.Add("PMGT_Price", PMGT_Price);
            if (YFI_Price != null) Label_Dict.Add("YFI_Price", YFI_Price);
            if (AAVE_Price != null) Label_Dict.Add("AAVE_Price", AAVE_Price);
            if (KNC_Price != null) Label_Dict.Add("KNC_Price", KNC_Price);

            if (XBT_Spread != null) Label_Dict.Add("XBT_Spread", XBT_Spread);
            if (ETH_Spread != null) Label_Dict.Add("ETH_Spread", ETH_Spread);
            if (LTC_Spread != null) Label_Dict.Add("LTC_Spread", LTC_Spread);
            if (XRP_Spread != null) Label_Dict.Add("XRP_Spread", XRP_Spread);
            if (BCH_Spread != null) Label_Dict.Add("BCH_Spread", BCH_Spread);
            if (EOS_Spread != null) Label_Dict.Add("EOS_Spread", EOS_Spread);
            if (OMG_Spread != null) Label_Dict.Add("OMG_Spread", OMG_Spread);
            if (ZRX_Spread != null) Label_Dict.Add("ZRX_Spread", ZRX_Spread);
            if (XLM_Spread != null) Label_Dict.Add("XLM_Spread", XLM_Spread);
            if (BAT_Spread != null) Label_Dict.Add("BAT_Spread", BAT_Spread);
            if (MKR_Spread != null) Label_Dict.Add("MKR_Spread", MKR_Spread);
            if (GNT_Spread != null) Label_Dict.Add("GNT_Spread", GNT_Spread);
            if (ETC_Spread != null) Label_Dict.Add("ETC_Spread", ETC_Spread);
            if (USDT_Spread != null) Label_Dict.Add("USDT_Spread", USDT_Spread);
            if (BSV_Spread != null) Label_Dict.Add("BSV_Spread", BSV_Spread);
            if (DAI_Spread != null) Label_Dict.Add("DAI_Spread", DAI_Spread);
            if (LINK_Spread != null) Label_Dict.Add("LINK_Spread", LINK_Spread);
            if (USDC_Spread != null) Label_Dict.Add("USDC_Spread", USDC_Spread);
            if (COMP_Spread != null) Label_Dict.Add("COMP_Spread", COMP_Spread);
            if (SNX_Spread != null) Label_Dict.Add("SNX_Spread", SNX_Spread);
            if (PMGT_Spread != null) Label_Dict.Add("PMGT_Spread", PMGT_Spread);
            if (YFI_Spread != null) Label_Dict.Add("YFI_Spread", YFI_Spread);
            if (AAVE_Spread != null) Label_Dict.Add("AAVE_Spread", AAVE_Spread);
            if (KNC_Spread != null) Label_Dict.Add("KNC_Spread", KNC_Spread);



            if (Account_XBT_Label != null) Label_Dict.Add("XBT_Account_Label", Account_XBT_Label);
            if (Account_ETH_Label != null) Label_Dict.Add("ETH_Account_Label", Account_ETH_Label);
            if (Account_XRP_Label != null) Label_Dict.Add("XRP_Account_Label", Account_XRP_Label);
            if (Account_BCH_Label != null) Label_Dict.Add("BCH_Account_Label", Account_BCH_Label);
            if (Account_BSV_Label != null) Label_Dict.Add("BSV_Account_Label", Account_BSV_Label);
            if (Account_USDT_Label != null) Label_Dict.Add("USDT_Account_Label", Account_USDT_Label);
            if (Account_LTC_Label != null) Label_Dict.Add("LTC_Account_Label", Account_LTC_Label);
            if (Account_EOS_Label != null) Label_Dict.Add("EOS_Account_Label", Account_EOS_Label);
            if (Account_XLM_Label != null) Label_Dict.Add("XLM_Account_Label", Account_XLM_Label);
            if (Account_ETC_Label != null) Label_Dict.Add("ETC_Account_Label", Account_ETC_Label);
            if (Account_BAT_Label != null) Label_Dict.Add("BAT_Account_Label", Account_BAT_Label);
            if (Account_OMG_Label != null) Label_Dict.Add("OMG_Account_Label", Account_OMG_Label);
            if (Account_MKR_Label != null) Label_Dict.Add("MKR_Account_Label", Account_MKR_Label);
            if (Account_ZRX_Label != null) Label_Dict.Add("ZRX_Account_Label", Account_ZRX_Label);
            if (Account_GNT_Label != null) Label_Dict.Add("GNT_Account_Label", Account_GNT_Label);
            if (Account_DAI_Label != null) Label_Dict.Add("DAI_Account_Label", Account_DAI_Label);
            if (Account_LINK_Label != null) Label_Dict.Add("LINK_Account_Label", Account_LINK_Label);
            if (Account_USDC_Label != null) Label_Dict.Add("USDC_Account_Label", Account_USDC_Label);
            if (Account_AUD_Label != null) Label_Dict.Add("AUD_Account_Label", Account_AUD_Label);
            if (Account_NZD_Label != null) Label_Dict.Add("NZD_Account_Label", Account_NZD_Label);
            if (Account_USD_Label != null) Label_Dict.Add("USD_Account_Label", Account_USD_Label);
            if (Account_COMP_Label != null) Label_Dict.Add("COMP_Account_Label", Account_COMP_Label);
            if (Account_SNX_Label != null) Label_Dict.Add("SNX_Account_Label", Account_SNX_Label);
            if (Account_PMGT_Label != null) Label_Dict.Add("PMGT_Account_Label", Account_PMGT_Label);
            if (Account_YFI_Label != null) Label_Dict.Add("YFI_Account_Label", Account_YFI_Label);
            if (Account_AAVE_Label != null) Label_Dict.Add("AAVE_Account_Label", Account_AAVE_Label);
            if (Account_KNC_Label != null) Label_Dict.Add("KNC_Account_Label", Account_KNC_Label);

            if (Account_XBT_Value != null) Label_Dict.Add("XBT_Account_Value", Account_XBT_Value);
            if (Account_ETH_Value != null) Label_Dict.Add("ETH_Account_Value", Account_ETH_Value);
            if (Account_XRP_Value != null) Label_Dict.Add("XRP_Account_Value", Account_XRP_Value);
            if (Account_BCH_Value != null) Label_Dict.Add("BCH_Account_Value", Account_BCH_Value);
            if (Account_BSV_Value != null) Label_Dict.Add("BSV_Account_Value", Account_BSV_Value);
            if (Account_USDT_Value != null) Label_Dict.Add("USDT_Account_Value", Account_USDT_Value);
            if (Account_LTC_Value != null) Label_Dict.Add("LTC_Account_Value", Account_LTC_Value);
            if (Account_EOS_Value != null) Label_Dict.Add("EOS_Account_Value", Account_EOS_Value);
            if (Account_XLM_Value != null) Label_Dict.Add("XLM_Account_Value", Account_XLM_Value);
            if (Account_ETC_Value != null) Label_Dict.Add("ETC_Account_Value", Account_ETC_Value);
            if (Account_BAT_Value != null) Label_Dict.Add("BAT_Account_Value", Account_BAT_Value);
            if (Account_OMG_Value != null) Label_Dict.Add("OMG_Account_Value", Account_OMG_Value);
            if (Account_MKR_Value != null) Label_Dict.Add("MKR_Account_Value", Account_MKR_Value);
            if (Account_ZRX_Value != null) Label_Dict.Add("ZRX_Account_Value", Account_ZRX_Value);
            if (Account_GNT_Value != null) Label_Dict.Add("GNT_Account_Value", Account_GNT_Value);
            if (Account_DAI_Value != null) Label_Dict.Add("DAI_Account_Value", Account_DAI_Value);
            if (Account_LINK_Value != null) Label_Dict.Add("LINK_Account_Value", Account_LINK_Value);
            if (Account_USDC_Value != null) Label_Dict.Add("USDC_Account_Value", Account_USDC_Value);
            if (Account_COMP_Value != null) Label_Dict.Add("COMP_Account_Value", Account_COMP_Value);
            if (Account_SNX_Value != null) Label_Dict.Add("SNX_Account_Value", Account_SNX_Value);
            if (Account_PMGT_Value != null) Label_Dict.Add("PMGT_Account_Value", Account_PMGT_Value);
            if (Account_YFI_Value != null) Label_Dict.Add("YFI_Account_Value", Account_YFI_Value);
            if (Account_AAVE_Value != null) Label_Dict.Add("AAVE_Account_Value", Account_AAVE_Value);
            if (Account_KNC_Value != null) Label_Dict.Add("KNC_Account_Value", Account_KNC_Value);

            if (Account_XBT_Total != null) Label_Dict.Add("XBT_Account_Total", Account_XBT_Total);
            if (Account_ETH_Total != null) Label_Dict.Add("ETH_Account_Total", Account_ETH_Total);
            if (Account_XRP_Total != null) Label_Dict.Add("XRP_Account_Total", Account_XRP_Total);
            if (Account_BCH_Total != null) Label_Dict.Add("BCH_Account_Total", Account_BCH_Total);
            if (Account_BSV_Total != null) Label_Dict.Add("BSV_Account_Total", Account_BSV_Total);
            if (Account_USDT_Total != null) Label_Dict.Add("USDT_Account_Total", Account_USDT_Total);
            if (Account_LTC_Total != null) Label_Dict.Add("LTC_Account_Total", Account_LTC_Total);
            if (Account_EOS_Total != null) Label_Dict.Add("EOS_Account_Total", Account_EOS_Total);
            if (Account_XLM_Total != null) Label_Dict.Add("XLM_Account_Total", Account_XLM_Total);
            if (Account_ETC_Total != null) Label_Dict.Add("ETC_Account_Total", Account_ETC_Total);
            if (Account_BAT_Total != null) Label_Dict.Add("BAT_Account_Total", Account_BAT_Total);
            if (Account_OMG_Total != null) Label_Dict.Add("OMG_Account_Total", Account_OMG_Total);
            if (Account_MKR_Total != null) Label_Dict.Add("MKR_Account_Total", Account_MKR_Total);
            if (Account_ZRX_Total != null) Label_Dict.Add("ZRX_Account_Total", Account_ZRX_Total);
            if (Account_GNT_Total != null) Label_Dict.Add("GNT_Account_Total", Account_GNT_Total);
            if (Account_DAI_Total != null) Label_Dict.Add("DAI_Account_Total", Account_DAI_Total);
            if (Account_LINK_Total != null) Label_Dict.Add("LINK_Account_Total", Account_LINK_Total);
            if (Account_USDC_Total != null) Label_Dict.Add("USDC_Account_Total", Account_USDC_Total);
            if (Account_AUD_Total != null) Label_Dict.Add("AUD_Account_Total", Account_AUD_Total);
            if (Account_NZD_Total != null) Label_Dict.Add("NZD_Account_Total", Account_NZD_Total);
            if (Account_USD_Total != null) Label_Dict.Add("USD_Account_Total", Account_USD_Total);
            if (Account_COMP_Total != null) Label_Dict.Add("COMP_Account_Total", Account_COMP_Total);
            if (Account_SNX_Total != null) Label_Dict.Add("SNX_Account_Total", Account_SNX_Total);
            if (Account_PMGT_Total != null) Label_Dict.Add("PMGT_Account_Total", Account_PMGT_Total);
            if (Account_YFI_Total != null) Label_Dict.Add("YFI_Account_Total", Account_YFI_Total);
            if (Account_AAVE_Total != null) Label_Dict.Add("AAVE_Account_Total", Account_AAVE_Total);
            if (Account_KNC_Total != null) Label_Dict.Add("KNC_Account_Total", Account_KNC_Total);
        }
    }
}
