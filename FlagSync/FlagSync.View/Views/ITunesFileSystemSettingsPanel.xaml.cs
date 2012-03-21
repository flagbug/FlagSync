using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for ITunesFileSystemSettingsPanel.xaml
    /// </summary>
    public sealed partial class ITunesFileSystemSettingsPanel
    {
        public ITunesFileSystemSettingsPanel(ITunesFileSystemSettingsViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
        }
    }
}