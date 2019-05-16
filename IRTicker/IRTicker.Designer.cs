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
            this.toolbarFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.folderDialogButton = new System.Windows.Forms.Button();
            this.folderDialogTextbox = new System.Windows.Forms.TextBox();
            this.pollingThread = new System.ComponentModel.BackgroundWorker();
            this.Settings = new System.Windows.Forms.Panel();
            this.ExportSummarised_Checkbox = new System.Windows.Forms.CheckBox();
            this.ExportSummarised_Label = new System.Windows.Forms.Label();
            this.ExportFull_Checkbox = new System.Windows.Forms.CheckBox();
            this.ExportFull_Label = new System.Windows.Forms.Label();
            this.Help_Button = new System.Windows.Forms.Button();
            this.EnableGDAXLevel3_CheckBox = new System.Windows.Forms.CheckBox();
            this.EnableGDAXLevel3 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.SettingsOKButton = new System.Windows.Forms.Button();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.GIFLabel = new System.Windows.Forms.Label();
            this.Main = new System.Windows.Forms.Panel();
            this.BTCM_GroupBox = new System.Windows.Forms.GroupBox();
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
            this.CSPT_GroupBox = new System.Windows.Forms.GroupBox();
            this.CSPT_XRP_Label2 = new System.Windows.Forms.Label();
            this.CSPT_XRP_Label3 = new System.Windows.Forms.Label();
            this.CSPT_XRP_Label1 = new System.Windows.Forms.Label();
            this.CSPT_AvgPrice_Label = new System.Windows.Forms.Label();
            this.CSPT_CryptoComboBox = new System.Windows.Forms.ComboBox();
            this.CSPT_NumCoinsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.CSPT_BuySellComboBox = new System.Windows.Forms.ComboBox();
            this.CSPT_XBT_Label2 = new System.Windows.Forms.Label();
            this.CSPT_ETH_Label2 = new System.Windows.Forms.Label();
            this.CSPT_EOS_Label2 = new System.Windows.Forms.Label();
            this.CSPT_LTC_Label2 = new System.Windows.Forms.Label();
            this.CSPT_LTC_Label3 = new System.Windows.Forms.Label();
            this.CSPT_EOS_Label3 = new System.Windows.Forms.Label();
            this.CSPT_ETH_Label3 = new System.Windows.Forms.Label();
            this.CSPT_XBT_Label3 = new System.Windows.Forms.Label();
            this.CSPT_LTC_Label1 = new System.Windows.Forms.Label();
            this.CSPT_EOS_Label1 = new System.Windows.Forms.Label();
            this.CSPT_ETH_Label1 = new System.Windows.Forms.Label();
            this.CSPT_XBT_Label1 = new System.Windows.Forms.Label();
            this.BFX_GroupBox = new System.Windows.Forms.GroupBox();
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
            this.fiatRefresh_checkBox = new System.Windows.Forms.CheckBox();
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
            this.IR_CurrencyBox = new System.Windows.Forms.ComboBox();
            this.IR_XLM_Label2 = new System.Windows.Forms.Label();
            this.IR_XLM_Label3 = new System.Windows.Forms.Label();
            this.IR_XLM_Label1 = new System.Windows.Forms.Label();
            this.IR_Reset_Button = new System.Windows.Forms.Button();
            this.IR_EOS_Label2 = new System.Windows.Forms.Label();
            this.IR_EOS_Label3 = new System.Windows.Forms.Label();
            this.IR_EOS_Label1 = new System.Windows.Forms.Label();
            this.IR_PLA_Label2 = new System.Windows.Forms.Label();
            this.IR_PLA_Label1 = new System.Windows.Forms.Label();
            this.IR_PLA_Label3 = new System.Windows.Forms.Label();
            this.SpreadVolumeTitle_Label = new System.Windows.Forms.Label();
            this.IR_ZRX_Label2 = new System.Windows.Forms.Label();
            this.IR_ZRX_Label1 = new System.Windows.Forms.Label();
            this.IR_OMG_Label2 = new System.Windows.Forms.Label();
            this.IR_OMG_Label3 = new System.Windows.Forms.Label();
            this.IR_XRP_Label3 = new System.Windows.Forms.Label();
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
            this.GDAX_GroupBox = new System.Windows.Forms.GroupBox();
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
            this.IR_XBT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_ETH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_BCH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_LTC_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_XBT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_ETH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_BCH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_LTC_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_XRP_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_XBT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_ETH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_BCH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_LTC_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_XBT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_ETH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_BCH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_LTC_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.CSPT_XBT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.CSPT_ETH_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.CSPT_EOS_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.CSPT_LTC_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_AvgPriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_AvgPriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_AvgPriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_AvgPriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.CSPT_AvgPriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_XRP_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_XRP_PriceTT = new System.Windows.Forms.ToolTip(this.components);
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
            this.IR_OMG_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_ZRX_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_OMG_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_OMG_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_ZRX_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.CSPT_XRP_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_ZRX_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_PLA_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_XRP_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_EOS_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_EOS_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_XLM_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BTCM_XLM_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.GDAX_XLM_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.BFX_XLM_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_BAT_Label2 = new System.Windows.Forms.Label();
            this.IR_BAT_Label1 = new System.Windows.Forms.Label();
            this.IR_BAT_Label3 = new System.Windows.Forms.Label();
            this.IR_REP_Label2 = new System.Windows.Forms.Label();
            this.IR_REP_Label1 = new System.Windows.Forms.Label();
            this.IR_REP_Label3 = new System.Windows.Forms.Label();
            this.IR_GNT_Label2 = new System.Windows.Forms.Label();
            this.IR_GNT_Label1 = new System.Windows.Forms.Label();
            this.IR_GNT_Label3 = new System.Windows.Forms.Label();
            this.IR_BAT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_REP_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.IR_GNT_PriceTT = new System.Windows.Forms.ToolTip(this.components);
            this.Settings.SuspendLayout();
            this.LoadingPanel.SuspendLayout();
            this.Main.SuspendLayout();
            this.BTCM_GroupBox.SuspendLayout();
            this.CSPT_GroupBox.SuspendLayout();
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
            // folderDialogButton
            // 
            this.folderDialogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.folderDialogButton.Location = new System.Drawing.Point(364, 221);
            this.folderDialogButton.Name = "folderDialogButton";
            this.folderDialogButton.Size = new System.Drawing.Size(141, 31);
            this.folderDialogButton.TabIndex = 2;
            this.folderDialogButton.Text = "Choose folder...";
            this.folderDialogButton.UseVisualStyleBackColor = true;
            this.folderDialogButton.Click += new System.EventHandler(this.FolderDialogButton_Click);
            // 
            // folderDialogTextbox
            // 
            this.folderDialogTextbox.Location = new System.Drawing.Point(245, 191);
            this.folderDialogTextbox.Name = "folderDialogTextbox";
            this.folderDialogTextbox.ReadOnly = true;
            this.folderDialogTextbox.Size = new System.Drawing.Size(260, 20);
            this.folderDialogTextbox.TabIndex = 3;
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
            this.Settings.Controls.Add(this.ExportSummarised_Checkbox);
            this.Settings.Controls.Add(this.ExportSummarised_Label);
            this.Settings.Controls.Add(this.ExportFull_Checkbox);
            this.Settings.Controls.Add(this.ExportFull_Label);
            this.Settings.Controls.Add(this.Help_Button);
            this.Settings.Controls.Add(this.EnableGDAXLevel3_CheckBox);
            this.Settings.Controls.Add(this.EnableGDAXLevel3);
            this.Settings.Controls.Add(this.VersionLabel);
            this.Settings.Controls.Add(this.SettingsOKButton);
            this.Settings.Controls.Add(this.FolderLabel);
            this.Settings.Controls.Add(this.refreshFrequencyLabel);
            this.Settings.Controls.Add(this.folderDialogTextbox);
            this.Settings.Controls.Add(this.refreshFrequencyTextbox);
            this.Settings.Controls.Add(this.folderDialogButton);
            this.Settings.Location = new System.Drawing.Point(0, 0);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(585, 723);
            this.Settings.TabIndex = 4;
            // 
            // ExportSummarised_Checkbox
            // 
            this.ExportSummarised_Checkbox.AccessibleName = "";
            this.ExportSummarised_Checkbox.AutoSize = true;
            this.ExportSummarised_Checkbox.Location = new System.Drawing.Point(490, 387);
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
            this.ExportSummarised_Label.Location = new System.Drawing.Point(73, 387);
            this.ExportSummarised_Label.Name = "ExportSummarised_Label";
            this.ExportSummarised_Label.Size = new System.Drawing.Size(263, 26);
            this.ExportSummarised_Label.TabIndex = 17;
            this.ExportSummarised_Label.Text = "Export summary spread data?\r\nWill be saved to the G:\\IR\\IRTicker\\<your username>";
            // 
            // ExportFull_Checkbox
            // 
            this.ExportFull_Checkbox.AccessibleName = "";
            this.ExportFull_Checkbox.AutoSize = true;
            this.ExportFull_Checkbox.Location = new System.Drawing.Point(490, 341);
            this.ExportFull_Checkbox.Name = "ExportFull_Checkbox";
            this.ExportFull_Checkbox.Size = new System.Drawing.Size(15, 14);
            this.ExportFull_Checkbox.TabIndex = 16;
            this.ExportFull_Checkbox.UseVisualStyleBackColor = true;
            this.ExportFull_Checkbox.CheckedChanged += new System.EventHandler(this.ExportFull_Checkbox_CheckedChanged);
            // 
            // ExportFull_Label
            // 
            this.ExportFull_Label.AccessibleName = "";
            this.ExportFull_Label.AutoSize = true;
            this.ExportFull_Label.Location = new System.Drawing.Point(73, 341);
            this.ExportFull_Label.Name = "ExportFull_Label";
            this.ExportFull_Label.Size = new System.Drawing.Size(263, 26);
            this.ExportFull_Label.TabIndex = 15;
            this.ExportFull_Label.Text = "Export full spread history automatically?\r\nWill be saved to the local machine\'s D" +
    "ocuments library";
            // 
            // Help_Button
            // 
            this.Help_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Help_Button.Location = new System.Drawing.Point(502, 569);
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
            this.EnableGDAXLevel3_CheckBox.Location = new System.Drawing.Point(490, 287);
            this.EnableGDAXLevel3_CheckBox.Name = "EnableGDAXLevel3_CheckBox";
            this.EnableGDAXLevel3_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.EnableGDAXLevel3_CheckBox.TabIndex = 13;
            this.EnableGDAXLevel3_CheckBox.UseVisualStyleBackColor = true;
            // 
            // EnableGDAXLevel3
            // 
            this.EnableGDAXLevel3.AccessibleName = "";
            this.EnableGDAXLevel3.AutoSize = true;
            this.EnableGDAXLevel3.Location = new System.Drawing.Point(73, 287);
            this.EnableGDAXLevel3.Name = "EnableGDAXLevel3";
            this.EnableGDAXLevel3.Size = new System.Drawing.Size(241, 39);
            this.EnableGDAXLevel3.TabIndex = 12;
            this.EnableGDAXLevel3.Text = "Pull full Coinbase Pro order book?\r\n(Not recommended if you\'re doing lots of aver" +
    "age \r\ncoin price checks - you will be rate limited)";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(19, 571);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(0, 13);
            this.VersionLabel.TabIndex = 11;
            // 
            // SettingsOKButton
            // 
            this.SettingsOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsOKButton.Location = new System.Drawing.Point(502, 598);
            this.SettingsOKButton.Name = "SettingsOKButton";
            this.SettingsOKButton.Size = new System.Drawing.Size(75, 23);
            this.SettingsOKButton.TabIndex = 4;
            this.SettingsOKButton.Text = "Close";
            this.SettingsOKButton.UseVisualStyleBackColor = true;
            this.SettingsOKButton.Click += new System.EventHandler(this.SettingsOKButton_Click);
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(73, 191);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(115, 13);
            this.FolderLabel.TabIndex = 5;
            this.FolderLabel.Text = "Toolbar folder location:";
            // 
            // LoadingPanel
            // 
            this.LoadingPanel.Controls.Add(this.GIFLabel);
            this.LoadingPanel.Location = new System.Drawing.Point(0, 0);
            this.LoadingPanel.Name = "LoadingPanel";
            this.LoadingPanel.Size = new System.Drawing.Size(585, 631);
            this.LoadingPanel.TabIndex = 10;
            // 
            // GIFLabel
            // 
            this.GIFLabel.BackColor = System.Drawing.Color.Black;
            this.GIFLabel.ForeColor = System.Drawing.Color.Red;
            this.GIFLabel.Image = global::IRTicker.Properties.Resources.rainbow_space_bricks_jpg;
            this.GIFLabel.Location = new System.Drawing.Point(0, 0);
            this.GIFLabel.Name = "GIFLabel";
            this.GIFLabel.Size = new System.Drawing.Size(585, 634);
            this.GIFLabel.TabIndex = 0;
            this.GIFLabel.Text = "\r\n\r\n\r\n\r\n\r\n\r\nDownloading bitcoins...";
            this.GIFLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Main
            // 
            this.Main.BackColor = System.Drawing.Color.White;
            this.Main.Controls.Add(this.BTCM_GroupBox);
            this.Main.Controls.Add(this.CSPT_GroupBox);
            this.Main.Controls.Add(this.BFX_GroupBox);
            this.Main.Controls.Add(this.fiat_GroupBox);
            this.Main.Controls.Add(this.SettingsButton);
            this.Main.Controls.Add(this.IR_GroupBox);
            this.Main.Controls.Add(this.GDAX_GroupBox);
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.Size = new System.Drawing.Size(585, 783);
            this.Main.TabIndex = 5;
            // 
            // BTCM_GroupBox
            // 
            this.BTCM_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.btcm3;
            this.BTCM_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
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
            this.BTCM_GroupBox.Location = new System.Drawing.Point(305, 13);
            this.BTCM_GroupBox.Name = "BTCM_GroupBox";
            this.BTCM_GroupBox.Size = new System.Drawing.Size(262, 220);
            this.BTCM_GroupBox.TabIndex = 1;
            this.BTCM_GroupBox.TabStop = false;
            this.BTCM_GroupBox.Text = "BTC Markets";
            // 
            // BTCM_XLM_Label2
            // 
            this.BTCM_XLM_Label2.AutoSize = true;
            this.BTCM_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BTCM_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XLM_Label2.Location = new System.Drawing.Point(45, 123);
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
            this.BTCM_XLM_Label3.Location = new System.Drawing.Point(119, 123);
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
            this.BTCM_XLM_Label1.Location = new System.Drawing.Point(6, 123);
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
            this.BTCM_OMG_Label2.Location = new System.Drawing.Point(45, 143);
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
            this.BTCM_OMG_Label3.Location = new System.Drawing.Point(119, 143);
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
            this.BTCM_OMG_Label1.Location = new System.Drawing.Point(6, 143);
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
            this.BTCM_AvgPrice_Label.Location = new System.Drawing.Point(6, 163);
            this.BTCM_AvgPrice_Label.Name = "BTCM_AvgPrice_Label";
            this.BTCM_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.BTCM_AvgPrice_Label.TabIndex = 16;
            // 
            // BTCM_CryptoComboBox
            // 
            this.BTCM_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BTCM_CryptoComboBox.Location = new System.Drawing.Point(194, 191);
            this.BTCM_CryptoComboBox.Name = "BTCM_CryptoComboBox";
            this.BTCM_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.BTCM_CryptoComboBox.TabIndex = 17;
            this.BTCM_CryptoComboBox.DropDown += new System.EventHandler(this.BTCM_CryptoComboBox_DropDown);
            this.BTCM_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.BTCM_CryptoComboBox_SelectedIndexChanged);
            // 
            // BTCM_NumCoinsTextBox
            // 
            this.BTCM_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BTCM_NumCoinsTextBox.Location = new System.Drawing.Point(96, 191);
            this.BTCM_NumCoinsTextBox.Mask = "00000000";
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
            this.BTCM_BuySellComboBox.Location = new System.Drawing.Point(10, 191);
            this.BTCM_BuySellComboBox.Name = "BTCM_BuySellComboBox";
            this.BTCM_BuySellComboBox.Size = new System.Drawing.Size(58, 21);
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
            this.BTCM_LTC_Label2.Location = new System.Drawing.Point(45, 103);
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
            this.BTCM_LTC_Label3.Location = new System.Drawing.Point(119, 103);
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
            this.BTCM_LTC_Label1.Location = new System.Drawing.Point(6, 103);
            this.BTCM_LTC_Label1.Name = "BTCM_LTC_Label1";
            this.BTCM_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.BTCM_LTC_Label1.TabIndex = 11;
            this.BTCM_LTC_Label1.Text = "LTC:";
            // 
            // CSPT_GroupBox
            // 
            this.CSPT_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.coinspot_faded2;
            this.CSPT_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CSPT_GroupBox.Controls.Add(this.CSPT_XRP_Label2);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_XRP_Label3);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_XRP_Label1);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_AvgPrice_Label);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_CryptoComboBox);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_NumCoinsTextBox);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_BuySellComboBox);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_XBT_Label2);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_ETH_Label2);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_EOS_Label2);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_LTC_Label2);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_LTC_Label3);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_EOS_Label3);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_ETH_Label3);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_XBT_Label3);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_LTC_Label1);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_EOS_Label1);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_ETH_Label1);
            this.CSPT_GroupBox.Controls.Add(this.CSPT_XBT_Label1);
            this.CSPT_GroupBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.CSPT_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.CSPT_GroupBox.Location = new System.Drawing.Point(305, 517);
            this.CSPT_GroupBox.Name = "CSPT_GroupBox";
            this.CSPT_GroupBox.Size = new System.Drawing.Size(262, 182);
            this.CSPT_GroupBox.TabIndex = 16;
            this.CSPT_GroupBox.TabStop = false;
            this.CSPT_GroupBox.Text = "CoinSpot";
            // 
            // CSPT_XRP_Label2
            // 
            this.CSPT_XRP_Label2.AutoSize = true;
            this.CSPT_XRP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_XRP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_XRP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_XRP_Label2.Location = new System.Drawing.Point(53, 43);
            this.CSPT_XRP_Label2.Name = "CSPT_XRP_Label2";
            this.CSPT_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.CSPT_XRP_Label2.TabIndex = 17;
            this.CSPT_XRP_Label2.Tag = "CSPT";
            // 
            // CSPT_XRP_Label3
            // 
            this.CSPT_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CSPT_XRP_Label3.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_XRP_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_XRP_Label3.Location = new System.Drawing.Point(118, 43);
            this.CSPT_XRP_Label3.Name = "CSPT_XRP_Label3";
            this.CSPT_XRP_Label3.Size = new System.Drawing.Size(134, 13);
            this.CSPT_XRP_Label3.TabIndex = 18;
            this.CSPT_XRP_Label3.Tag = "";
            this.CSPT_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.CSPT_XRP_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CSPT_XRP_Label3_MouseDoubleClick);
            // 
            // CSPT_XRP_Label1
            // 
            this.CSPT_XRP_Label1.AutoSize = true;
            this.CSPT_XRP_Label1.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_XRP_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_XRP_Label1.Location = new System.Drawing.Point(6, 43);
            this.CSPT_XRP_Label1.Name = "CSPT_XRP_Label1";
            this.CSPT_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.CSPT_XRP_Label1.TabIndex = 16;
            this.CSPT_XRP_Label1.Text = "XRP:";
            // 
            // CSPT_AvgPrice_Label
            // 
            this.CSPT_AvgPrice_Label.BackColor = System.Drawing.Color.LightCyan;
            this.CSPT_AvgPrice_Label.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_AvgPrice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_AvgPrice_Label.Location = new System.Drawing.Point(6, 121);
            this.CSPT_AvgPrice_Label.Name = "CSPT_AvgPrice_Label";
            this.CSPT_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.CSPT_AvgPrice_Label.TabIndex = 15;
            // 
            // CSPT_CryptoComboBox
            // 
            this.CSPT_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CSPT_CryptoComboBox.Enabled = false;
            this.CSPT_CryptoComboBox.Location = new System.Drawing.Point(193, 150);
            this.CSPT_CryptoComboBox.Name = "CSPT_CryptoComboBox";
            this.CSPT_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.CSPT_CryptoComboBox.TabIndex = 14;
            // 
            // CSPT_NumCoinsTextBox
            // 
            this.CSPT_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_NumCoinsTextBox.Enabled = false;
            this.CSPT_NumCoinsTextBox.Location = new System.Drawing.Point(95, 151);
            this.CSPT_NumCoinsTextBox.Mask = "00000000";
            this.CSPT_NumCoinsTextBox.Name = "CSPT_NumCoinsTextBox";
            this.CSPT_NumCoinsTextBox.PromptChar = ' ';
            this.CSPT_NumCoinsTextBox.Size = new System.Drawing.Size(70, 20);
            this.CSPT_NumCoinsTextBox.TabIndex = 13;
            this.CSPT_NumCoinsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CSPT_NumCoinsTextBox.ValidatingType = typeof(int);
            // 
            // CSPT_BuySellComboBox
            // 
            this.CSPT_BuySellComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_BuySellComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CSPT_BuySellComboBox.Enabled = false;
            this.CSPT_BuySellComboBox.FormattingEnabled = true;
            this.CSPT_BuySellComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.CSPT_BuySellComboBox.Location = new System.Drawing.Point(9, 151);
            this.CSPT_BuySellComboBox.Name = "CSPT_BuySellComboBox";
            this.CSPT_BuySellComboBox.Size = new System.Drawing.Size(58, 21);
            this.CSPT_BuySellComboBox.TabIndex = 12;
            // 
            // CSPT_XBT_Label2
            // 
            this.CSPT_XBT_Label2.AutoSize = true;
            this.CSPT_XBT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_XBT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_XBT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_XBT_Label2.Location = new System.Drawing.Point(53, 23);
            this.CSPT_XBT_Label2.Name = "CSPT_XBT_Label2";
            this.CSPT_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.CSPT_XBT_Label2.TabIndex = 4;
            this.CSPT_XBT_Label2.Tag = "CSPT";
            // 
            // CSPT_ETH_Label2
            // 
            this.CSPT_ETH_Label2.AutoSize = true;
            this.CSPT_ETH_Label2.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_ETH_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_ETH_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_ETH_Label2.Location = new System.Drawing.Point(53, 63);
            this.CSPT_ETH_Label2.Name = "CSPT_ETH_Label2";
            this.CSPT_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.CSPT_ETH_Label2.TabIndex = 5;
            this.CSPT_ETH_Label2.Tag = "CSPT";
            // 
            // CSPT_EOS_Label2
            // 
            this.CSPT_EOS_Label2.AutoSize = true;
            this.CSPT_EOS_Label2.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_EOS_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_EOS_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_EOS_Label2.Location = new System.Drawing.Point(53, 83);
            this.CSPT_EOS_Label2.Name = "CSPT_EOS_Label2";
            this.CSPT_EOS_Label2.Size = new System.Drawing.Size(0, 13);
            this.CSPT_EOS_Label2.TabIndex = 6;
            this.CSPT_EOS_Label2.Tag = "CSPT";
            // 
            // CSPT_LTC_Label2
            // 
            this.CSPT_LTC_Label2.AutoSize = true;
            this.CSPT_LTC_Label2.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_LTC_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_LTC_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_LTC_Label2.Location = new System.Drawing.Point(53, 103);
            this.CSPT_LTC_Label2.Name = "CSPT_LTC_Label2";
            this.CSPT_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.CSPT_LTC_Label2.TabIndex = 7;
            this.CSPT_LTC_Label2.Tag = "CSPT";
            // 
            // CSPT_LTC_Label3
            // 
            this.CSPT_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CSPT_LTC_Label3.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_LTC_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_LTC_Label3.Location = new System.Drawing.Point(118, 103);
            this.CSPT_LTC_Label3.Name = "CSPT_LTC_Label3";
            this.CSPT_LTC_Label3.Size = new System.Drawing.Size(134, 13);
            this.CSPT_LTC_Label3.TabIndex = 11;
            this.CSPT_LTC_Label3.Tag = "";
            this.CSPT_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.CSPT_LTC_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CSPT_LTC_Label3_MouseDoubleClick);
            // 
            // CSPT_EOS_Label3
            // 
            this.CSPT_EOS_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CSPT_EOS_Label3.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_EOS_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_EOS_Label3.Location = new System.Drawing.Point(118, 83);
            this.CSPT_EOS_Label3.Name = "CSPT_EOS_Label3";
            this.CSPT_EOS_Label3.Size = new System.Drawing.Size(134, 13);
            this.CSPT_EOS_Label3.TabIndex = 10;
            this.CSPT_EOS_Label3.Tag = "";
            this.CSPT_EOS_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.CSPT_EOS_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CSPT_DOGE_Label3_MouseDoubleClick);
            // 
            // CSPT_ETH_Label3
            // 
            this.CSPT_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CSPT_ETH_Label3.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_ETH_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_ETH_Label3.Location = new System.Drawing.Point(118, 63);
            this.CSPT_ETH_Label3.Name = "CSPT_ETH_Label3";
            this.CSPT_ETH_Label3.Size = new System.Drawing.Size(134, 13);
            this.CSPT_ETH_Label3.TabIndex = 9;
            this.CSPT_ETH_Label3.Tag = "";
            this.CSPT_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.CSPT_ETH_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CSPT_ETH_Label3_MouseDoubleClick);
            // 
            // CSPT_XBT_Label3
            // 
            this.CSPT_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CSPT_XBT_Label3.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_XBT_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_XBT_Label3.Location = new System.Drawing.Point(118, 23);
            this.CSPT_XBT_Label3.Name = "CSPT_XBT_Label3";
            this.CSPT_XBT_Label3.Size = new System.Drawing.Size(134, 13);
            this.CSPT_XBT_Label3.TabIndex = 8;
            this.CSPT_XBT_Label3.Tag = "";
            this.CSPT_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.CSPT_XBT_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CSPT_XBT_Label3_MouseDoubleClick);
            // 
            // CSPT_LTC_Label1
            // 
            this.CSPT_LTC_Label1.AutoSize = true;
            this.CSPT_LTC_Label1.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_LTC_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_LTC_Label1.Location = new System.Drawing.Point(6, 103);
            this.CSPT_LTC_Label1.Name = "CSPT_LTC_Label1";
            this.CSPT_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.CSPT_LTC_Label1.TabIndex = 3;
            this.CSPT_LTC_Label1.Text = "LTC:";
            // 
            // CSPT_EOS_Label1
            // 
            this.CSPT_EOS_Label1.AutoSize = true;
            this.CSPT_EOS_Label1.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_EOS_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_EOS_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_EOS_Label1.Location = new System.Drawing.Point(6, 83);
            this.CSPT_EOS_Label1.Name = "CSPT_EOS_Label1";
            this.CSPT_EOS_Label1.Size = new System.Drawing.Size(36, 13);
            this.CSPT_EOS_Label1.TabIndex = 2;
            this.CSPT_EOS_Label1.Text = "EOS:";
            // 
            // CSPT_ETH_Label1
            // 
            this.CSPT_ETH_Label1.AutoSize = true;
            this.CSPT_ETH_Label1.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_ETH_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_ETH_Label1.Location = new System.Drawing.Point(6, 63);
            this.CSPT_ETH_Label1.Name = "CSPT_ETH_Label1";
            this.CSPT_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.CSPT_ETH_Label1.TabIndex = 1;
            this.CSPT_ETH_Label1.Text = "ETH:";
            // 
            // CSPT_XBT_Label1
            // 
            this.CSPT_XBT_Label1.AutoSize = true;
            this.CSPT_XBT_Label1.BackColor = System.Drawing.Color.Transparent;
            this.CSPT_XBT_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CSPT_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSPT_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.CSPT_XBT_Label1.Name = "CSPT_XBT_Label1";
            this.CSPT_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.CSPT_XBT_Label1.TabIndex = 0;
            this.CSPT_XBT_Label1.Text = "BTC:";
            // 
            // BFX_GroupBox
            // 
            this.BFX_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.bfx_faded;
            this.BFX_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
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
            this.BFX_GroupBox.Location = new System.Drawing.Point(305, 245);
            this.BFX_GroupBox.Name = "BFX_GroupBox";
            this.BFX_GroupBox.Size = new System.Drawing.Size(262, 260);
            this.BFX_GroupBox.TabIndex = 9;
            this.BFX_GroupBox.TabStop = false;
            this.BFX_GroupBox.Text = "BitFinex";
            this.BFX_GroupBox.Click += new System.EventHandler(this.BFX_GroupBox_Click);
            // 
            // BFX_XLM_Label2
            // 
            this.BFX_XLM_Label2.AutoSize = true;
            this.BFX_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.BFX_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XLM_Label2.Location = new System.Drawing.Point(45, 143);
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
            this.BFX_XLM_Label3.Location = new System.Drawing.Point(119, 143);
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
            this.BFX_XLM_Label1.Location = new System.Drawing.Point(6, 143);
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
            this.BFX_EOS_Label2.Location = new System.Drawing.Point(45, 83);
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
            this.BFX_EOS_Label3.Location = new System.Drawing.Point(119, 83);
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
            this.BFX_EOS_Label1.Location = new System.Drawing.Point(6, 83);
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
            this.BFX_ZRX_Label2.Location = new System.Drawing.Point(45, 183);
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
            this.BFX_ZRX_Label1.Location = new System.Drawing.Point(6, 183);
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
            this.BFX_OMG_Label2.Location = new System.Drawing.Point(45, 163);
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
            this.BFX_OMG_Label3.Location = new System.Drawing.Point(119, 163);
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
            this.BFX_ZRX_Label3.Location = new System.Drawing.Point(119, 182);
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
            this.BFX_OMG_Label1.Location = new System.Drawing.Point(6, 163);
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
            this.BFX_CryptoComboBox.Location = new System.Drawing.Point(194, 230);
            this.BFX_CryptoComboBox.Name = "BFX_CryptoComboBox";
            this.BFX_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.BFX_CryptoComboBox.TabIndex = 20;
            this.BFX_CryptoComboBox.DropDown += new System.EventHandler(this.BFX_CryptoComboBox_DropDown);
            this.BFX_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.BFX_CryptoComboBox_SelectedIndexChanged);
            // 
            // BFX_NumCoinsTextBox
            // 
            this.BFX_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BFX_NumCoinsTextBox.Location = new System.Drawing.Point(96, 230);
            this.BFX_NumCoinsTextBox.Mask = "00000000";
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
            this.BFX_BuySellComboBox.Location = new System.Drawing.Point(10, 230);
            this.BFX_BuySellComboBox.Name = "BFX_BuySellComboBox";
            this.BFX_BuySellComboBox.Size = new System.Drawing.Size(58, 21);
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
            this.BFX_BCH_Label2.Location = new System.Drawing.Point(45, 103);
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
            this.BFX_LTC_Label2.Location = new System.Drawing.Point(45, 123);
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
            this.BFX_BCH_Label3.Location = new System.Drawing.Point(119, 103);
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
            this.BFX_LTC_Label3.Location = new System.Drawing.Point(119, 123);
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
            this.BFX_LTC_Label1.Location = new System.Drawing.Point(6, 123);
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
            this.BFX_BCH_Label1.Location = new System.Drawing.Point(6, 103);
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
            this.BFX_AvgPrice_Label.Location = new System.Drawing.Point(6, 201);
            this.BFX_AvgPrice_Label.Name = "BFX_AvgPrice_Label";
            this.BFX_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.BFX_AvgPrice_Label.TabIndex = 19;
            // 
            // fiat_GroupBox
            // 
            this.fiat_GroupBox.Controls.Add(this.fiatRefresh_checkBox);
            this.fiat_GroupBox.Controls.Add(this.USD_Label2);
            this.fiat_GroupBox.Controls.Add(this.USD_Label1);
            this.fiat_GroupBox.Controls.Add(this.AUD_Label2);
            this.fiat_GroupBox.Controls.Add(this.NZD_Label2);
            this.fiat_GroupBox.Controls.Add(this.EUR_Label2);
            this.fiat_GroupBox.Controls.Add(this.EUR_Label1);
            this.fiat_GroupBox.Controls.Add(this.NZD_Label1);
            this.fiat_GroupBox.Controls.Add(this.AUD_Label1);
            this.fiat_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.fiat_GroupBox.Location = new System.Drawing.Point(19, 597);
            this.fiat_GroupBox.Name = "fiat_GroupBox";
            this.fiat_GroupBox.Size = new System.Drawing.Size(263, 131);
            this.fiat_GroupBox.TabIndex = 9;
            this.fiat_GroupBox.TabStop = false;
            this.fiat_GroupBox.Text = "Fiat rates";
            this.fiat_GroupBox.Click += new System.EventHandler(this.Fiat_GroupBox_Click);
            // 
            // fiatRefresh_checkBox
            // 
            this.fiatRefresh_checkBox.AutoSize = true;
            this.fiatRefresh_checkBox.Location = new System.Drawing.Point(10, 107);
            this.fiatRefresh_checkBox.Name = "fiatRefresh_checkBox";
            this.fiatRefresh_checkBox.Size = new System.Drawing.Size(143, 17);
            this.fiatRefresh_checkBox.TabIndex = 9;
            this.fiatRefresh_checkBox.Text = "Tick to queue an update";
            this.fiatRefresh_checkBox.UseVisualStyleBackColor = true;
            this.fiatRefresh_checkBox.CheckedChanged += new System.EventHandler(this.Fiat_checkBox_CheckedChanged);
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
            this.SettingsButton.Location = new System.Drawing.Point(207, 746);
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
            this.IR_GroupBox.Controls.Add(this.IR_PLA_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_PLA_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_PLA_Label3);
            this.IR_GroupBox.Controls.Add(this.SpreadVolumeTitle_Label);
            this.IR_GroupBox.Controls.Add(this.IR_ZRX_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_ZRX_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_OMG_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_OMG_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_XRP_Label3);
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
            this.IR_GroupBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IR_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.IR_GroupBox.Location = new System.Drawing.Point(19, 13);
            this.IR_GroupBox.Name = "IR_GroupBox";
            this.IR_GroupBox.Size = new System.Drawing.Size(263, 343);
            this.IR_GroupBox.TabIndex = 0;
            this.IR_GroupBox.TabStop = false;
            this.IR_GroupBox.Text = "Independent Reserve";
            this.IR_GroupBox.Click += new System.EventHandler(this.IR_GroupBox_Click);
            // 
            // IR_CurrencyBox
            // 
            this.IR_CurrencyBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_CurrencyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_CurrencyBox.FormattingEnabled = true;
            this.IR_CurrencyBox.Items.AddRange(new object[] {
            "crypto",
            "fiat"});
            this.IR_CurrencyBox.Location = new System.Drawing.Point(131, 314);
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
            this.IR_XLM_Label2.Location = new System.Drawing.Point(45, 149);
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
            this.IR_XLM_Label3.Location = new System.Drawing.Point(119, 149);
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
            this.IR_XLM_Label1.Location = new System.Drawing.Point(6, 149);
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
            this.IR_Reset_Button.Location = new System.Drawing.Point(207, 286);
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
            this.IR_EOS_Label2.Location = new System.Drawing.Point(45, 89);
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
            this.IR_EOS_Label3.Location = new System.Drawing.Point(119, 89);
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
            this.IR_EOS_Label1.Location = new System.Drawing.Point(6, 89);
            this.IR_EOS_Label1.Name = "IR_EOS_Label1";
            this.IR_EOS_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_EOS_Label1.TabIndex = 29;
            this.IR_EOS_Label1.Text = "EOS:";
            this.IR_EOS_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_PLA_Label2
            // 
            this.IR_PLA_Label2.AutoSize = true;
            this.IR_PLA_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_PLA_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_PLA_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_PLA_Label2.Location = new System.Drawing.Point(45, 209);
            this.IR_PLA_Label2.Name = "IR_PLA_Label2";
            this.IR_PLA_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_PLA_Label2.TabIndex = 28;
            this.IR_PLA_Label2.Tag = "IR";
            this.IR_PLA_Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_PLA_Label1
            // 
            this.IR_PLA_Label1.AutoSize = true;
            this.IR_PLA_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_PLA_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_PLA_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_PLA_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_PLA_Label1.Location = new System.Drawing.Point(6, 209);
            this.IR_PLA_Label1.Name = "IR_PLA_Label1";
            this.IR_PLA_Label1.Size = new System.Drawing.Size(34, 13);
            this.IR_PLA_Label1.TabIndex = 27;
            this.IR_PLA_Label1.Text = "PLA:";
            this.IR_PLA_Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IR_PLA_Label3
            // 
            this.IR_PLA_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_PLA_Label3.BackColor = System.Drawing.Color.Transparent;
            this.IR_PLA_Label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_PLA_Label3.Location = new System.Drawing.Point(119, 209);
            this.IR_PLA_Label3.Name = "IR_PLA_Label3";
            this.IR_PLA_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_PLA_Label3.TabIndex = 26;
            this.IR_PLA_Label3.Tag = "";
            this.IR_PLA_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.IR_ZRX_Label2.Location = new System.Drawing.Point(45, 189);
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
            this.IR_ZRX_Label1.Location = new System.Drawing.Point(6, 189);
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
            this.IR_OMG_Label2.Location = new System.Drawing.Point(45, 169);
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
            this.IR_OMG_Label3.Location = new System.Drawing.Point(119, 169);
            this.IR_OMG_Label3.Name = "IR_OMG_Label3";
            this.IR_OMG_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_OMG_Label3.TabIndex = 22;
            this.IR_OMG_Label3.Tag = "";
            this.IR_OMG_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IR_OMG_Label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IR_OMG_Label3_MouseDoubleClick);
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
            // IR_OMG_Label1
            // 
            this.IR_OMG_Label1.AutoSize = true;
            this.IR_OMG_Label1.BackColor = System.Drawing.Color.Transparent;
            this.IR_OMG_Label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_OMG_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_OMG_Label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.IR_OMG_Label1.Location = new System.Drawing.Point(6, 169);
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
            this.IR_XRP_Label2.Location = new System.Drawing.Point(45, 49);
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
            this.IR_ZRX_Label3.Location = new System.Drawing.Point(119, 189);
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
            this.IR_AvgPrice_Label.Location = new System.Drawing.Point(6, 287);
            this.IR_AvgPrice_Label.Name = "IR_AvgPrice_Label";
            this.IR_AvgPrice_Label.Size = new System.Drawing.Size(194, 16);
            this.IR_AvgPrice_Label.TabIndex = 15;
            // 
            // IR_CryptoComboBox
            // 
            this.IR_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IR_CryptoComboBox.Location = new System.Drawing.Point(193, 314);
            this.IR_CryptoComboBox.Name = "IR_CryptoComboBox";
            this.IR_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.IR_CryptoComboBox.TabIndex = 14;
            this.IR_CryptoComboBox.DropDown += new System.EventHandler(this.IR_CryptoComboBox_DropDown);
            this.IR_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.IR_CryptoComboBox_SelectedIndexChanged);
            // 
            // IR_NumCoinsTextBox
            // 
            this.IR_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_NumCoinsTextBox.Location = new System.Drawing.Point(58, 314);
            this.IR_NumCoinsTextBox.Mask = "00000000";
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
            this.IR_BuySellComboBox.Location = new System.Drawing.Point(9, 314);
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
            this.IR_XBT_Label2.Location = new System.Drawing.Point(45, 29);
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
            this.IR_ETH_Label2.Location = new System.Drawing.Point(45, 69);
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
            this.IR_BCH_Label2.Location = new System.Drawing.Point(45, 109);
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
            this.IR_LTC_Label2.Location = new System.Drawing.Point(45, 129);
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
            this.IR_LTC_Label3.Location = new System.Drawing.Point(119, 129);
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
            this.IR_BCH_Label3.Location = new System.Drawing.Point(119, 109);
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
            this.IR_LTC_Label1.Location = new System.Drawing.Point(6, 129);
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
            this.IR_BCH_Label1.Location = new System.Drawing.Point(6, 109);
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
            // GDAX_GroupBox
            // 
            this.GDAX_GroupBox.BackgroundImage = global::IRTicker.Properties.Resources.coinbasepro_logo3;
            this.GDAX_GroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
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
            this.GDAX_GroupBox.Location = new System.Drawing.Point(19, 362);
            this.GDAX_GroupBox.Name = "GDAX_GroupBox";
            this.GDAX_GroupBox.Size = new System.Drawing.Size(263, 222);
            this.GDAX_GroupBox.TabIndex = 8;
            this.GDAX_GroupBox.TabStop = false;
            this.GDAX_GroupBox.Text = "Coinbase Pro";
            this.GDAX_GroupBox.Click += new System.EventHandler(this.GDAX_GroupBox_Click);
            // 
            // GDAX_XLM_Label2
            // 
            this.GDAX_XLM_Label2.AutoSize = true;
            this.GDAX_XLM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.GDAX_XLM_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_XLM_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XLM_Label2.Location = new System.Drawing.Point(45, 123);
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
            this.GDAX_XLM_Label3.Location = new System.Drawing.Point(119, 123);
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
            this.GDAX_XLM_Label1.Location = new System.Drawing.Point(6, 123);
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
            this.GDAX_ZRX_Label2.Location = new System.Drawing.Point(45, 143);
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
            this.GDAX_ZRX_Label3.Location = new System.Drawing.Point(119, 143);
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
            this.GDAX_ZRX_Label1.Location = new System.Drawing.Point(6, 143);
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
            this.GDAX_AvgPrice_Label.Location = new System.Drawing.Point(6, 161);
            this.GDAX_AvgPrice_Label.Name = "GDAX_AvgPrice_Label";
            this.GDAX_AvgPrice_Label.Size = new System.Drawing.Size(251, 16);
            this.GDAX_AvgPrice_Label.TabIndex = 18;
            // 
            // GDAX_CryptoComboBox
            // 
            this.GDAX_CryptoComboBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_CryptoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GDAX_CryptoComboBox.Location = new System.Drawing.Point(193, 191);
            this.GDAX_CryptoComboBox.Name = "GDAX_CryptoComboBox";
            this.GDAX_CryptoComboBox.Size = new System.Drawing.Size(58, 21);
            this.GDAX_CryptoComboBox.TabIndex = 17;
            this.GDAX_CryptoComboBox.DropDown += new System.EventHandler(this.GDAX_CryptoComboBox_DropDown);
            this.GDAX_CryptoComboBox.SelectedIndexChanged += new System.EventHandler(this.GDAX_CryptoComboBox_SelectedIndexChanged);
            // 
            // GDAX_NumCoinsTextBox
            // 
            this.GDAX_NumCoinsTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GDAX_NumCoinsTextBox.Location = new System.Drawing.Point(95, 191);
            this.GDAX_NumCoinsTextBox.Mask = "00000000";
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
            this.GDAX_BuySellComboBox.Location = new System.Drawing.Point(9, 191);
            this.GDAX_BuySellComboBox.Name = "GDAX_BuySellComboBox";
            this.GDAX_BuySellComboBox.Size = new System.Drawing.Size(58, 21);
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
            // IR_XBT_PriceTT
            // 
            this.IR_XBT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_XBT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_ETH_PriceTT
            // 
            this.IR_ETH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_ETH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_BCH_PriceTT
            // 
            this.IR_BCH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_BCH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_LTC_PriceTT
            // 
            this.IR_LTC_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_LTC_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_XBT_PriceTT
            // 
            this.BTCM_XBT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_XBT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_ETH_PriceTT
            // 
            this.BTCM_ETH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_ETH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_BCH_PriceTT
            // 
            this.BTCM_BCH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_BCH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_LTC_PriceTT
            // 
            this.BTCM_LTC_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_LTC_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_XRP_PriceTT
            // 
            this.BTCM_XRP_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_XRP_PriceTT.ToolTipTitle = "Spread details";
            // 
            // GDAX_XBT_PriceTT
            // 
            this.GDAX_XBT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_XBT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // GDAX_ETH_PriceTT
            // 
            this.GDAX_ETH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_ETH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // GDAX_BCH_PriceTT
            // 
            this.GDAX_BCH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_BCH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // GDAX_LTC_PriceTT
            // 
            this.GDAX_LTC_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_LTC_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_XBT_PriceTT
            // 
            this.BFX_XBT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_XBT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_ETH_PriceTT
            // 
            this.BFX_ETH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_ETH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_BCH_PriceTT
            // 
            this.BFX_BCH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_BCH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_LTC_PriceTT
            // 
            this.BFX_LTC_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_LTC_PriceTT.ToolTipTitle = "Spread details";
            // 
            // CSPT_XBT_PriceTT
            // 
            this.CSPT_XBT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CSPT_XBT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // CSPT_ETH_PriceTT
            // 
            this.CSPT_ETH_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CSPT_ETH_PriceTT.ToolTipTitle = "Spread details";
            // 
            // CSPT_EOS_PriceTT
            // 
            this.CSPT_EOS_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CSPT_EOS_PriceTT.ToolTipTitle = "Spread details";
            // 
            // CSPT_LTC_PriceTT
            // 
            this.CSPT_LTC_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CSPT_LTC_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_AvgPriceTT
            // 
            this.IR_AvgPriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_AvgPriceTT.ToolTipTitle = "Average price details";
            // 
            // BTCM_AvgPriceTT
            // 
            this.BTCM_AvgPriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_AvgPriceTT.ToolTipTitle = "Average price details";
            // 
            // GDAX_AvgPriceTT
            // 
            this.GDAX_AvgPriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_AvgPriceTT.ToolTipTitle = "Average price details";
            // 
            // BFX_AvgPriceTT
            // 
            this.BFX_AvgPriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_AvgPriceTT.ToolTipTitle = "Average price details";
            // 
            // CSPT_AvgPriceTT
            // 
            this.CSPT_AvgPriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CSPT_AvgPriceTT.ToolTipTitle = "Average price details";
            // 
            // BFX_XRP_PriceTT
            // 
            this.BFX_XRP_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_XRP_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_XRP_PriceTT
            // 
            this.IR_XRP_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_XRP_PriceTT.ToolTipTitle = "Spread details";
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
            // IR_OMG_PriceTT
            // 
            this.IR_OMG_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_OMG_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_ZRX_PriceTT
            // 
            this.IR_ZRX_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_ZRX_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_OMG_PriceTT
            // 
            this.BTCM_OMG_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_OMG_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_OMG_PriceTT
            // 
            this.BFX_OMG_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_OMG_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_ZRX_PriceTT
            // 
            this.BFX_ZRX_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_ZRX_PriceTT.ToolTipTitle = "Spread details";
            // 
            // CSPT_XRP_PriceTT
            // 
            this.CSPT_XRP_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CSPT_XRP_PriceTT.ToolTipTitle = "Spread details";
            // 
            // GDAX_ZRX_PriceTT
            // 
            this.GDAX_ZRX_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_ZRX_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_PLA_PriceTT
            // 
            this.IR_PLA_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_PLA_PriceTT.ToolTipTitle = "Average price details";
            // 
            // GDAX_XRP_PriceTT
            // 
            this.GDAX_XRP_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_XRP_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_EOS_PriceTT
            // 
            this.IR_EOS_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_EOS_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_EOS_PriceTT
            // 
            this.BFX_EOS_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_EOS_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_XLM_PriceTT
            // 
            this.IR_XLM_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_XLM_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BTCM_XLM_PriceTT
            // 
            this.BTCM_XLM_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BTCM_XLM_PriceTT.ToolTipTitle = "Spread details";
            // 
            // GDAX_XLM_PriceTT
            // 
            this.GDAX_XLM_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GDAX_XLM_PriceTT.ToolTipTitle = "Spread details";
            // 
            // BFX_XLM_PriceTT
            // 
            this.BFX_XLM_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BFX_XLM_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_BAT_Label2
            // 
            this.IR_BAT_Label2.AutoSize = true;
            this.IR_BAT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_BAT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_BAT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BAT_Label2.Location = new System.Drawing.Point(45, 229);
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
            this.IR_BAT_Label1.Location = new System.Drawing.Point(6, 229);
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
            this.IR_BAT_Label3.Location = new System.Drawing.Point(119, 229);
            this.IR_BAT_Label3.Name = "IR_BAT_Label3";
            this.IR_BAT_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_BAT_Label3.TabIndex = 37;
            this.IR_BAT_Label3.Tag = "";
            this.IR_BAT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_REP_Label2
            // 
            this.IR_REP_Label2.AutoSize = true;
            this.IR_REP_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_REP_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_REP_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_REP_Label2.Location = new System.Drawing.Point(45, 249);
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
            this.IR_REP_Label1.Location = new System.Drawing.Point(6, 249);
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
            this.IR_REP_Label3.Location = new System.Drawing.Point(119, 249);
            this.IR_REP_Label3.Name = "IR_REP_Label3";
            this.IR_REP_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_REP_Label3.TabIndex = 40;
            this.IR_REP_Label3.Tag = "";
            this.IR_REP_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_GNT_Label2
            // 
            this.IR_GNT_Label2.AutoSize = true;
            this.IR_GNT_Label2.BackColor = System.Drawing.Color.Transparent;
            this.IR_GNT_Label2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IR_GNT_Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_GNT_Label2.Location = new System.Drawing.Point(45, 269);
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
            this.IR_GNT_Label1.Location = new System.Drawing.Point(6, 269);
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
            this.IR_GNT_Label3.Location = new System.Drawing.Point(119, 269);
            this.IR_GNT_Label3.Name = "IR_GNT_Label3";
            this.IR_GNT_Label3.Size = new System.Drawing.Size(134, 13);
            this.IR_GNT_Label3.TabIndex = 43;
            this.IR_GNT_Label3.Tag = "";
            this.IR_GNT_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IR_BAT_PriceTT
            // 
            this.IR_BAT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_BAT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_REP_PriceTT
            // 
            this.IR_REP_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_REP_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IR_GNT_PriceTT
            // 
            this.IR_GNT_PriceTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IR_GNT_PriceTT.ToolTipTitle = "Spread details";
            // 
            // IRTicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 781);
            this.Controls.Add(this.Main);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.LoadingPanel);
            this.Controls.Add(this.OTCHelper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 820);
            this.MinimumSize = new System.Drawing.Size(600, 820);
            this.Name = "IRTicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IR Ticker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IRTicker_Closing);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.LoadingPanel.ResumeLayout(false);
            this.Main.ResumeLayout(false);
            this.BTCM_GroupBox.ResumeLayout(false);
            this.BTCM_GroupBox.PerformLayout();
            this.CSPT_GroupBox.ResumeLayout(false);
            this.CSPT_GroupBox.PerformLayout();
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
        private System.Windows.Forms.FolderBrowserDialog toolbarFolder;
        private System.Windows.Forms.Button folderDialogButton;
        private System.Windows.Forms.TextBox folderDialogTextbox;
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
        private System.Windows.Forms.Label FolderLabel;
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
        private System.Windows.Forms.GroupBox CSPT_GroupBox;
        private System.Windows.Forms.Label CSPT_AvgPrice_Label;
        private System.Windows.Forms.ComboBox CSPT_CryptoComboBox;
        private System.Windows.Forms.MaskedTextBox CSPT_NumCoinsTextBox;
        private System.Windows.Forms.ComboBox CSPT_BuySellComboBox;
        private System.Windows.Forms.Label CSPT_XBT_Label2;
        private System.Windows.Forms.Label CSPT_ETH_Label2;
        private System.Windows.Forms.Label CSPT_EOS_Label2;
        private System.Windows.Forms.Label CSPT_LTC_Label2;
        private System.Windows.Forms.Label CSPT_LTC_Label3;
        private System.Windows.Forms.Label CSPT_EOS_Label3;
        private System.Windows.Forms.Label CSPT_ETH_Label3;
        private System.Windows.Forms.Label CSPT_XBT_Label3;
        private System.Windows.Forms.Label CSPT_LTC_Label1;
        private System.Windows.Forms.Label CSPT_EOS_Label1;
        private System.Windows.Forms.Label CSPT_ETH_Label1;
        private System.Windows.Forms.Label CSPT_XBT_Label1;
        private System.Windows.Forms.ToolTip IR_XBT_PriceTT;
        private System.Windows.Forms.ToolTip IR_ETH_PriceTT;
        private System.Windows.Forms.ToolTip IR_BCH_PriceTT;
        private System.Windows.Forms.ToolTip IR_LTC_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_XBT_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_ETH_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_BCH_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_LTC_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_XRP_PriceTT;
        private System.Windows.Forms.ToolTip GDAX_XBT_PriceTT;
        private System.Windows.Forms.ToolTip GDAX_ETH_PriceTT;
        private System.Windows.Forms.ToolTip GDAX_BCH_PriceTT;
        private System.Windows.Forms.ToolTip GDAX_LTC_PriceTT;
        private System.Windows.Forms.ToolTip BFX_XBT_PriceTT;
        private System.Windows.Forms.ToolTip BFX_ETH_PriceTT;
        private System.Windows.Forms.ToolTip BFX_BCH_PriceTT;
        private System.Windows.Forms.ToolTip BFX_LTC_PriceTT;
        private System.Windows.Forms.ToolTip CSPT_XBT_PriceTT;
        private System.Windows.Forms.ToolTip CSPT_ETH_PriceTT;
        private System.Windows.Forms.ToolTip CSPT_EOS_PriceTT;
        private System.Windows.Forms.ToolTip CSPT_LTC_PriceTT;
        private System.Windows.Forms.ToolTip IR_AvgPriceTT;
        private System.Windows.Forms.ToolTip BTCM_AvgPriceTT;
        private System.Windows.Forms.ToolTip GDAX_AvgPriceTT;
        private System.Windows.Forms.ToolTip BFX_AvgPriceTT;
        private System.Windows.Forms.ToolTip CSPT_AvgPriceTT;
        private System.Windows.Forms.Label BFX_XRP_Label2;
        private System.Windows.Forms.Label BFX_XRP_Label3;
        private System.Windows.Forms.Label BFX_XRP_Label1;
        private System.Windows.Forms.ToolTip BFX_XRP_PriceTT;
        public System.Windows.Forms.Button Help_Button;
        private System.Windows.Forms.CheckBox ExportSummarised_Checkbox;
        private System.Windows.Forms.Label ExportSummarised_Label;
        private System.Windows.Forms.CheckBox ExportFull_Checkbox;
        private System.Windows.Forms.Label ExportFull_Label;
        private System.Windows.Forms.Label IR_XRP_Label2;
        private System.Windows.Forms.Label IR_ZRX_Label3;
        private System.Windows.Forms.Label IR_XRP_Label1;
        private System.Windows.Forms.ToolTip IR_XRP_PriceTT;
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
        private System.Windows.Forms.Label CSPT_XRP_Label2;
        private System.Windows.Forms.Label CSPT_XRP_Label3;
        private System.Windows.Forms.Label CSPT_XRP_Label1;
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
        private System.Windows.Forms.ToolTip IR_OMG_PriceTT;
        private System.Windows.Forms.ToolTip IR_ZRX_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_OMG_PriceTT;
        private System.Windows.Forms.ToolTip BFX_OMG_PriceTT;
        private System.Windows.Forms.ToolTip BFX_ZRX_PriceTT;
        private System.Windows.Forms.ToolTip CSPT_XRP_PriceTT;
        private System.Windows.Forms.Label SpreadVolumeTitle_Label;
        private System.Windows.Forms.Label GDAX_ZRX_Label2;
        private System.Windows.Forms.Label GDAX_ZRX_Label3;
        private System.Windows.Forms.Label GDAX_ZRX_Label1;
        private System.Windows.Forms.ToolTip GDAX_ZRX_PriceTT;
        private System.Windows.Forms.Label IR_PLA_Label2;
        private System.Windows.Forms.Label IR_PLA_Label1;
        private System.Windows.Forms.Label IR_PLA_Label3;
        private System.Windows.Forms.ToolTip IR_PLA_PriceTT;
        private System.Windows.Forms.Label GDAX_XRP_Label2;
        private System.Windows.Forms.Label GDAX_XRP_Label3;
        private System.Windows.Forms.Label GDAX_XRP_Label1;
        private System.Windows.Forms.ToolTip GDAX_XRP_PriceTT;
        private System.Windows.Forms.Label BFX_EOS_Label2;
        private System.Windows.Forms.Label BFX_EOS_Label3;
        private System.Windows.Forms.Label BFX_EOS_Label1;
        private System.Windows.Forms.Label IR_EOS_Label2;
        private System.Windows.Forms.Label IR_EOS_Label3;
        private System.Windows.Forms.Label IR_EOS_Label1;
        private System.Windows.Forms.ToolTip IR_EOS_PriceTT;
        private System.Windows.Forms.ToolTip BFX_EOS_PriceTT;
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
        private System.Windows.Forms.ToolTip IR_XLM_PriceTT;
        private System.Windows.Forms.ToolTip BTCM_XLM_PriceTT;
        private System.Windows.Forms.ToolTip GDAX_XLM_PriceTT;
        private System.Windows.Forms.ToolTip BFX_XLM_PriceTT;
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
        private System.Windows.Forms.ToolTip IR_BAT_PriceTT;
        private System.Windows.Forms.ToolTip IR_REP_PriceTT;
        private System.Windows.Forms.ToolTip IR_GNT_PriceTT;
    }
}

