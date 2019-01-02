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
            this.SuspendLayout();
            // 
            // BidsTextBox
            // 
            this.BidsTextBox.Location = new System.Drawing.Point(80, 55);
            this.BidsTextBox.Name = "BidsTextBox";
            this.BidsTextBox.Size = new System.Drawing.Size(119, 358);
            this.BidsTextBox.TabIndex = 0;
            this.BidsTextBox.Text = "";
            // 
            // OffersTextBox
            // 
            this.OffersTextBox.Location = new System.Drawing.Point(228, 55);
            this.OffersTextBox.Name = "OffersTextBox";
            this.OffersTextBox.Size = new System.Drawing.Size(154, 358);
            this.OffersTextBox.TabIndex = 1;
            this.OffersTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bids";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Offers";
            // 
            // OBview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 450);
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
    }
}