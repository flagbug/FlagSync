using System.Windows;
using System.Windows.Controls;
using FlagSync.Core;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for FtpJobSettingsPanel.xaml
    /// </summary>
    public partial class FtpJobSettingsPanel : UserControl
    {
        public FtpJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel.JobSetting = viewModel;

            switch (viewModel.SyncMode)
            {
                case SyncMode.FtpBackup:
                    this.jobSettingsBackupModeRadioButton.IsChecked = true;
                    break;

                case SyncMode.FtpSynchronization:
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
            this.viewModel.JobSetting.SyncMode = SyncMode.FtpBackup;
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsSyncModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsSyncModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.viewModel.JobSetting.SyncMode = SyncMode.FtpSynchronization;
        }
    }
}