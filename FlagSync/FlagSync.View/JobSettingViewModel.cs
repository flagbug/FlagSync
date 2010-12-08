using System.ComponentModel;
using System.IO;
using FlagLib.Patterns;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobSettingViewModel : ViewModelBase<JobSettingViewModel>, IDataErrorInfo
    {
        #region Fields

        private JobSetting internJobSetting;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is included for syncing.
        /// </summary>
        /// <value>true if this instance is included for syncing; otherwise, false.</value>
        public bool IsIncluded
        {
            get
            {
                return this.internJobSetting.IsIncluded;
            }

            set
            {
                if (this.IsIncluded != value)
                {
                    this.internJobSetting.IsIncluded = value;
                    this.OnPropertyChanged(view => view.IsIncluded);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory A.
        /// </summary>
        /// <value>The directory A.</value>
        public string DirectoryA
        {
            get
            {
                return this.internJobSetting.DirectoryA;
            }

            set
            {
                if (this.DirectoryA != value)
                {
                    this.internJobSetting.DirectoryA = value;
                    this.OnPropertyChanged(view => view.DirectoryA);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory B.
        /// </summary>
        /// <value>The directory B.</value>
        public string DirectoryB
        {
            get
            {
                return internJobSetting.DirectoryB;
            }

            set
            {
                if (this.DirectoryB != value)
                {
                    this.internJobSetting.DirectoryB = value;
                    this.OnPropertyChanged(view => view.DirectoryB);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.internJobSetting.Name;
            }

            set
            {
                if (this.Name != value)
                {
                    this.internJobSetting.Name = value;
                    this.OnPropertyChanged(view => view.Name);
                }
            }
        }

        /// <summary>
        /// Gets or sets the sync mode.
        /// </summary>
        /// <value>The sync mode.</value>
        public SyncMode SyncMode
        {
            get
            {
                return this.internJobSetting.SyncMode;
            }

            set
            {
                if (this.SyncMode != value)
                {
                    this.internJobSetting.SyncMode = value;
                    this.OnPropertyChanged(view => view.SyncMode);
                }
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error message</value>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified name.
        /// </summary>
        /// <value></value>
        public string this[string name]
        {
            get
            {
                string result = null;

                if ((name == "DirectoryA" && !Directory.Exists(this.DirectoryA)) || (name == "DirectoryB" && !Directory.Exists(this.DirectoryB)))
                {
                    result = "Directory doesn't exist!";
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the intern job setting.
        /// </summary>
        /// <value>The intern job setting.</value>
        public JobSetting InternJobSetting
        {
            get
            {
                return this.internJobSetting;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSettingViewModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public JobSettingViewModel(string name)
        {
            this.internJobSetting = new JobSetting(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSettingViewModel"/> class.
        /// </summary>
        /// <param name="internJobSetting">The intern job setting.</param>
        public JobSettingViewModel(JobSetting internJobSetting)
        {
            this.internJobSetting = internJobSetting;
        }

        #endregion Constructor

        #region Public methods

        public override string ToString()
        {
            return this.internJobSetting.ToString();
        }

        #endregion Public methods
    }
}