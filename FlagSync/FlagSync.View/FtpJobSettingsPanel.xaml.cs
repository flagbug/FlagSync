using System.Windows;
using System.Windows.Controls;
using FlagSync.View.ViewModels;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for FtpJobSettingsPanel.xaml
    /// </summary>
    public sealed partial class FtpJobSettingsPanel
    {
        public FtpJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel.JobSetting = viewModel;
        }

        /// <summary>
        /// Handles the PasswordChanged event of the PasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.JobSetting.FtpPassword = ((PasswordBox)sender).Password;
        }
    }
}