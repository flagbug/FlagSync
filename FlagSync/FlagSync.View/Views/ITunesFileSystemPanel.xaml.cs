using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for ITunesFileSystemPanel.xaml
    /// </summary>
    public sealed partial class ITunesFileSystemPanel
    {
        private ITunesFileSystemViewModel viewModel;

        public ITunesFileSystemPanel(ITunesFileSystemViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }
    }
}