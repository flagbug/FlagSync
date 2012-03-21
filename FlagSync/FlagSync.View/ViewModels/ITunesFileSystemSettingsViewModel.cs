using System.Collections.Generic;
using System.Linq;
using FlagSync.Data;
using iTunesLib;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class ITunesFileSystemSettingsViewModel : ViewModelBase<ITunesFileSystemSettingsViewModel>
    {
        private readonly ITunesFileSystemSetting setting;

        public string Playlist
        {
            get { return this.setting.Source; }
            set
            {
                if (this.setting.Source != value)
                {
                    this.setting.Source = value;
                    this.OnPropertyChanged(vm => vm.Playlist);
                }
            }
        }

        public static IEnumerable<string> AvailablePlaylists
        {
            get
            {
                var iTunesApp = new iTunesApp();

                return iTunesApp.LibrarySource.Playlists
                    .Cast<IITPlaylist>()
                    .Where(pl => pl.Kind == ITPlaylistKind.ITPlaylistKindUser)
                    .Select(pl => pl.Name)
                    .OrderBy(name => name);
            }
        }

        public ITunesFileSystemSettingsViewModel(ITunesFileSystemSetting setting)
        {
            this.setting = setting;

            this.Playlist = this.setting.Source ?? AvailablePlaylists.First();
        }

        public ITunesFileSystemSettingsViewModel()
        {
        }
    }
}