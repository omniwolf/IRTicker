using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IndependentReserve.DotNetClientApi.Data;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;


namespace IRTicker
{
    public partial class IRAccountsForm : Form {

        public string AccountSelectedCrypto = "XBT";
        //private Task updateOBTask;  // this is a local variable now, down in baiter code.
        private bool IRAccountsButtonJustClicked = true;  // true if the use has just clicked the IR Accounts button.  If true and GetAccounts fails, then we close the IR Accounts panel and head back to the Main panel.  If false and GetAccounts fails, we just do it silently

        private IRTicker IRT;
        private PrivateIR pIR;
        private DCE DCE_IR;
        private TelegramBot TGBot;

        private string lastPriceForUndo;  // holds the last typed price so when we "undo", we have a price to go back to
        private string buffer_lastPriceForUndo; // need to buffer the current price so we know what to set the undo value to
        private bool undoIsUpdatingPrice = false; // we temp sent this to true when we're updating the price so that we don't mess with other undo variables in the textChanged sub
        private decimal topPriceOtherOB;  // holds the top order of the side of the book we can't see (eg the top bid if we're showing the offers)

        public void InitialiseAccountsPanel() {
            AccountOrderVolume_textbox.Enabled = true;
            AccountLimitPrice_textbox.Enabled = true;
            //IRT.Settings.Visible = false;
            //IRAccount_panel.Visible = true;
            //Main.Visible = false;
            IRAccountsButtonJustClicked = true;

            // highlight the correct fiat label
            Label CurrentSecondaryCurrecyLabel = IRT.UIControls_Dict["IR"].Label_Dict[DCE_IR.CurrentSecondaryCurrency + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.DarkBlue;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

            IRT.populateIRAPIKeysSettings(this);

            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                PrivateIR.PrivateIREndPoints.GetAccounts,
                PrivateIR.PrivateIREndPoints.GetOpenOrders,
                PrivateIR.PrivateIREndPoints.GetClosedOrders,
                PrivateIR.PrivateIREndPoints.GetAddress,
                PrivateIR.PrivateIREndPoints.GetTradingFees 
            }));
            //AccountBuySell_listbox_Click(null, null);  // simulate a click to set things up
        }

        public IRAccountsForm(IRTicker _IRT, PrivateIR _pIR, DCE _DCE_IR, TelegramBot _TGBot) {
            IRT = _IRT;
            pIR = _pIR;
            DCE_IR = _DCE_IR;
            TGBot = _TGBot;

            InitializeComponent();

            AccountBuySell_listbox.SelectedIndex = 0;
            AccountOrderType_listbox.SelectedIndex = 0;

            //IRT.populateIRAPIKeysSettings();  // populates the drop down box of saved API keys

            IRT.UIControls_Dict["IR"].Account_XBT_Label = AccountXBT_label;
            IRT.UIControls_Dict["IR"].Account_XBT_Value = AccountXBT_value;
            IRT.UIControls_Dict["IR"].Account_ETH_Value = AccountETH_value;
            IRT.UIControls_Dict["IR"].Account_ETH_Label = AccountETH_label;
            IRT.UIControls_Dict["IR"].Account_XRP_Value = AccountXRP_value;
            IRT.UIControls_Dict["IR"].Account_XRP_Label = AccountXRP_label;
            IRT.UIControls_Dict["IR"].Account_BCH_Value = AccountBCH_value;
            IRT.UIControls_Dict["IR"].Account_BCH_Label = AccountBCH_label;
            IRT.UIControls_Dict["IR"].Account_BSV_Value = AccountBSV_value;
            IRT.UIControls_Dict["IR"].Account_BSV_Label = AccountBSV_label;
            IRT.UIControls_Dict["IR"].Account_USDT_Value = AccountUSDT_value;
            IRT.UIControls_Dict["IR"].Account_USDT_Label = AccountUSDT_label;
            IRT.UIControls_Dict["IR"].Account_LTC_Value = AccountLTC_value;
            IRT.UIControls_Dict["IR"].Account_LTC_Label = AccountLTC_label;
            IRT.UIControls_Dict["IR"].Account_EOS_Value = AccountEOS_value;
            IRT.UIControls_Dict["IR"].Account_EOS_Label = AccountEOS_label;
            IRT.UIControls_Dict["IR"].Account_XLM_Value = AccountXLM_value;
            IRT.UIControls_Dict["IR"].Account_XLM_Label = AccountXLM_label;
            IRT.UIControls_Dict["IR"].Account_ETC_Value = AccountETC_value;
            IRT.UIControls_Dict["IR"].Account_ETC_Label = AccountETC_label;
            IRT.UIControls_Dict["IR"].Account_BAT_Value = AccountBAT_value;
            IRT.UIControls_Dict["IR"].Account_BAT_Label = AccountBAT_label;
            IRT.UIControls_Dict["IR"].Account_OMG_Value = AccountOMG_value;
            IRT.UIControls_Dict["IR"].Account_OMG_Label = AccountOMG_label;
            IRT.UIControls_Dict["IR"].Account_MKR_Value = AccountMKR_value;
            IRT.UIControls_Dict["IR"].Account_MKR_Label = AccountMKR_label;
            IRT.UIControls_Dict["IR"].Account_ZRX_Value = AccountZRX_value;
            IRT.UIControls_Dict["IR"].Account_ZRX_Label = AccountZRX_label;
            IRT.UIControls_Dict["IR"].Account_GNT_Value = AccountGNT_value;
            IRT.UIControls_Dict["IR"].Account_GNT_Label = AccountGNT_label;
            IRT.UIControls_Dict["IR"].Account_DAI_Value = AccountDAI_value;
            IRT.UIControls_Dict["IR"].Account_DAI_Label = AccountDAI_label;
            IRT.UIControls_Dict["IR"].Account_USDC_Value = AccountUSDC_value;
            IRT.UIControls_Dict["IR"].Account_USDC_Label = AccountUSDC_label;
            IRT.UIControls_Dict["IR"].Account_XBT_Total = AccountXBT_total;
            IRT.UIControls_Dict["IR"].Account_ETH_Total = AccountETH_total;
            IRT.UIControls_Dict["IR"].Account_XRP_Total = AccountXRP_total;
            IRT.UIControls_Dict["IR"].Account_BCH_Total = AccountBCH_total;
            IRT.UIControls_Dict["IR"].Account_BSV_Total = AccountBSV_total;
            IRT.UIControls_Dict["IR"].Account_USDT_Total = AccountUSDT_total;
            IRT.UIControls_Dict["IR"].Account_LTC_Total = AccountLTC_total;
            IRT.UIControls_Dict["IR"].Account_EOS_Total = AccountEOS_total;
            IRT.UIControls_Dict["IR"].Account_XLM_Total = AccountXLM_total;
            IRT.UIControls_Dict["IR"].Account_ETC_Total = AccountETC_total;
            IRT.UIControls_Dict["IR"].Account_BAT_Total = AccountBAT_total;
            IRT.UIControls_Dict["IR"].Account_OMG_Total = AccountOMG_total;
            IRT.UIControls_Dict["IR"].Account_MKR_Total = AccountMKR_total;
            IRT.UIControls_Dict["IR"].Account_ZRX_Total = AccountZRX_total;
            IRT.UIControls_Dict["IR"].Account_GNT_Total = AccountGNT_total;
            IRT.UIControls_Dict["IR"].Account_DAI_Total = AccountDAI_total;
            IRT.UIControls_Dict["IR"].Account_LINK_Total = AccountLINK_total;
            IRT.UIControls_Dict["IR"].Account_LINK_Value = AccountLINK_value;
            IRT.UIControls_Dict["IR"].Account_LINK_Label = AccountLINK_label;
            IRT.UIControls_Dict["IR"].Account_USDC_Total = AccountUSDC_total;
            IRT.UIControls_Dict["IR"].Account_AUD_Total = AccountAUD_total;
            IRT.UIControls_Dict["IR"].Account_AUD_Label = AccountAUD_label;
            IRT.UIControls_Dict["IR"].Account_NZD_Total = AccountNZD_total;
            IRT.UIControls_Dict["IR"].Account_NZD_Label = AccountNZD_label;
            IRT.UIControls_Dict["IR"].Account_USD_Total = AccountUSD_total;
            IRT.UIControls_Dict["IR"].Account_USD_Label = AccountUSD_label;
            IRT.UIControls_Dict["IR"].Account_SGD_Total = AccountSGD_total;
            IRT.UIControls_Dict["IR"].Account_SGD_Label = AccountSGD_label;
            IRT.UIControls_Dict["IR"].Account_COMP_Total = AccountCOMP_total;
            IRT.UIControls_Dict["IR"].Account_COMP_Value = AccountCOMP_value;
            IRT.UIControls_Dict["IR"].Account_COMP_Label = AccountCOMP_label;
            IRT.UIControls_Dict["IR"].Account_SNX_Total = AccountSNX_total;
            IRT.UIControls_Dict["IR"].Account_SNX_Value = AccountSNX_value;
            IRT.UIControls_Dict["IR"].Account_SNX_Label = AccountSNX_label;
            IRT.UIControls_Dict["IR"].Account_PMGT_Total = AccountPMGT_total;
            IRT.UIControls_Dict["IR"].Account_PMGT_Value = AccountPMGT_value;
            IRT.UIControls_Dict["IR"].Account_PMGT_Label = AccountPMGT_label;
            IRT.UIControls_Dict["IR"].Account_YFI_Total = AccountYFI_total;
            IRT.UIControls_Dict["IR"].Account_YFI_Value = AccountYFI_value;
            IRT.UIControls_Dict["IR"].Account_YFI_Label = AccountYFI_label;
            IRT.UIControls_Dict["IR"].Account_AAVE_Total = AccountAAVE_total;
            IRT.UIControls_Dict["IR"].Account_AAVE_Value = AccountAAVE_value;
            IRT.UIControls_Dict["IR"].Account_AAVE_Label = AccountAAVE_label;
            IRT.UIControls_Dict["IR"].Account_KNC_Total = AccountKNC_total;
            IRT.UIControls_Dict["IR"].Account_KNC_Value = AccountKNC_value;
            IRT.UIControls_Dict["IR"].Account_KNC_Label = AccountKNC_label;
            IRT.UIControls_Dict["IR"].Account_DOT_Total = AccountDOT_total;
            IRT.UIControls_Dict["IR"].Account_DOT_Value = AccountDOT_value;
            IRT.UIControls_Dict["IR"].Account_DOT_Label = AccountDOT_label;
            IRT.UIControls_Dict["IR"].Account_GRT_Total = AccountGRT_total;
            IRT.UIControls_Dict["IR"].Account_GRT_Value = AccountGRT_value;
            IRT.UIControls_Dict["IR"].Account_GRT_Label = AccountGRT_label;
            IRT.UIControls_Dict["IR"].Account_UNI_Total = AccountUNI_total;
            IRT.UIControls_Dict["IR"].Account_UNI_Value = AccountUNI_value;
            IRT.UIControls_Dict["IR"].Account_UNI_Label = AccountUNI_label;
            IRT.UIControls_Dict["IR"].Account_ADA_Total = AccountADA_total;
            IRT.UIControls_Dict["IR"].Account_ADA_Value = AccountADA_value;
            IRT.UIControls_Dict["IR"].Account_ADA_Label = AccountADA_label;
            IRT.UIControls_Dict["IR"].Account_DOGE_Total = AccountDOGE_total;
            IRT.UIControls_Dict["IR"].Account_DOGE_Value = AccountDOGE_value;
            IRT.UIControls_Dict["IR"].Account_DOGE_Label = AccountDOGE_label;
            IRT.UIControls_Dict["IR"].Account_MATIC_Total = AccountMATIC_total;
            IRT.UIControls_Dict["IR"].Account_MATIC_Value = AccountMATIC_value;
            IRT.UIControls_Dict["IR"].Account_MATIC_Label = AccountMATIC_label;
            IRT.UIControls_Dict["IR"].Account_MANA_Total = AccountMANA_total;
            IRT.UIControls_Dict["IR"].Account_MANA_Value = AccountMANA_value;
            IRT.UIControls_Dict["IR"].Account_MANA_Label = AccountMANA_label;
            IRT.UIControls_Dict["IR"].Account_SOL_Total = AccountSOL_total;
            IRT.UIControls_Dict["IR"].Account_SOL_Value = AccountSOL_value;
            IRT.UIControls_Dict["IR"].Account_SOL_Label = AccountSOL_label;
            IRT.UIControls_Dict["IR"].Account_SAND_Total = AccountSAND_total;
            IRT.UIControls_Dict["IR"].Account_SAND_Value = AccountSAND_value;
            IRT.UIControls_Dict["IR"].Account_SAND_Label = AccountSAND_label;
            IRT.UIControls_Dict["IR"].Account_SHIB_Total = AccountSHIB_total;
            IRT.UIControls_Dict["IR"].Account_SHIB_Value = AccountSHIB_value;
            IRT.UIControls_Dict["IR"].Account_SHIB_Label = AccountSHIB_label;
            IRT.UIControls_Dict["IR"].Account_TRX_Total = AccountTRX_total;
            IRT.UIControls_Dict["IR"].Account_TRX_Value = AccountTRX_value;
            IRT.UIControls_Dict["IR"].Account_TRX_Label = AccountTRX_label;
            IRT.UIControls_Dict["IR"].Account_RENDER_Total = AccountRENDER_total;
            IRT.UIControls_Dict["IR"].Account_RENDER_Value = AccountRENDER_value;
            IRT.UIControls_Dict["IR"].Account_RENDER_Label = AccountRENDER_label;
            IRT.UIControls_Dict["IR"].Account_RLUSD_Total = AccountRLUSD_total;
            IRT.UIControls_Dict["IR"].Account_RLUSD_Value = AccountRLUSD_value;
            IRT.UIControls_Dict["IR"].Account_RLUSD_Label = AccountRLUSD_label;
            IRT.UIControls_Dict["IR"].Account_WIF_Total = AccountWIF_total;
            IRT.UIControls_Dict["IR"].Account_WIF_Value = AccountWIF_value;
            IRT.UIControls_Dict["IR"].Account_WIF_Label = AccountWIF_label;
            IRT.UIControls_Dict["IR"].Account_PEPE_Total = AccountPEPE_total;
            IRT.UIControls_Dict["IR"].Account_PEPE_Value = AccountPEPE_value;
            IRT.UIControls_Dict["IR"].Account_PEPE_Label = AccountPEPE_label;
            IRT.UIControls_Dict["IR"].Account_TRUMP_Total = AccountTRUMP_total;
            IRT.UIControls_Dict["IR"].Account_TRUMP_Value = AccountTRUMP_value;
            IRT.UIControls_Dict["IR"].Account_TRUMP_Label = AccountTRUMP_label;
            IRT.UIControls_Dict["IR"].Account_AVAX_Total = AccountAVAX_total;
            IRT.UIControls_Dict["IR"].Account_AVAX_Value = AccountAVAX_value;
            IRT.UIControls_Dict["IR"].Account_AVAX_Label = AccountAVAX_label;
            IRT.UIControls_Dict["IR"].Account_HYPE_Total = AccountHYPE_total;
            IRT.UIControls_Dict["IR"].Account_HYPE_Value = AccountHYPE_value;
            IRT.UIControls_Dict["IR"].Account_HYPE_Label = AccountHYPE_label;
            IRT.UIControls_Dict["IR"].Account_BONK_Total = AccountBONK_total;
            IRT.UIControls_Dict["IR"].Account_BONK_Value = AccountBONK_value;
            IRT.UIControls_Dict["IR"].Account_BONK_Label = AccountBONK_label;
            IRT.UIControls_Dict["IR"].Account_PENGU_Total = AccountPENGU_total;
            IRT.UIControls_Dict["IR"].Account_PENGU_Value = AccountPENGU_value;
            IRT.UIControls_Dict["IR"].Account_PENGU_Label = AccountPENGU_label;
            IRT.UIControls_Dict["IR"].Account_AUSD_Total = AccountAUSD_total;
            IRT.UIControls_Dict["IR"].Account_AUSD_Value = AccountAUSD_value;
            IRT.UIControls_Dict["IR"].Account_AUSD_Label = AccountAUSD_label;
            IRT.UIControls_Dict["IR"].Account_XAUT_Total = AccountXAUT_total;
            IRT.UIControls_Dict["IR"].Account_XAUT_Value = AccountXAUT_value;
            IRT.UIControls_Dict["IR"].Account_XAUT_Label = AccountXAUT_label;
            IRT.UIControls_Dict["IR"].Account_AUDM_Total = AccountAUDM_total;
            IRT.UIControls_Dict["IR"].Account_AUDM_Value = AccountAUDM_value;
            IRT.UIControls_Dict["IR"].Account_AUDM_Label = AccountAUDM_label;

            IRT.UIControls_Dict["IR"].CreateIRAccountControlDictionary();  // now that we have finally opened this form, we can add the form labels to the dictionary

            pIR.setIRAF(this); // we're aLiVe
            InitialiseAccountsPanel();
        }

        // resets all the label values (price, vol) to "-".  Usually used for when the API key is changed
        public void ResetLabels() {
            foreach (KeyValuePair<string, Label> labelKVP in IRT.UIControls_Dict["IR"].Label_Dict) {
                if (labelKVP.Key.Contains("_Account_Value") || labelKVP.Key.Contains("_Account_Total")) labelKVP.Value.Text = "-";
            }
        }

        public void DrawIRAccounts(Dictionary<string, Account> irAccounts) {

            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {

                var mSummaries = DCE_IR.GetCryptoPairs();

                Dictionary<string, Account> _irAccounts = (Dictionary<string, Account>)o;

                foreach (KeyValuePair<string, Account> acc in _irAccounts) {
                    if (IRT.UIControls_Dict["IR"].Label_Dict.ContainsKey(acc.Key + "_Account_Total")) {
                        IRT.UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Total"].Text =
                            Utilities.FormatValue(acc.Value.AvailableBalance);
                        IRT.UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Total"].Tag = acc.Value.AvailableBalance;
                        IRTickerTT_generic.SetToolTip(IRT.UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Total"], acc.Value.AvailableBalance.ToString());

                    }

                    if (IRT.UIControls_Dict["IR"].Label_Dict.ContainsKey(acc.Key + "_Account_Value") && mSummaries.ContainsKey(acc.Key + "-" + DCE_IR.CurrentSecondaryCurrency)) {
                        IRT.UIControls_Dict["IR"].Label_Dict[acc.Key + "_Account_Value"].Text =
                            Utilities.FormatValue(acc.Value.AvailableBalance * mSummaries[acc.Key + "-" + DCE_IR.CurrentSecondaryCurrency].CurrentHighestBidPrice);
                    }
                }
            }), irAccounts);
        }

        // runs these network calls in order
        // this method should only be called from the UI because it can surface messageboxes
        // eg baiter and telegram should never use this.
        private void bulkSequentialAPICalls(List<PrivateIR.PrivateIREndPoints> endPoints, decimal volume = 0, decimal price = 0, string orderGuid = "") {

            foreach (PrivateIR.PrivateIREndPoints endP in endPoints) {
                if (endP == PrivateIR.PrivateIREndPoints.GetAccounts) {

                    Dictionary<string, Account> irAccounts;
                    try {
                        irAccounts = pIR.GetAccounts();
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        Debug.Print(DateTime.Now + " - couldn't pull getAccounts pIR because: " + errorMsg);
                        irAccounts = null;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - GetAccounts", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //Debug.Print("PIR: gotACcounts");
                    if ((irAccounts == null) && IRAccountsButtonJustClicked) {
                        Debug.Print(DateTime.Now + " - there was an error, closing the accounts page");
                        IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            Close();
                        }), null);
                        return;  // close the IRAccounts panel
                    }
                    else if ((irAccounts == null) && !IRAccountsButtonJustClicked) continue;
                    DrawIRAccounts(irAccounts);
                    IRAccountsButtonJustClicked = false;  // we have now run this successfully once after opening the panel, can set this to false.
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetAddress) {

                    DigitalCurrencyDepositAddress addressData;
                    try {
                        addressData = pIR.GetDepositAddress(AccountSelectedCrypto);
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        Debug.Print(DateTime.Now + " - failed to call GetDepositAddress properly: " + errorMsg);
                        continue;
                    }
                    drawDepositAddress(addressData);
                }
                else if (endP == PrivateIR.PrivateIREndPoints.CheckAddress) {

                    // we don't really need to sync addresses anymore with Blockscan.  Reusing the form space for trading fees...

                    /*string address = AccountWithdrawalAddress_label.Text;
                    DigitalCurrencyDepositAddress addressData;
                    try {
                        addressData = pIR.CheckAddressNow(AccountSelectedCrypto, address);  // we don't really need to sync addresses anymore with Blockscan
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - CheckAddress", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (addressData != null) drawDepositAddress(addressData);*/
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetTradingFees) {
                    IEnumerable<BrokerageFee> fees = pIR.GetTradingFees();
                    if (null != fees) {
                        drawTradingFees(fees);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetOpenOrders) {
                    try {
                        var openOrders = pIR.GetOpenOrders(AccountSelectedCrypto, DCE_IR.CurrentSecondaryCurrency);
                        drawOpenOrders(openOrders.Data);
                    }
                    catch (Exception ex) {
                        IRT.showBalloon("Failed to get open orders", "Error: " + ex.Message);
                        Debug.Print(DateTime.Now + " - GetOpenOrders failed with: " + ex.Message);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.GetClosedOrders) {
                    List<BankHistoryOrder> closedOrders;
                    try {
                        closedOrders = pIR.GetClosedOrders(AccountSelectedCrypto, DCE_IR.CurrentSecondaryCurrency);
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        Debug.Print("Bulk method: couldn't pull closed orders: " + errorMsg);
                        continue;
                    }
                    if (closedOrders != null) {
                        drawClosedOrders(closedOrders);
                    }
                }
                // need to be more robust, and pull multiple pages if necessary
                else if (endP == PrivateIR.PrivateIREndPoints.PlaceMarketOrder) {
                    //OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.MarketBid : OrderType.MarketOffer;

                    BankOrder orderResult;
                    try {
                        orderResult = pIR.PlaceMarketOrder(AccountSelectedCrypto, DCE_IR.CurrentSecondaryCurrency, null, -1);
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - Market order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Market order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.PlaceLimitOrder) {
                    //OrderType oType = AccountBuySell_listbox.SelectedIndex == 0 ? OrderType.LimitBid : OrderType.LimitOffer;

                    BankOrder orderResult;
                    try {
                        orderResult = pIR.PlaceLimitOrder(AccountSelectedCrypto, DCE_IR.CurrentSecondaryCurrency, null, -1, -1);
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - Limit order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if ((orderResult != null) && (orderResult.Status == OrderStatus.Failed)) {
                        MessageBox.Show("Limit order failed!", "Order failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (endP == PrivateIR.PrivateIREndPoints.CancelOrder) {
                    BankOrder cancelledOrder;
                    try {
                        cancelledOrder = pIR.CancelOrder(orderGuid);
                    }
                    catch (Exception ex) {
                        string errorMsg = "";
                        if (ex.InnerException != null) errorMsg = ex.InnerException.Message;
                        else errorMsg = ex.Message;
                        MessageBox.Show("IR private API issue:" + Environment.NewLine + Environment.NewLine +
                            errorMsg, "Error - Cancel order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Debug.Print("cancelled order status: " + cancelledOrder.Status.ToString());
                }
            }
        }

        private string buildOrderTT(BankHistoryOrder order, bool isOrderOpen) {
            string tt = "";
            switch (order.OrderType) {
                case OrderType.LimitBid:
                    tt += "Limit bid order" + Environment.NewLine + Environment.NewLine;
                    break;
                case OrderType.LimitOffer:
                    tt += "Limit offer order" + Environment.NewLine + Environment.NewLine;
                    break;
                case OrderType.MarketBid:
                    tt += "Market bid order" + Environment.NewLine + Environment.NewLine;
                    break;
                case OrderType.MarketOffer:
                    tt += "Market offer order" + Environment.NewLine + Environment.NewLine;
                    break;
            }
            tt += "Date created: " + order.CreatedTimestampUtc.ToLocalTime().ToString() + Environment.NewLine;

            if (isOrderOpen) {
                if (order.Original != null) {
                    tt += "Original volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Original.Volume + Environment.NewLine;
                }
                if (order.Price.HasValue) {
                    tt += "Price: $ " + Utilities.FormatValue(order.Price.Value) + Environment.NewLine;
                }
                if (order.Outstanding.HasValue && (order.Outstanding.Value > 0)) {
                    tt += "Outstanding volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Outstanding.Value + Environment.NewLine;
                }
            }

            else {
                tt += "Volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + order.Volume + Environment.NewLine;
                if (order.AvgPrice.HasValue) tt += "Avg price: $ " + Utilities.FormatValue(order.AvgPrice.Value) + Environment.NewLine;
                tt += "Fee: " + Utilities.FormatValue((order.FeePercent * 100), 2, false) + "%" + Environment.NewLine;
                if (order.Value.HasValue) {
                    tt += "Notional value: $ " + Utilities.FormatValue(order.Value.Value) + Environment.NewLine;
                    tt += "Notional sans fee: $ " + Utilities.FormatValue(order.Value.Value * (1 - order.FeePercent), 8, false) + Environment.NewLine;
                    tt += "Fee value: $ " + Utilities.FormatValue(order.Value.Value * order.FeePercent) + Environment.NewLine;
                }
            }
            tt += "Status: " + order.Status;
            return tt;
        }

        public void drawClosedOrders(IEnumerable<BankHistoryOrder> closedOrders) {
            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                IEnumerable<BankHistoryOrder> _closedOrders = (IEnumerable<BankHistoryOrder>)o;

                if (_closedOrders == null) {
                    AccountClosedOrders_listview.Items.Clear();
                    AccountClosedOrders_listview.Items.Add(new System.Windows.Forms.ListViewItem("Loading..."));
                }
                else {
                    AccountClosedOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " closed orders";
                    AccountClosedOrders_listview.Items.Clear();
                    int drawCount = 0;  // we only need to draw 7 items, keep track and bail when done.
                    foreach (BankHistoryOrder order in _closedOrders) {
                        if ((order.Status != OrderStatus.Filled) && (order.Status != OrderStatus.PartiallyFilledAndCancelled) && (order.Status != OrderStatus.PartiallyFilledAndFailed)) continue;
                        decimal vol = order.Volume;
                        if (order.Outstanding.HasValue && order.Outstanding.Value > 0) {
                            vol = order.Volume - order.Outstanding.Value;
                        }
                        AccountClosedOrders_listview.Items.Add(new ListViewItem(new string[] {
                            order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                            Utilities.FormatValue(vol, 8, false),
                            Utilities.FormatValue((order.AvgPrice.HasValue ? order.AvgPrice.Value : 0), 2),
                            Utilities.FormatValue((order.Value.HasValue ? order.Value.Value : 0), 2)
                        }));
                        AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].ToolTipText = buildOrderTT(order, false);
                        if (order.OrderType == OrderType.LimitBid || order.OrderType == OrderType.MarketBid) {
                            AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].BackColor = Color.Thistle;
                        }
                        else {
                            AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].BackColor = Color.PeachPuff;
                        }
                        AccountClosedOrders_listview.Items[AccountClosedOrders_listview.Items.Count - 1].Tag = order;
                        drawCount++;
                        if (drawCount > 6) break;  // table can only support 7 entries, so bail here.
                    }
                }
            }), closedOrders);
        }

        private void drawTradingFees(IEnumerable<BrokerageFee> fees) {

            foreach (BrokerageFee fee in fees) {
                if (fee.CurrencyCode.ToString().ToUpper() == AccountSelectedCrypto) {

                    // we have the fee entry.  Let's pop into the UI thread and update the UI
                    IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        BrokerageFee _fee = (BrokerageFee)o;

                        AccountTradingFees_value.Text = (_fee.Fee * 100).ToString() + "%";

                    }), fee);
                    break;  // can only have 1 crypto selected, so if we find a match, that's all we need to do.
                }
            }
        }
        public void drawOpenOrders(IEnumerable<BankHistoryOrder> openOrders) {
            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                IEnumerable<BankHistoryOrder> _openOrders = (IEnumerable<BankHistoryOrder>)o;

                if (_openOrders == null) {
                    AccountOpenOrders_listview.Items.Clear();
                    AccountOpenOrders_listview.Items.Add(new System.Windows.Forms.ListViewItem("Loading..."));
                }
                else {
                    AccountOpenOrders_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " open orders";
                    AccountOpenOrders_listview.Items.Clear();

                    int loopCount = 1;
                    foreach (BankHistoryOrder order in _openOrders) {
                        if ((order.Status != OrderStatus.Open) && (order.Status != OrderStatus.PartiallyFilled)) continue;
                        AccountOpenOrders_listview.Items.Add(new ListViewItem(new string[] {
                            order.CreatedTimestampUtc.ToLocalTime().ToShortDateString(),
                            Utilities.FormatValue(order.Volume, 8, false),  // hopefully will format but leave decimals untouched..
                            Utilities.FormatValue((order.Price.HasValue ? order.Price.Value : 0), 2),
                            Utilities.FormatValue((order.Outstanding.HasValue ? order.Outstanding.Value : 0), 8, false)
                        }));
                        AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].ToolTipText = buildOrderTT(order, true);
                        if (order.OrderType == OrderType.LimitBid) {
                            AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].BackColor = Color.Thistle;
                        }
                        else {
                            AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].BackColor = Color.PeachPuff;
                        }
                        AccountOpenOrders_listview.Items[AccountOpenOrders_listview.Items.Count - 1].Tag = order;
                        loopCount++;
                        if (loopCount > 7) break;  // we only need to draw 7 items
                    }
                }
            }), openOrders);
        }

        public void drawDepositAddress(DigitalCurrencyDepositAddress deposAddress) {
            if (deposAddress == null) {  // construct an empty address object and draw blanks, this is used when our data is old and we need to show nothing until the new data is sent
                deposAddress = new DigitalCurrencyDepositAddress();
                deposAddress.DepositAddress = "";
                deposAddress.NextUpdateTimestampUtc = null;
                deposAddress.LastCheckedTimestampUtc = null;

            }
            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                DigitalCurrencyDepositAddress _deposAddress = (DigitalCurrencyDepositAddress)o;
                AccountWithdrawalCrypto_label.Text = (AccountSelectedCrypto == "XBT" ? "BTC" : AccountSelectedCrypto) + " deposit address";

                AccountWithdrawalAddress_label.Text = _deposAddress.DepositAddress;

                // don't do this anymore with blockscan.  using the space for fees
                /*string nextCheck = "";
                if (_deposAddress.NextUpdateTimestampUtc != null) {
                    if (_deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime() < DateTime.Now) nextCheck = "ASAP";
                    else nextCheck = _deposAddress.NextUpdateTimestampUtc.Value.ToLocalTime().ToString();
                }
                AccountWithdrawalNextCheck_label.Text = "Next check: " + nextCheck;

                string lastChecked = "";
                if (_deposAddress.LastCheckedTimestampUtc != null) lastChecked = _deposAddress.LastCheckedTimestampUtc.Value.ToLocalTime().ToString();
                AccountWithdrawalLastCheck_label.Text = "Last checked: " + lastChecked;*/

                if (string.IsNullOrEmpty(_deposAddress.Tag)) {
                    AccountWithdrawalTag_label.Visible = false;
                    AccountWithdrawalTag_value.Text = "";
                }
                else {
                    AccountWithdrawalTag_label.Visible = true;
                    AccountWithdrawalTag_value.Text = _deposAddress.Tag;
                }
            }), deposAddress);
        }

        public void setCurrencyValues(string crypto, decimal price) {
            if (pIR.accounts.ContainsKey(crypto)) {
                IRT.UIControls_Dict["IR"].Label_Dict[crypto + "_Account_Value"].Text = Utilities.FormatValue(price * pIR.accounts[crypto].AvailableBalance);
            }
        }

        // how is this accountORders.item2 string array made up?
        // count, pricePoint (not formatted), totalVolume, cumulativeVol (not formatted), cumulativeValue, includesMyOrder 
        public void drawAccountOrderBook(Tuple<decimal, List<decimal[]>> accountOrders, string pair, decimal _topPriceOtherOB) {

            topPriceOtherOB = _topPriceOtherOB;

            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                Tuple<decimal, List<decimal[]>> _accountOrders = (Tuple<decimal, List<decimal[]>>)o;

                if (AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency != pair) return;
                if (!DCE_IR.IR_OBs.ContainsKey(pair)) return;

                AccountOrders_listview.Items.Clear();

                bool cumVolumeReached = false;  // We need to track the cum volume on the orders.  When we find a row that has higher cumulative volume than the form volume value, we need to highlight this one, but no further ones.  use this flag to show we no longer need to highlight orders

                foreach (decimal[] lvi in _accountOrders.Item2) {
                    Tuple<string, string> pairTup = Utilities.SplitPair(pair);
                    AccountOrders_listview.Items.Add(new ListViewItem(new string[] { lvi[0].ToString(), Utilities.FormatValue(lvi[1], DCE_IR.currencyDecimalPlaces[pairTup.Item1].Item2, false), Utilities.FormatValue(lvi[2], 8, false), Utilities.FormatValue(lvi[3]), Utilities.FormatValue(lvi[4]) }));
                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].SubItems[1].Tag = lvi[1];  // need to store the price in an unformatted (and therefore parseable) format
                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].SubItems[3].Tag = lvi[3];  // need to store the cumulative volume in an unformatted (and therefore parseable) format

                    // if limit order or baiter, and can parse vol and limit price, and order book is showing the opposite side (ie if we're selling, and the OB is showing bids)
                    // if cumVol >= formVol then highlight

                    Tuple<bool, decimal, decimal> volPriceTup = VolumePriceParseable();

                    if (volPriceTup.Item1) {
                        if (((AccountBuySell_listbox.SelectedIndex == 0) && (pIR.OrderBookSide == "Offer")) ||
                            ((AccountBuySell_listbox.SelectedIndex == 1) && (pIR.OrderBookSide == "Bid"))) 
                        {
                            if (lvi[3] < volPriceTup.Item2) {
                                if (AccountOrderType_listbox.SelectedIndex > 0) {  // if we are have a limit or bait order type chosen, let's also stop highlighting at the price value
                                    if ((AccountBuySell_listbox.SelectedIndex == 0) && (lvi[1] <= volPriceTup.Item3) ||
                                        ((AccountBuySell_listbox.SelectedIndex == 1) && (lvi[1] >= volPriceTup.Item3))) {
                                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                                    }
                                    // if the price is beyond our limit price, then colour a different colour to signify that this is the price level the user
                                    // would need to enter if they wanted to fill this volume
                                    else {
                                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleVioletRed;
                                    }
                                }
                                else AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;  // else it's a market order, just highlight if the vol is good
                            }
                            else if (!cumVolumeReached) {  // we need to highlight one more row, as this will be the final order we'll eat into to fulfill our above order
                                if (AccountOrderType_listbox.SelectedIndex == 0) {  // if it's a market order, just colour it.  don't try and compare limit prices
                                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                                }
                                else if ((AccountBuySell_listbox.SelectedIndex == 0) && (lvi[1] <= volPriceTup.Item3) ||
                                ((AccountBuySell_listbox.SelectedIndex == 1) && (lvi[1] >= volPriceTup.Item3))) {
                                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                                }
                                // if the price is beyond our limit price, then colour a different colour to signify that this is the price level the user
                                // would need to enter if they wanted to fill this volume
                                else {
                                    AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleVioletRed;
                                }
                                cumVolumeReached = true;
                            }
                        }
                    }
                    else if (volPriceTup.Item3 >= 0) {  // vol not parsable, but price is.  let's colour some rows
                        if ((AccountBuySell_listbox.SelectedIndex == 0) && (lvi[1] <= volPriceTup.Item3) ||
                        ((AccountBuySell_listbox.SelectedIndex == 1) && (lvi[1] >= volPriceTup.Item3))) {
                            AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.PaleTurquoise;
                        }
                    }
                    if (lvi[5] == 1) {  // what a hack.  colourising any orders that are MINE
                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].ForeColor = Color.RoyalBlue;
                        AccountOrders_listview.Items[AccountOrders_listview.Items.Count - 1].BackColor = Color.Yellow;
                    }
                }
                if (_accountOrders.Item1 == -1) {
                    AccountEstOrderValue_value.Text = "Not enough depth!";
                    AccountEstOrderValue_value.Tag = -1m;
                }
                else if (_accountOrders.Item1 == -2) {
                    AccountEstOrderValue_value.Text = "";
                    AccountEstOrderValue_value.Tag = null;
                }
                else {  // leave it alone if a limit order
                    if (AccountOrderType_listbox.SelectedIndex == 0) {
                        AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(_accountOrders.Item1);
                        AccountEstOrderValue_value.Tag = _accountOrders.Item1;  // store the decimal here for later use
                    }
                }

                ValidateLimitOrder();  // update the main order button

                checkSufficientVolume(null, _accountOrders.Item1 != -1);  // account balance may have changed, let's update this validation

            }), accountOrders);

        }

        // is called when the user selects a different crypto from the side panel
        private void cryptoClicked(Label clickedLabel) {
            if (!pIR.marketBaiterActive) {  // can't let the crypto change while we're baitin'

                Label oldLabel = IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                oldLabel.ForeColor = Color.Black;
                oldLabel.Font = new Font(oldLabel.Font.FontFamily, 12f, FontStyle.Bold);

                oldLabel = IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
                oldLabel.ForeColor = Color.FromArgb(64, 64, 64);

                oldLabel = IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
                oldLabel.ForeColor = Color.FromArgb(64, 64, 64);

                AccountSelectedCrypto = (clickedLabel.Text.Substring(0, clickedLabel.Text.IndexOf(':'))).ToUpper();

                AccountOpenOrders_label.Text = AccountSelectedCrypto + " open orders";
                drawOpenOrders(null);

                AccountClosedOrders_label.Text = AccountSelectedCrypto + " closed orders";
                drawClosedOrders(null);

                drawDepositAddress(null);

                if (AccountSelectedCrypto == "BTC") AccountSelectedCrypto = "XBT";
                pIR.SelectedCrypto = AccountSelectedCrypto;  // inform the pIR object what our selected crypto is

                Label newLabel = IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Label"];
                newLabel.ForeColor = Color.DarkOrange;
                newLabel.Font = new Font(newLabel.Font.FontFamily, 12f, FontStyle.Bold);

                newLabel = IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"];
                newLabel.ForeColor = Color.DarkOrange;

                newLabel = IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Value"];
                newLabel.ForeColor = Color.DarkOrange;

                // simulate a change in text so we perform text validation and adjustments
                AccountOrderVolume_textbox_TextChanged(null, null);
                AccountLimitPrice_textbox_TextChanged(null, null);
            }

            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                PrivateIR.PrivateIREndPoints.GetAddress,PrivateIR.PrivateIREndPoints.GetClosedOrders,PrivateIR.PrivateIREndPoints.GetTradingFees,
                PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetAccounts
            }));
            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        // a sub to report which crypto closed orders we're pulling.. just to keep the user informed
        public void ReportClosedOrderStatus(string crypto, string pageProgress) {
            if (crypto == "XBT") crypto = "BTC";
            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                string _crypto = (string)o;

                if (AccountClosedOrders_listview.Items.Count == 0) {
                    AccountClosedOrders_listview.Items.Add(new ListViewItem(new string[] {
                        "Loading", _crypto, pageProgress }));
                }
                else if (AccountClosedOrders_listview.Items[0].Text.StartsWith("Loading")) {  // only if we have nothing else to show..
                    AccountClosedOrders_listview.Items[0] = new ListViewItem(new string[] {
                        "Loading", _crypto, pageProgress });
                }

            }), crypto);
        }

        public void fiatClicked(Label clickedLabel, string oldFiat = "") {

            //  colour the fiats

            // set the old fiat label back to normal
            if (string.IsNullOrEmpty(oldFiat)) {
                oldFiat = DCE_IR.CurrentSecondaryCurrency;
            }

            Label CurrentSecondaryCurrecyLabel = IRT.UIControls_Dict["IR"].Label_Dict[oldFiat + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.Black;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 14.25f, FontStyle.Regular);

            // highlight the new fiat label
            clickedLabel.ForeColor = Color.DarkBlue;
            clickedLabel.Font = new Font(clickedLabel.Font.FontFamily, 14.25f, FontStyle.Bold);

            // set these listview things to "loading.."
            drawOpenOrders(null);
            drawClosedOrders(null);
            drawDepositAddress(null);

            ValidateLimitOrder();  // update the main order button

            AccountOrderVolume_textbox_TextChanged(null, null);
            AccountLimitPrice_textbox_TextChanged(null, null);

            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                PrivateIR.PrivateIREndPoints.GetClosedOrders,PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetTradingFees,
                PrivateIR.PrivateIREndPoints.GetAccounts
            }));
            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        /*private void IRAccountClose_button_Click(object sender, EventArgs e) {
            Main.Visible = true;  // this has to be above the UpdateLabel() call, because UpdateLabels() exits if main is invisible.
            // we stopped the UI from updating when the IR Accounts screen was showing, so let's update all the pairs now that we're closing the ACcounts page
            foreach (string dExchange in Exchanges) {
                UpdateLabels(dExchange);
            }

            LastPanel = Main;
            IRAccount_panel.Visible = false;

            Label CurrentSecondaryCurrecyLabel = UIControls_Dict["IR"].Label_Dict[DCE_IR.CurrentSecondaryCurrency + "_Account_Label"];
            CurrentSecondaryCurrecyLabel.ForeColor = Color.Black;
            CurrentSecondaryCurrecyLabel.Font = new Font(CurrentSecondaryCurrecyLabel.Font.FontFamily, 12f, FontStyle.Regular);
        }*/

        private void AccountWithdrawalAddress_label_Click(object sender, EventArgs e) {
            Label address = (Label)sender;
            if (!string.IsNullOrEmpty(address.Text)) {
                Clipboard.SetText(address.Text);
            }
            else {
                address.Text = "No address to copy?";
            }
        }

        private void AccountWithdrawalNextCheck_label_Click(object sender, EventArgs e) {
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.CheckAddress }));
        }

        private void Account_label_Click(object sender, EventArgs e) {
            cryptoClicked((Label)sender);
        }

        // market order, limit order, market baiter list box
        private void AcccountOrderType_listbox_SelectedIndexChanged(object sender, EventArgs e) {
            if (null == pIR) return;  // this sub seems to get called when the app opens.. 
            if (AccountOrderType_listbox.SelectedIndex == 1) {
                SwitchOrderBookSide_button.Enabled = true;
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
                pIR.OrderTypeStr = "Limit";
                AccountSwitchOrderBook(false);
            }
            else if (AccountOrderType_listbox.SelectedIndex == 0) {
                SwitchOrderBookSide_button.Enabled = true;
                AccountLimitPrice_label.Visible = false;
                AccountLimitPrice_textbox.Visible = false;
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    AccountPlaceOrder_button.Text = "Buy now";
                }
                else {
                    AccountPlaceOrder_button.Text = "Sell now";
                }
                AccountLimitPrice_label.ForeColor = Color.Black;
                AccountPlaceOrder_button.ForeColor = Color.Black;
                IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                if (pIR != null) pIR.OrderTypeStr = "Market";
                AccountSwitchOrderBook(false);
            }
            else if (AccountOrderType_listbox.SelectedIndex == 2) {  // market baiter!
                AccountPlaceOrder_button.Text = "Start baitin'";
                AccountLimitPrice_label.Visible = true;
                AccountLimitPrice_textbox.Visible = true;
                AccountLimitPrice_textbox_TextChanged(null, null);  // simulate a change in the limit text box to recalculate the order value
                pIR.OrderTypeStr = "Limit";  // we can now place limit orders while baitin'

                // switch the order book to the side we're dealing in
                //SwitchOrderBookSide_button.Enabled = false;  // we're now monitoring this side, no changes allowed.
                AccountSwitchOrderBook(true);  // switches the OB shown
                pIR.OrderTypeStr = "Limit";
                //updateAccountOrderBook(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency);
            }
            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));

            AccountPlaceOrder_button.Enabled = VolumePriceParseable().Item1;
        }

        // if they chose Market Baiter, then we do the opposite - a buy will show bids and a sell will show offers
        // if they clicked market or limit, then we show the other order book, ie if they have buy selected, then we show offers, and if the have sell selected then we show bids
        private void AccountSwitchOrderBook(bool switchToLimit) {
            int BuySellIndex = AccountBuySell_listbox.SelectedIndex;
            if (switchToLimit) {
                if (BuySellIndex == 0) BuySellIndex = 1;
                else BuySellIndex = 0;
            }
            if (BuySellIndex == 1) {
                pIR.OrderBookSide = "Bid";
                AccountOrders_listview.Columns[1].Text = "Bids";
                AccountOrders_listview.BackColor = Color.Thistle;
            }
            else {
                pIR.OrderBookSide = "Offer";
                AccountOrders_listview.Columns[1].Text = "Offers";
                AccountOrders_listview.BackColor = Color.PeachPuff;
            }
            checkSufficientVolume(null);
        }

        private void AccountBuySell_listbox_Click(object sender, EventArgs e) {
            if (AccountOrderType_listbox.SelectedIndex < 2) {  // if we're baitin', then don't change the button
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    AccountPlaceOrder_button.Text = "Buy now";
                    pIR.BuySell = "Buy";
                }
                else {
                    AccountPlaceOrder_button.Text = "Sell now";
                    pIR.BuySell = "Sell";
                }
                AccountSwitchOrderBook(false);
            }
            else {  // baitin'
                    // switch the order book to the side we're dealing in
                if (AccountBuySell_listbox.SelectedIndex == 0) {
                    pIR.BuySell = "Buy";
                }
                else {
                    pIR.BuySell = "Sell";
                }
                AccountSwitchOrderBook(true);

                if (pIR.marketBaiterActive) {
                    if (AccountBuySell_listbox.SelectedIndex == 0) {
                        AccountPlaceOrder_button.Text = "Buy now";
                    }
                    else {
                        AccountPlaceOrder_button.Text = "Sell now";
                    }
                }
            }
            if (AccountOrderType_listbox.SelectedIndex > 0) {   //  limit or bait
                ValidateLimitOrder();
            }

            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        /// <summary>
        /// checks if the vol and price are parsable
        /// </summary>
        /// <returns>item1 (bool) is whether or not we could parse, item2 is volume, item3 is the price</returns>
        private Tuple<bool, decimal, decimal> VolumePriceParseable() {
            int orderType = AccountOrderType_listbox.SelectedIndex;
            string volume = AccountOrderVolume_textbox.Text;
            string price = AccountLimitPrice_textbox.Text;
            if (orderType == 0) {   // market, only care about volume
                if (decimal.TryParse(volume, out decimal orderVol)) {
                    if (orderVol > 0) {
                        return new Tuple<bool, decimal, decimal>(true, orderVol, -1);
                    }
                }
                return new Tuple<bool, decimal, decimal>(false, -1, -1);
            }
            else {  // limit order or market baiter, need to check both fields
                decimal orderPrice = -1;
                decimal orderVol = -1;
                bool canParsePrice = false;
                bool canParseVol = false;
                if (decimal.TryParse(price, out decimal _orderPrice)) {
                    orderPrice = _orderPrice;
                    canParsePrice = true;
                }

                if (decimal.TryParse(volume, out decimal _orderVol)) {
                    orderVol = _orderVol;
                    canParseVol = true;
                }

                if (canParseVol && canParsePrice && (orderVol > 0) && (orderPrice >= 0)) {
                    return new Tuple<bool, decimal, decimal>(true, orderVol, orderPrice);
                }

                return new Tuple<bool, decimal, decimal>(false, orderVol, orderPrice);
            }
        }

        private void AccountOrderVolume_textbox_TextChanged(object sender, EventArgs e) {
            Tuple<bool, decimal, decimal> volPriceTup = VolumePriceParseable();
            decimal adjustedVol = volPriceTup.Item2;
            if (volPriceTup.Item2 > 0) {
                int mantissaLen = BitConverter.GetBytes(decimal.GetBits(volPriceTup.Item2)[3])[2];
                if (mantissaLen > DCE_IR.currencyDecimalPlaces[AccountSelectedCrypto].Item1) {
                    adjustedVol = Utilities.Truncate(volPriceTup.Item2, (byte)(DCE_IR.currencyDecimalPlaces[AccountSelectedCrypto].Item1));
                    AccountOrderVolume_textbox.Text = adjustedVol.ToString();
                    AccountOrderVolume_textbox.SelectionStart = AccountOrderVolume_textbox.Text.Length;
                    AccountOrderVolume_textbox.SelectionLength = 0;
                }
                pIR.Volume = adjustedVol;  // need to set this here because we need to tell pIR if we have a legit vol, even if the price is illegit or not entered yet
            }
            else {  // volume field does not have a number in it, so we set vol to 0
                pIR.Volume = 0;
            }

            if (volPriceTup.Item1) {
                if (AccountOrderType_listbox.SelectedIndex > 0) {  // limit or bait, we set the estimated notional label
                    AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(adjustedVol * volPriceTup.Item3);
                    AccountEstOrderValue_value.Tag = adjustedVol * volPriceTup.Item3;
                }
                AccountPlaceOrder_button.Enabled = true;
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
                AccountEstOrderValue_value.Tag = null;
            }

            bool isEnoughDepth = true;
            if ((null != AccountEstOrderValue_value.Tag) && AccountEstOrderValue_value.Tag.GetType() == typeof(decimal)) {
                if ((decimal)AccountEstOrderValue_value.Tag == -1) {
                    isEnoughDepth = false;
                }
            }

            checkSufficientVolume(pIR.Volume, isEnoughDepth);

            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        /**
         * Compares the volume entered against how much the account has to trade, colours the volume input field red if we don't have enough
         * @param {decimal} currentVol - the current volume entered into the field.  We send this because sometimes the calling function might edit the volume.  If null, we will attempt to pull and parse the volume from the UI field
         * return {void}
         */        
        private void checkSufficientVolume(decimal? _currentVol, bool isEnoughDepth = true) {

            if (!isEnoughDepth) {
                AccountOrderVolume_textbox.BackColor = Color.OrangeRed;
                return;
            }

            decimal currentVol;

            // we we're trying to buy or sell more than we can, colour the volume textbox red
            decimal totalToCheck;
            if (AccountBuySell_listbox.SelectedIndex == 0) { // buy selected, we compare the secondary currency
                if (IRT.UIControls_Dict.ContainsKey("IR") && (null != IRT.UIControls_Dict["IR"].Label_Dict) &&
                    IRT.UIControls_Dict["IR"].Label_Dict.ContainsKey(DCE_IR.CurrentSecondaryCurrency + "_Account_Total") &&
                    null != IRT.UIControls_Dict["IR"].Label_Dict[DCE_IR.CurrentSecondaryCurrency + "_Account_Total"].Tag)
                {
                    totalToCheck = (decimal)IRT.UIControls_Dict["IR"].Label_Dict[DCE_IR.CurrentSecondaryCurrency + "_Account_Total"].Tag;
                    if ((null != AccountEstOrderValue_value.Tag) && (AccountEstOrderValue_value.Tag.GetType() == typeof(decimal)) && ((decimal)AccountEstOrderValue_value.Tag == -1)) {  // -1 means not enough depth in the order book
                        AccountOrderVolume_textbox.BackColor = Color.OrangeRed; 
                        return;
                    }
                    if (null != AccountEstOrderValue_value.Tag) {  // for a buy deal, we compare the value of the trade against the fiat.  we store the value of the trade in the AccountEstOrderValue_value.Tag property
                        currentVol = (decimal)AccountEstOrderValue_value.Tag;
                    }
                    else {
                        currentVol = -1;  // if it's null, let's assume no volume, so we colour white
                    }
                }
                else {
                    Debug.Print("IRAccounts - No total for " + DCE_IR.CurrentSecondaryCurrency + " stored in the tag (or the label hasn't been loaded), bailing");
                    return;
                }
            }
            else {  // for a sell we check the primary
                if (null != IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"].Tag) {
                    totalToCheck = (decimal)IRT.UIControls_Dict["IR"].Label_Dict[AccountSelectedCrypto + "_Account_Total"].Tag;
                    if (_currentVol.HasValue) {
                        currentVol = _currentVol.Value;
                    }
                    else {
                        if (decimal.TryParse(AccountOrderVolume_textbox.Text, out decimal vol)) {
                            currentVol = vol;
                        }
                        else {  // volume field is blank or has letters or somethnig
                            AccountOrderVolume_textbox.BackColor = Color.White;
                            return;
                        }
                    }
                }
                else {
                    Debug.Print("IRAccounts - No total for " + AccountSelectedCrypto + " stored in the tag, bailing");
                    return;
                }
            }

            if (totalToCheck < currentVol) {
                AccountOrderVolume_textbox.BackColor = Color.OrangeRed;
            }
            else {
                AccountOrderVolume_textbox.BackColor = Color.White;
            }
        }

        private void StopBaitin_button_Click(object sender, EventArgs e) {
            if (!pIR.marketBaiterActive) return;  // this button should only be able to be clicked if we're baitin'
            pIR.marketBaiterActive = false;
            AccountPlaceOrder_button.Size = new Size(294, 39);
            StopBaitin_button.Visible = false;
            StopBaitin_button.Enabled = false;
        }

        private async void AccountPlaceOrder_button_Click(object sender, EventArgs e) {
            string orderSide = "";
            if (AccountBuySell_listbox.SelectedIndex == 0) orderSide = "buy";
            else orderSide = "sell";

            int oType = AccountOrderType_listbox.SelectedIndex;
            if (pIR.marketBaiterActive) oType = 1;  // if they click this button while baitin', then we treat it like a limit order

            DialogResult res = DialogResult.Cancel;
            if ((oType < 2) || pIR.marketBaiterActive) {  // assume limit order if we're baitin' and user tries to place a new order
                res = MessageBox.Show("Placing " + orderSide + " order!" + Environment.NewLine + Environment.NewLine +
                    "Size of order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + Utilities.FormatValue(decimal.Parse(AccountOrderVolume_textbox.Text)) + Environment.NewLine +
                    (oType == 0 ? "" : oType == 1 ? Utilities.FirstLetterToUpper(orderSide) + " price: $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine : "") +
                    "Estimated value of order: " + AccountEstOrderValue_value.Text,
                    "Confirm order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
            else if ((oType == 2) && !pIR.marketBaiterActive) {  // start market baiter

                res = MessageBox.Show("Start the market baiter strategy?" + Environment.NewLine + Environment.NewLine +
                    "This will create a " + orderSide + " order that will automatically move with the best order " +
                    "on the market, never going beyond $ " + Utilities.FormatValue(decimal.Parse(AccountLimitPrice_textbox.Text)) + Environment.NewLine + Environment.NewLine +
                    "Size of moving order: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + Utilities.FormatValue(decimal.Parse(AccountOrderVolume_textbox.Text), -1, false),
                    "Confirm market baiter order", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }

            if (res == DialogResult.OK) {

                AccountPlaceOrder_button.Enabled = false;
                AccountOrderVolume_textbox.Enabled = false;
                AccountLimitPrice_textbox.Enabled = false;
                if (oType == 2) {  // market baiter

                    // this was annoying, i never used it.
                    // now ask if they want to start the avg price thingo
                    /*res = MessageBox.Show("Start recording the average order price?" + Environment.NewLine + Environment.NewLine +
                        "This will open up the Average Price Calculator window and enable the auto update setting",
                        "Average Price Calculator", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (res == DialogResult.Yes) {
                        var _AccAvgPrice = new AccAvgPrice(DCE_IR, pIR, this, true, AccountSelectedCrypto, DCE_IR.CurrentSecondaryCurrency, AccountBuySell_listbox.SelectedIndex);
                        _AccAvgPrice.Show();
                    }*/

                    AccountPlaceOrder_button.Size = new Size(170, 39);
                    StopBaitin_button.Enabled = true;
                    StopBaitin_button.Visible = true;
                }


                // no need to check if we can parse the volume value, we already checked in AccountOrderVolume_textbox_TextChanged
                if (oType == 0) {
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                    PrivateIR.PrivateIREndPoints.PlaceMarketOrder, PrivateIR.PrivateIREndPoints.GetClosedOrders, PrivateIR.PrivateIREndPoints.GetAccounts }, decimal.Parse(AccountOrderVolume_textbox.Text)));
                }
                else if (oType == 1) {  // Limit order
                    Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                    PrivateIR.PrivateIREndPoints.PlaceLimitOrder, PrivateIR.PrivateIREndPoints.GetOpenOrders,
                    PrivateIR.PrivateIREndPoints.GetClosedOrders, PrivateIR.PrivateIREndPoints.GetAccounts }, decimal.Parse(AccountOrderVolume_textbox.Text), decimal.Parse(AccountLimitPrice_textbox.Text)));
                }
                else if (oType == 2) {  // market baiter
                    // do something that starts the market baiter
                    if (AccountBuySell_listbox.SelectedIndex == 0) pIR.BaiterBookSide = "Bid";
                    else pIR.BaiterBookSide = "Offer";
                    pIR.marketBaiterActive = true;
                    //AccountPlaceOrder_button.Text = "Stop market baiter and cancel order";
                    AccountBuySell_listbox_Click(null, null); // simulate changing the buy/sell so we set the button name corretly
                    ValidateLimitOrder();
                    Text = "IR Ticker - Market Baiter Running...";  // this is the form title bar
                    AccountBuySell_listbox.Enabled = false;
                    AccountOrderType_listbox.Enabled = false;
                    // stop using bulksequentialAPICalls for updating OB - it's not a private call.  I'm pretty sure this below await thing would have worked sometimes at best
                    await Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
                    //await updateOBTask;  // the idea here is to await the completion of the pIR.compileAccountOrderBookAsync(...) method
                    startMarketBaiter(decimal.Parse(AccountOrderVolume_textbox.Text), decimal.Parse(AccountLimitPrice_textbox.Text));
                }

                // need to disable the button until we have a result
                AccountPlaceOrder_button.Enabled = true;
                AccountOrderVolume_textbox.Enabled = true;
                AccountLimitPrice_textbox.Enabled = true;
            }
        }

        private async void startMarketBaiter(decimal volume, decimal limitPrice) {

            try {
                await Task.Run(() => pIR.marketBaiterLoopAsync(AccountSelectedCrypto, DCE_IR.CurrentSecondaryCurrency, volume, limitPrice));
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - Market baiter crashed.  error: " + ex.Message);
                notificationFromMarketBaiter(new Tuple<string, string>("Market Baiter", "Market Baiter crashed, and has stopped."), true);
            }

            ValidateLimitOrder();
            AccountBuySell_listbox.Enabled = true;
            AccountOrderType_listbox.Enabled = true;
            AccountPlaceOrder_button.Text = "Start baitin'";  // the user can't change the order type while baiting, so we can be sure it's still set to baiter when it fineshes
            AccountPlaceOrder_button.Size = new Size(294, 39);
            StopBaitin_button.Visible = false;
            StopBaitin_button.Enabled = false;
            Text = "IR Ticker";
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetClosedOrders, PrivateIR.PrivateIREndPoints.GetAccounts }));
            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        //public void updateUIFromMarketBaiter(List<PrivateIR.PrivateIREndPoints> endPoints) {
        //synchronizationContext.Post(new SendOrPostCallback(o => {
        //         bulkSequentialAPICalls(/*(List<PrivateIR.PrivateIREndPoints>)o*/endPoints);  // we are in the market baiter htread here, stay here
        //}), endPoints);
        //}

        public void notificationFromMarketBaiter(Tuple<string, string> notifText, bool sendToTelegram = false) {
            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                Tuple<string, string> notif = (Tuple<string, string>)o;
                IRT.showBalloon(notif.Item1, notif.Item2);
            }), notifText);

            if (sendToTelegram && (TGBot != null)) {
                IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    Tuple<string, string> notif = (Tuple<string, string>)o;
                    TGBot.SendMessage("*" + notif.Item1 + "*" + Environment.NewLine + "  " + notif.Item2);
                }), notifText);
            }
        }

        // this method checks the limit price, and if it would make the order a market order, then highlight buttons and shit
        // can only be called if AccountOrderType_listbox.SelectedIndex is 1 or 2 (limit or bait)
        private void ValidateLimitOrder() {

            if ((AccountOrderType_listbox.SelectedIndex == 0) || string.IsNullOrEmpty(AccountLimitPrice_textbox.Text)) return;  // this can happen when changing cryptos, we simulate a price text box update to validate and adjust
            
            if (!decimal.TryParse(AccountLimitPrice_textbox.Text, out decimal price)) {
                return;
            }
            if (AccountOrders_listview.Items.Count > 0) {  // only continue if we have orders in the OB
                // we need to make sure we have the top order of the order book we'd be trading on with a market order.  If it's on the
                // side of the order book we're not showing, then the best price of the correct order book will be stored in the topPriceOtherOB variable.
                decimal topPrice = (decimal)AccountOrders_listview.Items[0].SubItems[1].Tag;
                string buySell = (AccountBuySell_listbox.SelectedIndex == 0 ? "Bid" : "Offer");
                if (buySell == pIR.OrderBookSide) {
                    topPrice = topPriceOtherOB;
                }

                //decimal unformattedPrice = (decimal)AccountOrders_listview.Items[0].SubItems[1].Tag;  // this is only correct if we're viewing the correct side of the order book
                if (AccountBuySell_listbox.SelectedIndex == 0) {  // buy
                    if (price >= topPrice) {
                        AccountPlaceOrder_button.Text = "Possible MARKET buy";
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is higher than the lowest offer, this will be a market order!");
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) {
                            AccountPlaceOrder_button.Text = "Start baitin'";
                            AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Black;
                            IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                        }
                    }
                    else {
                        AccountPlaceOrder_button.Text = "Place limit buy";
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) { // actually ..
                            AccountPlaceOrder_button.Text = "Start baitin'";
                        }
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.ForeColor = Color.Black;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                    }
                }
                else {  // sell
                    if (price <= topPrice) {
                        AccountPlaceOrder_button.Text = "Possible MARKET sell";
                        AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Red;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "Price is lower than the higest bid, this will be a market order!");
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) {
                            AccountPlaceOrder_button.Text = "Start baitin'";
                            AccountLimitPrice_label.ForeColor = AccountPlaceOrder_button.ForeColor = Color.Black;
                            IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                        }
                    }
                    else {
                        AccountLimitPrice_label.ForeColor = Color.Black;
                        AccountPlaceOrder_button.Text = "Place limit sell";
                        if (!pIR.marketBaiterActive && (AccountOrderType_listbox.SelectedIndex == 2)) { // actually ..
                            AccountPlaceOrder_button.Text = "Start baitin'";
                        }
                        AccountPlaceOrder_button.ForeColor = Color.Black;
                        IRTickerTT_generic.SetToolTip(AccountPlaceOrder_button, "");
                    }
                }
            }
        }

        private void AccountLimitPrice_textbox_TextChanged(object sender, EventArgs e) {
            Tuple<bool, decimal, decimal> volPriceTup = VolumePriceParseable();
            decimal adjustedPrice = volPriceTup.Item3;
            if (adjustedPrice >= 0) {  // item3 will be -1 if not parseable.  If parseable, let's truncate.
                // we truncate the price field to obey our max decimal places for this crypto
                int mantissaLen = BitConverter.GetBytes(decimal.GetBits(volPriceTup.Item3)[3])[2];
                if (mantissaLen > DCE_IR.currencyDecimalPlaces[AccountSelectedCrypto].Item2) {
                    adjustedPrice = Utilities.Truncate(volPriceTup.Item3, (byte)(DCE_IR.currencyDecimalPlaces[AccountSelectedCrypto].Item2));
                    AccountLimitPrice_textbox.Text = adjustedPrice.ToString();
                    AccountLimitPrice_textbox.SelectionStart = AccountLimitPrice_textbox.Text.Length;
                    AccountLimitPrice_textbox.SelectionLength = 0;
                }
                pIR.LimitPrice = adjustedPrice;
            }

            if (volPriceTup.Item1) {  // both price and vol are parseable
                AccountPlaceOrder_button.Enabled = true;
                ValidateLimitOrder();
                AccountEstOrderValue_value.Text = "$ " + Utilities.FormatValue(volPriceTup.Item2 * adjustedPrice);
                AccountEstOrderValue_value.Tag = volPriceTup.Item2 * adjustedPrice;
            }
            // if VolumePriceParseable() not true, but we can parse the price field on it's own, then we can still colour some UI elements
            else if (adjustedPrice >= 0) {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
                AccountEstOrderValue_value.Tag = null;
                //ValidateLimitOrder();  // this only checks price, if the price or volume aren't parseable, then why are we calling this?
            }
            else {
                AccountPlaceOrder_button.Enabled = false;
                AccountEstOrderValue_value.Text = "";
                AccountEstOrderValue_value.Tag = null;
            }

            // Price has possibly changed, let's investigate and set the undo value if appropriate
            if (!undoIsUpdatingPrice) {  // only do stuff if this change is not triggered by CTRL + Z in the first place
                if (AccountLimitPrice_textbox.Text != buffer_lastPriceForUndo) {  // this is a new value, let's update the undo value
                    lastPriceForUndo = buffer_lastPriceForUndo;
                }
            }
            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        private void AccountOpenOrders_listview_DoubleClick(object sender, EventArgs e) {
            DialogResult res;
            try {
                res = MessageBox.Show("Do you want to cancel this order?" + Environment.NewLine + Environment.NewLine +
                    "Date created: " + AccountOpenOrders_listview.SelectedItems[0].SubItems[0].Text + Environment.NewLine +
                    "Volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOpenOrders_listview.SelectedItems[0].SubItems[1].Text + Environment.NewLine +
                    "Price: $ " + AccountOpenOrders_listview.SelectedItems[0].SubItems[2].Text + Environment.NewLine +
                    "Outstanding volume: " + (AccountSelectedCrypto == "XBT" ? "BTC " : AccountSelectedCrypto + " ") + AccountOpenOrders_listview.SelectedItems[0].SubItems[3].Text,
                    "Cancel order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            catch (Exception ex) {
                Debug.Print("tried to cancel an order but there were no orders selected somehow, silently failing... error: " + ex.Message);
                return;
            }

            if (res == DialogResult.Yes) {
                if (AccountOpenOrders_listview.SelectedItems.Count == 0) return;
                string orderGuid = ((BankHistoryOrder)AccountOpenOrders_listview.SelectedItems[0].Tag).OrderGuid.ToString();

                Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() {
                    PrivateIR.PrivateIREndPoints.CancelOrder, PrivateIR.PrivateIREndPoints.GetOpenOrders, PrivateIR.PrivateIREndPoints.GetAccounts 
                }, 0, 0, orderGuid));

                Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
            }
        }

        private void SwitchOrderBookSide_button_Click(object sender, EventArgs e) {
            if (pIR.OrderBookSide == "Bid") {
                pIR.OrderBookSide = "Offer";
                AccountOrders_listview.Columns[1].Text = "Offers";
                AccountOrders_listview.BackColor = Color.PeachPuff;
            }
            else {
                pIR.OrderBookSide = "Bid";
                AccountOrders_listview.Columns[1].Text = "Bids";
                AccountOrders_listview.BackColor = Color.Thistle;
            }
            Task.Run(() => pIR.compileAccountOrderBookAsync(AccountSelectedCrypto + "-" + DCE_IR.CurrentSecondaryCurrency));
        }

        private void AccountOrderVolume_textbox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Tab) {
                AccountOrderVolume_textbox.SelectAll();
            }
        }
        private void AccountLimitPrice_textbox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Tab) {
                AccountLimitPrice_textbox.SelectAll();
            }
        }

        private void AccountOrderVolume_label_DoubleClick(object sender, EventArgs e) {
            AccountOrderVolume_textbox.Text = pIR.accounts[AccountSelectedCrypto].AvailableBalance.ToString();
        }
        private void IRAccount_AvgPrice_Button_Click(object sender, EventArgs e) {
            var _AccAvgPrice = new AccAvgPrice(DCE_IR, pIR, this, null, enableAutoUpdate: true, crypto: AccountSelectedCrypto, fiat: DCE_IR.CurrentSecondaryCurrency, direction: AccountBuySell_listbox.SelectedIndex);
            _AccAvgPrice.Show();
        }

        // I had a crash here once, can't reproduce it.  instance not set to an object or something, but I couldn't see what was wrong.
        // maybe i should check that cOrders isn't somehow null?
        public void SignalAveragePriceUpdate(List<BankHistoryOrder> cOrders) {
            IRT.synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                foreach (Form frm in Application.OpenForms) {
                    if (frm.Name == "AccAvgPrice") {
                        ((AccAvgPrice)frm).UpdatePrice((List<BankHistoryOrder>)o);
                    }
                }
            }), cOrders);
        }

        public void RecalculateAvgPriceVariables(AccAvgPrice closingForm) {

            // first, let's reset the pIR.earliestClosedOrderRequired - we want this to be the earliest start date that isn't the closing form's date
            DateTime oldestDT = new DateTime(3000, 1, 1);  // if someone is still using this app in the year 3000, then I must have transcended time and space.  Congrats dude.
            pIR.AvgPriceSelectedCrypto.Clear();
            pIR.fiatCurrenciesSelected = new ConcurrentBag<string>();
            foreach (Form frm in Application.OpenForms) {
                if (frm.Name == "AccAvgPrice") {
                    if ((null != closingForm) && ((AccAvgPrice)frm == closingForm)) continue;  // ignore the form that's closing

                    if (((AccAvgPrice)frm).AccAvgPrice_Start_DTPicker.Value < oldestDT) {
                        oldestDT = ((AccAvgPrice)frm).AccAvgPrice_Start_DTPicker.Value;
                    }

                    // next let's reset pIR.AvgPriceSelectedCrypto
                    string normalisedCrypto = (((AccAvgPrice)frm).AccAvgPrice_Crypto_ComboBox.Text == "BTC" ? "XBT" : ((AccAvgPrice)frm).AccAvgPrice_Crypto_ComboBox.Text);

                    if (!pIR.AvgPriceSelectedCrypto.Contains(normalisedCrypto)) {
                        pIR.AvgPriceSelectedCrypto.Add(normalisedCrypto);
                    }

                    // and finally reset pIR.fiatCurrenciesSelected
                    foreach (var fiatButt in ((AccAvgPrice)frm).fiatCurrenciesSelected) {
                        if (!pIR.fiatCurrenciesSelected.Contains(fiatButt.Key)) {
                            if (fiatButt.Value.Item2) {
                                pIR.fiatCurrenciesSelected.Add(fiatButt.Key);
                            }
                        }
                    }
                }
            }
            if (oldestDT.Year != 3000) pIR.earliestClosedOrderRequired = oldestDT;
            else pIR.earliestClosedOrderRequired = null;
        }

        public void IRAccount_FillVolumeField(string vol) {
            AccountOrderVolume_textbox.Text = vol;

            // simulate a change in the text
            AccountOrderVolume_textbox_TextChanged(null, null);
        }

        // called when you click any of the fiat labels
        private void fiatLabel_Click(object sender, EventArgs e) {
            fiatClicked((System.Windows.Forms.Label)sender);
            IRT.IR_GroupBox_Click(sender, null);
        }

        public void AccountAPIKeys_comboBox_SelectedIndexChanged(object sender, EventArgs e) {

            AccountTradingFees_value.Text = "?";
            IRT.pIRAccountChanged((System.Windows.Forms.ComboBox)sender);
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.GetTradingFees }));

        }

        // double clicking a row in the OB view will populate the price and volume fields
        // with values that will buy/sell up to and including the order double clicked
        private void AccountOrders_listview_DoubleClick(object sender, EventArgs e) {
            if (AccountOrders_listview.SelectedItems.Count == 0) return;

            // this will only work if the order hasn't disappeared.  if it has, these will be 0
            //decimal price = (decimal)AccountOrders_listview.SelectedItems[0].SubItems[1].Tag;  // let's not set the price - usually we want this to be static when trading
            decimal volume = (decimal)AccountOrders_listview.SelectedItems[0].SubItems[3].Tag;

            //if ((price > 0) && (volume > 0)) {
            if (volume > 0) {
                //AccountLimitPrice_textbox.Text = price.ToString();  // let's not set the price - usually we want this to be static when trading
                AccountOrderVolume_textbox.Text = volume.ToString();
            }
        }

        private void AccountLimitPrice_textbox_Leave(object sender, EventArgs e) {
             
            buffer_lastPriceForUndo = AccountLimitPrice_textbox.Text;
        }

        bool bUndoDown = false;
        private void IRAccountsForm_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {

                case Keys.Z:
                    if (bUndoDown)
                        break;

                    if (e.Modifiers == (Keys.Control | Keys.Shift)) {
                        bUndoDown = true;
                    }
                    else if (Control.ModifierKeys == Keys.Control) {
                        bUndoDown = true;
                        if (AccountLimitPrice_textbox.Focused) {
                            undoIsUpdatingPrice = true;  // stop the textChanged undo code from running when we're changing the price because of undo keys..
                            AccountLimitPrice_textbox.Text = lastPriceForUndo;
                            undoIsUpdatingPrice = false;  // alright, undo text change is done, continue to store undo data as normal.
                        }
                    }
                    break;
            }
        }

        private void IRAccountsForm_KeyUp(object sender, KeyEventArgs e) {
            if (bUndoDown) {
                if (e.KeyCode == Keys.Z) {
                    bUndoDown = false;
                }
            }
        }

        // when they click an order in the order book list view
        private void AccountOrders_listview_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                // Determine the item that was clicked
                ListViewItem item = AccountOrders_listview.GetItemAt(e.X, e.Y);
                if (item != null) {
                    // Item is not null, a row was clicked
                    // Perform your action here
                    // For example, you can show a context menu
                    if (AccountOrders_listview.SelectedItems.Count == 0) return;

                    // this will only work if the order hasn't disappeared.  if it has, these will be 0
                    //decimal price = (decimal)AccountOrders_listview.SelectedItems[0].SubItems[1].Tag;  // let's not set the price - usually we want this to be static when trading
                    decimal volume = (decimal)AccountOrders_listview.SelectedItems[0].SubItems[3].Tag;

                    //if ((price > 0) && (volume > 0)) {
                    if (volume > 0) {
                        //AccountLimitPrice_textbox.Text = price.ToString();  // let's not set the price - usually we want this to be static when trading
                        AccountOrderVolume_textbox.Text = volume.ToString();
                    }
                }
            }
        }

        private void AccountTradingFees_value_MouseClick(object sender, MouseEventArgs e) {
            AccountTradingFees_value.Text = "Refreshing...";
            Task.Run(() => bulkSequentialAPICalls(new List<PrivateIR.PrivateIREndPoints>() { PrivateIR.PrivateIREndPoints.GetTradingFees }));
        }
    }
}
