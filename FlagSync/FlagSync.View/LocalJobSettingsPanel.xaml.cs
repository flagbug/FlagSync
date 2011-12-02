using System.Windows;
using System.Windows.Controls;
using FlagSync.Data;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for LocalJobSettingsPanel.xaml
    /// </summary>
    public sealed partial class LocalJobSettingsPanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalJobSettingsPanel"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public LocalJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel.JobSetting = viewModel;

            switch (viewModel.SyncMode)
            {
                case SyncMode.LocalBackup:
                    this.jobSettingsBackupModeRadioButton.IsChecked = true;
                    break;

                case SyncMode.LocalSynchronization:
                    this.jobSettingsSyncModeRadioButton.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsBackupModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsBackupModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.viewModel.JobSetting.SyncMode = SyncMode.LocalBackup;
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsSyncModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsSyncModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.viewModel.JobSetting.SyncMode = SyncMode.LocalSynchronization;
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
        /// Handles the Click event of the directoryBButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryBButton_Click(object sender, RoutedEventArgs e)
        {
            string result = ShowFolderDialog();

            if (result != null)
            {
                this.viewModel.JobSetting.DirectoryB = result;
            }
        }

        /// <summary>
        /// Shows a folder dialog.
        /// </summary>
        /// <returns>
        /// The selected folder
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