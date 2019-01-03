namespace IRTicker
{
    partial class OBview
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
            this.BidsTextBox = new System.Windows.Forms.RichTextBox();
            this.OffersTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ETHOffersTextBox = new System.Windows.Forms.RichTextBox();
            this.ETHBidsTextBox = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BidsTextBox
            // 
            this.BidsTextBox.Location = new System.Drawing.Point(80, 39);
            this.BidsTextBox.Name = "BidsTextBox";
            this.BidsTextBox.Size = new System.Drawing.Size(119, 192);
            this.BidsTextBox.TabIndex = 0;
            this.BidsTextBox.Text = "";
            // 
            // OffersTextBox
            // 
            this.OffersTextBox.Location = new System.Drawing.Point(242, 39);
            this.OffersTextBox.Name = "OffersTextBox";
            this.OffersTextBox.Size = new System.Drawing.Size(119, 192);
            this.OffersTextBox.TabIndex = 1;
            this.OffersTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bids";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Offers";
            // 
            // ETHOffersTextBox
            // 
            this.ETHOffersTextBox.Location = new System.Drawing.Point(242, 246);
            this.ETHOffersTextBox.Name = "ETHOffersTextBox";
            this.ETHOffersTextBox.Size = new System.Drawing.Size(119, 192);
            this.ETHOffersTextBox.TabIndex = 5;
            this.ETHOffersTextBox.Text = "";
            // 
            // ETHBidsTextBox
            // 
            this.ETHBidsTextBox.Location = new System.Drawing.Point(80, 246);
            this.ETHBidsTextBox.Name = "ETHBidsTextBox";
            this.ETHBidsTextBox.Size = new System.Drawing.Size(119, 192);
            this.ETHBidsTextBox.TabIndex = 4;
            this.ETHBidsTextBox.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "XBT-AUD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 335);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "ETH-AUD";
            // 
            // OBview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ETHOffersTextBox);
            this.Controls.Add(this.ETHBidsTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OffersTextBox);
            this.Controls.Add(this.BidsTextBox);
            this.Name = "OBview";
            this.Text = "OBview";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox BidsTextBox;
        private System.Windows.Forms.RichTextBox OffersTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox ETHOffersTextBox;
        private System.Windows.Forms.RichTextBox ETHBidsTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}