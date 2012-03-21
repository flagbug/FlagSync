using System.Windows.Controls;
using FlagSync.Data;
using FlagSync.View.Views;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class FtpFileSystemViewModel : ViewModelBase<FtpFileSystemViewModel>, IFileSystemViewModel
    {
        private readonly FtpFileSystemSetting setting;

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

        public FtpFileSystemViewModel(FtpFileSystemSetting setting)
        {
            this.setting = setting;
        }

        public UserControl CreateView()
        {
            return new FtpFileSystemSettingsPanel(this);
        }
    }
}