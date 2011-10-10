using System;
using System.ComponentModel;
using System.IO;
using FlagLib.Patterns.MVVM;
using FlagLib.Reflection;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobSettingViewModel : ViewModelBase<JobSettingViewModel>, IDataErrorInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is included for syncing.
        /// </summary>
        /// <value>true if this instance is included for syncing; otherwise, false.</value>
        /// <remarks></remarks>
        public bool IsIncluded
        {
            get { return this.InternJobSetting.IsIncluded; }
            set
            {
                if (this.IsIncluded != value)
                {
                    this.InternJobSetting.IsIncluded = value;
                    this.OnPropertyChanged(view => view.IsIncluded);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory A.
        /// </summary>
        /// <value>The directory A.</value>
        /// <remarks></remarks>
        public string DirectoryA
        {
            get
            {
                switch (this.SyncMode)
                {
                    case Core.SyncMode.FtpBackup:
                    case Core.SyncMode.FtpSynchronization:
                        return this.FtpAddress;

                    case Core.SyncMode.ITunes:
                        return this.ITunesPlaylist;
                }

                return this.InternJobSetting.DirectoryA;
            }
            set
            {
                if (this.DirectoryA != value)
                {
                    this.InternJobSetting.DirectoryA = value;
                    this.OnPropertyChanged(view => view.DirectoryA);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory B.
        /// </summary>
        /// <value>The directory B.</value>
        /// <remarks></remarks>
        public string DirectoryB
        {
            get { return InternJobSetting.DirectoryB; }
            set
            {
                if (this.DirectoryB != value)
                {
                    this.InternJobSetting.DirectoryB = value;
                    this.OnPropertyChanged(view => view.DirectoryB);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name
        {
            get { return this.InternJobSetting.Name; }
            set
            {
                if (this.Name != value)
                {
                    this.InternJobSetting.Name = value;
                    this.OnPropertyChanged(view => view.Name);
                }
            }
        }

        /// <summary>
        /// Gets or sets the sync mode.
        /// </summary>
        /// <value>The sync mode.</value>
        /// <remarks></remarks>
        public SyncMode SyncMode
        {
            get { return this.InternJobSetting.SyncMode; }
            set
            {
                if (this.SyncMode != value)
                {
                    this.InternJobSetting.SyncMode = value;
                    this.OnPropertyChanged(vm => vm.SyncMode);
                    this.OnPropertyChanged(vm => vm.SyncModeString);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string SyncModeString
        {
            get
            {
                string syncMode = string.Empty;

                switch (this.SyncMode)
                {
                    case Core.SyncMode.FtpBackup:
                        syncMode = Properties.Resources.FtpServerBackupString;
                        break;

                    case Core.SyncMode.FtpSynchronization:
                        syncMode = Properties.Resources.FtpServerSynchronizationString;
                        break;

                    case Core.SyncMode.LocalBackup:
                        syncMode = Properties.Resources.LocalBackupString;
                        break;

                    case Core.SyncMode.LocalSynchronization:
                        syncMode = Properties.Resources.LocalSynchronizationString;
                        break;

                    case Core.SyncMode.ITunes:
                        syncMode = Properties.Resources.iTunesString;
                        break;
                }

                return syncMode;
            }
        }

        /// <summary>
        /// Gets or sets the FTP server address.
        /// </summary>
        /// <value>The FTP server address.</value>
        /// <remarks></remarks>
        public string FtpAddress
        {
            get { return this.InternJobSetting.FtpAddress; }
            set
            {
                if (this.FtpAddress != value)
                {
                    this.InternJobSetting.FtpAddress = value;
                    this.OnPropertyChanged(vm => vm.FtpAddress);
                }
            }
        }

        /// <summary>
        /// Gets or sets the login name of the FTP server user.
        /// </summary>
        /// <value>The login name of the FTP user.</value>
        /// <remarks></remarks>
        public string FtpUserName
        {
            get { return this.InternJobSetting.FtpUserName; }
            set
            {
                if (this.FtpUserName != value)
                {
                    this.InternJobSetting.FtpUserName = value;
                    this.OnPropertyChanged(vm => vm.FtpUserName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the FTP server password.
        /// </summary>
        /// <value>
        /// The FTP server password.
        /// </value>
        public string FtpPassword
        {
            get { return this.InternJobSetting.FtpPassword; }
            set
            {
                if (this.FtpPassword != value)
                {
                    this.InternJobSetting.FtpPassword = value;
                    this.OnPropertyChanged(vm => vm.FtpPassword);
                }
            }
        }

        /// <summary>
        /// Gets or sets the iTunes playlist.
        /// </summary>
        /// <value>
        /// The iTunes playlist.
        /// </value>
        public string ITunesPlaylist
        {
            get { return this.InternJobSetting.ITunesPlaylist; }
            set
            {
                if (this.ITunesPlaylist != value)
                {
                    this.InternJobSetting.ITunesPlaylist = value;
                    this.OnPropertyChanged(vm => vm.ITunesPlaylist);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether scheduling is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if scheduling is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableScheduling
        {
            get { return this.InternJobSetting.EnableScheduling; }
            set
            {
                if (this.EnableScheduling != value)
                {
                    this.InternJobSetting.EnableScheduling = value;
                    this.OnPropertyChanged(vm => vm.EnableScheduling);
                }
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error message</value>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        ///   </returns>
        /// <remarks></remarks>
        public string Error
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        ///   </returns>
        /// <remarks></remarks>
        public string this[string name]
        {
            get
            {
                string result = null;

                switch (this.SyncMode)
                {
                    case Core.SyncMode.LocalBackup:
                    case Core.SyncMode.LocalSynchronization:
                        if ((name == Reflector.GetMemberName(() => this.DirectoryA) && !Directory.Exists(this.DirectoryA)))
                        {
                            result = Properties.Resources.DirectoryDoesntExistMessage;
                        }

                        break;

                    case Core.SyncMode.ITunes:
                        if (name == Reflector.GetMemberName(() => this.ITunesPlaylist) && string.IsNullOrEmpty(this.ITunesPlaylist))
                        {
                            result = Properties.Resources.SelectPlaylistErrorMessage;
                        }
                        break;
                }

                if (name == Reflector.GetMemberName(() => this.DirectoryB) && !Directory.Exists(this.DirectoryB))
                {
                    result = Properties.Resources.DirectoryDoesntExistMessage;
                }

                if (name == Reflector.GetMemberName(() => this.Name) && this.Name == String.Empty)
                {
                    result = Properties.Resources.NameFieldCantBeEmptyMessage;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the intern job setting.
        /// </summary>
        /// <value>The intern job setting.</value>
        /// <remarks></remarks>
        public JobSetting InternJobSetting { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSettingViewModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <remarks></remarks>
        public JobSettingViewModel(string name)
            : this(new JobSetting(name))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSettingViewModel"/> class.
        /// </summary>
        /// <param name="internJobSetting">The intern job setting.</param>
        /// <remarks></remarks>
        public JobSettingViewModel(JobSetting internJobSetting)
        {
            this.InternJobSetting = internJobSetting;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.InternJobSetting.ToString();
        }
    }
}