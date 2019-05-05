namespace IRTicker {
    partial class SpreadGraph {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.SpreadChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.SpreadChart)).BeginInit();
            this.SuspendLayout();
            // 
            // SpreadChart
            // 
            chartArea1.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.Title = "Spread";
            chartArea1.Name = "ChartArea1";
            this.SpreadChart.ChartAreas.Add(chartArea1);
            this.SpreadChart.Location = new System.Drawing.Point(0, -1);
            this.SpreadChart.Name = "SpreadChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.LabelFormat = "### ##0.##";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.SpreadChart.Series.Add(series1);
            this.SpreadChart.Size = new System.Drawing.Size(804, 455);
            this.SpreadChart.TabIndex = 0;
            this.SpreadChart.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "Spread";
            this.SpreadChart.Titles.Add(title1);
            // 
            // SpreadGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SpreadChart);
            this.Name = "SpreadGraph";
            this.Text = "SpreadGraph";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadGraph_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.SpreadChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart SpreadChart;
    }
}