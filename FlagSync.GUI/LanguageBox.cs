using System;
using System.Windows.Forms;

namespace FlagSync.GUI
{
    public partial class LanguageBox : Form
    {
        private string selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                return this.selectedLanguage;
            }
        }
        
        public LanguageBox()
        {
            InitializeComponent();

            this.languageComboBox.SelectedItem = this.languageComboBox.Items[0];
        }

        private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string language = languageComboBox.SelectedItem.ToString();

            if (language == "English")
            {
                this.selectedLanguage = "en-US";
            }

            else if (language == "Deutsch")
            {
                this.selectedLanguage = "de-DE";
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
