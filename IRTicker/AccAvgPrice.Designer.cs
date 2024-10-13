namespace IRTicker
{
    partial class AccAvgPrice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccAvgPrice));
            this.AccAvgPrice_Crypto_ComboBox = new System.Windows.Forms.ComboBox();
            this.AccAvgPrice_Start_DTPicker = new System.Windows.Forms.DateTimePicker();
            this.AccAvgPrice_Go_Button = new System.Windows.Forms.Button();
            this.AccAvgPrice_Status_Label = new System.Windows.Forms.Label();
            this.AccAvgPrice_End_DTPicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AccAvgPrice_End_Label = new System.Windows.Forms.Label();
            this.AccAvgPrice_Start_Label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AccAvgPrice_Result_TextBox = new System.Windows.Forms.TextBox();
            this.AccAvgPrice_CopyAvg_Button = new System.Windows.Forms.Button();
            this.AccAvgPrice_AutoUpdate_CheckBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.AccAvgPrice_BuySell_ComboBox = new System.Windows.Forms.ComboBox();
            this.AccAvgPrice_TT = new System.Windows.Forms.ToolTip(this.components);
            this.AccAvgPrice_DealSize_TextBox = new System.Windows.Forms.TextBox();
            this.AccAvgPrice_DealSizeCurrency_ComboBox = new System.Windows.Forms.ComboBox();
            this.AccAvgPrice_SendRemainingToVolumeField_button = new System.Windows.Forms.Button();
            this.AccAvgPrice_DealComment_TextBox = new System.Windows.Forms.TextBox();
            this.AccAvgPrice_EndDay_Label = new System.Windows.Forms.Label();
            this.AccAvgPrice_StartDay_Label = new System.Windows.Forms.Label();
            this.AccAvgPrice_TotalCrypto_TextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.AccAvgPrice_TotalFiat_TextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AccAvgPrice_RemainingToDeal_TextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.AccAvgPrice_RemaingToDealCurrency_Label = new System.Windows.Forms.Label();
            this.AccAvgPrice_FiatAUD_button = new System.Windows.Forms.Button();
            this.AccAvgPrice_FiatUSD_button = new System.Windows.Forms.Button();
            this.AccAvgPrice_FiatNZD_button = new System.Windows.Forms.Button();
            this.AccAvgPrice_FiatSGD_button = new System.Windows.Forms.Button();
            this.AccAvgPrice_DealComment_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AccAvgPrice_Crypto_ComboBox
            // 
            this.AccAvgPrice_Crypto_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccAvgPrice_Crypto_ComboBox.FormattingEnabled = true;
            this.AccAvgPrice_Crypto_ComboBox.Location = new System.Drawing.Point(112, 12);
            this.AccAvgPrice_Crypto_ComboBox.Name = "AccAvgPrice_Crypto_ComboBox";
            this.AccAvgPrice_Crypto_ComboBox.Size = new System.Drawing.Size(106, 21);
            this.AccAvgPrice_Crypto_ComboBox.TabIndex = 0;
            this.AccAvgPrice_Crypto_ComboBox.SelectedIndexChanged += new System.EventHandler(this.AccAvgPrice_Crypto_ComboBox_SelectedIndexChanged);
            // 
            // AccAvgPrice_Start_DTPicker
            // 
            this.AccAvgPrice_Start_DTPicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.AccAvgPrice_Start_DTPicker.Location = new System.Drawing.Point(112, 117);
            this.AccAvgPrice_Start_DTPicker.Name = "AccAvgPrice_Start_DTPicker";
            this.AccAvgPrice_Start_DTPicker.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_Start_DTPicker.TabIndex = 2;
            this.AccAvgPrice_Start_DTPicker.ValueChanged += new System.EventHandler(this.AccAvgPrice_Start_DTPicker_ValueChanged);
            // 
            // AccAvgPrice_Go_Button
            // 
            this.AccAvgPrice_Go_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AccAvgPrice_Go_Button.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccAvgPrice_Go_Button.Location = new System.Drawing.Point(230, 10);
            this.AccAvgPrice_Go_Button.Name = "AccAvgPrice_Go_Button";
            this.AccAvgPrice_Go_Button.Size = new System.Drawing.Size(98, 52);
            this.AccAvgPrice_Go_Button.TabIndex = 3;
            this.AccAvgPrice_Go_Button.Text = "Calculate avg price";
            this.AccAvgPrice_Go_Button.UseVisualStyleBackColor = true;
            this.AccAvgPrice_Go_Button.Click += new System.EventHandler(this.AccAvgPrice_Go_Button_Click);
            // 
            // AccAvgPrice_Status_Label
            // 
            this.AccAvgPrice_Status_Label.AutoSize = true;
            this.AccAvgPrice_Status_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_Status_Label.Location = new System.Drawing.Point(109, 186);
            this.AccAvgPrice_Status_Label.Name = "AccAvgPrice_Status_Label";
            this.AccAvgPrice_Status_Label.Size = new System.Drawing.Size(47, 13);
            this.AccAvgPrice_Status_Label.TabIndex = 4;
            this.AccAvgPrice_Status_Label.Text = "Ready...";
            // 
            // AccAvgPrice_End_DTPicker
            // 
            this.AccAvgPrice_End_DTPicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.AccAvgPrice_End_DTPicker.Location = new System.Drawing.Point(112, 143);
            this.AccAvgPrice_End_DTPicker.Name = "AccAvgPrice_End_DTPicker";
            this.AccAvgPrice_End_DTPicker.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_End_DTPicker.TabIndex = 5;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_End_DTPicker, "Defaults to +24 hours from the current time");
            this.AccAvgPrice_End_DTPicker.ValueChanged += new System.EventHandler(this.AccAvgPrice_End_DTPicker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Crypto:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Fiat:";
            // 
            // AccAvgPrice_End_Label
            // 
            this.AccAvgPrice_End_Label.AutoSize = true;
            this.AccAvgPrice_End_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_End_Label.Location = new System.Drawing.Point(13, 147);
            this.AccAvgPrice_End_Label.Name = "AccAvgPrice_End_Label";
            this.AccAvgPrice_End_Label.Size = new System.Drawing.Size(90, 13);
            this.AccAvgPrice_End_Label.TabIndex = 10;
            this.AccAvgPrice_End_Label.Text = "Last order before:";
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_End_Label, "Double click to set this to now + 1 day");
            this.AccAvgPrice_End_Label.DoubleClick += new System.EventHandler(this.AccAvgPrice_End_Label_DoubleClick);
            // 
            // AccAvgPrice_Start_Label
            // 
            this.AccAvgPrice_Start_Label.AutoSize = true;
            this.AccAvgPrice_Start_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_Start_Label.Location = new System.Drawing.Point(13, 121);
            this.AccAvgPrice_Start_Label.Name = "AccAvgPrice_Start_Label";
            this.AccAvgPrice_Start_Label.Size = new System.Drawing.Size(80, 13);
            this.AccAvgPrice_Start_Label.TabIndex = 9;
            this.AccAvgPrice_Start_Label.Text = "First order after:";
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_Start_Label, "Double click to set this to now");
            this.AccAvgPrice_Start_Label.DoubleClick += new System.EventHandler(this.AccAvgPrice_Start_Label_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Average price:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(13, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Status:";
            // 
            // AccAvgPrice_Result_TextBox
            // 
            this.AccAvgPrice_Result_TextBox.Location = new System.Drawing.Point(112, 209);
            this.AccAvgPrice_Result_TextBox.Name = "AccAvgPrice_Result_TextBox";
            this.AccAvgPrice_Result_TextBox.ReadOnly = true;
            this.AccAvgPrice_Result_TextBox.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_Result_TextBox.TabIndex = 13;
            // 
            // AccAvgPrice_CopyAvg_Button
            // 
            this.AccAvgPrice_CopyAvg_Button.Enabled = false;
            this.AccAvgPrice_CopyAvg_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AccAvgPrice_CopyAvg_Button.Location = new System.Drawing.Point(230, 207);
            this.AccAvgPrice_CopyAvg_Button.Name = "AccAvgPrice_CopyAvg_Button";
            this.AccAvgPrice_CopyAvg_Button.Size = new System.Drawing.Size(98, 23);
            this.AccAvgPrice_CopyAvg_Button.TabIndex = 14;
            this.AccAvgPrice_CopyAvg_Button.Text = "Copy price";
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_CopyAvg_Button, "Will copy the average price with no formatting, just a decimal value");
            this.AccAvgPrice_CopyAvg_Button.UseVisualStyleBackColor = true;
            this.AccAvgPrice_CopyAvg_Button.Click += new System.EventHandler(this.AccAvgPrice_Copy_Button_Click);
            // 
            // AccAvgPrice_AutoUpdate_CheckBox
            // 
            this.AccAvgPrice_AutoUpdate_CheckBox.AutoSize = true;
            this.AccAvgPrice_AutoUpdate_CheckBox.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_AutoUpdate_CheckBox.Checked = true;
            this.AccAvgPrice_AutoUpdate_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AccAvgPrice_AutoUpdate_CheckBox.Location = new System.Drawing.Point(230, 68);
            this.AccAvgPrice_AutoUpdate_CheckBox.Name = "AccAvgPrice_AutoUpdate_CheckBox";
            this.AccAvgPrice_AutoUpdate_CheckBox.Size = new System.Drawing.Size(110, 17);
            this.AccAvgPrice_AutoUpdate_CheckBox.TabIndex = 15;
            this.AccAvgPrice_AutoUpdate_CheckBox.Text = "Auto update price";
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_AutoUpdate_CheckBox, "If enabled will automatically calculate the average price every time the app pull" +
        "s the closed orders");
            this.AccAvgPrice_AutoUpdate_CheckBox.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(13, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Direction:";
            // 
            // AccAvgPrice_BuySell_ComboBox
            // 
            this.AccAvgPrice_BuySell_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccAvgPrice_BuySell_ComboBox.FormattingEnabled = true;
            this.AccAvgPrice_BuySell_ComboBox.Items.AddRange(new object[] {
            "Buy",
            "Sell",
            "Both"});
            this.AccAvgPrice_BuySell_ComboBox.Location = new System.Drawing.Point(112, 91);
            this.AccAvgPrice_BuySell_ComboBox.Name = "AccAvgPrice_BuySell_ComboBox";
            this.AccAvgPrice_BuySell_ComboBox.Size = new System.Drawing.Size(106, 21);
            this.AccAvgPrice_BuySell_ComboBox.TabIndex = 16;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_BuySell_ComboBox, "Not sure why \"both\" would be useful, but it\'s there anyway");
            // 
            // AccAvgPrice_DealSize_TextBox
            // 
            this.AccAvgPrice_DealSize_TextBox.Location = new System.Drawing.Point(112, 249);
            this.AccAvgPrice_DealSize_TextBox.Name = "AccAvgPrice_DealSize_TextBox";
            this.AccAvgPrice_DealSize_TextBox.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_DealSize_TextBox.TabIndex = 25;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_DealSize_TextBox, "Enter here what size you need to do on the order book.");
            this.AccAvgPrice_DealSize_TextBox.TextChanged += new System.EventHandler(this.AccAvgPrice_DealSize_TextBox_TextChanged);
            // 
            // AccAvgPrice_DealSizeCurrency_ComboBox
            // 
            this.AccAvgPrice_DealSizeCurrency_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccAvgPrice_DealSizeCurrency_ComboBox.FormattingEnabled = true;
            this.AccAvgPrice_DealSizeCurrency_ComboBox.Items.AddRange(new object[] {
            "",
            "Crypto",
            "Fiat"});
            this.AccAvgPrice_DealSizeCurrency_ComboBox.Location = new System.Drawing.Point(230, 249);
            this.AccAvgPrice_DealSizeCurrency_ComboBox.Name = "AccAvgPrice_DealSizeCurrency_ComboBox";
            this.AccAvgPrice_DealSizeCurrency_ComboBox.Size = new System.Drawing.Size(98, 21);
            this.AccAvgPrice_DealSizeCurrency_ComboBox.TabIndex = 26;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_DealSizeCurrency_ComboBox, "Choose the currency of the size of the deal value");
            this.AccAvgPrice_DealSizeCurrency_ComboBox.SelectedIndexChanged += new System.EventHandler(this.AccAvgPrice_DealSizeCurrency_ComboBox_SelectedIndexChanged);
            // 
            // AccAvgPrice_SendRemainingToVolumeField_button
            // 
            this.AccAvgPrice_SendRemainingToVolumeField_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AccAvgPrice_SendRemainingToVolumeField_button.Location = new System.Drawing.Point(268, 273);
            this.AccAvgPrice_SendRemainingToVolumeField_button.Name = "AccAvgPrice_SendRemainingToVolumeField_button";
            this.AccAvgPrice_SendRemainingToVolumeField_button.Size = new System.Drawing.Size(60, 23);
            this.AccAvgPrice_SendRemainingToVolumeField_button.TabIndex = 30;
            this.AccAvgPrice_SendRemainingToVolumeField_button.Text = "Set vol";
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_SendRemainingToVolumeField_button, "Clicking will copy the \"Remaining to deal\" value to the volume field on the main " +
        "app.   \r\nOnly works if the \"Size of deal\" drop down menu is set to \"Crypto\".");
            this.AccAvgPrice_SendRemainingToVolumeField_button.UseVisualStyleBackColor = true;
            this.AccAvgPrice_SendRemainingToVolumeField_button.Click += new System.EventHandler(this.AccAvgPrice_SendRemainingToVolumeField_button_Click);
            // 
            // AccAvgPrice_DealComment_TextBox
            // 
            this.AccAvgPrice_DealComment_TextBox.Location = new System.Drawing.Point(230, 341);
            this.AccAvgPrice_DealComment_TextBox.Name = "AccAvgPrice_DealComment_TextBox";
            this.AccAvgPrice_DealComment_TextBox.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_DealComment_TextBox.TabIndex = 36;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_DealComment_TextBox, "Enter here what size you need to do on the order book.");
            // 
            // AccAvgPrice_EndDay_Label
            // 
            this.AccAvgPrice_EndDay_Label.AutoSize = true;
            this.AccAvgPrice_EndDay_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_EndDay_Label.Location = new System.Drawing.Point(227, 147);
            this.AccAvgPrice_EndDay_Label.Name = "AccAvgPrice_EndDay_Label";
            this.AccAvgPrice_EndDay_Label.Size = new System.Drawing.Size(0, 13);
            this.AccAvgPrice_EndDay_Label.TabIndex = 39;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_EndDay_Label, "Double click to set this to now + 1 day");
            // 
            // AccAvgPrice_StartDay_Label
            // 
            this.AccAvgPrice_StartDay_Label.AutoSize = true;
            this.AccAvgPrice_StartDay_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_StartDay_Label.Location = new System.Drawing.Point(227, 121);
            this.AccAvgPrice_StartDay_Label.Name = "AccAvgPrice_StartDay_Label";
            this.AccAvgPrice_StartDay_Label.Size = new System.Drawing.Size(0, 13);
            this.AccAvgPrice_StartDay_Label.TabIndex = 38;
            this.AccAvgPrice_TT.SetToolTip(this.AccAvgPrice_StartDay_Label, "Double click to set this to now");
            // 
            // AccAvgPrice_TotalCrypto_TextBox
            // 
            this.AccAvgPrice_TotalCrypto_TextBox.Location = new System.Drawing.Point(112, 315);
            this.AccAvgPrice_TotalCrypto_TextBox.Name = "AccAvgPrice_TotalCrypto_TextBox";
            this.AccAvgPrice_TotalCrypto_TextBox.ReadOnly = true;
            this.AccAvgPrice_TotalCrypto_TextBox.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_TotalCrypto_TextBox.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(13, 318);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Total crypto dealt:";
            // 
            // AccAvgPrice_TotalFiat_TextBox
            // 
            this.AccAvgPrice_TotalFiat_TextBox.Location = new System.Drawing.Point(112, 341);
            this.AccAvgPrice_TotalFiat_TextBox.Name = "AccAvgPrice_TotalFiat_TextBox";
            this.AccAvgPrice_TotalFiat_TextBox.ReadOnly = true;
            this.AccAvgPrice_TotalFiat_TextBox.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_TotalFiat_TextBox.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(13, 344);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Total fiat dealt:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(13, 252);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Size of deal:";
            // 
            // AccAvgPrice_RemainingToDeal_TextBox
            // 
            this.AccAvgPrice_RemainingToDeal_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.AccAvgPrice_RemainingToDeal_TextBox.ForeColor = System.Drawing.Color.Black;
            this.AccAvgPrice_RemainingToDeal_TextBox.Location = new System.Drawing.Point(112, 275);
            this.AccAvgPrice_RemainingToDeal_TextBox.Name = "AccAvgPrice_RemainingToDeal_TextBox";
            this.AccAvgPrice_RemainingToDeal_TextBox.ReadOnly = true;
            this.AccAvgPrice_RemainingToDeal_TextBox.Size = new System.Drawing.Size(106, 20);
            this.AccAvgPrice_RemainingToDeal_TextBox.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(13, 278);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Remaining to deal:";
            // 
            // AccAvgPrice_RemaingToDealCurrency_Label
            // 
            this.AccAvgPrice_RemaingToDealCurrency_Label.AutoSize = true;
            this.AccAvgPrice_RemaingToDealCurrency_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_RemaingToDealCurrency_Label.Location = new System.Drawing.Point(227, 278);
            this.AccAvgPrice_RemaingToDealCurrency_Label.Name = "AccAvgPrice_RemaingToDealCurrency_Label";
            this.AccAvgPrice_RemaingToDealCurrency_Label.Size = new System.Drawing.Size(28, 13);
            this.AccAvgPrice_RemaingToDealCurrency_Label.TabIndex = 29;
            this.AccAvgPrice_RemaingToDealCurrency_Label.Text = "BTC";
            // 
            // AccAvgPrice_FiatAUD_button
            // 
            this.AccAvgPrice_FiatAUD_button.BackColor = System.Drawing.Color.White;
            this.AccAvgPrice_FiatAUD_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AccAvgPrice_FiatAUD_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AccAvgPrice_FiatAUD_button.Location = new System.Drawing.Point(113, 39);
            this.AccAvgPrice_FiatAUD_button.Name = "AccAvgPrice_FiatAUD_button";
            this.AccAvgPrice_FiatAUD_button.Size = new System.Drawing.Size(51, 23);
            this.AccAvgPrice_FiatAUD_button.TabIndex = 31;
            this.AccAvgPrice_FiatAUD_button.Text = "AUD";
            this.AccAvgPrice_FiatAUD_button.UseVisualStyleBackColor = false;
            this.AccAvgPrice_FiatAUD_button.Click += new System.EventHandler(this.AccAvgPrice_Fiat_button_click);
            // 
            // AccAvgPrice_FiatUSD_button
            // 
            this.AccAvgPrice_FiatUSD_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AccAvgPrice_FiatUSD_button.Location = new System.Drawing.Point(167, 39);
            this.AccAvgPrice_FiatUSD_button.Name = "AccAvgPrice_FiatUSD_button";
            this.AccAvgPrice_FiatUSD_button.Size = new System.Drawing.Size(51, 23);
            this.AccAvgPrice_FiatUSD_button.TabIndex = 32;
            this.AccAvgPrice_FiatUSD_button.Text = "USD";
            this.AccAvgPrice_FiatUSD_button.UseVisualStyleBackColor = true;
            this.AccAvgPrice_FiatUSD_button.Click += new System.EventHandler(this.AccAvgPrice_Fiat_button_click);
            // 
            // AccAvgPrice_FiatNZD_button
            // 
            this.AccAvgPrice_FiatNZD_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AccAvgPrice_FiatNZD_button.Location = new System.Drawing.Point(113, 64);
            this.AccAvgPrice_FiatNZD_button.Name = "AccAvgPrice_FiatNZD_button";
            this.AccAvgPrice_FiatNZD_button.Size = new System.Drawing.Size(51, 23);
            this.AccAvgPrice_FiatNZD_button.TabIndex = 33;
            this.AccAvgPrice_FiatNZD_button.Text = "NZD";
            this.AccAvgPrice_FiatNZD_button.UseVisualStyleBackColor = true;
            this.AccAvgPrice_FiatNZD_button.Click += new System.EventHandler(this.AccAvgPrice_Fiat_button_click);
            // 
            // AccAvgPrice_FiatSGD_button
            // 
            this.AccAvgPrice_FiatSGD_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AccAvgPrice_FiatSGD_button.Location = new System.Drawing.Point(167, 64);
            this.AccAvgPrice_FiatSGD_button.Name = "AccAvgPrice_FiatSGD_button";
            this.AccAvgPrice_FiatSGD_button.Size = new System.Drawing.Size(51, 23);
            this.AccAvgPrice_FiatSGD_button.TabIndex = 34;
            this.AccAvgPrice_FiatSGD_button.Text = "SGD";
            this.AccAvgPrice_FiatSGD_button.UseVisualStyleBackColor = true;
            this.AccAvgPrice_FiatSGD_button.Click += new System.EventHandler(this.AccAvgPrice_Fiat_button_click);
            // 
            // AccAvgPrice_DealComment_Label
            // 
            this.AccAvgPrice_DealComment_Label.AutoSize = true;
            this.AccAvgPrice_DealComment_Label.BackColor = System.Drawing.Color.Transparent;
            this.AccAvgPrice_DealComment_Label.Location = new System.Drawing.Point(227, 318);
            this.AccAvgPrice_DealComment_Label.Name = "AccAvgPrice_DealComment_Label";
            this.AccAvgPrice_DealComment_Label.Size = new System.Drawing.Size(59, 13);
            this.AccAvgPrice_DealComment_Label.TabIndex = 35;
            this.AccAvgPrice_DealComment_Label.Text = "Comments:";
            // 
            // AccAvgPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::IRTicker.Properties.Resources.IR_Eagel_Transparent___small_faded2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(344, 371);
            this.Controls.Add(this.AccAvgPrice_EndDay_Label);
            this.Controls.Add(this.AccAvgPrice_StartDay_Label);
            this.Controls.Add(this.AccAvgPrice_DealComment_TextBox);
            this.Controls.Add(this.AccAvgPrice_DealComment_Label);
            this.Controls.Add(this.AccAvgPrice_FiatSGD_button);
            this.Controls.Add(this.AccAvgPrice_FiatNZD_button);
            this.Controls.Add(this.AccAvgPrice_FiatUSD_button);
            this.Controls.Add(this.AccAvgPrice_FiatAUD_button);
            this.Controls.Add(this.AccAvgPrice_SendRemainingToVolumeField_button);
            this.Controls.Add(this.AccAvgPrice_RemaingToDealCurrency_Label);
            this.Controls.Add(this.AccAvgPrice_RemainingToDeal_TextBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.AccAvgPrice_DealSizeCurrency_ComboBox);
            this.Controls.Add(this.AccAvgPrice_DealSize_TextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AccAvgPrice_TotalFiat_TextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.AccAvgPrice_TotalCrypto_TextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.AccAvgPrice_BuySell_ComboBox);
            this.Controls.Add(this.AccAvgPrice_AutoUpdate_CheckBox);
            this.Controls.Add(this.AccAvgPrice_CopyAvg_Button);
            this.Controls.Add(this.AccAvgPrice_Result_TextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AccAvgPrice_End_Label);
            this.Controls.Add(this.AccAvgPrice_Start_Label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AccAvgPrice_End_DTPicker);
            this.Controls.Add(this.AccAvgPrice_Status_Label);
            this.Controls.Add(this.AccAvgPrice_Go_Button);
            this.Controls.Add(this.AccAvgPrice_Start_DTPicker);
            this.Controls.Add(this.AccAvgPrice_Crypto_ComboBox);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(360, 410);
            this.MinimumSize = new System.Drawing.Size(360, 410);
            this.Name = "AccAvgPrice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Average price calculator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AccAvgPrice_FormClosing);
            this.Load += new System.EventHandler(this.AccAvgPrice_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AccAvgPrice_Go_Button;
        private System.Windows.Forms.Label AccAvgPrice_Status_Label;
        private System.Windows.Forms.DateTimePicker AccAvgPrice_End_DTPicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AccAvgPrice_End_Label;
        private System.Windows.Forms.Label AccAvgPrice_Start_Label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox AccAvgPrice_Result_TextBox;
        private System.Windows.Forms.Button AccAvgPrice_CopyAvg_Button;
        private System.Windows.Forms.CheckBox AccAvgPrice_AutoUpdate_CheckBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox AccAvgPrice_BuySell_ComboBox;
        private System.Windows.Forms.ToolTip AccAvgPrice_TT;
        private System.Windows.Forms.TextBox AccAvgPrice_TotalCrypto_TextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox AccAvgPrice_TotalFiat_TextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox AccAvgPrice_DealSize_TextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox AccAvgPrice_DealSizeCurrency_ComboBox;
        private System.Windows.Forms.TextBox AccAvgPrice_RemainingToDeal_TextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label AccAvgPrice_RemaingToDealCurrency_Label;
        private System.Windows.Forms.Button AccAvgPrice_SendRemainingToVolumeField_button;
        private System.Windows.Forms.Button AccAvgPrice_FiatAUD_button;
        private System.Windows.Forms.Button AccAvgPrice_FiatUSD_button;
        private System.Windows.Forms.Button AccAvgPrice_FiatNZD_button;
        private System.Windows.Forms.Button AccAvgPrice_FiatSGD_button;
        private System.Windows.Forms.Label AccAvgPrice_DealComment_Label;
        private System.Windows.Forms.TextBox AccAvgPrice_DealComment_TextBox;
        private System.Windows.Forms.Label AccAvgPrice_EndDay_Label;
        private System.Windows.Forms.Label AccAvgPrice_StartDay_Label;
        public System.Windows.Forms.DateTimePicker AccAvgPrice_Start_DTPicker;
        public System.Windows.Forms.ComboBox AccAvgPrice_Crypto_ComboBox;
    }
}