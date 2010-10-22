using System.ComponentModel;
using System.IO;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobSettingViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Members
        private JobSetting intern;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the directory A.
        /// </summary>
        /// <value>The directory A.</value>
        public string DirectoryA
        {
            get
            {
                return this.intern.DirectoryA;
            }

            set
            {
                if (this.DirectoryA != value)
                {
                    this.intern.DirectoryA = value;
                    this.OnPropertyChanged("DirectoryA");
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
                return intern.DirectoryB;
            }

            set
            {
                if (this.DirectoryB != value)
                {
                    this.intern.DirectoryB = value;
                    this.OnPropertyChanged("DirectoryB");
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
                return this.intern.Name;
            }

            set
            {
                if (this.Name != value)
                {
                    this.intern.Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the sync mode.
        /// </summary>
        /// <value>The sync mode.</value>
        public JobWorker.SyncMode SyncMode
        {
            get
            {
                return this.intern.SyncMode;
            }

            set
            {
                if (this.SyncMode != value)
                {
                    this.intern.SyncMode = value;
                    this.OnPropertyChanged("SyncMode");
                }
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value></value>
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

        public JobSetting Intern
        {
            get
            {
                return this.intern;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public JobSettingViewModel(string name)
        {
            this.intern = new JobSetting(name);
            this.Name = name;
        }
        #endregion

        #region Public methods
        public override string ToString()
        {
            return this.intern.ToString();
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Called when a property has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
        #endregion
}