using System;

namespace FlagSync.Core
{
    /// <summary>
    /// Sync mode of a job
    /// </summary>
    public enum SyncMode
    {
        /// <summary>
        /// Backup mode
        /// </summary>
        Backup,
        /// <summary>
        /// Synchronization mode
        /// </summary>
        Synchronization
    }

    [Serializable]
    public class JobSetting
    {
        private string directoryA = String.Empty;
        private string directoryB = String.Empty;
        private SyncMode syncMode = SyncMode.Backup;
        private string name = String.Empty;
        private bool isIncluded = true;

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
                this.directoryA = value;
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
                this.directoryB = value;
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
                return this.syncMode;
            }

            set
            {
                this.syncMode = value;
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
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is included.
        /// </summary>
        /// <value>true if this instance is included; otherwise, false.</value>
        public bool IsIncluded
        {
            get
            {
                return this.isIncluded;
            }

            set
            {
                this.isIncluded = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSetting"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public JobSetting(string name)
        {
            this.name = name;
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