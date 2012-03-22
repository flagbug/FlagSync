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
        private IFileSystemViewModel secondFileSystem;

        public ObservableCollection<UserControl> CurrentFirstFileSystem { get; private set; }

        public ObservableCollection<UserControl> CurrentSecondFileSystem { get; private set; }

        public IFileSystemViewModel FirstFileSystem
        {
            set
            {
                this.firstFileSystem = value;

                this.CurrentFirstFileSystem.Clear();
                this.CurrentFirstFileSystem.Add(this.firstFileSystem.CreateView());
            }
        }

        public IFileSystemViewModel SecondFileSystem
        {
            set
            {
                this.secondFileSystem = value;

                this.CurrentSecondFileSystem.Clear();
                this.CurrentSecondFileSystem.Add(this.secondFileSystem.CreateView());
            }
        }

        public bool IsBackup
        {
            get { return this.setting.SyncMode == SyncMode.Backup; }
            set
            {
                if (this.IsBackup != value)
                {
                    this.setting.SyncMode = value ? SyncMode.Backup : SyncMode.Synchronization;

                    this.OnPropertyChanged(vm => vm.IsBackup);
                }
            }
        }

        public bool IsSynchronization
        {
            get { return this.setting.SyncMode == SyncMode.Synchronization; }
            set
            {
                if (this.IsSynchronization != value)
                {
                    this.setting.SyncMode = value ? SyncMode.Synchronization : SyncMode.Backup;

                    this.OnPropertyChanged(vm => vm.IsSynchronization);
                }
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

        public ICommand CreateSecondFileSystemCommand
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
                                this.setting.SecondFileSystem = localSetting;
                                this.SecondFileSystem = new LocalFileSystemViewModel(localSetting);
                                break;

                            case "Ftp":
                                var ftpSetting = new FtpFileSystemSetting();
                                this.setting.SecondFileSystem = ftpSetting;
                                this.SecondFileSystem = new FtpFileSystemViewModel(ftpSetting);
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
            this.CurrentSecondFileSystem = new ObservableCollection<UserControl>();
        }
    }
}