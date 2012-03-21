using FlagSync.Data;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class LocalJobSettingsPanelViewModel : ViewModelBase<LocalJobSettingsPanelViewModel>
    {
        private LocalFileSystemSetting setting;

        public string Directory

        public LocalJobSettingsPanelViewModel(LocalFileSystemSetting setting)
        {
            this.setting = setting;
        }
    }
}