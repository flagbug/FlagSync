using FlagSync.Data;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class FtpFileSystemSettingsViewModel : ViewModelBase<FtpFileSystemSettingsViewModel>
    {
        private FtpFileSystemSetting setting;

        public string ServerAddress
        {
            get { return this.setting.Source; }
            set
            {
                if (this.setting.Source != value)
                {
                    this.setting.Source = value;
                    this.OnPropertyChanged(vm => vm.ServerAddress);
                }
            }
        }

        public string Username
        {
            get { return this.setting.Username; }
            set
            {
                if (this.setting.Username != value)
                {
                    this.setting.Username = value;
                    this.OnPropertyChanged(vm => vm.Username);
                }
            }
        }

        public string Password
        {
            get { return this.setting.Password; }
            set
            {
                if (this.setting.Password != value)
                {
                    this.setting.Password = value;
                    this.OnPropertyChanged(vm => vm.Password);
                }
            }
        }

        public FtpFileSystemSettingsViewModel(FtpFileSystemSetting setting)
        {
            this.setting = setting;
        }

        public FtpFileSystemSettingsViewModel()
        {
        }
    }
}