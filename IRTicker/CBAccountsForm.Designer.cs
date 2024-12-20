namespace IRTicker {
    partial class CBAccountsForm {
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
            this.CB_trade_controls_panel = new System.Windows.Forms.Panel();
            this.CB_pair_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_order_type_listbox = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.CB_orderbook_panel = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView2 = new System.Windows.Forms.ListView();
            this.CB_trade_controls_panel.SuspendLayout();
            this.CB_orderbook_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_trade_controls_panel
            // 
            this.CB_trade_controls_panel.BackColor = System.Drawing.Color.YellowGreen;
            this.CB_trade_controls_panel.Controls.Add(this.button1);
            this.CB_trade_controls_panel.Controls.Add(this.label3);
            this.CB_trade_controls_panel.Controls.Add(this.textBox2);
            this.CB_trade_controls_panel.Controls.Add(this.label2);
            this.CB_trade_controls_panel.Controls.Add(this.textBox1);
            this.CB_trade_controls_panel.Controls.Add(this.CB_order_type_listbox);
            this.CB_trade_controls_panel.Controls.Add(this.label1);
            this.CB_trade_controls_panel.Controls.Add(this.CB_pair_comboBox);
            this.CB_trade_controls_panel.Location = new System.Drawing.Point(0, 0);
            this.CB_trade_controls_panel.Name = "CB_trade_controls_panel";
            this.CB_trade_controls_panel.Size = new System.Drawing.Size(210, 449);
            this.CB_trade_controls_panel.TabIndex = 0;
            // 
            // CB_pair_comboBox
            // 
            this.CB_pair_comboBox.FormattingEnabled = true;
            this.CB_pair_comboBox.Location = new System.Drawing.Point(39, 27);
            this.CB_pair_comboBox.Name = "CB_pair_comboBox";
            this.CB_pair_comboBox.Size = new System.Drawing.Size(153, 21);
            this.CB_pair_comboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pair:";
            // 
            // CB_order_type_listbox
            // 
            this.CB_order_type_listbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_order_type_listbox.FormattingEnabled = true;
            this.CB_order_type_listbox.ItemHeight = 20;
            this.CB_order_type_listbox.Items.AddRange(new object[] {
            "Limit",
            "Market",
            "Market baiter"});
            this.CB_order_type_listbox.Location = new System.Drawing.Point(39, 54);
            this.CB_order_type_listbox.Name = "CB_order_type_listbox";
            this.CB_order_type_listbox.Size = new System.Drawing.Size(153, 64);
            this.CB_order_type_listbox.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(39, 148);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Volume:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Price:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(39, 202);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 251);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Place order";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CB_orderbook_panel
            // 
            this.CB_orderbook_panel.BackColor = System.Drawing.Color.Black;
            this.CB_orderbook_panel.Controls.Add(this.listView2);
            this.CB_orderbook_panel.Controls.Add(this.listView1);
            this.CB_orderbook_panel.Location = new System.Drawing.Point(210, 0);
            this.CB_orderbook_panel.Name = "CB_orderbook_panel";
            this.CB_orderbook_panel.Size = new System.Drawing.Size(200, 449);
            this.CB_orderbook_panel.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.Gray;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(200, 201);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.Color.Gray;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, 226);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(200, 223);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // CBAccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 449);
            this.Controls.Add(this.CB_orderbook_panel);
            this.Controls.Add(this.CB_trade_controls_panel);
            this.Name = "CBAccountsForm";
            this.Text = "CBAccountsForm";
            this.CB_trade_controls_panel.ResumeLayout(false);
            this.CB_trade_controls_panel.PerformLayout();
            this.CB_orderbook_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel CB_trade_controls_panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CB_pair_comboBox;
        private System.Windows.Forms.ListBox CB_order_type_listbox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel CB_orderbook_panel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
    }
}