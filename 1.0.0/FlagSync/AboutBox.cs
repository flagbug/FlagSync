using System;
using System.Windows.Forms;

namespace FlagSync
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.VersionLabel.Text += Application.ProductVersion;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
