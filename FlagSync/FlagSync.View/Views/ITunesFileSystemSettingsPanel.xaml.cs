using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for ITunesFileSystemSettingsPanel.xaml
    /// </summary>
    public sealed partial class ITunesFileSystemSettingsPanel
    {
        private ITunesFileSystemViewModel viewModel;

        public ITunesFileSystemSettingsPanel(ITunesFileSystemViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }
    }
}