namespace IRTicker {
    partial class Help {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
            this.HelpTitle_Label = new System.Windows.Forms.Label();
            this.HelpBody_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HelpTitle_Label
            // 
            this.HelpTitle_Label.AutoSize = true;
            this.HelpTitle_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpTitle_Label.Location = new System.Drawing.Point(32, 22);
            this.HelpTitle_Label.Name = "HelpTitle_Label";
            this.HelpTitle_Label.Size = new System.Drawing.Size(389, 46);
            this.HelpTitle_Label.TabIndex = 0;
            this.HelpTitle_Label.Text = "Some tips on the UI";
            // 
            // HelpBody_Label
            // 
            this.HelpBody_Label.AutoSize = true;
            this.HelpBody_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpBody_Label.Location = new System.Drawing.Point(40, 103);
            this.HelpBody_Label.Name = "HelpBody_Label";
            this.HelpBody_Label.Size = new System.Drawing.Size(747, 340);
            this.HelpBody_Label.TabIndex = 1;
            this.HelpBody_Label.Text = resources.GetString("HelpBody_Label.Text");
            this.HelpBody_Label.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HelpBody_Label_MouseDoubleClick);
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 471);
            this.Controls.Add(this.HelpBody_Label);
            this.Controls.Add(this.HelpTitle_Label);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(816, 510);
            this.MinimumSize = new System.Drawing.Size(816, 510);
            this.Name = "Help";
            this.Text = "Help";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Help_FormClosing);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Help_MouseDoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HelpTitle_Label;
        private System.Windows.Forms.Label HelpBody_Label;
    }
}