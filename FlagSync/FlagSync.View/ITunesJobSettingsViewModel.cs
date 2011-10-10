using System.Collections.Generic;
using System.Linq;
using FlagLib.Patterns.MVVM;
using iTunesLib;

namespace FlagSync.View
{
    internal class ITunesJobSettingsViewModel : ViewModelBase<ITunesJobSettingsViewModel>
    {
        private JobSettingViewModel jobSetting;

        /// <summary>
        /// Gets or sets the job setting.
        /// </summary>
        /// <value>The job setting.</value>
        /// <remarks></remarks>
        public JobSettingViewModel JobSetting
        {
            get { return this.jobSetting; }
            set
            {
                if (this.JobSetting != value)
                {
                    this.jobSetting = value;
                    this.OnPropertyChanged(vm => vm.JobSetting);
                }
            }
        }

        /// <summary>
        /// Gets the playlists from iTunes.
        /// </summary>
        /// <value>The playlists from iTunes.</value>
        /// <remarks></remarks>
        public IEnumerable<string> ITunesPlaylists
        {
            get
            {
                iTunesApp iTunes = new iTunesApp();
                return iTunes.LibrarySource.Playlists
                    .Cast<IITPlaylist>()
                    .Where(pl => pl.Kind == ITPlaylistKind.ITPlaylistKindUser)
                    .Select(pl => pl.Name)
                    .OrderBy(name => name);
            }
        }
    }
}