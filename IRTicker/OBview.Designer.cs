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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OBview));
            this.BidsTextBox = new System.Windows.Forms.RichTextBox();
            this.OffersTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ETHOffersTextBox = new System.Windows.Forms.RichTextBox();
            this.ETHBidsTextBox = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OBProgress = new System.Windows.Forms.Label();
            this.BidTopGuid_InputBox = new System.Windows.Forms.RichTextBox();
            this.OfferTopGuid_InputBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BidsTextBox
            // 
            this.BidsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BidsTextBox.Location = new System.Drawing.Point(80, 39);
            this.BidsTextBox.Name = "BidsTextBox";
            this.BidsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.BidsTextBox.Size = new System.Drawing.Size(251, 247);
            this.BidsTextBox.TabIndex = 0;
            this.BidsTextBox.Text = "1234.54  |  0.01570000  |  0.01570000\n6545.59  |  1.24600000  | 1.2557000\n6545.64" +
    "  |  40.326\n6987.58  |  3.31\n9871.45  |  0.13354978\n123\n123\n132\n123\n123\n123";
            this.BidsTextBox.WordWrap = false;
            // 
            // OffersTextBox
            // 
            this.OffersTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OffersTextBox.Location = new System.Drawing.Point(374, 39);
            this.OffersTextBox.Name = "OffersTextBox";
            this.OffersTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.OffersTextBox.Size = new System.Drawing.Size(251, 247);
            this.OffersTextBox.TabIndex = 1;
            this.OffersTextBox.Text = "";
            this.OffersTextBox.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bids";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(337, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Offers";
            // 
            // ETHOffersTextBox
            // 
            this.ETHOffersTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ETHOffersTextBox.Location = new System.Drawing.Point(374, 330);
            this.ETHOffersTextBox.Name = "ETHOffersTextBox";
            this.ETHOffersTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ETHOffersTextBox.Size = new System.Drawing.Size(251, 192);
            this.ETHOffersTextBox.TabIndex = 5;
            this.ETHOffersTextBox.Text = "";
            // 
            // ETHBidsTextBox
            // 
            this.ETHBidsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ETHBidsTextBox.Location = new System.Drawing.Point(81, 330);
            this.ETHBidsTextBox.Name = "ETHBidsTextBox";
            this.ETHBidsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ETHBidsTextBox.Size = new System.Drawing.Size(251, 192);
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
            // OBProgress
            // 
            this.OBProgress.AutoSize = true;
            this.OBProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OBProgress.Location = new System.Drawing.Point(17, 219);
            this.OBProgress.Name = "OBProgress";
            this.OBProgress.Size = new System.Drawing.Size(32, 31);
            this.OBProgress.TabIndex = 8;
            this.OBProgress.Text = "--";
            // 
            // BidTopGuid_InputBox
            // 
            this.BidTopGuid_InputBox.Location = new System.Drawing.Point(80, 12);
            this.BidTopGuid_InputBox.Name = "BidTopGuid_InputBox";
            this.BidTopGuid_InputBox.Size = new System.Drawing.Size(251, 21);
            this.BidTopGuid_InputBox.TabIndex = 9;
            this.BidTopGuid_InputBox.Text = "";
            // 
            // OfferTopGuid_InputBox
            // 
            this.OfferTopGuid_InputBox.Location = new System.Drawing.Point(374, 12);
            this.OfferTopGuid_InputBox.Name = "OfferTopGuid_InputBox";
            this.OfferTopGuid_InputBox.Size = new System.Drawing.Size(251, 21);
            this.OfferTopGuid_InputBox.TabIndex = 10;
            this.OfferTopGuid_InputBox.Text = "";
            // 
            // OBview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 542);
            this.Controls.Add(this.OfferTopGuid_InputBox);
            this.Controls.Add(this.BidTopGuid_InputBox);
            this.Controls.Add(this.OBProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ETHOffersTextBox);
            this.Controls.Add(this.ETHBidsTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OffersTextBox);
            this.Controls.Add(this.BidsTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
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
        public System.Windows.Forms.Label OBProgress;
        private System.Windows.Forms.RichTextBox BidTopGuid_InputBox;
        private System.Windows.Forms.RichTextBox OfferTopGuid_InputBox;
    }
}