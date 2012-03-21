using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using FlagSync.Data;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class JobCompositionViewModel : ViewModelBase<JobCompositionViewModel>
    {
        private readonly JobSettingViewModel setting;
        private IFileSystemViewModel firstFileSystem;

        public ObservableCollection<UserControl> CurrentFirstFileSystem { get; private set; }

        public IFileSystemViewModel FirstFileSystem
        {
            set
            {
                this.firstFileSystem = value;

                this.CurrentFirstFileSystem.Clear();
                this.CurrentFirstFileSystem.Add(this.firstFileSystem.CreateView());
            }
        }

        public ICommand CreateFirstFileSystemCommand
        {
            get
            {
                return new RelayCommand
                (
                    param =>
                    {
                        var fileSystem = (string)param;

                        switch (fileSystem)
                        {
                            case "Local":
                                var localSetting = new LocalFileSystemSetting();
                                this.setting.FirstFileSystem = localSetting;
                                this.FirstFileSystem = new LocalFileSystemViewModel(localSetting);
                                break;

                            case "Ftp":
                                var ftpSetting = new FtpFileSystemSetting();
                                this.setting.FirstFileSystem = ftpSetting;
                                this.FirstFileSystem = new FtpFileSystemViewModel(ftpSetting);
                                break;

                            case "iTunes":
                                var iTunesSetting = new ITunesFileSystemSetting();
                                this.setting.FirstFileSystem = iTunesSetting;
                                this.FirstFileSystem = new ITunesFileSystemViewModel(iTunesSetting);
                                break;
                        }
                    }
                );
            }
        }

        public JobCompositionViewModel(JobSettingViewModel setting)
        {
            this.setting = setting;
            this.CurrentFirstFileSystem = new ObservableCollection<UserControl>();
        }
    }
}