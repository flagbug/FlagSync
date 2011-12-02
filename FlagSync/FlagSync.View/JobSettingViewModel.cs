using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FlagLib.Patterns.MVVM;
using FlagLib.Reflection;
using FlagSync.Data;
using FlagSync.View.Properties;

namespace FlagSync.View
{
    public class JobSettingViewModel : ViewModelBase<JobSettingViewModel>, IDataErrorInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is included for syncing.
        /// </summary>
        /// <value>
        /// true if this instance is included for syncing; otherwise, false.
        /// </value>
        public bool IsIncluded
        {
            get { return this.InternJobSetting.IsIncluded; }
            set
            {
                if (this.IsIncluded != value)
                {
                    this.InternJobSetting.IsIncluded = value;
                    this.OnPropertyChanged(view => view.IsIncluded);
                    this.OnPropertyChanged(vm => vm.HasErrors);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory A.
        /// </summary>
        /// <value>
        /// The directory A.
        /// </value>
        public string DirectoryA
        {
            get
            {
                switch (this.SyncMode)
                {
                    case SyncMode.FtpBackup:
                    case SyncMode.FtpSynchronization:
                        return this.FtpAddress;

                    case SyncMode.ITunes:
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
                    this.OnPropertyChanged(vm => vm.HasErrors);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory B.
        /// </summary>
        /// <value>
        /// The directory B.
        /// </value>
        public string DirectoryB
        {
            get { return InternJobSetting.DirectoryB; }
            set
            {
                if (this.DirectoryB != value)
                {
                    this.InternJobSetting.DirectoryB = value;
                    this.OnPropertyChanged(view => view.DirectoryB);
                    this.OnPropertyChanged(vm => vm.HasErrors);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.InternJobSetting.Name; }
            set
            {
                if (this.Name != value)
                {
                    this.InternJobSetting.Name = value;
                    this.OnPropertyChanged(view => view.Name);
                    this.OnPropertyChanged(vm => vm.HasErrors);
                }
            }
        }

        /// <summary>
        /// Gets or sets the sync mode.
        /// </summary>
        /// <value>
        /// The sync mode.
        /// </value>
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
        /// <value>
        /// The name.
        /// </value>
        public string SyncModeString
        {
            get
            {
                string syncMode = string.Empty;

                switch (this.SyncMode)
                {
                    case SyncMode.FtpBackup:
                        syncMode = Resources.FtpServerBackupString;
                        break;

                    case SyncMode.FtpSynchronization:
                        syncMode = Resources.FtpServerSynchronizationString;
                        break;

                    case SyncMode.LocalBackup:
                        syncMode = Resources.LocalBackupString;
                        break;

                    case SyncMode.LocalSynchronization:
                        syncMode = Resources.LocalSynchronizationString;
                        break;

                    case SyncMode.ITunes:
                        syncMode = Resources.iTunesString;
                        break;
                }

                return syncMode;
            }
        }

        /// <summary>
        /// Gets or sets the FTP server address.
        /// </summary>
        /// <value>
        /// The FTP server address.
        /// </value>
        public string FtpAddress
        {
            get { return this.InternJobSetting.FtpAddress; }
            set
            {
                if (this.FtpAddress != value)
                {
                    this.InternJobSetting.FtpAddress = value;
                    this.OnPropertyChanged(vm => vm.FtpAddress);
                    this.OnPropertyChanged(vm => vm.HasErrors);
                }
            }
        }

        /// <summary>
        /// Gets or sets the login name of the FTP server user.
        /// </summary>
        /// <value>
        /// The login name of the FTP user.
        /// </value>
        public string FtpUserName
        {
            get { return this.InternJobSetting.FtpUserName; }
            set
            {
                if (this.FtpUserName != value)
                {
                    this.InternJobSetting.FtpUserName = value;
                    this.OnPropertyChanged(vm => vm.FtpUserName);
                    this.OnPropertyChanged(vm => vm.HasErrors);
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
                    this.OnPropertyChanged(vm => vm.HasErrors);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Itunes playlist.
        /// </summary>
        /// <value>
        /// The Itunes playlist.
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
                    this.OnPropertyChanged(vm => vm.HasErrors);
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
        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (this.SyncMode)
                {
                    case SyncMode.LocalBackup:
                    case SyncMode.LocalSynchronization:
                        if ((columnName == Reflector.GetMemberName(() => this.DirectoryA) && !Directory.Exists(this.DirectoryA)))
                        {
                            result = Resources.DirectoryDoesntExistMessage;
                        }

                        break;

                    case SyncMode.ITunes:
                        if (columnName == Reflector.GetMemberName(() => this.ITunesPlaylist) && String.IsNullOrEmpty(this.ITunesPlaylist))
                        {
                            result = Resources.SelectPlaylistErrorMessage;
                        }
                        break;

                    case SyncMode.FtpBackup:
                    case SyncMode.FtpSynchronization:
                        if (columnName == Reflector.GetMemberName(() => this.FtpAddress))
                        {
                            if (String.IsNullOrEmpty(this.FtpAddress))
                            {
                                result = Resources.FTPAddressCantBeEmptyMessage;
                            }

                            else
                            {
                                try
                                {
                                    Uri uri = new Uri(this.FtpAddress);

                                    if (uri.Scheme != Uri.UriSchemeFtp)
                                    {
                                        result = Resources.FTPAdressWrongFormatMessage;
                                    }
                                }

                                catch (UriFormatException)
                                {
                                    result = Resources.FTPAdressWrongFormatMessage;
                                }
                            }
                        }
                        break;
                }

                if (columnName == Reflector.GetMemberName(() => this.DirectoryB) && !Directory.Exists(this.DirectoryB))
                {
                    result = Resources.DirectoryDoesntExistMessage;
                }

                if (columnName == Reflector.GetMemberName(() => this.Name) && String.IsNullOrEmpty(this.Name))
                {
                    result = Resources.NameFieldCantBeEmptyMessage;
                }

                return result;
            }
        }

        /// <summary>
        /// Determines whether this instance has errors.
        /// </summary>
        /// <value>
        ///   true if this instance has errors; otherwise, false.
        /// </value>
        public bool HasErrors
        {
            get
            {
                return this.GetType()
                    .GetProperties()
                    .Any(property => this[property.Name] != null);
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