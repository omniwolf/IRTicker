namespace IRTicker
{
    partial class IRTicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRTicker));
            this.refreshFrequencyTextbox = new System.Windows.Forms.MaskedTextBox();
            this.refreshFrequencyLabel = new System.Windows.Forms.Label();
            this.pollingThread = new System.ComponentModel.BackgroundWorker();
            this.Settings = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Settings_coinbase_panel = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Settings_coinbase_secret_textbox = new System.Windows.Forms.TextBox();
            this.Settings_coinbase_secret_label = new System.Windows.Forms.Label();
            this.Settings_coinbase_apikey_label = new System.Windows.Forms.Label();
            this.Settings_coinbase_apikey_textbox = new System.Windows.Forms.TextBox();
            this.Settings_coinbase_passphrase_textbox = new System.Windows.Forms.TextBox();
            this.Settings_coinbase_passphrase_label = new System.Windows.Forms.Label();
            this.IRAccountSettings_groupBox = new System.Windows.Forms.GroupBox();
            this.TGBot_Enable_checkBox = new System.Windows.Forms.CheckBox();
            this.TGBot_Enable_label = new System.Windows.Forms.Label();
            this.TelegramNewMessages_checkBox = new System.Windows.Forms.CheckBox();
            this.TelegramNewMessages_label = new System.Windows.Forms.Label();
            this.TGBot_APIToken_label = new System.Windows.Forms.Label();
            this.TelegramBotAPIToken_textBox = new System.Windows.Forms.TextBox();
            this.TGReset_button = new System.Windows.Forms.Button();
            this.TelegramCode_textBox = new System.Windows.Forms.TextBox();
            this.TelegramCode_label = new System.Windows.Forms.Label();
            this.IR_APIKey_label = new System.Windows.Forms.Label();
            this.EditKeys_button = new System.Windows.Forms.Button();
            this.APIKeys_comboBox = new System.Windows.Forms.ComboBox();
            this.ExportSummarised_Label = new System.Windows.Forms.Label();
            this.ExportSummarised_Checkbox = new System.Windows.Forms.CheckBox();
            this.spreadHistoryCustomFolderValue_Textbox = new System.Windows.Forms.TextBox();
            this.flashForm_label = new System.Windows.Forms.Label();
            this.spreadHistoryCustomFolder_label = new System.Windows.Forms.Label();
            this.flashForm_checkBox = new System.Windows.Forms.CheckBox();
            this.NegativeSpread_checkBox = new System.Windows.Forms.CheckBox();
            this.OB_label = new System.Windows.Forms.Label();
            this.NegativeSpread_label = new System.Windows.Forms.Label();
            this.OB_checkBox = new System.Windows.Forms.CheckBox();
            this.SlackSettings_groupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SlackNameEmojiCrypto_comboBox = new System.Windows.Forms.ComboBox();
            this.SlackNameCurrency_label = new System.Windows.Forms.Label();
            this.SlackNameFiatCurrency_comboBox = new System.Windows.Forms.ComboBox();
            this.Slack_label = new System.Windows.Forms.Label();
            this.Slack_checkBox = new System.Windows.Forms.CheckBox();
            this.slackToken_textBox = new System.Windows.Forms.TextBox();
            this.slackDefaultNameLabel = new System.Windows.Forms.Label();
            this.slackNameChangeLabel = new System.Windows.Forms.Label();
            this.slackDefaultNameTextBox = new System.Windows.Forms.TextBox();
            this.slackNameChangeCheckBox = new System.Windows.Forms.CheckBox();
            this.UITimerFreq_label = new System.Windows.Forms.Label();
            this.UITimerFreq_maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.SettingsSeparator_label = new System.Windows.Forms.Label();
            this.SessionStartedRel_label = new System.Windows.Forms.Label();
            this.SessionStartedAbs_label = new System.Windows.Forms.Label();
            this.SessionStart_label = new System.Windows.Forms.Label();
            this.Help_Button = new System.Windows.Forms.Button();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.SettingsOKButton = new System.Windows.Forms.Button();
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.GIFLabel = new System.Windows.Forms.Label();
            this.Main = new System.Windows.Forms.Panel();
            this.TradingVenues_comboBox = new System.Windows.Forms.ComboBox();
            this.IRUSD_GroupBox = new System.Windows.Forms.GroupBox();
            this.IR_panel_USD = new System.Windows.Forms.Panel();
            this.IRUSD_WIF_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_WIF_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_WIF_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_RENDER_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_RENDER_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_RENDER_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_TRX_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_TRX_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_TRX_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_SHIB_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_SHIB_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_SHIB_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_SAND_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_SAND_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_SAND_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_SOL_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_SOL_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_SOL_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_MANA_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_MANA_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_MANA_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_MATIC_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_MATIC_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_MATIC_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_DOGE_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_DOGE_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_DOGE_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_ADA_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_ADA_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_ADA_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_UNI_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_UNI_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_UNI_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_GRT_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_GRT_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_GRT_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_DOT_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_DOT_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_DOT_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_AAVE_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_AAVE_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_AAVE_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_YFI_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_YFI_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_YFI_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_SNX_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_SNX_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_SNX_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_COMP_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_COMP_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_COMP_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_USDC_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_USDC_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_USDC_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_LINK_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_LINK_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_LINK_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_DAI_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_DAI_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_DAI_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_USDT_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_USDT_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_USDT_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_ETC_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_ETC_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_ETC_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_MKR_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_MKR_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_MKR_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_BAT_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_BAT_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_BAT_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_XLM_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_XLM_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_XLM_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_EOS_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_EOS_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_EOS_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_ZRX_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_ZRX_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_XRP_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_ZRX_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_XRP_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_XBT_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_ETH_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_BCH_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_LTC_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_LTC_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_BCH_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_ETH_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_XBT_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_LTC_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_BCH_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_ETH_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_XBT_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_XRP_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_GroupBox = new System.Windows.Forms.GroupBox();
            this.IR_panel_SGD = new System.Windows.Forms.Panel();
            this.IRSGD_WIF_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_WIF_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_WIF_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_SHIB_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_SHIB_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_SHIB_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_TRX_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_TRX_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_TRX_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_RENDER_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_RENDER_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_RENDER_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_RLUSD_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_RLUSD_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_RLUSD_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_SAND_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_SAND_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_SAND_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_SOL_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_SOL_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_SOL_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_MANA_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_MANA_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_MANA_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_MATIC_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_MATIC_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_MATIC_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_DOGE_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_DOGE_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_DOGE_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_ADA_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_ADA_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_ADA_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_UNI_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_UNI_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_UNI_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_GRT_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_GRT_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_GRT_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_DOT_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_DOT_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_DOT_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_USDC_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_USDC_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_USDC_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_LINK_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_LINK_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_LINK_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_USDT_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_USDT_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_USDT_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_ETC_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_ETC_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_ETC_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_XLM_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_XLM_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_XLM_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_XRP_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_XRP_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_XBT_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_ETH_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_BCH_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_LTC_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_LTC_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_BCH_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_ETH_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_XBT_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_LTC_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_BCH_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_ETH_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_XBT_Label1 = new System.Windows.Forms.Label();
            this.IRSGD_XRP_Label3 = new System.Windows.Forms.Label();
            this.Balance_button = new System.Windows.Forms.Button();
            this.cryptoFees_groupBox = new System.Windows.Forms.GroupBox();
            this.cryptoFees_Panel = new System.Windows.Forms.Panel();
            this.BTC_LastBlock_Time_label = new System.Windows.Forms.Label();
            this.BTC_LastBlock_Time_value = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cryptoFees_LastUpdated_value = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cryptoFees_ETH_value = new System.Windows.Forms.Label();
            this.cryptoFees_BTC_value = new System.Windows.Forms.Label();
            this.BTCM_GroupBox = new System.Windows.Forms.GroupBox();
            this.BTCM_panel = new System.Windows.Forms.Panel();
            this.BTCM_ADA_Label2 = new System.Windows.Forms.Label();
            this.BTCM_ADA_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ADA_Label1 = new System.Windows.Forms.Label();
            this.BTCM_SOL_Label1 = new System.Windows.Forms.Label();
            this.BTCM_DOT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_SOL_Label3 = new System.Windows.Forms.Label();
            this.BTCM_DOT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_SOL_Label2 = new System.Windows.Forms.Label();
            this.BTCM_DOT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_AAVE_Label2 = new System.Windows.Forms.Label();
            this.BTCM_AAVE_Label3 = new System.Windows.Forms.Label();
            this.BTCM_AAVE_Label1 = new System.Windows.Forms.Label();
            this.BTCM_USDC_Label2 = new System.Windows.Forms.Label();
            this.BTCM_USDC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_USDC_Label1 = new System.Windows.Forms.Label();
            this.BTCM_MANA_Label2 = new System.Windows.Forms.Label();
            this.BTCM_MANA_Label3 = new System.Windows.Forms.Label();
            this.BTCM_MANA_Label1 = new System.Windows.Forms.Label();
            this.BTCM_SAND_Label1 = new System.Windows.Forms.Label();
            this.BTCM_UNI_Label2 = new System.Windows.Forms.Label();
            this.BTCM_SAND_Label3 = new System.Windows.Forms.Label();
            this.BTCM_UNI_Label3 = new System.Windows.Forms.Label();
            this.BTCM_SAND_Label2 = new System.Windows.Forms.Label();
            this.BTCM_UNI_Label1 = new System.Windows.Forms.Label();
            this.BTCM_USDT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_USDT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_USDT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_COMP_Label2 = new System.Windows.Forms.Label();
            this.BTCM_COMP_Label3 = new System.Windows.Forms.Label();
            this.BTCM_COMP_Label1 = new System.Windows.Forms.Label();
            this.BTCM_LINK_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_LINK_Label3 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label1 = new System.Windows.Forms.Label();
            this.BTCM_LINK_Label1 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label1 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label1 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label1 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETC_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETC_Label1 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label3 = new System.Windows.Forms.Label();
            this.BTCM_BAT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BAT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BAT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XLM_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XLM_Label3 = new System.Windows.Forms.Label();
            this.BTCM_XLM_Label1 = new System.Windows.Forms.Label();
            this.BTCM_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.BTCM_AvgPrice_Label = new System.Windows.Forms.Label();
            this.BTCM_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.BTCM_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.BTCM_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.BAR_GroupBox = new System.Windows.Forms.GroupBox();
            this.BAR_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.BAR_XBT_Label2 = new System.Windows.Forms.Label();
            this.BAR_XBT_Label3 = new System.Windows.Forms.Label();
            this.BAR_AvgPrice_Label = new System.Windows.Forms.Label();
            this.BAR_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.BAR_XBT_Label1 = new System.Windows.Forms.Label();
            this.BAR_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.BAR_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.IR_GroupBox = new System.Windows.Forms.GroupBox();
            this.IR_panel = new System.Windows.Forms.Panel();
            this.IR_WIF_Label2 = new System.Windows.Forms.Label();
            this.IR_WIF_Label1 = new System.Windows.Forms.Label();
            this.IR_WIF_Label3 = new System.Windows.Forms.Label();
            this.IR_RENDER_Label2 = new System.Windows.Forms.Label();
            this.IR_RENDER_Label1 = new System.Windows.Forms.Label();
            this.IR_RENDER_Label3 = new System.Windows.Forms.Label();
            this.IR_TRX_Label2 = new System.Windows.Forms.Label();
            this.IR_TRX_Label1 = new System.Windows.Forms.Label();
            this.IR_TRX_Label3 = new System.Windows.Forms.Label();
            this.IR_SHIB_Label2 = new System.Windows.Forms.Label();
            this.IR_SHIB_Label1 = new System.Windows.Forms.Label();
            this.IR_SHIB_Label3 = new System.Windows.Forms.Label();
            this.IR_SAND_Label2 = new System.Windows.Forms.Label();
            this.IR_SAND_Label1 = new System.Windows.Forms.Label();
            this.IR_SAND_Label3 = new System.Windows.Forms.Label();
            this.IR_SOL_Label2 = new System.Windows.Forms.Label();
            this.IR_SOL_Label1 = new System.Windows.Forms.Label();
            this.IR_SOL_Label3 = new System.Windows.Forms.Label();
            this.IR_MANA_Label2 = new System.Windows.Forms.Label();
            this.IR_MANA_Label1 = new System.Windows.Forms.Label();
            this.IR_MANA_Label3 = new System.Windows.Forms.Label();
            this.IR_MATIC_Label2 = new System.Windows.Forms.Label();
            this.IR_MATIC_Label1 = new System.Windows.Forms.Label();
            this.IR_MATIC_Label3 = new System.Windows.Forms.Label();
            this.IR_DOGE_Label2 = new System.Windows.Forms.Label();
            this.IR_DOGE_Label1 = new System.Windows.Forms.Label();
            this.IR_DOGE_Label3 = new System.Windows.Forms.Label();
            this.IR_ADA_Label2 = new System.Windows.Forms.Label();
            this.IR_ADA_Label1 = new System.Windows.Forms.Label();
            this.IR_ADA_Label3 = new System.Windows.Forms.Label();
            this.IR_UNI_Label2 = new System.Windows.Forms.Label();
            this.IR_UNI_Label1 = new System.Windows.Forms.Label();
            this.IR_UNI_Label3 = new System.Windows.Forms.Label();
            this.IR_GRT_Label2 = new System.Windows.Forms.Label();
            this.IR_GRT_Label1 = new System.Windows.Forms.Label();
            this.IR_GRT_Label3 = new System.Windows.Forms.Label();
            this.IR_DOT_Label2 = new System.Windows.Forms.Label();
            this.IR_DOT_Label1 = new System.Windows.Forms.Label();
            this.IR_DOT_Label3 = new System.Windows.Forms.Label();
            this.IR_AAVE_Label2 = new System.Windows.Forms.Label();
            this.IR_AAVE_Label1 = new System.Windows.Forms.Label();
            this.IR_AAVE_Label3 = new System.Windows.Forms.Label();
            this.IR_YFI_Label2 = new System.Windows.Forms.Label();
            this.IR_YFI_Label1 = new System.Windows.Forms.Label();
            this.IR_YFI_Label3 = new System.Windows.Forms.Label();
            this.IR_SNX_Label2 = new System.Windows.Forms.Label();
            this.IR_SNX_Label1 = new System.Windows.Forms.Label();
            this.IR_SNX_Label3 = new System.Windows.Forms.Label();
            this.IR_COMP_Label2 = new System.Windows.Forms.Label();
            this.IR_COMP_Label1 = new System.Windows.Forms.Label();
            this.IR_COMP_Label3 = new System.Windows.Forms.Label();
            this.IR_USDC_Label2 = new System.Windows.Forms.Label();
            this.IR_USDC_Label1 = new System.Windows.Forms.Label();
            this.IR_USDC_Label3 = new System.Windows.Forms.Label();
            this.IR_LINK_Label2 = new System.Windows.Forms.Label();
            this.IR_LINK_Label1 = new System.Windows.Forms.Label();
            this.IR_LINK_Label3 = new System.Windows.Forms.Label();
            this.IR_DAI_Label2 = new System.Windows.Forms.Label();
            this.IR_DAI_Label1 = new System.Windows.Forms.Label();
            this.IR_DAI_Label3 = new System.Windows.Forms.Label();
            this.IR_USDT_Label2 = new System.Windows.Forms.Label();
            this.IR_USDT_Label3 = new System.Windows.Forms.Label();
            this.IR_USDT_Label1 = new System.Windows.Forms.Label();
            this.IR_ETC_Label2 = new System.Windows.Forms.Label();
            this.IR_ETC_Label3 = new System.Windows.Forms.Label();
            this.IR_ETC_Label1 = new System.Windows.Forms.Label();
            this.IR_MKR_Label2 = new System.Windows.Forms.Label();
            this.IR_MKR_Label1 = new System.Windows.Forms.Label();
            this.IR_MKR_Label3 = new System.Windows.Forms.Label();
            this.IR_BAT_Label2 = new System.Windows.Forms.Label();
            this.IR_BAT_Label1 = new System.Windows.Forms.Label();
            this.IR_BAT_Label3 = new System.Windows.Forms.Label();
            this.IR_XLM_Label2 = new System.Windows.Forms.Label();
            this.IR_XLM_Label3 = new System.Windows.Forms.Label();
            this.IR_XLM_Label1 = new System.Windows.Forms.Label();
            this.IR_EOS_Label2 = new System.Windows.Forms.Label();
            this.IR_EOS_Label3 = new System.Windows.Forms.Label();
            this.IR_EOS_Label1 = new System.Windows.Forms.Label();
            this.IR_ZRX_Label2 = new System.Windows.Forms.Label();
            this.IR_ZRX_Label1 = new System.Windows.Forms.Label();
            this.IR_XRP_Label2 = new System.Windows.Forms.Label();
            this.IR_ZRX_Label3 = new System.Windows.Forms.Label();
            this.IR_XRP_Label1 = new System.Windows.Forms.Label();
            this.IR_XBT_Label2 = new System.Windows.Forms.Label();
            this.IR_ETH_Label2 = new System.Windows.Forms.Label();
            this.IR_BCH_Label2 = new System.Windows.Forms.Label();
            this.IR_LTC_Label2 = new System.Windows.Forms.Label();
            this.IR_LTC_Label3 = new System.Windows.Forms.Label();
            this.IR_BCH_Label3 = new System.Windows.Forms.Label();
            this.IR_ETH_Label3 = new System.Windows.Forms.Label();
            this.IR_XBT_Label3 = new System.Windows.Forms.Label();
            this.IR_LTC_Label1 = new System.Windows.Forms.Label();
            this.IR_BCH_Label1 = new System.Windows.Forms.Label();
            this.IR_ETH_Label1 = new System.Windows.Forms.Label();
            this.IR_XBT_Label1 = new System.Windows.Forms.Label();
            this.IR_XRP_Label3 = new System.Windows.Forms.Label();
            this.IR_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.IR_Reset_Button = new System.Windows.Forms.Button();
            this.SpreadVolumeTitle_Label = new System.Windows.Forms.Label();
            this.IR_AvgPrice_Label = new System.Windows.Forms.Label();
            this.IR_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.IR_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.IR_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.SGD_Label2 = new System.Windows.Forms.Label();
            this.AUD_Label1 = new System.Windows.Forms.Label();
            this.EUR_Label1 = new System.Windows.Forms.Label();
            this.fiatRefresh_checkBox = new System.Windows.Forms.CheckBox();
            this.NZD_Label1 = new System.Windows.Forms.Label();
            this.SGD_Label1 = new System.Windows.Forms.Label();
            this.EUR_Label2 = new System.Windows.Forms.Label();
            this.NZD_Label2 = new System.Windows.Forms.Label();
            this.USD_Label2 = new System.Windows.Forms.Label();
            this.AUD_Label2 = new System.Windows.Forms.Label();
            this.USD_Label1 = new System.Windows.Forms.Label();
            this.IRTickerTT_spread = new System.Windows.Forms.ToolTip(this.components);
            this.OTCHelper = new System.Windows.Forms.Panel();
            this.StepVolume_Label7 = new System.Windows.Forms.Label();
            this.StepPrice_Label7 = new System.Windows.Forms.Label();
            this.StepVolume_Label6 = new System.Windows.Forms.Label();
            this.StepPrice_Label6 = new System.Windows.Forms.Label();
            this.StepVolume_Label5 = new System.Windows.Forms.Label();
            this.StepPrice_Label5 = new System.Windows.Forms.Label();
            this.StepVolume_Label4 = new System.Windows.Forms.Label();
            this.StepPrice_Label4 = new System.Windows.Forms.Label();
            this.StepPrice_Label3 = new System.Windows.Forms.Label();
            this.StepVolume_Label3 = new System.Windows.Forms.Label();
            this.StepVolume_Label2 = new System.Windows.Forms.Label();
            this.StepPrice_Label2 = new System.Windows.Forms.Label();
            this.StepVolume_Label1 = new System.Windows.Forms.Label();
            this.StepPrice_Label1 = new System.Windows.Forms.Label();
            this.MarketBuyCrypto_Label = new System.Windows.Forms.Label();
            this.CashInput_MaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.CryptoChooser_ComboBox = new System.Windows.Forms.ComboBox();
            this.spreadHistory_FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.IRTickerTT_avgPrice = new System.Windows.Forms.ToolTip(this.components);
            this.IRTickerTT_generic = new System.Windows.Forms.ToolTip(this.components);
            this.IRT_notification = new System.Windows.Forms.NotifyIcon(this.components);
            this.IR_PEPE_Label2 = new System.Windows.Forms.Label();
            this.IR_PEPE_Label1 = new System.Windows.Forms.Label();
            this.IR_PEPE_Label3 = new System.Windows.Forms.Label();
            this.IR_TRUMP_Label2 = new System.Windows.Forms.Label();
            this.IR_TRUMP_Label1 = new System.Windows.Forms.Label();
            this.IR_TRUMP_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_PEPE_Label2 = new System.Windows.Forms.Label();
            this.IRSGD_PEPE_Label3 = new System.Windows.Forms.Label();
            this.IRSGD_PEPE_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_PEPE_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_PEPE_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_PEPE_Label3 = new System.Windows.Forms.Label();
            this.IRUSD_TRUMP_Label2 = new System.Windows.Forms.Label();
            this.IRUSD_TRUMP_Label1 = new System.Windows.Forms.Label();
            this.IRUSD_TRUMP_Label3 = new System.Windows.Forms.Label();
            this.Settings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Settings_coinbase_panel.SuspendLayout();
            this.IRAccountSettings_groupBox.SuspendLayout();
            this.SlackSettings_groupBox.SuspendLayout();
            this.LoadingPanel.SuspendLayout();
            this.Main.SuspendLayout();
            this.IRUSD_GroupBox.SuspendLayout();
            this.IR_panel_USD.SuspendLayout();
            this.IRSGD_GroupBox.SuspendLayout();
            this.IR_panel_SGD.SuspendLayout();
            this.cryptoFees_groupBox.SuspendLayout();
            this.cryptoFees_Panel.SuspendLayout();
            this.BTCM_GroupBox.SuspendLayout();
            this.BTCM_panel.SuspendLayout();
            this.BAR_GroupBox.SuspendLayout();
            this.IR_GroupBox.SuspendLayout();
            this.IR_panel.SuspendLayout();
            this.OTCHelper.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshFrequencyTextbox
            // 
            this.refreshFrequencyTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshFrequencyTextbox.Location = new System.Drawing.Point(414, 2);
            this.refreshFrequencyTextbox.Mask = "00000";
            this.refreshFrequencyTextbox.Name = "refreshFrequencyTextbox";
            this.refreshFrequencyTextbox.PromptChar = ' ';
            this.refreshFrequencyTextbox.Size = new System.Drawing.Size(68, 32);
            this.refreshFrequencyTextbox.TabIndex = 0;
            this.refreshFrequencyTextbox.Text = "1";
            this.refreshFrequencyTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.refreshFrequencyTextbox.ValidatingType = typeof(int);
            this.refreshFrequencyTextbox.TextChanged += new System.EventHandler(this.RefreshFrequencyTextbox_TextChanged);
            // 
            // refreshFrequencyLabel
            // 
            this.refreshFrequencyLabel.AutoSize = true;
            this.refreshFrequencyLabel.Location = new System.Drawing.Point(50, 9);
            this.refreshFrequencyLabel.Name = "refreshFrequencyLabel";
            this.refreshFrequencyLabel.Size = new System.Drawing.Size(223, 13);
            this.refreshFrequencyLabel.TabIndex = 1;
            this.refreshFrequencyLabel.Text = "How fast should the app refresh (in seconds)?";
            // 
            // pollingThread
            // 
            this.pollingThread.WorkerReportsProgress = true;
            this.pollingThread.WorkerSupportsCancellation = true;
            this.pollingThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PollingThread_DoWork);
            this.pollingThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.PollingThread_ReportProgress);
            this.pollingThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PollingThread_RunWorkerCompleted);
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.White;
            this.Settings.Controls.Add(this.panel1);
            this.Settings.Controls.Add(this.SettingsSeparator_label);
            this.Settings.Controls.Add(this.SessionStartedRel_label);
            this.Settings.Controls.Add(this.SessionStartedAbs_label);
            this.Settings.Controls.Add(this.SessionStart_label);
            this.Settings.Controls.Add(this.Help_Button);
            this.Settings.Controls.Add(this.VersionLabel);
            this.Settings.Controls.Add(this.SettingsOKButton);
            this.Settings.Location = new System.Drawing.Point(0, 0);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(585, 842);
            this.Settings.TabIndex = 4;
            this.Settings.Visible = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.Settings_coinbase_panel);
            this.panel1.Controls.Add(this.refreshFrequencyLabel);
            this.panel1.Controls.Add(this.IRAccountSettings_groupBox);
            this.panel1.Controls.Add(this.refreshFrequencyTextbox);
            this.panel1.Controls.Add(this.ExportSummarised_Label);
            this.panel1.Controls.Add(this.ExportSummarised_Checkbox);
            this.panel1.Controls.Add(this.spreadHistoryCustomFolderValue_Textbox);
            this.panel1.Controls.Add(this.flashForm_label);
            this.panel1.Controls.Add(this.spreadHistoryCustomFolder_label);
            this.panel1.Controls.Add(this.flashForm_checkBox);
            this.panel1.Controls.Add(this.NegativeSpread_checkBox);
            this.panel1.Controls.Add(this.OB_label);
            this.panel1.Controls.Add(this.NegativeSpread_label);
            this.panel1.Controls.Add(this.OB_checkBox);
            this.panel1.Controls.Add(this.SlackSettings_groupBox);
            this.panel1.Controls.Add(this.UITimerFreq_label);
            this.panel1.Controls.Add(this.UITimerFreq_maskedTextBox);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 755);
            this.panel1.TabIndex = 42;
            // 
            // Settings_coinbase_panel
            // 
            this.Settings_coinbase_panel.BackColor = System.Drawing.Color.Gainsboro;
            this.Settings_coinbase_panel.Controls.Add(this.label1);
            this.Settings_coinbase_panel.Controls.Add(this.Settings_coinbase_secret_textbox);
            this.Settings_coinbase_panel.Controls.Add(this.Settings_coinbase_secret_label);
            this.Settings_coinbase_panel.Controls.Add(this.Settings_coinbase_apikey_label);
            this.Settings_coinbase_panel.Controls.Add(this.Settings_coinbase_apikey_textbox);
            this.Settings_coinbase_panel.Controls.Add(this.Settings_coinbase_passphrase_textbox);
            this.Settings_coinbase_panel.Controls.Add(this.Settings_coinbase_passphrase_label);
            this.Settings_coinbase_panel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_panel.Location = new System.Drawing.Point(53, 703);
            this.Settings_coinbase_panel.Name = "Settings_coinbase_panel";
            this.Settings_coinbase_panel.Size = new System.Drawing.Size(461, 193);
            this.Settings_coinbase_panel.TabIndex = 48;
            this.Settings_coinbase_panel.TabStop = false;
            this.Settings_coinbase_panel.Text = "Coinbase account";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 48);
            this.label1.TabIndex = 48;
            this.label1.Text = "Coinbase key needs at least Trade permissions.\r\nThese API key details are shared " +
    "between here\r\nand the Balance settings screen.";
            // 
            // Settings_coinbase_secret_textbox
            // 
            this.Settings_coinbase_secret_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_secret_textbox.Location = new System.Drawing.Point(106, 90);
            this.Settings_coinbase_secret_textbox.Name = "Settings_coinbase_secret_textbox";
            this.Settings_coinbase_secret_textbox.Size = new System.Drawing.Size(338, 20);
            this.Settings_coinbase_secret_textbox.TabIndex = 46;
            this.IRTickerTT_generic.SetToolTip(this.Settings_coinbase_secret_textbox, "Leave blank to disable Telegram integration");
            this.Settings_coinbase_secret_textbox.UseSystemPasswordChar = true;
            // 
            // Settings_coinbase_secret_label
            // 
            this.Settings_coinbase_secret_label.AccessibleName = "";
            this.Settings_coinbase_secret_label.AutoSize = true;
            this.Settings_coinbase_secret_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_secret_label.Location = new System.Drawing.Point(24, 91);
            this.Settings_coinbase_secret_label.Name = "Settings_coinbase_secret_label";
            this.Settings_coinbase_secret_label.Size = new System.Drawing.Size(46, 16);
            this.Settings_coinbase_secret_label.TabIndex = 47;
            this.Settings_coinbase_secret_label.Text = "Secret";
            // 
            // Settings_coinbase_apikey_label
            // 
            this.Settings_coinbase_apikey_label.AccessibleName = "";
            this.Settings_coinbase_apikey_label.AutoSize = true;
            this.Settings_coinbase_apikey_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_apikey_label.Location = new System.Drawing.Point(24, 32);
            this.Settings_coinbase_apikey_label.Name = "Settings_coinbase_apikey_label";
            this.Settings_coinbase_apikey_label.Size = new System.Drawing.Size(53, 16);
            this.Settings_coinbase_apikey_label.TabIndex = 45;
            this.Settings_coinbase_apikey_label.Text = "API key";
            // 
            // Settings_coinbase_apikey_textbox
            // 
            this.Settings_coinbase_apikey_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_apikey_textbox.Location = new System.Drawing.Point(106, 30);
            this.Settings_coinbase_apikey_textbox.Name = "Settings_coinbase_apikey_textbox";
            this.Settings_coinbase_apikey_textbox.Size = new System.Drawing.Size(338, 20);
            this.Settings_coinbase_apikey_textbox.TabIndex = 37;
            // 
            // Settings_coinbase_passphrase_textbox
            // 
            this.Settings_coinbase_passphrase_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_passphrase_textbox.Location = new System.Drawing.Point(106, 59);
            this.Settings_coinbase_passphrase_textbox.Name = "Settings_coinbase_passphrase_textbox";
            this.Settings_coinbase_passphrase_textbox.Size = new System.Drawing.Size(338, 20);
            this.Settings_coinbase_passphrase_textbox.TabIndex = 37;
            this.IRTickerTT_generic.SetToolTip(this.Settings_coinbase_passphrase_textbox, "Leave blank to disable Telegram integration");
            this.Settings_coinbase_passphrase_textbox.UseSystemPasswordChar = true;
            // 
            // Settings_coinbase_passphrase_label
            // 
            this.Settings_coinbase_passphrase_label.AccessibleName = "";
            this.Settings_coinbase_passphrase_label.AutoSize = true;
            this.Settings_coinbase_passphrase_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_coinbase_passphrase_label.Location = new System.Drawing.Point(24, 60);
            this.Settings_coinbase_passphrase_label.Name = "Settings_coinbase_passphrase_label";
            this.Settings_coinbase_passphrase_label.Size = new System.Drawing.Size(80, 16);
            this.Settings_coinbase_passphrase_label.TabIndex = 42;
            this.Settings_coinbase_passphrase_label.Text = "Passphrase";
            // 
            // IRAccountSettings_groupBox
            // 
            this.IRAccountSettings_groupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.IRAccountSettings_groupBox.Controls.Add(this.TGBot_Enable_checkBox);
            this.IRAccountSettings_groupBox.Controls.Add(this.TGBot_Enable_label);
            this.IRAccountSettings_groupBox.Controls.Add(this.TelegramNewMessages_checkBox);
            this.IRAccountSettings_groupBox.Controls.Add(this.TelegramNewMessages_label);
            this.IRAccountSettings_groupBox.Controls.Add(this.TGBot_APIToken_label);
            this.IRAccountSettings_groupBox.Controls.Add(this.TelegramBotAPIToken_textBox);
            this.IRAccountSettings_groupBox.Controls.Add(this.TGReset_button);
            this.IRAccountSettings_groupBox.Controls.Add(this.TelegramCode_textBox);
            this.IRAccountSettings_groupBox.Controls.Add(this.TelegramCode_label);
            this.IRAccountSettings_groupBox.Controls.Add(this.IR_APIKey_label);
            this.IRAccountSettings_groupBox.Controls.Add(this.EditKeys_button);
            this.IRAccountSettings_groupBox.Controls.Add(this.APIKeys_comboBox);
            this.IRAccountSettings_groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRAccountSettings_groupBox.Location = new System.Drawing.Point(53, 501);
            this.IRAccountSettings_groupBox.Name = "IRAccountSettings_groupBox";
            this.IRAccountSettings_groupBox.Size = new System.Drawing.Size(461, 190);
            this.IRAccountSettings_groupBox.TabIndex = 37;
            this.IRAccountSettings_groupBox.TabStop = false;
            this.IRAccountSettings_groupBox.Text = "IR account";
            // 
            // TGBot_Enable_checkBox
            // 
            this.TGBot_Enable_checkBox.AccessibleName = "";
            this.TGBot_Enable_checkBox.AutoSize = true;
            this.TGBot_Enable_checkBox.Location = new System.Drawing.Point(429, 82);
            this.TGBot_Enable_checkBox.Name = "TGBot_Enable_checkBox";
            this.TGBot_Enable_checkBox.Size = new System.Drawing.Size(15, 14);
            this.TGBot_Enable_checkBox.TabIndex = 47;
            this.TGBot_Enable_checkBox.UseVisualStyleBackColor = true;
            // 
            // TGBot_Enable_label
            // 
            this.TGBot_Enable_label.AccessibleName = "";
            this.TGBot_Enable_label.AutoSize = true;
            this.TGBot_Enable_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TGBot_Enable_label.Location = new System.Drawing.Point(24, 80);
            this.TGBot_Enable_label.Name = "TGBot_Enable_label";
            this.TGBot_Enable_label.Size = new System.Drawing.Size(177, 16);
            this.TGBot_Enable_label.TabIndex = 46;
            this.TGBot_Enable_label.Text = "Enable Telegram integration";
            this.IRTickerTT_generic.SetToolTip(this.TGBot_Enable_label, resources.GetString("TGBot_Enable_label.ToolTip"));
            // 
            // TelegramNewMessages_checkBox
            // 
            this.TelegramNewMessages_checkBox.AccessibleName = "";
            this.TelegramNewMessages_checkBox.AutoSize = true;
            this.TelegramNewMessages_checkBox.Location = new System.Drawing.Point(429, 164);
            this.TelegramNewMessages_checkBox.Name = "TelegramNewMessages_checkBox";
            this.TelegramNewMessages_checkBox.Size = new System.Drawing.Size(15, 14);
            this.TelegramNewMessages_checkBox.TabIndex = 43;
            this.TelegramNewMessages_checkBox.UseVisualStyleBackColor = true;
            // 
            // TelegramNewMessages_label
            // 
            this.TelegramNewMessages_label.AccessibleName = "";
            this.TelegramNewMessages_label.AutoSize = true;
            this.TelegramNewMessages_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TelegramNewMessages_label.Location = new System.Drawing.Point(24, 162);
            this.TelegramNewMessages_label.Name = "TelegramNewMessages_label";
            this.TelegramNewMessages_label.Size = new System.Drawing.Size(288, 16);
            this.TelegramNewMessages_label.TabIndex = 42;
            this.TelegramNewMessages_label.Text = "Make all Telegram messages new for dictation:";
            this.IRTickerTT_generic.SetToolTip(this.TelegramNewMessages_label, resources.GetString("TelegramNewMessages_label.ToolTip"));
            // 
            // TGBot_APIToken_label
            // 
            this.TGBot_APIToken_label.AccessibleName = "";
            this.TGBot_APIToken_label.AutoSize = true;
            this.TGBot_APIToken_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TGBot_APIToken_label.Location = new System.Drawing.Point(24, 105);
            this.TGBot_APIToken_label.Name = "TGBot_APIToken_label";
            this.TGBot_APIToken_label.Size = new System.Drawing.Size(151, 16);
            this.TGBot_APIToken_label.TabIndex = 45;
            this.TGBot_APIToken_label.Text = "Telegram bot API token:";
            // 
            // TelegramBotAPIToken_textBox
            // 
            this.TelegramBotAPIToken_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TelegramBotAPIToken_textBox.Location = new System.Drawing.Point(188, 103);
            this.TelegramBotAPIToken_textBox.Name = "TelegramBotAPIToken_textBox";
            this.TelegramBotAPIToken_textBox.PasswordChar = '●';
            this.TelegramBotAPIToken_textBox.Size = new System.Drawing.Size(256, 20);
            this.TelegramBotAPIToken_textBox.TabIndex = 37;
            this.TelegramBotAPIToken_textBox.UseSystemPasswordChar = true;
            // 
            // TGReset_button
            // 
            this.TGReset_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TGReset_button.Location = new System.Drawing.Point(360, 129);
            this.TGReset_button.Name = "TGReset_button";
            this.TGReset_button.Size = new System.Drawing.Size(84, 23);
            this.TGReset_button.TabIndex = 44;
            this.TGReset_button.Text = "Reset bot";
            this.IRTickerTT_generic.SetToolTip(this.TGReset_button, "Clicking this button will un-authenticate IR Ticker with any Telegram chat");
            this.TGReset_button.UseVisualStyleBackColor = true;
            this.TGReset_button.Click += new System.EventHandler(this.TGReset_button_Click);
            // 
            // TelegramCode_textBox
            // 
            this.TelegramCode_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TelegramCode_textBox.Location = new System.Drawing.Point(188, 132);
            this.TelegramCode_textBox.Name = "TelegramCode_textBox";
            this.TelegramCode_textBox.Size = new System.Drawing.Size(168, 20);
            this.TelegramCode_textBox.TabIndex = 37;
            this.IRTickerTT_generic.SetToolTip(this.TelegramCode_textBox, "Leave blank to disable Telegram integration");
            // 
            // TelegramCode_label
            // 
            this.TelegramCode_label.AccessibleName = "";
            this.TelegramCode_label.AutoSize = true;
            this.TelegramCode_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TelegramCode_label.Location = new System.Drawing.Point(24, 133);
            this.TelegramCode_label.Name = "TelegramCode_label";
            this.TelegramCode_label.Size = new System.Drawing.Size(143, 16);
            this.TelegramCode_label.TabIndex = 42;
            this.TelegramCode_label.Text = "Telegram secret code:";
            // 
            // IR_APIKey_label
            // 
            this.IR_APIKey_label.AutoSize = true;
            this.IR_APIKey_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_APIKey_label.Location = new System.Drawing.Point(24, 26);
            this.IR_APIKey_label.Name = "IR_APIKey_label";
            this.IR_APIKey_label.Size = new System.Drawing.Size(228, 16);
            this.IR_APIKey_label.TabIndex = 43;
            this.IR_APIKey_label.Text = "Choose which API keyto connect with:";
            // 
            // EditKeys_button
            // 
            this.EditKeys_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditKeys_button.Location = new System.Drawing.Point(360, 52);
            this.EditKeys_button.Name = "EditKeys_button";
            this.EditKeys_button.Size = new System.Drawing.Size(84, 23);
            this.EditKeys_button.TabIndex = 42;
            this.EditKeys_button.Text = "Edit keys";
            this.EditKeys_button.UseVisualStyleBackColor = true;
            this.EditKeys_button.Click += new System.EventHandler(this.EditKeys_button_Click);
            // 
            // APIKeys_comboBox
            // 
            this.APIKeys_comboBox.DisplayMember = "friendlyName";
            this.APIKeys_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.APIKeys_comboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.APIKeys_comboBox.FormattingEnabled = true;
            this.APIKeys_comboBox.Location = new System.Drawing.Point(19, 53);
            this.APIKeys_comboBox.Name = "APIKeys_comboBox";
            this.APIKeys_comboBox.Size = new System.Drawing.Size(323, 24);
            this.APIKeys_comboBox.TabIndex = 32;
            this.APIKeys_comboBox.ValueMember = "friendlyName";
            this.APIKeys_comboBox.SelectedIndexChanged += new System.EventHandler(this.APIKeys_comboBox_SelectedIndexChanged);
            // 
            // ExportSummarised_Label
            // 
            this.ExportSummarised_Label.AccessibleName = "";
            this.ExportSummarised_Label.AutoSize = true;
            this.ExportSummarised_Label.Location = new System.Drawing.Point(50, 44);
            this.ExportSummarised_Label.Name = "ExportSummarised_Label";
            this.ExportSummarised_Label.Size = new System.Drawing.Size(146, 13);
            this.ExportSummarised_Label.TabIndex = 17;
            this.ExportSummarised_Label.Text = "Export summary spread data?";
            this.IRTickerTT_generic.SetToolTip(this.ExportSummarised_Label, "This will save one datapoint per hour, which is averaged over the last hour.");
            // 
            // ExportSummarised_Checkbox
            // 
            this.ExportSummarised_Checkbox.AccessibleName = "";
            this.ExportSummarised_Checkbox.AutoSize = true;
            this.ExportSummarised_Checkbox.Location = new System.Drawing.Point(467, 44);
            this.ExportSummarised_Checkbox.Name = "ExportSummarised_Checkbox";
            this.ExportSummarised_Checkbox.Size = new System.Drawing.Size(15, 14);
            this.ExportSummarised_Checkbox.TabIndex = 18;
            this.ExportSummarised_Checkbox.UseVisualStyleBackColor = true;
            this.ExportSummarised_Checkbox.CheckedChanged += new System.EventHandler(this.ExportSummarised_Checkbox_CheckedChanged);
            // 
            // spreadHistoryCustomFolderValue_Textbox
            // 
            this.spreadHistoryCustomFolderValue_Textbox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.spreadHistoryCustomFolderValue_Textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spreadHistoryCustomFolderValue_Textbox.Location = new System.Drawing.Point(203, 73);
            this.spreadHistoryCustomFolderValue_Textbox.Name = "spreadHistoryCustomFolderValue_Textbox";
            this.spreadHistoryCustomFolderValue_Textbox.ReadOnly = true;
            this.spreadHistoryCustomFolderValue_Textbox.Size = new System.Drawing.Size(279, 20);
            this.spreadHistoryCustomFolderValue_Textbox.TabIndex = 37;
            this.spreadHistoryCustomFolderValue_Textbox.Text = "G:\\My Drive\\IR\\IRTicker\\Spread history data\\";
            this.IRTickerTT_generic.SetToolTip(this.spreadHistoryCustomFolderValue_Textbox, "Default is G:\\My Drive\\IR\\IRTicker\\Spread history data\\");
            this.spreadHistoryCustomFolderValue_Textbox.Click += new System.EventHandler(this.spreadHistoryCustomFolderValue_Textbox_Click);
            // 
            // flashForm_label
            // 
            this.flashForm_label.AccessibleName = "";
            this.flashForm_label.Location = new System.Drawing.Point(50, 372);
            this.flashForm_label.Name = "flashForm_label";
            this.flashForm_label.Size = new System.Drawing.Size(298, 26);
            this.flashForm_label.TabIndex = 21;
            this.flashForm_label.Text = "Flash the window if IR resets (only useful for debugging)";
            // 
            // spreadHistoryCustomFolder_label
            // 
            this.spreadHistoryCustomFolder_label.AccessibleName = "";
            this.spreadHistoryCustomFolder_label.AutoSize = true;
            this.spreadHistoryCustomFolder_label.Location = new System.Drawing.Point(50, 73);
            this.spreadHistoryCustomFolder_label.Name = "spreadHistoryCustomFolder_label";
            this.spreadHistoryCustomFolder_label.Size = new System.Drawing.Size(144, 26);
            this.spreadHistoryCustomFolder_label.TabIndex = 35;
            this.spreadHistoryCustomFolder_label.Text = "Choose a custom base folder\r\nto export spread data to";
            this.IRTickerTT_generic.SetToolTip(this.spreadHistoryCustomFolder_label, "A folder under this one will be created to store the CSVs in, it will be named yo" +
        "ur current logged in username");
            // 
            // flashForm_checkBox
            // 
            this.flashForm_checkBox.AccessibleName = "";
            this.flashForm_checkBox.AutoSize = true;
            this.flashForm_checkBox.Location = new System.Drawing.Point(467, 372);
            this.flashForm_checkBox.Name = "flashForm_checkBox";
            this.flashForm_checkBox.Size = new System.Drawing.Size(15, 14);
            this.flashForm_checkBox.TabIndex = 22;
            this.flashForm_checkBox.UseVisualStyleBackColor = true;
            this.flashForm_checkBox.CheckedChanged += new System.EventHandler(this.flashForm_checkBox_CheckedChanged);
            // 
            // NegativeSpread_checkBox
            // 
            this.NegativeSpread_checkBox.AccessibleName = "";
            this.NegativeSpread_checkBox.AutoSize = true;
            this.NegativeSpread_checkBox.Checked = true;
            this.NegativeSpread_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NegativeSpread_checkBox.Location = new System.Drawing.Point(467, 468);
            this.NegativeSpread_checkBox.Name = "NegativeSpread_checkBox";
            this.NegativeSpread_checkBox.Size = new System.Drawing.Size(15, 14);
            this.NegativeSpread_checkBox.TabIndex = 34;
            this.NegativeSpread_checkBox.UseVisualStyleBackColor = true;
            this.NegativeSpread_checkBox.CheckedChanged += new System.EventHandler(this.NegativeSpread_checkBox_CheckedChanged);
            // 
            // OB_label
            // 
            this.OB_label.AccessibleName = "";
            this.OB_label.AutoSize = true;
            this.OB_label.Location = new System.Drawing.Point(50, 402);
            this.OB_label.Name = "OB_label";
            this.OB_label.Size = new System.Drawing.Size(372, 26);
            this.OB_label.TabIndex = 24;
            this.OB_label.Text = "Show real time BTC-AUD orderbook for IR?\r\nRequires app restart.  This was mainly " +
    "used for debugging, so it\'s pretty messy";
            // 
            // NegativeSpread_label
            // 
            this.NegativeSpread_label.AccessibleName = "";
            this.NegativeSpread_label.AutoSize = true;
            this.NegativeSpread_label.Location = new System.Drawing.Point(50, 468);
            this.NegativeSpread_label.Name = "NegativeSpread_label";
            this.NegativeSpread_label.Size = new System.Drawing.Size(226, 13);
            this.NegativeSpread_label.TabIndex = 33;
            this.NegativeSpread_label.Text = "Monitor for negative spreads and reset if found";
            // 
            // OB_checkBox
            // 
            this.OB_checkBox.AccessibleName = "";
            this.OB_checkBox.AutoSize = true;
            this.OB_checkBox.Location = new System.Drawing.Point(467, 402);
            this.OB_checkBox.Name = "OB_checkBox";
            this.OB_checkBox.Size = new System.Drawing.Size(15, 14);
            this.OB_checkBox.TabIndex = 25;
            this.OB_checkBox.UseVisualStyleBackColor = true;
            this.OB_checkBox.CheckedChanged += new System.EventHandler(this.OB_checkBox_CheckedChanged);
            // 
            // SlackSettings_groupBox
            // 
            this.SlackSettings_groupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.SlackSettings_groupBox.Controls.Add(this.label4);
            this.SlackSettings_groupBox.Controls.Add(this.SlackNameEmojiCrypto_comboBox);
            this.SlackSettings_groupBox.Controls.Add(this.SlackNameCurrency_label);
            this.SlackSettings_groupBox.Controls.Add(this.SlackNameFiatCurrency_comboBox);
            this.SlackSettings_groupBox.Controls.Add(this.Slack_label);
            this.SlackSettings_groupBox.Controls.Add(this.Slack_checkBox);
            this.SlackSettings_groupBox.Controls.Add(this.slackToken_textBox);
            this.SlackSettings_groupBox.Controls.Add(this.slackDefaultNameLabel);
            this.SlackSettings_groupBox.Controls.Add(this.slackNameChangeLabel);
            this.SlackSettings_groupBox.Controls.Add(this.slackDefaultNameTextBox);
            this.SlackSettings_groupBox.Controls.Add(this.slackNameChangeCheckBox);
            this.SlackSettings_groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SlackSettings_groupBox.Location = new System.Drawing.Point(53, 115);
            this.SlackSettings_groupBox.Name = "SlackSettings_groupBox";
            this.SlackSettings_groupBox.Size = new System.Drawing.Size(461, 242);
            this.SlackSettings_groupBox.TabIndex = 32;
            this.SlackSettings_groupBox.TabStop = false;
            this.SlackSettings_groupBox.Text = "Slack";
            // 
            // label4
            // 
            this.label4.AccessibleName = "";
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label4.Location = new System.Drawing.Point(16, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(203, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Crypto to show in name and use for emoji:";
            // 
            // SlackNameEmojiCrypto_comboBox
            // 
            this.SlackNameEmojiCrypto_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SlackNameEmojiCrypto_comboBox.Enabled = false;
            this.SlackNameEmojiCrypto_comboBox.FormattingEnabled = true;
            this.SlackNameEmojiCrypto_comboBox.Location = new System.Drawing.Point(350, 177);
            this.SlackNameEmojiCrypto_comboBox.Name = "SlackNameEmojiCrypto_comboBox";
            this.SlackNameEmojiCrypto_comboBox.Size = new System.Drawing.Size(94, 24);
            this.SlackNameEmojiCrypto_comboBox.TabIndex = 37;
            this.SlackNameEmojiCrypto_comboBox.SelectedIndexChanged += new System.EventHandler(this.SlackNameEmojiCrypto_comboBox_SelectedIndexChanged);
            // 
            // SlackNameCurrency_label
            // 
            this.SlackNameCurrency_label.AccessibleName = "";
            this.SlackNameCurrency_label.AutoSize = true;
            this.SlackNameCurrency_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.SlackNameCurrency_label.Location = new System.Drawing.Point(16, 211);
            this.SlackNameCurrency_label.Name = "SlackNameCurrency_label";
            this.SlackNameCurrency_label.Size = new System.Drawing.Size(181, 13);
            this.SlackNameCurrency_label.TabIndex = 36;
            this.SlackNameCurrency_label.Text = "Secondary currency for name crypto:";
            // 
            // SlackNameFiatCurrency_comboBox
            // 
            this.SlackNameFiatCurrency_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SlackNameFiatCurrency_comboBox.Enabled = false;
            this.SlackNameFiatCurrency_comboBox.FormattingEnabled = true;
            this.SlackNameFiatCurrency_comboBox.Location = new System.Drawing.Point(350, 206);
            this.SlackNameFiatCurrency_comboBox.Name = "SlackNameFiatCurrency_comboBox";
            this.SlackNameFiatCurrency_comboBox.Size = new System.Drawing.Size(94, 24);
            this.SlackNameFiatCurrency_comboBox.TabIndex = 35;
            this.SlackNameFiatCurrency_comboBox.SelectedIndexChanged += new System.EventHandler(this.SlackNameCurrency_comboBox_SelectedIndexChanged);
            // 
            // Slack_label
            // 
            this.Slack_label.AccessibleName = "";
            this.Slack_label.AutoSize = true;
            this.Slack_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Slack_label.Location = new System.Drawing.Point(16, 31);
            this.Slack_label.Name = "Slack_label";
            this.Slack_label.Size = new System.Drawing.Size(263, 52);
            this.Slack_label.TabIndex = 19;
            this.Slack_label.Text = "Slack integration - will set your profile emoji depending \r\non whether we\'re beat" +
    "ing BTC Markets or not.\r\n\r\nEnter your Slack xoxp token below (required):";
            // 
            // Slack_checkBox
            // 
            this.Slack_checkBox.AccessibleName = "";
            this.Slack_checkBox.AutoSize = true;
            this.Slack_checkBox.Location = new System.Drawing.Point(429, 31);
            this.Slack_checkBox.Name = "Slack_checkBox";
            this.Slack_checkBox.Size = new System.Drawing.Size(15, 14);
            this.Slack_checkBox.TabIndex = 20;
            this.Slack_checkBox.UseVisualStyleBackColor = true;
            this.Slack_checkBox.CheckedChanged += new System.EventHandler(this.Slack_checkBox_CheckedChanged);
            // 
            // slackToken_textBox
            // 
            this.slackToken_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slackToken_textBox.Location = new System.Drawing.Point(19, 90);
            this.slackToken_textBox.Name = "slackToken_textBox";
            this.slackToken_textBox.PasswordChar = '●';
            this.slackToken_textBox.Size = new System.Drawing.Size(425, 20);
            this.slackToken_textBox.TabIndex = 23;
            this.slackToken_textBox.UseSystemPasswordChar = true;
            // 
            // slackDefaultNameLabel
            // 
            this.slackDefaultNameLabel.AccessibleName = "";
            this.slackDefaultNameLabel.AutoSize = true;
            this.slackDefaultNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.slackDefaultNameLabel.Location = new System.Drawing.Point(16, 152);
            this.slackDefaultNameLabel.Name = "slackDefaultNameLabel";
            this.slackDefaultNameLabel.Size = new System.Drawing.Size(210, 13);
            this.slackDefaultNameLabel.TabIndex = 29;
            this.slackDefaultNameLabel.Text = "Enter your normal Slack display name here:";
            this.IRTickerTT_generic.SetToolTip(this.slackDefaultNameLabel, "Please leave this populated even if you decide to disable the name integration so" +
        " the app knows what to set you back to");
            // 
            // slackNameChangeLabel
            // 
            this.slackNameChangeLabel.AccessibleName = "";
            this.slackNameChangeLabel.AutoSize = true;
            this.slackNameChangeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.slackNameChangeLabel.Location = new System.Drawing.Point(16, 128);
            this.slackNameChangeLabel.Name = "slackNameChangeLabel";
            this.slackNameChangeLabel.Size = new System.Drawing.Size(347, 13);
            this.slackNameChangeLabel.TabIndex = 26;
            this.slackNameChangeLabel.Text = "Change Slack username to have below crypto  price midpoint appended";
            // 
            // slackDefaultNameTextBox
            // 
            this.slackDefaultNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slackDefaultNameTextBox.Location = new System.Drawing.Point(269, 149);
            this.slackDefaultNameTextBox.Name = "slackDefaultNameTextBox";
            this.slackDefaultNameTextBox.Size = new System.Drawing.Size(175, 20);
            this.slackDefaultNameTextBox.TabIndex = 28;
            this.IRTickerTT_generic.SetToolTip(this.slackDefaultNameTextBox, "Please leave this populated even if you decide to disable the name integration so" +
        " the app knows what to set you back to");
            // 
            // slackNameChangeCheckBox
            // 
            this.slackNameChangeCheckBox.AccessibleName = "";
            this.slackNameChangeCheckBox.AutoSize = true;
            this.slackNameChangeCheckBox.Location = new System.Drawing.Point(429, 128);
            this.slackNameChangeCheckBox.Name = "slackNameChangeCheckBox";
            this.slackNameChangeCheckBox.Size = new System.Drawing.Size(15, 14);
            this.slackNameChangeCheckBox.TabIndex = 27;
            this.slackNameChangeCheckBox.UseVisualStyleBackColor = true;
            this.slackNameChangeCheckBox.CheckedChanged += new System.EventHandler(this.slackNameChangeCheckBox_CheckedChanged);
            // 
            // UITimerFreq_label
            // 
            this.UITimerFreq_label.AccessibleName = "";
            this.UITimerFreq_label.AutoSize = true;
            this.UITimerFreq_label.Location = new System.Drawing.Point(50, 440);
            this.UITimerFreq_label.Name = "UITimerFreq_label";
            this.UITimerFreq_label.Size = new System.Drawing.Size(252, 13);
            this.UITimerFreq_label.TabIndex = 30;
            this.UITimerFreq_label.Text = "How often to update the UI get for BTC on IR in ms:";
            // 
            // UITimerFreq_maskedTextBox
            // 
            this.UITimerFreq_maskedTextBox.Location = new System.Drawing.Point(382, 437);
            this.UITimerFreq_maskedTextBox.Mask = "0000";
            this.UITimerFreq_maskedTextBox.Name = "UITimerFreq_maskedTextBox";
            this.UITimerFreq_maskedTextBox.PromptChar = ' ';
            this.UITimerFreq_maskedTextBox.Size = new System.Drawing.Size(100, 20);
            this.UITimerFreq_maskedTextBox.TabIndex = 31;
            // 
            // SettingsSeparator_label
            // 
            this.SettingsSeparator_label.AutoSize = true;
            this.SettingsSeparator_label.Location = new System.Drawing.Point(90, 749);
            this.SettingsSeparator_label.Name = "SettingsSeparator_label";
            this.SettingsSeparator_label.Size = new System.Drawing.Size(409, 13);
            this.SettingsSeparator_label.TabIndex = 41;
            this.SettingsSeparator_label.Text = "___________________________________________________________________";
            // 
            // SessionStartedRel_label
            // 
            this.SessionStartedRel_label.AutoSize = true;
            this.SessionStartedRel_label.Location = new System.Drawing.Point(271, 818);
            this.SessionStartedRel_label.Name = "SessionStartedRel_label";
            this.SessionStartedRel_label.Size = new System.Drawing.Size(0, 13);
            this.SessionStartedRel_label.TabIndex = 40;
            // 
            // SessionStartedAbs_label
            // 
            this.SessionStartedAbs_label.AutoSize = true;
            this.SessionStartedAbs_label.Location = new System.Drawing.Point(271, 779);
            this.SessionStartedAbs_label.Name = "SessionStartedAbs_label";
            this.SessionStartedAbs_label.Size = new System.Drawing.Size(0, 13);
            this.SessionStartedAbs_label.TabIndex = 39;
            // 
            // SessionStart_label
            // 
            this.SessionStart_label.AutoSize = true;
            this.SessionStart_label.Location = new System.Drawing.Point(185, 779);
            this.SessionStart_label.Name = "SessionStart_label";
            this.SessionStart_label.Size = new System.Drawing.Size(82, 13);
            this.SessionStart_label.TabIndex = 38;
            this.SessionStart_label.Text = "Session started:";
            // 
            // Help_Button
            // 
            this.Help_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Help_Button.Location = new System.Drawing.Point(439, 774);
            this.Help_Button.Name = "Help_Button";
            this.Help_Button.Size = new System.Drawing.Size(97, 23);
            this.Help_Button.TabIndex = 14;
            this.Help_Button.Text = "Help";
            this.Help_Button.UseVisualStyleBackColor = true;
            this.Help_Button.Click += new System.EventHandler(this.Help_Button_Click);
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(35, 787);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(0, 13);
            this.VersionLabel.TabIndex = 11;
            // 
            // SettingsOKButton
            // 
            this.SettingsOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsOKButton.Location = new System.Drawing.Point(439, 803);
            this.SettingsOKButton.Name = "SettingsOKButton";
            this.SettingsOKButton.Size = new System.Drawing.Size(97, 23);
            this.SettingsOKButton.TabIndex = 4;
            this.SettingsOKButton.Text = "Save and close";
            this.SettingsOKButton.UseVisualStyleBackColor = true;
            this.SettingsOKButton.Click += new System.EventHandler(this.SettingsOKButton_Click);
            // 
            // LoadingPanel
            // 
            this.LoadingPanel.Controls.Add(this.GIFLabel);
            this.LoadingPanel.Location = new System.Drawing.Point(0, 0);
            this.LoadingPanel.Name = "LoadingPanel";
            this.LoadingPanel.Size = new System.Drawing.Size(585, 843);
            this.LoadingPanel.TabIndex = 10;
            // 
            // GIFLabel
            // 
            this.GIFLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(17)))));
            this.GIFLabel.ForeColor = System.Drawing.Color.Red;
            this.GIFLabel.Image = global::IRTicker.Properties.Resources.colour_circle;
            this.GIFLabel.Location = new System.Drawing.Point(0, 0);
            this.GIFLabel.Name = "GIFLabel";
            this.GIFLabel.Size = new System.Drawing.Size(585, 843);
            this.GIFLabel.TabIndex = 0;
            this.GIFLabel.Text = "\r\n\r\n\r\n\r\n\r\n\r\nDownloading bitcoins...";
            this.GIFLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Main
            // 
            this.Main.BackColor = System.Drawing.Color.White;
            this.Main.Controls.Add(this.TradingVenues_comboBox);
            this.Main.Controls.Add(this.IRUSD_GroupBox);
            this.Main.Controls.Add(this.IRSGD_GroupBox);
            this.Main.Controls.Add(this.Balance_button);
            this.Main.Controls.Add(this.cryptoFees_groupBox);
            this.Main.Controls.Add(this.BTCM_GroupBox);
            this.Main.Controls.Add(this.BAR_GroupBox);
            this.Main.Controls.Add(this.SettingsButton);
            this.Main.Controls.Add(this.IR_GroupBox);
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.Size = new System.Drawing.Size(585, 843);
            this.Main.TabIndex = 5;
            this.Main.Visible = false;
            // 
            // TradingVenues_comboBox
            // 
            this.TradingVenues_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TradingVenues_comboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TradingVenues_comboBox.FormattingEnabled = true;
            this.TradingVenues_comboBox.ItemHeight = 16;
            this.TradingVenues_comboBox.Items.AddRange(new object[] {
            "Trade"});
            this.TradingVenues_comboBox.Location = new System.Drawing.Point(411, 810);
            this.TradingVenues_comboBox.Name = "TradingVenues_comboBox";
            this.TradingVenues_comboBox.Size = new System.Drawing.Size(75, 24);
            this.TradingVenues_comboBox.TabIndex = 69;
            this.TradingVenues_comboBox.SelectedIndexChanged += new System.EventHandler(this.TradingVenues_comboBox_SelectedIndexChanged);
            // 
            // IRUSD_GroupBox
            // 
            this.IRUSD_GroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IRUSD_GroupBox.BackgroundImage")));
            this.IRUSD_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.IRUSD_GroupBox.Controls.Add(this.IR_panel_USD);
            this.IRUSD_GroupBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.IRUSD_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.IRUSD_GroupBox.Location = new System.Drawing.Point(306, 305);
            this.IRUSD_GroupBox.Name = "IRUSD_GroupBox";
            this.IRUSD_GroupBox.Size = new System.Drawing.Size(263, 288);
            this.IRUSD_GroupBox.TabIndex = 68;
            this.IRUSD_GroupBox.TabStop = false;
            this.IRUSD_GroupBox.Text = "Independent Reserve";
            // 
            // IR_panel_USD
            // 
            this.IR_panel_USD.AutoScroll = true;
            this.IR_panel_USD.BackColor = System.Drawing.Color.Transparent;
            this.IR_panel_USD.Controls.Add(this.IRUSD_TRUMP_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_TRUMP_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_TRUMP_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_PEPE_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_PEPE_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_PEPE_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_WIF_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_WIF_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_WIF_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_RENDER_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_RENDER_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_RENDER_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_TRX_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_TRX_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_TRX_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SHIB_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SHIB_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SHIB_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SAND_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SAND_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SAND_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SOL_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SOL_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SOL_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MANA_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MANA_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MANA_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MATIC_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MATIC_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MATIC_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DOGE_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DOGE_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DOGE_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ADA_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ADA_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ADA_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_UNI_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_UNI_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_UNI_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_GRT_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_GRT_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_GRT_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DOT_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DOT_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DOT_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_AAVE_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_AAVE_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_AAVE_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_YFI_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_YFI_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_YFI_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SNX_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SNX_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_SNX_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_COMP_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_COMP_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_COMP_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_USDC_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_USDC_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_USDC_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_LINK_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_LINK_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_LINK_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DAI_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DAI_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_DAI_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_USDT_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_USDT_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_USDT_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ETC_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ETC_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ETC_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MKR_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MKR_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_MKR_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_BAT_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_BAT_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_BAT_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XLM_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XLM_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XLM_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_EOS_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_EOS_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_EOS_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ZRX_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ZRX_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XRP_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ZRX_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XRP_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XBT_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ETH_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_BCH_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_LTC_Label2);
            this.IR_panel_USD.Controls.Add(this.IRUSD_LTC_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_BCH_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ETH_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XBT_Label3);
            this.IR_panel_USD.Controls.Add(this.IRUSD_LTC_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_BCH_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_ETH_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XBT_Label1);
            this.IR_panel_USD.Controls.Add(this.IRUSD_XRP_Label3);
            this.IR_panel_USD.Location = new System.Drawing.Point(1, 16);
            this.IR_panel_USD.Name = "IR_panel_USD";
            this.IR_panel_USD.Size = new System.Drawing.Size(262, 272);
            this.IR_panel_USD.TabIndex = 66;
            // 
            // IRUSD_WIF_Label2
            // 
            this.IRUSD_WIF_Label2.AutoSize = true;
            this.IRUSD_WIF_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_WIF_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_WIF_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_WIF_Label2.Location = new System.Drawing.Point(60, 607);
            this.IRUSD_WIF_Label2.Name = "IRUSD_WIF_Label2";
            this.IRUSD_WIF_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_WIF_Label2.TabIndex = 201;
            this.IRUSD_WIF_Label2.Tag = "IR";
            this.IRUSD_WIF_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_WIF_Label1
            // 
            this.IRUSD_WIF_Label1.AutoSize = true;
            this.IRUSD_WIF_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_WIF_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_WIF_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_WIF_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_WIF_Label1.Location = new System.Drawing.Point(6, 607);
            this.IRUSD_WIF_Label1.Name = "IRUSD_WIF_Label1";
            this.IRUSD_WIF_Label1.Size = new System.Drawing.Size(34, 13);
            this.IRUSD_WIF_Label1.TabIndex = 200;
            this.IRUSD_WIF_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_WIF_Label1.Text = "WIF:";
            this.IRUSD_WIF_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_WIF_Label3
            // 
            this.IRUSD_WIF_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_WIF_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_WIF_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_WIF_Label3.Location = new System.Drawing.Point(120, 607);
            this.IRUSD_WIF_Label3.Name = "IRUSD_WIF_Label3";
            this.IRUSD_WIF_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_WIF_Label3.TabIndex = 199;
            this.IRUSD_WIF_Label3.Tag = "";
            this.IRUSD_WIF_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_RENDER_Label2
            // 
            this.IRUSD_RENDER_Label2.AutoSize = true;
            this.IRUSD_RENDER_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_RENDER_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_RENDER_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_RENDER_Label2.Location = new System.Drawing.Point(69, 467);
            this.IRUSD_RENDER_Label2.Name = "IRUSD_RENDER_Label2";
            this.IRUSD_RENDER_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_RENDER_Label2.TabIndex = 198;
            this.IRUSD_RENDER_Label2.Tag = "IR";
            this.IRUSD_RENDER_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_RENDER_Label1
            // 
            this.IRUSD_RENDER_Label1.AutoSize = true;
            this.IRUSD_RENDER_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_RENDER_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_RENDER_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_RENDER_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_RENDER_Label1.Location = new System.Drawing.Point(6, 467);
            this.IRUSD_RENDER_Label1.Name = "IRUSD_RENDER_Label1";
            this.IRUSD_RENDER_Label1.Size = new System.Drawing.Size(63, 13);
            this.IRUSD_RENDER_Label1.TabIndex = 197;
            this.IRUSD_RENDER_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_RENDER_Label1.Text = "RENDER:";
            this.IRUSD_RENDER_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_RENDER_Label3
            // 
            this.IRUSD_RENDER_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_RENDER_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_RENDER_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_RENDER_Label3.Location = new System.Drawing.Point(121, 467);
            this.IRUSD_RENDER_Label3.Name = "IRUSD_RENDER_Label3";
            this.IRUSD_RENDER_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_RENDER_Label3.TabIndex = 196;
            this.IRUSD_RENDER_Label3.Tag = "";
            this.IRUSD_RENDER_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_TRX_Label2
            // 
            this.IRUSD_TRX_Label2.AutoSize = true;
            this.IRUSD_TRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_TRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_TRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_TRX_Label2.Location = new System.Drawing.Point(60, 567);
            this.IRUSD_TRX_Label2.Name = "IRUSD_TRX_Label2";
            this.IRUSD_TRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_TRX_Label2.TabIndex = 195;
            this.IRUSD_TRX_Label2.Tag = "IR";
            this.IRUSD_TRX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_TRX_Label1
            // 
            this.IRUSD_TRX_Label1.AutoSize = true;
            this.IRUSD_TRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_TRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_TRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_TRX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_TRX_Label1.Location = new System.Drawing.Point(6, 567);
            this.IRUSD_TRX_Label1.Name = "IRUSD_TRX_Label1";
            this.IRUSD_TRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_TRX_Label1.TabIndex = 194;
            this.IRUSD_TRX_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_TRX_Label1.Text = "TRX:";
            this.IRUSD_TRX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_TRX_Label3
            // 
            this.IRUSD_TRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_TRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_TRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_TRX_Label3.Location = new System.Drawing.Point(121, 567);
            this.IRUSD_TRX_Label3.Name = "IRUSD_TRX_Label3";
            this.IRUSD_TRX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_TRX_Label3.TabIndex = 193;
            this.IRUSD_TRX_Label3.Tag = "";
            this.IRUSD_TRX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_SHIB_Label2
            // 
            this.IRUSD_SHIB_Label2.AutoSize = true;
            this.IRUSD_SHIB_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SHIB_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SHIB_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SHIB_Label2.Location = new System.Drawing.Point(60, 507);
            this.IRUSD_SHIB_Label2.Name = "IRUSD_SHIB_Label2";
            this.IRUSD_SHIB_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_SHIB_Label2.TabIndex = 192;
            this.IRUSD_SHIB_Label2.Tag = "IR";
            this.IRUSD_SHIB_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SHIB_Label1
            // 
            this.IRUSD_SHIB_Label1.AutoSize = true;
            this.IRUSD_SHIB_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SHIB_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SHIB_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SHIB_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_SHIB_Label1.Location = new System.Drawing.Point(6, 507);
            this.IRUSD_SHIB_Label1.Name = "IRUSD_SHIB_Label1";
            this.IRUSD_SHIB_Label1.Size = new System.Drawing.Size(40, 13);
            this.IRUSD_SHIB_Label1.TabIndex = 191;
            this.IRUSD_SHIB_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_SHIB_Label1.Text = "SHIB:";
            this.IRUSD_SHIB_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SHIB_Label3
            // 
            this.IRUSD_SHIB_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_SHIB_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SHIB_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SHIB_Label3.Location = new System.Drawing.Point(120, 507);
            this.IRUSD_SHIB_Label3.Name = "IRUSD_SHIB_Label3";
            this.IRUSD_SHIB_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_SHIB_Label3.TabIndex = 190;
            this.IRUSD_SHIB_Label3.Tag = "";
            this.IRUSD_SHIB_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_SAND_Label2
            // 
            this.IRUSD_SAND_Label2.AutoSize = true;
            this.IRUSD_SAND_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SAND_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SAND_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SAND_Label2.Location = new System.Drawing.Point(61, 487);
            this.IRUSD_SAND_Label2.Name = "IRUSD_SAND_Label2";
            this.IRUSD_SAND_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_SAND_Label2.TabIndex = 189;
            this.IRUSD_SAND_Label2.Tag = "IR";
            this.IRUSD_SAND_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SAND_Label1
            // 
            this.IRUSD_SAND_Label1.AutoSize = true;
            this.IRUSD_SAND_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SAND_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SAND_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SAND_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_SAND_Label1.Location = new System.Drawing.Point(7, 487);
            this.IRUSD_SAND_Label1.Name = "IRUSD_SAND_Label1";
            this.IRUSD_SAND_Label1.Size = new System.Drawing.Size(45, 13);
            this.IRUSD_SAND_Label1.TabIndex = 188;
            this.IRUSD_SAND_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_SAND_Label1.Text = "SAND:";
            this.IRUSD_SAND_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SAND_Label3
            // 
            this.IRUSD_SAND_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_SAND_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SAND_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SAND_Label3.Location = new System.Drawing.Point(121, 487);
            this.IRUSD_SAND_Label3.Name = "IRUSD_SAND_Label3";
            this.IRUSD_SAND_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_SAND_Label3.TabIndex = 187;
            this.IRUSD_SAND_Label3.Tag = "";
            this.IRUSD_SAND_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_SOL_Label2
            // 
            this.IRUSD_SOL_Label2.AutoSize = true;
            this.IRUSD_SOL_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SOL_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SOL_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SOL_Label2.Location = new System.Drawing.Point(61, 47);
            this.IRUSD_SOL_Label2.Name = "IRUSD_SOL_Label2";
            this.IRUSD_SOL_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_SOL_Label2.TabIndex = 186;
            this.IRUSD_SOL_Label2.Tag = "IR";
            this.IRUSD_SOL_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SOL_Label1
            // 
            this.IRUSD_SOL_Label1.AutoSize = true;
            this.IRUSD_SOL_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SOL_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SOL_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SOL_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_SOL_Label1.Location = new System.Drawing.Point(7, 47);
            this.IRUSD_SOL_Label1.Name = "IRUSD_SOL_Label1";
            this.IRUSD_SOL_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRUSD_SOL_Label1.TabIndex = 185;
            this.IRUSD_SOL_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_SOL_Label1.Text = "SOL:";
            this.IRUSD_SOL_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SOL_Label3
            // 
            this.IRUSD_SOL_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_SOL_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SOL_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SOL_Label3.Location = new System.Drawing.Point(121, 47);
            this.IRUSD_SOL_Label3.Name = "IRUSD_SOL_Label3";
            this.IRUSD_SOL_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_SOL_Label3.TabIndex = 184;
            this.IRUSD_SOL_Label3.Tag = "";
            this.IRUSD_SOL_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_MANA_Label2
            // 
            this.IRUSD_MANA_Label2.AutoSize = true;
            this.IRUSD_MANA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MANA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MANA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_MANA_Label2.Location = new System.Drawing.Point(61, 387);
            this.IRUSD_MANA_Label2.Name = "IRUSD_MANA_Label2";
            this.IRUSD_MANA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_MANA_Label2.TabIndex = 183;
            this.IRUSD_MANA_Label2.Tag = "IR";
            this.IRUSD_MANA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MANA_Label1
            // 
            this.IRUSD_MANA_Label1.AutoSize = true;
            this.IRUSD_MANA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MANA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MANA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_MANA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_MANA_Label1.Location = new System.Drawing.Point(7, 387);
            this.IRUSD_MANA_Label1.Name = "IRUSD_MANA_Label1";
            this.IRUSD_MANA_Label1.Size = new System.Drawing.Size(46, 13);
            this.IRUSD_MANA_Label1.TabIndex = 182;
            this.IRUSD_MANA_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_MANA_Label1.Text = "MANA:";
            this.IRUSD_MANA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MANA_Label3
            // 
            this.IRUSD_MANA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_MANA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MANA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MANA_Label3.Location = new System.Drawing.Point(121, 387);
            this.IRUSD_MANA_Label3.Name = "IRUSD_MANA_Label3";
            this.IRUSD_MANA_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_MANA_Label3.TabIndex = 181;
            this.IRUSD_MANA_Label3.Tag = "";
            this.IRUSD_MANA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_MATIC_Label2
            // 
            this.IRUSD_MATIC_Label2.AutoSize = true;
            this.IRUSD_MATIC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MATIC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MATIC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_MATIC_Label2.Location = new System.Drawing.Point(60, 407);
            this.IRUSD_MATIC_Label2.Name = "IRUSD_MATIC_Label2";
            this.IRUSD_MATIC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_MATIC_Label2.TabIndex = 180;
            this.IRUSD_MATIC_Label2.Tag = "IR";
            this.IRUSD_MATIC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MATIC_Label1
            // 
            this.IRUSD_MATIC_Label1.AutoSize = true;
            this.IRUSD_MATIC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MATIC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MATIC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_MATIC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_MATIC_Label1.Location = new System.Drawing.Point(6, 407);
            this.IRUSD_MATIC_Label1.Name = "IRUSD_MATIC_Label1";
            this.IRUSD_MATIC_Label1.Size = new System.Drawing.Size(49, 13);
            this.IRUSD_MATIC_Label1.TabIndex = 179;
            this.IRUSD_MATIC_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_MATIC_Label1.Text = "MATIC:";
            this.IRUSD_MATIC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MATIC_Label3
            // 
            this.IRUSD_MATIC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_MATIC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MATIC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MATIC_Label3.Location = new System.Drawing.Point(121, 407);
            this.IRUSD_MATIC_Label3.Name = "IRUSD_MATIC_Label3";
            this.IRUSD_MATIC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_MATIC_Label3.TabIndex = 178;
            this.IRUSD_MATIC_Label3.Tag = "";
            this.IRUSD_MATIC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_DOGE_Label2
            // 
            this.IRUSD_DOGE_Label2.AutoSize = true;
            this.IRUSD_DOGE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DOGE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DOGE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_DOGE_Label2.Location = new System.Drawing.Point(60, 247);
            this.IRUSD_DOGE_Label2.Name = "IRUSD_DOGE_Label2";
            this.IRUSD_DOGE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_DOGE_Label2.TabIndex = 177;
            this.IRUSD_DOGE_Label2.Tag = "IR";
            this.IRUSD_DOGE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_DOGE_Label1
            // 
            this.IRUSD_DOGE_Label1.AutoSize = true;
            this.IRUSD_DOGE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DOGE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DOGE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_DOGE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_DOGE_Label1.Location = new System.Drawing.Point(6, 247);
            this.IRUSD_DOGE_Label1.Name = "IRUSD_DOGE_Label1";
            this.IRUSD_DOGE_Label1.Size = new System.Drawing.Size(46, 13);
            this.IRUSD_DOGE_Label1.TabIndex = 176;
            this.IRUSD_DOGE_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_DOGE_Label1.Text = "DOGE:";
            this.IRUSD_DOGE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_DOGE_Label3
            // 
            this.IRUSD_DOGE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_DOGE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DOGE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DOGE_Label3.Location = new System.Drawing.Point(121, 247);
            this.IRUSD_DOGE_Label3.Name = "IRUSD_DOGE_Label3";
            this.IRUSD_DOGE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_DOGE_Label3.TabIndex = 175;
            this.IRUSD_DOGE_Label3.Tag = "";
            this.IRUSD_DOGE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_ADA_Label2
            // 
            this.IRUSD_ADA_Label2.AutoSize = true;
            this.IRUSD_ADA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ADA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ADA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ADA_Label2.Location = new System.Drawing.Point(61, 147);
            this.IRUSD_ADA_Label2.Name = "IRUSD_ADA_Label2";
            this.IRUSD_ADA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_ADA_Label2.TabIndex = 174;
            this.IRUSD_ADA_Label2.Tag = "IR";
            this.IRUSD_ADA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ADA_Label1
            // 
            this.IRUSD_ADA_Label1.AutoSize = true;
            this.IRUSD_ADA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ADA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ADA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ADA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_ADA_Label1.Location = new System.Drawing.Point(7, 147);
            this.IRUSD_ADA_Label1.Name = "IRUSD_ADA_Label1";
            this.IRUSD_ADA_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_ADA_Label1.TabIndex = 173;
            this.IRUSD_ADA_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_ADA_Label1.Text = "ADA:";
            this.IRUSD_ADA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ADA_Label3
            // 
            this.IRUSD_ADA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_ADA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ADA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ADA_Label3.Location = new System.Drawing.Point(121, 147);
            this.IRUSD_ADA_Label3.Name = "IRUSD_ADA_Label3";
            this.IRUSD_ADA_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_ADA_Label3.TabIndex = 172;
            this.IRUSD_ADA_Label3.Tag = "";
            this.IRUSD_ADA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_UNI_Label2
            // 
            this.IRUSD_UNI_Label2.AutoSize = true;
            this.IRUSD_UNI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_UNI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_UNI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_UNI_Label2.Location = new System.Drawing.Point(60, 587);
            this.IRUSD_UNI_Label2.Name = "IRUSD_UNI_Label2";
            this.IRUSD_UNI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_UNI_Label2.TabIndex = 171;
            this.IRUSD_UNI_Label2.Tag = "IR";
            this.IRUSD_UNI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_UNI_Label1
            // 
            this.IRUSD_UNI_Label1.AutoSize = true;
            this.IRUSD_UNI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_UNI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_UNI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_UNI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_UNI_Label1.Location = new System.Drawing.Point(6, 587);
            this.IRUSD_UNI_Label1.Name = "IRUSD_UNI_Label1";
            this.IRUSD_UNI_Label1.Size = new System.Drawing.Size(33, 13);
            this.IRUSD_UNI_Label1.TabIndex = 170;
            this.IRUSD_UNI_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_UNI_Label1.Text = "UNI:";
            this.IRUSD_UNI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_UNI_Label3
            // 
            this.IRUSD_UNI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_UNI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_UNI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_UNI_Label3.Location = new System.Drawing.Point(120, 587);
            this.IRUSD_UNI_Label3.Name = "IRUSD_UNI_Label3";
            this.IRUSD_UNI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_UNI_Label3.TabIndex = 169;
            this.IRUSD_UNI_Label3.Tag = "";
            this.IRUSD_UNI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_GRT_Label2
            // 
            this.IRUSD_GRT_Label2.AutoSize = true;
            this.IRUSD_GRT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_GRT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_GRT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_GRT_Label2.Location = new System.Drawing.Point(61, 327);
            this.IRUSD_GRT_Label2.Name = "IRUSD_GRT_Label2";
            this.IRUSD_GRT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_GRT_Label2.TabIndex = 168;
            this.IRUSD_GRT_Label2.Tag = "IR";
            this.IRUSD_GRT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_GRT_Label1
            // 
            this.IRUSD_GRT_Label1.AutoSize = true;
            this.IRUSD_GRT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_GRT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_GRT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_GRT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_GRT_Label1.Location = new System.Drawing.Point(7, 327);
            this.IRUSD_GRT_Label1.Name = "IRUSD_GRT_Label1";
            this.IRUSD_GRT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IRUSD_GRT_Label1.TabIndex = 167;
            this.IRUSD_GRT_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_GRT_Label1.Text = "GRT:";
            this.IRUSD_GRT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_GRT_Label3
            // 
            this.IRUSD_GRT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_GRT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_GRT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_GRT_Label3.Location = new System.Drawing.Point(121, 327);
            this.IRUSD_GRT_Label3.Name = "IRUSD_GRT_Label3";
            this.IRUSD_GRT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_GRT_Label3.TabIndex = 166;
            this.IRUSD_GRT_Label3.Tag = "";
            this.IRUSD_GRT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_DOT_Label2
            // 
            this.IRUSD_DOT_Label2.AutoSize = true;
            this.IRUSD_DOT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DOT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DOT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_DOT_Label2.Location = new System.Drawing.Point(61, 267);
            this.IRUSD_DOT_Label2.Name = "IRUSD_DOT_Label2";
            this.IRUSD_DOT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_DOT_Label2.TabIndex = 165;
            this.IRUSD_DOT_Label2.Tag = "IR";
            this.IRUSD_DOT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_DOT_Label1
            // 
            this.IRUSD_DOT_Label1.AutoSize = true;
            this.IRUSD_DOT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DOT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DOT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_DOT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_DOT_Label1.Location = new System.Drawing.Point(7, 267);
            this.IRUSD_DOT_Label1.Name = "IRUSD_DOT_Label1";
            this.IRUSD_DOT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IRUSD_DOT_Label1.TabIndex = 164;
            this.IRUSD_DOT_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_DOT_Label1.Text = "DOT:";
            this.IRUSD_DOT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_DOT_Label3
            // 
            this.IRUSD_DOT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_DOT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DOT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DOT_Label3.Location = new System.Drawing.Point(121, 267);
            this.IRUSD_DOT_Label3.Name = "IRUSD_DOT_Label3";
            this.IRUSD_DOT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_DOT_Label3.TabIndex = 163;
            this.IRUSD_DOT_Label3.Tag = "";
            this.IRUSD_DOT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_AAVE_Label2
            // 
            this.IRUSD_AAVE_Label2.AutoSize = true;
            this.IRUSD_AAVE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_AAVE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_AAVE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_AAVE_Label2.Location = new System.Drawing.Point(61, 127);
            this.IRUSD_AAVE_Label2.Name = "IRUSD_AAVE_Label2";
            this.IRUSD_AAVE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_AAVE_Label2.TabIndex = 162;
            this.IRUSD_AAVE_Label2.Tag = "IR";
            this.IRUSD_AAVE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_AAVE_Label1
            // 
            this.IRUSD_AAVE_Label1.AutoSize = true;
            this.IRUSD_AAVE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_AAVE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_AAVE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_AAVE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_AAVE_Label1.Location = new System.Drawing.Point(7, 127);
            this.IRUSD_AAVE_Label1.Name = "IRUSD_AAVE_Label1";
            this.IRUSD_AAVE_Label1.Size = new System.Drawing.Size(43, 13);
            this.IRUSD_AAVE_Label1.TabIndex = 161;
            this.IRUSD_AAVE_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_AAVE_Label1.Text = "AAVE:";
            this.IRUSD_AAVE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_AAVE_Label3
            // 
            this.IRUSD_AAVE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_AAVE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_AAVE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_AAVE_Label3.Location = new System.Drawing.Point(121, 127);
            this.IRUSD_AAVE_Label3.Name = "IRUSD_AAVE_Label3";
            this.IRUSD_AAVE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_AAVE_Label3.TabIndex = 160;
            this.IRUSD_AAVE_Label3.Tag = "";
            this.IRUSD_AAVE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_YFI_Label2
            // 
            this.IRUSD_YFI_Label2.AutoSize = true;
            this.IRUSD_YFI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_YFI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_YFI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_YFI_Label2.Location = new System.Drawing.Point(61, 647);
            this.IRUSD_YFI_Label2.Name = "IRUSD_YFI_Label2";
            this.IRUSD_YFI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_YFI_Label2.TabIndex = 159;
            this.IRUSD_YFI_Label2.Tag = "IR";
            this.IRUSD_YFI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_YFI_Label1
            // 
            this.IRUSD_YFI_Label1.AutoSize = true;
            this.IRUSD_YFI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_YFI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_YFI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_YFI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_YFI_Label1.Location = new System.Drawing.Point(7, 647);
            this.IRUSD_YFI_Label1.Name = "IRUSD_YFI_Label1";
            this.IRUSD_YFI_Label1.Size = new System.Drawing.Size(30, 13);
            this.IRUSD_YFI_Label1.TabIndex = 158;
            this.IRUSD_YFI_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_YFI_Label1.Text = "YFI:";
            this.IRUSD_YFI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_YFI_Label3
            // 
            this.IRUSD_YFI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_YFI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_YFI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_YFI_Label3.Location = new System.Drawing.Point(121, 647);
            this.IRUSD_YFI_Label3.Name = "IRUSD_YFI_Label3";
            this.IRUSD_YFI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_YFI_Label3.TabIndex = 157;
            this.IRUSD_YFI_Label3.Tag = "";
            this.IRUSD_YFI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_SNX_Label2
            // 
            this.IRUSD_SNX_Label2.AutoSize = true;
            this.IRUSD_SNX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SNX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SNX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SNX_Label2.Location = new System.Drawing.Point(60, 527);
            this.IRUSD_SNX_Label2.Name = "IRUSD_SNX_Label2";
            this.IRUSD_SNX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_SNX_Label2.TabIndex = 153;
            this.IRUSD_SNX_Label2.Tag = "IR";
            this.IRUSD_SNX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SNX_Label1
            // 
            this.IRUSD_SNX_Label1.AutoSize = true;
            this.IRUSD_SNX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SNX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SNX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_SNX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_SNX_Label1.Location = new System.Drawing.Point(6, 527);
            this.IRUSD_SNX_Label1.Name = "IRUSD_SNX_Label1";
            this.IRUSD_SNX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_SNX_Label1.TabIndex = 152;
            this.IRUSD_SNX_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_SNX_Label1.Text = "SNX:";
            this.IRUSD_SNX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_SNX_Label3
            // 
            this.IRUSD_SNX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_SNX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_SNX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_SNX_Label3.Location = new System.Drawing.Point(121, 527);
            this.IRUSD_SNX_Label3.Name = "IRUSD_SNX_Label3";
            this.IRUSD_SNX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_SNX_Label3.TabIndex = 151;
            this.IRUSD_SNX_Label3.Tag = "";
            this.IRUSD_SNX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_COMP_Label2
            // 
            this.IRUSD_COMP_Label2.AutoSize = true;
            this.IRUSD_COMP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_COMP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_COMP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_COMP_Label2.Location = new System.Drawing.Point(60, 207);
            this.IRUSD_COMP_Label2.Name = "IRUSD_COMP_Label2";
            this.IRUSD_COMP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_COMP_Label2.TabIndex = 150;
            this.IRUSD_COMP_Label2.Tag = "IR";
            this.IRUSD_COMP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_COMP_Label1
            // 
            this.IRUSD_COMP_Label1.AutoSize = true;
            this.IRUSD_COMP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_COMP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_COMP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_COMP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_COMP_Label1.Location = new System.Drawing.Point(6, 207);
            this.IRUSD_COMP_Label1.Name = "IRUSD_COMP_Label1";
            this.IRUSD_COMP_Label1.Size = new System.Drawing.Size(46, 13);
            this.IRUSD_COMP_Label1.TabIndex = 149;
            this.IRUSD_COMP_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_COMP_Label1.Text = "COMP:";
            this.IRUSD_COMP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_COMP_Label3
            // 
            this.IRUSD_COMP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_COMP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_COMP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_COMP_Label3.Location = new System.Drawing.Point(121, 207);
            this.IRUSD_COMP_Label3.Name = "IRUSD_COMP_Label3";
            this.IRUSD_COMP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_COMP_Label3.TabIndex = 148;
            this.IRUSD_COMP_Label3.Tag = "";
            this.IRUSD_COMP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_USDC_Label2
            // 
            this.IRUSD_USDC_Label2.AutoSize = true;
            this.IRUSD_USDC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_USDC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_USDC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_USDC_Label2.Location = new System.Drawing.Point(60, 87);
            this.IRUSD_USDC_Label2.Name = "IRUSD_USDC_Label2";
            this.IRUSD_USDC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_USDC_Label2.TabIndex = 147;
            this.IRUSD_USDC_Label2.Tag = "IR";
            this.IRUSD_USDC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_USDC_Label1
            // 
            this.IRUSD_USDC_Label1.AutoSize = true;
            this.IRUSD_USDC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_USDC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_USDC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_USDC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_USDC_Label1.Location = new System.Drawing.Point(6, 87);
            this.IRUSD_USDC_Label1.Name = "IRUSD_USDC_Label1";
            this.IRUSD_USDC_Label1.Size = new System.Drawing.Size(45, 13);
            this.IRUSD_USDC_Label1.TabIndex = 146;
            this.IRUSD_USDC_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_USDC_Label1.Text = "USDC:";
            this.IRUSD_USDC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_USDC_Label3
            // 
            this.IRUSD_USDC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_USDC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_USDC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_USDC_Label3.Location = new System.Drawing.Point(121, 87);
            this.IRUSD_USDC_Label3.Name = "IRUSD_USDC_Label3";
            this.IRUSD_USDC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_USDC_Label3.TabIndex = 145;
            this.IRUSD_USDC_Label3.Tag = "";
            this.IRUSD_USDC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_LINK_Label2
            // 
            this.IRUSD_LINK_Label2.AutoSize = true;
            this.IRUSD_LINK_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_LINK_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_LINK_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_LINK_Label2.Location = new System.Drawing.Point(60, 347);
            this.IRUSD_LINK_Label2.Name = "IRUSD_LINK_Label2";
            this.IRUSD_LINK_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_LINK_Label2.TabIndex = 144;
            this.IRUSD_LINK_Label2.Tag = "IR";
            this.IRUSD_LINK_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_LINK_Label1
            // 
            this.IRUSD_LINK_Label1.AutoSize = true;
            this.IRUSD_LINK_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_LINK_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_LINK_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_LINK_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_LINK_Label1.Location = new System.Drawing.Point(6, 347);
            this.IRUSD_LINK_Label1.Name = "IRUSD_LINK_Label1";
            this.IRUSD_LINK_Label1.Size = new System.Drawing.Size(39, 13);
            this.IRUSD_LINK_Label1.TabIndex = 143;
            this.IRUSD_LINK_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_LINK_Label1.Text = "LINK:";
            this.IRUSD_LINK_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_LINK_Label3
            // 
            this.IRUSD_LINK_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_LINK_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_LINK_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_LINK_Label3.Location = new System.Drawing.Point(121, 347);
            this.IRUSD_LINK_Label3.Name = "IRUSD_LINK_Label3";
            this.IRUSD_LINK_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_LINK_Label3.TabIndex = 142;
            this.IRUSD_LINK_Label3.Tag = "";
            this.IRUSD_LINK_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_DAI_Label2
            // 
            this.IRUSD_DAI_Label2.AutoSize = true;
            this.IRUSD_DAI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DAI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DAI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_DAI_Label2.Location = new System.Drawing.Point(60, 227);
            this.IRUSD_DAI_Label2.Name = "IRUSD_DAI_Label2";
            this.IRUSD_DAI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_DAI_Label2.TabIndex = 141;
            this.IRUSD_DAI_Label2.Tag = "IR";
            this.IRUSD_DAI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_DAI_Label1
            // 
            this.IRUSD_DAI_Label1.AutoSize = true;
            this.IRUSD_DAI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DAI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DAI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_DAI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_DAI_Label1.Location = new System.Drawing.Point(6, 227);
            this.IRUSD_DAI_Label1.Name = "IRUSD_DAI_Label1";
            this.IRUSD_DAI_Label1.Size = new System.Drawing.Size(32, 13);
            this.IRUSD_DAI_Label1.TabIndex = 140;
            this.IRUSD_DAI_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_DAI_Label1.Text = "DAI:";
            this.IRUSD_DAI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_DAI_Label3
            // 
            this.IRUSD_DAI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_DAI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_DAI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_DAI_Label3.Location = new System.Drawing.Point(121, 227);
            this.IRUSD_DAI_Label3.Name = "IRUSD_DAI_Label3";
            this.IRUSD_DAI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_DAI_Label3.TabIndex = 139;
            this.IRUSD_DAI_Label3.Tag = "";
            this.IRUSD_DAI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_USDT_Label2
            // 
            this.IRUSD_USDT_Label2.AutoSize = true;
            this.IRUSD_USDT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_USDT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_USDT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_USDT_Label2.Location = new System.Drawing.Point(60, 107);
            this.IRUSD_USDT_Label2.Name = "IRUSD_USDT_Label2";
            this.IRUSD_USDT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_USDT_Label2.TabIndex = 137;
            this.IRUSD_USDT_Label2.Tag = "IR";
            this.IRUSD_USDT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_USDT_Label3
            // 
            this.IRUSD_USDT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_USDT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_USDT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_USDT_Label3.Location = new System.Drawing.Point(121, 107);
            this.IRUSD_USDT_Label3.Name = "IRUSD_USDT_Label3";
            this.IRUSD_USDT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_USDT_Label3.TabIndex = 138;
            this.IRUSD_USDT_Label3.Tag = "";
            this.IRUSD_USDT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_USDT_Label1
            // 
            this.IRUSD_USDT_Label1.AutoSize = true;
            this.IRUSD_USDT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_USDT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_USDT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_USDT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_USDT_Label1.Location = new System.Drawing.Point(6, 107);
            this.IRUSD_USDT_Label1.Name = "IRUSD_USDT_Label1";
            this.IRUSD_USDT_Label1.Size = new System.Drawing.Size(45, 13);
            this.IRUSD_USDT_Label1.TabIndex = 136;
            this.IRUSD_USDT_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_USDT_Label1.Text = "USDT:";
            this.IRUSD_USDT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ETC_Label2
            // 
            this.IRUSD_ETC_Label2.AutoSize = true;
            this.IRUSD_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ETC_Label2.Location = new System.Drawing.Point(60, 307);
            this.IRUSD_ETC_Label2.Name = "IRUSD_ETC_Label2";
            this.IRUSD_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_ETC_Label2.TabIndex = 134;
            this.IRUSD_ETC_Label2.Tag = "IR";
            this.IRUSD_ETC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ETC_Label3
            // 
            this.IRUSD_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ETC_Label3.Location = new System.Drawing.Point(121, 307);
            this.IRUSD_ETC_Label3.Name = "IRUSD_ETC_Label3";
            this.IRUSD_ETC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_ETC_Label3.TabIndex = 135;
            this.IRUSD_ETC_Label3.Tag = "";
            this.IRUSD_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_ETC_Label1
            // 
            this.IRUSD_ETC_Label1.AutoSize = true;
            this.IRUSD_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ETC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_ETC_Label1.Location = new System.Drawing.Point(6, 307);
            this.IRUSD_ETC_Label1.Name = "IRUSD_ETC_Label1";
            this.IRUSD_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRUSD_ETC_Label1.TabIndex = 133;
            this.IRUSD_ETC_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_ETC_Label1.Text = "ETC:";
            this.IRUSD_ETC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MKR_Label2
            // 
            this.IRUSD_MKR_Label2.AutoSize = true;
            this.IRUSD_MKR_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MKR_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MKR_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_MKR_Label2.Location = new System.Drawing.Point(60, 427);
            this.IRUSD_MKR_Label2.Name = "IRUSD_MKR_Label2";
            this.IRUSD_MKR_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_MKR_Label2.TabIndex = 132;
            this.IRUSD_MKR_Label2.Tag = "IR";
            this.IRUSD_MKR_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MKR_Label1
            // 
            this.IRUSD_MKR_Label1.AutoSize = true;
            this.IRUSD_MKR_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MKR_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MKR_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_MKR_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_MKR_Label1.Location = new System.Drawing.Point(6, 427);
            this.IRUSD_MKR_Label1.Name = "IRUSD_MKR_Label1";
            this.IRUSD_MKR_Label1.Size = new System.Drawing.Size(38, 13);
            this.IRUSD_MKR_Label1.TabIndex = 131;
            this.IRUSD_MKR_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_MKR_Label1.Text = "MKR:";
            this.IRUSD_MKR_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_MKR_Label3
            // 
            this.IRUSD_MKR_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_MKR_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_MKR_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_MKR_Label3.Location = new System.Drawing.Point(121, 427);
            this.IRUSD_MKR_Label3.Name = "IRUSD_MKR_Label3";
            this.IRUSD_MKR_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_MKR_Label3.TabIndex = 130;
            this.IRUSD_MKR_Label3.Tag = "";
            this.IRUSD_MKR_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_BAT_Label2
            // 
            this.IRUSD_BAT_Label2.AutoSize = true;
            this.IRUSD_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_BAT_Label2.Location = new System.Drawing.Point(60, 167);
            this.IRUSD_BAT_Label2.Name = "IRUSD_BAT_Label2";
            this.IRUSD_BAT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_BAT_Label2.TabIndex = 129;
            this.IRUSD_BAT_Label2.Tag = "IR";
            this.IRUSD_BAT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_BAT_Label1
            // 
            this.IRUSD_BAT_Label1.AutoSize = true;
            this.IRUSD_BAT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_BAT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_BAT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_BAT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_BAT_Label1.Location = new System.Drawing.Point(6, 167);
            this.IRUSD_BAT_Label1.Name = "IRUSD_BAT_Label1";
            this.IRUSD_BAT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRUSD_BAT_Label1.TabIndex = 128;
            this.IRUSD_BAT_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_BAT_Label1.Text = "BAT:";
            this.IRUSD_BAT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_BAT_Label3
            // 
            this.IRUSD_BAT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_BAT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_BAT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_BAT_Label3.Location = new System.Drawing.Point(121, 167);
            this.IRUSD_BAT_Label3.Name = "IRUSD_BAT_Label3";
            this.IRUSD_BAT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_BAT_Label3.TabIndex = 127;
            this.IRUSD_BAT_Label3.Tag = "";
            this.IRUSD_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_XLM_Label2
            // 
            this.IRUSD_XLM_Label2.AutoSize = true;
            this.IRUSD_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_XLM_Label2.Location = new System.Drawing.Point(60, 627);
            this.IRUSD_XLM_Label2.Name = "IRUSD_XLM_Label2";
            this.IRUSD_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_XLM_Label2.TabIndex = 125;
            this.IRUSD_XLM_Label2.Tag = "IR";
            this.IRUSD_XLM_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_XLM_Label3
            // 
            this.IRUSD_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XLM_Label3.Location = new System.Drawing.Point(121, 627);
            this.IRUSD_XLM_Label3.Name = "IRUSD_XLM_Label3";
            this.IRUSD_XLM_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_XLM_Label3.TabIndex = 126;
            this.IRUSD_XLM_Label3.Tag = "";
            this.IRUSD_XLM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_XLM_Label1
            // 
            this.IRUSD_XLM_Label1.AutoSize = true;
            this.IRUSD_XLM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XLM_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XLM_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_XLM_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_XLM_Label1.Location = new System.Drawing.Point(6, 627);
            this.IRUSD_XLM_Label1.Name = "IRUSD_XLM_Label1";
            this.IRUSD_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_XLM_Label1.TabIndex = 124;
            this.IRUSD_XLM_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_XLM_Label1.Text = "XLM:";
            this.IRUSD_XLM_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_EOS_Label2
            // 
            this.IRUSD_EOS_Label2.AutoSize = true;
            this.IRUSD_EOS_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_EOS_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_EOS_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_EOS_Label2.Location = new System.Drawing.Point(60, 287);
            this.IRUSD_EOS_Label2.Name = "IRUSD_EOS_Label2";
            this.IRUSD_EOS_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_EOS_Label2.TabIndex = 122;
            this.IRUSD_EOS_Label2.Tag = "IR";
            this.IRUSD_EOS_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_EOS_Label3
            // 
            this.IRUSD_EOS_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_EOS_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_EOS_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_EOS_Label3.Location = new System.Drawing.Point(121, 287);
            this.IRUSD_EOS_Label3.Name = "IRUSD_EOS_Label3";
            this.IRUSD_EOS_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_EOS_Label3.TabIndex = 123;
            this.IRUSD_EOS_Label3.Tag = "";
            this.IRUSD_EOS_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_EOS_Label1
            // 
            this.IRUSD_EOS_Label1.AutoSize = true;
            this.IRUSD_EOS_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_EOS_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_EOS_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_EOS_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_EOS_Label1.Location = new System.Drawing.Point(6, 287);
            this.IRUSD_EOS_Label1.Name = "IRUSD_EOS_Label1";
            this.IRUSD_EOS_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_EOS_Label1.TabIndex = 121;
            this.IRUSD_EOS_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_EOS_Label1.Text = "EOS:";
            this.IRUSD_EOS_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ZRX_Label2
            // 
            this.IRUSD_ZRX_Label2.AutoSize = true;
            this.IRUSD_ZRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ZRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ZRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ZRX_Label2.Location = new System.Drawing.Point(60, 667);
            this.IRUSD_ZRX_Label2.Name = "IRUSD_ZRX_Label2";
            this.IRUSD_ZRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_ZRX_Label2.TabIndex = 120;
            this.IRUSD_ZRX_Label2.Tag = "IR";
            this.IRUSD_ZRX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ZRX_Label1
            // 
            this.IRUSD_ZRX_Label1.AutoSize = true;
            this.IRUSD_ZRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ZRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ZRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ZRX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_ZRX_Label1.Location = new System.Drawing.Point(6, 667);
            this.IRUSD_ZRX_Label1.Name = "IRUSD_ZRX_Label1";
            this.IRUSD_ZRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_ZRX_Label1.TabIndex = 119;
            this.IRUSD_ZRX_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_ZRX_Label1.Text = "ZRX:";
            this.IRUSD_ZRX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_XRP_Label2
            // 
            this.IRUSD_XRP_Label2.AutoSize = true;
            this.IRUSD_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_XRP_Label2.Location = new System.Drawing.Point(60, 67);
            this.IRUSD_XRP_Label2.Name = "IRUSD_XRP_Label2";
            this.IRUSD_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_XRP_Label2.TabIndex = 113;
            this.IRUSD_XRP_Label2.Tag = "IR";
            this.IRUSD_XRP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ZRX_Label3
            // 
            this.IRUSD_ZRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_ZRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ZRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ZRX_Label3.Location = new System.Drawing.Point(121, 667);
            this.IRUSD_ZRX_Label3.Name = "IRUSD_ZRX_Label3";
            this.IRUSD_ZRX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_ZRX_Label3.TabIndex = 114;
            this.IRUSD_ZRX_Label3.Tag = "";
            this.IRUSD_ZRX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_XRP_Label1
            // 
            this.IRUSD_XRP_Label1.AutoSize = true;
            this.IRUSD_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_XRP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_XRP_Label1.Location = new System.Drawing.Point(6, 67);
            this.IRUSD_XRP_Label1.Name = "IRUSD_XRP_Label1";
            this.IRUSD_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_XRP_Label1.TabIndex = 112;
            this.IRUSD_XRP_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_XRP_Label1.Text = "XRP:";
            this.IRUSD_XRP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_XBT_Label2
            // 
            this.IRUSD_XBT_Label2.AutoSize = true;
            this.IRUSD_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_XBT_Label2.Location = new System.Drawing.Point(60, 7);
            this.IRUSD_XBT_Label2.Name = "IRUSD_XBT_Label2";
            this.IRUSD_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_XBT_Label2.TabIndex = 104;
            this.IRUSD_XBT_Label2.Tag = "IR";
            this.IRUSD_XBT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ETH_Label2
            // 
            this.IRUSD_ETH_Label2.AutoSize = true;
            this.IRUSD_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ETH_Label2.Location = new System.Drawing.Point(60, 27);
            this.IRUSD_ETH_Label2.Name = "IRUSD_ETH_Label2";
            this.IRUSD_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_ETH_Label2.TabIndex = 105;
            this.IRUSD_ETH_Label2.Tag = "IR";
            this.IRUSD_ETH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_BCH_Label2
            // 
            this.IRUSD_BCH_Label2.AutoSize = true;
            this.IRUSD_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_BCH_Label2.Location = new System.Drawing.Point(60, 187);
            this.IRUSD_BCH_Label2.Name = "IRUSD_BCH_Label2";
            this.IRUSD_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_BCH_Label2.TabIndex = 106;
            this.IRUSD_BCH_Label2.Tag = "IR";
            this.IRUSD_BCH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_LTC_Label2
            // 
            this.IRUSD_LTC_Label2.AutoSize = true;
            this.IRUSD_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_LTC_Label2.Location = new System.Drawing.Point(60, 367);
            this.IRUSD_LTC_Label2.Name = "IRUSD_LTC_Label2";
            this.IRUSD_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_LTC_Label2.TabIndex = 107;
            this.IRUSD_LTC_Label2.Tag = "IR";
            this.IRUSD_LTC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_LTC_Label3
            // 
            this.IRUSD_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_LTC_Label3.Location = new System.Drawing.Point(121, 367);
            this.IRUSD_LTC_Label3.Name = "IRUSD_LTC_Label3";
            this.IRUSD_LTC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_LTC_Label3.TabIndex = 111;
            this.IRUSD_LTC_Label3.Tag = "";
            this.IRUSD_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_BCH_Label3
            // 
            this.IRUSD_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_BCH_Label3.Location = new System.Drawing.Point(121, 187);
            this.IRUSD_BCH_Label3.Name = "IRUSD_BCH_Label3";
            this.IRUSD_BCH_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_BCH_Label3.TabIndex = 110;
            this.IRUSD_BCH_Label3.Tag = "";
            this.IRUSD_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_ETH_Label3
            // 
            this.IRUSD_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ETH_Label3.Location = new System.Drawing.Point(121, 27);
            this.IRUSD_ETH_Label3.Name = "IRUSD_ETH_Label3";
            this.IRUSD_ETH_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_ETH_Label3.TabIndex = 109;
            this.IRUSD_ETH_Label3.Tag = "";
            this.IRUSD_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_XBT_Label3
            // 
            this.IRUSD_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XBT_Label3.Location = new System.Drawing.Point(121, 7);
            this.IRUSD_XBT_Label3.Name = "IRUSD_XBT_Label3";
            this.IRUSD_XBT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_XBT_Label3.TabIndex = 108;
            this.IRUSD_XBT_Label3.Tag = "";
            this.IRUSD_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_LTC_Label1
            // 
            this.IRUSD_LTC_Label1.AutoSize = true;
            this.IRUSD_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_LTC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_LTC_Label1.Location = new System.Drawing.Point(6, 367);
            this.IRUSD_LTC_Label1.Name = "IRUSD_LTC_Label1";
            this.IRUSD_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.IRUSD_LTC_Label1.TabIndex = 103;
            this.IRUSD_LTC_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_LTC_Label1.Text = "LTC:";
            this.IRUSD_LTC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_BCH_Label1
            // 
            this.IRUSD_BCH_Label1.AutoSize = true;
            this.IRUSD_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_BCH_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_BCH_Label1.Location = new System.Drawing.Point(6, 187);
            this.IRUSD_BCH_Label1.Name = "IRUSD_BCH_Label1";
            this.IRUSD_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_BCH_Label1.TabIndex = 102;
            this.IRUSD_BCH_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_BCH_Label1.Text = "BCH:";
            this.IRUSD_BCH_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_ETH_Label1
            // 
            this.IRUSD_ETH_Label1.AutoSize = true;
            this.IRUSD_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_ETH_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_ETH_Label1.Location = new System.Drawing.Point(6, 27);
            this.IRUSD_ETH_Label1.Name = "IRUSD_ETH_Label1";
            this.IRUSD_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRUSD_ETH_Label1.TabIndex = 101;
            this.IRUSD_ETH_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_ETH_Label1.Text = "ETH:";
            this.IRUSD_ETH_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_XBT_Label1
            // 
            this.IRUSD_XBT_Label1.AutoSize = true;
            this.IRUSD_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_XBT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_XBT_Label1.Location = new System.Drawing.Point(6, 7);
            this.IRUSD_XBT_Label1.Name = "IRUSD_XBT_Label1";
            this.IRUSD_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRUSD_XBT_Label1.TabIndex = 100;
            this.IRUSD_XBT_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_XBT_Label1.Text = "BTC:";
            this.IRUSD_XBT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_XRP_Label3
            // 
            this.IRUSD_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_XRP_Label3.Location = new System.Drawing.Point(121, 67);
            this.IRUSD_XRP_Label3.Name = "IRUSD_XRP_Label3";
            this.IRUSD_XRP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_XRP_Label3.TabIndex = 117;
            this.IRUSD_XRP_Label3.Tag = "";
            this.IRUSD_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_GroupBox
            // 
            this.IRSGD_GroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IRSGD_GroupBox.BackgroundImage")));
            this.IRSGD_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.IRSGD_GroupBox.Controls.Add(this.IR_panel_SGD);
            this.IRSGD_GroupBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.IRSGD_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.IRSGD_GroupBox.Location = new System.Drawing.Point(306, 4);
            this.IRSGD_GroupBox.Name = "IRSGD_GroupBox";
            this.IRSGD_GroupBox.Size = new System.Drawing.Size(263, 288);
            this.IRSGD_GroupBox.TabIndex = 67;
            this.IRSGD_GroupBox.TabStop = false;
            this.IRSGD_GroupBox.Text = "Independent Reserve";
            // 
            // IR_panel_SGD
            // 
            this.IR_panel_SGD.AutoScroll = true;
            this.IR_panel_SGD.BackColor = System.Drawing.Color.Transparent;
            this.IR_panel_SGD.Controls.Add(this.IRSGD_PEPE_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_PEPE_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_PEPE_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_WIF_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_WIF_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_WIF_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SHIB_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SHIB_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SHIB_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_TRX_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_TRX_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_TRX_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_RENDER_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_RENDER_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_RENDER_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_RLUSD_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_RLUSD_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_RLUSD_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SAND_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SAND_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SAND_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SOL_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SOL_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_SOL_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_MANA_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_MANA_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_MANA_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_MATIC_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_MATIC_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_MATIC_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_DOGE_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_DOGE_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_DOGE_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ADA_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ADA_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ADA_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_UNI_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_UNI_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_UNI_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_GRT_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_GRT_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_GRT_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_DOT_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_DOT_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_DOT_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_USDC_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_USDC_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_USDC_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_LINK_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_LINK_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_LINK_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_USDT_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_USDT_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_USDT_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ETC_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ETC_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ETC_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XLM_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XLM_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XLM_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XRP_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XRP_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XBT_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ETH_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_BCH_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_LTC_Label2);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_LTC_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_BCH_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ETH_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XBT_Label3);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_LTC_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_BCH_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_ETH_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XBT_Label1);
            this.IR_panel_SGD.Controls.Add(this.IRSGD_XRP_Label3);
            this.IR_panel_SGD.Location = new System.Drawing.Point(1, 16);
            this.IR_panel_SGD.Name = "IR_panel_SGD";
            this.IR_panel_SGD.Size = new System.Drawing.Size(262, 272);
            this.IR_panel_SGD.TabIndex = 66;
            // 
            // IRSGD_WIF_Label2
            // 
            this.IRSGD_WIF_Label2.AutoSize = true;
            this.IRSGD_WIF_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_WIF_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_WIF_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_WIF_Label2.Location = new System.Drawing.Point(60, 467);
            this.IRSGD_WIF_Label2.Name = "IRSGD_WIF_Label2";
            this.IRSGD_WIF_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_WIF_Label2.TabIndex = 204;
            this.IRSGD_WIF_Label2.Tag = "IR";
            this.IRSGD_WIF_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_WIF_Label1
            // 
            this.IRSGD_WIF_Label1.AutoSize = true;
            this.IRSGD_WIF_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_WIF_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_WIF_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_WIF_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_WIF_Label1.Location = new System.Drawing.Point(6, 467);
            this.IRSGD_WIF_Label1.Name = "IRSGD_WIF_Label1";
            this.IRSGD_WIF_Label1.Size = new System.Drawing.Size(34, 13);
            this.IRSGD_WIF_Label1.TabIndex = 203;
            this.IRSGD_WIF_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_WIF_Label1.Text = "WIF:";
            this.IRSGD_WIF_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_WIF_Label3
            // 
            this.IRSGD_WIF_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_WIF_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_WIF_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_WIF_Label3.Location = new System.Drawing.Point(116, 467);
            this.IRSGD_WIF_Label3.Name = "IRSGD_WIF_Label3";
            this.IRSGD_WIF_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_WIF_Label3.TabIndex = 202;
            this.IRSGD_WIF_Label3.Tag = "";
            this.IRSGD_WIF_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_SHIB_Label2
            // 
            this.IRSGD_SHIB_Label2.AutoSize = true;
            this.IRSGD_SHIB_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SHIB_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SHIB_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_SHIB_Label2.Location = new System.Drawing.Point(60, 407);
            this.IRSGD_SHIB_Label2.Name = "IRSGD_SHIB_Label2";
            this.IRSGD_SHIB_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_SHIB_Label2.TabIndex = 201;
            this.IRSGD_SHIB_Label2.Tag = "IR";
            this.IRSGD_SHIB_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SHIB_Label1
            // 
            this.IRSGD_SHIB_Label1.AutoSize = true;
            this.IRSGD_SHIB_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SHIB_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SHIB_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_SHIB_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_SHIB_Label1.Location = new System.Drawing.Point(6, 407);
            this.IRSGD_SHIB_Label1.Name = "IRSGD_SHIB_Label1";
            this.IRSGD_SHIB_Label1.Size = new System.Drawing.Size(40, 13);
            this.IRSGD_SHIB_Label1.TabIndex = 200;
            this.IRSGD_SHIB_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_SHIB_Label1.Text = "SHIB:";
            this.IRSGD_SHIB_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SHIB_Label3
            // 
            this.IRSGD_SHIB_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_SHIB_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SHIB_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SHIB_Label3.Location = new System.Drawing.Point(115, 407);
            this.IRSGD_SHIB_Label3.Name = "IRSGD_SHIB_Label3";
            this.IRSGD_SHIB_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_SHIB_Label3.TabIndex = 199;
            this.IRSGD_SHIB_Label3.Tag = "";
            this.IRSGD_SHIB_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_TRX_Label2
            // 
            this.IRSGD_TRX_Label2.AutoSize = true;
            this.IRSGD_TRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_TRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_TRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_TRX_Label2.Location = new System.Drawing.Point(60, 427);
            this.IRSGD_TRX_Label2.Name = "IRSGD_TRX_Label2";
            this.IRSGD_TRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_TRX_Label2.TabIndex = 198;
            this.IRSGD_TRX_Label2.Tag = "IR";
            this.IRSGD_TRX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_TRX_Label1
            // 
            this.IRSGD_TRX_Label1.AutoSize = true;
            this.IRSGD_TRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_TRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_TRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_TRX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_TRX_Label1.Location = new System.Drawing.Point(6, 427);
            this.IRSGD_TRX_Label1.Name = "IRSGD_TRX_Label1";
            this.IRSGD_TRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRSGD_TRX_Label1.TabIndex = 197;
            this.IRSGD_TRX_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_TRX_Label1.Text = "TRX:";
            this.IRSGD_TRX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_TRX_Label3
            // 
            this.IRSGD_TRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_TRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_TRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_TRX_Label3.Location = new System.Drawing.Point(115, 427);
            this.IRSGD_TRX_Label3.Name = "IRSGD_TRX_Label3";
            this.IRSGD_TRX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_TRX_Label3.TabIndex = 196;
            this.IRSGD_TRX_Label3.Tag = "";
            this.IRSGD_TRX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_RENDER_Label2
            // 
            this.IRSGD_RENDER_Label2.AutoSize = true;
            this.IRSGD_RENDER_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_RENDER_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_RENDER_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_RENDER_Label2.Location = new System.Drawing.Point(70, 347);
            this.IRSGD_RENDER_Label2.Name = "IRSGD_RENDER_Label2";
            this.IRSGD_RENDER_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_RENDER_Label2.TabIndex = 195;
            this.IRSGD_RENDER_Label2.Tag = "IR";
            this.IRSGD_RENDER_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_RENDER_Label1
            // 
            this.IRSGD_RENDER_Label1.AutoSize = true;
            this.IRSGD_RENDER_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_RENDER_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_RENDER_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_RENDER_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_RENDER_Label1.Location = new System.Drawing.Point(6, 347);
            this.IRSGD_RENDER_Label1.Name = "IRSGD_RENDER_Label1";
            this.IRSGD_RENDER_Label1.Size = new System.Drawing.Size(63, 13);
            this.IRSGD_RENDER_Label1.TabIndex = 194;
            this.IRSGD_RENDER_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_RENDER_Label1.Text = "RENDER:";
            this.IRSGD_RENDER_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_RENDER_Label3
            // 
            this.IRSGD_RENDER_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_RENDER_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_RENDER_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_RENDER_Label3.Location = new System.Drawing.Point(115, 347);
            this.IRSGD_RENDER_Label3.Name = "IRSGD_RENDER_Label3";
            this.IRSGD_RENDER_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_RENDER_Label3.TabIndex = 193;
            this.IRSGD_RENDER_Label3.Tag = "";
            this.IRSGD_RENDER_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_RLUSD_Label2
            // 
            this.IRSGD_RLUSD_Label2.AutoSize = true;
            this.IRSGD_RLUSD_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_RLUSD_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_RLUSD_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_RLUSD_Label2.Location = new System.Drawing.Point(60, 367);
            this.IRSGD_RLUSD_Label2.Name = "IRSGD_RLUSD_Label2";
            this.IRSGD_RLUSD_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_RLUSD_Label2.TabIndex = 191;
            this.IRSGD_RLUSD_Label2.Tag = "IR";
            this.IRSGD_RLUSD_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_RLUSD_Label3
            // 
            this.IRSGD_RLUSD_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_RLUSD_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_RLUSD_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_RLUSD_Label3.Location = new System.Drawing.Point(115, 367);
            this.IRSGD_RLUSD_Label3.Name = "IRSGD_RLUSD_Label3";
            this.IRSGD_RLUSD_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_RLUSD_Label3.TabIndex = 192;
            this.IRSGD_RLUSD_Label3.Tag = "";
            this.IRSGD_RLUSD_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_RLUSD_Label1
            // 
            this.IRSGD_RLUSD_Label1.AutoSize = true;
            this.IRSGD_RLUSD_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_RLUSD_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_RLUSD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_RLUSD_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_RLUSD_Label1.Location = new System.Drawing.Point(6, 367);
            this.IRSGD_RLUSD_Label1.Name = "IRSGD_RLUSD_Label1";
            this.IRSGD_RLUSD_Label1.Size = new System.Drawing.Size(53, 13);
            this.IRSGD_RLUSD_Label1.TabIndex = 190;
            this.IRSGD_RLUSD_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_RLUSD_Label1.Text = "RLUSD:";
            this.IRSGD_RLUSD_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SAND_Label2
            // 
            this.IRSGD_SAND_Label2.AutoSize = true;
            this.IRSGD_SAND_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SAND_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SAND_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_SAND_Label2.Location = new System.Drawing.Point(60, 387);
            this.IRSGD_SAND_Label2.Name = "IRSGD_SAND_Label2";
            this.IRSGD_SAND_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_SAND_Label2.TabIndex = 189;
            this.IRSGD_SAND_Label2.Tag = "IR";
            this.IRSGD_SAND_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SAND_Label1
            // 
            this.IRSGD_SAND_Label1.AutoSize = true;
            this.IRSGD_SAND_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SAND_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SAND_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_SAND_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_SAND_Label1.Location = new System.Drawing.Point(6, 387);
            this.IRSGD_SAND_Label1.Name = "IRSGD_SAND_Label1";
            this.IRSGD_SAND_Label1.Size = new System.Drawing.Size(45, 13);
            this.IRSGD_SAND_Label1.TabIndex = 188;
            this.IRSGD_SAND_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_SAND_Label1.Text = "SAND:";
            this.IRSGD_SAND_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SAND_Label3
            // 
            this.IRSGD_SAND_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_SAND_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SAND_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SAND_Label3.Location = new System.Drawing.Point(115, 387);
            this.IRSGD_SAND_Label3.Name = "IRSGD_SAND_Label3";
            this.IRSGD_SAND_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_SAND_Label3.TabIndex = 187;
            this.IRSGD_SAND_Label3.Tag = "";
            this.IRSGD_SAND_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_SOL_Label2
            // 
            this.IRSGD_SOL_Label2.AutoSize = true;
            this.IRSGD_SOL_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SOL_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SOL_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_SOL_Label2.Location = new System.Drawing.Point(60, 47);
            this.IRSGD_SOL_Label2.Name = "IRSGD_SOL_Label2";
            this.IRSGD_SOL_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_SOL_Label2.TabIndex = 186;
            this.IRSGD_SOL_Label2.Tag = "IR";
            this.IRSGD_SOL_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SOL_Label1
            // 
            this.IRSGD_SOL_Label1.AutoSize = true;
            this.IRSGD_SOL_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SOL_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SOL_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_SOL_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_SOL_Label1.Location = new System.Drawing.Point(6, 47);
            this.IRSGD_SOL_Label1.Name = "IRSGD_SOL_Label1";
            this.IRSGD_SOL_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRSGD_SOL_Label1.TabIndex = 185;
            this.IRSGD_SOL_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_SOL_Label1.Text = "SOL:";
            this.IRSGD_SOL_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_SOL_Label3
            // 
            this.IRSGD_SOL_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_SOL_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_SOL_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_SOL_Label3.Location = new System.Drawing.Point(115, 47);
            this.IRSGD_SOL_Label3.Name = "IRSGD_SOL_Label3";
            this.IRSGD_SOL_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_SOL_Label3.TabIndex = 184;
            this.IRSGD_SOL_Label3.Tag = "";
            this.IRSGD_SOL_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_MANA_Label2
            // 
            this.IRSGD_MANA_Label2.AutoSize = true;
            this.IRSGD_MANA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_MANA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_MANA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_MANA_Label2.Location = new System.Drawing.Point(60, 287);
            this.IRSGD_MANA_Label2.Name = "IRSGD_MANA_Label2";
            this.IRSGD_MANA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_MANA_Label2.TabIndex = 183;
            this.IRSGD_MANA_Label2.Tag = "IR";
            this.IRSGD_MANA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_MANA_Label1
            // 
            this.IRSGD_MANA_Label1.AutoSize = true;
            this.IRSGD_MANA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_MANA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_MANA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_MANA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_MANA_Label1.Location = new System.Drawing.Point(6, 287);
            this.IRSGD_MANA_Label1.Name = "IRSGD_MANA_Label1";
            this.IRSGD_MANA_Label1.Size = new System.Drawing.Size(46, 13);
            this.IRSGD_MANA_Label1.TabIndex = 182;
            this.IRSGD_MANA_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_MANA_Label1.Text = "MANA:";
            this.IRSGD_MANA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_MANA_Label3
            // 
            this.IRSGD_MANA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_MANA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_MANA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_MANA_Label3.Location = new System.Drawing.Point(115, 287);
            this.IRSGD_MANA_Label3.Name = "IRSGD_MANA_Label3";
            this.IRSGD_MANA_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_MANA_Label3.TabIndex = 181;
            this.IRSGD_MANA_Label3.Tag = "";
            this.IRSGD_MANA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_MATIC_Label2
            // 
            this.IRSGD_MATIC_Label2.AutoSize = true;
            this.IRSGD_MATIC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_MATIC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_MATIC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_MATIC_Label2.Location = new System.Drawing.Point(60, 307);
            this.IRSGD_MATIC_Label2.Name = "IRSGD_MATIC_Label2";
            this.IRSGD_MATIC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_MATIC_Label2.TabIndex = 180;
            this.IRSGD_MATIC_Label2.Tag = "IR";
            this.IRSGD_MATIC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_MATIC_Label1
            // 
            this.IRSGD_MATIC_Label1.AutoSize = true;
            this.IRSGD_MATIC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_MATIC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_MATIC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_MATIC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_MATIC_Label1.Location = new System.Drawing.Point(6, 307);
            this.IRSGD_MATIC_Label1.Name = "IRSGD_MATIC_Label1";
            this.IRSGD_MATIC_Label1.Size = new System.Drawing.Size(49, 13);
            this.IRSGD_MATIC_Label1.TabIndex = 179;
            this.IRSGD_MATIC_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_MATIC_Label1.Text = "MATIC:";
            this.IRSGD_MATIC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_MATIC_Label3
            // 
            this.IRSGD_MATIC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_MATIC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_MATIC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_MATIC_Label3.Location = new System.Drawing.Point(115, 307);
            this.IRSGD_MATIC_Label3.Name = "IRSGD_MATIC_Label3";
            this.IRSGD_MATIC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_MATIC_Label3.TabIndex = 178;
            this.IRSGD_MATIC_Label3.Tag = "";
            this.IRSGD_MATIC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_DOGE_Label2
            // 
            this.IRSGD_DOGE_Label2.AutoSize = true;
            this.IRSGD_DOGE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_DOGE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_DOGE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_DOGE_Label2.Location = new System.Drawing.Point(60, 167);
            this.IRSGD_DOGE_Label2.Name = "IRSGD_DOGE_Label2";
            this.IRSGD_DOGE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_DOGE_Label2.TabIndex = 177;
            this.IRSGD_DOGE_Label2.Tag = "IR";
            this.IRSGD_DOGE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_DOGE_Label1
            // 
            this.IRSGD_DOGE_Label1.AutoSize = true;
            this.IRSGD_DOGE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_DOGE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_DOGE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_DOGE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_DOGE_Label1.Location = new System.Drawing.Point(6, 167);
            this.IRSGD_DOGE_Label1.Name = "IRSGD_DOGE_Label1";
            this.IRSGD_DOGE_Label1.Size = new System.Drawing.Size(46, 13);
            this.IRSGD_DOGE_Label1.TabIndex = 176;
            this.IRSGD_DOGE_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_DOGE_Label1.Text = "DOGE:";
            this.IRSGD_DOGE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_DOGE_Label3
            // 
            this.IRSGD_DOGE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_DOGE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_DOGE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_DOGE_Label3.Location = new System.Drawing.Point(115, 167);
            this.IRSGD_DOGE_Label3.Name = "IRSGD_DOGE_Label3";
            this.IRSGD_DOGE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_DOGE_Label3.TabIndex = 175;
            this.IRSGD_DOGE_Label3.Tag = "";
            this.IRSGD_DOGE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_ADA_Label2
            // 
            this.IRSGD_ADA_Label2.AutoSize = true;
            this.IRSGD_ADA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ADA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ADA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_ADA_Label2.Location = new System.Drawing.Point(60, 127);
            this.IRSGD_ADA_Label2.Name = "IRSGD_ADA_Label2";
            this.IRSGD_ADA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_ADA_Label2.TabIndex = 174;
            this.IRSGD_ADA_Label2.Tag = "IR";
            this.IRSGD_ADA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_ADA_Label1
            // 
            this.IRSGD_ADA_Label1.AutoSize = true;
            this.IRSGD_ADA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ADA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ADA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_ADA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_ADA_Label1.Location = new System.Drawing.Point(6, 127);
            this.IRSGD_ADA_Label1.Name = "IRSGD_ADA_Label1";
            this.IRSGD_ADA_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRSGD_ADA_Label1.TabIndex = 173;
            this.IRSGD_ADA_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_ADA_Label1.Text = "ADA:";
            this.IRSGD_ADA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_ADA_Label3
            // 
            this.IRSGD_ADA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_ADA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ADA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ADA_Label3.Location = new System.Drawing.Point(115, 127);
            this.IRSGD_ADA_Label3.Name = "IRSGD_ADA_Label3";
            this.IRSGD_ADA_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_ADA_Label3.TabIndex = 172;
            this.IRSGD_ADA_Label3.Tag = "";
            this.IRSGD_ADA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_UNI_Label2
            // 
            this.IRSGD_UNI_Label2.AutoSize = true;
            this.IRSGD_UNI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_UNI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_UNI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_UNI_Label2.Location = new System.Drawing.Point(60, 447);
            this.IRSGD_UNI_Label2.Name = "IRSGD_UNI_Label2";
            this.IRSGD_UNI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_UNI_Label2.TabIndex = 171;
            this.IRSGD_UNI_Label2.Tag = "IR";
            this.IRSGD_UNI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_UNI_Label1
            // 
            this.IRSGD_UNI_Label1.AutoSize = true;
            this.IRSGD_UNI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_UNI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_UNI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_UNI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_UNI_Label1.Location = new System.Drawing.Point(6, 447);
            this.IRSGD_UNI_Label1.Name = "IRSGD_UNI_Label1";
            this.IRSGD_UNI_Label1.Size = new System.Drawing.Size(33, 13);
            this.IRSGD_UNI_Label1.TabIndex = 170;
            this.IRSGD_UNI_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_UNI_Label1.Text = "UNI:";
            this.IRSGD_UNI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_UNI_Label3
            // 
            this.IRSGD_UNI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_UNI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_UNI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_UNI_Label3.Location = new System.Drawing.Point(115, 447);
            this.IRSGD_UNI_Label3.Name = "IRSGD_UNI_Label3";
            this.IRSGD_UNI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_UNI_Label3.TabIndex = 169;
            this.IRSGD_UNI_Label3.Tag = "";
            this.IRSGD_UNI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_GRT_Label2
            // 
            this.IRSGD_GRT_Label2.AutoSize = true;
            this.IRSGD_GRT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_GRT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_GRT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_GRT_Label2.Location = new System.Drawing.Point(60, 227);
            this.IRSGD_GRT_Label2.Name = "IRSGD_GRT_Label2";
            this.IRSGD_GRT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_GRT_Label2.TabIndex = 168;
            this.IRSGD_GRT_Label2.Tag = "IR";
            this.IRSGD_GRT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_GRT_Label1
            // 
            this.IRSGD_GRT_Label1.AutoSize = true;
            this.IRSGD_GRT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_GRT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_GRT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_GRT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_GRT_Label1.Location = new System.Drawing.Point(6, 227);
            this.IRSGD_GRT_Label1.Name = "IRSGD_GRT_Label1";
            this.IRSGD_GRT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IRSGD_GRT_Label1.TabIndex = 167;
            this.IRSGD_GRT_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_GRT_Label1.Text = "GRT:";
            this.IRSGD_GRT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_GRT_Label3
            // 
            this.IRSGD_GRT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_GRT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_GRT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_GRT_Label3.Location = new System.Drawing.Point(115, 227);
            this.IRSGD_GRT_Label3.Name = "IRSGD_GRT_Label3";
            this.IRSGD_GRT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_GRT_Label3.TabIndex = 166;
            this.IRSGD_GRT_Label3.Tag = "";
            this.IRSGD_GRT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_DOT_Label2
            // 
            this.IRSGD_DOT_Label2.AutoSize = true;
            this.IRSGD_DOT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_DOT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_DOT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_DOT_Label2.Location = new System.Drawing.Point(60, 187);
            this.IRSGD_DOT_Label2.Name = "IRSGD_DOT_Label2";
            this.IRSGD_DOT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_DOT_Label2.TabIndex = 165;
            this.IRSGD_DOT_Label2.Tag = "IR";
            this.IRSGD_DOT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_DOT_Label1
            // 
            this.IRSGD_DOT_Label1.AutoSize = true;
            this.IRSGD_DOT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_DOT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_DOT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_DOT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_DOT_Label1.Location = new System.Drawing.Point(6, 187);
            this.IRSGD_DOT_Label1.Name = "IRSGD_DOT_Label1";
            this.IRSGD_DOT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IRSGD_DOT_Label1.TabIndex = 164;
            this.IRSGD_DOT_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_DOT_Label1.Text = "DOT:";
            this.IRSGD_DOT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_DOT_Label3
            // 
            this.IRSGD_DOT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_DOT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_DOT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_DOT_Label3.Location = new System.Drawing.Point(115, 187);
            this.IRSGD_DOT_Label3.Name = "IRSGD_DOT_Label3";
            this.IRSGD_DOT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_DOT_Label3.TabIndex = 163;
            this.IRSGD_DOT_Label3.Tag = "";
            this.IRSGD_DOT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_USDC_Label2
            // 
            this.IRSGD_USDC_Label2.AutoSize = true;
            this.IRSGD_USDC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_USDC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_USDC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_USDC_Label2.Location = new System.Drawing.Point(60, 87);
            this.IRSGD_USDC_Label2.Name = "IRSGD_USDC_Label2";
            this.IRSGD_USDC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_USDC_Label2.TabIndex = 147;
            this.IRSGD_USDC_Label2.Tag = "IR";
            this.IRSGD_USDC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_USDC_Label1
            // 
            this.IRSGD_USDC_Label1.AutoSize = true;
            this.IRSGD_USDC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_USDC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_USDC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_USDC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_USDC_Label1.Location = new System.Drawing.Point(6, 87);
            this.IRSGD_USDC_Label1.Name = "IRSGD_USDC_Label1";
            this.IRSGD_USDC_Label1.Size = new System.Drawing.Size(45, 13);
            this.IRSGD_USDC_Label1.TabIndex = 146;
            this.IRSGD_USDC_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_USDC_Label1.Text = "USDC:";
            this.IRSGD_USDC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_USDC_Label3
            // 
            this.IRSGD_USDC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_USDC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_USDC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_USDC_Label3.Location = new System.Drawing.Point(115, 87);
            this.IRSGD_USDC_Label3.Name = "IRSGD_USDC_Label3";
            this.IRSGD_USDC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_USDC_Label3.TabIndex = 145;
            this.IRSGD_USDC_Label3.Tag = "";
            this.IRSGD_USDC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_LINK_Label2
            // 
            this.IRSGD_LINK_Label2.AutoSize = true;
            this.IRSGD_LINK_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_LINK_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_LINK_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_LINK_Label2.Location = new System.Drawing.Point(60, 247);
            this.IRSGD_LINK_Label2.Name = "IRSGD_LINK_Label2";
            this.IRSGD_LINK_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_LINK_Label2.TabIndex = 144;
            this.IRSGD_LINK_Label2.Tag = "IR";
            this.IRSGD_LINK_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_LINK_Label1
            // 
            this.IRSGD_LINK_Label1.AutoSize = true;
            this.IRSGD_LINK_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_LINK_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_LINK_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_LINK_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_LINK_Label1.Location = new System.Drawing.Point(6, 247);
            this.IRSGD_LINK_Label1.Name = "IRSGD_LINK_Label1";
            this.IRSGD_LINK_Label1.Size = new System.Drawing.Size(39, 13);
            this.IRSGD_LINK_Label1.TabIndex = 143;
            this.IRSGD_LINK_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_LINK_Label1.Text = "LINK:";
            this.IRSGD_LINK_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_LINK_Label3
            // 
            this.IRSGD_LINK_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_LINK_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_LINK_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_LINK_Label3.Location = new System.Drawing.Point(115, 247);
            this.IRSGD_LINK_Label3.Name = "IRSGD_LINK_Label3";
            this.IRSGD_LINK_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_LINK_Label3.TabIndex = 142;
            this.IRSGD_LINK_Label3.Tag = "";
            this.IRSGD_LINK_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_USDT_Label2
            // 
            this.IRSGD_USDT_Label2.AutoSize = true;
            this.IRSGD_USDT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_USDT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_USDT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_USDT_Label2.Location = new System.Drawing.Point(60, 107);
            this.IRSGD_USDT_Label2.Name = "IRSGD_USDT_Label2";
            this.IRSGD_USDT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_USDT_Label2.TabIndex = 137;
            this.IRSGD_USDT_Label2.Tag = "IR";
            this.IRSGD_USDT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_USDT_Label3
            // 
            this.IRSGD_USDT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_USDT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_USDT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_USDT_Label3.Location = new System.Drawing.Point(115, 107);
            this.IRSGD_USDT_Label3.Name = "IRSGD_USDT_Label3";
            this.IRSGD_USDT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_USDT_Label3.TabIndex = 138;
            this.IRSGD_USDT_Label3.Tag = "";
            this.IRSGD_USDT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_USDT_Label1
            // 
            this.IRSGD_USDT_Label1.AutoSize = true;
            this.IRSGD_USDT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_USDT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_USDT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_USDT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_USDT_Label1.Location = new System.Drawing.Point(6, 107);
            this.IRSGD_USDT_Label1.Name = "IRSGD_USDT_Label1";
            this.IRSGD_USDT_Label1.Size = new System.Drawing.Size(45, 13);
            this.IRSGD_USDT_Label1.TabIndex = 136;
            this.IRSGD_USDT_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_USDT_Label1.Text = "USDT:";
            this.IRSGD_USDT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_ETC_Label2
            // 
            this.IRSGD_ETC_Label2.AutoSize = true;
            this.IRSGD_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_ETC_Label2.Location = new System.Drawing.Point(60, 207);
            this.IRSGD_ETC_Label2.Name = "IRSGD_ETC_Label2";
            this.IRSGD_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_ETC_Label2.TabIndex = 134;
            this.IRSGD_ETC_Label2.Tag = "IR";
            this.IRSGD_ETC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_ETC_Label3
            // 
            this.IRSGD_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ETC_Label3.Location = new System.Drawing.Point(115, 207);
            this.IRSGD_ETC_Label3.Name = "IRSGD_ETC_Label3";
            this.IRSGD_ETC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_ETC_Label3.TabIndex = 135;
            this.IRSGD_ETC_Label3.Tag = "";
            this.IRSGD_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_ETC_Label1
            // 
            this.IRSGD_ETC_Label1.AutoSize = true;
            this.IRSGD_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_ETC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_ETC_Label1.Location = new System.Drawing.Point(6, 207);
            this.IRSGD_ETC_Label1.Name = "IRSGD_ETC_Label1";
            this.IRSGD_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRSGD_ETC_Label1.TabIndex = 133;
            this.IRSGD_ETC_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_ETC_Label1.Text = "ETC:";
            this.IRSGD_ETC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XLM_Label2
            // 
            this.IRSGD_XLM_Label2.AutoSize = true;
            this.IRSGD_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_XLM_Label2.Location = new System.Drawing.Point(60, 487);
            this.IRSGD_XLM_Label2.Name = "IRSGD_XLM_Label2";
            this.IRSGD_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_XLM_Label2.TabIndex = 125;
            this.IRSGD_XLM_Label2.Tag = "IR";
            this.IRSGD_XLM_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XLM_Label3
            // 
            this.IRSGD_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XLM_Label3.Location = new System.Drawing.Point(115, 487);
            this.IRSGD_XLM_Label3.Name = "IRSGD_XLM_Label3";
            this.IRSGD_XLM_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_XLM_Label3.TabIndex = 126;
            this.IRSGD_XLM_Label3.Tag = "";
            this.IRSGD_XLM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_XLM_Label1
            // 
            this.IRSGD_XLM_Label1.AutoSize = true;
            this.IRSGD_XLM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XLM_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XLM_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_XLM_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_XLM_Label1.Location = new System.Drawing.Point(6, 487);
            this.IRSGD_XLM_Label1.Name = "IRSGD_XLM_Label1";
            this.IRSGD_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRSGD_XLM_Label1.TabIndex = 124;
            this.IRSGD_XLM_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_XLM_Label1.Text = "XLM:";
            this.IRSGD_XLM_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XRP_Label2
            // 
            this.IRSGD_XRP_Label2.AutoSize = true;
            this.IRSGD_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_XRP_Label2.Location = new System.Drawing.Point(60, 67);
            this.IRSGD_XRP_Label2.Name = "IRSGD_XRP_Label2";
            this.IRSGD_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_XRP_Label2.TabIndex = 113;
            this.IRSGD_XRP_Label2.Tag = "IR";
            this.IRSGD_XRP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XRP_Label1
            // 
            this.IRSGD_XRP_Label1.AutoSize = true;
            this.IRSGD_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_XRP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_XRP_Label1.Location = new System.Drawing.Point(6, 67);
            this.IRSGD_XRP_Label1.Name = "IRSGD_XRP_Label1";
            this.IRSGD_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRSGD_XRP_Label1.TabIndex = 112;
            this.IRSGD_XRP_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_XRP_Label1.Text = "XRP:";
            this.IRSGD_XRP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XBT_Label2
            // 
            this.IRSGD_XBT_Label2.AutoSize = true;
            this.IRSGD_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_XBT_Label2.Location = new System.Drawing.Point(60, 7);
            this.IRSGD_XBT_Label2.Name = "IRSGD_XBT_Label2";
            this.IRSGD_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_XBT_Label2.TabIndex = 104;
            this.IRSGD_XBT_Label2.Tag = "IR";
            this.IRSGD_XBT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_ETH_Label2
            // 
            this.IRSGD_ETH_Label2.AutoSize = true;
            this.IRSGD_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_ETH_Label2.Location = new System.Drawing.Point(60, 27);
            this.IRSGD_ETH_Label2.Name = "IRSGD_ETH_Label2";
            this.IRSGD_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_ETH_Label2.TabIndex = 105;
            this.IRSGD_ETH_Label2.Tag = "IR";
            this.IRSGD_ETH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_BCH_Label2
            // 
            this.IRSGD_BCH_Label2.AutoSize = true;
            this.IRSGD_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_BCH_Label2.Location = new System.Drawing.Point(60, 147);
            this.IRSGD_BCH_Label2.Name = "IRSGD_BCH_Label2";
            this.IRSGD_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_BCH_Label2.TabIndex = 106;
            this.IRSGD_BCH_Label2.Tag = "IR";
            this.IRSGD_BCH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_LTC_Label2
            // 
            this.IRSGD_LTC_Label2.AutoSize = true;
            this.IRSGD_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_LTC_Label2.Location = new System.Drawing.Point(60, 267);
            this.IRSGD_LTC_Label2.Name = "IRSGD_LTC_Label2";
            this.IRSGD_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_LTC_Label2.TabIndex = 107;
            this.IRSGD_LTC_Label2.Tag = "IR";
            this.IRSGD_LTC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_LTC_Label3
            // 
            this.IRSGD_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_LTC_Label3.Location = new System.Drawing.Point(115, 267);
            this.IRSGD_LTC_Label3.Name = "IRSGD_LTC_Label3";
            this.IRSGD_LTC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_LTC_Label3.TabIndex = 111;
            this.IRSGD_LTC_Label3.Tag = "";
            this.IRSGD_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_BCH_Label3
            // 
            this.IRSGD_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_BCH_Label3.Location = new System.Drawing.Point(115, 147);
            this.IRSGD_BCH_Label3.Name = "IRSGD_BCH_Label3";
            this.IRSGD_BCH_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_BCH_Label3.TabIndex = 110;
            this.IRSGD_BCH_Label3.Tag = "";
            this.IRSGD_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_ETH_Label3
            // 
            this.IRSGD_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ETH_Label3.Location = new System.Drawing.Point(115, 27);
            this.IRSGD_ETH_Label3.Name = "IRSGD_ETH_Label3";
            this.IRSGD_ETH_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_ETH_Label3.TabIndex = 109;
            this.IRSGD_ETH_Label3.Tag = "";
            this.IRSGD_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_XBT_Label3
            // 
            this.IRSGD_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XBT_Label3.Location = new System.Drawing.Point(115, 7);
            this.IRSGD_XBT_Label3.Name = "IRSGD_XBT_Label3";
            this.IRSGD_XBT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_XBT_Label3.TabIndex = 108;
            this.IRSGD_XBT_Label3.Tag = "";
            this.IRSGD_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_LTC_Label1
            // 
            this.IRSGD_LTC_Label1.AutoSize = true;
            this.IRSGD_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_LTC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_LTC_Label1.Location = new System.Drawing.Point(6, 267);
            this.IRSGD_LTC_Label1.Name = "IRSGD_LTC_Label1";
            this.IRSGD_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.IRSGD_LTC_Label1.TabIndex = 103;
            this.IRSGD_LTC_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_LTC_Label1.Text = "LTC:";
            this.IRSGD_LTC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_BCH_Label1
            // 
            this.IRSGD_BCH_Label1.AutoSize = true;
            this.IRSGD_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_BCH_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_BCH_Label1.Location = new System.Drawing.Point(6, 147);
            this.IRSGD_BCH_Label1.Name = "IRSGD_BCH_Label1";
            this.IRSGD_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRSGD_BCH_Label1.TabIndex = 102;
            this.IRSGD_BCH_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_BCH_Label1.Text = "BCH:";
            this.IRSGD_BCH_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_ETH_Label1
            // 
            this.IRSGD_ETH_Label1.AutoSize = true;
            this.IRSGD_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_ETH_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_ETH_Label1.Location = new System.Drawing.Point(6, 27);
            this.IRSGD_ETH_Label1.Name = "IRSGD_ETH_Label1";
            this.IRSGD_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IRSGD_ETH_Label1.TabIndex = 101;
            this.IRSGD_ETH_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_ETH_Label1.Text = "ETH:";
            this.IRSGD_ETH_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XBT_Label1
            // 
            this.IRSGD_XBT_Label1.AutoSize = true;
            this.IRSGD_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_XBT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_XBT_Label1.Location = new System.Drawing.Point(6, 7);
            this.IRSGD_XBT_Label1.Name = "IRSGD_XBT_Label1";
            this.IRSGD_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IRSGD_XBT_Label1.TabIndex = 100;
            this.IRSGD_XBT_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_XBT_Label1.Text = "BTC:";
            this.IRSGD_XBT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_XRP_Label3
            // 
            this.IRSGD_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_XRP_Label3.Location = new System.Drawing.Point(115, 67);
            this.IRSGD_XRP_Label3.Name = "IRSGD_XRP_Label3";
            this.IRSGD_XRP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_XRP_Label3.TabIndex = 117;
            this.IRSGD_XRP_Label3.Tag = "";
            this.IRSGD_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Balance_button
            // 
            this.Balance_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Balance_button.Location = new System.Drawing.Point(332, 810);
            this.Balance_button.Name = "Balance_button";
            this.Balance_button.Size = new System.Drawing.Size(75, 23);
            this.Balance_button.TabIndex = 18;
            this.Balance_button.Text = "Balance²";
            this.Balance_button.UseVisualStyleBackColor = true;
            this.Balance_button.Click += new System.EventHandler(this.Balance_button_Click);
            // 
            // cryptoFees_groupBox
            // 
            this.cryptoFees_groupBox.Controls.Add(this.cryptoFees_Panel);
            this.cryptoFees_groupBox.ForeColor = System.Drawing.Color.Gray;
            this.cryptoFees_groupBox.Location = new System.Drawing.Point(306, 697);
            this.cryptoFees_groupBox.Name = "cryptoFees_groupBox";
            this.cryptoFees_groupBox.Size = new System.Drawing.Size(263, 102);
            this.cryptoFees_groupBox.TabIndex = 10;
            this.cryptoFees_groupBox.TabStop = false;
            this.cryptoFees_groupBox.Text = "Crypto fees";
            // 
            // cryptoFees_Panel
            // 
            this.cryptoFees_Panel.BackColor = System.Drawing.Color.Transparent;
            this.cryptoFees_Panel.Controls.Add(this.BTC_LastBlock_Time_label);
            this.cryptoFees_Panel.Controls.Add(this.BTC_LastBlock_Time_value);
            this.cryptoFees_Panel.Controls.Add(this.label2);
            this.cryptoFees_Panel.Controls.Add(this.cryptoFees_LastUpdated_value);
            this.cryptoFees_Panel.Controls.Add(this.label3);
            this.cryptoFees_Panel.Controls.Add(this.label6);
            this.cryptoFees_Panel.Controls.Add(this.cryptoFees_ETH_value);
            this.cryptoFees_Panel.Controls.Add(this.cryptoFees_BTC_value);
            this.cryptoFees_Panel.Location = new System.Drawing.Point(0, 16);
            this.cryptoFees_Panel.Name = "cryptoFees_Panel";
            this.cryptoFees_Panel.Size = new System.Drawing.Size(263, 85);
            this.cryptoFees_Panel.TabIndex = 0;
            // 
            // BTC_LastBlock_Time_label
            // 
            this.BTC_LastBlock_Time_label.AutoSize = true;
            this.BTC_LastBlock_Time_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTC_LastBlock_Time_label.Location = new System.Drawing.Point(154, 8);
            this.BTC_LastBlock_Time_label.Name = "BTC_LastBlock_Time_label";
            this.BTC_LastBlock_Time_label.Size = new System.Drawing.Size(98, 13);
            this.BTC_LastBlock_Time_label.TabIndex = 8;
            this.BTC_LastBlock_Time_label.Text = "Last BTC block:";
            // 
            // BTC_LastBlock_Time_value
            // 
            this.BTC_LastBlock_Time_value.AutoSize = true;
            this.BTC_LastBlock_Time_value.Location = new System.Drawing.Point(182, 29);
            this.BTC_LastBlock_Time_value.Name = "BTC_LastBlock_Time_value";
            this.BTC_LastBlock_Time_value.Size = new System.Drawing.Size(0, 13);
            this.BTC_LastBlock_Time_value.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Last updated:";
            // 
            // cryptoFees_LastUpdated_value
            // 
            this.cryptoFees_LastUpdated_value.AutoSize = true;
            this.cryptoFees_LastUpdated_value.Location = new System.Drawing.Point(82, 66);
            this.cryptoFees_LastUpdated_value.Name = "cryptoFees_LastUpdated_value";
            this.cryptoFees_LastUpdated_value.Size = new System.Drawing.Size(0, 13);
            this.cryptoFees_LastUpdated_value.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "BTC:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "ETH:";
            // 
            // cryptoFees_ETH_value
            // 
            this.cryptoFees_ETH_value.AutoSize = true;
            this.cryptoFees_ETH_value.Location = new System.Drawing.Point(56, 29);
            this.cryptoFees_ETH_value.Name = "cryptoFees_ETH_value";
            this.cryptoFees_ETH_value.Size = new System.Drawing.Size(0, 13);
            this.cryptoFees_ETH_value.TabIndex = 5;
            // 
            // cryptoFees_BTC_value
            // 
            this.cryptoFees_BTC_value.AutoSize = true;
            this.cryptoFees_BTC_value.Location = new System.Drawing.Point(56, 9);
            this.cryptoFees_BTC_value.Name = "cryptoFees_BTC_value";
            this.cryptoFees_BTC_value.Size = new System.Drawing.Size(0, 13);
            this.cryptoFees_BTC_value.TabIndex = 4;
            // 
            // BTCM_GroupBox
            // 
            this.BTCM_GroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BTCM_GroupBox.BackgroundImage")));
            this.BTCM_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BTCM_GroupBox.Controls.Add(this.BTCM_panel);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_CurrencyBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_AvgPrice_Label);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_CryptoComboBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_NumCoinsTextBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BuySellComboBox);
            this.BTCM_GroupBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.BTCM_GroupBox.Location = new System.Drawing.Point(19, 599);
            this.BTCM_GroupBox.Name = "BTCM_GroupBox";
            this.BTCM_GroupBox.Size = new System.Drawing.Size(262, 234);
            this.BTCM_GroupBox.TabIndex = 1;
            this.BTCM_GroupBox.TabStop = false;
            this.BTCM_GroupBox.Text = "BTC Markets";
            // 
            // BTCM_panel
            // 
            this.BTCM_panel.AutoScroll = true;
            this.BTCM_panel.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_panel.Controls.Add(this.BTCM_ADA_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_ADA_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_ADA_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_SOL_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_DOT_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_SOL_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_DOT_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_SOL_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_DOT_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_AAVE_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_AAVE_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_AAVE_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_USDC_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_USDC_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_USDC_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_MANA_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_MANA_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_MANA_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_SAND_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_UNI_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_SAND_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_UNI_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_SAND_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_UNI_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_USDT_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_USDT_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_USDT_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_COMP_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_COMP_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_COMP_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_LINK_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_XBT_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_LINK_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_LTC_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_LINK_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_BCH_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_ETH_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_XRP_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_XBT_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_ETH_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_ETC_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_BCH_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_ETC_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_LTC_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_ETC_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_XRP_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_BAT_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_LTC_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_BAT_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_BCH_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_BAT_Label1);
            this.BTCM_panel.Controls.Add(this.BTCM_ETH_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_XBT_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_XRP_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_XLM_Label2);
            this.BTCM_panel.Controls.Add(this.BTCM_XLM_Label3);
            this.BTCM_panel.Controls.Add(this.BTCM_XLM_Label1);
            this.BTCM_panel.Location = new System.Drawing.Point(1, 19);
            this.BTCM_panel.Name = "BTCM_panel";
            this.BTCM_panel.Size = new System.Drawing.Size(261, 158);
            this.BTCM_panel.TabIndex = 65;
            // 
            // BTCM_ADA_Label2
            // 
            this.BTCM_ADA_Label2.AutoSize = true;
            this.BTCM_ADA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ADA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ADA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ADA_Label2.Location = new System.Drawing.Point(45, 145);
            this.BTCM_ADA_Label2.Name = "BTCM_ADA_Label2";
            this.BTCM_ADA_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_ADA_Label2.TabIndex = 94;
            this.BTCM_ADA_Label2.Tag = "BTCM";
            // 
            // BTCM_ADA_Label3
            // 
            this.BTCM_ADA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_ADA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ADA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ADA_Label3.Location = new System.Drawing.Point(113, 145);
            this.BTCM_ADA_Label3.Name = "BTCM_ADA_Label3";
            this.BTCM_ADA_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_ADA_Label3.TabIndex = 93;
            this.BTCM_ADA_Label3.Tag = "";
            this.BTCM_ADA_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_ADA_Label1
            // 
            this.BTCM_ADA_Label1.AutoSize = true;
            this.BTCM_ADA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ADA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ADA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ADA_Label1.Location = new System.Drawing.Point(3, 145);
            this.BTCM_ADA_Label1.Name = "BTCM_ADA_Label1";
            this.BTCM_ADA_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_ADA_Label1.TabIndex = 92;
            this.BTCM_ADA_Label1.Tag = "DCECryptoLabel";
            this.BTCM_ADA_Label1.Text = "ADA:";
            // 
            // BTCM_SOL_Label1
            // 
            this.BTCM_SOL_Label1.AutoSize = true;
            this.BTCM_SOL_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_SOL_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_SOL_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_SOL_Label1.Location = new System.Drawing.Point(3, 45);
            this.BTCM_SOL_Label1.Name = "BTCM_SOL_Label1";
            this.BTCM_SOL_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_SOL_Label1.TabIndex = 86;
            this.BTCM_SOL_Label1.Tag = "DCECryptoLabel";
            this.BTCM_SOL_Label1.Text = "SOL:";
            // 
            // BTCM_DOT_Label2
            // 
            this.BTCM_DOT_Label2.AutoSize = true;
            this.BTCM_DOT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_DOT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_DOT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_DOT_Label2.Location = new System.Drawing.Point(45, 225);
            this.BTCM_DOT_Label2.Name = "BTCM_DOT_Label2";
            this.BTCM_DOT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_DOT_Label2.TabIndex = 91;
            this.BTCM_DOT_Label2.Tag = "BTCM";
            // 
            // BTCM_SOL_Label3
            // 
            this.BTCM_SOL_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_SOL_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_SOL_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_SOL_Label3.Location = new System.Drawing.Point(113, 45);
            this.BTCM_SOL_Label3.Name = "BTCM_SOL_Label3";
            this.BTCM_SOL_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_SOL_Label3.TabIndex = 88;
            this.BTCM_SOL_Label3.Tag = "";
            this.BTCM_SOL_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_DOT_Label3
            // 
            this.BTCM_DOT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_DOT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_DOT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_DOT_Label3.Location = new System.Drawing.Point(113, 225);
            this.BTCM_DOT_Label3.Name = "BTCM_DOT_Label3";
            this.BTCM_DOT_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_DOT_Label3.TabIndex = 90;
            this.BTCM_DOT_Label3.Tag = "";
            this.BTCM_DOT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_SOL_Label2
            // 
            this.BTCM_SOL_Label2.AutoSize = true;
            this.BTCM_SOL_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_SOL_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_SOL_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_SOL_Label2.Location = new System.Drawing.Point(45, 45);
            this.BTCM_SOL_Label2.Name = "BTCM_SOL_Label2";
            this.BTCM_SOL_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_SOL_Label2.TabIndex = 87;
            this.BTCM_SOL_Label2.Tag = "BTCM";
            // 
            // BTCM_DOT_Label1
            // 
            this.BTCM_DOT_Label1.AutoSize = true;
            this.BTCM_DOT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_DOT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_DOT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_DOT_Label1.Location = new System.Drawing.Point(3, 225);
            this.BTCM_DOT_Label1.Name = "BTCM_DOT_Label1";
            this.BTCM_DOT_Label1.Size = new System.Drawing.Size(37, 13);
            this.BTCM_DOT_Label1.TabIndex = 89;
            this.BTCM_DOT_Label1.Tag = "DCECryptoLabel";
            this.BTCM_DOT_Label1.Text = "DOT:";
            // 
            // BTCM_AAVE_Label2
            // 
            this.BTCM_AAVE_Label2.AutoSize = true;
            this.BTCM_AAVE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_AAVE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_AAVE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_AAVE_Label2.Location = new System.Drawing.Point(45, 125);
            this.BTCM_AAVE_Label2.Name = "BTCM_AAVE_Label2";
            this.BTCM_AAVE_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_AAVE_Label2.TabIndex = 84;
            this.BTCM_AAVE_Label2.Tag = "BTCM";
            // 
            // BTCM_AAVE_Label3
            // 
            this.BTCM_AAVE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_AAVE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_AAVE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_AAVE_Label3.Location = new System.Drawing.Point(113, 125);
            this.BTCM_AAVE_Label3.Name = "BTCM_AAVE_Label3";
            this.BTCM_AAVE_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_AAVE_Label3.TabIndex = 85;
            this.BTCM_AAVE_Label3.Tag = "";
            this.BTCM_AAVE_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_AAVE_Label1
            // 
            this.BTCM_AAVE_Label1.AutoSize = true;
            this.BTCM_AAVE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_AAVE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_AAVE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_AAVE_Label1.Location = new System.Drawing.Point(3, 125);
            this.BTCM_AAVE_Label1.Name = "BTCM_AAVE_Label1";
            this.BTCM_AAVE_Label1.Size = new System.Drawing.Size(43, 13);
            this.BTCM_AAVE_Label1.TabIndex = 83;
            this.BTCM_AAVE_Label1.Tag = "DCECryptoLabel";
            this.BTCM_AAVE_Label1.Text = "AAVE:";
            // 
            // BTCM_USDC_Label2
            // 
            this.BTCM_USDC_Label2.AutoSize = true;
            this.BTCM_USDC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_USDC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_USDC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_USDC_Label2.Location = new System.Drawing.Point(45, 85);
            this.BTCM_USDC_Label2.Name = "BTCM_USDC_Label2";
            this.BTCM_USDC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_USDC_Label2.TabIndex = 81;
            this.BTCM_USDC_Label2.Tag = "BTCM";
            // 
            // BTCM_USDC_Label3
            // 
            this.BTCM_USDC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_USDC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_USDC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_USDC_Label3.Location = new System.Drawing.Point(113, 85);
            this.BTCM_USDC_Label3.Name = "BTCM_USDC_Label3";
            this.BTCM_USDC_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_USDC_Label3.TabIndex = 82;
            this.BTCM_USDC_Label3.Tag = "";
            this.BTCM_USDC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_USDC_Label1
            // 
            this.BTCM_USDC_Label1.AutoSize = true;
            this.BTCM_USDC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_USDC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_USDC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_USDC_Label1.Location = new System.Drawing.Point(3, 85);
            this.BTCM_USDC_Label1.Name = "BTCM_USDC_Label1";
            this.BTCM_USDC_Label1.Size = new System.Drawing.Size(45, 13);
            this.BTCM_USDC_Label1.TabIndex = 80;
            this.BTCM_USDC_Label1.Tag = "DCECryptoLabel";
            this.BTCM_USDC_Label1.Text = "USDC:";
            // 
            // BTCM_MANA_Label2
            // 
            this.BTCM_MANA_Label2.AutoSize = true;
            this.BTCM_MANA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_MANA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_MANA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_MANA_Label2.Location = new System.Drawing.Point(45, 305);
            this.BTCM_MANA_Label2.Name = "BTCM_MANA_Label2";
            this.BTCM_MANA_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_MANA_Label2.TabIndex = 79;
            this.BTCM_MANA_Label2.Tag = "BTCM";
            // 
            // BTCM_MANA_Label3
            // 
            this.BTCM_MANA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_MANA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_MANA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_MANA_Label3.Location = new System.Drawing.Point(113, 305);
            this.BTCM_MANA_Label3.Name = "BTCM_MANA_Label3";
            this.BTCM_MANA_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_MANA_Label3.TabIndex = 78;
            this.BTCM_MANA_Label3.Tag = "";
            this.BTCM_MANA_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_MANA_Label1
            // 
            this.BTCM_MANA_Label1.AutoSize = true;
            this.BTCM_MANA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_MANA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_MANA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_MANA_Label1.Location = new System.Drawing.Point(3, 305);
            this.BTCM_MANA_Label1.Name = "BTCM_MANA_Label1";
            this.BTCM_MANA_Label1.Size = new System.Drawing.Size(46, 13);
            this.BTCM_MANA_Label1.TabIndex = 77;
            this.BTCM_MANA_Label1.Tag = "DCECryptoLabel";
            this.BTCM_MANA_Label1.Text = "MANA:";
            // 
            // BTCM_SAND_Label1
            // 
            this.BTCM_SAND_Label1.AutoSize = true;
            this.BTCM_SAND_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_SAND_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_SAND_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_SAND_Label1.Location = new System.Drawing.Point(3, 325);
            this.BTCM_SAND_Label1.Name = "BTCM_SAND_Label1";
            this.BTCM_SAND_Label1.Size = new System.Drawing.Size(45, 13);
            this.BTCM_SAND_Label1.TabIndex = 71;
            this.BTCM_SAND_Label1.Tag = "DCECryptoLabel";
            this.BTCM_SAND_Label1.Text = "SAND:";
            // 
            // BTCM_UNI_Label2
            // 
            this.BTCM_UNI_Label2.AutoSize = true;
            this.BTCM_UNI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_UNI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_UNI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_UNI_Label2.Location = new System.Drawing.Point(45, 345);
            this.BTCM_UNI_Label2.Name = "BTCM_UNI_Label2";
            this.BTCM_UNI_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_UNI_Label2.TabIndex = 76;
            this.BTCM_UNI_Label2.Tag = "BTCM";
            // 
            // BTCM_SAND_Label3
            // 
            this.BTCM_SAND_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_SAND_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_SAND_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_SAND_Label3.Location = new System.Drawing.Point(113, 325);
            this.BTCM_SAND_Label3.Name = "BTCM_SAND_Label3";
            this.BTCM_SAND_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_SAND_Label3.TabIndex = 73;
            this.BTCM_SAND_Label3.Tag = "";
            this.BTCM_SAND_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_UNI_Label3
            // 
            this.BTCM_UNI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_UNI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_UNI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_UNI_Label3.Location = new System.Drawing.Point(113, 345);
            this.BTCM_UNI_Label3.Name = "BTCM_UNI_Label3";
            this.BTCM_UNI_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_UNI_Label3.TabIndex = 75;
            this.BTCM_UNI_Label3.Tag = "";
            this.BTCM_UNI_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_SAND_Label2
            // 
            this.BTCM_SAND_Label2.AutoSize = true;
            this.BTCM_SAND_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_SAND_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_SAND_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_SAND_Label2.Location = new System.Drawing.Point(45, 325);
            this.BTCM_SAND_Label2.Name = "BTCM_SAND_Label2";
            this.BTCM_SAND_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_SAND_Label2.TabIndex = 72;
            this.BTCM_SAND_Label2.Tag = "BTCM";
            // 
            // BTCM_UNI_Label1
            // 
            this.BTCM_UNI_Label1.AutoSize = true;
            this.BTCM_UNI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_UNI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_UNI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_UNI_Label1.Location = new System.Drawing.Point(3, 345);
            this.BTCM_UNI_Label1.Name = "BTCM_UNI_Label1";
            this.BTCM_UNI_Label1.Size = new System.Drawing.Size(33, 13);
            this.BTCM_UNI_Label1.TabIndex = 74;
            this.BTCM_UNI_Label1.Tag = "DCECryptoLabel";
            this.BTCM_UNI_Label1.Text = "UNI:";
            // 
            // BTCM_USDT_Label1
            // 
            this.BTCM_USDT_Label1.AutoSize = true;
            this.BTCM_USDT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_USDT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_USDT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_USDT_Label1.Location = new System.Drawing.Point(3, 105);
            this.BTCM_USDT_Label1.Name = "BTCM_USDT_Label1";
            this.BTCM_USDT_Label1.Size = new System.Drawing.Size(45, 13);
            this.BTCM_USDT_Label1.TabIndex = 68;
            this.BTCM_USDT_Label1.Tag = "DCECryptoLabel";
            this.BTCM_USDT_Label1.Text = "USDT:";
            // 
            // BTCM_USDT_Label3
            // 
            this.BTCM_USDT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_USDT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_USDT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_USDT_Label3.Location = new System.Drawing.Point(113, 105);
            this.BTCM_USDT_Label3.Name = "BTCM_USDT_Label3";
            this.BTCM_USDT_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_USDT_Label3.TabIndex = 69;
            this.BTCM_USDT_Label3.Tag = "";
            this.BTCM_USDT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_USDT_Label2
            // 
            this.BTCM_USDT_Label2.AutoSize = true;
            this.BTCM_USDT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_USDT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_USDT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_USDT_Label2.Location = new System.Drawing.Point(45, 105);
            this.BTCM_USDT_Label2.Name = "BTCM_USDT_Label2";
            this.BTCM_USDT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_USDT_Label2.TabIndex = 70;
            this.BTCM_USDT_Label2.Tag = "BTCM";
            // 
            // BTCM_COMP_Label2
            // 
            this.BTCM_COMP_Label2.AutoSize = true;
            this.BTCM_COMP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_COMP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_COMP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_COMP_Label2.Location = new System.Drawing.Point(45, 205);
            this.BTCM_COMP_Label2.Name = "BTCM_COMP_Label2";
            this.BTCM_COMP_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_COMP_Label2.TabIndex = 66;
            this.BTCM_COMP_Label2.Tag = "BTCM";
            // 
            // BTCM_COMP_Label3
            // 
            this.BTCM_COMP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_COMP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_COMP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_COMP_Label3.Location = new System.Drawing.Point(113, 205);
            this.BTCM_COMP_Label3.Name = "BTCM_COMP_Label3";
            this.BTCM_COMP_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_COMP_Label3.TabIndex = 67;
            this.BTCM_COMP_Label3.Tag = "";
            this.BTCM_COMP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_COMP_Label1
            // 
            this.BTCM_COMP_Label1.AutoSize = true;
            this.BTCM_COMP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_COMP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_COMP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_COMP_Label1.Location = new System.Drawing.Point(3, 205);
            this.BTCM_COMP_Label1.Name = "BTCM_COMP_Label1";
            this.BTCM_COMP_Label1.Size = new System.Drawing.Size(46, 13);
            this.BTCM_COMP_Label1.TabIndex = 65;
            this.BTCM_COMP_Label1.Tag = "DCECryptoLabel";
            this.BTCM_COMP_Label1.Text = "COMP:";
            // 
            // BTCM_LINK_Label2
            // 
            this.BTCM_LINK_Label2.AutoSize = true;
            this.BTCM_LINK_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LINK_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LINK_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LINK_Label2.Location = new System.Drawing.Point(45, 265);
            this.BTCM_LINK_Label2.Name = "BTCM_LINK_Label2";
            this.BTCM_LINK_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_LINK_Label2.TabIndex = 63;
            this.BTCM_LINK_Label2.Tag = "BTCM";
            // 
            // BTCM_XBT_Label1
            // 
            this.BTCM_XBT_Label1.AutoSize = true;
            this.BTCM_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XBT_Label1.Location = new System.Drawing.Point(3, 5);
            this.BTCM_XBT_Label1.Name = "BTCM_XBT_Label1";
            this.BTCM_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_XBT_Label1.TabIndex = 8;
            this.BTCM_XBT_Label1.Tag = "DCECryptoLabel";
            this.BTCM_XBT_Label1.Text = "BTC:";
            // 
            // BTCM_LINK_Label3
            // 
            this.BTCM_LINK_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_LINK_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LINK_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LINK_Label3.Location = new System.Drawing.Point(113, 265);
            this.BTCM_LINK_Label3.Name = "BTCM_LINK_Label3";
            this.BTCM_LINK_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_LINK_Label3.TabIndex = 64;
            this.BTCM_LINK_Label3.Tag = "";
            this.BTCM_LINK_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_LTC_Label1
            // 
            this.BTCM_LTC_Label1.AutoSize = true;
            this.BTCM_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LTC_Label1.Location = new System.Drawing.Point(3, 285);
            this.BTCM_LTC_Label1.Name = "BTCM_LTC_Label1";
            this.BTCM_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.BTCM_LTC_Label1.TabIndex = 11;
            this.BTCM_LTC_Label1.Tag = "DCECryptoLabel";
            this.BTCM_LTC_Label1.Text = "LTC:";
            // 
            // BTCM_LINK_Label1
            // 
            this.BTCM_LINK_Label1.AutoSize = true;
            this.BTCM_LINK_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LINK_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LINK_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LINK_Label1.Location = new System.Drawing.Point(3, 265);
            this.BTCM_LINK_Label1.Name = "BTCM_LINK_Label1";
            this.BTCM_LINK_Label1.Size = new System.Drawing.Size(39, 13);
            this.BTCM_LINK_Label1.TabIndex = 62;
            this.BTCM_LINK_Label1.Tag = "DCECryptoLabel";
            this.BTCM_LINK_Label1.Text = "LINK:";
            // 
            // BTCM_BCH_Label1
            // 
            this.BTCM_BCH_Label1.AutoSize = true;
            this.BTCM_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BCH_Label1.Location = new System.Drawing.Point(3, 185);
            this.BTCM_BCH_Label1.Name = "BTCM_BCH_Label1";
            this.BTCM_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_BCH_Label1.TabIndex = 10;
            this.BTCM_BCH_Label1.Tag = "DCECryptoLabel";
            this.BTCM_BCH_Label1.Text = "BCH:";
            // 
            // BTCM_ETH_Label1
            // 
            this.BTCM_ETH_Label1.AutoSize = true;
            this.BTCM_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETH_Label1.Location = new System.Drawing.Point(3, 25);
            this.BTCM_ETH_Label1.Name = "BTCM_ETH_Label1";
            this.BTCM_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_ETH_Label1.TabIndex = 9;
            this.BTCM_ETH_Label1.Tag = "DCECryptoLabel";
            this.BTCM_ETH_Label1.Text = "ETH:";
            // 
            // BTCM_XRP_Label1
            // 
            this.BTCM_XRP_Label1.AutoSize = true;
            this.BTCM_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XRP_Label1.Location = new System.Drawing.Point(3, 65);
            this.BTCM_XRP_Label1.Name = "BTCM_XRP_Label1";
            this.BTCM_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_XRP_Label1.TabIndex = 8;
            this.BTCM_XRP_Label1.Tag = "DCECryptoLabel";
            this.BTCM_XRP_Label1.Text = "XRP:";
            // 
            // BTCM_XBT_Label3
            // 
            this.BTCM_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XBT_Label3.Location = new System.Drawing.Point(113, 5);
            this.BTCM_XBT_Label3.Name = "BTCM_XBT_Label3";
            this.BTCM_XBT_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_XBT_Label3.TabIndex = 12;
            this.BTCM_XBT_Label3.Tag = "";
            this.BTCM_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_XBT_Label3_MouseDoubleClick);
            // 
            // BTCM_ETH_Label3
            // 
            this.BTCM_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETH_Label3.Location = new System.Drawing.Point(113, 25);
            this.BTCM_ETH_Label3.Name = "BTCM_ETH_Label3";
            this.BTCM_ETH_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_ETH_Label3.TabIndex = 13;
            this.BTCM_ETH_Label3.Tag = "";
            this.BTCM_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_ETH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_ETH_Label3_MouseDoubleClick);
            // 
            // BTCM_ETC_Label2
            // 
            this.BTCM_ETC_Label2.AutoSize = true;
            this.BTCM_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETC_Label2.Location = new System.Drawing.Point(45, 245);
            this.BTCM_ETC_Label2.Name = "BTCM_ETC_Label2";
            this.BTCM_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_ETC_Label2.TabIndex = 32;
            this.BTCM_ETC_Label2.Tag = "BTCM";
            // 
            // BTCM_BCH_Label3
            // 
            this.BTCM_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BCH_Label3.Location = new System.Drawing.Point(113, 185);
            this.BTCM_BCH_Label3.Name = "BTCM_BCH_Label3";
            this.BTCM_BCH_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_BCH_Label3.TabIndex = 14;
            this.BTCM_BCH_Label3.Tag = "";
            this.BTCM_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_BCH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_BCH_Label3_MouseDoubleClick);
            // 
            // BTCM_ETC_Label3
            // 
            this.BTCM_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETC_Label3.Location = new System.Drawing.Point(113, 245);
            this.BTCM_ETC_Label3.Name = "BTCM_ETC_Label3";
            this.BTCM_ETC_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_ETC_Label3.TabIndex = 31;
            this.BTCM_ETC_Label3.Tag = "";
            this.BTCM_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_LTC_Label3
            // 
            this.BTCM_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LTC_Label3.Location = new System.Drawing.Point(113, 285);
            this.BTCM_LTC_Label3.Name = "BTCM_LTC_Label3";
            this.BTCM_LTC_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_LTC_Label3.TabIndex = 15;
            this.BTCM_LTC_Label3.Tag = "";
            this.BTCM_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_LTC_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_LTC_Label3_MouseDoubleClick);
            // 
            // BTCM_ETC_Label1
            // 
            this.BTCM_ETC_Label1.AutoSize = true;
            this.BTCM_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETC_Label1.Location = new System.Drawing.Point(3, 245);
            this.BTCM_ETC_Label1.Name = "BTCM_ETC_Label1";
            this.BTCM_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_ETC_Label1.TabIndex = 30;
            this.BTCM_ETC_Label1.Tag = "DCECryptoLabel";
            this.BTCM_ETC_Label1.Text = "ETC:";
            // 
            // BTCM_XRP_Label3
            // 
            this.BTCM_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XRP_Label3.Location = new System.Drawing.Point(113, 65);
            this.BTCM_XRP_Label3.Name = "BTCM_XRP_Label3";
            this.BTCM_XRP_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_XRP_Label3.TabIndex = 16;
            this.BTCM_XRP_Label3.Tag = "";
            this.BTCM_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_XRP_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_XRP_Label3_MouseDoubleClick);
            // 
            // BTCM_BAT_Label2
            // 
            this.BTCM_BAT_Label2.AutoSize = true;
            this.BTCM_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BAT_Label2.Location = new System.Drawing.Point(45, 165);
            this.BTCM_BAT_Label2.Name = "BTCM_BAT_Label2";
            this.BTCM_BAT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_BAT_Label2.TabIndex = 29;
            this.BTCM_BAT_Label2.Tag = "BTCM";
            // 
            // BTCM_LTC_Label2
            // 
            this.BTCM_LTC_Label2.AutoSize = true;
            this.BTCM_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LTC_Label2.Location = new System.Drawing.Point(45, 285);
            this.BTCM_LTC_Label2.Name = "BTCM_LTC_Label2";
            this.BTCM_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_LTC_Label2.TabIndex = 15;
            this.BTCM_LTC_Label2.Tag = "BTCM";
            // 
            // BTCM_BAT_Label3
            // 
            this.BTCM_BAT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_BAT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BAT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BAT_Label3.Location = new System.Drawing.Point(113, 165);
            this.BTCM_BAT_Label3.Name = "BTCM_BAT_Label3";
            this.BTCM_BAT_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_BAT_Label3.TabIndex = 28;
            this.BTCM_BAT_Label3.Tag = "";
            this.BTCM_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_BCH_Label2
            // 
            this.BTCM_BCH_Label2.AutoSize = true;
            this.BTCM_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BCH_Label2.Location = new System.Drawing.Point(45, 185);
            this.BTCM_BCH_Label2.Name = "BTCM_BCH_Label2";
            this.BTCM_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_BCH_Label2.TabIndex = 14;
            this.BTCM_BCH_Label2.Tag = "BTCM";
            // 
            // BTCM_BAT_Label1
            // 
            this.BTCM_BAT_Label1.AutoSize = true;
            this.BTCM_BAT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BAT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BAT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BAT_Label1.Location = new System.Drawing.Point(3, 165);
            this.BTCM_BAT_Label1.Name = "BTCM_BAT_Label1";
            this.BTCM_BAT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_BAT_Label1.TabIndex = 27;
            this.BTCM_BAT_Label1.Tag = "DCECryptoLabel";
            this.BTCM_BAT_Label1.Text = "BAT:";
            // 
            // BTCM_ETH_Label2
            // 
            this.BTCM_ETH_Label2.AutoSize = true;
            this.BTCM_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETH_Label2.Location = new System.Drawing.Point(45, 25);
            this.BTCM_ETH_Label2.Name = "BTCM_ETH_Label2";
            this.BTCM_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_ETH_Label2.TabIndex = 13;
            this.BTCM_ETH_Label2.Tag = "BTCM";
            // 
            // BTCM_XBT_Label2
            // 
            this.BTCM_XBT_Label2.AutoSize = true;
            this.BTCM_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XBT_Label2.Location = new System.Drawing.Point(45, 5);
            this.BTCM_XBT_Label2.Name = "BTCM_XBT_Label2";
            this.BTCM_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XBT_Label2.TabIndex = 12;
            this.BTCM_XBT_Label2.Tag = "BTCM";
            // 
            // BTCM_XRP_Label2
            // 
            this.BTCM_XRP_Label2.AutoSize = true;
            this.BTCM_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XRP_Label2.Location = new System.Drawing.Point(45, 65);
            this.BTCM_XRP_Label2.Name = "BTCM_XRP_Label2";
            this.BTCM_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XRP_Label2.TabIndex = 9;
            this.BTCM_XRP_Label2.Tag = "BTCM";
            // 
            // BTCM_XLM_Label2
            // 
            this.BTCM_XLM_Label2.AutoSize = true;
            this.BTCM_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XLM_Label2.Location = new System.Drawing.Point(45, 365);
            this.BTCM_XLM_Label2.Name = "BTCM_XLM_Label2";
            this.BTCM_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XLM_Label2.TabIndex = 23;
            this.BTCM_XLM_Label2.Tag = "BTCM";
            // 
            // BTCM_XLM_Label3
            // 
            this.BTCM_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCM_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XLM_Label3.Location = new System.Drawing.Point(113, 365);
            this.BTCM_XLM_Label3.Name = "BTCM_XLM_Label3";
            this.BTCM_XLM_Label3.Size = new System.Drawing.Size(124, 13);
            this.BTCM_XLM_Label3.TabIndex = 22;
            this.BTCM_XLM_Label3.Tag = "";
            this.BTCM_XLM_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_XLM_Label1
            // 
            this.BTCM_XLM_Label1.AutoSize = true;
            this.BTCM_XLM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XLM_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XLM_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XLM_Label1.Location = new System.Drawing.Point(3, 365);
            this.BTCM_XLM_Label1.Name = "BTCM_XLM_Label1";
            this.BTCM_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_XLM_Label1.TabIndex = 21;
            this.BTCM_XLM_Label1.Tag = "DCECryptoLabel";
            this.BTCM_XLM_Label1.Text = "XLM:";
            // 
            // BTCM_CurrencyBox
            // 
            this.BTCM_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_CurrencyBox.DisplayMember = "fiat";
            this.BTCM_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BTCM_CurrencyBox.FormattingEnabled = true;
            this.BTCM_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.BTCM_CurrencyBox.Location = new System.Drawing.Point(131, 206);
            this.BTCM_CurrencyBox.Name = "BTCM_CurrencyBox";
            this.BTCM_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.BTCM_CurrencyBox.TabIndex = 61;
            // 
            // BTCM_AvgPrice_Label
            // 
            this.BTCM_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.BTCM_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_AvgPrice_Label.Location = new System.Drawing.Point(9, 183);
            this.BTCM_AvgPrice_Label.Name = "BTCM_AvgPrice_Label";
            this.BTCM_AvgPrice_Label.Size = new System.Drawing.Size(244, 16);
            this.BTCM_AvgPrice_Label.TabIndex = 16;
            // 
            // BTCM_CryptoComboBox
            // 
            this.BTCM_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BTCM_CryptoComboBox.Location = new System.Drawing.Point(194, 206);
            this.BTCM_CryptoComboBox.Name = "BTCM_CryptoComboBox";
            this.BTCM_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.BTCM_CryptoComboBox.TabIndex = 17;
            this.BTCM_CryptoComboBox.DropDown += new System.EventHandler(this.BTCM_CryptoComboBox_DropDown);
            this.BTCM_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.BTCM_CryptoComboBox_SelectedIndexChanged);
            // 
            // BTCM_NumCoinsTextBox
            // 
            this.BTCM_NumCoinsTextBox.AsciiOnly = true;
            this.BTCM_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_NumCoinsTextBox.Location = new System.Drawing.Point(58, 206);
            this.BTCM_NumCoinsTextBox.Name = "BTCM_NumCoinsTextBox";
            this.BTCM_NumCoinsTextBox.PromptChar = ' ';
            this.BTCM_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.BTCM_NumCoinsTextBox.TabIndex = 16;
            this.BTCM_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BTCM_NumCoinsTextBox.ValidatingType = typeof(int);
            this.BTCM_NumCoinsTextBox.TextChanged += new System.EventHandler(this.BTCM_NumCoinsTextBox_TextChanged);
            this.BTCM_NumCoinsTextBox.Enter += new System.EventHandler(this.BTCM_NumCoinsTextBox_Enter);
            // 
            // BTCM_BuySellComboBox
            // 
            this.BTCM_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BTCM_BuySellComboBox.FormattingEnabled = true;
            this.BTCM_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.BTCM_BuySellComboBox.Location = new System.Drawing.Point(10, 206);
            this.BTCM_BuySellComboBox.Name = "BTCM_BuySellComboBox";
            this.BTCM_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.BTCM_BuySellComboBox.TabIndex = 15;
            this.BTCM_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.BTCM_BuySellComboBox_SelectedIndexChanged);
            // 
            // BAR_GroupBox
            // 
            this.BAR_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BAR_GroupBox.Controls.Add(this.BAR_CurrencyBox);
            this.BAR_GroupBox.Controls.Add(this.BAR_XBT_Label2);
            this.BAR_GroupBox.Controls.Add(this.BAR_XBT_Label3);
            this.BAR_GroupBox.Controls.Add(this.BAR_AvgPrice_Label);
            this.BAR_GroupBox.Controls.Add(this.BAR_CryptoComboBox);
            this.BAR_GroupBox.Controls.Add(this.BAR_XBT_Label1);
            this.BAR_GroupBox.Controls.Add(this.BAR_NumCoinsTextBox);
            this.BAR_GroupBox.Controls.Add(this.BAR_BuySellComboBox);
            this.BAR_GroupBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.BAR_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.BAR_GroupBox.Location = new System.Drawing.Point(306, 599);
            this.BAR_GroupBox.Name = "BAR_GroupBox";
            this.BAR_GroupBox.Size = new System.Drawing.Size(262, 92);
            this.BAR_GroupBox.TabIndex = 16;
            this.BAR_GroupBox.TabStop = false;
            this.BAR_GroupBox.Text = "Bitaroo";
            // 
            // BAR_CurrencyBox
            // 
            this.BAR_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BAR_CurrencyBox.FormattingEnabled = true;
            this.BAR_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.BAR_CurrencyBox.Location = new System.Drawing.Point(131, 65);
            this.BAR_CurrencyBox.Name = "BAR_CurrencyBox";
            this.BAR_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.BAR_CurrencyBox.TabIndex = 60;
            // 
            // BAR_XBT_Label2
            // 
            this.BAR_XBT_Label2.AutoSize = true;
            this.BAR_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BAR_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BAR_XBT_Label2.Location = new System.Drawing.Point(53, 21);
            this.BAR_XBT_Label2.Name = "BAR_XBT_Label2";
            this.BAR_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BAR_XBT_Label2.TabIndex = 4;
            this.BAR_XBT_Label2.Tag = "BAR";
            // 
            // BAR_XBT_Label3
            // 
            this.BAR_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BAR_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BAR_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_XBT_Label3.Location = new System.Drawing.Point(117, 21);
            this.BAR_XBT_Label3.Name = "BAR_XBT_Label3";
            this.BAR_XBT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BAR_XBT_Label3.TabIndex = 8;
            this.BAR_XBT_Label3.Tag = "";
            this.BAR_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BAR_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CSPT_XBT_Label3_MouseDoubleClick);
            // 
            // BAR_AvgPrice_Label
            // 
            this.BAR_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.BAR_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BAR_AvgPrice_Label.Location = new System.Drawing.Point(9, 43);
            this.BAR_AvgPrice_Label.Name = "BAR_AvgPrice_Label";
            this.BAR_AvgPrice_Label.Size = new System.Drawing.Size(242, 17);
            this.BAR_AvgPrice_Label.TabIndex = 58;
            // 
            // BAR_CryptoComboBox
            // 
            this.BAR_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BAR_CryptoComboBox.Location = new System.Drawing.Point(193, 65);
            this.BAR_CryptoComboBox.Name = "BAR_CryptoComboBox";
            this.BAR_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.BAR_CryptoComboBox.TabIndex = 57;
            this.BAR_CryptoComboBox.DropDown += new System.EventHandler(this.BAR_CryptoComboBox_DropDown);
            this.BAR_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.BAR_CryptoComboBox_SelectedIndexChanged);
            // 
            // BAR_XBT_Label1
            // 
            this.BAR_XBT_Label1.AutoSize = true;
            this.BAR_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BAR_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BAR_XBT_Label1.Location = new System.Drawing.Point(6, 21);
            this.BAR_XBT_Label1.Name = "BAR_XBT_Label1";
            this.BAR_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BAR_XBT_Label1.TabIndex = 0;
            this.BAR_XBT_Label1.Tag = "DCECryptoLabel";
            this.BAR_XBT_Label1.Text = "BTC:";
            // 
            // BAR_NumCoinsTextBox
            // 
            this.BAR_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_NumCoinsTextBox.Location = new System.Drawing.Point(58, 65);
            this.BAR_NumCoinsTextBox.Name = "BAR_NumCoinsTextBox";
            this.BAR_NumCoinsTextBox.PromptChar = ' ';
            this.BAR_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.BAR_NumCoinsTextBox.TabIndex = 56;
            this.BAR_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BAR_NumCoinsTextBox.ValidatingType = typeof(System.DateTime);
            this.BAR_NumCoinsTextBox.TextChanged += new System.EventHandler(this.BAR_NumCoinsTextBox_TextChanged);
            // 
            // BAR_BuySellComboBox
            // 
            this.BAR_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BAR_BuySellComboBox.FormattingEnabled = true;
            this.BAR_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.BAR_BuySellComboBox.Location = new System.Drawing.Point(9, 65);
            this.BAR_BuySellComboBox.Name = "BAR_BuySellComboBox";
            this.BAR_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.BAR_BuySellComboBox.TabIndex = 55;
            this.BAR_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.BAR_BuySellComboBox_SelectedIndexChanged);
            // 
            // SettingsButton
            // 
            this.SettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsButton.Location = new System.Drawing.Point(492, 810);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(75, 23);
            this.SettingsButton.TabIndex = 2;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // IR_GroupBox
            // 
            this.IR_GroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IR_GroupBox.BackgroundImage")));
            this.IR_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.IR_GroupBox.Controls.Add(this.IR_panel);
            this.IR_GroupBox.Controls.Add(this.IR_CurrencyBox);
            this.IR_GroupBox.Controls.Add(this.IR_Reset_Button);
            this.IR_GroupBox.Controls.Add(this.SpreadVolumeTitle_Label);
            this.IR_GroupBox.Controls.Add(this.IR_AvgPrice_Label);
            this.IR_GroupBox.Controls.Add(this.IR_CryptoComboBox);
            this.IR_GroupBox.Controls.Add(this.IR_NumCoinsTextBox);
            this.IR_GroupBox.Controls.Add(this.IR_BuySellComboBox);
            this.IR_GroupBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IR_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.IR_GroupBox.Location = new System.Drawing.Point(19, 4);
            this.IR_GroupBox.Name = "IR_GroupBox";
            this.IR_GroupBox.Size = new System.Drawing.Size(263, 589);
            this.IR_GroupBox.TabIndex = 0;
            this.IR_GroupBox.TabStop = false;
            this.IR_GroupBox.Text = "Independent Reserve";
            this.IR_GroupBox.Click += new System.EventHandler(this.IR_GroupBox_Click);
            // 
            // IR_panel
            // 
            this.IR_panel.AutoScroll = true;
            this.IR_panel.BackColor = System.Drawing.Color.Transparent;
            this.IR_panel.Controls.Add(this.IR_TRUMP_Label2);
            this.IR_panel.Controls.Add(this.IR_TRUMP_Label1);
            this.IR_panel.Controls.Add(this.IR_TRUMP_Label3);
            this.IR_panel.Controls.Add(this.IR_PEPE_Label2);
            this.IR_panel.Controls.Add(this.IR_PEPE_Label1);
            this.IR_panel.Controls.Add(this.IR_PEPE_Label3);
            this.IR_panel.Controls.Add(this.IR_WIF_Label2);
            this.IR_panel.Controls.Add(this.IR_WIF_Label1);
            this.IR_panel.Controls.Add(this.IR_WIF_Label3);
            this.IR_panel.Controls.Add(this.IR_RENDER_Label2);
            this.IR_panel.Controls.Add(this.IR_RENDER_Label1);
            this.IR_panel.Controls.Add(this.IR_RENDER_Label3);
            this.IR_panel.Controls.Add(this.IR_TRX_Label2);
            this.IR_panel.Controls.Add(this.IR_TRX_Label1);
            this.IR_panel.Controls.Add(this.IR_TRX_Label3);
            this.IR_panel.Controls.Add(this.IR_SHIB_Label2);
            this.IR_panel.Controls.Add(this.IR_SHIB_Label1);
            this.IR_panel.Controls.Add(this.IR_SHIB_Label3);
            this.IR_panel.Controls.Add(this.IR_SAND_Label2);
            this.IR_panel.Controls.Add(this.IR_SAND_Label1);
            this.IR_panel.Controls.Add(this.IR_SAND_Label3);
            this.IR_panel.Controls.Add(this.IR_SOL_Label2);
            this.IR_panel.Controls.Add(this.IR_SOL_Label1);
            this.IR_panel.Controls.Add(this.IR_SOL_Label3);
            this.IR_panel.Controls.Add(this.IR_MANA_Label2);
            this.IR_panel.Controls.Add(this.IR_MANA_Label1);
            this.IR_panel.Controls.Add(this.IR_MANA_Label3);
            this.IR_panel.Controls.Add(this.IR_MATIC_Label2);
            this.IR_panel.Controls.Add(this.IR_MATIC_Label1);
            this.IR_panel.Controls.Add(this.IR_MATIC_Label3);
            this.IR_panel.Controls.Add(this.IR_DOGE_Label2);
            this.IR_panel.Controls.Add(this.IR_DOGE_Label1);
            this.IR_panel.Controls.Add(this.IR_DOGE_Label3);
            this.IR_panel.Controls.Add(this.IR_ADA_Label2);
            this.IR_panel.Controls.Add(this.IR_ADA_Label1);
            this.IR_panel.Controls.Add(this.IR_ADA_Label3);
            this.IR_panel.Controls.Add(this.IR_UNI_Label2);
            this.IR_panel.Controls.Add(this.IR_UNI_Label1);
            this.IR_panel.Controls.Add(this.IR_UNI_Label3);
            this.IR_panel.Controls.Add(this.IR_GRT_Label2);
            this.IR_panel.Controls.Add(this.IR_GRT_Label1);
            this.IR_panel.Controls.Add(this.IR_GRT_Label3);
            this.IR_panel.Controls.Add(this.IR_DOT_Label2);
            this.IR_panel.Controls.Add(this.IR_DOT_Label1);
            this.IR_panel.Controls.Add(this.IR_DOT_Label3);
            this.IR_panel.Controls.Add(this.IR_AAVE_Label2);
            this.IR_panel.Controls.Add(this.IR_AAVE_Label1);
            this.IR_panel.Controls.Add(this.IR_AAVE_Label3);
            this.IR_panel.Controls.Add(this.IR_YFI_Label2);
            this.IR_panel.Controls.Add(this.IR_YFI_Label1);
            this.IR_panel.Controls.Add(this.IR_YFI_Label3);
            this.IR_panel.Controls.Add(this.IR_SNX_Label2);
            this.IR_panel.Controls.Add(this.IR_SNX_Label1);
            this.IR_panel.Controls.Add(this.IR_SNX_Label3);
            this.IR_panel.Controls.Add(this.IR_COMP_Label2);
            this.IR_panel.Controls.Add(this.IR_COMP_Label1);
            this.IR_panel.Controls.Add(this.IR_COMP_Label3);
            this.IR_panel.Controls.Add(this.IR_USDC_Label2);
            this.IR_panel.Controls.Add(this.IR_USDC_Label1);
            this.IR_panel.Controls.Add(this.IR_USDC_Label3);
            this.IR_panel.Controls.Add(this.IR_LINK_Label2);
            this.IR_panel.Controls.Add(this.IR_LINK_Label1);
            this.IR_panel.Controls.Add(this.IR_LINK_Label3);
            this.IR_panel.Controls.Add(this.IR_DAI_Label2);
            this.IR_panel.Controls.Add(this.IR_DAI_Label1);
            this.IR_panel.Controls.Add(this.IR_DAI_Label3);
            this.IR_panel.Controls.Add(this.IR_USDT_Label2);
            this.IR_panel.Controls.Add(this.IR_USDT_Label3);
            this.IR_panel.Controls.Add(this.IR_USDT_Label1);
            this.IR_panel.Controls.Add(this.IR_ETC_Label2);
            this.IR_panel.Controls.Add(this.IR_ETC_Label3);
            this.IR_panel.Controls.Add(this.IR_ETC_Label1);
            this.IR_panel.Controls.Add(this.IR_MKR_Label2);
            this.IR_panel.Controls.Add(this.IR_MKR_Label1);
            this.IR_panel.Controls.Add(this.IR_MKR_Label3);
            this.IR_panel.Controls.Add(this.IR_BAT_Label2);
            this.IR_panel.Controls.Add(this.IR_BAT_Label1);
            this.IR_panel.Controls.Add(this.IR_BAT_Label3);
            this.IR_panel.Controls.Add(this.IR_XLM_Label2);
            this.IR_panel.Controls.Add(this.IR_XLM_Label3);
            this.IR_panel.Controls.Add(this.IR_XLM_Label1);
            this.IR_panel.Controls.Add(this.IR_EOS_Label2);
            this.IR_panel.Controls.Add(this.IR_EOS_Label3);
            this.IR_panel.Controls.Add(this.IR_EOS_Label1);
            this.IR_panel.Controls.Add(this.IR_ZRX_Label2);
            this.IR_panel.Controls.Add(this.IR_ZRX_Label1);
            this.IR_panel.Controls.Add(this.IR_XRP_Label2);
            this.IR_panel.Controls.Add(this.IR_ZRX_Label3);
            this.IR_panel.Controls.Add(this.IR_XRP_Label1);
            this.IR_panel.Controls.Add(this.IR_XBT_Label2);
            this.IR_panel.Controls.Add(this.IR_ETH_Label2);
            this.IR_panel.Controls.Add(this.IR_BCH_Label2);
            this.IR_panel.Controls.Add(this.IR_LTC_Label2);
            this.IR_panel.Controls.Add(this.IR_LTC_Label3);
            this.IR_panel.Controls.Add(this.IR_BCH_Label3);
            this.IR_panel.Controls.Add(this.IR_ETH_Label3);
            this.IR_panel.Controls.Add(this.IR_XBT_Label3);
            this.IR_panel.Controls.Add(this.IR_LTC_Label1);
            this.IR_panel.Controls.Add(this.IR_BCH_Label1);
            this.IR_panel.Controls.Add(this.IR_ETH_Label1);
            this.IR_panel.Controls.Add(this.IR_XBT_Label1);
            this.IR_panel.Controls.Add(this.IR_XRP_Label3);
            this.IR_panel.Location = new System.Drawing.Point(0, 24);
            this.IR_panel.Name = "IR_panel";
            this.IR_panel.Size = new System.Drawing.Size(262, 510);
            this.IR_panel.TabIndex = 66;
            // 
            // IR_WIF_Label2
            // 
            this.IR_WIF_Label2.AutoSize = true;
            this.IR_WIF_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_WIF_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_WIF_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_WIF_Label2.Location = new System.Drawing.Point(70, 607);
            this.IR_WIF_Label2.Name = "IR_WIF_Label2";
            this.IR_WIF_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_WIF_Label2.TabIndex = 201;
            this.IR_WIF_Label2.Tag = "IR";
            this.IR_WIF_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_WIF_Label1
            // 
            this.IR_WIF_Label1.AutoSize = true;
            this.IR_WIF_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_WIF_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_WIF_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_WIF_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_WIF_Label1.Location = new System.Drawing.Point(6, 607);
            this.IR_WIF_Label1.Name = "IR_WIF_Label1";
            this.IR_WIF_Label1.Size = new System.Drawing.Size(34, 13);
            this.IR_WIF_Label1.TabIndex = 200;
            this.IR_WIF_Label1.Tag = "DCECryptoLabel";
            this.IR_WIF_Label1.Text = "WIF:";
            this.IR_WIF_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_WIF_Label3
            // 
            this.IR_WIF_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_WIF_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_WIF_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_WIF_Label3.Location = new System.Drawing.Point(111, 607);
            this.IR_WIF_Label3.Name = "IR_WIF_Label3";
            this.IR_WIF_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_WIF_Label3.TabIndex = 199;
            this.IR_WIF_Label3.Tag = "";
            this.IR_WIF_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_RENDER_Label2
            // 
            this.IR_RENDER_Label2.AutoSize = true;
            this.IR_RENDER_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_RENDER_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_RENDER_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_RENDER_Label2.Location = new System.Drawing.Point(70, 467);
            this.IR_RENDER_Label2.Name = "IR_RENDER_Label2";
            this.IR_RENDER_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_RENDER_Label2.TabIndex = 198;
            this.IR_RENDER_Label2.Tag = "IR";
            this.IR_RENDER_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_RENDER_Label1
            // 
            this.IR_RENDER_Label1.AutoSize = true;
            this.IR_RENDER_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_RENDER_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_RENDER_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_RENDER_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_RENDER_Label1.Location = new System.Drawing.Point(6, 467);
            this.IR_RENDER_Label1.Name = "IR_RENDER_Label1";
            this.IR_RENDER_Label1.Size = new System.Drawing.Size(63, 13);
            this.IR_RENDER_Label1.TabIndex = 197;
            this.IR_RENDER_Label1.Tag = "DCECryptoLabel";
            this.IR_RENDER_Label1.Text = "RENDER:";
            this.IR_RENDER_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_RENDER_Label3
            // 
            this.IR_RENDER_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_RENDER_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_RENDER_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_RENDER_Label3.Location = new System.Drawing.Point(111, 467);
            this.IR_RENDER_Label3.Name = "IR_RENDER_Label3";
            this.IR_RENDER_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_RENDER_Label3.TabIndex = 196;
            this.IR_RENDER_Label3.Tag = "";
            this.IR_RENDER_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_TRX_Label2
            // 
            this.IR_TRX_Label2.AutoSize = true;
            this.IR_TRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_TRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_TRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_TRX_Label2.Location = new System.Drawing.Point(70, 567);
            this.IR_TRX_Label2.Name = "IR_TRX_Label2";
            this.IR_TRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_TRX_Label2.TabIndex = 195;
            this.IR_TRX_Label2.Tag = "IR";
            this.IR_TRX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_TRX_Label1
            // 
            this.IR_TRX_Label1.AutoSize = true;
            this.IR_TRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_TRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_TRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_TRX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_TRX_Label1.Location = new System.Drawing.Point(6, 567);
            this.IR_TRX_Label1.Name = "IR_TRX_Label1";
            this.IR_TRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_TRX_Label1.TabIndex = 194;
            this.IR_TRX_Label1.Tag = "DCECryptoLabel";
            this.IR_TRX_Label1.Text = "TRX:";
            this.IR_TRX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_TRX_Label3
            // 
            this.IR_TRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_TRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_TRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_TRX_Label3.Location = new System.Drawing.Point(111, 567);
            this.IR_TRX_Label3.Name = "IR_TRX_Label3";
            this.IR_TRX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_TRX_Label3.TabIndex = 193;
            this.IR_TRX_Label3.Tag = "";
            this.IR_TRX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_SHIB_Label2
            // 
            this.IR_SHIB_Label2.AutoSize = true;
            this.IR_SHIB_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_SHIB_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SHIB_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SHIB_Label2.Location = new System.Drawing.Point(70, 507);
            this.IR_SHIB_Label2.Name = "IR_SHIB_Label2";
            this.IR_SHIB_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_SHIB_Label2.TabIndex = 192;
            this.IR_SHIB_Label2.Tag = "IR";
            this.IR_SHIB_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SHIB_Label1
            // 
            this.IR_SHIB_Label1.AutoSize = true;
            this.IR_SHIB_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_SHIB_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SHIB_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SHIB_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_SHIB_Label1.Location = new System.Drawing.Point(6, 507);
            this.IR_SHIB_Label1.Name = "IR_SHIB_Label1";
            this.IR_SHIB_Label1.Size = new System.Drawing.Size(40, 13);
            this.IR_SHIB_Label1.TabIndex = 191;
            this.IR_SHIB_Label1.Tag = "DCECryptoLabel";
            this.IR_SHIB_Label1.Text = "SHIB:";
            this.IR_SHIB_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SHIB_Label3
            // 
            this.IR_SHIB_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_SHIB_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_SHIB_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SHIB_Label3.Location = new System.Drawing.Point(111, 507);
            this.IR_SHIB_Label3.Name = "IR_SHIB_Label3";
            this.IR_SHIB_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_SHIB_Label3.TabIndex = 190;
            this.IR_SHIB_Label3.Tag = "";
            this.IR_SHIB_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_SAND_Label2
            // 
            this.IR_SAND_Label2.AutoSize = true;
            this.IR_SAND_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_SAND_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SAND_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SAND_Label2.Location = new System.Drawing.Point(70, 487);
            this.IR_SAND_Label2.Name = "IR_SAND_Label2";
            this.IR_SAND_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_SAND_Label2.TabIndex = 189;
            this.IR_SAND_Label2.Tag = "IR";
            this.IR_SAND_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SAND_Label1
            // 
            this.IR_SAND_Label1.AutoSize = true;
            this.IR_SAND_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_SAND_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SAND_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SAND_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_SAND_Label1.Location = new System.Drawing.Point(6, 487);
            this.IR_SAND_Label1.Name = "IR_SAND_Label1";
            this.IR_SAND_Label1.Size = new System.Drawing.Size(45, 13);
            this.IR_SAND_Label1.TabIndex = 188;
            this.IR_SAND_Label1.Tag = "DCECryptoLabel";
            this.IR_SAND_Label1.Text = "SAND:";
            this.IR_SAND_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SAND_Label3
            // 
            this.IR_SAND_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_SAND_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_SAND_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SAND_Label3.Location = new System.Drawing.Point(111, 487);
            this.IR_SAND_Label3.Name = "IR_SAND_Label3";
            this.IR_SAND_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_SAND_Label3.TabIndex = 187;
            this.IR_SAND_Label3.Tag = "";
            this.IR_SAND_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_SOL_Label2
            // 
            this.IR_SOL_Label2.AutoSize = true;
            this.IR_SOL_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_SOL_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SOL_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SOL_Label2.Location = new System.Drawing.Point(70, 47);
            this.IR_SOL_Label2.Name = "IR_SOL_Label2";
            this.IR_SOL_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_SOL_Label2.TabIndex = 186;
            this.IR_SOL_Label2.Tag = "IR";
            this.IR_SOL_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SOL_Label1
            // 
            this.IR_SOL_Label1.AutoSize = true;
            this.IR_SOL_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_SOL_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SOL_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SOL_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_SOL_Label1.Location = new System.Drawing.Point(6, 47);
            this.IR_SOL_Label1.Name = "IR_SOL_Label1";
            this.IR_SOL_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_SOL_Label1.TabIndex = 185;
            this.IR_SOL_Label1.Tag = "DCECryptoLabel";
            this.IR_SOL_Label1.Text = "SOL:";
            this.IR_SOL_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SOL_Label3
            // 
            this.IR_SOL_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_SOL_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_SOL_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SOL_Label3.Location = new System.Drawing.Point(111, 47);
            this.IR_SOL_Label3.Name = "IR_SOL_Label3";
            this.IR_SOL_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_SOL_Label3.TabIndex = 184;
            this.IR_SOL_Label3.Tag = "";
            this.IR_SOL_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_MANA_Label2
            // 
            this.IR_MANA_Label2.AutoSize = true;
            this.IR_MANA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_MANA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MANA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_MANA_Label2.Location = new System.Drawing.Point(70, 387);
            this.IR_MANA_Label2.Name = "IR_MANA_Label2";
            this.IR_MANA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_MANA_Label2.TabIndex = 183;
            this.IR_MANA_Label2.Tag = "IR";
            this.IR_MANA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MANA_Label1
            // 
            this.IR_MANA_Label1.AutoSize = true;
            this.IR_MANA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_MANA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MANA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_MANA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_MANA_Label1.Location = new System.Drawing.Point(6, 387);
            this.IR_MANA_Label1.Name = "IR_MANA_Label1";
            this.IR_MANA_Label1.Size = new System.Drawing.Size(46, 13);
            this.IR_MANA_Label1.TabIndex = 182;
            this.IR_MANA_Label1.Tag = "DCECryptoLabel";
            this.IR_MANA_Label1.Text = "MANA:";
            this.IR_MANA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MANA_Label3
            // 
            this.IR_MANA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_MANA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_MANA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MANA_Label3.Location = new System.Drawing.Point(111, 387);
            this.IR_MANA_Label3.Name = "IR_MANA_Label3";
            this.IR_MANA_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_MANA_Label3.TabIndex = 181;
            this.IR_MANA_Label3.Tag = "";
            this.IR_MANA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_MATIC_Label2
            // 
            this.IR_MATIC_Label2.AutoSize = true;
            this.IR_MATIC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_MATIC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MATIC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_MATIC_Label2.Location = new System.Drawing.Point(70, 407);
            this.IR_MATIC_Label2.Name = "IR_MATIC_Label2";
            this.IR_MATIC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_MATIC_Label2.TabIndex = 180;
            this.IR_MATIC_Label2.Tag = "IR";
            this.IR_MATIC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MATIC_Label1
            // 
            this.IR_MATIC_Label1.AutoSize = true;
            this.IR_MATIC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_MATIC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MATIC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_MATIC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_MATIC_Label1.Location = new System.Drawing.Point(6, 407);
            this.IR_MATIC_Label1.Name = "IR_MATIC_Label1";
            this.IR_MATIC_Label1.Size = new System.Drawing.Size(49, 13);
            this.IR_MATIC_Label1.TabIndex = 179;
            this.IR_MATIC_Label1.Tag = "DCECryptoLabel";
            this.IR_MATIC_Label1.Text = "MATIC:";
            this.IR_MATIC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MATIC_Label3
            // 
            this.IR_MATIC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_MATIC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_MATIC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MATIC_Label3.Location = new System.Drawing.Point(111, 407);
            this.IR_MATIC_Label3.Name = "IR_MATIC_Label3";
            this.IR_MATIC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_MATIC_Label3.TabIndex = 178;
            this.IR_MATIC_Label3.Tag = "";
            this.IR_MATIC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_DOGE_Label2
            // 
            this.IR_DOGE_Label2.AutoSize = true;
            this.IR_DOGE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_DOGE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DOGE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_DOGE_Label2.Location = new System.Drawing.Point(70, 247);
            this.IR_DOGE_Label2.Name = "IR_DOGE_Label2";
            this.IR_DOGE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_DOGE_Label2.TabIndex = 177;
            this.IR_DOGE_Label2.Tag = "IR";
            this.IR_DOGE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_DOGE_Label1
            // 
            this.IR_DOGE_Label1.AutoSize = true;
            this.IR_DOGE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_DOGE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DOGE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_DOGE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_DOGE_Label1.Location = new System.Drawing.Point(6, 247);
            this.IR_DOGE_Label1.Name = "IR_DOGE_Label1";
            this.IR_DOGE_Label1.Size = new System.Drawing.Size(46, 13);
            this.IR_DOGE_Label1.TabIndex = 176;
            this.IR_DOGE_Label1.Tag = "DCECryptoLabel";
            this.IR_DOGE_Label1.Text = "DOGE:";
            this.IR_DOGE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_DOGE_Label3
            // 
            this.IR_DOGE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_DOGE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_DOGE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DOGE_Label3.Location = new System.Drawing.Point(111, 247);
            this.IR_DOGE_Label3.Name = "IR_DOGE_Label3";
            this.IR_DOGE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_DOGE_Label3.TabIndex = 175;
            this.IR_DOGE_Label3.Tag = "";
            this.IR_DOGE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_ADA_Label2
            // 
            this.IR_ADA_Label2.AutoSize = true;
            this.IR_ADA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ADA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ADA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ADA_Label2.Location = new System.Drawing.Point(70, 147);
            this.IR_ADA_Label2.Name = "IR_ADA_Label2";
            this.IR_ADA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ADA_Label2.TabIndex = 174;
            this.IR_ADA_Label2.Tag = "IR";
            this.IR_ADA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ADA_Label1
            // 
            this.IR_ADA_Label1.AutoSize = true;
            this.IR_ADA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_ADA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ADA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ADA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_ADA_Label1.Location = new System.Drawing.Point(6, 147);
            this.IR_ADA_Label1.Name = "IR_ADA_Label1";
            this.IR_ADA_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_ADA_Label1.TabIndex = 173;
            this.IR_ADA_Label1.Tag = "DCECryptoLabel";
            this.IR_ADA_Label1.Text = "ADA:";
            this.IR_ADA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ADA_Label3
            // 
            this.IR_ADA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_ADA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ADA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ADA_Label3.Location = new System.Drawing.Point(111, 147);
            this.IR_ADA_Label3.Name = "IR_ADA_Label3";
            this.IR_ADA_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_ADA_Label3.TabIndex = 172;
            this.IR_ADA_Label3.Tag = "";
            this.IR_ADA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_UNI_Label2
            // 
            this.IR_UNI_Label2.AutoSize = true;
            this.IR_UNI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_UNI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_UNI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_UNI_Label2.Location = new System.Drawing.Point(70, 587);
            this.IR_UNI_Label2.Name = "IR_UNI_Label2";
            this.IR_UNI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_UNI_Label2.TabIndex = 171;
            this.IR_UNI_Label2.Tag = "IR";
            this.IR_UNI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_UNI_Label1
            // 
            this.IR_UNI_Label1.AutoSize = true;
            this.IR_UNI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_UNI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_UNI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_UNI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_UNI_Label1.Location = new System.Drawing.Point(6, 587);
            this.IR_UNI_Label1.Name = "IR_UNI_Label1";
            this.IR_UNI_Label1.Size = new System.Drawing.Size(33, 13);
            this.IR_UNI_Label1.TabIndex = 170;
            this.IR_UNI_Label1.Tag = "DCECryptoLabel";
            this.IR_UNI_Label1.Text = "UNI:";
            this.IR_UNI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_UNI_Label3
            // 
            this.IR_UNI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_UNI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_UNI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_UNI_Label3.Location = new System.Drawing.Point(111, 587);
            this.IR_UNI_Label3.Name = "IR_UNI_Label3";
            this.IR_UNI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_UNI_Label3.TabIndex = 169;
            this.IR_UNI_Label3.Tag = "";
            this.IR_UNI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_GRT_Label2
            // 
            this.IR_GRT_Label2.AutoSize = true;
            this.IR_GRT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_GRT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GRT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_GRT_Label2.Location = new System.Drawing.Point(70, 327);
            this.IR_GRT_Label2.Name = "IR_GRT_Label2";
            this.IR_GRT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_GRT_Label2.TabIndex = 168;
            this.IR_GRT_Label2.Tag = "IR";
            this.IR_GRT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_GRT_Label1
            // 
            this.IR_GRT_Label1.AutoSize = true;
            this.IR_GRT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_GRT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GRT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_GRT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_GRT_Label1.Location = new System.Drawing.Point(6, 327);
            this.IR_GRT_Label1.Name = "IR_GRT_Label1";
            this.IR_GRT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IR_GRT_Label1.TabIndex = 167;
            this.IR_GRT_Label1.Tag = "DCECryptoLabel";
            this.IR_GRT_Label1.Text = "GRT:";
            this.IR_GRT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_GRT_Label3
            // 
            this.IR_GRT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_GRT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_GRT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GRT_Label3.Location = new System.Drawing.Point(111, 327);
            this.IR_GRT_Label3.Name = "IR_GRT_Label3";
            this.IR_GRT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_GRT_Label3.TabIndex = 166;
            this.IR_GRT_Label3.Tag = "";
            this.IR_GRT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_DOT_Label2
            // 
            this.IR_DOT_Label2.AutoSize = true;
            this.IR_DOT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_DOT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DOT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_DOT_Label2.Location = new System.Drawing.Point(70, 267);
            this.IR_DOT_Label2.Name = "IR_DOT_Label2";
            this.IR_DOT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_DOT_Label2.TabIndex = 165;
            this.IR_DOT_Label2.Tag = "IR";
            this.IR_DOT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_DOT_Label1
            // 
            this.IR_DOT_Label1.AutoSize = true;
            this.IR_DOT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_DOT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DOT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_DOT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_DOT_Label1.Location = new System.Drawing.Point(6, 267);
            this.IR_DOT_Label1.Name = "IR_DOT_Label1";
            this.IR_DOT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IR_DOT_Label1.TabIndex = 164;
            this.IR_DOT_Label1.Tag = "DCECryptoLabel";
            this.IR_DOT_Label1.Text = "DOT:";
            this.IR_DOT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_DOT_Label3
            // 
            this.IR_DOT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_DOT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_DOT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DOT_Label3.Location = new System.Drawing.Point(111, 267);
            this.IR_DOT_Label3.Name = "IR_DOT_Label3";
            this.IR_DOT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_DOT_Label3.TabIndex = 163;
            this.IR_DOT_Label3.Tag = "";
            this.IR_DOT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_AAVE_Label2
            // 
            this.IR_AAVE_Label2.AutoSize = true;
            this.IR_AAVE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_AAVE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_AAVE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_AAVE_Label2.Location = new System.Drawing.Point(70, 127);
            this.IR_AAVE_Label2.Name = "IR_AAVE_Label2";
            this.IR_AAVE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_AAVE_Label2.TabIndex = 162;
            this.IR_AAVE_Label2.Tag = "IR";
            this.IR_AAVE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_AAVE_Label1
            // 
            this.IR_AAVE_Label1.AutoSize = true;
            this.IR_AAVE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_AAVE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_AAVE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_AAVE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_AAVE_Label1.Location = new System.Drawing.Point(6, 127);
            this.IR_AAVE_Label1.Name = "IR_AAVE_Label1";
            this.IR_AAVE_Label1.Size = new System.Drawing.Size(43, 13);
            this.IR_AAVE_Label1.TabIndex = 161;
            this.IR_AAVE_Label1.Tag = "DCECryptoLabel";
            this.IR_AAVE_Label1.Text = "AAVE:";
            this.IR_AAVE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_AAVE_Label3
            // 
            this.IR_AAVE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_AAVE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_AAVE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_AAVE_Label3.Location = new System.Drawing.Point(111, 127);
            this.IR_AAVE_Label3.Name = "IR_AAVE_Label3";
            this.IR_AAVE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_AAVE_Label3.TabIndex = 160;
            this.IR_AAVE_Label3.Tag = "";
            this.IR_AAVE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_YFI_Label2
            // 
            this.IR_YFI_Label2.AutoSize = true;
            this.IR_YFI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_YFI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_YFI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_YFI_Label2.Location = new System.Drawing.Point(70, 647);
            this.IR_YFI_Label2.Name = "IR_YFI_Label2";
            this.IR_YFI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_YFI_Label2.TabIndex = 159;
            this.IR_YFI_Label2.Tag = "IR";
            this.IR_YFI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_YFI_Label1
            // 
            this.IR_YFI_Label1.AutoSize = true;
            this.IR_YFI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_YFI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_YFI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_YFI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_YFI_Label1.Location = new System.Drawing.Point(6, 647);
            this.IR_YFI_Label1.Name = "IR_YFI_Label1";
            this.IR_YFI_Label1.Size = new System.Drawing.Size(30, 13);
            this.IR_YFI_Label1.TabIndex = 158;
            this.IR_YFI_Label1.Tag = "DCECryptoLabel";
            this.IR_YFI_Label1.Text = "YFI:";
            this.IR_YFI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_YFI_Label3
            // 
            this.IR_YFI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_YFI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_YFI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_YFI_Label3.Location = new System.Drawing.Point(111, 647);
            this.IR_YFI_Label3.Name = "IR_YFI_Label3";
            this.IR_YFI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_YFI_Label3.TabIndex = 157;
            this.IR_YFI_Label3.Tag = "";
            this.IR_YFI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_SNX_Label2
            // 
            this.IR_SNX_Label2.AutoSize = true;
            this.IR_SNX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_SNX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SNX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SNX_Label2.Location = new System.Drawing.Point(70, 527);
            this.IR_SNX_Label2.Name = "IR_SNX_Label2";
            this.IR_SNX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_SNX_Label2.TabIndex = 153;
            this.IR_SNX_Label2.Tag = "IR";
            this.IR_SNX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SNX_Label1
            // 
            this.IR_SNX_Label1.AutoSize = true;
            this.IR_SNX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_SNX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SNX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_SNX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_SNX_Label1.Location = new System.Drawing.Point(6, 527);
            this.IR_SNX_Label1.Name = "IR_SNX_Label1";
            this.IR_SNX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_SNX_Label1.TabIndex = 152;
            this.IR_SNX_Label1.Tag = "DCECryptoLabel";
            this.IR_SNX_Label1.Text = "SNX:";
            this.IR_SNX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_SNX_Label3
            // 
            this.IR_SNX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_SNX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_SNX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_SNX_Label3.Location = new System.Drawing.Point(111, 527);
            this.IR_SNX_Label3.Name = "IR_SNX_Label3";
            this.IR_SNX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_SNX_Label3.TabIndex = 151;
            this.IR_SNX_Label3.Tag = "";
            this.IR_SNX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_COMP_Label2
            // 
            this.IR_COMP_Label2.AutoSize = true;
            this.IR_COMP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_COMP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_COMP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_COMP_Label2.Location = new System.Drawing.Point(70, 207);
            this.IR_COMP_Label2.Name = "IR_COMP_Label2";
            this.IR_COMP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_COMP_Label2.TabIndex = 150;
            this.IR_COMP_Label2.Tag = "IR";
            this.IR_COMP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_COMP_Label1
            // 
            this.IR_COMP_Label1.AutoSize = true;
            this.IR_COMP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_COMP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_COMP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_COMP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_COMP_Label1.Location = new System.Drawing.Point(6, 207);
            this.IR_COMP_Label1.Name = "IR_COMP_Label1";
            this.IR_COMP_Label1.Size = new System.Drawing.Size(46, 13);
            this.IR_COMP_Label1.TabIndex = 149;
            this.IR_COMP_Label1.Tag = "DCECryptoLabel";
            this.IR_COMP_Label1.Text = "COMP:";
            this.IR_COMP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_COMP_Label3
            // 
            this.IR_COMP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_COMP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_COMP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_COMP_Label3.Location = new System.Drawing.Point(111, 207);
            this.IR_COMP_Label3.Name = "IR_COMP_Label3";
            this.IR_COMP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_COMP_Label3.TabIndex = 148;
            this.IR_COMP_Label3.Tag = "";
            this.IR_COMP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_USDC_Label2
            // 
            this.IR_USDC_Label2.AutoSize = true;
            this.IR_USDC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_USDC_Label2.Location = new System.Drawing.Point(70, 87);
            this.IR_USDC_Label2.Name = "IR_USDC_Label2";
            this.IR_USDC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_USDC_Label2.TabIndex = 147;
            this.IR_USDC_Label2.Tag = "IR";
            this.IR_USDC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_USDC_Label1
            // 
            this.IR_USDC_Label1.AutoSize = true;
            this.IR_USDC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_USDC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_USDC_Label1.Location = new System.Drawing.Point(6, 87);
            this.IR_USDC_Label1.Name = "IR_USDC_Label1";
            this.IR_USDC_Label1.Size = new System.Drawing.Size(45, 13);
            this.IR_USDC_Label1.TabIndex = 146;
            this.IR_USDC_Label1.Tag = "DCECryptoLabel";
            this.IR_USDC_Label1.Text = "USDC:";
            this.IR_USDC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_USDC_Label3
            // 
            this.IR_USDC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_USDC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDC_Label3.Location = new System.Drawing.Point(111, 87);
            this.IR_USDC_Label3.Name = "IR_USDC_Label3";
            this.IR_USDC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_USDC_Label3.TabIndex = 145;
            this.IR_USDC_Label3.Tag = "";
            this.IR_USDC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_LINK_Label2
            // 
            this.IR_LINK_Label2.AutoSize = true;
            this.IR_LINK_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_LINK_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LINK_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LINK_Label2.Location = new System.Drawing.Point(70, 347);
            this.IR_LINK_Label2.Name = "IR_LINK_Label2";
            this.IR_LINK_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_LINK_Label2.TabIndex = 144;
            this.IR_LINK_Label2.Tag = "IR";
            this.IR_LINK_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_LINK_Label1
            // 
            this.IR_LINK_Label1.AutoSize = true;
            this.IR_LINK_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_LINK_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LINK_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LINK_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_LINK_Label1.Location = new System.Drawing.Point(6, 347);
            this.IR_LINK_Label1.Name = "IR_LINK_Label1";
            this.IR_LINK_Label1.Size = new System.Drawing.Size(39, 13);
            this.IR_LINK_Label1.TabIndex = 143;
            this.IR_LINK_Label1.Tag = "DCECryptoLabel";
            this.IR_LINK_Label1.Text = "LINK:";
            this.IR_LINK_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_LINK_Label3
            // 
            this.IR_LINK_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_LINK_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_LINK_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LINK_Label3.Location = new System.Drawing.Point(111, 347);
            this.IR_LINK_Label3.Name = "IR_LINK_Label3";
            this.IR_LINK_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_LINK_Label3.TabIndex = 142;
            this.IR_LINK_Label3.Tag = "";
            this.IR_LINK_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_DAI_Label2
            // 
            this.IR_DAI_Label2.AutoSize = true;
            this.IR_DAI_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_DAI_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DAI_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_DAI_Label2.Location = new System.Drawing.Point(70, 227);
            this.IR_DAI_Label2.Name = "IR_DAI_Label2";
            this.IR_DAI_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_DAI_Label2.TabIndex = 141;
            this.IR_DAI_Label2.Tag = "IR";
            this.IR_DAI_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_DAI_Label1
            // 
            this.IR_DAI_Label1.AutoSize = true;
            this.IR_DAI_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_DAI_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DAI_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_DAI_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_DAI_Label1.Location = new System.Drawing.Point(6, 227);
            this.IR_DAI_Label1.Name = "IR_DAI_Label1";
            this.IR_DAI_Label1.Size = new System.Drawing.Size(32, 13);
            this.IR_DAI_Label1.TabIndex = 140;
            this.IR_DAI_Label1.Tag = "DCECryptoLabel";
            this.IR_DAI_Label1.Text = "DAI:";
            this.IR_DAI_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_DAI_Label3
            // 
            this.IR_DAI_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_DAI_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_DAI_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_DAI_Label3.Location = new System.Drawing.Point(111, 227);
            this.IR_DAI_Label3.Name = "IR_DAI_Label3";
            this.IR_DAI_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_DAI_Label3.TabIndex = 139;
            this.IR_DAI_Label3.Tag = "";
            this.IR_DAI_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_USDT_Label2
            // 
            this.IR_USDT_Label2.AutoSize = true;
            this.IR_USDT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_USDT_Label2.Location = new System.Drawing.Point(70, 107);
            this.IR_USDT_Label2.Name = "IR_USDT_Label2";
            this.IR_USDT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_USDT_Label2.TabIndex = 137;
            this.IR_USDT_Label2.Tag = "IR";
            this.IR_USDT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_USDT_Label3
            // 
            this.IR_USDT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_USDT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDT_Label3.Location = new System.Drawing.Point(111, 107);
            this.IR_USDT_Label3.Name = "IR_USDT_Label3";
            this.IR_USDT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_USDT_Label3.TabIndex = 138;
            this.IR_USDT_Label3.Tag = "";
            this.IR_USDT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_USDT_Label1
            // 
            this.IR_USDT_Label1.AutoSize = true;
            this.IR_USDT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_USDT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_USDT_Label1.Location = new System.Drawing.Point(6, 107);
            this.IR_USDT_Label1.Name = "IR_USDT_Label1";
            this.IR_USDT_Label1.Size = new System.Drawing.Size(45, 13);
            this.IR_USDT_Label1.TabIndex = 136;
            this.IR_USDT_Label1.Tag = "DCECryptoLabel";
            this.IR_USDT_Label1.Text = "USDT:";
            this.IR_USDT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETC_Label2
            // 
            this.IR_ETC_Label2.AutoSize = true;
            this.IR_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETC_Label2.Location = new System.Drawing.Point(70, 307);
            this.IR_ETC_Label2.Name = "IR_ETC_Label2";
            this.IR_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ETC_Label2.TabIndex = 134;
            this.IR_ETC_Label2.Tag = "IR";
            this.IR_ETC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETC_Label3
            // 
            this.IR_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETC_Label3.Location = new System.Drawing.Point(111, 307);
            this.IR_ETC_Label3.Name = "IR_ETC_Label3";
            this.IR_ETC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_ETC_Label3.TabIndex = 135;
            this.IR_ETC_Label3.Tag = "";
            this.IR_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_ETC_Label1
            // 
            this.IR_ETC_Label1.AutoSize = true;
            this.IR_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_ETC_Label1.Location = new System.Drawing.Point(6, 307);
            this.IR_ETC_Label1.Name = "IR_ETC_Label1";
            this.IR_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_ETC_Label1.TabIndex = 133;
            this.IR_ETC_Label1.Tag = "DCECryptoLabel";
            this.IR_ETC_Label1.Text = "ETC:";
            this.IR_ETC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MKR_Label2
            // 
            this.IR_MKR_Label2.AutoSize = true;
            this.IR_MKR_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_MKR_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MKR_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_MKR_Label2.Location = new System.Drawing.Point(70, 427);
            this.IR_MKR_Label2.Name = "IR_MKR_Label2";
            this.IR_MKR_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_MKR_Label2.TabIndex = 132;
            this.IR_MKR_Label2.Tag = "IR";
            this.IR_MKR_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MKR_Label1
            // 
            this.IR_MKR_Label1.AutoSize = true;
            this.IR_MKR_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_MKR_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MKR_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_MKR_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_MKR_Label1.Location = new System.Drawing.Point(6, 427);
            this.IR_MKR_Label1.Name = "IR_MKR_Label1";
            this.IR_MKR_Label1.Size = new System.Drawing.Size(38, 13);
            this.IR_MKR_Label1.TabIndex = 131;
            this.IR_MKR_Label1.Tag = "DCECryptoLabel";
            this.IR_MKR_Label1.Text = "MKR:";
            this.IR_MKR_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_MKR_Label3
            // 
            this.IR_MKR_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_MKR_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_MKR_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_MKR_Label3.Location = new System.Drawing.Point(111, 427);
            this.IR_MKR_Label3.Name = "IR_MKR_Label3";
            this.IR_MKR_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_MKR_Label3.TabIndex = 130;
            this.IR_MKR_Label3.Tag = "";
            this.IR_MKR_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_BAT_Label2
            // 
            this.IR_BAT_Label2.AutoSize = true;
            this.IR_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BAT_Label2.Location = new System.Drawing.Point(70, 167);
            this.IR_BAT_Label2.Name = "IR_BAT_Label2";
            this.IR_BAT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_BAT_Label2.TabIndex = 129;
            this.IR_BAT_Label2.Tag = "IR";
            this.IR_BAT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BAT_Label1
            // 
            this.IR_BAT_Label1.AutoSize = true;
            this.IR_BAT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_BAT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BAT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BAT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_BAT_Label1.Location = new System.Drawing.Point(6, 167);
            this.IR_BAT_Label1.Name = "IR_BAT_Label1";
            this.IR_BAT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_BAT_Label1.TabIndex = 128;
            this.IR_BAT_Label1.Tag = "DCECryptoLabel";
            this.IR_BAT_Label1.Text = "BAT:";
            this.IR_BAT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BAT_Label3
            // 
            this.IR_BAT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_BAT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_BAT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BAT_Label3.Location = new System.Drawing.Point(111, 167);
            this.IR_BAT_Label3.Name = "IR_BAT_Label3";
            this.IR_BAT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_BAT_Label3.TabIndex = 127;
            this.IR_BAT_Label3.Tag = "";
            this.IR_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_XLM_Label2
            // 
            this.IR_XLM_Label2.AutoSize = true;
            this.IR_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XLM_Label2.Location = new System.Drawing.Point(70, 627);
            this.IR_XLM_Label2.Name = "IR_XLM_Label2";
            this.IR_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XLM_Label2.TabIndex = 125;
            this.IR_XLM_Label2.Tag = "IR";
            this.IR_XLM_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XLM_Label3
            // 
            this.IR_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XLM_Label3.Location = new System.Drawing.Point(111, 627);
            this.IR_XLM_Label3.Name = "IR_XLM_Label3";
            this.IR_XLM_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_XLM_Label3.TabIndex = 126;
            this.IR_XLM_Label3.Tag = "";
            this.IR_XLM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_XLM_Label1
            // 
            this.IR_XLM_Label1.AutoSize = true;
            this.IR_XLM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_XLM_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XLM_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XLM_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_XLM_Label1.Location = new System.Drawing.Point(6, 627);
            this.IR_XLM_Label1.Name = "IR_XLM_Label1";
            this.IR_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_XLM_Label1.TabIndex = 124;
            this.IR_XLM_Label1.Tag = "DCECryptoLabel";
            this.IR_XLM_Label1.Text = "XLM:";
            this.IR_XLM_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_EOS_Label2
            // 
            this.IR_EOS_Label2.AutoSize = true;
            this.IR_EOS_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_EOS_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_EOS_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_EOS_Label2.Location = new System.Drawing.Point(70, 287);
            this.IR_EOS_Label2.Name = "IR_EOS_Label2";
            this.IR_EOS_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_EOS_Label2.TabIndex = 122;
            this.IR_EOS_Label2.Tag = "IR";
            this.IR_EOS_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_EOS_Label3
            // 
            this.IR_EOS_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_EOS_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_EOS_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_EOS_Label3.Location = new System.Drawing.Point(111, 287);
            this.IR_EOS_Label3.Name = "IR_EOS_Label3";
            this.IR_EOS_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_EOS_Label3.TabIndex = 123;
            this.IR_EOS_Label3.Tag = "";
            this.IR_EOS_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_EOS_Label1
            // 
            this.IR_EOS_Label1.AutoSize = true;
            this.IR_EOS_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_EOS_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_EOS_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_EOS_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_EOS_Label1.Location = new System.Drawing.Point(6, 287);
            this.IR_EOS_Label1.Name = "IR_EOS_Label1";
            this.IR_EOS_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_EOS_Label1.TabIndex = 121;
            this.IR_EOS_Label1.Tag = "DCECryptoLabel";
            this.IR_EOS_Label1.Text = "EOS:";
            this.IR_EOS_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ZRX_Label2
            // 
            this.IR_ZRX_Label2.AutoSize = true;
            this.IR_ZRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ZRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ZRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ZRX_Label2.Location = new System.Drawing.Point(70, 667);
            this.IR_ZRX_Label2.Name = "IR_ZRX_Label2";
            this.IR_ZRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ZRX_Label2.TabIndex = 120;
            this.IR_ZRX_Label2.Tag = "IR";
            this.IR_ZRX_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ZRX_Label1
            // 
            this.IR_ZRX_Label1.AutoSize = true;
            this.IR_ZRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_ZRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ZRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ZRX_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_ZRX_Label1.Location = new System.Drawing.Point(6, 667);
            this.IR_ZRX_Label1.Name = "IR_ZRX_Label1";
            this.IR_ZRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_ZRX_Label1.TabIndex = 119;
            this.IR_ZRX_Label1.Tag = "DCECryptoLabel";
            this.IR_ZRX_Label1.Text = "ZRX:";
            this.IR_ZRX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XRP_Label2
            // 
            this.IR_XRP_Label2.AutoSize = true;
            this.IR_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XRP_Label2.Location = new System.Drawing.Point(70, 67);
            this.IR_XRP_Label2.Name = "IR_XRP_Label2";
            this.IR_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XRP_Label2.TabIndex = 113;
            this.IR_XRP_Label2.Tag = "IR";
            this.IR_XRP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ZRX_Label3
            // 
            this.IR_ZRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_ZRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ZRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ZRX_Label3.Location = new System.Drawing.Point(111, 667);
            this.IR_ZRX_Label3.Name = "IR_ZRX_Label3";
            this.IR_ZRX_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_ZRX_Label3.TabIndex = 114;
            this.IR_ZRX_Label3.Tag = "";
            this.IR_ZRX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_XRP_Label1
            // 
            this.IR_XRP_Label1.AutoSize = true;
            this.IR_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XRP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_XRP_Label1.Location = new System.Drawing.Point(6, 67);
            this.IR_XRP_Label1.Name = "IR_XRP_Label1";
            this.IR_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_XRP_Label1.TabIndex = 112;
            this.IR_XRP_Label1.Tag = "DCECryptoLabel";
            this.IR_XRP_Label1.Text = "XRP:";
            this.IR_XRP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XBT_Label2
            // 
            this.IR_XBT_Label2.AutoSize = true;
            this.IR_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XBT_Label2.Location = new System.Drawing.Point(70, 7);
            this.IR_XBT_Label2.Name = "IR_XBT_Label2";
            this.IR_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XBT_Label2.TabIndex = 104;
            this.IR_XBT_Label2.Tag = "IR";
            this.IR_XBT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETH_Label2
            // 
            this.IR_ETH_Label2.AutoSize = true;
            this.IR_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETH_Label2.Location = new System.Drawing.Point(70, 27);
            this.IR_ETH_Label2.Name = "IR_ETH_Label2";
            this.IR_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ETH_Label2.TabIndex = 105;
            this.IR_ETH_Label2.Tag = "IR";
            this.IR_ETH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BCH_Label2
            // 
            this.IR_BCH_Label2.AutoSize = true;
            this.IR_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BCH_Label2.Location = new System.Drawing.Point(70, 187);
            this.IR_BCH_Label2.Name = "IR_BCH_Label2";
            this.IR_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_BCH_Label2.TabIndex = 106;
            this.IR_BCH_Label2.Tag = "IR";
            this.IR_BCH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_LTC_Label2
            // 
            this.IR_LTC_Label2.AutoSize = true;
            this.IR_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LTC_Label2.Location = new System.Drawing.Point(70, 367);
            this.IR_LTC_Label2.Name = "IR_LTC_Label2";
            this.IR_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_LTC_Label2.TabIndex = 107;
            this.IR_LTC_Label2.Tag = "IR";
            this.IR_LTC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_LTC_Label3
            // 
            this.IR_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LTC_Label3.Location = new System.Drawing.Point(111, 367);
            this.IR_LTC_Label3.Name = "IR_LTC_Label3";
            this.IR_LTC_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_LTC_Label3.TabIndex = 111;
            this.IR_LTC_Label3.Tag = "";
            this.IR_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_BCH_Label3
            // 
            this.IR_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BCH_Label3.Location = new System.Drawing.Point(111, 187);
            this.IR_BCH_Label3.Name = "IR_BCH_Label3";
            this.IR_BCH_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_BCH_Label3.TabIndex = 110;
            this.IR_BCH_Label3.Tag = "";
            this.IR_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_ETH_Label3
            // 
            this.IR_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETH_Label3.Location = new System.Drawing.Point(111, 27);
            this.IR_ETH_Label3.Name = "IR_ETH_Label3";
            this.IR_ETH_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_ETH_Label3.TabIndex = 109;
            this.IR_ETH_Label3.Tag = "";
            this.IR_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_XBT_Label3
            // 
            this.IR_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XBT_Label3.Location = new System.Drawing.Point(111, 7);
            this.IR_XBT_Label3.Name = "IR_XBT_Label3";
            this.IR_XBT_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_XBT_Label3.TabIndex = 108;
            this.IR_XBT_Label3.Tag = "";
            this.IR_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_LTC_Label1
            // 
            this.IR_LTC_Label1.AutoSize = true;
            this.IR_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LTC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_LTC_Label1.Location = new System.Drawing.Point(6, 367);
            this.IR_LTC_Label1.Name = "IR_LTC_Label1";
            this.IR_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.IR_LTC_Label1.TabIndex = 103;
            this.IR_LTC_Label1.Tag = "DCECryptoLabel";
            this.IR_LTC_Label1.Text = "LTC:";
            this.IR_LTC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BCH_Label1
            // 
            this.IR_BCH_Label1.AutoSize = true;
            this.IR_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BCH_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_BCH_Label1.Location = new System.Drawing.Point(6, 187);
            this.IR_BCH_Label1.Name = "IR_BCH_Label1";
            this.IR_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_BCH_Label1.TabIndex = 102;
            this.IR_BCH_Label1.Tag = "DCECryptoLabel";
            this.IR_BCH_Label1.Text = "BCH:";
            this.IR_BCH_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETH_Label1
            // 
            this.IR_ETH_Label1.AutoSize = true;
            this.IR_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETH_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_ETH_Label1.Location = new System.Drawing.Point(6, 27);
            this.IR_ETH_Label1.Name = "IR_ETH_Label1";
            this.IR_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_ETH_Label1.TabIndex = 101;
            this.IR_ETH_Label1.Tag = "DCECryptoLabel";
            this.IR_ETH_Label1.Text = "ETH:";
            this.IR_ETH_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XBT_Label1
            // 
            this.IR_XBT_Label1.AutoSize = true;
            this.IR_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XBT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_XBT_Label1.Location = new System.Drawing.Point(6, 7);
            this.IR_XBT_Label1.Name = "IR_XBT_Label1";
            this.IR_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_XBT_Label1.TabIndex = 100;
            this.IR_XBT_Label1.Tag = "DCECryptoLabel";
            this.IR_XBT_Label1.Text = "BTC:";
            this.IR_XBT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XRP_Label3
            // 
            this.IR_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XRP_Label3.Location = new System.Drawing.Point(111, 67);
            this.IR_XRP_Label3.Name = "IR_XRP_Label3";
            this.IR_XRP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_XRP_Label3.TabIndex = 117;
            this.IR_XRP_Label3.Tag = "";
            this.IR_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_CurrencyBox
            // 
            this.IR_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_CurrencyBox.FormattingEnabled = true;
            this.IR_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.IR_CurrencyBox.Location = new System.Drawing.Point(131, 560);
            this.IR_CurrencyBox.Name = "IR_CurrencyBox";
            this.IR_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.IR_CurrencyBox.TabIndex = 36;
            this.IR_CurrencyBox.SelectedIndexChanged += new System.EventHandler(this.IR_CurrencyBox_SelectedIndexChanged);
            // 
            // IR_Reset_Button
            // 
            this.IR_Reset_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_Reset_Button.ForeColor = System.Drawing.Color.Black;
            this.IR_Reset_Button.Location = new System.Drawing.Point(217, 537);
            this.IR_Reset_Button.Name = "IR_Reset_Button";
            this.IR_Reset_Button.Size = new System.Drawing.Size(34, 17);
            this.IR_Reset_Button.TabIndex = 32;
            this.IR_Reset_Button.Text = "Reset";
            this.IR_Reset_Button.UseVisualStyleBackColor = true;
            this.IR_Reset_Button.Click += new System.EventHandler(this.IR_Reset_Button_Click);
            // 
            // SpreadVolumeTitle_Label
            // 
            this.SpreadVolumeTitle_Label.AutoSize = true;
            this.SpreadVolumeTitle_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadVolumeTitle_Label.Location = new System.Drawing.Point(138, 12);
            this.SpreadVolumeTitle_Label.Name = "SpreadVolumeTitle_Label";
            this.SpreadVolumeTitle_Label.Size = new System.Drawing.Size(96, 12);
            this.SpreadVolumeTitle_Label.TabIndex = 25;
            this.SpreadVolumeTitle_Label.Text = "Spread (bps) / Volume";
            // 
            // IR_AvgPrice_Label
            // 
            this.IR_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.IR_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_AvgPrice_Label.Location = new System.Drawing.Point(9, 537);
            this.IR_AvgPrice_Label.Name = "IR_AvgPrice_Label";
            this.IR_AvgPrice_Label.Size = new System.Drawing.Size(202, 17);
            this.IR_AvgPrice_Label.TabIndex = 15;
            // 
            // IR_CryptoComboBox
            // 
            this.IR_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_CryptoComboBox.Location = new System.Drawing.Point(193, 560);
            this.IR_CryptoComboBox.Name = "IR_CryptoComboBox";
            this.IR_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.IR_CryptoComboBox.TabIndex = 14;
            this.IR_CryptoComboBox.DropDown += new System.EventHandler(this.IR_CryptoComboBox_DropDown);
            this.IR_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.IR_CryptoComboBox_SelectedIndexChanged);
            // 
            // IR_NumCoinsTextBox
            // 
            this.IR_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_NumCoinsTextBox.Location = new System.Drawing.Point(58, 560);
            this.IR_NumCoinsTextBox.Name = "IR_NumCoinsTextBox";
            this.IR_NumCoinsTextBox.PromptChar = ' ';
            this.IR_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.IR_NumCoinsTextBox.TabIndex = 13;
            this.IR_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IR_NumCoinsTextBox.ValidatingType = typeof(int);
            this.IR_NumCoinsTextBox.TextChanged += new System.EventHandler(this.IR_NumCoinsTextBox_TextChanged);
            this.IR_NumCoinsTextBox.Enter += new System.EventHandler(this.IR_NumCoinsTextBox_Enter);
            // 
            // IR_BuySellComboBox
            // 
            this.IR_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_BuySellComboBox.FormattingEnabled = true;
            this.IR_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.IR_BuySellComboBox.Location = new System.Drawing.Point(9, 560);
            this.IR_BuySellComboBox.Name = "IR_BuySellComboBox";
            this.IR_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.IR_BuySellComboBox.TabIndex = 12;
            this.IR_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.IR_BuySellComboBox_SelectedIndexChanged);
            // 
            // SGD_Label2
            // 
            this.SGD_Label2.Location = new System.Drawing.Point(0, 0);
            this.SGD_Label2.Name = "SGD_Label2";
            this.SGD_Label2.Size = new System.Drawing.Size(100, 23);
            this.SGD_Label2.TabIndex = 0;
            // 
            // AUD_Label1
            // 
            this.AUD_Label1.Location = new System.Drawing.Point(0, 0);
            this.AUD_Label1.Name = "AUD_Label1";
            this.AUD_Label1.Size = new System.Drawing.Size(100, 23);
            this.AUD_Label1.TabIndex = 0;
            // 
            // EUR_Label1
            // 
            this.EUR_Label1.Location = new System.Drawing.Point(0, 0);
            this.EUR_Label1.Name = "EUR_Label1";
            this.EUR_Label1.Size = new System.Drawing.Size(100, 23);
            this.EUR_Label1.TabIndex = 0;
            // 
            // fiatRefresh_checkBox
            // 
            this.fiatRefresh_checkBox.Location = new System.Drawing.Point(0, 0);
            this.fiatRefresh_checkBox.Name = "fiatRefresh_checkBox";
            this.fiatRefresh_checkBox.Size = new System.Drawing.Size(104, 24);
            this.fiatRefresh_checkBox.TabIndex = 0;
            // 
            // NZD_Label1
            // 
            this.NZD_Label1.Location = new System.Drawing.Point(0, 0);
            this.NZD_Label1.Name = "NZD_Label1";
            this.NZD_Label1.Size = new System.Drawing.Size(100, 23);
            this.NZD_Label1.TabIndex = 0;
            // 
            // SGD_Label1
            // 
            this.SGD_Label1.Location = new System.Drawing.Point(0, 0);
            this.SGD_Label1.Name = "SGD_Label1";
            this.SGD_Label1.Size = new System.Drawing.Size(100, 23);
            this.SGD_Label1.TabIndex = 0;
            // 
            // EUR_Label2
            // 
            this.EUR_Label2.Location = new System.Drawing.Point(0, 0);
            this.EUR_Label2.Name = "EUR_Label2";
            this.EUR_Label2.Size = new System.Drawing.Size(100, 23);
            this.EUR_Label2.TabIndex = 0;
            // 
            // NZD_Label2
            // 
            this.NZD_Label2.Location = new System.Drawing.Point(0, 0);
            this.NZD_Label2.Name = "NZD_Label2";
            this.NZD_Label2.Size = new System.Drawing.Size(100, 23);
            this.NZD_Label2.TabIndex = 0;
            // 
            // USD_Label2
            // 
            this.USD_Label2.Location = new System.Drawing.Point(0, 0);
            this.USD_Label2.Name = "USD_Label2";
            this.USD_Label2.Size = new System.Drawing.Size(100, 23);
            this.USD_Label2.TabIndex = 0;
            // 
            // AUD_Label2
            // 
            this.AUD_Label2.Location = new System.Drawing.Point(0, 0);
            this.AUD_Label2.Name = "AUD_Label2";
            this.AUD_Label2.Size = new System.Drawing.Size(100, 23);
            this.AUD_Label2.TabIndex = 0;
            // 
            // USD_Label1
            // 
            this.USD_Label1.Location = new System.Drawing.Point(0, 0);
            this.USD_Label1.Name = "USD_Label1";
            this.USD_Label1.Size = new System.Drawing.Size(100, 23);
            this.USD_Label1.TabIndex = 0;
            // 
            // IRTickerTT_spread
            // 
            this.IRTickerTT_spread.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IRTickerTT_spread.ToolTipTitle = "Market details";
            // 
            // OTCHelper
            // 
            this.OTCHelper.Controls.Add(this.StepVolume_Label7);
            this.OTCHelper.Controls.Add(this.StepPrice_Label7);
            this.OTCHelper.Controls.Add(this.StepVolume_Label6);
            this.OTCHelper.Controls.Add(this.StepPrice_Label6);
            this.OTCHelper.Controls.Add(this.StepVolume_Label5);
            this.OTCHelper.Controls.Add(this.StepPrice_Label5);
            this.OTCHelper.Controls.Add(this.StepVolume_Label4);
            this.OTCHelper.Controls.Add(this.StepPrice_Label4);
            this.OTCHelper.Controls.Add(this.StepPrice_Label3);
            this.OTCHelper.Controls.Add(this.StepVolume_Label3);
            this.OTCHelper.Controls.Add(this.StepVolume_Label2);
            this.OTCHelper.Controls.Add(this.StepPrice_Label2);
            this.OTCHelper.Controls.Add(this.StepVolume_Label1);
            this.OTCHelper.Controls.Add(this.StepPrice_Label1);
            this.OTCHelper.Controls.Add(this.MarketBuyCrypto_Label);
            this.OTCHelper.Controls.Add(this.CashInput_MaskedTextBox);
            this.OTCHelper.Controls.Add(this.CryptoChooser_ComboBox);
            this.OTCHelper.Location = new System.Drawing.Point(0, 0);
            this.OTCHelper.Name = "OTCHelper";
            this.OTCHelper.Size = new System.Drawing.Size(585, 613);
            this.OTCHelper.TabIndex = 1;
            // 
            // StepVolume_Label7
            // 
            this.StepVolume_Label7.AutoSize = true;
            this.StepVolume_Label7.Location = new System.Drawing.Point(136, 312);
            this.StepVolume_Label7.Name = "StepVolume_Label7";
            this.StepVolume_Label7.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label7.TabIndex = 16;
            this.StepVolume_Label7.Text = "label1";
            // 
            // StepPrice_Label7
            // 
            this.StepPrice_Label7.AutoSize = true;
            this.StepPrice_Label7.Location = new System.Drawing.Point(51, 312);
            this.StepPrice_Label7.Name = "StepPrice_Label7";
            this.StepPrice_Label7.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label7.TabIndex = 15;
            this.StepPrice_Label7.Text = "label1";
            // 
            // StepVolume_Label6
            // 
            this.StepVolume_Label6.AutoSize = true;
            this.StepVolume_Label6.Location = new System.Drawing.Point(136, 274);
            this.StepVolume_Label6.Name = "StepVolume_Label6";
            this.StepVolume_Label6.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label6.TabIndex = 14;
            this.StepVolume_Label6.Text = "label1";
            // 
            // StepPrice_Label6
            // 
            this.StepPrice_Label6.AutoSize = true;
            this.StepPrice_Label6.Location = new System.Drawing.Point(51, 274);
            this.StepPrice_Label6.Name = "StepPrice_Label6";
            this.StepPrice_Label6.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label6.TabIndex = 13;
            this.StepPrice_Label6.Text = "label1";
            // 
            // StepVolume_Label5
            // 
            this.StepVolume_Label5.AutoSize = true;
            this.StepVolume_Label5.Location = new System.Drawing.Point(136, 240);
            this.StepVolume_Label5.Name = "StepVolume_Label5";
            this.StepVolume_Label5.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label5.TabIndex = 12;
            this.StepVolume_Label5.Text = "label1";
            // 
            // StepPrice_Label5
            // 
            this.StepPrice_Label5.AutoSize = true;
            this.StepPrice_Label5.Location = new System.Drawing.Point(51, 240);
            this.StepPrice_Label5.Name = "StepPrice_Label5";
            this.StepPrice_Label5.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label5.TabIndex = 11;
            this.StepPrice_Label5.Text = "label1";
            // 
            // StepVolume_Label4
            // 
            this.StepVolume_Label4.AutoSize = true;
            this.StepVolume_Label4.Location = new System.Drawing.Point(136, 214);
            this.StepVolume_Label4.Name = "StepVolume_Label4";
            this.StepVolume_Label4.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label4.TabIndex = 10;
            this.StepVolume_Label4.Text = "label1";
            // 
            // StepPrice_Label4
            // 
            this.StepPrice_Label4.AutoSize = true;
            this.StepPrice_Label4.Location = new System.Drawing.Point(51, 214);
            this.StepPrice_Label4.Name = "StepPrice_Label4";
            this.StepPrice_Label4.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label4.TabIndex = 9;
            this.StepPrice_Label4.Text = "label1";
            // 
            // StepPrice_Label3
            // 
            this.StepPrice_Label3.AutoSize = true;
            this.StepPrice_Label3.Location = new System.Drawing.Point(51, 182);
            this.StepPrice_Label3.Name = "StepPrice_Label3";
            this.StepPrice_Label3.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label3.TabIndex = 8;
            this.StepPrice_Label3.Text = "label1";
            // 
            // StepVolume_Label3
            // 
            this.StepVolume_Label3.AutoSize = true;
            this.StepVolume_Label3.Location = new System.Drawing.Point(136, 182);
            this.StepVolume_Label3.Name = "StepVolume_Label3";
            this.StepVolume_Label3.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label3.TabIndex = 7;
            this.StepVolume_Label3.Text = "label1";
            // 
            // StepVolume_Label2
            // 
            this.StepVolume_Label2.AutoSize = true;
            this.StepVolume_Label2.Location = new System.Drawing.Point(136, 150);
            this.StepVolume_Label2.Name = "StepVolume_Label2";
            this.StepVolume_Label2.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label2.TabIndex = 6;
            this.StepVolume_Label2.Text = "label1";
            // 
            // StepPrice_Label2
            // 
            this.StepPrice_Label2.AutoSize = true;
            this.StepPrice_Label2.Location = new System.Drawing.Point(51, 150);
            this.StepPrice_Label2.Name = "StepPrice_Label2";
            this.StepPrice_Label2.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label2.TabIndex = 5;
            this.StepPrice_Label2.Text = "label1";
            // 
            // StepVolume_Label1
            // 
            this.StepVolume_Label1.AutoSize = true;
            this.StepVolume_Label1.Location = new System.Drawing.Point(136, 116);
            this.StepVolume_Label1.Name = "StepVolume_Label1";
            this.StepVolume_Label1.Size = new System.Drawing.Size(35, 13);
            this.StepVolume_Label1.TabIndex = 4;
            this.StepVolume_Label1.Text = "label1";
            // 
            // StepPrice_Label1
            // 
            this.StepPrice_Label1.AutoSize = true;
            this.StepPrice_Label1.Location = new System.Drawing.Point(51, 116);
            this.StepPrice_Label1.Name = "StepPrice_Label1";
            this.StepPrice_Label1.Size = new System.Drawing.Size(35, 13);
            this.StepPrice_Label1.TabIndex = 3;
            this.StepPrice_Label1.Text = "label1";
            // 
            // MarketBuyCrypto_Label
            // 
            this.MarketBuyCrypto_Label.AutoSize = true;
            this.MarketBuyCrypto_Label.Location = new System.Drawing.Point(325, 43);
            this.MarketBuyCrypto_Label.Name = "MarketBuyCrypto_Label";
            this.MarketBuyCrypto_Label.Size = new System.Drawing.Size(35, 13);
            this.MarketBuyCrypto_Label.TabIndex = 2;
            this.MarketBuyCrypto_Label.Text = "label1";
            // 
            // CashInput_MaskedTextBox
            // 
            this.CashInput_MaskedTextBox.Location = new System.Drawing.Point(200, 37);
            this.CashInput_MaskedTextBox.Name = "CashInput_MaskedTextBox";
            this.CashInput_MaskedTextBox.Size = new System.Drawing.Size(100, 20);
            this.CashInput_MaskedTextBox.TabIndex = 1;
            // 
            // CryptoChooser_ComboBox
            // 
            this.CryptoChooser_ComboBox.FormattingEnabled = true;
            this.CryptoChooser_ComboBox.Location = new System.Drawing.Point(41, 36);
            this.CryptoChooser_ComboBox.Name = "CryptoChooser_ComboBox";
            this.CryptoChooser_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.CryptoChooser_ComboBox.TabIndex = 0;
            // 
            // IRTickerTT_avgPrice
            // 
            this.IRTickerTT_avgPrice.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IRTickerTT_avgPrice.ToolTipTitle = "Market order estimation";
            // 
            // IRT_notification
            // 
            this.IRT_notification.Icon = ((System.Drawing.Icon)(resources.GetObject("IRT_notification.Icon")));
            this.IRT_notification.Text = "IR Ticker";
            // 
            // IR_PEPE_Label2
            // 
            this.IR_PEPE_Label2.AutoSize = true;
            this.IR_PEPE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_PEPE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_PEPE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_PEPE_Label2.Location = new System.Drawing.Point(70, 447);
            this.IR_PEPE_Label2.Name = "IR_PEPE_Label2";
            this.IR_PEPE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_PEPE_Label2.TabIndex = 204;
            this.IR_PEPE_Label2.Tag = "IR";
            this.IR_PEPE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_PEPE_Label1
            // 
            this.IR_PEPE_Label1.AutoSize = true;
            this.IR_PEPE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_PEPE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_PEPE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_PEPE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_PEPE_Label1.Location = new System.Drawing.Point(6, 447);
            this.IR_PEPE_Label1.Name = "IR_PEPE_Label1";
            this.IR_PEPE_Label1.Size = new System.Drawing.Size(43, 13);
            this.IR_PEPE_Label1.TabIndex = 203;
            this.IR_PEPE_Label1.Tag = "DCECryptoLabel";
            this.IR_PEPE_Label1.Text = "PEPE:";
            this.IR_PEPE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_PEPE_Label3
            // 
            this.IR_PEPE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_PEPE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_PEPE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_PEPE_Label3.Location = new System.Drawing.Point(111, 447);
            this.IR_PEPE_Label3.Name = "IR_PEPE_Label3";
            this.IR_PEPE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_PEPE_Label3.TabIndex = 202;
            this.IR_PEPE_Label3.Tag = "";
            this.IR_PEPE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_TRUMP_Label2
            // 
            this.IR_TRUMP_Label2.AutoSize = true;
            this.IR_TRUMP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_TRUMP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_TRUMP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_TRUMP_Label2.Location = new System.Drawing.Point(70, 547);
            this.IR_TRUMP_Label2.Name = "IR_TRUMP_Label2";
            this.IR_TRUMP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_TRUMP_Label2.TabIndex = 207;
            this.IR_TRUMP_Label2.Tag = "IR";
            this.IR_TRUMP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_TRUMP_Label1
            // 
            this.IR_TRUMP_Label1.AutoSize = true;
            this.IR_TRUMP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_TRUMP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_TRUMP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_TRUMP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_TRUMP_Label1.Location = new System.Drawing.Point(6, 547);
            this.IR_TRUMP_Label1.Name = "IR_TRUMP_Label1";
            this.IR_TRUMP_Label1.Size = new System.Drawing.Size(55, 13);
            this.IR_TRUMP_Label1.TabIndex = 206;
            this.IR_TRUMP_Label1.Tag = "DCECryptoLabel";
            this.IR_TRUMP_Label1.Text = "TRUMP:";
            this.IR_TRUMP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_TRUMP_Label3
            // 
            this.IR_TRUMP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IR_TRUMP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_TRUMP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_TRUMP_Label3.Location = new System.Drawing.Point(111, 547);
            this.IR_TRUMP_Label3.Name = "IR_TRUMP_Label3";
            this.IR_TRUMP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IR_TRUMP_Label3.TabIndex = 205;
            this.IR_TRUMP_Label3.Tag = "";
            this.IR_TRUMP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_PEPE_Label2
            // 
            this.IRSGD_PEPE_Label2.AutoSize = true;
            this.IRSGD_PEPE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_PEPE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_PEPE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_PEPE_Label2.Location = new System.Drawing.Point(60, 327);
            this.IRSGD_PEPE_Label2.Name = "IRSGD_PEPE_Label2";
            this.IRSGD_PEPE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRSGD_PEPE_Label2.TabIndex = 206;
            this.IRSGD_PEPE_Label2.Tag = "IR";
            this.IRSGD_PEPE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRSGD_PEPE_Label3
            // 
            this.IRSGD_PEPE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRSGD_PEPE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_PEPE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_PEPE_Label3.Location = new System.Drawing.Point(115, 327);
            this.IRSGD_PEPE_Label3.Name = "IRSGD_PEPE_Label3";
            this.IRSGD_PEPE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRSGD_PEPE_Label3.TabIndex = 207;
            this.IRSGD_PEPE_Label3.Tag = "";
            this.IRSGD_PEPE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRSGD_PEPE_Label1
            // 
            this.IRSGD_PEPE_Label1.AutoSize = true;
            this.IRSGD_PEPE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRSGD_PEPE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRSGD_PEPE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRSGD_PEPE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRSGD_PEPE_Label1.Location = new System.Drawing.Point(6, 327);
            this.IRSGD_PEPE_Label1.Name = "IRSGD_PEPE_Label1";
            this.IRSGD_PEPE_Label1.Size = new System.Drawing.Size(43, 13);
            this.IRSGD_PEPE_Label1.TabIndex = 205;
            this.IRSGD_PEPE_Label1.Tag = "DCECryptoLabel";
            this.IRSGD_PEPE_Label1.Text = "PEPE:";
            this.IRSGD_PEPE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_PEPE_Label2
            // 
            this.IRUSD_PEPE_Label2.AutoSize = true;
            this.IRUSD_PEPE_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_PEPE_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_PEPE_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_PEPE_Label2.Location = new System.Drawing.Point(60, 447);
            this.IRUSD_PEPE_Label2.Name = "IRUSD_PEPE_Label2";
            this.IRUSD_PEPE_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_PEPE_Label2.TabIndex = 204;
            this.IRUSD_PEPE_Label2.Tag = "IR";
            this.IRUSD_PEPE_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_PEPE_Label1
            // 
            this.IRUSD_PEPE_Label1.AutoSize = true;
            this.IRUSD_PEPE_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_PEPE_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_PEPE_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_PEPE_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_PEPE_Label1.Location = new System.Drawing.Point(6, 447);
            this.IRUSD_PEPE_Label1.Name = "IRUSD_PEPE_Label1";
            this.IRUSD_PEPE_Label1.Size = new System.Drawing.Size(43, 13);
            this.IRUSD_PEPE_Label1.TabIndex = 203;
            this.IRUSD_PEPE_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_PEPE_Label1.Text = "PEPE:";
            this.IRUSD_PEPE_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_PEPE_Label3
            // 
            this.IRUSD_PEPE_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_PEPE_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_PEPE_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_PEPE_Label3.Location = new System.Drawing.Point(121, 447);
            this.IRUSD_PEPE_Label3.Name = "IRUSD_PEPE_Label3";
            this.IRUSD_PEPE_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_PEPE_Label3.TabIndex = 202;
            this.IRUSD_PEPE_Label3.Tag = "";
            this.IRUSD_PEPE_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRUSD_TRUMP_Label2
            // 
            this.IRUSD_TRUMP_Label2.AutoSize = true;
            this.IRUSD_TRUMP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_TRUMP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_TRUMP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_TRUMP_Label2.Location = new System.Drawing.Point(67, 547);
            this.IRUSD_TRUMP_Label2.Name = "IRUSD_TRUMP_Label2";
            this.IRUSD_TRUMP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IRUSD_TRUMP_Label2.TabIndex = 207;
            this.IRUSD_TRUMP_Label2.Tag = "IR";
            this.IRUSD_TRUMP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_TRUMP_Label1
            // 
            this.IRUSD_TRUMP_Label1.AutoSize = true;
            this.IRUSD_TRUMP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_TRUMP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_TRUMP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IRUSD_TRUMP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IRUSD_TRUMP_Label1.Location = new System.Drawing.Point(6, 547);
            this.IRUSD_TRUMP_Label1.Name = "IRUSD_TRUMP_Label1";
            this.IRUSD_TRUMP_Label1.Size = new System.Drawing.Size(55, 13);
            this.IRUSD_TRUMP_Label1.TabIndex = 206;
            this.IRUSD_TRUMP_Label1.Tag = "DCECryptoLabel";
            this.IRUSD_TRUMP_Label1.Text = "TRUMP:";
            this.IRUSD_TRUMP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IRUSD_TRUMP_Label3
            // 
            this.IRUSD_TRUMP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IRUSD_TRUMP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IRUSD_TRUMP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IRUSD_TRUMP_Label3.Location = new System.Drawing.Point(121, 547);
            this.IRUSD_TRUMP_Label3.Name = "IRUSD_TRUMP_Label3";
            this.IRUSD_TRUMP_Label3.Size = new System.Drawing.Size(124, 13);
            this.IRUSD_TRUMP_Label3.TabIndex = 205;
            this.IRUSD_TRUMP_Label3.Tag = "";
            this.IRUSD_TRUMP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IRTicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(584, 841);
            this.Controls.Add(this.Main);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.LoadingPanel);
            this.Controls.Add(this.OTCHelper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 880);
            this.MinimumSize = new System.Drawing.Size(600, 880);
            this.Name = "IRTicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IR Ticker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IRTicker_Closing);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.Settings_coinbase_panel.ResumeLayout(false);
            this.Settings_coinbase_panel.PerformLayout();
            this.IRAccountSettings_groupBox.ResumeLayout(false);
            this.IRAccountSettings_groupBox.PerformLayout();
            this.SlackSettings_groupBox.ResumeLayout(false);
            this.SlackSettings_groupBox.PerformLayout();
            this.LoadingPanel.ResumeLayout(false);
            this.Main.ResumeLayout(false);
            this.IRUSD_GroupBox.ResumeLayout(false);
            this.IR_panel_USD.ResumeLayout(false);
            this.IR_panel_USD.PerformLayout();
            this.IRSGD_GroupBox.ResumeLayout(false);
            this.IR_panel_SGD.ResumeLayout(false);
            this.IR_panel_SGD.PerformLayout();
            this.cryptoFees_groupBox.ResumeLayout(false);
            this.cryptoFees_Panel.ResumeLayout(false);
            this.cryptoFees_Panel.PerformLayout();
            this.BTCM_GroupBox.ResumeLayout(false);
            this.BTCM_GroupBox.PerformLayout();
            this.BTCM_panel.ResumeLayout(false);
            this.BTCM_panel.PerformLayout();
            this.BAR_GroupBox.ResumeLayout(false);
            this.BAR_GroupBox.PerformLayout();
            this.IR_GroupBox.ResumeLayout(false);
            this.IR_GroupBox.PerformLayout();
            this.IR_panel.ResumeLayout(false);
            this.IR_panel.PerformLayout();
            this.OTCHelper.ResumeLayout(false);
            this.OTCHelper.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox refreshFrequencyTextbox;
        private System.Windows.Forms.Label refreshFrequencyLabel;
        private System.Windows.Forms.Panel Settings;
        private System.Windows.Forms.Panel Main;
        private System.Windows.Forms.GroupBox BTCM_GroupBox;
        private System.Windows.Forms.Label BTCM_XBT_Label2;
        private System.Windows.Forms.Label BTCM_ETH_Label1;
        private System.Windows.Forms.Label BTCM_ETH_Label2;
        private System.Windows.Forms.Label BTCM_XBT_Label1;
        private System.Windows.Forms.Label BTCM_BCH_Label2;
        private System.Windows.Forms.Label BTCM_BCH_Label1;
        private System.Windows.Forms.Label BTCM_LTC_Label2;
        private System.Windows.Forms.Label BTCM_LTC_Label1;
        private System.Windows.Forms.GroupBox IR_GroupBox;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Button SettingsOKButton;
        private System.Windows.Forms.Label AUD_Label2;
        private System.Windows.Forms.Label NZD_Label2;
        private System.Windows.Forms.Label EUR_Label2;
        private System.Windows.Forms.Label EUR_Label1;
        private System.Windows.Forms.Label NZD_Label1;
        private System.Windows.Forms.Label AUD_Label1;
        private System.Windows.Forms.Label BTCM_XRP_Label2;
        private System.Windows.Forms.Label BTCM_XRP_Label1;
        private System.Windows.Forms.Label USD_Label2;
        private System.Windows.Forms.Label USD_Label1;
        private System.Windows.Forms.CheckBox fiatRefresh_checkBox;
        private System.Windows.Forms.Panel LoadingPanel;
        private System.Windows.Forms.Label GIFLabel;
        private System.Windows.Forms.Label BTCM_XRP_Label3;
        private System.Windows.Forms.Label BTCM_LTC_Label3;
        private System.Windows.Forms.Label BTCM_BCH_Label3;
        private System.Windows.Forms.Label BTCM_ETH_Label3;
        private System.Windows.Forms.Label BTCM_XBT_Label3;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ComboBox IR_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox IR_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox IR_BuySellComboBox;
        private System.Windows.Forms.ComboBox BTCM_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox BTCM_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox BTCM_BuySellComboBox;
        private System.Windows.Forms.Label BTCM_AvgPrice_Label;
        private System.Windows.Forms.Label IR_AvgPrice_Label;
        private System.Windows.Forms.ToolTip IRTickerTT_spread;
        public System.Windows.Forms.Button Help_Button;
        private System.Windows.Forms.CheckBox ExportSummarised_Checkbox;
        private System.Windows.Forms.Label ExportSummarised_Label;
        private System.Windows.Forms.Panel OTCHelper;
        private System.Windows.Forms.Label StepVolume_Label7;
        private System.Windows.Forms.Label StepPrice_Label7;
        private System.Windows.Forms.Label StepVolume_Label6;
        private System.Windows.Forms.Label StepPrice_Label6;
        private System.Windows.Forms.Label StepVolume_Label5;
        private System.Windows.Forms.Label StepPrice_Label5;
        private System.Windows.Forms.Label StepVolume_Label4;
        private System.Windows.Forms.Label StepPrice_Label4;
        private System.Windows.Forms.Label StepPrice_Label3;
        private System.Windows.Forms.Label StepVolume_Label3;
        private System.Windows.Forms.Label StepVolume_Label2;
        private System.Windows.Forms.Label StepPrice_Label2;
        private System.Windows.Forms.Label StepVolume_Label1;
        private System.Windows.Forms.Label StepPrice_Label1;
        private System.Windows.Forms.Label MarketBuyCrypto_Label;
        private System.Windows.Forms.MaskedTextBox CashInput_MaskedTextBox;
        private System.Windows.Forms.ComboBox CryptoChooser_ComboBox;
        private System.Windows.Forms.Label SpreadVolumeTitle_Label;
        private System.Windows.Forms.Button IR_Reset_Button;
        private System.Windows.Forms.Label BTCM_XLM_Label2;
        private System.Windows.Forms.Label BTCM_XLM_Label3;
        private System.Windows.Forms.Label BTCM_XLM_Label1;
        private System.Windows.Forms.ComboBox IR_CurrencyBox;
        private System.Windows.Forms.Label BTCM_BAT_Label2;
        private System.Windows.Forms.Label BTCM_BAT_Label3;
        private System.Windows.Forms.Label BTCM_BAT_Label1;
        private System.Windows.Forms.Label BTCM_ETC_Label2;
        private System.Windows.Forms.Label BTCM_ETC_Label3;
        private System.Windows.Forms.Label BTCM_ETC_Label1;
        private System.Windows.Forms.Label SGD_Label2;
        private System.Windows.Forms.Label SGD_Label1;
        private System.Windows.Forms.CheckBox Slack_checkBox;
        private System.Windows.Forms.Label Slack_label;
        private System.Windows.Forms.CheckBox flashForm_checkBox;
        private System.Windows.Forms.Label flashForm_label;
        private System.Windows.Forms.TextBox slackToken_textBox;
        private System.Windows.Forms.CheckBox OB_checkBox;
        private System.Windows.Forms.Label OB_label;
        private System.Windows.Forms.Label slackDefaultNameLabel;
        private System.Windows.Forms.TextBox slackDefaultNameTextBox;
        private System.Windows.Forms.CheckBox slackNameChangeCheckBox;
        private System.Windows.Forms.Label slackNameChangeLabel;
        private System.Windows.Forms.MaskedTextBox UITimerFreq_maskedTextBox;
        private System.Windows.Forms.Label UITimerFreq_label;
        private System.Windows.Forms.GroupBox SlackSettings_groupBox;
        private System.Windows.Forms.GroupBox BAR_GroupBox;
        private System.Windows.Forms.Label BAR_XBT_Label2;
        private System.Windows.Forms.Label BAR_XBT_Label3;
        private System.Windows.Forms.Label BAR_XBT_Label1;
        private System.Windows.Forms.CheckBox NegativeSpread_checkBox;
        private System.Windows.Forms.Label NegativeSpread_label;
        private System.Windows.Forms.Label SlackNameCurrency_label;
        private System.Windows.Forms.ComboBox SlackNameFiatCurrency_comboBox;
        private System.Windows.Forms.ComboBox BAR_CurrencyBox;
        private System.Windows.Forms.Label BAR_AvgPrice_Label;
        private System.Windows.Forms.ComboBox BAR_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox BAR_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox BAR_BuySellComboBox;
        private System.Windows.Forms.FolderBrowserDialog spreadHistory_FolderDialog;
        private System.Windows.Forms.TextBox spreadHistoryCustomFolderValue_Textbox;
        private System.Windows.Forms.Label spreadHistoryCustomFolder_label;
        private System.Windows.Forms.ComboBox BTCM_CurrencyBox;
        private System.Windows.Forms.Label SessionStartedRel_label;
        private System.Windows.Forms.Label SessionStartedAbs_label;
        private System.Windows.Forms.Label SessionStart_label;
        private System.Windows.Forms.Label SettingsSeparator_label;
        private System.Windows.Forms.GroupBox IRAccountSettings_groupBox;
        private System.Windows.Forms.ToolTip IRTickerTT_avgPrice;
        private System.Windows.Forms.ToolTip IRTickerTT_generic;
        private System.ComponentModel.BackgroundWorker pollingThread;
        private System.Windows.Forms.ComboBox APIKeys_comboBox;
        public System.Windows.Forms.Button EditKeys_button;
        private System.Windows.Forms.Label IR_APIKey_label;
        private System.Windows.Forms.Label BTCM_LINK_Label2;
        private System.Windows.Forms.Label BTCM_LINK_Label3;
        private System.Windows.Forms.Label BTCM_LINK_Label1;
        private System.Windows.Forms.TextBox TelegramCode_textBox;
        private System.Windows.Forms.Label TelegramCode_label;
        public System.Windows.Forms.Button TGReset_button;
        private System.Windows.Forms.Label TGBot_APIToken_label;
        private System.Windows.Forms.TextBox TelegramBotAPIToken_textBox;
        private System.Windows.Forms.Panel BTCM_panel;
        private System.Windows.Forms.Label BTCM_COMP_Label2;
        private System.Windows.Forms.Label BTCM_COMP_Label3;
        private System.Windows.Forms.Label BTCM_COMP_Label1;
        private System.Windows.Forms.CheckBox TelegramNewMessages_checkBox;
        private System.Windows.Forms.Label TelegramNewMessages_label;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox SlackNameEmojiCrypto_comboBox;
        private System.Windows.Forms.CheckBox TGBot_Enable_checkBox;
        private System.Windows.Forms.Label TGBot_Enable_label;
        private System.Windows.Forms.Label BTCM_USDT_Label1;
        private System.Windows.Forms.Label BTCM_USDT_Label3;
        private System.Windows.Forms.Label BTCM_USDT_Label2;
        private System.Windows.Forms.GroupBox cryptoFees_groupBox;
        private System.Windows.Forms.Panel cryptoFees_Panel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label cryptoFees_LastUpdated_value;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label cryptoFees_ETH_value;
        private System.Windows.Forms.Label cryptoFees_BTC_value;
        private System.Windows.Forms.Label BTC_LastBlock_Time_label;
        private System.Windows.Forms.Label BTC_LastBlock_Time_value;
        private System.Windows.Forms.Button Balance_button;
        public System.Windows.Forms.NotifyIcon IRT_notification;
        private System.Windows.Forms.Panel IR_panel;
        private System.Windows.Forms.Label IR_SAND_Label2;
        private System.Windows.Forms.Label IR_SAND_Label1;
        private System.Windows.Forms.Label IR_SAND_Label3;
        private System.Windows.Forms.Label IR_SOL_Label2;
        private System.Windows.Forms.Label IR_SOL_Label1;
        private System.Windows.Forms.Label IR_SOL_Label3;
        private System.Windows.Forms.Label IR_MANA_Label2;
        private System.Windows.Forms.Label IR_MANA_Label1;
        private System.Windows.Forms.Label IR_MANA_Label3;
        private System.Windows.Forms.Label IR_MATIC_Label2;
        private System.Windows.Forms.Label IR_MATIC_Label1;
        private System.Windows.Forms.Label IR_MATIC_Label3;
        private System.Windows.Forms.Label IR_DOGE_Label2;
        private System.Windows.Forms.Label IR_DOGE_Label1;
        private System.Windows.Forms.Label IR_DOGE_Label3;
        private System.Windows.Forms.Label IR_ADA_Label2;
        private System.Windows.Forms.Label IR_ADA_Label1;
        private System.Windows.Forms.Label IR_ADA_Label3;
        private System.Windows.Forms.Label IR_UNI_Label2;
        private System.Windows.Forms.Label IR_UNI_Label1;
        private System.Windows.Forms.Label IR_UNI_Label3;
        private System.Windows.Forms.Label IR_GRT_Label2;
        private System.Windows.Forms.Label IR_GRT_Label1;
        private System.Windows.Forms.Label IR_GRT_Label3;
        private System.Windows.Forms.Label IR_DOT_Label2;
        private System.Windows.Forms.Label IR_DOT_Label1;
        private System.Windows.Forms.Label IR_DOT_Label3;
        private System.Windows.Forms.Label IR_AAVE_Label2;
        private System.Windows.Forms.Label IR_AAVE_Label1;
        private System.Windows.Forms.Label IR_AAVE_Label3;
        private System.Windows.Forms.Label IR_YFI_Label2;
        private System.Windows.Forms.Label IR_YFI_Label1;
        private System.Windows.Forms.Label IR_YFI_Label3;
        private System.Windows.Forms.Label IR_SNX_Label2;
        private System.Windows.Forms.Label IR_SNX_Label1;
        private System.Windows.Forms.Label IR_SNX_Label3;
        private System.Windows.Forms.Label IR_COMP_Label2;
        private System.Windows.Forms.Label IR_COMP_Label1;
        private System.Windows.Forms.Label IR_COMP_Label3;
        private System.Windows.Forms.Label IR_USDC_Label2;
        private System.Windows.Forms.Label IR_USDC_Label1;
        private System.Windows.Forms.Label IR_USDC_Label3;
        private System.Windows.Forms.Label IR_LINK_Label2;
        private System.Windows.Forms.Label IR_LINK_Label1;
        private System.Windows.Forms.Label IR_LINK_Label3;
        private System.Windows.Forms.Label IR_DAI_Label2;
        private System.Windows.Forms.Label IR_DAI_Label1;
        private System.Windows.Forms.Label IR_DAI_Label3;
        private System.Windows.Forms.Label IR_USDT_Label2;
        private System.Windows.Forms.Label IR_USDT_Label3;
        private System.Windows.Forms.Label IR_USDT_Label1;
        private System.Windows.Forms.Label IR_ETC_Label2;
        private System.Windows.Forms.Label IR_ETC_Label3;
        private System.Windows.Forms.Label IR_ETC_Label1;
        private System.Windows.Forms.Label IR_MKR_Label2;
        private System.Windows.Forms.Label IR_MKR_Label1;
        private System.Windows.Forms.Label IR_MKR_Label3;
        private System.Windows.Forms.Label IR_BAT_Label2;
        private System.Windows.Forms.Label IR_BAT_Label1;
        private System.Windows.Forms.Label IR_BAT_Label3;
        private System.Windows.Forms.Label IR_XLM_Label2;
        private System.Windows.Forms.Label IR_XLM_Label3;
        private System.Windows.Forms.Label IR_XLM_Label1;
        private System.Windows.Forms.Label IR_EOS_Label2;
        private System.Windows.Forms.Label IR_EOS_Label3;
        private System.Windows.Forms.Label IR_EOS_Label1;
        private System.Windows.Forms.Label IR_ZRX_Label2;
        private System.Windows.Forms.Label IR_ZRX_Label1;
        private System.Windows.Forms.Label IR_XRP_Label2;
        private System.Windows.Forms.Label IR_ZRX_Label3;
        private System.Windows.Forms.Label IR_XRP_Label1;
        private System.Windows.Forms.Label IR_XBT_Label2;
        private System.Windows.Forms.Label IR_ETH_Label2;
        private System.Windows.Forms.Label IR_BCH_Label2;
        private System.Windows.Forms.Label IR_LTC_Label2;
        private System.Windows.Forms.Label IR_LTC_Label3;
        private System.Windows.Forms.Label IR_BCH_Label3;
        private System.Windows.Forms.Label IR_ETH_Label3;
        private System.Windows.Forms.Label IR_XBT_Label3;
        private System.Windows.Forms.Label IR_LTC_Label1;
        private System.Windows.Forms.Label IR_BCH_Label1;
        private System.Windows.Forms.Label IR_ETH_Label1;
        private System.Windows.Forms.Label IR_XBT_Label1;
        private System.Windows.Forms.Label IR_XRP_Label3;
        private System.Windows.Forms.GroupBox IRUSD_GroupBox;
        private System.Windows.Forms.Panel IR_panel_USD;
        private System.Windows.Forms.Label IRUSD_SAND_Label2;
        private System.Windows.Forms.Label IRUSD_SAND_Label1;
        private System.Windows.Forms.Label IRUSD_SAND_Label3;
        private System.Windows.Forms.Label IRUSD_SOL_Label2;
        private System.Windows.Forms.Label IRUSD_SOL_Label1;
        private System.Windows.Forms.Label IRUSD_SOL_Label3;
        private System.Windows.Forms.Label IRUSD_MANA_Label2;
        private System.Windows.Forms.Label IRUSD_MANA_Label1;
        private System.Windows.Forms.Label IRUSD_MANA_Label3;
        private System.Windows.Forms.Label IRUSD_MATIC_Label2;
        private System.Windows.Forms.Label IRUSD_MATIC_Label1;
        private System.Windows.Forms.Label IRUSD_MATIC_Label3;
        private System.Windows.Forms.Label IRUSD_DOGE_Label2;
        private System.Windows.Forms.Label IRUSD_DOGE_Label1;
        private System.Windows.Forms.Label IRUSD_DOGE_Label3;
        private System.Windows.Forms.Label IRUSD_ADA_Label2;
        private System.Windows.Forms.Label IRUSD_ADA_Label1;
        private System.Windows.Forms.Label IRUSD_ADA_Label3;
        private System.Windows.Forms.Label IRUSD_UNI_Label2;
        private System.Windows.Forms.Label IRUSD_UNI_Label1;
        private System.Windows.Forms.Label IRUSD_UNI_Label3;
        private System.Windows.Forms.Label IRUSD_GRT_Label2;
        private System.Windows.Forms.Label IRUSD_GRT_Label1;
        private System.Windows.Forms.Label IRUSD_GRT_Label3;
        private System.Windows.Forms.Label IRUSD_DOT_Label2;
        private System.Windows.Forms.Label IRUSD_DOT_Label1;
        private System.Windows.Forms.Label IRUSD_DOT_Label3;
        private System.Windows.Forms.Label IRUSD_AAVE_Label2;
        private System.Windows.Forms.Label IRUSD_AAVE_Label1;
        private System.Windows.Forms.Label IRUSD_AAVE_Label3;
        private System.Windows.Forms.Label IRUSD_YFI_Label2;
        private System.Windows.Forms.Label IRUSD_YFI_Label1;
        private System.Windows.Forms.Label IRUSD_YFI_Label3;
        private System.Windows.Forms.Label IRUSD_SNX_Label2;
        private System.Windows.Forms.Label IRUSD_SNX_Label1;
        private System.Windows.Forms.Label IRUSD_SNX_Label3;
        private System.Windows.Forms.Label IRUSD_COMP_Label2;
        private System.Windows.Forms.Label IRUSD_COMP_Label1;
        private System.Windows.Forms.Label IRUSD_COMP_Label3;
        private System.Windows.Forms.Label IRUSD_USDC_Label2;
        private System.Windows.Forms.Label IRUSD_USDC_Label1;
        private System.Windows.Forms.Label IRUSD_USDC_Label3;
        private System.Windows.Forms.Label IRUSD_LINK_Label2;
        private System.Windows.Forms.Label IRUSD_LINK_Label1;
        private System.Windows.Forms.Label IRUSD_LINK_Label3;
        private System.Windows.Forms.Label IRUSD_DAI_Label2;
        private System.Windows.Forms.Label IRUSD_DAI_Label1;
        private System.Windows.Forms.Label IRUSD_DAI_Label3;
        private System.Windows.Forms.Label IRUSD_USDT_Label2;
        private System.Windows.Forms.Label IRUSD_USDT_Label3;
        private System.Windows.Forms.Label IRUSD_USDT_Label1;
        private System.Windows.Forms.Label IRUSD_ETC_Label2;
        private System.Windows.Forms.Label IRUSD_ETC_Label3;
        private System.Windows.Forms.Label IRUSD_ETC_Label1;
        private System.Windows.Forms.Label IRUSD_MKR_Label2;
        private System.Windows.Forms.Label IRUSD_MKR_Label1;
        private System.Windows.Forms.Label IRUSD_MKR_Label3;
        private System.Windows.Forms.Label IRUSD_BAT_Label2;
        private System.Windows.Forms.Label IRUSD_BAT_Label1;
        private System.Windows.Forms.Label IRUSD_BAT_Label3;
        private System.Windows.Forms.Label IRUSD_XLM_Label2;
        private System.Windows.Forms.Label IRUSD_XLM_Label3;
        private System.Windows.Forms.Label IRUSD_XLM_Label1;
        private System.Windows.Forms.Label IRUSD_EOS_Label2;
        private System.Windows.Forms.Label IRUSD_EOS_Label3;
        private System.Windows.Forms.Label IRUSD_EOS_Label1;
        private System.Windows.Forms.Label IRUSD_ZRX_Label2;
        private System.Windows.Forms.Label IRUSD_ZRX_Label1;
        private System.Windows.Forms.Label IRUSD_XRP_Label2;
        private System.Windows.Forms.Label IRUSD_ZRX_Label3;
        private System.Windows.Forms.Label IRUSD_XRP_Label1;
        private System.Windows.Forms.Label IRUSD_XBT_Label2;
        private System.Windows.Forms.Label IRUSD_ETH_Label2;
        private System.Windows.Forms.Label IRUSD_BCH_Label2;
        private System.Windows.Forms.Label IRUSD_LTC_Label2;
        private System.Windows.Forms.Label IRUSD_LTC_Label3;
        private System.Windows.Forms.Label IRUSD_BCH_Label3;
        private System.Windows.Forms.Label IRUSD_ETH_Label3;
        private System.Windows.Forms.Label IRUSD_XBT_Label3;
        private System.Windows.Forms.Label IRUSD_LTC_Label1;
        private System.Windows.Forms.Label IRUSD_BCH_Label1;
        private System.Windows.Forms.Label IRUSD_ETH_Label1;
        private System.Windows.Forms.Label IRUSD_XBT_Label1;
        private System.Windows.Forms.Label IRUSD_XRP_Label3;
        private System.Windows.Forms.GroupBox IRSGD_GroupBox;
        private System.Windows.Forms.Panel IR_panel_SGD;
        private System.Windows.Forms.Label IRSGD_SAND_Label2;
        private System.Windows.Forms.Label IRSGD_SAND_Label1;
        private System.Windows.Forms.Label IRSGD_SAND_Label3;
        private System.Windows.Forms.Label IRSGD_SOL_Label2;
        private System.Windows.Forms.Label IRSGD_SOL_Label1;
        private System.Windows.Forms.Label IRSGD_SOL_Label3;
        private System.Windows.Forms.Label IRSGD_MANA_Label2;
        private System.Windows.Forms.Label IRSGD_MANA_Label1;
        private System.Windows.Forms.Label IRSGD_MANA_Label3;
        private System.Windows.Forms.Label IRSGD_MATIC_Label2;
        private System.Windows.Forms.Label IRSGD_MATIC_Label1;
        private System.Windows.Forms.Label IRSGD_MATIC_Label3;
        private System.Windows.Forms.Label IRSGD_DOGE_Label2;
        private System.Windows.Forms.Label IRSGD_DOGE_Label1;
        private System.Windows.Forms.Label IRSGD_DOGE_Label3;
        private System.Windows.Forms.Label IRSGD_ADA_Label2;
        private System.Windows.Forms.Label IRSGD_ADA_Label1;
        private System.Windows.Forms.Label IRSGD_ADA_Label3;
        private System.Windows.Forms.Label IRSGD_UNI_Label2;
        private System.Windows.Forms.Label IRSGD_UNI_Label1;
        private System.Windows.Forms.Label IRSGD_UNI_Label3;
        private System.Windows.Forms.Label IRSGD_GRT_Label2;
        private System.Windows.Forms.Label IRSGD_GRT_Label1;
        private System.Windows.Forms.Label IRSGD_GRT_Label3;
        private System.Windows.Forms.Label IRSGD_DOT_Label2;
        private System.Windows.Forms.Label IRSGD_DOT_Label1;
        private System.Windows.Forms.Label IRSGD_DOT_Label3;
        private System.Windows.Forms.Label IRSGD_USDC_Label2;
        private System.Windows.Forms.Label IRSGD_USDC_Label1;
        private System.Windows.Forms.Label IRSGD_USDC_Label3;
        private System.Windows.Forms.Label IRSGD_LINK_Label2;
        private System.Windows.Forms.Label IRSGD_LINK_Label1;
        private System.Windows.Forms.Label IRSGD_LINK_Label3;
        private System.Windows.Forms.Label IRSGD_USDT_Label2;
        private System.Windows.Forms.Label IRSGD_USDT_Label3;
        private System.Windows.Forms.Label IRSGD_USDT_Label1;
        private System.Windows.Forms.Label IRSGD_ETC_Label2;
        private System.Windows.Forms.Label IRSGD_ETC_Label3;
        private System.Windows.Forms.Label IRSGD_ETC_Label1;
        private System.Windows.Forms.Label IRSGD_XLM_Label2;
        private System.Windows.Forms.Label IRSGD_XLM_Label3;
        private System.Windows.Forms.Label IRSGD_XLM_Label1;
        private System.Windows.Forms.Label IRSGD_XRP_Label2;
        private System.Windows.Forms.Label IRSGD_XRP_Label1;
        private System.Windows.Forms.Label IRSGD_XBT_Label2;
        private System.Windows.Forms.Label IRSGD_ETH_Label2;
        private System.Windows.Forms.Label IRSGD_BCH_Label2;
        private System.Windows.Forms.Label IRSGD_LTC_Label2;
        private System.Windows.Forms.Label IRSGD_LTC_Label3;
        private System.Windows.Forms.Label IRSGD_BCH_Label3;
        private System.Windows.Forms.Label IRSGD_ETH_Label3;
        private System.Windows.Forms.Label IRSGD_XBT_Label3;
        private System.Windows.Forms.Label IRSGD_LTC_Label1;
        private System.Windows.Forms.Label IRSGD_BCH_Label1;
        private System.Windows.Forms.Label IRSGD_ETH_Label1;
        private System.Windows.Forms.Label IRSGD_XBT_Label1;
        private System.Windows.Forms.Label IRSGD_XRP_Label3;
        private System.Windows.Forms.Label BTCM_AAVE_Label2;
        private System.Windows.Forms.Label BTCM_AAVE_Label3;
        private System.Windows.Forms.Label BTCM_AAVE_Label1;
        private System.Windows.Forms.Label BTCM_USDC_Label2;
        private System.Windows.Forms.Label BTCM_USDC_Label3;
        private System.Windows.Forms.Label BTCM_USDC_Label1;
        private System.Windows.Forms.Label BTCM_MANA_Label2;
        private System.Windows.Forms.Label BTCM_MANA_Label3;
        private System.Windows.Forms.Label BTCM_MANA_Label1;
        private System.Windows.Forms.Label BTCM_SAND_Label1;
        private System.Windows.Forms.Label BTCM_UNI_Label2;
        private System.Windows.Forms.Label BTCM_SAND_Label3;
        private System.Windows.Forms.Label BTCM_UNI_Label3;
        private System.Windows.Forms.Label BTCM_SAND_Label2;
        private System.Windows.Forms.Label BTCM_UNI_Label1;
        private System.Windows.Forms.Label BTCM_ADA_Label2;
        private System.Windows.Forms.Label BTCM_ADA_Label3;
        private System.Windows.Forms.Label BTCM_ADA_Label1;
        private System.Windows.Forms.Label BTCM_SOL_Label1;
        private System.Windows.Forms.Label BTCM_DOT_Label2;
        private System.Windows.Forms.Label BTCM_SOL_Label3;
        private System.Windows.Forms.Label BTCM_DOT_Label3;
        private System.Windows.Forms.Label BTCM_SOL_Label2;
        private System.Windows.Forms.Label BTCM_DOT_Label1;
        private System.Windows.Forms.Label IR_SHIB_Label2;
        private System.Windows.Forms.Label IR_SHIB_Label1;
        private System.Windows.Forms.Label IR_SHIB_Label3;
        private System.Windows.Forms.Label IRUSD_SHIB_Label2;
        private System.Windows.Forms.Label IRUSD_SHIB_Label1;
        private System.Windows.Forms.Label IRUSD_SHIB_Label3;
        private System.Windows.Forms.Label IR_TRX_Label2;
        private System.Windows.Forms.Label IR_TRX_Label1;
        private System.Windows.Forms.Label IR_TRX_Label3;
        private System.Windows.Forms.Label IRUSD_TRX_Label2;
        private System.Windows.Forms.Label IRUSD_TRX_Label1;
        private System.Windows.Forms.Label IRUSD_TRX_Label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox Settings_coinbase_panel;
        private System.Windows.Forms.TextBox Settings_coinbase_secret_textbox;
        private System.Windows.Forms.Label Settings_coinbase_secret_label;
        private System.Windows.Forms.Label Settings_coinbase_apikey_label;
        private System.Windows.Forms.TextBox Settings_coinbase_apikey_textbox;
        private System.Windows.Forms.TextBox Settings_coinbase_passphrase_textbox;
        private System.Windows.Forms.Label Settings_coinbase_passphrase_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TradingVenues_comboBox;
        private System.Windows.Forms.Label IRSGD_RENDER_Label2;
        private System.Windows.Forms.Label IRSGD_RENDER_Label1;
        private System.Windows.Forms.Label IRSGD_RENDER_Label3;
        private System.Windows.Forms.Label IRSGD_RLUSD_Label2;
        private System.Windows.Forms.Label IRSGD_RLUSD_Label3;
        private System.Windows.Forms.Label IRSGD_RLUSD_Label1;
        private System.Windows.Forms.Label IRSGD_WIF_Label2;
        private System.Windows.Forms.Label IRSGD_WIF_Label1;
        private System.Windows.Forms.Label IRSGD_WIF_Label3;
        private System.Windows.Forms.Label IRSGD_SHIB_Label2;
        private System.Windows.Forms.Label IRSGD_SHIB_Label1;
        private System.Windows.Forms.Label IRSGD_SHIB_Label3;
        private System.Windows.Forms.Label IRSGD_TRX_Label2;
        private System.Windows.Forms.Label IRSGD_TRX_Label1;
        private System.Windows.Forms.Label IRSGD_TRX_Label3;
        private System.Windows.Forms.Label IR_WIF_Label2;
        private System.Windows.Forms.Label IR_WIF_Label1;
        private System.Windows.Forms.Label IR_WIF_Label3;
        private System.Windows.Forms.Label IR_RENDER_Label2;
        private System.Windows.Forms.Label IR_RENDER_Label1;
        private System.Windows.Forms.Label IR_RENDER_Label3;
        private System.Windows.Forms.Label IRUSD_WIF_Label2;
        private System.Windows.Forms.Label IRUSD_WIF_Label1;
        private System.Windows.Forms.Label IRUSD_WIF_Label3;
        private System.Windows.Forms.Label IRUSD_RENDER_Label2;
        private System.Windows.Forms.Label IRUSD_RENDER_Label1;
        private System.Windows.Forms.Label IRUSD_RENDER_Label3;
        private System.Windows.Forms.Label IR_PEPE_Label2;
        private System.Windows.Forms.Label IR_PEPE_Label1;
        private System.Windows.Forms.Label IR_PEPE_Label3;
        private System.Windows.Forms.Label IRSGD_PEPE_Label2;
        private System.Windows.Forms.Label IRSGD_PEPE_Label3;
        private System.Windows.Forms.Label IRSGD_PEPE_Label1;
        private System.Windows.Forms.Label IR_TRUMP_Label2;
        private System.Windows.Forms.Label IR_TRUMP_Label1;
        private System.Windows.Forms.Label IR_TRUMP_Label3;
        private System.Windows.Forms.Label IRUSD_PEPE_Label2;
        private System.Windows.Forms.Label IRUSD_PEPE_Label1;
        private System.Windows.Forms.Label IRUSD_PEPE_Label3;
        private System.Windows.Forms.Label IRUSD_TRUMP_Label2;
        private System.Windows.Forms.Label IRUSD_TRUMP_Label1;
        private System.Windows.Forms.Label IRUSD_TRUMP_Label3;
    }
}

