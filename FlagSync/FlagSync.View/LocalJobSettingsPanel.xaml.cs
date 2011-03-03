using System.Windows;
using System.Windows.Controls;
using FlagSync.Core;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for LocalJobSettingsPanel.xaml
    /// </summary>
    public partial class LocalJobSettingsPanel : UserControl
    {
        private JobSettingViewModel jobSettingViewModel;

        public LocalJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();
            this.jobSettingViewModel = viewModel;
            this.DataContext = viewModel;
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsBackupModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsBackupModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.jobSettingViewModel.SyncMode = SyncMode.LocalBackup;
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsSyncModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsSyncModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.jobSettingViewModel.SyncMode = SyncMode.LocalSynchronization;
        }

        /// <summary>
        /// Handles the Click event of the directoryAButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryAButton_Click(object sender, RoutedEventArgs e)
        {
            this.jobSettingViewModel.DirectoryA = this.ShowFolderDialog();
        }

        /// <summary>
        /// Handles the Click event of the directoryBButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryBButton_Click(object sender, RoutedEventArgs e)
        {
            this.jobSettingViewModel.DirectoryB = this.ShowFolderDialog();
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