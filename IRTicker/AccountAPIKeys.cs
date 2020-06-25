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
    public partial class AccountAPIKeys : Form {
        public AccountAPIKeys() {
            InitializeComponent();

            // populate fields
            FriendlyName_textbox1.Text = Properties.Settings.Default.APIFriendly1;
            FriendlyName_textbox2.Text = Properties.Settings.Default.APIFriendly2;
            FriendlyName_textbox3.Text = Properties.Settings.Default.APIFriendly3;
            FriendlyName_textbox4.Text = Properties.Settings.Default.APIFriendly4;
            FriendlyName_textbox5.Text = Properties.Settings.Default.APIFriendly5;

            PubKey_textBox1.Text = Properties.Settings.Default.IRAPIPubKey1;
            PubKey_textBox2.Text = Properties.Settings.Default.IRAPIPubKey2;
            PubKey_textBox3.Text = Properties.Settings.Default.IRAPIPubKey3;
            PubKey_textBox4.Text = Properties.Settings.Default.IRAPIPubKey4;
            PubKey_textBox5.Text = Properties.Settings.Default.IRAPIPubKey5;

            PrivKey_textBox1.Text = Properties.Settings.Default.IRAPIPrivKey1;
            PrivKey_textBox2.Text = Properties.Settings.Default.IRAPIPrivKey2;
            PrivKey_textBox3.Text = Properties.Settings.Default.IRAPIPrivKey3;
            PrivKey_textBox4.Text = Properties.Settings.Default.IRAPIPrivKey4;
            PrivKey_textBox5.Text = Properties.Settings.Default.IRAPIPrivKey5;

        }

        private void SettingsOKButton_Click(object sender, EventArgs e) {
            Properties.Settings.Default.APIFriendly1 = FriendlyName_textbox1.Text;
            Properties.Settings.Default.APIFriendly2 = FriendlyName_textbox2.Text;
            Properties.Settings.Default.APIFriendly3 = FriendlyName_textbox3.Text;
            Properties.Settings.Default.APIFriendly4 = FriendlyName_textbox4.Text;
            Properties.Settings.Default.APIFriendly5 = FriendlyName_textbox5.Text;

            Properties.Settings.Default.IRAPIPubKey1 = PubKey_textBox1.Text;
            Properties.Settings.Default.IRAPIPubKey2 = PubKey_textBox2.Text;
            Properties.Settings.Default.IRAPIPubKey3 = PubKey_textBox3.Text;
            Properties.Settings.Default.IRAPIPubKey4 = PubKey_textBox4.Text;
            Properties.Settings.Default.IRAPIPubKey5 = PubKey_textBox5.Text;

            Properties.Settings.Default.IRAPIPrivKey1 = PrivKey_textBox1.Text;
            Properties.Settings.Default.IRAPIPrivKey2 = PrivKey_textBox2.Text;
            Properties.Settings.Default.IRAPIPrivKey3 = PrivKey_textBox3.Text;
            Properties.Settings.Default.IRAPIPrivKey4 = PrivKey_textBox4.Text;
            Properties.Settings.Default.IRAPIPrivKey5 = PrivKey_textBox5.Text;

            Properties.Settings.Default.Save();
            
            Close();
        }

        public class APIKeyGroup {
            public APIKeyGroup(string _friendlyName, string _pubKey, string _privKey) {
                friendlyName = _friendlyName;
                pubKey = _pubKey;
                privKey = _privKey;
            }
            public string friendlyName { get; set; }
            public string pubKey { get; set; }
            public string privKey { get; set; }

            public override string ToString() {
                return friendlyName;
            }
        }

        private void APIKeyCloseNoSave_button_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
