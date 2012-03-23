using System.Windows;
using System.Windows.Controls;
using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for FtpFileSystemPanel.xaml
    /// </summary>
    public sealed partial class FtpFileSystemPanel
    {
        private readonly FtpFileSystemViewModel viewModel;

        public FtpFileSystemPanel(FtpFileSystemViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}