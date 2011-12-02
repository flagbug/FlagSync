using System.Windows;
using System.Windows.Controls;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for ITunesJobSettingsPanel.xaml
    /// </summary>
    public sealed partial class ITunesJobSettingsPanel : UserControl
    {
        private readonly ITunesJobSettingsViewModel viewModel;

        public ITunesJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = new ITunesJobSettingsViewModel(viewModel);

            this.DataContext = this.viewModel;
        }

        /// <summary>
        /// Handles the Click event of the directoryBButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryButton_Click(object sender, RoutedEventArgs e)
        {
            string result = this.ShowFolderDialog();

            if (result != null)
            {
                this.viewModel.JobSetting.DirectoryB = result;
            }
        }

        /// <summary>
        /// Shows a folder dialog.
        /// </summary>
        /// <returns>The selected folder</returns>
        private string ShowFolderDialog()
        {
            string selectedFolder = null;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolder = dialog.SelectedPath;
                }
            }

            return selectedFolder;
        }
    }
}