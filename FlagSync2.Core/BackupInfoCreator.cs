using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlagSync2.Core
{
    public class BackupInfoCreator : FolderInfoCreator
    {
        #region Private fields
        private DirectoryInfo source;
        private DirectoryInfo target;
        #endregion

        #region Public properties
        public DirectoryInfo Source
        {
            get
            {
                return this.source;
            }

            set
            {
                this.source = value;
            }
        }

        public DirectoryInfo Target
        {
            get
            {
                return this.target;
            }

            set
            {
                this.target = value;
            }
        }
        #endregion

        #region Constructor
        public BackupInfoCreator()
        {

        }

        public BackupInfoCreator(DirectoryInfo source, DirectoryInfo target)
        {
            this.source = source;
            this.target = target;
        }
        #endregion

        #region Public methods
        public override IEnumerable<SyncInfo> CreateSyncInfos()
        {
            List<SyncInfo> syncInfos = new List<SyncInfo>();

            syncInfos.AddRange(this.CreateBackupList(this.source, this.target));
            syncInfos.AddRange(this.CreateDeletionList(this.source, this.target));

            return syncInfos;
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Checks if the files in the target folder are contained in the source folder, if not they get a SyncInfo for deletion
        /// </summary>
        /// <param name="source">The source folder</param>
        /// <param name="target">The target folder</param>
        /// <returns>A list of SyncInfos</returns>
        protected List<SyncInfo> CreateDeletionList(DirectoryInfo source, DirectoryInfo target)
        {
            List<SyncInfo> syncInfos = new List<SyncInfo>();

            //Search files
            foreach(FileInfo targetFile in target.GetFiles())
            {
                FileInfo sourceFile = new FileInfo(Path.Combine(source.FullName, targetFile.Name));

                if(!sourceFile.Exists)
                {
                    SyncInfo syncInfo = new SyncInfo(null, targetFile, SyncInfoType.Deletion);
                    syncInfos.Add(syncInfo);
                    this.OnNewSyncInfo(syncInfo);
                }
            }

            //Search sub folders
            foreach(DirectoryInfo targetDirectory in this.target.GetDirectories())
            {
                DirectoryInfo sourceDirectory = new DirectoryInfo(Path.Combine(source.FullName, targetDirectory.Name));

                if(!sourceDirectory.Exists)
                {
                    SyncInfo syncInfo = new SyncInfo(null, sourceDirectory, SyncInfoType.Deletion);
                    syncInfos.Add(syncInfo);
                    this.OnNewSyncInfo(syncInfo);
                }

                else
                {
                    syncInfos.AddRange(CreateDeletionList(targetDirectory, sourceDirectory));
                }
            }

            return syncInfos;
        }
        #endregion
    }
}

