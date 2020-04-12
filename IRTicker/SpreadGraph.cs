using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace IRTicker {
    public partial class SpreadGraph : Form {

        private DCE Exchange;
        private string pair;
        private IRTicker IRT;

        public SpreadGraph(DCE _Exchange, string _pair, IRTicker _IRT) {
            InitializeComponent();

            Exchange = _Exchange;
            pair = _pair;
            IRT = _IRT;
            SpreadChart.Titles[0].Text = this.Text = Exchange.FriendlyName + " " + pair + " spread history";            
            Redraw();
        }

        public void Redraw() {

            if (!Exchange.GetSpreadHistory().ContainsKey(pair)) return;  // early on it's possible there's no spread graph yet
            SpreadChart.Series["Series1"].Points.Clear();

            foreach (DataPoint dp in Exchange.GetSpreadHistory()[pair]) {
                SpreadChart.Series["Series1"].Points.Add(dp);
            }

            // Here we set the X axis formatting
            if (SpreadChart.Series[0].Points.Count > 0) {
                TimeSpan tempTS = DateTime.FromOADate(SpreadChart.Series[0].Points.Last().XValue) - DateTime.FromOADate(SpreadChart.Series[0].Points.First().XValue);

                // all graphs should be graphing almost exactly the same time span, so if one (voltage in this case) graph is a certain timespan, they all should be.  no point in testing each one..
                if (tempTS.TotalMinutes > 1440) {  // greater than 24 hours, we use dates
                    SpreadChart.ChartAreas[0].AxisX.LabelStyle.Format = "d MMM";
                }
                else if (tempTS.TotalMinutes > 5) {
                    SpreadChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm";
                }
                else if (tempTS.TotalMinutes <= 5) {
                    SpreadChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                }
            }
            else {
                SpreadChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            }
        }

        private void SpreadGraph_FormClosing(object sender, FormClosingEventArgs e) {
            // need to remove this form object from the spreadGraph
            IRT.SpreadGraph_Dict.TryRemove(Exchange.CodeName + "-" + pair, out SpreadGraph x);
        }
    }
}
