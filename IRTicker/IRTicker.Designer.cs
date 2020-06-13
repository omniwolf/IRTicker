namespace IRTicker {
    partial class IRTicker {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRTicker));
            this.refreshFrequencyTextbox = new System.Windows.Forms.MaskedTextBox();
            this.refreshFrequencyLabel = new System.Windows.Forms.Label();
            this.pollingThread = new System.ComponentModel.BackgroundWorker();
            this.Settings = new System.Windows.Forms.Panel();
            this.spreadHistoryCustomFolderValue_Textbox = new System.Windows.Forms.TextBox();
            this.spreadHistoryCustomFolder_label = new System.Windows.Forms.Label();
            this.NegativeSpread_checkBox = new System.Windows.Forms.CheckBox();
            this.NegativeSpread_label = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SlackNameCurrency_label = new System.Windows.Forms.Label();
            this.SlackNameCurrency_comboBox = new System.Windows.Forms.ComboBox();
            this.Slack_label = new System.Windows.Forms.Label();
            this.Slack_checkBox = new System.Windows.Forms.CheckBox();
            this.slackToken_textBox = new System.Windows.Forms.TextBox();
            this.slackDefaultNameLabel = new System.Windows.Forms.Label();
            this.slackNameChangeLabel = new System.Windows.Forms.Label();
            this.slackDefaultNameTextBox = new System.Windows.Forms.TextBox();
            this.slackNameChangeCheckBox = new System.Windows.Forms.CheckBox();
            this.UITimerFreq_maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.UITimerFreq_label = new System.Windows.Forms.Label();
            this.OB_checkBox = new System.Windows.Forms.CheckBox();
            this.OB_label = new System.Windows.Forms.Label();
            this.flashForm_checkBox = new System.Windows.Forms.CheckBox();
            this.flashForm_label = new System.Windows.Forms.Label();
            this.ExportSummarised_Checkbox = new System.Windows.Forms.CheckBox();
            this.ExportSummarised_Label = new System.Windows.Forms.Label();
            this.Help_Button = new System.Windows.Forms.Button();
            this.EnableGDAXLevel3_CheckBox = new System.Windows.Forms.CheckBox();
            this.EnableGDAXLevel3 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.SettingsOKButton = new System.Windows.Forms.Button();
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.GIFLabel = new System.Windows.Forms.Label();
            this.Main = new System.Windows.Forms.Panel();
            this.fiatRefresh_checkBox = new System.Windows.Forms.CheckBox();
            this.BTCM_GroupBox = new System.Windows.Forms.GroupBox();
            this.BTCM_BSV_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BSV_Label3 = new System.Windows.Forms.Label();
            this.BTCM_BSV_Label1 = new System.Windows.Forms.Label();
            this.BTCM_ETC_Label2 = new System.Windows.Forms.Label();
            this.BTCM_ETC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETC_Label1 = new System.Windows.Forms.Label();
            this.BTCM_BAT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BAT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_BAT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_GNT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_GNT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_GNT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_XLM_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XLM_Label3 = new System.Windows.Forms.Label();
            this.BTCM_XLM_Label1 = new System.Windows.Forms.Label();
            this.BTCM_OMG_Label2 = new System.Windows.Forms.Label();
            this.BTCM_OMG_Label3 = new System.Windows.Forms.Label();
            this.BTCM_OMG_Label1 = new System.Windows.Forms.Label();
            this.BTCM_AvgPrice_Label = new System.Windows.Forms.Label();
            this.BTCM_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.BTCM_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.BTCM_XRP_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.BTCM_XBT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label2 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label3 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label3 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label1 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label1 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label1 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label1 = new System.Windows.Forms.Label();
            this.BAR_GroupBox = new System.Windows.Forms.GroupBox();
            this.BAR_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.BAR_XBT_Label2 = new System.Windows.Forms.Label();
            this.BAR_XBT_Label3 = new System.Windows.Forms.Label();
            this.BAR_AvgPrice_Label = new System.Windows.Forms.Label();
            this.BAR_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.BAR_XBT_Label1 = new System.Windows.Forms.Label();
            this.BAR_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.BAR_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.BFX_GroupBox = new System.Windows.Forms.GroupBox();
            this.BFX_USDT_Label2 = new System.Windows.Forms.Label();
            this.BFX_USDT_Label3 = new System.Windows.Forms.Label();
            this.BFX_USDT_Label1 = new System.Windows.Forms.Label();
            this.BFX_BSV_Label2 = new System.Windows.Forms.Label();
            this.BFX_BSV_Label3 = new System.Windows.Forms.Label();
            this.BFX_BSV_Label1 = new System.Windows.Forms.Label();
            this.BFX_ETC_Label2 = new System.Windows.Forms.Label();
            this.BFX_ETC_Label3 = new System.Windows.Forms.Label();
            this.BFX_ETC_Label1 = new System.Windows.Forms.Label();
            this.BFX_BAT_Label2 = new System.Windows.Forms.Label();
            this.BFX_BAT_Label3 = new System.Windows.Forms.Label();
            this.BFX_BAT_Label1 = new System.Windows.Forms.Label();
            this.BFX_GNT_Label2 = new System.Windows.Forms.Label();
            this.BFX_GNT_Label1 = new System.Windows.Forms.Label();
            this.BFX_REP_Label2 = new System.Windows.Forms.Label();
            this.BFX_REP_Label3 = new System.Windows.Forms.Label();
            this.BFX_GNT_Label3 = new System.Windows.Forms.Label();
            this.BFX_REP_Label1 = new System.Windows.Forms.Label();
            this.BFX_XLM_Label2 = new System.Windows.Forms.Label();
            this.BFX_XLM_Label3 = new System.Windows.Forms.Label();
            this.BFX_XLM_Label1 = new System.Windows.Forms.Label();
            this.BFX_EOS_Label2 = new System.Windows.Forms.Label();
            this.BFX_EOS_Label3 = new System.Windows.Forms.Label();
            this.BFX_EOS_Label1 = new System.Windows.Forms.Label();
            this.BFX_ZRX_Label2 = new System.Windows.Forms.Label();
            this.BFX_ZRX_Label1 = new System.Windows.Forms.Label();
            this.BFX_OMG_Label2 = new System.Windows.Forms.Label();
            this.BFX_OMG_Label3 = new System.Windows.Forms.Label();
            this.BFX_ZRX_Label3 = new System.Windows.Forms.Label();
            this.BFX_OMG_Label1 = new System.Windows.Forms.Label();
            this.BFX_XRP_Label2 = new System.Windows.Forms.Label();
            this.BFX_XRP_Label3 = new System.Windows.Forms.Label();
            this.BFX_XRP_Label1 = new System.Windows.Forms.Label();
            this.BFX_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.BFX_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.BFX_XBT_Label2 = new System.Windows.Forms.Label();
            this.BFX_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.BFX_ETH_Label2 = new System.Windows.Forms.Label();
            this.BFX_BCH_Label2 = new System.Windows.Forms.Label();
            this.BFX_LTC_Label2 = new System.Windows.Forms.Label();
            this.BFX_BCH_Label3 = new System.Windows.Forms.Label();
            this.BFX_LTC_Label3 = new System.Windows.Forms.Label();
            this.BFX_ETH_Label3 = new System.Windows.Forms.Label();
            this.BFX_XBT_Label3 = new System.Windows.Forms.Label();
            this.BFX_LTC_Label1 = new System.Windows.Forms.Label();
            this.BFX_BCH_Label1 = new System.Windows.Forms.Label();
            this.BFX_ETH_Label1 = new System.Windows.Forms.Label();
            this.BFX_XBT_Label1 = new System.Windows.Forms.Label();
            this.BFX_AvgPrice_Label = new System.Windows.Forms.Label();
            this.fiat_GroupBox = new System.Windows.Forms.GroupBox();
            this.SGD_Label2 = new System.Windows.Forms.Label();
            this.SGD_Label1 = new System.Windows.Forms.Label();
            this.USD_Label2 = new System.Windows.Forms.Label();
            this.USD_Label1 = new System.Windows.Forms.Label();
            this.AUD_Label2 = new System.Windows.Forms.Label();
            this.NZD_Label2 = new System.Windows.Forms.Label();
            this.EUR_Label2 = new System.Windows.Forms.Label();
            this.EUR_Label1 = new System.Windows.Forms.Label();
            this.NZD_Label1 = new System.Windows.Forms.Label();
            this.AUD_Label1 = new System.Windows.Forms.Label();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.IR_GroupBox = new System.Windows.Forms.GroupBox();
            this.IR_USDT_Label2 = new System.Windows.Forms.Label();
            this.IR_USDT_Label3 = new System.Windows.Forms.Label();
            this.IR_USDT_Label1 = new System.Windows.Forms.Label();
            this.IR_BSV_Label2 = new System.Windows.Forms.Label();
            this.IR_BSV_Label3 = new System.Windows.Forms.Label();
            this.IR_BSV_Label1 = new System.Windows.Forms.Label();
            this.IR_ETC_Label2 = new System.Windows.Forms.Label();
            this.IR_ETC_Label3 = new System.Windows.Forms.Label();
            this.IR_ETC_Label1 = new System.Windows.Forms.Label();
            this.IR_GNT_Label2 = new System.Windows.Forms.Label();
            this.IR_GNT_Label1 = new System.Windows.Forms.Label();
            this.IR_GNT_Label3 = new System.Windows.Forms.Label();
            this.IR_REP_Label2 = new System.Windows.Forms.Label();
            this.IR_REP_Label1 = new System.Windows.Forms.Label();
            this.IR_REP_Label3 = new System.Windows.Forms.Label();
            this.IR_BAT_Label2 = new System.Windows.Forms.Label();
            this.IR_BAT_Label1 = new System.Windows.Forms.Label();
            this.IR_BAT_Label3 = new System.Windows.Forms.Label();
            this.IR_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.IR_XLM_Label2 = new System.Windows.Forms.Label();
            this.IR_XLM_Label3 = new System.Windows.Forms.Label();
            this.IR_XLM_Label1 = new System.Windows.Forms.Label();
            this.IR_Reset_Button = new System.Windows.Forms.Button();
            this.IR_EOS_Label2 = new System.Windows.Forms.Label();
            this.IR_EOS_Label3 = new System.Windows.Forms.Label();
            this.IR_EOS_Label1 = new System.Windows.Forms.Label();
            this.SpreadVolumeTitle_Label = new System.Windows.Forms.Label();
            this.IR_ZRX_Label2 = new System.Windows.Forms.Label();
            this.IR_ZRX_Label1 = new System.Windows.Forms.Label();
            this.IR_OMG_Label2 = new System.Windows.Forms.Label();
            this.IR_OMG_Label3 = new System.Windows.Forms.Label();
            this.IR_OMG_Label1 = new System.Windows.Forms.Label();
            this.IR_XRP_Label2 = new System.Windows.Forms.Label();
            this.IR_ZRX_Label3 = new System.Windows.Forms.Label();
            this.IR_XRP_Label1 = new System.Windows.Forms.Label();
            this.IR_AvgPrice_Label = new System.Windows.Forms.Label();
            this.IR_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.IR_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.IR_BuySellComboBox = new System.Windows.Forms.ComboBox();
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
            this.GDAX_GroupBox = new System.Windows.Forms.GroupBox();
            this.GDAX_ETC_Label2 = new System.Windows.Forms.Label();
            this.GDAX_ETC_Label3 = new System.Windows.Forms.Label();
            this.GDAX_ETC_Label1 = new System.Windows.Forms.Label();
            this.GDAX_REP_Label2 = new System.Windows.Forms.Label();
            this.GDAX_REP_Label3 = new System.Windows.Forms.Label();
            this.GDAX_REP_Label1 = new System.Windows.Forms.Label();
            this.GDAX_XLM_Label2 = new System.Windows.Forms.Label();
            this.GDAX_XLM_Label3 = new System.Windows.Forms.Label();
            this.GDAX_XLM_Label1 = new System.Windows.Forms.Label();
            this.GDAX_XRP_Label2 = new System.Windows.Forms.Label();
            this.GDAX_XRP_Label3 = new System.Windows.Forms.Label();
            this.GDAX_XRP_Label1 = new System.Windows.Forms.Label();
            this.GDAX_ZRX_Label2 = new System.Windows.Forms.Label();
            this.GDAX_ZRX_Label3 = new System.Windows.Forms.Label();
            this.GDAX_ZRX_Label1 = new System.Windows.Forms.Label();
            this.GDAX_AvgPrice_Label = new System.Windows.Forms.Label();
            this.GDAX_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.GDAX_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.GDAX_XBT_Label2 = new System.Windows.Forms.Label();
            this.GDAX_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.GDAX_ETH_Label2 = new System.Windows.Forms.Label();
            this.GDAX_BCH_Label2 = new System.Windows.Forms.Label();
            this.GDAX_LTC_Label2 = new System.Windows.Forms.Label();
            this.GDAX_LTC_Label3 = new System.Windows.Forms.Label();
            this.GDAX_BCH_Label3 = new System.Windows.Forms.Label();
            this.GDAX_ETH_Label3 = new System.Windows.Forms.Label();
            this.GDAX_XBT_Label3 = new System.Windows.Forms.Label();
            this.GDAX_LTC_Label1 = new System.Windows.Forms.Label();
            this.GDAX_BCH_Label1 = new System.Windows.Forms.Label();
            this.GDAX_ETH_Label1 = new System.Windows.Forms.Label();
            this.GDAX_XBT_Label1 = new System.Windows.Forms.Label();
            this.IRTickerTT = new System.Windows.Forms.ToolTip(this.components);
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
            this.BlinkStickBW = new System.ComponentModel.BackgroundWorker();
            this.BlinkStickWhite_Thread = new System.ComponentModel.BackgroundWorker();
            this.spreadHistory_FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.GDAX_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.BFX_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.BTCM_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.SessionStart_label = new System.Windows.Forms.Label();
            this.SessionStartedAbs_label = new System.Windows.Forms.Label();
            this.SessionStartedRel_label = new System.Windows.Forms.Label();
            this.SettingsSeparator_label = new System.Windows.Forms.Label();
            this.Settings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.LoadingPanel.SuspendLayout();
            this.Main.SuspendLayout();
            this.BTCM_GroupBox.SuspendLayout();
            this.BAR_GroupBox.SuspendLayout();
            this.BFX_GroupBox.SuspendLayout();
            this.fiat_GroupBox.SuspendLayout();
            this.IR_GroupBox.SuspendLayout();
            this.GDAX_GroupBox.SuspendLayout();
            this.OTCHelper.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshFrequencyTextbox
            // 
            this.refreshFrequencyTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshFrequencyTextbox.Location = new System.Drawing.Point(437, 50);
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
            this.refreshFrequencyLabel.Location = new System.Drawing.Point(73, 57);
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
            this.Settings.Controls.Add(this.SettingsSeparator_label);
            this.Settings.Controls.Add(this.SessionStartedRel_label);
            this.Settings.Controls.Add(this.SessionStartedAbs_label);
            this.Settings.Controls.Add(this.SessionStart_label);
            this.Settings.Controls.Add(this.spreadHistoryCustomFolderValue_Textbox);
            this.Settings.Controls.Add(this.spreadHistoryCustomFolder_label);
            this.Settings.Controls.Add(this.NegativeSpread_checkBox);
            this.Settings.Controls.Add(this.NegativeSpread_label);
            this.Settings.Controls.Add(this.groupBox1);
            this.Settings.Controls.Add(this.UITimerFreq_maskedTextBox);
            this.Settings.Controls.Add(this.UITimerFreq_label);
            this.Settings.Controls.Add(this.OB_checkBox);
            this.Settings.Controls.Add(this.OB_label);
            this.Settings.Controls.Add(this.flashForm_checkBox);
            this.Settings.Controls.Add(this.flashForm_label);
            this.Settings.Controls.Add(this.ExportSummarised_Checkbox);
            this.Settings.Controls.Add(this.ExportSummarised_Label);
            this.Settings.Controls.Add(this.Help_Button);
            this.Settings.Controls.Add(this.EnableGDAXLevel3_CheckBox);
            this.Settings.Controls.Add(this.EnableGDAXLevel3);
            this.Settings.Controls.Add(this.VersionLabel);
            this.Settings.Controls.Add(this.SettingsOKButton);
            this.Settings.Controls.Add(this.refreshFrequencyLabel);
            this.Settings.Controls.Add(this.refreshFrequencyTextbox);
            this.Settings.Location = new System.Drawing.Point(0, 0);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(585, 843);
            this.Settings.TabIndex = 4;
            // 
            // spreadHistoryCustomFolderValue_Textbox
            // 
            this.spreadHistoryCustomFolderValue_Textbox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.spreadHistoryCustomFolderValue_Textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spreadHistoryCustomFolderValue_Textbox.Location = new System.Drawing.Point(226, 244);
            this.spreadHistoryCustomFolderValue_Textbox.Name = "spreadHistoryCustomFolderValue_Textbox";
            this.spreadHistoryCustomFolderValue_Textbox.ReadOnly = true;
            this.spreadHistoryCustomFolderValue_Textbox.Size = new System.Drawing.Size(279, 20);
            this.spreadHistoryCustomFolderValue_Textbox.TabIndex = 37;
            this.spreadHistoryCustomFolderValue_Textbox.Text = "G:\\My Drive\\IR\\IRTicker\\Spread history data\\";
            this.IRTickerTT.SetToolTip(this.spreadHistoryCustomFolderValue_Textbox, "Default is G:\\My Drive\\IR\\IRTicker\\Spread history data\\");
            this.spreadHistoryCustomFolderValue_Textbox.Click += new System.EventHandler(this.spreadHistoryCustomFolderValue_Textbox_Click);
            // 
            // spreadHistoryCustomFolder_label
            // 
            this.spreadHistoryCustomFolder_label.AccessibleName = "";
            this.spreadHistoryCustomFolder_label.AutoSize = true;
            this.spreadHistoryCustomFolder_label.Location = new System.Drawing.Point(73, 244);
            this.spreadHistoryCustomFolder_label.Name = "spreadHistoryCustomFolder_label";
            this.spreadHistoryCustomFolder_label.Size = new System.Drawing.Size(144, 26);
            this.spreadHistoryCustomFolder_label.TabIndex = 35;
            this.spreadHistoryCustomFolder_label.Text = "Choose a custom base folder\r\nto export spread data to";
            this.IRTickerTT.SetToolTip(this.spreadHistoryCustomFolder_label, "A folder under this one will be created to store the CSVs in, it will be named yo" +
        "ur current logged in username");
            // 
            // NegativeSpread_checkBox
            // 
            this.NegativeSpread_checkBox.AccessibleName = "";
            this.NegativeSpread_checkBox.AutoSize = true;
            this.NegativeSpread_checkBox.Checked = true;
            this.NegativeSpread_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NegativeSpread_checkBox.Location = new System.Drawing.Point(490, 692);
            this.NegativeSpread_checkBox.Name = "NegativeSpread_checkBox";
            this.NegativeSpread_checkBox.Size = new System.Drawing.Size(15, 14);
            this.NegativeSpread_checkBox.TabIndex = 34;
            this.NegativeSpread_checkBox.UseVisualStyleBackColor = true;
            this.NegativeSpread_checkBox.CheckedChanged += new System.EventHandler(this.NegativeSpread_checkBox_CheckedChanged);
            // 
            // NegativeSpread_label
            // 
            this.NegativeSpread_label.AccessibleName = "";
            this.NegativeSpread_label.AutoSize = true;
            this.NegativeSpread_label.Location = new System.Drawing.Point(73, 692);
            this.NegativeSpread_label.Name = "NegativeSpread_label";
            this.NegativeSpread_label.Size = new System.Drawing.Size(226, 13);
            this.NegativeSpread_label.TabIndex = 33;
            this.NegativeSpread_label.Text = "Monitor for negative spreads and reset if found";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.SlackNameCurrency_label);
            this.groupBox1.Controls.Add(this.SlackNameCurrency_comboBox);
            this.groupBox1.Controls.Add(this.Slack_label);
            this.groupBox1.Controls.Add(this.Slack_checkBox);
            this.groupBox1.Controls.Add(this.slackToken_textBox);
            this.groupBox1.Controls.Add(this.slackDefaultNameLabel);
            this.groupBox1.Controls.Add(this.slackNameChangeLabel);
            this.groupBox1.Controls.Add(this.slackDefaultNameTextBox);
            this.groupBox1.Controls.Add(this.slackNameChangeCheckBox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(76, 303);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 222);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Slack";
            // 
            // SlackNameCurrency_label
            // 
            this.SlackNameCurrency_label.AccessibleName = "";
            this.SlackNameCurrency_label.AutoSize = true;
            this.SlackNameCurrency_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.SlackNameCurrency_label.Location = new System.Drawing.Point(16, 191);
            this.SlackNameCurrency_label.Name = "SlackNameCurrency_label";
            this.SlackNameCurrency_label.Size = new System.Drawing.Size(193, 13);
            this.SlackNameCurrency_label.TabIndex = 36;
            this.SlackNameCurrency_label.Text = "Base pair for BTC price shown in name:";
            this.IRTickerTT.SetToolTip(this.SlackNameCurrency_label, "Please leave this populated even if you decide to disable the name integration so" +
        " the app knows what to set you back to");
            // 
            // SlackNameCurrency_comboBox
            // 
            this.SlackNameCurrency_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SlackNameCurrency_comboBox.Enabled = false;
            this.SlackNameCurrency_comboBox.FormattingEnabled = true;
            this.SlackNameCurrency_comboBox.Location = new System.Drawing.Point(329, 186);
            this.SlackNameCurrency_comboBox.Name = "SlackNameCurrency_comboBox";
            this.SlackNameCurrency_comboBox.Size = new System.Drawing.Size(94, 24);
            this.SlackNameCurrency_comboBox.TabIndex = 35;
            this.SlackNameCurrency_comboBox.SelectedIndexChanged += new System.EventHandler(this.SlackNameCurrency_comboBox_SelectedIndexChanged);
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
            this.Slack_checkBox.Location = new System.Drawing.Point(408, 31);
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
            this.slackToken_textBox.Size = new System.Drawing.Size(404, 20);
            this.slackToken_textBox.TabIndex = 23;
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
            this.IRTickerTT.SetToolTip(this.slackDefaultNameLabel, "Please leave this populated even if you decide to disable the name integration so" +
        " the app knows what to set you back to");
            // 
            // slackNameChangeLabel
            // 
            this.slackNameChangeLabel.AccessibleName = "";
            this.slackNameChangeLabel.AutoSize = true;
            this.slackNameChangeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.slackNameChangeLabel.Location = new System.Drawing.Point(16, 128);
            this.slackNameChangeLabel.Name = "slackNameChangeLabel";
            this.slackNameChangeLabel.Size = new System.Drawing.Size(305, 13);
            this.slackNameChangeLabel.TabIndex = 26;
            this.slackNameChangeLabel.Text = "Change Slack username to have BTC price midpoint appended";
            // 
            // slackDefaultNameTextBox
            // 
            this.slackDefaultNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slackDefaultNameTextBox.Location = new System.Drawing.Point(248, 149);
            this.slackDefaultNameTextBox.Name = "slackDefaultNameTextBox";
            this.slackDefaultNameTextBox.Size = new System.Drawing.Size(175, 20);
            this.slackDefaultNameTextBox.TabIndex = 28;
            // 
            // slackNameChangeCheckBox
            // 
            this.slackNameChangeCheckBox.AccessibleName = "";
            this.slackNameChangeCheckBox.AutoSize = true;
            this.slackNameChangeCheckBox.Location = new System.Drawing.Point(408, 128);
            this.slackNameChangeCheckBox.Name = "slackNameChangeCheckBox";
            this.slackNameChangeCheckBox.Size = new System.Drawing.Size(15, 14);
            this.slackNameChangeCheckBox.TabIndex = 27;
            this.slackNameChangeCheckBox.UseVisualStyleBackColor = true;
            this.slackNameChangeCheckBox.CheckedChanged += new System.EventHandler(this.slackNameChangeCheckBox_CheckedChanged);
            // 
            // UITimerFreq_maskedTextBox
            // 
            this.UITimerFreq_maskedTextBox.Location = new System.Drawing.Point(405, 651);
            this.UITimerFreq_maskedTextBox.Mask = "0000";
            this.UITimerFreq_maskedTextBox.Name = "UITimerFreq_maskedTextBox";
            this.UITimerFreq_maskedTextBox.PromptChar = ' ';
            this.UITimerFreq_maskedTextBox.Size = new System.Drawing.Size(100, 20);
            this.UITimerFreq_maskedTextBox.TabIndex = 31;
            // 
            // UITimerFreq_label
            // 
            this.UITimerFreq_label.AccessibleName = "";
            this.UITimerFreq_label.AutoSize = true;
            this.UITimerFreq_label.Location = new System.Drawing.Point(73, 654);
            this.UITimerFreq_label.Name = "UITimerFreq_label";
            this.UITimerFreq_label.Size = new System.Drawing.Size(252, 13);
            this.UITimerFreq_label.TabIndex = 30;
            this.UITimerFreq_label.Text = "How often to update the UI get for BTC on IR in ms:";
            // 
            // OB_checkBox
            // 
            this.OB_checkBox.AccessibleName = "";
            this.OB_checkBox.AutoSize = true;
            this.OB_checkBox.Location = new System.Drawing.Point(490, 600);
            this.OB_checkBox.Name = "OB_checkBox";
            this.OB_checkBox.Size = new System.Drawing.Size(15, 14);
            this.OB_checkBox.TabIndex = 25;
            this.OB_checkBox.UseVisualStyleBackColor = true;
            this.OB_checkBox.CheckedChanged += new System.EventHandler(this.OB_checkBox_CheckedChanged);
            // 
            // OB_label
            // 
            this.OB_label.AccessibleName = "";
            this.OB_label.AutoSize = true;
            this.OB_label.Location = new System.Drawing.Point(73, 600);
            this.OB_label.Name = "OB_label";
            this.OB_label.Size = new System.Drawing.Size(372, 26);
            this.OB_label.TabIndex = 24;
            this.OB_label.Text = "Show real time BTC-AUD orderbook for IR?\r\nRequires app restart.  This was mainly " +
    "used for debugging, so it\'s pretty messy";
            // 
            // flashForm_checkBox
            // 
            this.flashForm_checkBox.AccessibleName = "";
            this.flashForm_checkBox.AutoSize = true;
            this.flashForm_checkBox.Location = new System.Drawing.Point(490, 545);
            this.flashForm_checkBox.Name = "flashForm_checkBox";
            this.flashForm_checkBox.Size = new System.Drawing.Size(15, 14);
            this.flashForm_checkBox.TabIndex = 22;
            this.flashForm_checkBox.UseVisualStyleBackColor = true;
            this.flashForm_checkBox.CheckedChanged += new System.EventHandler(this.flashForm_checkBox_CheckedChanged);
            // 
            // flashForm_label
            // 
            this.flashForm_label.AccessibleName = "";
            this.flashForm_label.AutoSize = true;
            this.flashForm_label.Location = new System.Drawing.Point(73, 545);
            this.flashForm_label.Name = "flashForm_label";
            this.flashForm_label.Size = new System.Drawing.Size(198, 26);
            this.flashForm_label.TabIndex = 21;
            this.flashForm_label.Text = "Flash the window if IR resets (only useful\r\nfor debugging)";
            // 
            // ExportSummarised_Checkbox
            // 
            this.ExportSummarised_Checkbox.AccessibleName = "";
            this.ExportSummarised_Checkbox.AutoSize = true;
            this.ExportSummarised_Checkbox.Location = new System.Drawing.Point(490, 188);
            this.ExportSummarised_Checkbox.Name = "ExportSummarised_Checkbox";
            this.ExportSummarised_Checkbox.Size = new System.Drawing.Size(15, 14);
            this.ExportSummarised_Checkbox.TabIndex = 18;
            this.ExportSummarised_Checkbox.UseVisualStyleBackColor = true;
            this.ExportSummarised_Checkbox.CheckedChanged += new System.EventHandler(this.ExportSummarised_Checkbox_CheckedChanged);
            // 
            // ExportSummarised_Label
            // 
            this.ExportSummarised_Label.AccessibleName = "";
            this.ExportSummarised_Label.AutoSize = true;
            this.ExportSummarised_Label.Location = new System.Drawing.Point(73, 188);
            this.ExportSummarised_Label.Name = "ExportSummarised_Label";
            this.ExportSummarised_Label.Size = new System.Drawing.Size(393, 39);
            this.ExportSummarised_Label.TabIndex = 17;
            this.ExportSummarised_Label.Text = "Export summary spread data?\r\nWill be saved to G:\\My Drive\\IR\\IRTicker\\Spread hist" +
    "ory data\\<your username>\\\r\nThis will save one datapoint per hour, which is avera" +
    "ged over the last hour.";
            // 
            // Help_Button
            // 
            this.Help_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Help_Button.Location = new System.Drawing.Point(499, 779);
            this.Help_Button.Name = "Help_Button";
            this.Help_Button.Size = new System.Drawing.Size(75, 23);
            this.Help_Button.TabIndex = 14;
            this.Help_Button.Text = "Help";
            this.Help_Button.UseVisualStyleBackColor = true;
            this.Help_Button.Click += new System.EventHandler(this.Help_Button_Click);
            // 
            // EnableGDAXLevel3_CheckBox
            // 
            this.EnableGDAXLevel3_CheckBox.AccessibleName = "";
            this.EnableGDAXLevel3_CheckBox.AutoSize = true;
            this.EnableGDAXLevel3_CheckBox.Checked = true;
            this.EnableGDAXLevel3_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableGDAXLevel3_CheckBox.Location = new System.Drawing.Point(490, 124);
            this.EnableGDAXLevel3_CheckBox.Name = "EnableGDAXLevel3_CheckBox";
            this.EnableGDAXLevel3_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.EnableGDAXLevel3_CheckBox.TabIndex = 13;
            this.EnableGDAXLevel3_CheckBox.UseVisualStyleBackColor = true;
            // 
            // EnableGDAXLevel3
            // 
            this.EnableGDAXLevel3.AccessibleName = "";
            this.EnableGDAXLevel3.AutoSize = true;
            this.EnableGDAXLevel3.Location = new System.Drawing.Point(73, 124);
            this.EnableGDAXLevel3.Name = "EnableGDAXLevel3";
            this.EnableGDAXLevel3.Size = new System.Drawing.Size(241, 39);
            this.EnableGDAXLevel3.TabIndex = 12;
            this.EnableGDAXLevel3.Text = "Pull full Coinbase Pro order book?\r\n(Not recommended if you\'re doing lots of aver" +
    "age \r\ncoin price checks - you will be rate limited)";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(73, 817);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(0, 13);
            this.VersionLabel.TabIndex = 11;
            // 
            // SettingsOKButton
            // 
            this.SettingsOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsOKButton.Location = new System.Drawing.Point(499, 808);
            this.SettingsOKButton.Name = "SettingsOKButton";
            this.SettingsOKButton.Size = new System.Drawing.Size(75, 23);
            this.SettingsOKButton.TabIndex = 4;
            this.SettingsOKButton.Text = "Close";
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
            this.GIFLabel.BackColor = System.Drawing.Color.Black;
            this.GIFLabel.ForeColor = System.Drawing.Color.Red;
            this.GIFLabel.Image = global::IRTicker.Properties.Resources.rainbow_space_bricks_jpg;
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
            this.Main.Controls.Add(this.fiatRefresh_checkBox);
            this.Main.Controls.Add(this.BTCM_GroupBox);
            this.Main.Controls.Add(this.BAR_GroupBox);
            this.Main.Controls.Add(this.BFX_GroupBox);
            this.Main.Controls.Add(this.fiat_GroupBox);
            this.Main.Controls.Add(this.SettingsButton);
            this.Main.Controls.Add(this.IR_GroupBox);
            this.Main.Controls.Add(this.GDAX_GroupBox);
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.Size = new System.Drawing.Size(585, 843);
            this.Main.TabIndex = 5;
            // 
            // fiatRefresh_checkBox
            // 
            this.fiatRefresh_checkBox.AutoSize = true;
            this.fiatRefresh_checkBox.Location = new System.Drawing.Point(30, 813);
            this.fiatRefresh_checkBox.Name = "fiatRefresh_checkBox";
            this.fiatRefresh_checkBox.Size = new System.Drawing.Size(145, 17);
            this.fiatRefresh_checkBox.TabIndex = 9;
            this.fiatRefresh_checkBox.Text = "Tick to queue fiat update";
            this.fiatRefresh_checkBox.UseVisualStyleBackColor = true;
            this.fiatRefresh_checkBox.CheckedChanged += new System.EventHandler(this.Fiat_checkBox_CheckedChanged);
            // 
            // BTCM_GroupBox
            // 
            this.BTCM_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.btcm3;
            this.BTCM_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BTCM_GroupBox.Controls.Add(this.BTCM_CurrencyBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BSV_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BSV_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BSV_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETC_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETC_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETC_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BAT_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BAT_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BAT_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_GNT_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_GNT_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_GNT_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XLM_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XLM_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XLM_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_OMG_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_OMG_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_OMG_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_AvgPrice_Label);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_CryptoComboBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_NumCoinsTextBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XRP_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BuySellComboBox);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XBT_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETH_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BCH_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_LTC_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XRP_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_LTC_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BCH_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETH_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XBT_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XRP_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETH_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XBT_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BCH_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_LTC_Label1);
            this.BTCM_GroupBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.BTCM_GroupBox.Location = new System.Drawing.Point(305, 8);
            this.BTCM_GroupBox.Name = "BTCM_GroupBox";
            this.BTCM_GroupBox.Size = new System.Drawing.Size(262, 300);
            this.BTCM_GroupBox.TabIndex = 1;
            this.BTCM_GroupBox.TabStop = false;
            this.BTCM_GroupBox.Text = "BTC Markets";
            // 
            // BTCM_BSV_Label2
            // 
            this.BTCM_BSV_Label2.AutoSize = true;
            this.BTCM_BSV_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BSV_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BSV_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BSV_Label2.Location = new System.Drawing.Point(45, 103);
            this.BTCM_BSV_Label2.Name = "BTCM_BSV_Label2";
            this.BTCM_BSV_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_BSV_Label2.TabIndex = 35;
            this.BTCM_BSV_Label2.Tag = "BTCM";
            // 
            // BTCM_BSV_Label3
            // 
            this.BTCM_BSV_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_BSV_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BSV_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BSV_Label3.Location = new System.Drawing.Point(119, 103);
            this.BTCM_BSV_Label3.Name = "BTCM_BSV_Label3";
            this.BTCM_BSV_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_BSV_Label3.TabIndex = 34;
            this.BTCM_BSV_Label3.Tag = "";
            this.BTCM_BSV_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_BSV_Label1
            // 
            this.BTCM_BSV_Label1.AutoSize = true;
            this.BTCM_BSV_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BSV_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BSV_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BSV_Label1.Location = new System.Drawing.Point(6, 103);
            this.BTCM_BSV_Label1.Name = "BTCM_BSV_Label1";
            this.BTCM_BSV_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_BSV_Label1.TabIndex = 33;
            this.BTCM_BSV_Label1.Text = "BSV:";
            // 
            // BTCM_ETC_Label2
            // 
            this.BTCM_ETC_Label2.AutoSize = true;
            this.BTCM_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETC_Label2.Location = new System.Drawing.Point(45, 143);
            this.BTCM_ETC_Label2.Name = "BTCM_ETC_Label2";
            this.BTCM_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_ETC_Label2.TabIndex = 32;
            this.BTCM_ETC_Label2.Tag = "BTCM";
            // 
            // BTCM_ETC_Label3
            // 
            this.BTCM_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETC_Label3.Location = new System.Drawing.Point(119, 143);
            this.BTCM_ETC_Label3.Name = "BTCM_ETC_Label3";
            this.BTCM_ETC_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_ETC_Label3.TabIndex = 31;
            this.BTCM_ETC_Label3.Tag = "";
            this.BTCM_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_ETC_Label1
            // 
            this.BTCM_ETC_Label1.AutoSize = true;
            this.BTCM_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETC_Label1.Location = new System.Drawing.Point(6, 143);
            this.BTCM_ETC_Label1.Name = "BTCM_ETC_Label1";
            this.BTCM_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_ETC_Label1.TabIndex = 30;
            this.BTCM_ETC_Label1.Text = "ETC:";
            // 
            // BTCM_BAT_Label2
            // 
            this.BTCM_BAT_Label2.AutoSize = true;
            this.BTCM_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BAT_Label2.Location = new System.Drawing.Point(45, 203);
            this.BTCM_BAT_Label2.Name = "BTCM_BAT_Label2";
            this.BTCM_BAT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_BAT_Label2.TabIndex = 29;
            this.BTCM_BAT_Label2.Tag = "BTCM";
            // 
            // BTCM_BAT_Label3
            // 
            this.BTCM_BAT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_BAT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BAT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BAT_Label3.Location = new System.Drawing.Point(119, 203);
            this.BTCM_BAT_Label3.Name = "BTCM_BAT_Label3";
            this.BTCM_BAT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_BAT_Label3.TabIndex = 28;
            this.BTCM_BAT_Label3.Tag = "";
            this.BTCM_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_BAT_Label1
            // 
            this.BTCM_BAT_Label1.AutoSize = true;
            this.BTCM_BAT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BAT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BAT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BAT_Label1.Location = new System.Drawing.Point(6, 203);
            this.BTCM_BAT_Label1.Name = "BTCM_BAT_Label1";
            this.BTCM_BAT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_BAT_Label1.TabIndex = 27;
            this.BTCM_BAT_Label1.Text = "BAT:";
            // 
            // BTCM_GNT_Label2
            // 
            this.BTCM_GNT_Label2.AutoSize = true;
            this.BTCM_GNT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_GNT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_GNT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_GNT_Label2.Location = new System.Drawing.Point(45, 223);
            this.BTCM_GNT_Label2.Name = "BTCM_GNT_Label2";
            this.BTCM_GNT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_GNT_Label2.TabIndex = 25;
            this.BTCM_GNT_Label2.Tag = "BTCM";
            // 
            // BTCM_GNT_Label3
            // 
            this.BTCM_GNT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_GNT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_GNT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_GNT_Label3.Location = new System.Drawing.Point(119, 223);
            this.BTCM_GNT_Label3.Name = "BTCM_GNT_Label3";
            this.BTCM_GNT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_GNT_Label3.TabIndex = 26;
            this.BTCM_GNT_Label3.Tag = "";
            this.BTCM_GNT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_GNT_Label1
            // 
            this.BTCM_GNT_Label1.AutoSize = true;
            this.BTCM_GNT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_GNT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_GNT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_GNT_Label1.Location = new System.Drawing.Point(6, 223);
            this.BTCM_GNT_Label1.Name = "BTCM_GNT_Label1";
            this.BTCM_GNT_Label1.Size = new System.Drawing.Size(37, 13);
            this.BTCM_GNT_Label1.TabIndex = 24;
            this.BTCM_GNT_Label1.Text = "GNT:";
            // 
            // BTCM_XLM_Label2
            // 
            this.BTCM_XLM_Label2.AutoSize = true;
            this.BTCM_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XLM_Label2.Location = new System.Drawing.Point(45, 163);
            this.BTCM_XLM_Label2.Name = "BTCM_XLM_Label2";
            this.BTCM_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XLM_Label2.TabIndex = 23;
            this.BTCM_XLM_Label2.Tag = "BTCM";
            // 
            // BTCM_XLM_Label3
            // 
            this.BTCM_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XLM_Label3.Location = new System.Drawing.Point(119, 163);
            this.BTCM_XLM_Label3.Name = "BTCM_XLM_Label3";
            this.BTCM_XLM_Label3.Size = new System.Drawing.Size(134, 13);
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
            this.BTCM_XLM_Label1.Location = new System.Drawing.Point(6, 163);
            this.BTCM_XLM_Label1.Name = "BTCM_XLM_Label1";
            this.BTCM_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_XLM_Label1.TabIndex = 21;
            this.BTCM_XLM_Label1.Text = "XLM:";
            // 
            // BTCM_OMG_Label2
            // 
            this.BTCM_OMG_Label2.AutoSize = true;
            this.BTCM_OMG_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_OMG_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_OMG_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_OMG_Label2.Location = new System.Drawing.Point(45, 183);
            this.BTCM_OMG_Label2.Name = "BTCM_OMG_Label2";
            this.BTCM_OMG_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_OMG_Label2.TabIndex = 19;
            this.BTCM_OMG_Label2.Tag = "BTCM";
            // 
            // BTCM_OMG_Label3
            // 
            this.BTCM_OMG_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_OMG_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_OMG_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_OMG_Label3.Location = new System.Drawing.Point(119, 183);
            this.BTCM_OMG_Label3.Name = "BTCM_OMG_Label3";
            this.BTCM_OMG_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_OMG_Label3.TabIndex = 20;
            this.BTCM_OMG_Label3.Tag = "";
            this.BTCM_OMG_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_OMG_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_OMG_Label3_MouseDoubleClick);
            // 
            // BTCM_OMG_Label1
            // 
            this.BTCM_OMG_Label1.AutoSize = true;
            this.BTCM_OMG_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_OMG_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_OMG_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_OMG_Label1.Location = new System.Drawing.Point(6, 183);
            this.BTCM_OMG_Label1.Name = "BTCM_OMG_Label1";
            this.BTCM_OMG_Label1.Size = new System.Drawing.Size(39, 13);
            this.BTCM_OMG_Label1.TabIndex = 18;
            this.BTCM_OMG_Label1.Text = "OMG:";
            // 
            // BTCM_AvgPrice_Label
            // 
            this.BTCM_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.BTCM_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_AvgPrice_Label.Location = new System.Drawing.Point(6, 243);
            this.BTCM_AvgPrice_Label.Name = "BTCM_AvgPrice_Label";
            this.BTCM_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.BTCM_AvgPrice_Label.TabIndex = 16;
            // 
            // BTCM_CryptoComboBox
            // 
            this.BTCM_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BTCM_CryptoComboBox.Location = new System.Drawing.Point(194, 271);
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
            this.BTCM_NumCoinsTextBox.Location = new System.Drawing.Point(58, 271);
            this.BTCM_NumCoinsTextBox.Name = "BTCM_NumCoinsTextBox";
            this.BTCM_NumCoinsTextBox.PromptChar = ' ';
            this.BTCM_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.BTCM_NumCoinsTextBox.TabIndex = 16;
            this.BTCM_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BTCM_NumCoinsTextBox.ValidatingType = typeof(int);
            this.BTCM_NumCoinsTextBox.TextChanged += new System.EventHandler(this.BTCM_NumCoinsTextBox_TextChanged);
            this.BTCM_NumCoinsTextBox.Enter += new System.EventHandler(this.BTCM_NumCoinsTextBox_Enter);
            // 
            // BTCM_XRP_Label2
            // 
            this.BTCM_XRP_Label2.AutoSize = true;
            this.BTCM_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XRP_Label2.Location = new System.Drawing.Point(45, 43);
            this.BTCM_XRP_Label2.Name = "BTCM_XRP_Label2";
            this.BTCM_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XRP_Label2.TabIndex = 9;
            this.BTCM_XRP_Label2.Tag = "BTCM";
            // 
            // BTCM_BuySellComboBox
            // 
            this.BTCM_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BTCM_BuySellComboBox.FormattingEnabled = true;
            this.BTCM_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.BTCM_BuySellComboBox.Location = new System.Drawing.Point(10, 271);
            this.BTCM_BuySellComboBox.Name = "BTCM_BuySellComboBox";
            this.BTCM_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.BTCM_BuySellComboBox.TabIndex = 15;
            this.BTCM_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.BTCM_BuySellComboBox_SelectedIndexChanged);
            // 
            // BTCM_XBT_Label2
            // 
            this.BTCM_XBT_Label2.AutoSize = true;
            this.BTCM_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XBT_Label2.Location = new System.Drawing.Point(45, 23);
            this.BTCM_XBT_Label2.Name = "BTCM_XBT_Label2";
            this.BTCM_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XBT_Label2.TabIndex = 12;
            this.BTCM_XBT_Label2.Tag = "BTCM";
            // 
            // BTCM_ETH_Label2
            // 
            this.BTCM_ETH_Label2.AutoSize = true;
            this.BTCM_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETH_Label2.Location = new System.Drawing.Point(45, 63);
            this.BTCM_ETH_Label2.Name = "BTCM_ETH_Label2";
            this.BTCM_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_ETH_Label2.TabIndex = 13;
            this.BTCM_ETH_Label2.Tag = "BTCM";
            // 
            // BTCM_BCH_Label2
            // 
            this.BTCM_BCH_Label2.AutoSize = true;
            this.BTCM_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BCH_Label2.Location = new System.Drawing.Point(45, 83);
            this.BTCM_BCH_Label2.Name = "BTCM_BCH_Label2";
            this.BTCM_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_BCH_Label2.TabIndex = 14;
            this.BTCM_BCH_Label2.Tag = "BTCM";
            // 
            // BTCM_LTC_Label2
            // 
            this.BTCM_LTC_Label2.AutoSize = true;
            this.BTCM_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LTC_Label2.Location = new System.Drawing.Point(45, 123);
            this.BTCM_LTC_Label2.Name = "BTCM_LTC_Label2";
            this.BTCM_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_LTC_Label2.TabIndex = 15;
            this.BTCM_LTC_Label2.Tag = "BTCM";
            // 
            // BTCM_XRP_Label3
            // 
            this.BTCM_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XRP_Label3.Location = new System.Drawing.Point(119, 43);
            this.BTCM_XRP_Label3.Name = "BTCM_XRP_Label3";
            this.BTCM_XRP_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_XRP_Label3.TabIndex = 16;
            this.BTCM_XRP_Label3.Tag = "";
            this.BTCM_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_XRP_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_XRP_Label3_MouseDoubleClick);
            // 
            // BTCM_LTC_Label3
            // 
            this.BTCM_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LTC_Label3.Location = new System.Drawing.Point(119, 123);
            this.BTCM_LTC_Label3.Name = "BTCM_LTC_Label3";
            this.BTCM_LTC_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_LTC_Label3.TabIndex = 15;
            this.BTCM_LTC_Label3.Tag = "";
            this.BTCM_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_LTC_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_LTC_Label3_MouseDoubleClick);
            // 
            // BTCM_BCH_Label3
            // 
            this.BTCM_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BCH_Label3.Location = new System.Drawing.Point(119, 83);
            this.BTCM_BCH_Label3.Name = "BTCM_BCH_Label3";
            this.BTCM_BCH_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_BCH_Label3.TabIndex = 14;
            this.BTCM_BCH_Label3.Tag = "";
            this.BTCM_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_BCH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_BCH_Label3_MouseDoubleClick);
            // 
            // BTCM_ETH_Label3
            // 
            this.BTCM_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETH_Label3.Location = new System.Drawing.Point(119, 63);
            this.BTCM_ETH_Label3.Name = "BTCM_ETH_Label3";
            this.BTCM_ETH_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_ETH_Label3.TabIndex = 13;
            this.BTCM_ETH_Label3.Tag = "";
            this.BTCM_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_ETH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_ETH_Label3_MouseDoubleClick);
            // 
            // BTCM_XBT_Label3
            // 
            this.BTCM_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XBT_Label3.Location = new System.Drawing.Point(119, 23);
            this.BTCM_XBT_Label3.Name = "BTCM_XBT_Label3";
            this.BTCM_XBT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BTCM_XBT_Label3.TabIndex = 12;
            this.BTCM_XBT_Label3.Tag = "";
            this.BTCM_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BTCM_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BTCM_XBT_Label3_MouseDoubleClick);
            // 
            // BTCM_XRP_Label1
            // 
            this.BTCM_XRP_Label1.AutoSize = true;
            this.BTCM_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XRP_Label1.Location = new System.Drawing.Point(6, 43);
            this.BTCM_XRP_Label1.Name = "BTCM_XRP_Label1";
            this.BTCM_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_XRP_Label1.TabIndex = 8;
            this.BTCM_XRP_Label1.Text = "XRP:";
            // 
            // BTCM_ETH_Label1
            // 
            this.BTCM_ETH_Label1.AutoSize = true;
            this.BTCM_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETH_Label1.Location = new System.Drawing.Point(6, 63);
            this.BTCM_ETH_Label1.Name = "BTCM_ETH_Label1";
            this.BTCM_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_ETH_Label1.TabIndex = 9;
            this.BTCM_ETH_Label1.Text = "ETH:";
            // 
            // BTCM_XBT_Label1
            // 
            this.BTCM_XBT_Label1.AutoSize = true;
            this.BTCM_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.BTCM_XBT_Label1.Name = "BTCM_XBT_Label1";
            this.BTCM_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_XBT_Label1.TabIndex = 8;
            this.BTCM_XBT_Label1.Text = "BTC:";
            // 
            // BTCM_BCH_Label1
            // 
            this.BTCM_BCH_Label1.AutoSize = true;
            this.BTCM_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BCH_Label1.Location = new System.Drawing.Point(6, 83);
            this.BTCM_BCH_Label1.Name = "BTCM_BCH_Label1";
            this.BTCM_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_BCH_Label1.TabIndex = 10;
            this.BTCM_BCH_Label1.Text = "BCH:";
            // 
            // BTCM_LTC_Label1
            // 
            this.BTCM_LTC_Label1.AutoSize = true;
            this.BTCM_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LTC_Label1.Location = new System.Drawing.Point(6, 123);
            this.BTCM_LTC_Label1.Name = "BTCM_LTC_Label1";
            this.BTCM_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.BTCM_LTC_Label1.TabIndex = 11;
            this.BTCM_LTC_Label1.Text = "LTC:";
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
            this.BAR_GroupBox.Location = new System.Drawing.Point(305, 314);
            this.BAR_GroupBox.Name = "BAR_GroupBox";
            this.BAR_GroupBox.Size = new System.Drawing.Size(262, 102);
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
            this.BAR_CurrencyBox.Location = new System.Drawing.Point(131, 72);
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
            this.BAR_XBT_Label2.Location = new System.Drawing.Point(53, 23);
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
            this.BAR_XBT_Label3.Location = new System.Drawing.Point(117, 25);
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
            this.BAR_AvgPrice_Label.Location = new System.Drawing.Point(6, 45);
            this.BAR_AvgPrice_Label.Name = "BAR_AvgPrice_Label";
            this.BAR_AvgPrice_Label.Size = new System.Drawing.Size(245, 17);
            this.BAR_AvgPrice_Label.TabIndex = 58;
            // 
            // BAR_CryptoComboBox
            // 
            this.BAR_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BAR_CryptoComboBox.Location = new System.Drawing.Point(193, 72);
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
            this.BAR_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.BAR_XBT_Label1.Name = "BAR_XBT_Label1";
            this.BAR_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BAR_XBT_Label1.TabIndex = 0;
            this.BAR_XBT_Label1.Text = "BTC:";
            // 
            // BAR_NumCoinsTextBox
            // 
            this.BAR_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BAR_NumCoinsTextBox.Location = new System.Drawing.Point(58, 72);
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
            this.BAR_BuySellComboBox.Location = new System.Drawing.Point(9, 72);
            this.BAR_BuySellComboBox.Name = "BAR_BuySellComboBox";
            this.BAR_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.BAR_BuySellComboBox.TabIndex = 55;
            this.BAR_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.BAR_BuySellComboBox_SelectedIndexChanged);
            // 
            // BFX_GroupBox
            // 
            this.BFX_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.bfx_faded;
            this.BFX_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BFX_GroupBox.Controls.Add(this.BFX_CurrencyBox);
            this.BFX_GroupBox.Controls.Add(this.BFX_USDT_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_USDT_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_USDT_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_BSV_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_BSV_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_BSV_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETC_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETC_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETC_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_BAT_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_BAT_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_BAT_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_GNT_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_GNT_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_REP_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_REP_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_GNT_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_REP_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_XLM_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_XLM_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_XLM_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_EOS_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_EOS_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_EOS_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_ZRX_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_ZRX_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_OMG_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_OMG_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_ZRX_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_OMG_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_XRP_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_XRP_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_XRP_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_CryptoComboBox);
            this.BFX_GroupBox.Controls.Add(this.BFX_NumCoinsTextBox);
            this.BFX_GroupBox.Controls.Add(this.BFX_XBT_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_BuySellComboBox);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETH_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_BCH_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_LTC_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_BCH_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_LTC_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETH_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_XBT_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_LTC_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_BCH_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETH_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_XBT_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_AvgPrice_Label);
            this.BFX_GroupBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BFX_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.BFX_GroupBox.Location = new System.Drawing.Point(305, 422);
            this.BFX_GroupBox.Name = "BFX_GroupBox";
            this.BFX_GroupBox.Size = new System.Drawing.Size(262, 380);
            this.BFX_GroupBox.TabIndex = 9;
            this.BFX_GroupBox.TabStop = false;
            this.BFX_GroupBox.Text = "BitFinex";
            this.BFX_GroupBox.Click += new System.EventHandler(this.BFX_GroupBox_Click);
            // 
            // BFX_USDT_Label2
            // 
            this.BFX_USDT_Label2.AutoSize = true;
            this.BFX_USDT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_USDT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_USDT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_USDT_Label2.Location = new System.Drawing.Point(44, 83);
            this.BFX_USDT_Label2.Name = "BFX_USDT_Label2";
            this.BFX_USDT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_USDT_Label2.TabIndex = 49;
            this.BFX_USDT_Label2.Tag = "BFX";
            // 
            // BFX_USDT_Label3
            // 
            this.BFX_USDT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_USDT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_USDT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_USDT_Label3.Location = new System.Drawing.Point(118, 83);
            this.BFX_USDT_Label3.Name = "BFX_USDT_Label3";
            this.BFX_USDT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_USDT_Label3.TabIndex = 50;
            this.BFX_USDT_Label3.Tag = "";
            this.BFX_USDT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_USDT_Label1
            // 
            this.BFX_USDT_Label1.AutoSize = true;
            this.BFX_USDT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_USDT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_USDT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_USDT_Label1.Location = new System.Drawing.Point(5, 83);
            this.BFX_USDT_Label1.Name = "BFX_USDT_Label1";
            this.BFX_USDT_Label1.Size = new System.Drawing.Size(45, 13);
            this.BFX_USDT_Label1.TabIndex = 48;
            this.BFX_USDT_Label1.Text = "USDT:";
            // 
            // BFX_BSV_Label2
            // 
            this.BFX_BSV_Label2.AutoSize = true;
            this.BFX_BSV_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BSV_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BSV_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BSV_Label2.Location = new System.Drawing.Point(45, 143);
            this.BFX_BSV_Label2.Name = "BFX_BSV_Label2";
            this.BFX_BSV_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_BSV_Label2.TabIndex = 46;
            this.BFX_BSV_Label2.Tag = "BFX";
            // 
            // BFX_BSV_Label3
            // 
            this.BFX_BSV_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_BSV_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BSV_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BSV_Label3.Location = new System.Drawing.Point(119, 143);
            this.BFX_BSV_Label3.Name = "BFX_BSV_Label3";
            this.BFX_BSV_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_BSV_Label3.TabIndex = 47;
            this.BFX_BSV_Label3.Tag = "";
            this.BFX_BSV_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_BSV_Label1
            // 
            this.BFX_BSV_Label1.AutoSize = true;
            this.BFX_BSV_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BSV_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BSV_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BSV_Label1.Location = new System.Drawing.Point(6, 143);
            this.BFX_BSV_Label1.Name = "BFX_BSV_Label1";
            this.BFX_BSV_Label1.Size = new System.Drawing.Size(35, 13);
            this.BFX_BSV_Label1.TabIndex = 45;
            this.BFX_BSV_Label1.Text = "BSV:";
            // 
            // BFX_ETC_Label2
            // 
            this.BFX_ETC_Label2.AutoSize = true;
            this.BFX_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ETC_Label2.Location = new System.Drawing.Point(45, 183);
            this.BFX_ETC_Label2.Name = "BFX_ETC_Label2";
            this.BFX_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_ETC_Label2.TabIndex = 43;
            this.BFX_ETC_Label2.Tag = "BFX";
            // 
            // BFX_ETC_Label3
            // 
            this.BFX_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ETC_Label3.Location = new System.Drawing.Point(119, 183);
            this.BFX_ETC_Label3.Name = "BFX_ETC_Label3";
            this.BFX_ETC_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_ETC_Label3.TabIndex = 44;
            this.BFX_ETC_Label3.Tag = "";
            this.BFX_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_ETC_Label1
            // 
            this.BFX_ETC_Label1.AutoSize = true;
            this.BFX_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ETC_Label1.Location = new System.Drawing.Point(6, 183);
            this.BFX_ETC_Label1.Name = "BFX_ETC_Label1";
            this.BFX_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.BFX_ETC_Label1.TabIndex = 42;
            this.BFX_ETC_Label1.Text = "ETC:";
            // 
            // BFX_BAT_Label2
            // 
            this.BFX_BAT_Label2.AutoSize = true;
            this.BFX_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BAT_Label2.Location = new System.Drawing.Point(45, 263);
            this.BFX_BAT_Label2.Name = "BFX_BAT_Label2";
            this.BFX_BAT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_BAT_Label2.TabIndex = 40;
            this.BFX_BAT_Label2.Tag = "BFX";
            // 
            // BFX_BAT_Label3
            // 
            this.BFX_BAT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_BAT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BAT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BAT_Label3.Location = new System.Drawing.Point(119, 263);
            this.BFX_BAT_Label3.Name = "BFX_BAT_Label3";
            this.BFX_BAT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_BAT_Label3.TabIndex = 41;
            this.BFX_BAT_Label3.Tag = "";
            this.BFX_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_BAT_Label1
            // 
            this.BFX_BAT_Label1.AutoSize = true;
            this.BFX_BAT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BAT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BAT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BAT_Label1.Location = new System.Drawing.Point(6, 263);
            this.BFX_BAT_Label1.Name = "BFX_BAT_Label1";
            this.BFX_BAT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BFX_BAT_Label1.TabIndex = 39;
            this.BFX_BAT_Label1.Text = "BAT:";
            // 
            // BFX_GNT_Label2
            // 
            this.BFX_GNT_Label2.AutoSize = true;
            this.BFX_GNT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_GNT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_GNT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_GNT_Label2.Location = new System.Drawing.Point(45, 303);
            this.BFX_GNT_Label2.Name = "BFX_GNT_Label2";
            this.BFX_GNT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_GNT_Label2.TabIndex = 37;
            this.BFX_GNT_Label2.Tag = "BFX";
            // 
            // BFX_GNT_Label1
            // 
            this.BFX_GNT_Label1.AutoSize = true;
            this.BFX_GNT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_GNT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_GNT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_GNT_Label1.Location = new System.Drawing.Point(6, 303);
            this.BFX_GNT_Label1.Name = "BFX_GNT_Label1";
            this.BFX_GNT_Label1.Size = new System.Drawing.Size(37, 13);
            this.BFX_GNT_Label1.TabIndex = 35;
            this.BFX_GNT_Label1.Text = "GNT:";
            // 
            // BFX_REP_Label2
            // 
            this.BFX_REP_Label2.AutoSize = true;
            this.BFX_REP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_REP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_REP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_REP_Label2.Location = new System.Drawing.Point(45, 283);
            this.BFX_REP_Label2.Name = "BFX_REP_Label2";
            this.BFX_REP_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_REP_Label2.TabIndex = 34;
            this.BFX_REP_Label2.Tag = "BFX";
            // 
            // BFX_REP_Label3
            // 
            this.BFX_REP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_REP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_REP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_REP_Label3.Location = new System.Drawing.Point(119, 283);
            this.BFX_REP_Label3.Name = "BFX_REP_Label3";
            this.BFX_REP_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_REP_Label3.TabIndex = 36;
            this.BFX_REP_Label3.Tag = "";
            this.BFX_REP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_GNT_Label3
            // 
            this.BFX_GNT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_GNT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_GNT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_GNT_Label3.Location = new System.Drawing.Point(119, 303);
            this.BFX_GNT_Label3.Name = "BFX_GNT_Label3";
            this.BFX_GNT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_GNT_Label3.TabIndex = 38;
            this.BFX_GNT_Label3.Tag = "";
            this.BFX_GNT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_REP_Label1
            // 
            this.BFX_REP_Label1.AutoSize = true;
            this.BFX_REP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_REP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_REP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_REP_Label1.Location = new System.Drawing.Point(6, 283);
            this.BFX_REP_Label1.Name = "BFX_REP_Label1";
            this.BFX_REP_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_REP_Label1.TabIndex = 33;
            this.BFX_REP_Label1.Text = "REP:";
            // 
            // BFX_XLM_Label2
            // 
            this.BFX_XLM_Label2.AutoSize = true;
            this.BFX_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XLM_Label2.Location = new System.Drawing.Point(45, 203);
            this.BFX_XLM_Label2.Name = "BFX_XLM_Label2";
            this.BFX_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_XLM_Label2.TabIndex = 31;
            this.BFX_XLM_Label2.Tag = "BFX";
            // 
            // BFX_XLM_Label3
            // 
            this.BFX_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XLM_Label3.Location = new System.Drawing.Point(119, 203);
            this.BFX_XLM_Label3.Name = "BFX_XLM_Label3";
            this.BFX_XLM_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_XLM_Label3.TabIndex = 32;
            this.BFX_XLM_Label3.Tag = "";
            this.BFX_XLM_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_XLM_Label1
            // 
            this.BFX_XLM_Label1.AutoSize = true;
            this.BFX_XLM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XLM_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XLM_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XLM_Label1.Location = new System.Drawing.Point(6, 203);
            this.BFX_XLM_Label1.Name = "BFX_XLM_Label1";
            this.BFX_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_XLM_Label1.TabIndex = 30;
            this.BFX_XLM_Label1.Text = "XLM:";
            // 
            // BFX_EOS_Label2
            // 
            this.BFX_EOS_Label2.AutoSize = true;
            this.BFX_EOS_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_EOS_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_EOS_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_EOS_Label2.Location = new System.Drawing.Point(45, 103);
            this.BFX_EOS_Label2.Name = "BFX_EOS_Label2";
            this.BFX_EOS_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_EOS_Label2.TabIndex = 28;
            this.BFX_EOS_Label2.Tag = "BFX";
            // 
            // BFX_EOS_Label3
            // 
            this.BFX_EOS_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_EOS_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_EOS_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_EOS_Label3.Location = new System.Drawing.Point(119, 103);
            this.BFX_EOS_Label3.Name = "BFX_EOS_Label3";
            this.BFX_EOS_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_EOS_Label3.TabIndex = 29;
            this.BFX_EOS_Label3.Tag = "";
            this.BFX_EOS_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_EOS_Label1
            // 
            this.BFX_EOS_Label1.AutoSize = true;
            this.BFX_EOS_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_EOS_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_EOS_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_EOS_Label1.Location = new System.Drawing.Point(6, 103);
            this.BFX_EOS_Label1.Name = "BFX_EOS_Label1";
            this.BFX_EOS_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_EOS_Label1.TabIndex = 27;
            this.BFX_EOS_Label1.Text = "EOS:";
            // 
            // BFX_ZRX_Label2
            // 
            this.BFX_ZRX_Label2.AutoSize = true;
            this.BFX_ZRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ZRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ZRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ZRX_Label2.Location = new System.Drawing.Point(45, 243);
            this.BFX_ZRX_Label2.Name = "BFX_ZRX_Label2";
            this.BFX_ZRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_ZRX_Label2.TabIndex = 25;
            this.BFX_ZRX_Label2.Tag = "BFX";
            // 
            // BFX_ZRX_Label1
            // 
            this.BFX_ZRX_Label1.AutoSize = true;
            this.BFX_ZRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ZRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ZRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ZRX_Label1.Location = new System.Drawing.Point(6, 243);
            this.BFX_ZRX_Label1.Name = "BFX_ZRX_Label1";
            this.BFX_ZRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_ZRX_Label1.TabIndex = 23;
            this.BFX_ZRX_Label1.Text = "ZRX:";
            // 
            // BFX_OMG_Label2
            // 
            this.BFX_OMG_Label2.AutoSize = true;
            this.BFX_OMG_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_OMG_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_OMG_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_OMG_Label2.Location = new System.Drawing.Point(45, 223);
            this.BFX_OMG_Label2.Name = "BFX_OMG_Label2";
            this.BFX_OMG_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_OMG_Label2.TabIndex = 22;
            this.BFX_OMG_Label2.Tag = "BFX";
            // 
            // BFX_OMG_Label3
            // 
            this.BFX_OMG_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_OMG_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_OMG_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_OMG_Label3.Location = new System.Drawing.Point(119, 223);
            this.BFX_OMG_Label3.Name = "BFX_OMG_Label3";
            this.BFX_OMG_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_OMG_Label3.TabIndex = 24;
            this.BFX_OMG_Label3.Tag = "";
            this.BFX_OMG_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_OMG_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_OMG_Label3_MouseDoubleClick);
            // 
            // BFX_ZRX_Label3
            // 
            this.BFX_ZRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_ZRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ZRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ZRX_Label3.Location = new System.Drawing.Point(119, 242);
            this.BFX_ZRX_Label3.Name = "BFX_ZRX_Label3";
            this.BFX_ZRX_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_ZRX_Label3.TabIndex = 26;
            this.BFX_ZRX_Label3.Tag = "";
            this.BFX_ZRX_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_ZRX_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_ZRX_Label3_MouseDoubleClick);
            // 
            // BFX_OMG_Label1
            // 
            this.BFX_OMG_Label1.AutoSize = true;
            this.BFX_OMG_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_OMG_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_OMG_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_OMG_Label1.Location = new System.Drawing.Point(6, 223);
            this.BFX_OMG_Label1.Name = "BFX_OMG_Label1";
            this.BFX_OMG_Label1.Size = new System.Drawing.Size(39, 13);
            this.BFX_OMG_Label1.TabIndex = 21;
            this.BFX_OMG_Label1.Text = "OMG:";
            // 
            // BFX_XRP_Label2
            // 
            this.BFX_XRP_Label2.AutoSize = true;
            this.BFX_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XRP_Label2.Location = new System.Drawing.Point(45, 43);
            this.BFX_XRP_Label2.Name = "BFX_XRP_Label2";
            this.BFX_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_XRP_Label2.TabIndex = 19;
            this.BFX_XRP_Label2.Tag = "BFX";
            // 
            // BFX_XRP_Label3
            // 
            this.BFX_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XRP_Label3.Location = new System.Drawing.Point(119, 43);
            this.BFX_XRP_Label3.Name = "BFX_XRP_Label3";
            this.BFX_XRP_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_XRP_Label3.TabIndex = 20;
            this.BFX_XRP_Label3.Tag = "";
            this.BFX_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_XRP_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_XRP_Label3_MouseDoubleClick);
            // 
            // BFX_XRP_Label1
            // 
            this.BFX_XRP_Label1.AutoSize = true;
            this.BFX_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XRP_Label1.Location = new System.Drawing.Point(6, 43);
            this.BFX_XRP_Label1.Name = "BFX_XRP_Label1";
            this.BFX_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_XRP_Label1.TabIndex = 18;
            this.BFX_XRP_Label1.Text = "XRP:";
            // 
            // BFX_CryptoComboBox
            // 
            this.BFX_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BFX_CryptoComboBox.Location = new System.Drawing.Point(194, 350);
            this.BFX_CryptoComboBox.Name = "BFX_CryptoComboBox";
            this.BFX_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.BFX_CryptoComboBox.TabIndex = 20;
            this.BFX_CryptoComboBox.DropDown += new System.EventHandler(this.BFX_CryptoComboBox_DropDown);
            this.BFX_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.BFX_CryptoComboBox_SelectedIndexChanged);
            // 
            // BFX_NumCoinsTextBox
            // 
            this.BFX_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_NumCoinsTextBox.Location = new System.Drawing.Point(58, 350);
            this.BFX_NumCoinsTextBox.Name = "BFX_NumCoinsTextBox";
            this.BFX_NumCoinsTextBox.PromptChar = ' ';
            this.BFX_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.BFX_NumCoinsTextBox.TabIndex = 19;
            this.BFX_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BFX_NumCoinsTextBox.ValidatingType = typeof(int);
            this.BFX_NumCoinsTextBox.TextChanged += new System.EventHandler(this.BFX_NumCoinsTextBox_TextChanged);
            this.BFX_NumCoinsTextBox.Enter += new System.EventHandler(this.BFX_NumCoinsTextBox_Enter);
            // 
            // BFX_XBT_Label2
            // 
            this.BFX_XBT_Label2.AutoSize = true;
            this.BFX_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XBT_Label2.Location = new System.Drawing.Point(45, 23);
            this.BFX_XBT_Label2.Name = "BFX_XBT_Label2";
            this.BFX_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_XBT_Label2.TabIndex = 4;
            this.BFX_XBT_Label2.Tag = "BFX";
            // 
            // BFX_BuySellComboBox
            // 
            this.BFX_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BFX_BuySellComboBox.FormattingEnabled = true;
            this.BFX_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.BFX_BuySellComboBox.Location = new System.Drawing.Point(10, 350);
            this.BFX_BuySellComboBox.Name = "BFX_BuySellComboBox";
            this.BFX_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.BFX_BuySellComboBox.TabIndex = 18;
            this.BFX_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.BFX_BuySellComboBox_SelectedIndexChanged);
            // 
            // BFX_ETH_Label2
            // 
            this.BFX_ETH_Label2.AutoSize = true;
            this.BFX_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ETH_Label2.Location = new System.Drawing.Point(45, 63);
            this.BFX_ETH_Label2.Name = "BFX_ETH_Label2";
            this.BFX_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_ETH_Label2.TabIndex = 5;
            this.BFX_ETH_Label2.Tag = "BFX";
            // 
            // BFX_BCH_Label2
            // 
            this.BFX_BCH_Label2.AutoSize = true;
            this.BFX_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BCH_Label2.Location = new System.Drawing.Point(45, 123);
            this.BFX_BCH_Label2.Name = "BFX_BCH_Label2";
            this.BFX_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_BCH_Label2.TabIndex = 6;
            this.BFX_BCH_Label2.Tag = "BFX";
            // 
            // BFX_LTC_Label2
            // 
            this.BFX_LTC_Label2.AutoSize = true;
            this.BFX_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_LTC_Label2.Location = new System.Drawing.Point(45, 163);
            this.BFX_LTC_Label2.Name = "BFX_LTC_Label2";
            this.BFX_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_LTC_Label2.TabIndex = 7;
            this.BFX_LTC_Label2.Tag = "BFX";
            // 
            // BFX_BCH_Label3
            // 
            this.BFX_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BCH_Label3.Location = new System.Drawing.Point(119, 123);
            this.BFX_BCH_Label3.Name = "BFX_BCH_Label3";
            this.BFX_BCH_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_BCH_Label3.TabIndex = 18;
            this.BFX_BCH_Label3.Tag = "";
            this.BFX_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_BCH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_BCH_Label3_MouseDoubleClick);
            // 
            // BFX_LTC_Label3
            // 
            this.BFX_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_LTC_Label3.Location = new System.Drawing.Point(119, 163);
            this.BFX_LTC_Label3.Name = "BFX_LTC_Label3";
            this.BFX_LTC_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_LTC_Label3.TabIndex = 19;
            this.BFX_LTC_Label3.Tag = "";
            this.BFX_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_LTC_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_LTC_Label3_MouseDoubleClick);
            // 
            // BFX_ETH_Label3
            // 
            this.BFX_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ETH_Label3.Location = new System.Drawing.Point(119, 63);
            this.BFX_ETH_Label3.Name = "BFX_ETH_Label3";
            this.BFX_ETH_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_ETH_Label3.TabIndex = 17;
            this.BFX_ETH_Label3.Tag = "";
            this.BFX_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_ETH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_ETH_Label3_MouseDoubleClick);
            // 
            // BFX_XBT_Label3
            // 
            this.BFX_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XBT_Label3.Location = new System.Drawing.Point(119, 23);
            this.BFX_XBT_Label3.Name = "BFX_XBT_Label3";
            this.BFX_XBT_Label3.Size = new System.Drawing.Size(134, 13);
            this.BFX_XBT_Label3.TabIndex = 16;
            this.BFX_XBT_Label3.Tag = "";
            this.BFX_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BFX_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFX_XBT_Label3_MouseDoubleClick);
            // 
            // BFX_LTC_Label1
            // 
            this.BFX_LTC_Label1.AutoSize = true;
            this.BFX_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_LTC_Label1.Location = new System.Drawing.Point(6, 163);
            this.BFX_LTC_Label1.Name = "BFX_LTC_Label1";
            this.BFX_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.BFX_LTC_Label1.TabIndex = 3;
            this.BFX_LTC_Label1.Text = "LTC:";
            // 
            // BFX_BCH_Label1
            // 
            this.BFX_BCH_Label1.AutoSize = true;
            this.BFX_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BCH_Label1.Location = new System.Drawing.Point(6, 123);
            this.BFX_BCH_Label1.Name = "BFX_BCH_Label1";
            this.BFX_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_BCH_Label1.TabIndex = 2;
            this.BFX_BCH_Label1.Text = "BCH:";
            // 
            // BFX_ETH_Label1
            // 
            this.BFX_ETH_Label1.AutoSize = true;
            this.BFX_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ETH_Label1.Location = new System.Drawing.Point(6, 63);
            this.BFX_ETH_Label1.Name = "BFX_ETH_Label1";
            this.BFX_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_ETH_Label1.TabIndex = 1;
            this.BFX_ETH_Label1.Text = "ETH:";
            // 
            // BFX_XBT_Label1
            // 
            this.BFX_XBT_Label1.AutoSize = true;
            this.BFX_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.BFX_XBT_Label1.Name = "BFX_XBT_Label1";
            this.BFX_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BFX_XBT_Label1.TabIndex = 0;
            this.BFX_XBT_Label1.Text = "BTC:";
            // 
            // BFX_AvgPrice_Label
            // 
            this.BFX_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.BFX_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_AvgPrice_Label.Location = new System.Drawing.Point(6, 321);
            this.BFX_AvgPrice_Label.Name = "BFX_AvgPrice_Label";
            this.BFX_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.BFX_AvgPrice_Label.TabIndex = 19;
            // 
            // fiat_GroupBox
            // 
            this.fiat_GroupBox.Controls.Add(this.SGD_Label2);
            this.fiat_GroupBox.Controls.Add(this.SGD_Label1);
            this.fiat_GroupBox.Controls.Add(this.USD_Label2);
            this.fiat_GroupBox.Controls.Add(this.USD_Label1);
            this.fiat_GroupBox.Controls.Add(this.AUD_Label2);
            this.fiat_GroupBox.Controls.Add(this.NZD_Label2);
            this.fiat_GroupBox.Controls.Add(this.EUR_Label2);
            this.fiat_GroupBox.Controls.Add(this.EUR_Label1);
            this.fiat_GroupBox.Controls.Add(this.NZD_Label1);
            this.fiat_GroupBox.Controls.Add(this.AUD_Label1);
            this.fiat_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.fiat_GroupBox.Location = new System.Drawing.Point(19, 671);
            this.fiat_GroupBox.Name = "fiat_GroupBox";
            this.fiat_GroupBox.Size = new System.Drawing.Size(263, 131);
            this.fiat_GroupBox.TabIndex = 9;
            this.fiat_GroupBox.TabStop = false;
            this.fiat_GroupBox.Text = "Fiat rates";
            this.fiat_GroupBox.Click += new System.EventHandler(this.Fiat_GroupBox_Click);
            // 
            // SGD_Label2
            // 
            this.SGD_Label2.AutoSize = true;
            this.SGD_Label2.Location = new System.Drawing.Point(58, 102);
            this.SGD_Label2.Name = "SGD_Label2";
            this.SGD_Label2.Size = new System.Drawing.Size(0, 13);
            this.SGD_Label2.TabIndex = 10;
            // 
            // SGD_Label1
            // 
            this.SGD_Label1.AutoSize = true;
            this.SGD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SGD_Label1.Location = new System.Drawing.Point(8, 103);
            this.SGD_Label1.Name = "SGD_Label1";
            this.SGD_Label1.Size = new System.Drawing.Size(37, 13);
            this.SGD_Label1.TabIndex = 9;
            this.SGD_Label1.Text = "SGD:";
            // 
            // USD_Label2
            // 
            this.USD_Label2.AutoSize = true;
            this.USD_Label2.Location = new System.Drawing.Point(58, 82);
            this.USD_Label2.Name = "USD_Label2";
            this.USD_Label2.Size = new System.Drawing.Size(0, 13);
            this.USD_Label2.TabIndex = 8;
            // 
            // USD_Label1
            // 
            this.USD_Label1.AutoSize = true;
            this.USD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USD_Label1.Location = new System.Drawing.Point(8, 83);
            this.USD_Label1.Name = "USD_Label1";
            this.USD_Label1.Size = new System.Drawing.Size(37, 13);
            this.USD_Label1.TabIndex = 7;
            this.USD_Label1.Text = "USD:";
            // 
            // AUD_Label2
            // 
            this.AUD_Label2.AutoSize = true;
            this.AUD_Label2.Location = new System.Drawing.Point(57, 23);
            this.AUD_Label2.Name = "AUD_Label2";
            this.AUD_Label2.Size = new System.Drawing.Size(0, 13);
            this.AUD_Label2.TabIndex = 4;
            // 
            // NZD_Label2
            // 
            this.NZD_Label2.AutoSize = true;
            this.NZD_Label2.Location = new System.Drawing.Point(57, 43);
            this.NZD_Label2.Name = "NZD_Label2";
            this.NZD_Label2.Size = new System.Drawing.Size(0, 13);
            this.NZD_Label2.TabIndex = 5;
            // 
            // EUR_Label2
            // 
            this.EUR_Label2.AutoSize = true;
            this.EUR_Label2.Location = new System.Drawing.Point(58, 62);
            this.EUR_Label2.Name = "EUR_Label2";
            this.EUR_Label2.Size = new System.Drawing.Size(0, 13);
            this.EUR_Label2.TabIndex = 6;
            // 
            // EUR_Label1
            // 
            this.EUR_Label1.AutoSize = true;
            this.EUR_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EUR_Label1.Location = new System.Drawing.Point(8, 63);
            this.EUR_Label1.Name = "EUR_Label1";
            this.EUR_Label1.Size = new System.Drawing.Size(37, 13);
            this.EUR_Label1.TabIndex = 2;
            this.EUR_Label1.Text = "EUR:";
            // 
            // NZD_Label1
            // 
            this.NZD_Label1.AutoSize = true;
            this.NZD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NZD_Label1.Location = new System.Drawing.Point(7, 43);
            this.NZD_Label1.Name = "NZD_Label1";
            this.NZD_Label1.Size = new System.Drawing.Size(37, 13);
            this.NZD_Label1.TabIndex = 1;
            this.NZD_Label1.Text = "NZD:";
            // 
            // AUD_Label1
            // 
            this.AUD_Label1.AutoSize = true;
            this.AUD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AUD_Label1.Location = new System.Drawing.Point(7, 23);
            this.AUD_Label1.Name = "AUD_Label1";
            this.AUD_Label1.Size = new System.Drawing.Size(37, 13);
            this.AUD_Label1.TabIndex = 0;
            this.AUD_Label1.Text = "AUD:";
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
            this.IR_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.IR_Eagel_Transparent___small_faded2;
            this.IR_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.IR_GroupBox.Controls.Add(this.IR_USDT_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_USDT_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_USDT_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_BSV_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_BSV_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_BSV_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_ETC_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_ETC_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_ETC_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_GNT_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_GNT_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_GNT_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_REP_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_REP_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_REP_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_BAT_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_BAT_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_BAT_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_CurrencyBox);
            this.IR_GroupBox.Controls.Add(this.IR_XLM_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_XLM_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_XLM_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_Reset_Button);
            this.IR_GroupBox.Controls.Add(this.IR_EOS_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_EOS_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_EOS_Label1);
            this.IR_GroupBox.Controls.Add(this.SpreadVolumeTitle_Label);
            this.IR_GroupBox.Controls.Add(this.IR_ZRX_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_ZRX_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_OMG_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_OMG_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_OMG_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_XRP_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_ZRX_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_XRP_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_AvgPrice_Label);
            this.IR_GroupBox.Controls.Add(this.IR_CryptoComboBox);
            this.IR_GroupBox.Controls.Add(this.IR_NumCoinsTextBox);
            this.IR_GroupBox.Controls.Add(this.IR_BuySellComboBox);
            this.IR_GroupBox.Controls.Add(this.IR_XBT_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_ETH_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_BCH_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_LTC_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_LTC_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_BCH_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_ETH_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_XBT_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_LTC_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_BCH_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_ETH_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_XBT_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_XRP_Label3);
            this.IR_GroupBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IR_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.IR_GroupBox.Location = new System.Drawing.Point(19, 8);
            this.IR_GroupBox.Name = "IR_GroupBox";
            this.IR_GroupBox.Size = new System.Drawing.Size(263, 383);
            this.IR_GroupBox.TabIndex = 0;
            this.IR_GroupBox.TabStop = false;
            this.IR_GroupBox.Text = "Independent Reserve";
            this.IR_GroupBox.Click += new System.EventHandler(this.IR_GroupBox_Click);
            // 
            // IR_USDT_Label2
            // 
            this.IR_USDT_Label2.AutoSize = true;
            this.IR_USDT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_USDT_Label2.Location = new System.Drawing.Point(70, 89);
            this.IR_USDT_Label2.Name = "IR_USDT_Label2";
            this.IR_USDT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_USDT_Label2.TabIndex = 53;
            this.IR_USDT_Label2.Tag = "IR";
            this.IR_USDT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_USDT_Label3
            // 
            this.IR_USDT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_USDT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_USDT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_USDT_Label3.Location = new System.Drawing.Point(119, 89);
            this.IR_USDT_Label3.Name = "IR_USDT_Label3";
            this.IR_USDT_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_USDT_Label3.TabIndex = 54;
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
            this.IR_USDT_Label1.Location = new System.Drawing.Point(6, 89);
            this.IR_USDT_Label1.Name = "IR_USDT_Label1";
            this.IR_USDT_Label1.Size = new System.Drawing.Size(45, 13);
            this.IR_USDT_Label1.TabIndex = 52;
            this.IR_USDT_Label1.Text = "USDT:";
            this.IR_USDT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BSV_Label2
            // 
            this.IR_BSV_Label2.AutoSize = true;
            this.IR_BSV_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_BSV_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BSV_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BSV_Label2.Location = new System.Drawing.Point(70, 149);
            this.IR_BSV_Label2.Name = "IR_BSV_Label2";
            this.IR_BSV_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_BSV_Label2.TabIndex = 50;
            this.IR_BSV_Label2.Tag = "IR";
            this.IR_BSV_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BSV_Label3
            // 
            this.IR_BSV_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_BSV_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_BSV_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BSV_Label3.Location = new System.Drawing.Point(119, 149);
            this.IR_BSV_Label3.Name = "IR_BSV_Label3";
            this.IR_BSV_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_BSV_Label3.TabIndex = 51;
            this.IR_BSV_Label3.Tag = "";
            this.IR_BSV_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_BSV_Label1
            // 
            this.IR_BSV_Label1.AutoSize = true;
            this.IR_BSV_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_BSV_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BSV_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BSV_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_BSV_Label1.Location = new System.Drawing.Point(6, 149);
            this.IR_BSV_Label1.Name = "IR_BSV_Label1";
            this.IR_BSV_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_BSV_Label1.TabIndex = 49;
            this.IR_BSV_Label1.Text = "BSV:";
            this.IR_BSV_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETC_Label2
            // 
            this.IR_ETC_Label2.AutoSize = true;
            this.IR_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETC_Label2.Location = new System.Drawing.Point(70, 189);
            this.IR_ETC_Label2.Name = "IR_ETC_Label2";
            this.IR_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ETC_Label2.TabIndex = 47;
            this.IR_ETC_Label2.Tag = "IR";
            this.IR_ETC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETC_Label3
            // 
            this.IR_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETC_Label3.Location = new System.Drawing.Point(119, 189);
            this.IR_ETC_Label3.Name = "IR_ETC_Label3";
            this.IR_ETC_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_ETC_Label3.TabIndex = 48;
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
            this.IR_ETC_Label1.Location = new System.Drawing.Point(6, 189);
            this.IR_ETC_Label1.Name = "IR_ETC_Label1";
            this.IR_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_ETC_Label1.TabIndex = 46;
            this.IR_ETC_Label1.Text = "ETC:";
            this.IR_ETC_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_GNT_Label2
            // 
            this.IR_GNT_Label2.AutoSize = true;
            this.IR_GNT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_GNT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GNT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_GNT_Label2.Location = new System.Drawing.Point(70, 309);
            this.IR_GNT_Label2.Name = "IR_GNT_Label2";
            this.IR_GNT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_GNT_Label2.TabIndex = 45;
            this.IR_GNT_Label2.Tag = "IR";
            this.IR_GNT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_GNT_Label1
            // 
            this.IR_GNT_Label1.AutoSize = true;
            this.IR_GNT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_GNT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GNT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_GNT_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_GNT_Label1.Location = new System.Drawing.Point(6, 309);
            this.IR_GNT_Label1.Name = "IR_GNT_Label1";
            this.IR_GNT_Label1.Size = new System.Drawing.Size(37, 13);
            this.IR_GNT_Label1.TabIndex = 44;
            this.IR_GNT_Label1.Text = "GNT:";
            this.IR_GNT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_GNT_Label3
            // 
            this.IR_GNT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_GNT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_GNT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GNT_Label3.Location = new System.Drawing.Point(119, 309);
            this.IR_GNT_Label3.Name = "IR_GNT_Label3";
            this.IR_GNT_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_GNT_Label3.TabIndex = 43;
            this.IR_GNT_Label3.Tag = "";
            this.IR_GNT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_REP_Label2
            // 
            this.IR_REP_Label2.AutoSize = true;
            this.IR_REP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_REP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_REP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_REP_Label2.Location = new System.Drawing.Point(70, 289);
            this.IR_REP_Label2.Name = "IR_REP_Label2";
            this.IR_REP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_REP_Label2.TabIndex = 42;
            this.IR_REP_Label2.Tag = "IR";
            this.IR_REP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_REP_Label1
            // 
            this.IR_REP_Label1.AutoSize = true;
            this.IR_REP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_REP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_REP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_REP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_REP_Label1.Location = new System.Drawing.Point(6, 289);
            this.IR_REP_Label1.Name = "IR_REP_Label1";
            this.IR_REP_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_REP_Label1.TabIndex = 41;
            this.IR_REP_Label1.Text = "REP:";
            this.IR_REP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_REP_Label3
            // 
            this.IR_REP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_REP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_REP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_REP_Label3.Location = new System.Drawing.Point(119, 289);
            this.IR_REP_Label3.Name = "IR_REP_Label3";
            this.IR_REP_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_REP_Label3.TabIndex = 40;
            this.IR_REP_Label3.Tag = "";
            this.IR_REP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_BAT_Label2
            // 
            this.IR_BAT_Label2.AutoSize = true;
            this.IR_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BAT_Label2.Location = new System.Drawing.Point(70, 269);
            this.IR_BAT_Label2.Name = "IR_BAT_Label2";
            this.IR_BAT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_BAT_Label2.TabIndex = 39;
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
            this.IR_BAT_Label1.Location = new System.Drawing.Point(6, 269);
            this.IR_BAT_Label1.Name = "IR_BAT_Label1";
            this.IR_BAT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_BAT_Label1.TabIndex = 38;
            this.IR_BAT_Label1.Text = "BAT:";
            this.IR_BAT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BAT_Label3
            // 
            this.IR_BAT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_BAT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_BAT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BAT_Label3.Location = new System.Drawing.Point(119, 269);
            this.IR_BAT_Label3.Name = "IR_BAT_Label3";
            this.IR_BAT_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_BAT_Label3.TabIndex = 37;
            this.IR_BAT_Label3.Tag = "";
            this.IR_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_CurrencyBox
            // 
            this.IR_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_CurrencyBox.FormattingEnabled = true;
            this.IR_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.IR_CurrencyBox.Location = new System.Drawing.Point(131, 354);
            this.IR_CurrencyBox.Name = "IR_CurrencyBox";
            this.IR_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.IR_CurrencyBox.TabIndex = 36;
            this.IR_CurrencyBox.SelectedIndexChanged += new System.EventHandler(this.IR_CurrencyBox_SelectedIndexChanged);
            // 
            // IR_XLM_Label2
            // 
            this.IR_XLM_Label2.AutoSize = true;
            this.IR_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XLM_Label2.Location = new System.Drawing.Point(70, 209);
            this.IR_XLM_Label2.Name = "IR_XLM_Label2";
            this.IR_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XLM_Label2.TabIndex = 34;
            this.IR_XLM_Label2.Tag = "IR";
            this.IR_XLM_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XLM_Label3
            // 
            this.IR_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XLM_Label3.Location = new System.Drawing.Point(119, 209);
            this.IR_XLM_Label3.Name = "IR_XLM_Label3";
            this.IR_XLM_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_XLM_Label3.TabIndex = 35;
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
            this.IR_XLM_Label1.Location = new System.Drawing.Point(6, 209);
            this.IR_XLM_Label1.Name = "IR_XLM_Label1";
            this.IR_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_XLM_Label1.TabIndex = 33;
            this.IR_XLM_Label1.Text = "XLM:";
            this.IR_XLM_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_Reset_Button
            // 
            this.IR_Reset_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_Reset_Button.ForeColor = System.Drawing.Color.Black;
            this.IR_Reset_Button.Location = new System.Drawing.Point(207, 326);
            this.IR_Reset_Button.Name = "IR_Reset_Button";
            this.IR_Reset_Button.Size = new System.Drawing.Size(44, 17);
            this.IR_Reset_Button.TabIndex = 32;
            this.IR_Reset_Button.Text = "Reset";
            this.IR_Reset_Button.UseVisualStyleBackColor = true;
            this.IR_Reset_Button.Click += new System.EventHandler(this.IR_Reset_Button_Click);
            // 
            // IR_EOS_Label2
            // 
            this.IR_EOS_Label2.AutoSize = true;
            this.IR_EOS_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_EOS_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_EOS_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_EOS_Label2.Location = new System.Drawing.Point(70, 109);
            this.IR_EOS_Label2.Name = "IR_EOS_Label2";
            this.IR_EOS_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_EOS_Label2.TabIndex = 30;
            this.IR_EOS_Label2.Tag = "IR";
            this.IR_EOS_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_EOS_Label3
            // 
            this.IR_EOS_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_EOS_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_EOS_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_EOS_Label3.Location = new System.Drawing.Point(119, 109);
            this.IR_EOS_Label3.Name = "IR_EOS_Label3";
            this.IR_EOS_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_EOS_Label3.TabIndex = 31;
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
            this.IR_EOS_Label1.Location = new System.Drawing.Point(6, 109);
            this.IR_EOS_Label1.Name = "IR_EOS_Label1";
            this.IR_EOS_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_EOS_Label1.TabIndex = 29;
            this.IR_EOS_Label1.Text = "EOS:";
            this.IR_EOS_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // SpreadVolumeTitle_Label
            // 
            this.SpreadVolumeTitle_Label.AutoSize = true;
            this.SpreadVolumeTitle_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadVolumeTitle_Label.Location = new System.Drawing.Point(175, 14);
            this.SpreadVolumeTitle_Label.Name = "SpreadVolumeTitle_Label";
            this.SpreadVolumeTitle_Label.Size = new System.Drawing.Size(73, 12);
            this.SpreadVolumeTitle_Label.TabIndex = 25;
            this.SpreadVolumeTitle_Label.Text = "Spread / Volume";
            // 
            // IR_ZRX_Label2
            // 
            this.IR_ZRX_Label2.AutoSize = true;
            this.IR_ZRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ZRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ZRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ZRX_Label2.Location = new System.Drawing.Point(70, 249);
            this.IR_ZRX_Label2.Name = "IR_ZRX_Label2";
            this.IR_ZRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ZRX_Label2.TabIndex = 24;
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
            this.IR_ZRX_Label1.Location = new System.Drawing.Point(6, 249);
            this.IR_ZRX_Label1.Name = "IR_ZRX_Label1";
            this.IR_ZRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_ZRX_Label1.TabIndex = 23;
            this.IR_ZRX_Label1.Text = "ZRX:";
            this.IR_ZRX_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_OMG_Label2
            // 
            this.IR_OMG_Label2.AutoSize = true;
            this.IR_OMG_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_OMG_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_OMG_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_OMG_Label2.Location = new System.Drawing.Point(70, 229);
            this.IR_OMG_Label2.Name = "IR_OMG_Label2";
            this.IR_OMG_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_OMG_Label2.TabIndex = 20;
            this.IR_OMG_Label2.Tag = "IR";
            this.IR_OMG_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_OMG_Label3
            // 
            this.IR_OMG_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_OMG_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_OMG_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_OMG_Label3.Location = new System.Drawing.Point(119, 229);
            this.IR_OMG_Label3.Name = "IR_OMG_Label3";
            this.IR_OMG_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_OMG_Label3.TabIndex = 22;
            this.IR_OMG_Label3.Tag = "";
            this.IR_OMG_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_OMG_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_OMG_Label3_MouseDoubleClick);
            // 
            // IR_OMG_Label1
            // 
            this.IR_OMG_Label1.AutoSize = true;
            this.IR_OMG_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_OMG_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_OMG_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_OMG_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_OMG_Label1.Location = new System.Drawing.Point(6, 229);
            this.IR_OMG_Label1.Name = "IR_OMG_Label1";
            this.IR_OMG_Label1.Size = new System.Drawing.Size(39, 13);
            this.IR_OMG_Label1.TabIndex = 19;
            this.IR_OMG_Label1.Text = "OMG:";
            this.IR_OMG_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XRP_Label2
            // 
            this.IR_XRP_Label2.AutoSize = true;
            this.IR_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XRP_Label2.Location = new System.Drawing.Point(70, 49);
            this.IR_XRP_Label2.Name = "IR_XRP_Label2";
            this.IR_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XRP_Label2.TabIndex = 17;
            this.IR_XRP_Label2.Tag = "IR";
            this.IR_XRP_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ZRX_Label3
            // 
            this.IR_ZRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_ZRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ZRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ZRX_Label3.Location = new System.Drawing.Point(119, 249);
            this.IR_ZRX_Label3.Name = "IR_ZRX_Label3";
            this.IR_ZRX_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_ZRX_Label3.TabIndex = 18;
            this.IR_ZRX_Label3.Tag = "";
            this.IR_ZRX_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_ZRX_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_ZRX_Label3_MouseDoubleClick);
            // 
            // IR_XRP_Label1
            // 
            this.IR_XRP_Label1.AutoSize = true;
            this.IR_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XRP_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_XRP_Label1.Location = new System.Drawing.Point(6, 49);
            this.IR_XRP_Label1.Name = "IR_XRP_Label1";
            this.IR_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_XRP_Label1.TabIndex = 16;
            this.IR_XRP_Label1.Text = "XRP:";
            this.IR_XRP_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_AvgPrice_Label
            // 
            this.IR_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.IR_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_AvgPrice_Label.Location = new System.Drawing.Point(6, 327);
            this.IR_AvgPrice_Label.Name = "IR_AvgPrice_Label";
            this.IR_AvgPrice_Label.Size = new System.Drawing.Size(194, 16);
            this.IR_AvgPrice_Label.TabIndex = 15;
            // 
            // IR_CryptoComboBox
            // 
            this.IR_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_CryptoComboBox.Location = new System.Drawing.Point(193, 354);
            this.IR_CryptoComboBox.Name = "IR_CryptoComboBox";
            this.IR_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.IR_CryptoComboBox.TabIndex = 14;
            this.IR_CryptoComboBox.DropDown += new System.EventHandler(this.IR_CryptoComboBox_DropDown);
            this.IR_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.IR_CryptoComboBox_SelectedIndexChanged);
            // 
            // IR_NumCoinsTextBox
            // 
            this.IR_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_NumCoinsTextBox.Location = new System.Drawing.Point(58, 354);
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
            this.IR_BuySellComboBox.Location = new System.Drawing.Point(9, 354);
            this.IR_BuySellComboBox.Name = "IR_BuySellComboBox";
            this.IR_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.IR_BuySellComboBox.TabIndex = 12;
            this.IR_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.IR_BuySellComboBox_SelectedIndexChanged);
            // 
            // IR_XBT_Label2
            // 
            this.IR_XBT_Label2.AutoSize = true;
            this.IR_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XBT_Label2.Location = new System.Drawing.Point(70, 29);
            this.IR_XBT_Label2.Name = "IR_XBT_Label2";
            this.IR_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XBT_Label2.TabIndex = 4;
            this.IR_XBT_Label2.Tag = "IR";
            this.IR_XBT_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_ETH_Label2
            // 
            this.IR_ETH_Label2.AutoSize = true;
            this.IR_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETH_Label2.Location = new System.Drawing.Point(70, 69);
            this.IR_ETH_Label2.Name = "IR_ETH_Label2";
            this.IR_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ETH_Label2.TabIndex = 5;
            this.IR_ETH_Label2.Tag = "IR";
            this.IR_ETH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_BCH_Label2
            // 
            this.IR_BCH_Label2.AutoSize = true;
            this.IR_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BCH_Label2.Location = new System.Drawing.Point(70, 129);
            this.IR_BCH_Label2.Name = "IR_BCH_Label2";
            this.IR_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_BCH_Label2.TabIndex = 6;
            this.IR_BCH_Label2.Tag = "IR";
            this.IR_BCH_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_LTC_Label2
            // 
            this.IR_LTC_Label2.AutoSize = true;
            this.IR_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LTC_Label2.Location = new System.Drawing.Point(70, 169);
            this.IR_LTC_Label2.Name = "IR_LTC_Label2";
            this.IR_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_LTC_Label2.TabIndex = 7;
            this.IR_LTC_Label2.Tag = "IR";
            this.IR_LTC_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_LTC_Label3
            // 
            this.IR_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LTC_Label3.Location = new System.Drawing.Point(119, 169);
            this.IR_LTC_Label3.Name = "IR_LTC_Label3";
            this.IR_LTC_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_LTC_Label3.TabIndex = 11;
            this.IR_LTC_Label3.Tag = "";
            this.IR_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_LTC_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_LTC_Label3_MouseDoubleClick);
            // 
            // IR_BCH_Label3
            // 
            this.IR_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BCH_Label3.Location = new System.Drawing.Point(119, 129);
            this.IR_BCH_Label3.Name = "IR_BCH_Label3";
            this.IR_BCH_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_BCH_Label3.TabIndex = 10;
            this.IR_BCH_Label3.Tag = "";
            this.IR_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_BCH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_BCH_Label3_MouseDoubleClick);
            // 
            // IR_ETH_Label3
            // 
            this.IR_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_ETH_Label3.Location = new System.Drawing.Point(119, 69);
            this.IR_ETH_Label3.Name = "IR_ETH_Label3";
            this.IR_ETH_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_ETH_Label3.TabIndex = 9;
            this.IR_ETH_Label3.Tag = "";
            this.IR_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_ETH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_ETH_Label3_MouseDoubleClick);
            // 
            // IR_XBT_Label3
            // 
            this.IR_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XBT_Label3.Location = new System.Drawing.Point(119, 29);
            this.IR_XBT_Label3.Name = "IR_XBT_Label3";
            this.IR_XBT_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_XBT_Label3.TabIndex = 8;
            this.IR_XBT_Label3.Tag = "";
            this.IR_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_XBT_Label3_MouseDoubleClick);
            // 
            // IR_LTC_Label1
            // 
            this.IR_LTC_Label1.AutoSize = true;
            this.IR_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LTC_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_LTC_Label1.Location = new System.Drawing.Point(6, 169);
            this.IR_LTC_Label1.Name = "IR_LTC_Label1";
            this.IR_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.IR_LTC_Label1.TabIndex = 3;
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
            this.IR_BCH_Label1.Location = new System.Drawing.Point(6, 129);
            this.IR_BCH_Label1.Name = "IR_BCH_Label1";
            this.IR_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_BCH_Label1.TabIndex = 2;
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
            this.IR_ETH_Label1.Location = new System.Drawing.Point(6, 69);
            this.IR_ETH_Label1.Name = "IR_ETH_Label1";
            this.IR_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_ETH_Label1.TabIndex = 1;
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
            this.IR_XBT_Label1.Location = new System.Drawing.Point(6, 29);
            this.IR_XBT_Label1.Name = "IR_XBT_Label1";
            this.IR_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_XBT_Label1.TabIndex = 0;
            this.IR_XBT_Label1.Text = "BTC:";
            this.IR_XBT_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_XRP_Label3
            // 
            this.IR_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_XRP_Label3.Location = new System.Drawing.Point(119, 49);
            this.IR_XRP_Label3.Name = "IR_XRP_Label3";
            this.IR_XRP_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_XRP_Label3.TabIndex = 21;
            this.IR_XRP_Label3.Tag = "";
            this.IR_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_XRP_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_XRP_Label3_MouseDoubleClick);
            // 
            // GDAX_GroupBox
            // 
            this.GDAX_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.coinbasepro_logo3;
            this.GDAX_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GDAX_GroupBox.Controls.Add(this.GDAX_CurrencyBox);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETC_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETC_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETC_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_REP_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_REP_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_REP_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XLM_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XLM_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XLM_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XRP_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XRP_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XRP_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ZRX_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ZRX_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ZRX_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_AvgPrice_Label);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_CryptoComboBox);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_NumCoinsTextBox);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XBT_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BuySellComboBox);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETH_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BCH_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_LTC_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_LTC_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BCH_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETH_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XBT_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_LTC_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BCH_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETH_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XBT_Label1);
            this.GDAX_GroupBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GDAX_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.GDAX_GroupBox.Location = new System.Drawing.Point(19, 397);
            this.GDAX_GroupBox.Name = "GDAX_GroupBox";
            this.GDAX_GroupBox.Size = new System.Drawing.Size(263, 262);
            this.GDAX_GroupBox.TabIndex = 8;
            this.GDAX_GroupBox.TabStop = false;
            this.GDAX_GroupBox.Text = "Coinbase Pro";
            this.GDAX_GroupBox.Click += new System.EventHandler(this.GDAX_GroupBox_Click);
            // 
            // GDAX_ETC_Label2
            // 
            this.GDAX_ETC_Label2.AutoSize = true;
            this.GDAX_ETC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ETC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ETC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ETC_Label2.Location = new System.Drawing.Point(45, 123);
            this.GDAX_ETC_Label2.Name = "GDAX_ETC_Label2";
            this.GDAX_ETC_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_ETC_Label2.TabIndex = 32;
            this.GDAX_ETC_Label2.Tag = "GDAX";
            // 
            // GDAX_ETC_Label3
            // 
            this.GDAX_ETC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_ETC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ETC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ETC_Label3.Location = new System.Drawing.Point(119, 123);
            this.GDAX_ETC_Label3.Name = "GDAX_ETC_Label3";
            this.GDAX_ETC_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_ETC_Label3.TabIndex = 33;
            this.GDAX_ETC_Label3.Tag = "";
            this.GDAX_ETC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_ETC_Label1
            // 
            this.GDAX_ETC_Label1.AutoSize = true;
            this.GDAX_ETC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ETC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ETC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ETC_Label1.Location = new System.Drawing.Point(6, 123);
            this.GDAX_ETC_Label1.Name = "GDAX_ETC_Label1";
            this.GDAX_ETC_Label1.Size = new System.Drawing.Size(35, 13);
            this.GDAX_ETC_Label1.TabIndex = 31;
            this.GDAX_ETC_Label1.Text = "ETC:";
            // 
            // GDAX_REP_Label2
            // 
            this.GDAX_REP_Label2.AutoSize = true;
            this.GDAX_REP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_REP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_REP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_REP_Label2.Location = new System.Drawing.Point(45, 183);
            this.GDAX_REP_Label2.Name = "GDAX_REP_Label2";
            this.GDAX_REP_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_REP_Label2.TabIndex = 29;
            this.GDAX_REP_Label2.Tag = "GDAX";
            // 
            // GDAX_REP_Label3
            // 
            this.GDAX_REP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_REP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_REP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_REP_Label3.Location = new System.Drawing.Point(119, 183);
            this.GDAX_REP_Label3.Name = "GDAX_REP_Label3";
            this.GDAX_REP_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_REP_Label3.TabIndex = 30;
            this.GDAX_REP_Label3.Tag = "";
            this.GDAX_REP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_REP_Label1
            // 
            this.GDAX_REP_Label1.AutoSize = true;
            this.GDAX_REP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_REP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_REP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_REP_Label1.Location = new System.Drawing.Point(6, 183);
            this.GDAX_REP_Label1.Name = "GDAX_REP_Label1";
            this.GDAX_REP_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_REP_Label1.TabIndex = 28;
            this.GDAX_REP_Label1.Text = "REP:";
            // 
            // GDAX_XLM_Label2
            // 
            this.GDAX_XLM_Label2.AutoSize = true;
            this.GDAX_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XLM_Label2.Location = new System.Drawing.Point(45, 143);
            this.GDAX_XLM_Label2.Name = "GDAX_XLM_Label2";
            this.GDAX_XLM_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_XLM_Label2.TabIndex = 26;
            this.GDAX_XLM_Label2.Tag = "GDAX";
            // 
            // GDAX_XLM_Label3
            // 
            this.GDAX_XLM_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_XLM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XLM_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XLM_Label3.Location = new System.Drawing.Point(119, 143);
            this.GDAX_XLM_Label3.Name = "GDAX_XLM_Label3";
            this.GDAX_XLM_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_XLM_Label3.TabIndex = 27;
            this.GDAX_XLM_Label3.Tag = "";
            this.GDAX_XLM_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_XLM_Label1
            // 
            this.GDAX_XLM_Label1.AutoSize = true;
            this.GDAX_XLM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XLM_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XLM_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XLM_Label1.Location = new System.Drawing.Point(6, 143);
            this.GDAX_XLM_Label1.Name = "GDAX_XLM_Label1";
            this.GDAX_XLM_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_XLM_Label1.TabIndex = 25;
            this.GDAX_XLM_Label1.Text = "XLM:";
            // 
            // GDAX_XRP_Label2
            // 
            this.GDAX_XRP_Label2.AutoSize = true;
            this.GDAX_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XRP_Label2.Location = new System.Drawing.Point(45, 43);
            this.GDAX_XRP_Label2.Name = "GDAX_XRP_Label2";
            this.GDAX_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_XRP_Label2.TabIndex = 23;
            this.GDAX_XRP_Label2.Tag = "GDAX";
            // 
            // GDAX_XRP_Label3
            // 
            this.GDAX_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XRP_Label3.Location = new System.Drawing.Point(119, 43);
            this.GDAX_XRP_Label3.Name = "GDAX_XRP_Label3";
            this.GDAX_XRP_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_XRP_Label3.TabIndex = 24;
            this.GDAX_XRP_Label3.Tag = "";
            this.GDAX_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_XRP_Label1
            // 
            this.GDAX_XRP_Label1.AutoSize = true;
            this.GDAX_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XRP_Label1.Location = new System.Drawing.Point(6, 43);
            this.GDAX_XRP_Label1.Name = "GDAX_XRP_Label1";
            this.GDAX_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_XRP_Label1.TabIndex = 22;
            this.GDAX_XRP_Label1.Text = "XRP:";
            // 
            // GDAX_ZRX_Label2
            // 
            this.GDAX_ZRX_Label2.AutoSize = true;
            this.GDAX_ZRX_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ZRX_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ZRX_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ZRX_Label2.Location = new System.Drawing.Point(45, 163);
            this.GDAX_ZRX_Label2.Name = "GDAX_ZRX_Label2";
            this.GDAX_ZRX_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_ZRX_Label2.TabIndex = 20;
            this.GDAX_ZRX_Label2.Tag = "GDAX";
            // 
            // GDAX_ZRX_Label3
            // 
            this.GDAX_ZRX_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_ZRX_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ZRX_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ZRX_Label3.Location = new System.Drawing.Point(119, 163);
            this.GDAX_ZRX_Label3.Name = "GDAX_ZRX_Label3";
            this.GDAX_ZRX_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_ZRX_Label3.TabIndex = 21;
            this.GDAX_ZRX_Label3.Tag = "";
            this.GDAX_ZRX_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_ZRX_Label1
            // 
            this.GDAX_ZRX_Label1.AutoSize = true;
            this.GDAX_ZRX_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ZRX_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ZRX_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ZRX_Label1.Location = new System.Drawing.Point(6, 163);
            this.GDAX_ZRX_Label1.Name = "GDAX_ZRX_Label1";
            this.GDAX_ZRX_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_ZRX_Label1.TabIndex = 19;
            this.GDAX_ZRX_Label1.Text = "ZRX:";
            // 
            // GDAX_AvgPrice_Label
            // 
            this.GDAX_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.GDAX_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_AvgPrice_Label.Location = new System.Drawing.Point(6, 201);
            this.GDAX_AvgPrice_Label.Name = "GDAX_AvgPrice_Label";
            this.GDAX_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.GDAX_AvgPrice_Label.TabIndex = 18;
            // 
            // GDAX_CryptoComboBox
            // 
            this.GDAX_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GDAX_CryptoComboBox.Location = new System.Drawing.Point(193, 231);
            this.GDAX_CryptoComboBox.Name = "GDAX_CryptoComboBox";
            this.GDAX_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.GDAX_CryptoComboBox.TabIndex = 17;
            this.GDAX_CryptoComboBox.DropDown += new System.EventHandler(this.GDAX_CryptoComboBox_DropDown);
            this.GDAX_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.GDAX_CryptoComboBox_SelectedIndexChanged);
            // 
            // GDAX_NumCoinsTextBox
            // 
            this.GDAX_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_NumCoinsTextBox.Location = new System.Drawing.Point(58, 231);
            this.GDAX_NumCoinsTextBox.Name = "GDAX_NumCoinsTextBox";
            this.GDAX_NumCoinsTextBox.PromptChar = ' ';
            this.GDAX_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.GDAX_NumCoinsTextBox.TabIndex = 16;
            this.GDAX_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GDAX_NumCoinsTextBox.ValidatingType = typeof(int);
            this.GDAX_NumCoinsTextBox.TextChanged += new System.EventHandler(this.GDAX_NumCoinsTextBox_TextChanged);
            this.GDAX_NumCoinsTextBox.Enter += new System.EventHandler(this.GDAX_NumCoinsTextBox_Enter);
            // 
            // GDAX_XBT_Label2
            // 
            this.GDAX_XBT_Label2.AutoSize = true;
            this.GDAX_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XBT_Label2.Location = new System.Drawing.Point(45, 23);
            this.GDAX_XBT_Label2.Name = "GDAX_XBT_Label2";
            this.GDAX_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_XBT_Label2.TabIndex = 4;
            this.GDAX_XBT_Label2.Tag = "GDAX";
            // 
            // GDAX_BuySellComboBox
            // 
            this.GDAX_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GDAX_BuySellComboBox.FormattingEnabled = true;
            this.GDAX_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.GDAX_BuySellComboBox.Location = new System.Drawing.Point(9, 231);
            this.GDAX_BuySellComboBox.Name = "GDAX_BuySellComboBox";
            this.GDAX_BuySellComboBox.Size = new System.Drawing.Size(46, 21);
            this.GDAX_BuySellComboBox.TabIndex = 15;
            this.GDAX_BuySellComboBox.SelectedIndexChanged += new System.EventHandler(this.GDAX_BuySellComboBox_SelectedIndexChanged);
            // 
            // GDAX_ETH_Label2
            // 
            this.GDAX_ETH_Label2.AutoSize = true;
            this.GDAX_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ETH_Label2.Location = new System.Drawing.Point(45, 63);
            this.GDAX_ETH_Label2.Name = "GDAX_ETH_Label2";
            this.GDAX_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_ETH_Label2.TabIndex = 5;
            this.GDAX_ETH_Label2.Tag = "GDAX";
            // 
            // GDAX_BCH_Label2
            // 
            this.GDAX_BCH_Label2.AutoSize = true;
            this.GDAX_BCH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_BCH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_BCH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_BCH_Label2.Location = new System.Drawing.Point(45, 83);
            this.GDAX_BCH_Label2.Name = "GDAX_BCH_Label2";
            this.GDAX_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_BCH_Label2.TabIndex = 6;
            this.GDAX_BCH_Label2.Tag = "GDAX";
            // 
            // GDAX_LTC_Label2
            // 
            this.GDAX_LTC_Label2.AutoSize = true;
            this.GDAX_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_LTC_Label2.Location = new System.Drawing.Point(45, 103);
            this.GDAX_LTC_Label2.Name = "GDAX_LTC_Label2";
            this.GDAX_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_LTC_Label2.TabIndex = 7;
            this.GDAX_LTC_Label2.Tag = "GDAX";
            // 
            // GDAX_LTC_Label3
            // 
            this.GDAX_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_LTC_Label3.Location = new System.Drawing.Point(119, 103);
            this.GDAX_LTC_Label3.Name = "GDAX_LTC_Label3";
            this.GDAX_LTC_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_LTC_Label3.TabIndex = 15;
            this.GDAX_LTC_Label3.Tag = "";
            this.GDAX_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.GDAX_LTC_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GDAX_LTC_Label3_MouseDoubleClick);
            // 
            // GDAX_BCH_Label3
            // 
            this.GDAX_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_BCH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_BCH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_BCH_Label3.Location = new System.Drawing.Point(119, 83);
            this.GDAX_BCH_Label3.Name = "GDAX_BCH_Label3";
            this.GDAX_BCH_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_BCH_Label3.TabIndex = 14;
            this.GDAX_BCH_Label3.Tag = "";
            this.GDAX_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.GDAX_BCH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GDAX_BCH_Label3_MouseDoubleClick);
            // 
            // GDAX_ETH_Label3
            // 
            this.GDAX_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ETH_Label3.Location = new System.Drawing.Point(119, 63);
            this.GDAX_ETH_Label3.Name = "GDAX_ETH_Label3";
            this.GDAX_ETH_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_ETH_Label3.TabIndex = 13;
            this.GDAX_ETH_Label3.Tag = "";
            this.GDAX_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.GDAX_ETH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GDAX_ETH_Label3_MouseDoubleClick);
            // 
            // GDAX_XBT_Label3
            // 
            this.GDAX_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XBT_Label3.Location = new System.Drawing.Point(119, 23);
            this.GDAX_XBT_Label3.Name = "GDAX_XBT_Label3";
            this.GDAX_XBT_Label3.Size = new System.Drawing.Size(134, 13);
            this.GDAX_XBT_Label3.TabIndex = 12;
            this.GDAX_XBT_Label3.Tag = "";
            this.GDAX_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.GDAX_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GDAX_XBT_Label3_MouseDoubleClick);
            // 
            // GDAX_LTC_Label1
            // 
            this.GDAX_LTC_Label1.AutoSize = true;
            this.GDAX_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_LTC_Label1.Location = new System.Drawing.Point(6, 103);
            this.GDAX_LTC_Label1.Name = "GDAX_LTC_Label1";
            this.GDAX_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.GDAX_LTC_Label1.TabIndex = 3;
            this.GDAX_LTC_Label1.Text = "LTC:";
            // 
            // GDAX_BCH_Label1
            // 
            this.GDAX_BCH_Label1.AutoSize = true;
            this.GDAX_BCH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_BCH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_BCH_Label1.Location = new System.Drawing.Point(6, 83);
            this.GDAX_BCH_Label1.Name = "GDAX_BCH_Label1";
            this.GDAX_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_BCH_Label1.TabIndex = 2;
            this.GDAX_BCH_Label1.Text = "BCH:";
            // 
            // GDAX_ETH_Label1
            // 
            this.GDAX_ETH_Label1.AutoSize = true;
            this.GDAX_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ETH_Label1.Location = new System.Drawing.Point(6, 63);
            this.GDAX_ETH_Label1.Name = "GDAX_ETH_Label1";
            this.GDAX_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_ETH_Label1.TabIndex = 1;
            this.GDAX_ETH_Label1.Text = "ETH:";
            // 
            // GDAX_XBT_Label1
            // 
            this.GDAX_XBT_Label1.AutoSize = true;
            this.GDAX_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.GDAX_XBT_Label1.Name = "GDAX_XBT_Label1";
            this.GDAX_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.GDAX_XBT_Label1.TabIndex = 0;
            this.GDAX_XBT_Label1.Text = "BTC:";
            // 
            // IRTickerTT
            // 
            this.IRTickerTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IRTickerTT.ToolTipTitle = "Spread details";
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
            // BlinkStickBW
            // 
            this.BlinkStickBW.WorkerSupportsCancellation = true;
            this.BlinkStickBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BlinkStickBW_DoWork);
            this.BlinkStickBW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BlinkStickBW_RunWorkerCompleted);
            // 
            // BlinkStickWhite_Thread
            // 
            this.BlinkStickWhite_Thread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BlinkStickWhite_Thread_DoWork);
            // 
            // GDAX_CurrencyBox
            // 
            this.GDAX_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GDAX_CurrencyBox.FormattingEnabled = true;
            this.GDAX_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.GDAX_CurrencyBox.Location = new System.Drawing.Point(131, 231);
            this.GDAX_CurrencyBox.Name = "GDAX_CurrencyBox";
            this.GDAX_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.GDAX_CurrencyBox.TabIndex = 55;
            // 
            // BFX_CurrencyBox
            // 
            this.BFX_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BFX_CurrencyBox.FormattingEnabled = true;
            this.BFX_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.BFX_CurrencyBox.Location = new System.Drawing.Point(131, 350);
            this.BFX_CurrencyBox.Name = "BFX_CurrencyBox";
            this.BFX_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.BFX_CurrencyBox.TabIndex = 56;
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
            this.BTCM_CurrencyBox.Location = new System.Drawing.Point(131, 271);
            this.BTCM_CurrencyBox.Name = "BTCM_CurrencyBox";
            this.BTCM_CurrencyBox.Size = new System.Drawing.Size(58, 21);
            this.BTCM_CurrencyBox.TabIndex = 61;
            // 
            // SessionStart_label
            // 
            this.SessionStart_label.AutoSize = true;
            this.SessionStart_label.Location = new System.Drawing.Point(252, 784);
            this.SessionStart_label.Name = "SessionStart_label";
            this.SessionStart_label.Size = new System.Drawing.Size(82, 13);
            this.SessionStart_label.TabIndex = 38;
            this.SessionStart_label.Text = "Session started:";
            // 
            // SessionStartedAbs_label
            // 
            this.SessionStartedAbs_label.AutoSize = true;
            this.SessionStartedAbs_label.Location = new System.Drawing.Point(338, 784);
            this.SessionStartedAbs_label.Name = "SessionStartedAbs_label";
            this.SessionStartedAbs_label.Size = new System.Drawing.Size(0, 13);
            this.SessionStartedAbs_label.TabIndex = 39;
            // 
            // SessionStartedRel_label
            // 
            this.SessionStartedRel_label.AutoSize = true;
            this.SessionStartedRel_label.Location = new System.Drawing.Point(338, 813);
            this.SessionStartedRel_label.Name = "SessionStartedRel_label";
            this.SessionStartedRel_label.Size = new System.Drawing.Size(0, 13);
            this.SessionStartedRel_label.TabIndex = 40;
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
            // IRTicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 841);
            this.Controls.Add(this.LoadingPanel);
            this.Controls.Add(this.Main);
            this.Controls.Add(this.Settings);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.LoadingPanel.ResumeLayout(false);
            this.Main.ResumeLayout(false);
            this.Main.PerformLayout();
            this.BTCM_GroupBox.ResumeLayout(false);
            this.BTCM_GroupBox.PerformLayout();
            this.BAR_GroupBox.ResumeLayout(false);
            this.BAR_GroupBox.PerformLayout();
            this.BFX_GroupBox.ResumeLayout(false);
            this.BFX_GroupBox.PerformLayout();
            this.fiat_GroupBox.ResumeLayout(false);
            this.fiat_GroupBox.PerformLayout();
            this.IR_GroupBox.ResumeLayout(false);
            this.IR_GroupBox.PerformLayout();
            this.GDAX_GroupBox.ResumeLayout(false);
            this.GDAX_GroupBox.PerformLayout();
            this.OTCHelper.ResumeLayout(false);
            this.OTCHelper.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox refreshFrequencyTextbox;
        private System.Windows.Forms.Label refreshFrequencyLabel;
        private System.ComponentModel.BackgroundWorker pollingThread;
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
        private System.Windows.Forms.Label IR_XBT_Label2;
        private System.Windows.Forms.Label IR_ETH_Label2;
        private System.Windows.Forms.Label IR_BCH_Label2;
        private System.Windows.Forms.Label IR_LTC_Label2;
        private System.Windows.Forms.Label IR_LTC_Label1;
        private System.Windows.Forms.Label IR_BCH_Label1;
        private System.Windows.Forms.Label IR_ETH_Label1;
        private System.Windows.Forms.Label IR_XBT_Label1;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Button SettingsOKButton;
        private System.Windows.Forms.GroupBox GDAX_GroupBox;
        private System.Windows.Forms.Label GDAX_XBT_Label2;
        private System.Windows.Forms.Label GDAX_ETH_Label2;
        private System.Windows.Forms.Label GDAX_BCH_Label2;
        private System.Windows.Forms.Label GDAX_LTC_Label2;
        private System.Windows.Forms.Label GDAX_LTC_Label1;
        private System.Windows.Forms.Label GDAX_BCH_Label1;
        private System.Windows.Forms.Label GDAX_ETH_Label1;
        private System.Windows.Forms.Label GDAX_XBT_Label1;
        private System.Windows.Forms.GroupBox fiat_GroupBox;
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
        private System.Windows.Forms.GroupBox BFX_GroupBox;
        private System.Windows.Forms.Label BFX_XBT_Label2;
        private System.Windows.Forms.Label BFX_ETH_Label2;
        private System.Windows.Forms.Label BFX_BCH_Label2;
        private System.Windows.Forms.Label BFX_LTC_Label2;
        private System.Windows.Forms.Label BFX_LTC_Label1;
        private System.Windows.Forms.Label BFX_BCH_Label1;
        private System.Windows.Forms.Label BFX_ETH_Label1;
        private System.Windows.Forms.Label BFX_XBT_Label1;
        private System.Windows.Forms.Label BFX_LTC_Label3;
        private System.Windows.Forms.Label BFX_BCH_Label3;
        private System.Windows.Forms.Label BFX_ETH_Label3;
        private System.Windows.Forms.Label BFX_XBT_Label3;
        private System.Windows.Forms.Label GDAX_LTC_Label3;
        private System.Windows.Forms.Label GDAX_BCH_Label3;
        private System.Windows.Forms.Label GDAX_ETH_Label3;
        private System.Windows.Forms.Label GDAX_XBT_Label3;
        private System.Windows.Forms.Label BTCM_XRP_Label3;
        private System.Windows.Forms.Label BTCM_LTC_Label3;
        private System.Windows.Forms.Label BTCM_BCH_Label3;
        private System.Windows.Forms.Label BTCM_ETH_Label3;
        private System.Windows.Forms.Label BTCM_XBT_Label3;
        private System.Windows.Forms.Label IR_LTC_Label3;
        private System.Windows.Forms.Label IR_BCH_Label3;
        private System.Windows.Forms.Label IR_ETH_Label3;
        private System.Windows.Forms.Label IR_XBT_Label3;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ComboBox IR_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox IR_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox IR_BuySellComboBox;
        private System.Windows.Forms.ComboBox BFX_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox BFX_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox BFX_BuySellComboBox;
        private System.Windows.Forms.ComboBox GDAX_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox GDAX_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox GDAX_BuySellComboBox;
        private System.Windows.Forms.ComboBox BTCM_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox BTCM_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox BTCM_BuySellComboBox;
        private System.Windows.Forms.Label BFX_AvgPrice_Label;
        private System.Windows.Forms.Label GDAX_AvgPrice_Label;
        private System.Windows.Forms.Label BTCM_AvgPrice_Label;
        private System.Windows.Forms.Label IR_AvgPrice_Label;
        private System.Windows.Forms.CheckBox EnableGDAXLevel3_CheckBox;
        private System.Windows.Forms.Label EnableGDAXLevel3;
        private System.Windows.Forms.ToolTip IRTickerTT;
        private System.Windows.Forms.Label BFX_XRP_Label2;
        private System.Windows.Forms.Label BFX_XRP_Label3;
        private System.Windows.Forms.Label BFX_XRP_Label1;
        public System.Windows.Forms.Button Help_Button;
        private System.Windows.Forms.CheckBox ExportSummarised_Checkbox;
        private System.Windows.Forms.Label ExportSummarised_Label;
        private System.Windows.Forms.Label IR_XRP_Label2;
        private System.Windows.Forms.Label IR_ZRX_Label3;
        private System.Windows.Forms.Label IR_XRP_Label1;
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
        private System.Windows.Forms.Label BTCM_OMG_Label2;
        private System.Windows.Forms.Label BTCM_OMG_Label3;
        private System.Windows.Forms.Label BTCM_OMG_Label1;
        private System.Windows.Forms.Label BFX_ZRX_Label2;
        private System.Windows.Forms.Label BFX_ZRX_Label1;
        private System.Windows.Forms.Label BFX_OMG_Label2;
        private System.Windows.Forms.Label BFX_OMG_Label3;
        private System.Windows.Forms.Label BFX_ZRX_Label3;
        private System.Windows.Forms.Label BFX_OMG_Label1;
        private System.Windows.Forms.Label IR_ZRX_Label2;
        private System.Windows.Forms.Label IR_ZRX_Label1;
        private System.Windows.Forms.Label IR_OMG_Label2;
        private System.Windows.Forms.Label IR_OMG_Label3;
        private System.Windows.Forms.Label IR_XRP_Label3;
        private System.Windows.Forms.Label IR_OMG_Label1;
        private System.Windows.Forms.Label SpreadVolumeTitle_Label;
        private System.Windows.Forms.Label GDAX_ZRX_Label2;
        private System.Windows.Forms.Label GDAX_ZRX_Label3;
        private System.Windows.Forms.Label GDAX_ZRX_Label1;
        private System.Windows.Forms.Label GDAX_XRP_Label2;
        private System.Windows.Forms.Label GDAX_XRP_Label3;
        private System.Windows.Forms.Label GDAX_XRP_Label1;
        private System.Windows.Forms.Label BFX_EOS_Label2;
        private System.Windows.Forms.Label BFX_EOS_Label3;
        private System.Windows.Forms.Label BFX_EOS_Label1;
        private System.Windows.Forms.Label IR_EOS_Label2;
        private System.Windows.Forms.Label IR_EOS_Label3;
        private System.Windows.Forms.Label IR_EOS_Label1;
        private System.Windows.Forms.Button IR_Reset_Button;
        private System.Windows.Forms.Label BTCM_XLM_Label2;
        private System.Windows.Forms.Label BTCM_XLM_Label3;
        private System.Windows.Forms.Label BTCM_XLM_Label1;
        private System.Windows.Forms.Label BFX_XLM_Label2;
        private System.Windows.Forms.Label BFX_XLM_Label3;
        private System.Windows.Forms.Label BFX_XLM_Label1;
        private System.Windows.Forms.Label IR_XLM_Label2;
        private System.Windows.Forms.Label IR_XLM_Label3;
        private System.Windows.Forms.Label IR_XLM_Label1;
        private System.Windows.Forms.Label GDAX_XLM_Label2;
        private System.Windows.Forms.Label GDAX_XLM_Label3;
        private System.Windows.Forms.Label GDAX_XLM_Label1;
        private System.Windows.Forms.ComboBox IR_CurrencyBox;
        private System.Windows.Forms.Label IR_GNT_Label2;
        private System.Windows.Forms.Label IR_GNT_Label1;
        private System.Windows.Forms.Label IR_GNT_Label3;
        private System.Windows.Forms.Label IR_REP_Label2;
        private System.Windows.Forms.Label IR_REP_Label1;
        private System.Windows.Forms.Label IR_REP_Label3;
        private System.Windows.Forms.Label IR_BAT_Label2;
        private System.Windows.Forms.Label IR_BAT_Label1;
        private System.Windows.Forms.Label IR_BAT_Label3;
        private System.Windows.Forms.Label BTCM_BAT_Label2;
        private System.Windows.Forms.Label BTCM_BAT_Label3;
        private System.Windows.Forms.Label BTCM_BAT_Label1;
        private System.Windows.Forms.Label BTCM_GNT_Label2;
        private System.Windows.Forms.Label BTCM_GNT_Label3;
        private System.Windows.Forms.Label BTCM_GNT_Label1;
        private System.Windows.Forms.Label GDAX_REP_Label2;
        private System.Windows.Forms.Label GDAX_REP_Label3;
        private System.Windows.Forms.Label GDAX_REP_Label1;
        private System.Windows.Forms.Label BFX_BAT_Label2;
        private System.Windows.Forms.Label BFX_BAT_Label3;
        private System.Windows.Forms.Label BFX_BAT_Label1;
        private System.Windows.Forms.Label BFX_GNT_Label2;
        private System.Windows.Forms.Label BFX_GNT_Label1;
        private System.Windows.Forms.Label BFX_REP_Label2;
        private System.Windows.Forms.Label BFX_REP_Label3;
        private System.Windows.Forms.Label BFX_GNT_Label3;
        private System.Windows.Forms.Label BFX_REP_Label1;
        private System.Windows.Forms.Label IR_ETC_Label2;
        private System.Windows.Forms.Label IR_ETC_Label3;
        private System.Windows.Forms.Label IR_ETC_Label1;
        private System.Windows.Forms.Label BTCM_ETC_Label2;
        private System.Windows.Forms.Label BTCM_ETC_Label3;
        private System.Windows.Forms.Label BTCM_ETC_Label1;
        private System.Windows.Forms.Label BFX_ETC_Label2;
        private System.Windows.Forms.Label BFX_ETC_Label3;
        private System.Windows.Forms.Label BFX_ETC_Label1;
        private System.Windows.Forms.Label GDAX_ETC_Label2;
        private System.Windows.Forms.Label GDAX_ETC_Label3;
        private System.Windows.Forms.Label GDAX_ETC_Label1;
        private System.ComponentModel.BackgroundWorker BlinkStickBW;
        private System.ComponentModel.BackgroundWorker BlinkStickWhite_Thread;
        private System.Windows.Forms.Label BTCM_BSV_Label2;
        private System.Windows.Forms.Label BTCM_BSV_Label3;
        private System.Windows.Forms.Label BTCM_BSV_Label1;
        private System.Windows.Forms.Label BFX_USDT_Label2;
        private System.Windows.Forms.Label BFX_USDT_Label3;
        private System.Windows.Forms.Label BFX_USDT_Label1;
        private System.Windows.Forms.Label BFX_BSV_Label2;
        private System.Windows.Forms.Label BFX_BSV_Label3;
        private System.Windows.Forms.Label BFX_BSV_Label1;
        private System.Windows.Forms.Label IR_USDT_Label2;
        private System.Windows.Forms.Label IR_USDT_Label3;
        private System.Windows.Forms.Label IR_USDT_Label1;
        private System.Windows.Forms.Label IR_BSV_Label2;
        private System.Windows.Forms.Label IR_BSV_Label3;
        private System.Windows.Forms.Label IR_BSV_Label1;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox BAR_GroupBox;
        private System.Windows.Forms.Label BAR_XBT_Label2;
        private System.Windows.Forms.Label BAR_XBT_Label3;
        private System.Windows.Forms.Label BAR_XBT_Label1;
        private System.Windows.Forms.CheckBox NegativeSpread_checkBox;
        private System.Windows.Forms.Label NegativeSpread_label;
        private System.Windows.Forms.Label SlackNameCurrency_label;
        private System.Windows.Forms.ComboBox SlackNameCurrency_comboBox;
        private System.Windows.Forms.ComboBox BAR_CurrencyBox;
        private System.Windows.Forms.Label BAR_AvgPrice_Label;
        private System.Windows.Forms.ComboBox BAR_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox BAR_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox BAR_BuySellComboBox;
        private System.Windows.Forms.FolderBrowserDialog spreadHistory_FolderDialog;
        private System.Windows.Forms.TextBox spreadHistoryCustomFolderValue_Textbox;
        private System.Windows.Forms.Label spreadHistoryCustomFolder_label;
        private System.Windows.Forms.ComboBox BFX_CurrencyBox;
        private System.Windows.Forms.ComboBox GDAX_CurrencyBox;
        private System.Windows.Forms.ComboBox BTCM_CurrencyBox;
        private System.Windows.Forms.Label SessionStartedRel_label;
        private System.Windows.Forms.Label SessionStartedAbs_label;
        private System.Windows.Forms.Label SessionStart_label;
        private System.Windows.Forms.Label SettingsSeparator_label;
    }
}

