using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlagSync2.Core
{
    public abstract class FolderInfoCreator : IInfoCreator
    {
        #region Events
        /// <summary>
        /// Occurs when a new SyncInfo gets created
        /// </summary>
        public event EventHandler<InfoEventArgs> NewSyncInfo;

        /// <summary>
        /// Raises the NewSyncInfo event
        /// </summary>
        /// <param name="syncInfo">The new SyncInfo</param>
        protected virtual void OnNewSyncInfo(SyncInfo syncInfo)
        {
            if(this.NewSyncInfo != null)
            {
                this.NewSyncInfo.Invoke(this, new InfoEventArgs(syncInfo));
            }
        }
        #endregion

        public FolderInfoCreator()
        {
        
        }

        #region Public methods
        public abstract IEnumerable<SyncInfo> CreateSyncInfos();
        #endregion

        #region Protected methods
        /// <summary>
        /// Creates a SyncInfo list for a backup of a directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected List<SyncInfo> CreateBackupList(DirectoryInfo source, DirectoryInfo target)
        {
            List<SyncInfo> syncInfos = new List<SyncInfo>();

            syncInfos.AddRange(this.CreateNewAndModifiedList(source, target));

            foreach(DirectoryInfo sourceDirectory in source.GetDirectories())
            {
                DirectoryInfo targetDirectory = new DirectoryInfo(Path.Combine(target.FullName, sourceDirectory.Name));

                if(!targetDirectory.Exists)
                {
                    SyncInfo syncInfo = new SyncInfo(sourceDirectory, target, SyncInfoType.DirectoryCreation);
                    syncInfos.Add(syncInfo);
                    this.OnNewSyncInfo(syncInfo);
                }

                this.CreateNewAndModifiedList(sourceDirectory, targetDirectory);
            }

            return syncInfos;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Checks two directories on new and modified files
        /// </summary>
        /// <param name="source">The source directory</param>
        /// <param name="target">The target directory</param>
        /// <returns></returns>
        private List<SyncInfo> CreateNewAndModifiedList(DirectoryInfo source, DirectoryInfo target)
        {
            List<SyncInfo> syncInfos = new List<SyncInfo>();

            foreach(FileInfo sourceFile in source.GetFiles())
            {
                FileInfo targetFile = new FileInfo(Path.Combine(target.FullName, sourceFile.Name));
                SyncInfo syncInfo = null;

                //Check on new file
                if(!targetFile.Exists)
                {
                    syncInfo = new SyncInfo(sourceFile, target, SyncInfoType.FileCreation);
                }

                //Check on modified file
                else if(sourceFile.CreationTime.CompareTo(target.CreationTime) == 1)
                {
                    syncInfo = new SyncInfo(sourceFile, target, SyncInfoType.FileModification);
                }

                //Else do nothing with the file
                else
                {
                    syncInfo = new SyncInfo(sourceFile, targetFile, SyncInfoType.Nothing);
                }

                syncInfos.Add(syncInfo);
                this.OnNewSyncInfo(syncInfo);
            }

            return syncInfos;
        }
        #endregion
    }
}
