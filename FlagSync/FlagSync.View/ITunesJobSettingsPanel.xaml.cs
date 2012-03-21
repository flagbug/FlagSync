using FlagSync.View.ViewModels;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for ITunesJobSettingsPanel.xaml
    /// </summary>
    public sealed partial class ITunesJobSettingsPanel
    {
        private readonly ITunesJobSettingsViewModel viewModel;

        public ITunesJobSettingsPanel(JobSettingViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = new ITunesJobSettingsViewModel(viewModel);

            this.DataContext = this.viewModel;
        }
    }
}