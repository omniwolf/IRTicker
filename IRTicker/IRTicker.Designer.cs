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
            this.refreshFrequencyTextbox = new System.Windows.Forms.MaskedTextBox();
            this.refreshFrequencyLabel = new System.Windows.Forms.Label();
            this.toolbarFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.folderDialogButton = new System.Windows.Forms.Button();
            this.folderDialogTextbox = new System.Windows.Forms.TextBox();
            this.pollingThread = new System.ComponentModel.BackgroundWorker();
            this.Settings = new System.Windows.Forms.Panel();
            this.fiatInvert_checkBox = new System.Windows.Forms.CheckBox();
            this.InvertFiatLabel = new System.Windows.Forms.Label();
            this.SettingsOKButton = new System.Windows.Forms.Button();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.GIFLabel = new System.Windows.Forms.Label();
            this.Main = new System.Windows.Forms.Panel();
            this.BFX_GroupBox = new System.Windows.Forms.GroupBox();
            this.BFX_XBT_Label2 = new System.Windows.Forms.Label();
            this.BFX_ETH_Label2 = new System.Windows.Forms.Label();
            this.BFX_BCH_Label2 = new System.Windows.Forms.Label();
            this.BFX_LTC_Label2 = new System.Windows.Forms.Label();
            this.BFX_LTC_Label1 = new System.Windows.Forms.Label();
            this.BFX_BCH_Label1 = new System.Windows.Forms.Label();
            this.BFX_ETH_Label1 = new System.Windows.Forms.Label();
            this.BFX_XBT_Label1 = new System.Windows.Forms.Label();
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
            this.GDAX_GroupBox = new System.Windows.Forms.GroupBox();
            this.GDAX_XBT_Label2 = new System.Windows.Forms.Label();
            this.GDAX_ETH_Label2 = new System.Windows.Forms.Label();
            this.GDAX_BCH_Label2 = new System.Windows.Forms.Label();
            this.GDAX_LTC_Label2 = new System.Windows.Forms.Label();
            this.GDAX_LTC_Label1 = new System.Windows.Forms.Label();
            this.GDAX_BCH_Label1 = new System.Windows.Forms.Label();
            this.GDAX_ETH_Label1 = new System.Windows.Forms.Label();
            this.GDAX_XBT_Label1 = new System.Windows.Forms.Label();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.BTCM_GroupBox = new System.Windows.Forms.GroupBox();
            this.BTCM_XRP_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label1 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label1 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label2 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label1 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label2 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label1 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label2 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label1 = new System.Windows.Forms.Label();
            this.IR_GroupBox = new System.Windows.Forms.GroupBox();
            this.IR_XBT_Label2 = new System.Windows.Forms.Label();
            this.IR_ETH_Label2 = new System.Windows.Forms.Label();
            this.IR_BCH_Label2 = new System.Windows.Forms.Label();
            this.IR_LTC_Label2 = new System.Windows.Forms.Label();
            this.IR_LTC_Label1 = new System.Windows.Forms.Label();
            this.IR_BCH_Label1 = new System.Windows.Forms.Label();
            this.IR_ETH_Label1 = new System.Windows.Forms.Label();
            this.IR_XBT_Label1 = new System.Windows.Forms.Label();
            this.IR_XBT_Label3 = new System.Windows.Forms.Label();
            this.IR_ETH_Label3 = new System.Windows.Forms.Label();
            this.IR_BCH_Label3 = new System.Windows.Forms.Label();
            this.IR_LTC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_LTC_Label3 = new System.Windows.Forms.Label();
            this.BTCM_BCH_Label3 = new System.Windows.Forms.Label();
            this.BTCM_ETH_Label3 = new System.Windows.Forms.Label();
            this.BTCM_XBT_Label3 = new System.Windows.Forms.Label();
            this.BTCM_XRP_Label3 = new System.Windows.Forms.Label();
            this.GDAX_LTC_Label3 = new System.Windows.Forms.Label();
            this.GDAX_BCH_Label3 = new System.Windows.Forms.Label();
            this.GDAX_ETH_Label3 = new System.Windows.Forms.Label();
            this.GDAX_XBT_Label3 = new System.Windows.Forms.Label();
            this.BFX_LTC_Label3 = new System.Windows.Forms.Label();
            this.BFX_BCH_Label3 = new System.Windows.Forms.Label();
            this.BFX_ETH_Label3 = new System.Windows.Forms.Label();
            this.BFX_XBT_Label3 = new System.Windows.Forms.Label();
            this.Settings.SuspendLayout();
            this.LoadingPanel.SuspendLayout();
            this.Main.SuspendLayout();
            this.BFX_GroupBox.SuspendLayout();
            this.fiat_GroupBox.SuspendLayout();
            this.GDAX_GroupBox.SuspendLayout();
            this.BTCM_GroupBox.SuspendLayout();
            this.IR_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshFrequencyTextbox
            // 
            this.refreshFrequencyTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshFrequencyTextbox.Location = new System.Drawing.Point(383, 50);
            this.refreshFrequencyTextbox.Mask = "00000";
            this.refreshFrequencyTextbox.Name = "refreshFrequencyTextbox";
            this.refreshFrequencyTextbox.Size = new System.Drawing.Size(68, 32);
            this.refreshFrequencyTextbox.TabIndex = 0;
            this.refreshFrequencyTextbox.Text = "10";
            this.refreshFrequencyTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.refreshFrequencyTextbox.ValidatingType = typeof(int);
            this.refreshFrequencyTextbox.TextChanged += new System.EventHandler(this.refreshFrequencyTextbox_TextChanged);
            // 
            // refreshFrequencyLabel
            // 
            this.refreshFrequencyLabel.AutoSize = true;
            this.refreshFrequencyLabel.Location = new System.Drawing.Point(19, 57);
            this.refreshFrequencyLabel.Name = "refreshFrequencyLabel";
            this.refreshFrequencyLabel.Size = new System.Drawing.Size(126, 26);
            this.refreshFrequencyLabel.TabIndex = 1;
            this.refreshFrequencyLabel.Text = "How fast should the \r\napp refresh (in seconds)?";
            // 
            // folderDialogButton
            // 
            this.folderDialogButton.Location = new System.Drawing.Point(310, 221);
            this.folderDialogButton.Name = "folderDialogButton";
            this.folderDialogButton.Size = new System.Drawing.Size(141, 31);
            this.folderDialogButton.TabIndex = 2;
            this.folderDialogButton.Text = "Choose folder...";
            this.folderDialogButton.UseVisualStyleBackColor = true;
            this.folderDialogButton.Click += new System.EventHandler(this.folderDialogButton_Click);
            // 
            // folderDialogTextbox
            // 
            this.folderDialogTextbox.Location = new System.Drawing.Point(191, 191);
            this.folderDialogTextbox.Name = "folderDialogTextbox";
            this.folderDialogTextbox.ReadOnly = true;
            this.folderDialogTextbox.Size = new System.Drawing.Size(260, 20);
            this.folderDialogTextbox.TabIndex = 3;
            // 
            // pollingThread
            // 
            this.pollingThread.WorkerReportsProgress = true;
            this.pollingThread.WorkerSupportsCancellation = true;
            this.pollingThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.pollingThread_DoWork);
            this.pollingThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.pollingThread_ReportProgress);
            this.pollingThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.pollingThread_RunWorkerCompleted);
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.LoadingPanel);
            this.Settings.Controls.Add(this.fiatInvert_checkBox);
            this.Settings.Controls.Add(this.InvertFiatLabel);
            this.Settings.Controls.Add(this.SettingsOKButton);
            this.Settings.Controls.Add(this.FolderLabel);
            this.Settings.Controls.Add(this.refreshFrequencyLabel);
            this.Settings.Controls.Add(this.folderDialogTextbox);
            this.Settings.Controls.Add(this.refreshFrequencyTextbox);
            this.Settings.Controls.Add(this.folderDialogButton);
            this.Settings.Location = new System.Drawing.Point(-3, -1);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(495, 505);
            this.Settings.TabIndex = 4;
            // 
            // fiatInvert_checkBox
            // 
            this.fiatInvert_checkBox.AutoSize = true;
            this.fiatInvert_checkBox.Location = new System.Drawing.Point(436, 126);
            this.fiatInvert_checkBox.Name = "fiatInvert_checkBox";
            this.fiatInvert_checkBox.Size = new System.Drawing.Size(15, 14);
            this.fiatInvert_checkBox.TabIndex = 10;
            this.fiatInvert_checkBox.UseVisualStyleBackColor = true;
            this.fiatInvert_checkBox.CheckedChanged += new System.EventHandler(this.fiatInvert_checkBox_CheckedChanged);
            // 
            // InvertFiatLabel
            // 
            this.InvertFiatLabel.AutoSize = true;
            this.InvertFiatLabel.Location = new System.Drawing.Point(19, 126);
            this.InvertFiatLabel.Name = "InvertFiatLabel";
            this.InvertFiatLabel.Size = new System.Drawing.Size(91, 13);
            this.InvertFiatLabel.TabIndex = 6;
            this.InvertFiatLabel.Text = "Invert fiat fx rates:";
            // 
            // SettingsOKButton
            // 
            this.SettingsOKButton.Location = new System.Drawing.Point(410, 458);
            this.SettingsOKButton.Name = "SettingsOKButton";
            this.SettingsOKButton.Size = new System.Drawing.Size(75, 23);
            this.SettingsOKButton.TabIndex = 4;
            this.SettingsOKButton.Text = "OK";
            this.SettingsOKButton.UseVisualStyleBackColor = true;
            this.SettingsOKButton.Click += new System.EventHandler(this.SettingsOKButton_Click);
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(19, 191);
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
            this.LoadingPanel.Size = new System.Drawing.Size(492, 572);
            this.LoadingPanel.TabIndex = 10;
            // 
            // GIFLabel
            // 
            this.GIFLabel.Image = global::IRTicker.Properties.Resources.scrooge;
            this.GIFLabel.Location = new System.Drawing.Point(-6, -62);
            this.GIFLabel.Name = "GIFLabel";
            this.GIFLabel.Size = new System.Drawing.Size(498, 550);
            this.GIFLabel.TabIndex = 0;
            this.GIFLabel.Text = "\r\n\r\n\r\n\r\n\r\n\r\nDownloading bitcoins...";
            this.GIFLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Main
            // 
            this.Main.Controls.Add(this.BFX_GroupBox);
            this.Main.Controls.Add(this.fiat_GroupBox);
            this.Main.Controls.Add(this.GDAX_GroupBox);
            this.Main.Controls.Add(this.SettingsButton);
            this.Main.Controls.Add(this.BTCM_GroupBox);
            this.Main.Controls.Add(this.IR_GroupBox);
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.Size = new System.Drawing.Size(492, 487);
            this.Main.TabIndex = 5;
            // 
            // BFX_GroupBox
            // 
            this.BFX_GroupBox.Controls.Add(this.BFX_LTC_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_BCH_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_XBT_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETH_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETH_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_XBT_Label3);
            this.BFX_GroupBox.Controls.Add(this.BFX_BCH_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_LTC_Label2);
            this.BFX_GroupBox.Controls.Add(this.BFX_LTC_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_BCH_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_ETH_Label1);
            this.BFX_GroupBox.Controls.Add(this.BFX_XBT_Label1);
            this.BFX_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.BFX_GroupBox.Location = new System.Drawing.Point(260, 171);
            this.BFX_GroupBox.Name = "BFX_GroupBox";
            this.BFX_GroupBox.Size = new System.Drawing.Size(217, 130);
            this.BFX_GroupBox.TabIndex = 9;
            this.BFX_GroupBox.TabStop = false;
            this.BFX_GroupBox.Text = "BitFinex";
            this.BFX_GroupBox.Click += new System.EventHandler(this.BFX_GroupBox_Click);
            // 
            // BFX_XBT_Label2
            // 
            this.BFX_XBT_Label2.AutoSize = true;
            this.BFX_XBT_Label2.Location = new System.Drawing.Point(56, 27);
            this.BFX_XBT_Label2.Name = "BFX_XBT_Label2";
            this.BFX_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_XBT_Label2.TabIndex = 4;
            // 
            // BFX_ETH_Label2
            // 
            this.BFX_ETH_Label2.AutoSize = true;
            this.BFX_ETH_Label2.Location = new System.Drawing.Point(56, 47);
            this.BFX_ETH_Label2.Name = "BFX_ETH_Label2";
            this.BFX_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_ETH_Label2.TabIndex = 5;
            // 
            // BFX_BCH_Label2
            // 
            this.BFX_BCH_Label2.AutoSize = true;
            this.BFX_BCH_Label2.Location = new System.Drawing.Point(56, 67);
            this.BFX_BCH_Label2.Name = "BFX_BCH_Label2";
            this.BFX_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_BCH_Label2.TabIndex = 6;
            // 
            // BFX_LTC_Label2
            // 
            this.BFX_LTC_Label2.AutoSize = true;
            this.BFX_LTC_Label2.Location = new System.Drawing.Point(56, 87);
            this.BFX_LTC_Label2.Name = "BFX_LTC_Label2";
            this.BFX_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BFX_LTC_Label2.TabIndex = 7;
            // 
            // BFX_LTC_Label1
            // 
            this.BFX_LTC_Label1.AutoSize = true;
            this.BFX_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_LTC_Label1.Location = new System.Drawing.Point(6, 87);
            this.BFX_LTC_Label1.Name = "BFX_LTC_Label1";
            this.BFX_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.BFX_LTC_Label1.TabIndex = 3;
            this.BFX_LTC_Label1.Text = "LTC:";
            // 
            // BFX_BCH_Label1
            // 
            this.BFX_BCH_Label1.AutoSize = true;
            this.BFX_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_BCH_Label1.Location = new System.Drawing.Point(6, 67);
            this.BFX_BCH_Label1.Name = "BFX_BCH_Label1";
            this.BFX_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_BCH_Label1.TabIndex = 2;
            this.BFX_BCH_Label1.Text = "BCH:";
            // 
            // BFX_ETH_Label1
            // 
            this.BFX_ETH_Label1.AutoSize = true;
            this.BFX_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_ETH_Label1.Location = new System.Drawing.Point(6, 47);
            this.BFX_ETH_Label1.Name = "BFX_ETH_Label1";
            this.BFX_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BFX_ETH_Label1.TabIndex = 1;
            this.BFX_ETH_Label1.Text = "ETH:";
            // 
            // BFX_XBT_Label1
            // 
            this.BFX_XBT_Label1.AutoSize = true;
            this.BFX_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BFX_XBT_Label1.Location = new System.Drawing.Point(6, 27);
            this.BFX_XBT_Label1.Name = "BFX_XBT_Label1";
            this.BFX_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BFX_XBT_Label1.TabIndex = 0;
            this.BFX_XBT_Label1.Text = "XBT:";
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
            this.fiat_GroupBox.Location = new System.Drawing.Point(142, 320);
            this.fiat_GroupBox.Name = "fiat_GroupBox";
            this.fiat_GroupBox.Size = new System.Drawing.Size(217, 130);
            this.fiat_GroupBox.TabIndex = 9;
            this.fiat_GroupBox.TabStop = false;
            this.fiat_GroupBox.Text = "Fiat rates";
            this.fiat_GroupBox.Click += new System.EventHandler(this.fiat_GroupBox_Click);
            // 
            // fiatRefresh_checkBox
            // 
            this.fiatRefresh_checkBox.AutoSize = true;
            this.fiatRefresh_checkBox.Location = new System.Drawing.Point(9, 107);
            this.fiatRefresh_checkBox.Name = "fiatRefresh_checkBox";
            this.fiatRefresh_checkBox.Size = new System.Drawing.Size(143, 17);
            this.fiatRefresh_checkBox.TabIndex = 9;
            this.fiatRefresh_checkBox.Text = "Tick to queue an update";
            this.fiatRefresh_checkBox.UseVisualStyleBackColor = true;
            this.fiatRefresh_checkBox.CheckedChanged += new System.EventHandler(this.fiat_checkBox_CheckedChanged);
            // 
            // USD_Label2
            // 
            this.USD_Label2.AutoSize = true;
            this.USD_Label2.Location = new System.Drawing.Point(56, 87);
            this.USD_Label2.Name = "USD_Label2";
            this.USD_Label2.Size = new System.Drawing.Size(0, 13);
            this.USD_Label2.TabIndex = 8;
            // 
            // USD_Label1
            // 
            this.USD_Label1.AutoSize = true;
            this.USD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USD_Label1.Location = new System.Drawing.Point(6, 87);
            this.USD_Label1.Name = "USD_Label1";
            this.USD_Label1.Size = new System.Drawing.Size(37, 13);
            this.USD_Label1.TabIndex = 7;
            this.USD_Label1.Text = "USD:";
            // 
            // AUD_Label2
            // 
            this.AUD_Label2.AutoSize = true;
            this.AUD_Label2.Location = new System.Drawing.Point(56, 27);
            this.AUD_Label2.Name = "AUD_Label2";
            this.AUD_Label2.Size = new System.Drawing.Size(0, 13);
            this.AUD_Label2.TabIndex = 4;
            // 
            // NZD_Label2
            // 
            this.NZD_Label2.AutoSize = true;
            this.NZD_Label2.Location = new System.Drawing.Point(56, 47);
            this.NZD_Label2.Name = "NZD_Label2";
            this.NZD_Label2.Size = new System.Drawing.Size(0, 13);
            this.NZD_Label2.TabIndex = 5;
            // 
            // EUR_Label2
            // 
            this.EUR_Label2.AutoSize = true;
            this.EUR_Label2.Location = new System.Drawing.Point(56, 67);
            this.EUR_Label2.Name = "EUR_Label2";
            this.EUR_Label2.Size = new System.Drawing.Size(0, 13);
            this.EUR_Label2.TabIndex = 6;
            // 
            // EUR_Label1
            // 
            this.EUR_Label1.AutoSize = true;
            this.EUR_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EUR_Label1.Location = new System.Drawing.Point(6, 67);
            this.EUR_Label1.Name = "EUR_Label1";
            this.EUR_Label1.Size = new System.Drawing.Size(37, 13);
            this.EUR_Label1.TabIndex = 2;
            this.EUR_Label1.Text = "EUR:";
            // 
            // NZD_Label1
            // 
            this.NZD_Label1.AutoSize = true;
            this.NZD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NZD_Label1.Location = new System.Drawing.Point(6, 47);
            this.NZD_Label1.Name = "NZD_Label1";
            this.NZD_Label1.Size = new System.Drawing.Size(37, 13);
            this.NZD_Label1.TabIndex = 1;
            this.NZD_Label1.Text = "NZD:";
            // 
            // AUD_Label1
            // 
            this.AUD_Label1.AutoSize = true;
            this.AUD_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AUD_Label1.Location = new System.Drawing.Point(6, 27);
            this.AUD_Label1.Name = "AUD_Label1";
            this.AUD_Label1.Size = new System.Drawing.Size(37, 13);
            this.AUD_Label1.TabIndex = 0;
            this.AUD_Label1.Text = "AUD:";
            // 
            // GDAX_GroupBox
            // 
            this.GDAX_GroupBox.Controls.Add(this.GDAX_LTC_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BCH_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XBT_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETH_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETH_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XBT_Label3);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BCH_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_LTC_Label2);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_LTC_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_BCH_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_ETH_Label1);
            this.GDAX_GroupBox.Controls.Add(this.GDAX_XBT_Label1);
            this.GDAX_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.GDAX_GroupBox.Location = new System.Drawing.Point(19, 171);
            this.GDAX_GroupBox.Name = "GDAX_GroupBox";
            this.GDAX_GroupBox.Size = new System.Drawing.Size(217, 130);
            this.GDAX_GroupBox.TabIndex = 8;
            this.GDAX_GroupBox.TabStop = false;
            this.GDAX_GroupBox.Text = "GDAX";
            this.GDAX_GroupBox.Click += new System.EventHandler(this.GDAX_GroupBox_Click);
            // 
            // GDAX_XBT_Label2
            // 
            this.GDAX_XBT_Label2.AutoSize = true;
            this.GDAX_XBT_Label2.Location = new System.Drawing.Point(56, 27);
            this.GDAX_XBT_Label2.Name = "GDAX_XBT_Label2";
            this.GDAX_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_XBT_Label2.TabIndex = 4;
            // 
            // GDAX_ETH_Label2
            // 
            this.GDAX_ETH_Label2.AutoSize = true;
            this.GDAX_ETH_Label2.Location = new System.Drawing.Point(56, 47);
            this.GDAX_ETH_Label2.Name = "GDAX_ETH_Label2";
            this.GDAX_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_ETH_Label2.TabIndex = 5;
            // 
            // GDAX_BCH_Label2
            // 
            this.GDAX_BCH_Label2.AutoSize = true;
            this.GDAX_BCH_Label2.Location = new System.Drawing.Point(56, 67);
            this.GDAX_BCH_Label2.Name = "GDAX_BCH_Label2";
            this.GDAX_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_BCH_Label2.TabIndex = 6;
            // 
            // GDAX_LTC_Label2
            // 
            this.GDAX_LTC_Label2.AutoSize = true;
            this.GDAX_LTC_Label2.Location = new System.Drawing.Point(56, 87);
            this.GDAX_LTC_Label2.Name = "GDAX_LTC_Label2";
            this.GDAX_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.GDAX_LTC_Label2.TabIndex = 7;
            // 
            // GDAX_LTC_Label1
            // 
            this.GDAX_LTC_Label1.AutoSize = true;
            this.GDAX_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_LTC_Label1.Location = new System.Drawing.Point(6, 87);
            this.GDAX_LTC_Label1.Name = "GDAX_LTC_Label1";
            this.GDAX_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.GDAX_LTC_Label1.TabIndex = 3;
            this.GDAX_LTC_Label1.Text = "LTC:";
            // 
            // GDAX_BCH_Label1
            // 
            this.GDAX_BCH_Label1.AutoSize = true;
            this.GDAX_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_BCH_Label1.Location = new System.Drawing.Point(6, 67);
            this.GDAX_BCH_Label1.Name = "GDAX_BCH_Label1";
            this.GDAX_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_BCH_Label1.TabIndex = 2;
            this.GDAX_BCH_Label1.Text = "BCH:";
            // 
            // GDAX_ETH_Label1
            // 
            this.GDAX_ETH_Label1.AutoSize = true;
            this.GDAX_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_ETH_Label1.Location = new System.Drawing.Point(6, 47);
            this.GDAX_ETH_Label1.Name = "GDAX_ETH_Label1";
            this.GDAX_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.GDAX_ETH_Label1.TabIndex = 1;
            this.GDAX_ETH_Label1.Text = "ETH:";
            // 
            // GDAX_XBT_Label1
            // 
            this.GDAX_XBT_Label1.AutoSize = true;
            this.GDAX_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GDAX_XBT_Label1.Location = new System.Drawing.Point(6, 27);
            this.GDAX_XBT_Label1.Name = "GDAX_XBT_Label1";
            this.GDAX_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.GDAX_XBT_Label1.TabIndex = 0;
            this.GDAX_XBT_Label1.Text = "XBT:";
            // 
            // SettingsButton
            // 
            this.SettingsButton.Location = new System.Drawing.Point(407, 457);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(75, 23);
            this.SettingsButton.TabIndex = 2;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // BTCM_GroupBox
            // 
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XRP_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_LTC_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XRP_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BCH_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETH_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XBT_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XBT_Label3);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XRP_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETH_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_ETH_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_XBT_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BCH_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_BCH_Label1);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_LTC_Label2);
            this.BTCM_GroupBox.Controls.Add(this.BTCM_LTC_Label1);
            this.BTCM_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.BTCM_GroupBox.Location = new System.Drawing.Point(260, 22);
            this.BTCM_GroupBox.Name = "BTCM_GroupBox";
            this.BTCM_GroupBox.Size = new System.Drawing.Size(217, 130);
            this.BTCM_GroupBox.TabIndex = 1;
            this.BTCM_GroupBox.TabStop = false;
            this.BTCM_GroupBox.Text = "BTC Markets";
            // 
            // BTCM_XRP_Label2
            // 
            this.BTCM_XRP_Label2.AutoSize = true;
            this.BTCM_XRP_Label2.Location = new System.Drawing.Point(56, 103);
            this.BTCM_XRP_Label2.Name = "BTCM_XRP_Label2";
            this.BTCM_XRP_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XRP_Label2.TabIndex = 9;
            // 
            // BTCM_XBT_Label2
            // 
            this.BTCM_XBT_Label2.AutoSize = true;
            this.BTCM_XBT_Label2.Location = new System.Drawing.Point(56, 23);
            this.BTCM_XBT_Label2.Name = "BTCM_XBT_Label2";
            this.BTCM_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_XBT_Label2.TabIndex = 12;
            // 
            // BTCM_XRP_Label1
            // 
            this.BTCM_XRP_Label1.AutoSize = true;
            this.BTCM_XRP_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XRP_Label1.Location = new System.Drawing.Point(6, 103);
            this.BTCM_XRP_Label1.Name = "BTCM_XRP_Label1";
            this.BTCM_XRP_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_XRP_Label1.TabIndex = 8;
            this.BTCM_XRP_Label1.Text = "XRP:";
            // 
            // BTCM_ETH_Label1
            // 
            this.BTCM_ETH_Label1.AutoSize = true;
            this.BTCM_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_ETH_Label1.Location = new System.Drawing.Point(6, 43);
            this.BTCM_ETH_Label1.Name = "BTCM_ETH_Label1";
            this.BTCM_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_ETH_Label1.TabIndex = 9;
            this.BTCM_ETH_Label1.Text = "ETH:";
            // 
            // BTCM_ETH_Label2
            // 
            this.BTCM_ETH_Label2.AutoSize = true;
            this.BTCM_ETH_Label2.Location = new System.Drawing.Point(56, 43);
            this.BTCM_ETH_Label2.Name = "BTCM_ETH_Label2";
            this.BTCM_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_ETH_Label2.TabIndex = 13;
            // 
            // BTCM_XBT_Label1
            // 
            this.BTCM_XBT_Label1.AutoSize = true;
            this.BTCM_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.BTCM_XBT_Label1.Name = "BTCM_XBT_Label1";
            this.BTCM_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.BTCM_XBT_Label1.TabIndex = 8;
            this.BTCM_XBT_Label1.Text = "XBT:";
            // 
            // BTCM_BCH_Label2
            // 
            this.BTCM_BCH_Label2.AutoSize = true;
            this.BTCM_BCH_Label2.Location = new System.Drawing.Point(56, 63);
            this.BTCM_BCH_Label2.Name = "BTCM_BCH_Label2";
            this.BTCM_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_BCH_Label2.TabIndex = 14;
            // 
            // BTCM_BCH_Label1
            // 
            this.BTCM_BCH_Label1.AutoSize = true;
            this.BTCM_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_BCH_Label1.Location = new System.Drawing.Point(6, 63);
            this.BTCM_BCH_Label1.Name = "BTCM_BCH_Label1";
            this.BTCM_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.BTCM_BCH_Label1.TabIndex = 10;
            this.BTCM_BCH_Label1.Text = "BCH:";
            // 
            // BTCM_LTC_Label2
            // 
            this.BTCM_LTC_Label2.AutoSize = true;
            this.BTCM_LTC_Label2.Location = new System.Drawing.Point(56, 83);
            this.BTCM_LTC_Label2.Name = "BTCM_LTC_Label2";
            this.BTCM_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.BTCM_LTC_Label2.TabIndex = 15;
            // 
            // BTCM_LTC_Label1
            // 
            this.BTCM_LTC_Label1.AutoSize = true;
            this.BTCM_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTCM_LTC_Label1.Location = new System.Drawing.Point(6, 83);
            this.BTCM_LTC_Label1.Name = "BTCM_LTC_Label1";
            this.BTCM_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.BTCM_LTC_Label1.TabIndex = 11;
            this.BTCM_LTC_Label1.Text = "LTC:";
            // 
            // IR_GroupBox
            // 
            this.IR_GroupBox.Controls.Add(this.IR_LTC_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_BCH_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_ETH_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_XBT_Label3);
            this.IR_GroupBox.Controls.Add(this.IR_XBT_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_ETH_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_BCH_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_LTC_Label2);
            this.IR_GroupBox.Controls.Add(this.IR_LTC_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_BCH_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_ETH_Label1);
            this.IR_GroupBox.Controls.Add(this.IR_XBT_Label1);
            this.IR_GroupBox.ForeColor = System.Drawing.Color.Gray;
            this.IR_GroupBox.Location = new System.Drawing.Point(19, 22);
            this.IR_GroupBox.Name = "IR_GroupBox";
            this.IR_GroupBox.Size = new System.Drawing.Size(217, 130);
            this.IR_GroupBox.TabIndex = 0;
            this.IR_GroupBox.TabStop = false;
            this.IR_GroupBox.Text = "Independent Reserve";
            this.IR_GroupBox.Click += new System.EventHandler(this.IR_GroupBox_Click);
            // 
            // IR_XBT_Label2
            // 
            this.IR_XBT_Label2.AutoSize = true;
            this.IR_XBT_Label2.Location = new System.Drawing.Point(56, 23);
            this.IR_XBT_Label2.Name = "IR_XBT_Label2";
            this.IR_XBT_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_XBT_Label2.TabIndex = 4;
            // 
            // IR_ETH_Label2
            // 
            this.IR_ETH_Label2.AutoSize = true;
            this.IR_ETH_Label2.Location = new System.Drawing.Point(56, 43);
            this.IR_ETH_Label2.Name = "IR_ETH_Label2";
            this.IR_ETH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_ETH_Label2.TabIndex = 5;
            // 
            // IR_BCH_Label2
            // 
            this.IR_BCH_Label2.AutoSize = true;
            this.IR_BCH_Label2.Location = new System.Drawing.Point(56, 63);
            this.IR_BCH_Label2.Name = "IR_BCH_Label2";
            this.IR_BCH_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_BCH_Label2.TabIndex = 6;
            // 
            // IR_LTC_Label2
            // 
            this.IR_LTC_Label2.AutoSize = true;
            this.IR_LTC_Label2.Location = new System.Drawing.Point(56, 83);
            this.IR_LTC_Label2.Name = "IR_LTC_Label2";
            this.IR_LTC_Label2.Size = new System.Drawing.Size(0, 13);
            this.IR_LTC_Label2.TabIndex = 7;
            // 
            // IR_LTC_Label1
            // 
            this.IR_LTC_Label1.AutoSize = true;
            this.IR_LTC_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_LTC_Label1.Location = new System.Drawing.Point(6, 83);
            this.IR_LTC_Label1.Name = "IR_LTC_Label1";
            this.IR_LTC_Label1.Size = new System.Drawing.Size(34, 13);
            this.IR_LTC_Label1.TabIndex = 3;
            this.IR_LTC_Label1.Text = "LTC:";
            // 
            // IR_BCH_Label1
            // 
            this.IR_BCH_Label1.AutoSize = true;
            this.IR_BCH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_BCH_Label1.Location = new System.Drawing.Point(6, 63);
            this.IR_BCH_Label1.Name = "IR_BCH_Label1";
            this.IR_BCH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_BCH_Label1.TabIndex = 2;
            this.IR_BCH_Label1.Text = "BCH:";
            // 
            // IR_ETH_Label1
            // 
            this.IR_ETH_Label1.AutoSize = true;
            this.IR_ETH_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_ETH_Label1.Location = new System.Drawing.Point(6, 43);
            this.IR_ETH_Label1.Name = "IR_ETH_Label1";
            this.IR_ETH_Label1.Size = new System.Drawing.Size(36, 13);
            this.IR_ETH_Label1.TabIndex = 1;
            this.IR_ETH_Label1.Text = "ETH:";
            // 
            // IR_XBT_Label1
            // 
            this.IR_XBT_Label1.AutoSize = true;
            this.IR_XBT_Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IR_XBT_Label1.Location = new System.Drawing.Point(6, 23);
            this.IR_XBT_Label1.Name = "IR_XBT_Label1";
            this.IR_XBT_Label1.Size = new System.Drawing.Size(35, 13);
            this.IR_XBT_Label1.TabIndex = 0;
            this.IR_XBT_Label1.Text = "XBT:";
            // 
            // IR_XBT_Label3
            // 
            this.IR_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_XBT_Label3.Location = new System.Drawing.Point(123, 23);
            this.IR_XBT_Label3.Name = "IR_XBT_Label3";
            this.IR_XBT_Label3.Size = new System.Drawing.Size(84, 11);
            this.IR_XBT_Label3.TabIndex = 8;
            this.IR_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // IR_ETH_Label3
            // 
            this.IR_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_ETH_Label3.Location = new System.Drawing.Point(123, 43);
            this.IR_ETH_Label3.Name = "IR_ETH_Label3";
            this.IR_ETH_Label3.Size = new System.Drawing.Size(84, 13);
            this.IR_ETH_Label3.TabIndex = 9;
            this.IR_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // IR_BCH_Label3
            // 
            this.IR_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_BCH_Label3.Location = new System.Drawing.Point(123, 63);
            this.IR_BCH_Label3.Name = "IR_BCH_Label3";
            this.IR_BCH_Label3.Size = new System.Drawing.Size(84, 13);
            this.IR_BCH_Label3.TabIndex = 10;
            this.IR_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // IR_LTC_Label3
            // 
            this.IR_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.IR_LTC_Label3.Location = new System.Drawing.Point(123, 83);
            this.IR_LTC_Label3.Name = "IR_LTC_Label3";
            this.IR_LTC_Label3.Size = new System.Drawing.Size(84, 13);
            this.IR_LTC_Label3.TabIndex = 11;
            this.IR_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_LTC_Label3
            // 
            this.BTCM_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_LTC_Label3.Location = new System.Drawing.Point(128, 83);
            this.BTCM_LTC_Label3.Name = "BTCM_LTC_Label3";
            this.BTCM_LTC_Label3.Size = new System.Drawing.Size(78, 13);
            this.BTCM_LTC_Label3.TabIndex = 15;
            this.BTCM_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_BCH_Label3
            // 
            this.BTCM_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_BCH_Label3.Location = new System.Drawing.Point(128, 63);
            this.BTCM_BCH_Label3.Name = "BTCM_BCH_Label3";
            this.BTCM_BCH_Label3.Size = new System.Drawing.Size(78, 13);
            this.BTCM_BCH_Label3.TabIndex = 14;
            this.BTCM_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_ETH_Label3
            // 
            this.BTCM_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_ETH_Label3.Location = new System.Drawing.Point(128, 43);
            this.BTCM_ETH_Label3.Name = "BTCM_ETH_Label3";
            this.BTCM_ETH_Label3.Size = new System.Drawing.Size(78, 13);
            this.BTCM_ETH_Label3.TabIndex = 13;
            this.BTCM_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_XBT_Label3
            // 
            this.BTCM_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_XBT_Label3.Location = new System.Drawing.Point(128, 23);
            this.BTCM_XBT_Label3.Name = "BTCM_XBT_Label3";
            this.BTCM_XBT_Label3.Size = new System.Drawing.Size(78, 13);
            this.BTCM_XBT_Label3.TabIndex = 12;
            this.BTCM_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTCM_XRP_Label3
            // 
            this.BTCM_XRP_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BTCM_XRP_Label3.Location = new System.Drawing.Point(128, 103);
            this.BTCM_XRP_Label3.Name = "BTCM_XRP_Label3";
            this.BTCM_XRP_Label3.Size = new System.Drawing.Size(78, 13);
            this.BTCM_XRP_Label3.TabIndex = 16;
            this.BTCM_XRP_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_LTC_Label3
            // 
            this.GDAX_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_LTC_Label3.Location = new System.Drawing.Point(126, 87);
            this.GDAX_LTC_Label3.Name = "GDAX_LTC_Label3";
            this.GDAX_LTC_Label3.Size = new System.Drawing.Size(81, 13);
            this.GDAX_LTC_Label3.TabIndex = 15;
            this.GDAX_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_BCH_Label3
            // 
            this.GDAX_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_BCH_Label3.Location = new System.Drawing.Point(126, 67);
            this.GDAX_BCH_Label3.Name = "GDAX_BCH_Label3";
            this.GDAX_BCH_Label3.Size = new System.Drawing.Size(81, 13);
            this.GDAX_BCH_Label3.TabIndex = 14;
            this.GDAX_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_ETH_Label3
            // 
            this.GDAX_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_ETH_Label3.Location = new System.Drawing.Point(126, 47);
            this.GDAX_ETH_Label3.Name = "GDAX_ETH_Label3";
            this.GDAX_ETH_Label3.Size = new System.Drawing.Size(81, 13);
            this.GDAX_ETH_Label3.TabIndex = 13;
            this.GDAX_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GDAX_XBT_Label3
            // 
            this.GDAX_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GDAX_XBT_Label3.Location = new System.Drawing.Point(126, 27);
            this.GDAX_XBT_Label3.Name = "GDAX_XBT_Label3";
            this.GDAX_XBT_Label3.Size = new System.Drawing.Size(81, 13);
            this.GDAX_XBT_Label3.TabIndex = 12;
            this.GDAX_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_LTC_Label3
            // 
            this.BFX_LTC_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_LTC_Label3.Location = new System.Drawing.Point(131, 87);
            this.BFX_LTC_Label3.Name = "BFX_LTC_Label3";
            this.BFX_LTC_Label3.Size = new System.Drawing.Size(75, 13);
            this.BFX_LTC_Label3.TabIndex = 19;
            this.BFX_LTC_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_BCH_Label3
            // 
            this.BFX_BCH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_BCH_Label3.Location = new System.Drawing.Point(131, 67);
            this.BFX_BCH_Label3.Name = "BFX_BCH_Label3";
            this.BFX_BCH_Label3.Size = new System.Drawing.Size(75, 13);
            this.BFX_BCH_Label3.TabIndex = 18;
            this.BFX_BCH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_ETH_Label3
            // 
            this.BFX_ETH_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_ETH_Label3.Location = new System.Drawing.Point(131, 47);
            this.BFX_ETH_Label3.Name = "BFX_ETH_Label3";
            this.BFX_ETH_Label3.Size = new System.Drawing.Size(75, 13);
            this.BFX_ETH_Label3.TabIndex = 17;
            this.BFX_ETH_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BFX_XBT_Label3
            // 
            this.BFX_XBT_Label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BFX_XBT_Label3.Location = new System.Drawing.Point(131, 27);
            this.BFX_XBT_Label3.Name = "BFX_XBT_Label3";
            this.BFX_XBT_Label3.Size = new System.Drawing.Size(75, 13);
            this.BFX_XBT_Label3.TabIndex = 16;
            this.BFX_XBT_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // IRTicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 489);
            this.Controls.Add(this.Main);
            this.Controls.Add(this.Settings);
            this.Name = "IRTicker";
            this.Text = "IR Ticker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IRTicker_Closing);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.LoadingPanel.ResumeLayout(false);
            this.Main.ResumeLayout(false);
            this.BFX_GroupBox.ResumeLayout(false);
            this.BFX_GroupBox.PerformLayout();
            this.fiat_GroupBox.ResumeLayout(false);
            this.fiat_GroupBox.PerformLayout();
            this.GDAX_GroupBox.ResumeLayout(false);
            this.GDAX_GroupBox.PerformLayout();
            this.BTCM_GroupBox.ResumeLayout(false);
            this.BTCM_GroupBox.PerformLayout();
            this.IR_GroupBox.ResumeLayout(false);
            this.IR_GroupBox.PerformLayout();
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
        private System.Windows.Forms.CheckBox fiatInvert_checkBox;
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
        private System.Windows.Forms.Label InvertFiatLabel;
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
    }
}

