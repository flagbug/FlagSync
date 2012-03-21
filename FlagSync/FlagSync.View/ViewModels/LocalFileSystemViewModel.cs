using System.Windows.Controls;
using FlagSync.Data;
using FlagSync.View.Views;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class LocalFileSystemViewModel : ViewModelBase<LocalFileSystemViewModel>, IFileSystemViewModel
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

        public LocalFileSystemViewModel(LocalFileSystemSetting setting)
        {
            this.setting = setting;
        }

        public UserControl CreateView()
        {
            return new LocalFileSystemSettingsPanel(this);
        }
    }
}