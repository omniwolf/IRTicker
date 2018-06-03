using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRTicker {
    public partial class Help : Form {

        private IRTicker IRT;

        public Help(IRTicker _IRT) {
            InitializeComponent();

            IRT = _IRT;
        }

        private void Help_FormClosing(object sender, FormClosingEventArgs e) {
            IRT.Help_Button.Enabled = true;
        }

        private void Help_MouseDoubleClick(object sender, MouseEventArgs e) {
            Close();
        }

        private void HelpBody_Label_MouseDoubleClick(object sender, MouseEventArgs e) {
            Close();
        }
    }
}
