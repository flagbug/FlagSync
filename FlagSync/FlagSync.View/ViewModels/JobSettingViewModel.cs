using System;
using System.ComponentModel;
using FlagSync.Data;
using FlagSync.View.Properties;
using Rareform.Patterns.MVVM;

namespace FlagSync.View.ViewModels
{
    public class JobSettingViewModel : ViewModelBase<JobSettingViewModel>, IDataErrorInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether this job is included syncing.
        /// </summary>
        /// <value>
        /// true if this job is included for syncing; otherwise, false.
        /// </value>
        public bool IsIncluded
        {
            get { return this.InternJobSetting.IsIncluded; }
            set
            {
                if (this.IsIncluded != value)
                {
                    this.InternJobSetting.IsIncluded = value;
                    this.OnPropertyChanged(vm => vm.IsIncluded);
                }
            }
        }

        public FileSystemSetting FirstFileSystem
        {
            get { return this.InternJobSetting.FirstFileSystemSetting; }
            set
            {
                if (this.FirstFileSystem != value)
                {
                    this.InternJobSetting.FirstFileSystemSetting = value;
                    this.OnPropertyChanged(vm => vm.FirstFileSystem);
                }
            }
        }

        public FileSystemSetting SecondFileSystem
        {
            get { return this.InternJobSetting.SecondFileSystemSetting; }
            set
            {
                if (this.SecondFileSystem != value)
                {
                    this.InternJobSetting.SecondFileSystemSetting = value;
                    this.OnPropertyChanged(vm => vm.SecondFileSystem);
                }
            }
        }

        public string Name
        {
            get { return this.InternJobSetting.Name; }
            set
            {
                if (this.Name != value)
                {
                    this.InternJobSetting.Name = value;
                    this.OnPropertyChanged(vm => vm.Name);
                }
            }
        }

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
                switch (this.SyncMode)
                {
                    case SyncMode.Backup:
                        return Resources.BackupString;

                    case SyncMode.Synchronization:
                        return Resources.SynchronizationString;
                }

                throw new InvalidOperationException();
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

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasErrors
        {
            get { return false; }
        }
    }
}