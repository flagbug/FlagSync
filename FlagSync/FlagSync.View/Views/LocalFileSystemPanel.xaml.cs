using System.Windows;
using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for LocalFileSystemPanel.xaml
    /// </summary>
    public sealed partial class LocalFileSystemPanel
    {
        private readonly LocalFileSystemViewModel viewModel;

        public LocalFileSystemPanel(LocalFileSystemViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        private void directoryAButton_Click(object sender, RoutedEventArgs e)
        {
            string result = ShowFolderDialog();

            if (result != null)
            {
                this.viewModel.Directory = result;
            }
        }

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