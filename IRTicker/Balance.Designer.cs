
namespace IRTicker
{
    partial class Balance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Balance));
            this.Platform_comboBox = new System.Windows.Forms.ComboBox();
            this.BalReload_button = new System.Windows.Forms.Button();
            this.Legend_label = new System.Windows.Forms.Label();
            this.Legend_TT = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Platform_comboBox
            // 
            this.Platform_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Platform_comboBox.FormattingEnabled = true;
            this.Platform_comboBox.Items.AddRange(new object[] {
            "Independent Reserve",
            "B2C2",
            "TrigonX",
            "Coinbase",
            "IROTC MetaMask"});
            this.Platform_comboBox.Location = new System.Drawing.Point(12, 12);
            this.Platform_comboBox.Name = "Platform_comboBox";
            this.Platform_comboBox.Size = new System.Drawing.Size(212, 21);
            this.Platform_comboBox.TabIndex = 0;
            this.Platform_comboBox.SelectedIndexChanged += new System.EventHandler(this.Platform_comboBox_SelectedIndexChanged);
            // 
            // BalReload_button
            // 
            this.BalReload_button.Location = new System.Drawing.Point(272, 12);
            this.BalReload_button.Name = "BalReload_button";
            this.BalReload_button.Size = new System.Drawing.Size(115, 23);
            this.BalReload_button.TabIndex = 1;
            this.BalReload_button.Text = "Reload";
            this.BalReload_button.UseVisualStyleBackColor = true;
            this.BalReload_button.Click += new System.EventHandler(this.BalReload_button_Click);
            // 
            // Legend_label
            // 
            this.Legend_label.AutoSize = true;
            this.Legend_label.Location = new System.Drawing.Point(478, 19);
            this.Legend_label.Name = "Legend_label";
            this.Legend_label.Size = new System.Drawing.Size(142, 13);
            this.Legend_label.TabIndex = 2;
            this.Legend_label.Text = "Hover here for colour legend";
            this.Legend_TT.SetToolTip(this.Legend_label, "Green: perfect balance\r\nBlack: imperfect balance (close enough)\r\nRed: out of bala" +
        "nce, needs investigation\r\nPurple: out of balance, slush needs topping up?");
            // 
            // Balance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(734, 761);
            this.Controls.Add(this.Legend_label);
            this.Controls.Add(this.BalReload_button);
            this.Controls.Add(this.Platform_comboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Balance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Balance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Platform_comboBox;
        private System.Windows.Forms.Button BalReload_button;
        private System.Windows.Forms.Label Legend_label;
        private System.Windows.Forms.ToolTip Legend_TT;
    }
}