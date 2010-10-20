using System;
using System.ComponentModel;

namespace FlagSync.Core
{
    [Serializable]
    public class JobSetting : INotifyPropertyChanged
    {
        private string directoryA = String.Empty;
        private string directoryB = String.Empty;
        private JobWorker.SyncMode syncMode = JobWorker.SyncMode.Backup;
        private string name = String.Empty;

        /// <summary>
        /// Gets or sets the directory A.
        /// </summary>
        /// <value>The directory A.</value>
        public string DirectoryA
        {
            get
            {
                return this.directoryA;
            }

            set
            {
                if(this.directoryA != value)
                {
                    this.directoryA = value;
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
                return this.directoryB;
            }

            set
            {
                if(this.directoryB != value)
                {
                    this.directoryB = value;
                    this.OnPropertyChanged("DirectoryB");
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
                return this.syncMode;
            }

            set
            {
                if(this.syncMode != value)
                {
                    this.syncMode = value;
                    this.OnPropertyChanged("SyncMode");
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
                return this.name;
            }

            set
            {
                if(this.name != value)
                {
                    this.name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSetting"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public JobSetting(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Called when a property has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
