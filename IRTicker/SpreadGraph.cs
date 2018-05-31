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
        public SpreadGraph(List<DataPoint> pairSpreadHistory) {
            InitializeComponent();

            /*foreach (KeyValuePair<string, List<DataPoint>> spreadValue in Exchange.GetSpreadHistory()) {
                foreach ()
            }*/


            foreach (DataPoint dp in pairSpreadHistory) {
                SpreadChart.Series["Series1"].Points.Add(dp);
            }

        }

    }
}
