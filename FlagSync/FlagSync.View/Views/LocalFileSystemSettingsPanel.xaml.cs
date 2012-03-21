using System.Windows;
using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for LocalFileSystemSettingsPanel.xaml
    /// </summary>
    public sealed partial class LocalFileSystemSettingsPanel
    {
        public LocalFileSystemSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel.JobSetting = viewModel;
        }

        /// <summary>
        /// Handles the Click event of the directoryAButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryAButton_Click(object sender, RoutedEventArgs e)
        {
            string result = ShowFolderDialog();

            if (result != null)
            {
                this.viewModel.JobSetting.DirectoryA = result;
            }
        }

        /// <summary>
        /// Shows a folder dialog.
        /// </summary>
        /// <returns>
        /// The path of the selected folder.
        /// </returns>
        private static string ShowFolderDialog()
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