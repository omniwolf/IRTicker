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
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CB_order_type_listbox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_pair_comboBox = new System.Windows.Forms.ComboBox();
            this.CB_orderbook_panel = new System.Windows.Forms.Panel();
            this.CB_bids_listview = new System.Windows.Forms.ListView();
            this.CB_asks_listview = new System.Windows.Forms.ListView();
            this.Price_asks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Size_asks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Bids_price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Bids_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_open_orders_listview = new System.Windows.Forms.ListView();
            this.CB_open_orders_price_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_open_orders_size_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_open_orders_created_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_open_orders_remaining_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_closed_orders_listview = new System.Windows.Forms.ListView();
            this.CB_closed_orders_settled_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_closed_orders_price_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_closed_orders_volume_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_closed_orders_value_col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CB_trade_controls_panel.SuspendLayout();
            this.CB_orderbook_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_trade_controls_panel
            // 
            this.CB_trade_controls_panel.BackColor = System.Drawing.Color.YellowGreen;
            this.CB_trade_controls_panel.Controls.Add(this.label5);
            this.CB_trade_controls_panel.Controls.Add(this.label4);
            this.CB_trade_controls_panel.Controls.Add(this.CB_closed_orders_listview);
            this.CB_trade_controls_panel.Controls.Add(this.CB_open_orders_listview);
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
            this.CB_trade_controls_panel.Size = new System.Drawing.Size(311, 449);
            this.CB_trade_controls_panel.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(219, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 48);
            this.button1.TabIndex = 7;
            this.button1.Text = "Place order";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Price:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(92, 133);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Volume:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(92, 105);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
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
            this.CB_order_type_listbox.Location = new System.Drawing.Point(92, 35);
            this.CB_order_type_listbox.Name = "CB_order_type_listbox";
            this.CB_order_type_listbox.Size = new System.Drawing.Size(153, 64);
            this.CB_order_type_listbox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pair:";
            // 
            // CB_pair_comboBox
            // 
            this.CB_pair_comboBox.FormattingEnabled = true;
            this.CB_pair_comboBox.Location = new System.Drawing.Point(92, 8);
            this.CB_pair_comboBox.Name = "CB_pair_comboBox";
            this.CB_pair_comboBox.Size = new System.Drawing.Size(153, 21);
            this.CB_pair_comboBox.TabIndex = 0;
            // 
            // CB_orderbook_panel
            // 
            this.CB_orderbook_panel.BackColor = System.Drawing.Color.Black;
            this.CB_orderbook_panel.Controls.Add(this.CB_bids_listview);
            this.CB_orderbook_panel.Controls.Add(this.CB_asks_listview);
            this.CB_orderbook_panel.Location = new System.Drawing.Point(310, 0);
            this.CB_orderbook_panel.Name = "CB_orderbook_panel";
            this.CB_orderbook_panel.Size = new System.Drawing.Size(200, 449);
            this.CB_orderbook_panel.TabIndex = 1;
            // 
            // CB_bids_listview
            // 
            this.CB_bids_listview.BackColor = System.Drawing.Color.Black;
            this.CB_bids_listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Bids_price,
            this.Bids_size});
            this.CB_bids_listview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_bids_listview.ForeColor = System.Drawing.Color.LightGreen;
            this.CB_bids_listview.HideSelection = false;
            this.CB_bids_listview.Location = new System.Drawing.Point(0, 233);
            this.CB_bids_listview.Name = "CB_bids_listview";
            this.CB_bids_listview.ShowGroups = false;
            this.CB_bids_listview.Size = new System.Drawing.Size(200, 216);
            this.CB_bids_listview.TabIndex = 0;
            this.CB_bids_listview.UseCompatibleStateImageBehavior = false;
            this.CB_bids_listview.View = System.Windows.Forms.View.Details;
            // 
            // CB_asks_listview
            // 
            this.CB_asks_listview.BackColor = System.Drawing.Color.Black;
            this.CB_asks_listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Price_asks,
            this.Size_asks});
            this.CB_asks_listview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_asks_listview.ForeColor = System.Drawing.Color.Red;
            this.CB_asks_listview.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CB_asks_listview.Location = new System.Drawing.Point(0, 0);
            this.CB_asks_listview.Name = "CB_asks_listview";
            this.CB_asks_listview.ShowGroups = false;
            this.CB_asks_listview.Size = new System.Drawing.Size(200, 222);
            this.CB_asks_listview.TabIndex = 0;
            this.CB_asks_listview.UseCompatibleStateImageBehavior = false;
            this.CB_asks_listview.View = System.Windows.Forms.View.Details;
            // 
            // Price_asks
            // 
            this.Price_asks.Text = "Price";
            this.Price_asks.Width = 90;
            // 
            // Size_asks
            // 
            this.Size_asks.Text = "Volume";
            this.Size_asks.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Size_asks.Width = 90;
            // 
            // Bids_price
            // 
            this.Bids_price.Text = "Price";
            this.Bids_price.Width = 90;
            // 
            // Bids_size
            // 
            this.Bids_size.Text = "Volume";
            this.Bids_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Bids_size.Width = 90;
            // 
            // CB_open_orders_listview
            // 
            this.CB_open_orders_listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CB_open_orders_created_col,
            this.CB_open_orders_price_col,
            this.CB_open_orders_size_col,
            this.CB_open_orders_remaining_col});
            this.CB_open_orders_listview.GridLines = true;
            this.CB_open_orders_listview.HideSelection = false;
            this.CB_open_orders_listview.Location = new System.Drawing.Point(5, 183);
            this.CB_open_orders_listview.Name = "CB_open_orders_listview";
            this.CB_open_orders_listview.ShowGroups = false;
            this.CB_open_orders_listview.Size = new System.Drawing.Size(299, 113);
            this.CB_open_orders_listview.TabIndex = 8;
            this.CB_open_orders_listview.UseCompatibleStateImageBehavior = false;
            this.CB_open_orders_listview.View = System.Windows.Forms.View.Details;
            this.CB_open_orders_listview.DoubleClick += new System.EventHandler(this.CB_open_orders_listview_DoubleClick);
            // 
            // CB_open_orders_price_col
            // 
            this.CB_open_orders_price_col.Text = "Price";
            // 
            // CB_open_orders_size_col
            // 
            this.CB_open_orders_size_col.Text = "Volume";
            // 
            // CB_open_orders_created_col
            // 
            this.CB_open_orders_created_col.Text = "Created";
            this.CB_open_orders_created_col.Width = 80;
            // 
            // CB_open_orders_remaining_col
            // 
            this.CB_open_orders_remaining_col.Text = "Remaining";
            this.CB_open_orders_remaining_col.Width = 80;
            // 
            // CB_closed_orders_listview
            // 
            this.CB_closed_orders_listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CB_closed_orders_settled_col,
            this.CB_closed_orders_price_col,
            this.CB_closed_orders_volume_col,
            this.CB_closed_orders_value_col});
            this.CB_closed_orders_listview.GridLines = true;
            this.CB_closed_orders_listview.HideSelection = false;
            this.CB_closed_orders_listview.Location = new System.Drawing.Point(5, 333);
            this.CB_closed_orders_listview.Name = "CB_closed_orders_listview";
            this.CB_closed_orders_listview.ShowGroups = false;
            this.CB_closed_orders_listview.Size = new System.Drawing.Size(299, 113);
            this.CB_closed_orders_listview.TabIndex = 9;
            this.CB_closed_orders_listview.UseCompatibleStateImageBehavior = false;
            this.CB_closed_orders_listview.View = System.Windows.Forms.View.Details;
            // 
            // CB_closed_orders_settled_col
            // 
            this.CB_closed_orders_settled_col.Text = "Settled";
            this.CB_closed_orders_settled_col.Width = 80;
            // 
            // CB_closed_orders_price_col
            // 
            this.CB_closed_orders_price_col.Text = "Price";
            // 
            // CB_closed_orders_volume_col
            // 
            this.CB_closed_orders_volume_col.Text = "Volume";
            // 
            // CB_closed_orders_value_col
            // 
            this.CB_closed_orders_value_col.Text = "Value";
            this.CB_closed_orders_value_col.Width = 80;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Open orders:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Closed orders:";
            // 
            // CBAccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 449);
            this.Controls.Add(this.CB_orderbook_panel);
            this.Controls.Add(this.CB_trade_controls_panel);
            this.Name = "CBAccountsForm";
            this.Text = "CBAccountsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CBAccountsForm_FormClosing);
            this.Load += new System.EventHandler(this.CBAccountsForm_Load);
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
        private System.Windows.Forms.ListView CB_asks_listview;
        private System.Windows.Forms.ListView CB_bids_listview;
        private System.Windows.Forms.ColumnHeader Price_asks;
        private System.Windows.Forms.ColumnHeader Size_asks;
        private System.Windows.Forms.ColumnHeader Bids_price;
        private System.Windows.Forms.ColumnHeader Bids_size;
        private System.Windows.Forms.ListView CB_open_orders_listview;
        private System.Windows.Forms.ColumnHeader CB_open_orders_price_col;
        private System.Windows.Forms.ColumnHeader CB_open_orders_size_col;
        private System.Windows.Forms.ColumnHeader CB_open_orders_created_col;
        private System.Windows.Forms.ColumnHeader CB_open_orders_remaining_col;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView CB_closed_orders_listview;
        private System.Windows.Forms.ColumnHeader CB_closed_orders_settled_col;
        private System.Windows.Forms.ColumnHeader CB_closed_orders_price_col;
        private System.Windows.Forms.ColumnHeader CB_closed_orders_volume_col;
        private System.Windows.Forms.ColumnHeader CB_closed_orders_value_col;
        private System.Windows.Forms.Label label5;
    }
}