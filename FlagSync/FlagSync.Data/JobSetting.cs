using System;

namespace FlagSync.Data
{
    [Serializable]
    public class JobSetting
    {
        public FileSystemSetting FirstFileSystemSetting { get; set; }

        public FileSystemSetting SecondFileSystemSetting { get; set; }

        /// <summary>
        /// Gets or sets the sync mode.
        /// </summary>
        /// <value>The sync mode.</value>
        public SyncMode SyncMode { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is included.
        /// </summary>
        /// <value>true if this instance is included; otherwise, false.</value>
        public bool IsIncluded { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSetting"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public JobSetting(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSetting"/> class.
        /// </summary>
        public JobSetting()
        {
            this.IsIncluded = true;
        }
    }
}