using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for ITunesFileSystemSettingsPanel.xaml
    /// </summary>
    public sealed partial class ITunesFileSystemSettingsPanel
    {
        private readonly ITunesJobSettingsViewModel viewModel;

        public ITunesFileSystemSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = new ITunesJobSettingsViewModel(viewModel);

            this.DataContext = this.viewModel;
        }
    }
}