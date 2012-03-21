using FlagSync.Data;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class LocalFileSystemSettingsViewModel : ViewModelBase<LocalFileSystemSettingsViewModel>
    {
        private readonly LocalFileSystemSetting setting;

        public string Directory
        {
            get { return this.setting.Source; }
            set
            {
                if (this.setting.Source != value)
                {
                    this.setting.Source = value;
                    this.OnPropertyChanged(vm => vm.Directory);
                }
            }
        }

        public LocalFileSystemSettingsViewModel(LocalFileSystemSetting setting)
        {
            this.setting = setting;
        }
    }
}