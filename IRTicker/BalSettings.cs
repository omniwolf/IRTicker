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

            BalSettingsIROTCAPIKey_textbox.Text = Properties.Settings.Default.IROTCAPIKey;
            BalSettingsIROTCAPISecret_textbox.Text = Properties.Settings.Default.IROTCAPISecret;

            BalSettingsIROTCSGAPIKey_textbox.Text = Properties.Settings.Default.IROTCSGAPIKey;
            BalSettingsIROTCSGAPISecret_textbox.Text = Properties.Settings.Default.IROTCSGAPISecret;

            BalSettingsB2C2Token_textbox.Text = Properties.Settings.Default.B2C2Token;

            BalSettingsCoinbaseAPIKey_textbox.Text = Properties.Settings.Default.CoinbaseAPIKey;
            BalSettingsCoinbaseAPISecret_textbox.Text = Properties.Settings.Default.CoinbaseAPISecret;
            BalSettingsCoinbasePassPhrase_textbox.Text = Properties.Settings.Default.CoinbasePassPhrase;

            BalSettingsTrigonXToken_textbox.Text = Properties.Settings.Default.TrigonXToken;

            BalSettingsETHWallet_textbox.Text = Properties.Settings.Default.ETHWalletAddress;

            BalSettingsSlackToken_textbox.Text = Properties.Settings.Default.SlackBotToken;
            BalSettingsSlackChannel_textbox.Text = Properties.Settings.Default.SlackBotChannel;

            BalSettingsGDriveFolder_textbox.Text = Properties.Settings.Default.GDriveFolder_BalSettings;
        }

        private void BalSettingsDiscard_button_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void BalSettingsSave_button_Click(object sender, EventArgs e) {
            Properties.Settings.Default.IROTCAPIKey = BalSettingsIROTCAPIKey_textbox.Text;
            Properties.Settings.Default.IROTCAPISecret = BalSettingsIROTCAPISecret_textbox.Text;

            Properties.Settings.Default.IROTCSGAPIKey = BalSettingsIROTCSGAPIKey_textbox.Text;
            Properties.Settings.Default.IROTCSGAPISecret = BalSettingsIROTCSGAPISecret_textbox.Text;

            Properties.Settings.Default.B2C2Token = BalSettingsB2C2Token_textbox.Text;

            Properties.Settings.Default.CoinbaseAPIKey = BalSettingsCoinbaseAPIKey_textbox.Text;
            Properties.Settings.Default.CoinbaseAPISecret = BalSettingsCoinbaseAPISecret_textbox.Text;
            Properties.Settings.Default.CoinbasePassPhrase = BalSettingsCoinbasePassPhrase_textbox.Text;

            Properties.Settings.Default.TrigonXToken = BalSettingsTrigonXToken_textbox.Text;

            Properties.Settings.Default.ETHWalletAddress = BalSettingsETHWallet_textbox.Text;

            Properties.Settings.Default.SlackBotToken = BalSettingsSlackToken_textbox.Text;
            Properties.Settings.Default.SlackBotChannel = BalSettingsSlackChannel_textbox.Text;

            Properties.Settings.Default.GDriveFolder_BalSettings = BalSettingsGDriveFolder_textbox.Text;

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void BalSettingsGDriveFolder_textbox_Click(object sender, EventArgs e) {
            DialogResult GDriveRes = BalSettingsGDrive_folderBrowserDialog.ShowDialog();
            if (GDriveRes == DialogResult.OK) {
                BalSettingsGDriveFolder_textbox.Text = BalSettingsGDrive_folderBrowserDialog.SelectedPath;
            }
        }

        private void BalSettingsCopyToIROTC_button_Click(object sender, EventArgs e) {
            BalSettingsIROTCAPIKey_textbox.Text = Properties.Settings.Default.IRAPIPubKey;
            BalSettingsIROTCAPISecret_textbox.Text = Properties.Settings.Default.IRAPIPrivKey;
        }

        private void button1_Click(object sender, EventArgs e) {
            BalSettingsIROTCSGAPIKey_textbox.Text = Properties.Settings.Default.IRAPIPubKey;
            BalSettingsIROTCSGAPISecret_textbox.Text = Properties.Settings.Default.IRAPIPrivKey;
        }
    }
}
