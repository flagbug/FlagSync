using System.Windows;
using System.Windows.Controls;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for ITunesJobSettingsPanel.xaml
    /// </summary>
    public partial class ITunesJobSettingsPanel : UserControl
    {
        public ITunesJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel.JobSetting = viewModel;
        }

        /// <summary>
        /// Handles the Click event of the directoryBButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.JobSetting.DirectoryB = this.ShowFolderDialog();
        }

        /// <summary>
        /// Shows a folder dialog.
        /// </summary>
        /// <returns>The selected folder</returns>
        private string ShowFolderDialog()
        {
            string selectedFolder = string.Empty;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                selectedFolder = dialog.SelectedPath;
            }

            return selectedFolder;
        }
    }
}