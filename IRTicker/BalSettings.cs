using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRTicker
{
    public partial class BalSettings : Form
    {
        public BalSettings() {
            InitializeComponent();

            BalSettingsB2C2Token_textbox.Text = Properties.Settings.Default.B2C2Token;

            BalSettingsCoinbaseAPIKey_textbox.Text = Properties.Settings.Default.CoinbaseAPIKey;
            BalSettingsCoinbaseAPISecret_textbox.Text = Properties.Settings.Default.CoinbaseAPIKey;
            BalSettingsCoinbasePassPhrase_textbox.Text = Properties.Settings.Default.CoinbasePassPhrase;
        }

        private void BalSettingsDiscard_button_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void BalSettingsSave_button_Click(object sender, EventArgs e) {
            Properties.Settings.Default.B2C2Token = BalSettingsB2C2Token_textbox.Text;

            Properties.Settings.Default.CoinbaseAPIKey = BalSettingsCoinbaseAPIKey_textbox.Text;
            Properties.Settings.Default.CoinbaseAPISecret = BalSettingsCoinbaseAPISecret_textbox.Text;
            Properties.Settings.Default.CoinbasePassPhrase = BalSettingsCoinbasePassPhrase_textbox.Text;

            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
