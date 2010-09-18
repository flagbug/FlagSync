using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlagSync2.Core
{
    public enum SyncInfoType
    {
        DirectoryCreation,
        Deletion, 
        FileModification,
        FileCreation,
        Nothing
    }

    public class SyncInfo
    {
        #region Private fields
        private FileSystemInfo source;
        private FileSystemInfo target;
        private SyncInfoType syncType;
        #endregion

        #region Public properties
        /// <summary>
        /// The source file
        /// </summary>
        public FileSystemInfo Source
        {
            get
            {
                return this.source;
            }
        }

        /// <summary>
        /// The target file or directory
        /// </summary>
        public FileSystemInfo Target
        {
            get
            {
                return this.target;
            }
        }

        /// <summary>
        /// Type of the sync
        /// </summary>
        public SyncInfoType SyncType
        {
            get 
            { 
                return syncType; 
            }

            set
            {
                this.syncType = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SyncInfo class
        /// </summary>
        /// <param name="source">The source file or directory</param>
        /// <param name="target">The target file or directory</param>
        public SyncInfo(FileSystemInfo source, FileSystemInfo target, SyncInfoType syncType)
        {
            this.source = source;
            this.target = target;
            this.syncType = syncType;
        }
        #endregion
    }
}
