using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRTicker
{
    public class UIControls
    {

        public Dictionary<string, Label> Label_Dict;

        public string Name { get; set; }  // internal code representing the exchange, eg "IR", "BTCM", etc

        public GroupBox dExchange_GB { get; set; }

        public Label XBT_Label { get; set; }
        public Label ETH_Label { get; set; }
        public Label BCH_Label { get; set; }
        public Label LTC_Label { get; set; }
        public Label XRP_Label { get; set; }
        public Label EOS_Label { get; set; }
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
        public Label YFI_Label { get; set; }
        public Label AAVE_Label { get; set; }
        public Label KNC_Label { get; set; }
        public Label DOT_Label { get; set; }
        public Label GRT_Label { get; set; }
        public Label UNI_Label { get; set; }
        public Label ADA_Label { get; set; }
        public Label DOGE_Label { get; set; }
        public Label MATIC_Label { get; set; }
        public Label MANA_Label { get; set; }
        public Label SOL_Label { get; set; }
        public Label SAND_Label { get; set; }
        public Label SHIB_Label { get; set; }
        public Label TRX_Label { get; set; }
        public Label RENDER_Label { get; set; }
        public Label RLUSD_Label { get; set; }
        public Label WIF_Label { get; set; }

        public Label XBT_Price { get; set; }
        public Label ETH_Price { get; set; }
        public Label BCH_Price { get; set; }
        public Label LTC_Price { get; set; }
        public Label XRP_Price { get; set; }
        public Label EOS_Price { get; set; }
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
        public Label YFI_Price { get; set; }
        public Label AAVE_Price { get; set; }
        public Label KNC_Price { get; set; }
        public Label DOT_Price { get; set; }
        public Label GRT_Price { get; set; }
        public Label UNI_Price { get; set; }
        public Label ADA_Price { get; set; }
        public Label DOGE_Price { get; set; }
        public Label MATIC_Price { get; set; }
        public Label MANA_Price { get; set; }
        public Label SOL_Price { get; set; }
        public Label SAND_Price { get; set; }
        public Label SHIB_Price { get; set; }
        public Label TRX_Price { get; set; }
        public Label RENDER_Price { get; set; }
        public Label RLUSD_Price { get; set; }
        public Label WIF_Price { get; set; }


        public Label XBT_Spread { get; set; }
        public Label ETH_Spread { get; set; }
        public Label BCH_Spread { get; set; }
        public Label LTC_Spread { get; set; }
        public Label XRP_Spread { get; set; }
        public Label EOS_Spread { get; set; }
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
        public Label YFI_Spread { get; set; }
        public Label AAVE_Spread { get; set; }
        public Label KNC_Spread { get; set; }
        public Label DOT_Spread { get; set; }
        public Label GRT_Spread { get; set; }
        public Label UNI_Spread { get; set; }
        public Label ADA_Spread { get; set; }
        public Label DOGE_Spread { get; set; }
        public Label MATIC_Spread { get; set; }
        public Label MANA_Spread { get; set; }
        public Label SOL_Spread { get; set; }
        public Label SAND_Spread { get; set; }
        public Label SHIB_Spread { get; set; }
        public Label TRX_Spread { get; set; }
        public Label RENDER_Spread { get; set; }
        public Label RLUSD_Spread { get; set; }
        public Label WIF_Spread { get; set; }


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
        public Label Account_SGD_Label { get; set; }
        public Label Account_COMP_Label { get; set; }
        public Label Account_SNX_Label { get; set; }
        public Label Account_PMGT_Label { get; set; }
        public Label Account_YFI_Label { get; set; }
        public Label Account_AAVE_Label { get; set; }
        public Label Account_KNC_Label { get; set; }
        public Label Account_DOT_Label { get; set; }
        public Label Account_GRT_Label { get; set; }
        public Label Account_UNI_Label { get; set; }
        public Label Account_ADA_Label { get; set; }
        public Label Account_DOGE_Label { get; set; }
        public Label Account_MATIC_Label { get; set; }
        public Label Account_MANA_Label { get; set; }
        public Label Account_SOL_Label { get; set; }
        public Label Account_SAND_Label { get; set; }
        public Label Account_SHIB_Label { get; set; }
        public Label Account_TRX_Label { get; set; }
        public Label Account_RENDER_Label { get; set; }
        public Label Account_RLUSD_Label { get; set; }
        public Label Account_WIF_Label { get; set; }

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
        public Label Account_DOT_Value { get; set; }
        public Label Account_GRT_Value { get; set; }
        public Label Account_UNI_Value { get; set; }
        public Label Account_ADA_Value { get; set; }
        public Label Account_DOGE_Value { get; set; }
        public Label Account_MATIC_Value { get; set; }
        public Label Account_MANA_Value { get; set; }
        public Label Account_SOL_Value { get; set; }
        public Label Account_SAND_Value { get; set; }
        public Label Account_SHIB_Value { get; set; }
        public Label Account_TRX_Value { get; set; }
        public Label Account_RENDER_Value { get; set; }
        public Label Account_RLUSD_Value { get; set; }
        public Label Account_WIF_Value { get; set; }


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
        public Label Account_SGD_Total { get; set; }
        public Label Account_COMP_Total { get; set; }
        public Label Account_SNX_Total { get; set; }
        public Label Account_PMGT_Total { get; set; }
        public Label Account_YFI_Total { get; set; }
        public Label Account_AAVE_Total { get; set; }
        public Label Account_KNC_Total { get; set; }
        public Label Account_DOT_Total { get; set; }
        public Label Account_GRT_Total { get; set; }
        public Label Account_UNI_Total { get; set; }
        public Label Account_ADA_Total { get; set; }
        public Label Account_DOGE_Total { get; set; }
        public Label Account_MATIC_Total { get; set; }
        public Label Account_MANA_Total { get; set; }
        public Label Account_SOL_Total { get; set; }
        public Label Account_SAND_Total { get; set; }
        public Label Account_SHIB_Total { get; set; }
        public Label Account_TRX_Total { get; set; }
        public Label Account_RENDER_Total { get; set; }
        public Label Account_RLUSD_Total { get; set; }
        public Label Account_WIF_Total { get; set; }



        public void CreateIRAccountControlDictionary()
        {

            if (Account_XBT_Label != null) Label_Dict["XBT_Account_Label"] = Account_XBT_Label;
            if (Account_ETH_Label != null) Label_Dict["ETH_Account_Label"] = Account_ETH_Label;
            if (Account_XRP_Label != null) Label_Dict["XRP_Account_Label"] = Account_XRP_Label;
            if (Account_BCH_Label != null) Label_Dict["BCH_Account_Label"] = Account_BCH_Label;
            if (Account_BSV_Label != null) Label_Dict["BSV_Account_Label"] = Account_BSV_Label;
            if (Account_USDT_Label != null) Label_Dict["USDT_Account_Label"] = Account_USDT_Label;
            if (Account_LTC_Label != null) Label_Dict["LTC_Account_Label"] = Account_LTC_Label;
            if (Account_EOS_Label != null) Label_Dict["EOS_Account_Label"] = Account_EOS_Label;
            if (Account_XLM_Label != null) Label_Dict["XLM_Account_Label"] = Account_XLM_Label;
            if (Account_ETC_Label != null) Label_Dict["ETC_Account_Label"] = Account_ETC_Label;
            if (Account_BAT_Label != null) Label_Dict["BAT_Account_Label"] = Account_BAT_Label;
            if (Account_OMG_Label != null) Label_Dict["OMG_Account_Label"] = Account_OMG_Label;
            if (Account_MKR_Label != null) Label_Dict["MKR_Account_Label"] = Account_MKR_Label;
            if (Account_ZRX_Label != null) Label_Dict["ZRX_Account_Label"] = Account_ZRX_Label;
            if (Account_GNT_Label != null) Label_Dict["GNT_Account_Label"] = Account_GNT_Label;
            if (Account_DAI_Label != null) Label_Dict["DAI_Account_Label"] = Account_DAI_Label;
            if (Account_LINK_Label != null) Label_Dict["LINK_Account_Label"] = Account_LINK_Label;
            if (Account_USDC_Label != null) Label_Dict["USDC_Account_Label"] = Account_USDC_Label;
            if (Account_AUD_Label != null) Label_Dict["AUD_Account_Label"] = Account_AUD_Label;
            if (Account_NZD_Label != null) Label_Dict["NZD_Account_Label"] = Account_NZD_Label;
            if (Account_USD_Label != null) Label_Dict["USD_Account_Label"] = Account_USD_Label;
            if (Account_SGD_Label != null) Label_Dict["SGD_Account_Label"] = Account_SGD_Label;
            if (Account_COMP_Label != null) Label_Dict["COMP_Account_Label"] = Account_COMP_Label;
            if (Account_SNX_Label != null) Label_Dict["SNX_Account_Label"] = Account_SNX_Label;
            if (Account_PMGT_Label != null) Label_Dict["PMGT_Account_Label"] = Account_PMGT_Label;
            if (Account_YFI_Label != null) Label_Dict["YFI_Account_Label"] = Account_YFI_Label;
            if (Account_AAVE_Label != null) Label_Dict["AAVE_Account_Label"] = Account_AAVE_Label;
            if (Account_KNC_Label != null) Label_Dict["KNC_Account_Label"] = Account_KNC_Label;
            if (Account_DOT_Label != null) Label_Dict["DOT_Account_Label"] = Account_DOT_Label;
            if (Account_GRT_Label != null) Label_Dict["GRT_Account_Label"] = Account_GRT_Label;
            if (Account_UNI_Label != null) Label_Dict["UNI_Account_Label"] = Account_UNI_Label;
            if (Account_ADA_Label != null) Label_Dict["ADA_Account_Label"] = Account_ADA_Label;
            if (Account_DOGE_Label != null) Label_Dict["DOGE_Account_Label"] = Account_DOGE_Label;
            if (Account_MATIC_Label != null) Label_Dict["MATIC_Account_Label"] = Account_MATIC_Label;
            if (Account_MANA_Label != null) Label_Dict["MANA_Account_Label"] = Account_MANA_Label;
            if (Account_SOL_Label != null) Label_Dict["SOL_Account_Label"] = Account_SOL_Label;
            if (Account_SAND_Label != null) Label_Dict["SAND_Account_Label"] = Account_SAND_Label;
            if (Account_SHIB_Label != null) Label_Dict["SHIB_Account_Label"] = Account_SHIB_Label;
            if (Account_TRX_Label != null) Label_Dict["TRX_Account_Label"] = Account_TRX_Label;
            if (Account_RENDER_Label != null) Label_Dict["RENDER_Account_Label"] = Account_RENDER_Label;
            if (Account_RLUSD_Label != null) Label_Dict["RLUSD_Account_Label"] = Account_RLUSD_Label;
            if (Account_WIF_Label != null) Label_Dict["WIF_Account_Label"] = Account_WIF_Label;


            if (Account_XBT_Value != null) Label_Dict["XBT_Account_Value"] = Account_XBT_Value;
            if (Account_ETH_Value != null) Label_Dict["ETH_Account_Value"] = Account_ETH_Value;
            if (Account_XRP_Value != null) Label_Dict["XRP_Account_Value"] = Account_XRP_Value;
            if (Account_BCH_Value != null) Label_Dict["BCH_Account_Value"] = Account_BCH_Value;
            if (Account_BSV_Value != null) Label_Dict["BSV_Account_Value"] = Account_BSV_Value;
            if (Account_USDT_Value != null) Label_Dict["USDT_Account_Value"] = Account_USDT_Value;
            if (Account_LTC_Value != null) Label_Dict["LTC_Account_Value"] = Account_LTC_Value;
            if (Account_EOS_Value != null) Label_Dict["EOS_Account_Value"] = Account_EOS_Value;
            if (Account_XLM_Value != null) Label_Dict["XLM_Account_Value"] = Account_XLM_Value;
            if (Account_ETC_Value != null) Label_Dict["ETC_Account_Value"] = Account_ETC_Value;
            if (Account_BAT_Value != null) Label_Dict["BAT_Account_Value"] = Account_BAT_Value;
            if (Account_OMG_Value != null) Label_Dict["OMG_Account_Value"] = Account_OMG_Value;
            if (Account_MKR_Value != null) Label_Dict["MKR_Account_Value"] = Account_MKR_Value;
            if (Account_ZRX_Value != null) Label_Dict["ZRX_Account_Value"] = Account_ZRX_Value;
            if (Account_GNT_Value != null) Label_Dict["GNT_Account_Value"] = Account_GNT_Value;
            if (Account_DAI_Value != null) Label_Dict["DAI_Account_Value"] = Account_DAI_Value;
            if (Account_LINK_Value != null) Label_Dict["LINK_Account_Value"] = Account_LINK_Value;
            if (Account_USDC_Value != null) Label_Dict["USDC_Account_Value"] = Account_USDC_Value;
            if (Account_COMP_Value != null) Label_Dict["COMP_Account_Value"] = Account_COMP_Value;
            if (Account_SNX_Value != null) Label_Dict["SNX_Account_Value"] = Account_SNX_Value;
            if (Account_PMGT_Value != null) Label_Dict["PMGT_Account_Value"] = Account_PMGT_Value;
            if (Account_YFI_Value != null) Label_Dict["YFI_Account_Value"] = Account_YFI_Value;
            if (Account_AAVE_Value != null) Label_Dict["AAVE_Account_Value"] = Account_AAVE_Value;
            if (Account_KNC_Value != null) Label_Dict["KNC_Account_Value"] = Account_KNC_Value;
            if (Account_DOT_Value != null) Label_Dict["DOT_Account_Value"] = Account_DOT_Value;
            if (Account_GRT_Value != null) Label_Dict["GRT_Account_Value"] = Account_GRT_Value;
            if (Account_UNI_Value != null) Label_Dict["UNI_Account_Value"] = Account_UNI_Value;
            if (Account_ADA_Value != null) Label_Dict["ADA_Account_Value"] = Account_ADA_Value;
            if (Account_DOGE_Value != null) Label_Dict["DOGE_Account_Value"] = Account_DOGE_Value;
            if (Account_MATIC_Value != null) Label_Dict["MATIC_Account_Value"] = Account_MATIC_Value;
            if (Account_MANA_Value != null) Label_Dict["MANA_Account_Value"] = Account_MANA_Value;
            if (Account_SOL_Value != null) Label_Dict["SOL_Account_Value"] = Account_SOL_Value;
            if (Account_SAND_Value != null) Label_Dict["SAND_Account_Value"] = Account_SAND_Value;
            if (Account_SHIB_Value != null) Label_Dict["SHIB_Account_Value"] = Account_SHIB_Value;
            if (Account_TRX_Value != null) Label_Dict["TRX_Account_Value"] = Account_TRX_Value;
            if (Account_RENDER_Value != null) Label_Dict["RENDER_Account_Value"] = Account_RENDER_Value;
            if (Account_RLUSD_Value != null) Label_Dict["RLUSD_Account_Value"] = Account_RLUSD_Value;
            if (Account_WIF_Value != null) Label_Dict["WIF_Account_Value"] = Account_WIF_Value;


            if (Account_XBT_Total != null) Label_Dict["XBT_Account_Total"] = Account_XBT_Total;
            if (Account_ETH_Total != null) Label_Dict["ETH_Account_Total"] = Account_ETH_Total;
            if (Account_XRP_Total != null) Label_Dict["XRP_Account_Total"] = Account_XRP_Total;
            if (Account_BCH_Total != null) Label_Dict["BCH_Account_Total"] = Account_BCH_Total;
            if (Account_BSV_Total != null) Label_Dict["BSV_Account_Total"] = Account_BSV_Total;
            if (Account_USDT_Total != null) Label_Dict["USDT_Account_Total"] = Account_USDT_Total;
            if (Account_LTC_Total != null) Label_Dict["LTC_Account_Total"] = Account_LTC_Total;
            if (Account_EOS_Total != null) Label_Dict["EOS_Account_Total"] = Account_EOS_Total;
            if (Account_XLM_Total != null) Label_Dict["XLM_Account_Total"] = Account_XLM_Total;
            if (Account_ETC_Total != null) Label_Dict["ETC_Account_Total"] = Account_ETC_Total;
            if (Account_BAT_Total != null) Label_Dict["BAT_Account_Total"] = Account_BAT_Total;
            if (Account_OMG_Total != null) Label_Dict["OMG_Account_Total"] = Account_OMG_Total;
            if (Account_MKR_Total != null) Label_Dict["MKR_Account_Total"] = Account_MKR_Total;
            if (Account_ZRX_Total != null) Label_Dict["ZRX_Account_Total"] = Account_ZRX_Total;
            if (Account_GNT_Total != null) Label_Dict["GNT_Account_Total"] = Account_GNT_Total;
            if (Account_DAI_Total != null) Label_Dict["DAI_Account_Total"] = Account_DAI_Total;
            if (Account_LINK_Total != null) Label_Dict["LINK_Account_Total"] = Account_LINK_Total;
            if (Account_USDC_Total != null) Label_Dict["USDC_Account_Total"] = Account_USDC_Total;
            if (Account_AUD_Total != null) Label_Dict["AUD_Account_Total"] = Account_AUD_Total;
            if (Account_NZD_Total != null) Label_Dict["NZD_Account_Total"] = Account_NZD_Total;
            if (Account_USD_Total != null) Label_Dict["USD_Account_Total"] = Account_USD_Total;
            if (Account_SGD_Total != null) Label_Dict["SGD_Account_Total"] = Account_SGD_Total;
            if (Account_COMP_Total != null) Label_Dict["COMP_Account_Total"] = Account_COMP_Total;
            if (Account_SNX_Total != null) Label_Dict["SNX_Account_Total"] = Account_SNX_Total;
            if (Account_PMGT_Total != null) Label_Dict["PMGT_Account_Total"] = Account_PMGT_Total;
            if (Account_YFI_Total != null) Label_Dict["YFI_Account_Total"] = Account_YFI_Total;
            if (Account_AAVE_Total != null) Label_Dict["AAVE_Account_Total"] = Account_AAVE_Total;
            if (Account_KNC_Total != null) Label_Dict["KNC_Account_Total"] = Account_KNC_Total;
            if (Account_DOT_Total != null) Label_Dict["DOT_Account_Total"] = Account_DOT_Total;
            if (Account_GRT_Total != null) Label_Dict["GRT_Account_Total"] = Account_GRT_Total;
            if (Account_UNI_Total != null) Label_Dict["UNI_Account_Total"] = Account_UNI_Total;
            if (Account_ADA_Total != null) Label_Dict["ADA_Account_Total"] = Account_ADA_Total;
            if (Account_DOGE_Total != null) Label_Dict["DOGE_Account_Total"] = Account_DOGE_Total;
            if (Account_MATIC_Total != null) Label_Dict["MATIC_Account_Total"] = Account_MATIC_Total;
            if (Account_MANA_Total != null) Label_Dict["MANA_Account_Total"] = Account_MANA_Total;
            if (Account_SOL_Total != null) Label_Dict["SOL_Account_Total"] = Account_SOL_Total;
            if (Account_SAND_Total != null) Label_Dict["SAND_Account_Total"] = Account_SAND_Total;
            if (Account_SHIB_Total != null) Label_Dict["SHIB_Account_Total"] = Account_SHIB_Total;
            if (Account_TRX_Total != null) Label_Dict["TRX_Account_Total"] = Account_TRX_Total;
            if (Account_RENDER_Total != null) Label_Dict["RENDER_Account_Total"] = Account_RENDER_Total;
            if (Account_RLUSD_Total != null) Label_Dict["RLUSD_Account_Total"] = Account_RLUSD_Total;
            if (Account_WIF_Total != null) Label_Dict["WIF_Account_Total"] = Account_WIF_Total;
        }


        public void CreateControlDictionaries()
        {

            // Labels
            Label_Dict = new Dictionary<string, Label>();
            if (XBT_Label != null) Label_Dict.Add("XBT_Label", XBT_Label);
            if (ETH_Label != null) Label_Dict.Add("ETH_Label", ETH_Label);
            if (LTC_Label != null) Label_Dict.Add("LTC_Label", LTC_Label);
            if (XRP_Label != null) Label_Dict.Add("XRP_Label", XRP_Label);
            if (BCH_Label != null) Label_Dict.Add("BCH_Label", BCH_Label);
            if (EOS_Label != null) Label_Dict.Add("EOS_Label", EOS_Label);
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
            if (YFI_Label != null) Label_Dict.Add("YFI_Label", YFI_Label);
            if (AAVE_Label != null) Label_Dict.Add("AAVE_Label", AAVE_Label);
            if (KNC_Label != null) Label_Dict.Add("KNC_Label", KNC_Label);
            if (DOT_Label != null) Label_Dict.Add("DOT_Label", DOT_Label);
            if (GRT_Label != null) Label_Dict.Add("GRT_Label", GRT_Label);
            if (UNI_Label != null) Label_Dict.Add("UNI_Label", UNI_Label);
            if (ADA_Label != null) Label_Dict.Add("ADA_Label", ADA_Label);
            if (DOGE_Label != null) Label_Dict.Add("DOGE_Label", DOGE_Label);
            if (MATIC_Label != null) Label_Dict.Add("MATIC_Label", MATIC_Label);
            if (MANA_Label != null) Label_Dict.Add("MANA_Label", MANA_Label);
            if (SOL_Label != null) Label_Dict.Add("SOL_Label", SOL_Label);
            if (SAND_Label != null) Label_Dict.Add("SAND_Label", SAND_Label);
            if (SHIB_Label != null) Label_Dict.Add("SHIB_Label", SHIB_Label);
            if (TRX_Label != null) Label_Dict.Add("TRX_Label", TRX_Label);
            if (RENDER_Label != null) Label_Dict.Add("RENDER_Label", RENDER_Label);
            if (RLUSD_Label != null) Label_Dict.Add("RLUSD_Label", RLUSD_Label);
            if (WIF_Label != null) Label_Dict.Add("WIF_Label", WIF_Label);



            if (XBT_Price != null) Label_Dict.Add("XBT_Price", XBT_Price);
            if (ETH_Price != null) Label_Dict.Add("ETH_Price", ETH_Price);
            if (LTC_Price != null) Label_Dict.Add("LTC_Price", LTC_Price);
            if (XRP_Price != null) Label_Dict.Add("XRP_Price", XRP_Price);
            if (BCH_Price != null) Label_Dict.Add("BCH_Price", BCH_Price);
            if (EOS_Price != null) Label_Dict.Add("EOS_Price", EOS_Price);
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
            if (YFI_Price != null) Label_Dict.Add("YFI_Price", YFI_Price);
            if (AAVE_Price != null) Label_Dict.Add("AAVE_Price", AAVE_Price);
            if (KNC_Price != null) Label_Dict.Add("KNC_Price", KNC_Price);
            if (DOT_Price != null) Label_Dict.Add("DOT_Price", DOT_Price);
            if (GRT_Price != null) Label_Dict.Add("GRT_Price", GRT_Price);
            if (UNI_Price != null) Label_Dict.Add("UNI_Price", UNI_Price);
            if (ADA_Price != null) Label_Dict.Add("ADA_Price", ADA_Price);
            if (DOGE_Price != null) Label_Dict.Add("DOGE_Price", DOGE_Price);
            if (MATIC_Price != null) Label_Dict.Add("MATIC_Price", MATIC_Price);
            if (MANA_Price != null) Label_Dict.Add("MANA_Price", MANA_Price);
            if (SOL_Price != null) Label_Dict.Add("SOL_Price", SOL_Price);
            if (SAND_Price != null) Label_Dict.Add("SAND_Price", SAND_Price);
            if (SHIB_Price != null) Label_Dict.Add("SHIB_Price", SHIB_Price);
            if (TRX_Price != null) Label_Dict.Add("TRX_Price", TRX_Price);
            if (RENDER_Price != null) Label_Dict.Add("RENDER_Price", RENDER_Price);
            if (RLUSD_Price != null) Label_Dict.Add("RLUSD_Price", RLUSD_Price);
            if (WIF_Price != null) Label_Dict.Add("WIF_Price", WIF_Price);


            if (XBT_Spread != null) Label_Dict.Add("XBT_Spread", XBT_Spread);
            if (ETH_Spread != null) Label_Dict.Add("ETH_Spread", ETH_Spread);
            if (LTC_Spread != null) Label_Dict.Add("LTC_Spread", LTC_Spread);
            if (XRP_Spread != null) Label_Dict.Add("XRP_Spread", XRP_Spread);
            if (BCH_Spread != null) Label_Dict.Add("BCH_Spread", BCH_Spread);
            if (EOS_Spread != null) Label_Dict.Add("EOS_Spread", EOS_Spread);
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
            if (YFI_Spread != null) Label_Dict.Add("YFI_Spread", YFI_Spread);
            if (AAVE_Spread != null) Label_Dict.Add("AAVE_Spread", AAVE_Spread);
            if (KNC_Spread != null) Label_Dict.Add("KNC_Spread", KNC_Spread);
            if (DOT_Spread != null) Label_Dict.Add("DOT_Spread", DOT_Spread);
            if (GRT_Spread != null) Label_Dict.Add("GRT_Spread", GRT_Spread);
            if (UNI_Spread != null) Label_Dict.Add("UNI_Spread", UNI_Spread);
            if (ADA_Spread != null) Label_Dict.Add("ADA_Spread", ADA_Spread);
            if (DOGE_Spread != null) Label_Dict.Add("DOGE_Spread", DOGE_Spread);
            if (MATIC_Spread != null) Label_Dict.Add("MATIC_Spread", MATIC_Spread);
            if (MANA_Spread != null) Label_Dict.Add("MANA_Spread", MANA_Spread);
            if (SOL_Spread != null) Label_Dict.Add("SOL_Spread", SOL_Spread);
            if (SAND_Spread != null) Label_Dict.Add("SAND_Spread", SAND_Spread);
            if (SHIB_Spread != null) Label_Dict.Add("SHIB_Spread", SHIB_Spread);
            if (TRX_Spread != null) Label_Dict.Add("TRX_Spread", TRX_Spread);
            if (RENDER_Spread != null) Label_Dict.Add("RENDER_Spread", RENDER_Spread);
            if (RLUSD_Spread != null) Label_Dict.Add("RLUSD_Spread", RLUSD_Spread);
            if (WIF_Spread != null) Label_Dict.Add("WIF_Spread", WIF_Spread);
        }
    }
}
