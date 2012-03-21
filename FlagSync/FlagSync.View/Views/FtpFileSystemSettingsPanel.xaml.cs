using System.Windows;
using System.Windows.Controls;
using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for FtpFileSystemSettingsPanel.xaml
    /// </summary>
    public sealed partial class FtpFileSystemSettingsPanel
    {
        public FtpFileSystemSettingsPanel(FtpFileSystemSettingsViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
        }

        /// <summary>
        /// Handles the PasswordChanged event of the PasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}