using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlagSync2.Core
{
    #region JobEventArgs
    public class FileCreationEventArgs : EventArgs
    {
        private FileInfo source;
        private DirectoryInfo target;

        public FileInfo Source
        {
            get
            {
                return this.source;
            }
        }

        public DirectoryInfo Target
        {
            get
            {
                return this.target;
            }
        }

        public FileCreationEventArgs(FileInfo source, DirectoryInfo target)
        {
            this.source = source;
            this.target = target;
        }
    }

    public class DirectoryCreationEventArgs : EventArgs
    {
        private DirectoryInfo source;
        private DirectoryInfo target;

        public DirectoryInfo Source
        {
            get
            {
                return this.source;
            }
        }

        public DirectoryInfo Target
        {
            get
            {
                return this.target;
            }
        }

        public DirectoryCreationEventArgs(DirectoryInfo source, DirectoryInfo target)
        {
            this.source = source;
            this.target = target;
        }
    }

    public class DeletionEventArgs : EventArgs
    {
        private FileSystemInfo source;
        public FileSystemInfo Source
        {
            get
            {
                return this.source;
            }
        }

        public DeletionEventArgs(FileSystemInfo source)
        {
            this.source = source;
        }
    }

    public class FileModificationEventArgs : EventArgs
    {
        private FileInfo source;
        private FileInfo target;

        public FileInfo Source
        {
            get
            {
                return this.source;
            }
        }

        public FileInfo Target
        {
            get
            {
                return this.target;
            }
        }

        public FileModificationEventArgs(FileInfo source, FileInfo target)
        {
            this.source = source;
            this.target = target;
        }
    }
    #endregion

    public enum JobType
    {
        Backup,
        Synchronization,
        iTunes
    }

    public class Job
    {
        #region Private fields
        private string name;
        private IInfoCreator syncInfosCreator;
        private List<SyncInfo> syncInfos;
        private long writtenBytes;
        private JobType type;
        #endregion

        #region Public properties
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

        public IInfoCreator SyncInfosCreator
        {
            get
            {
                return this.syncInfosCreator;
            }

            set
            {
                this.syncInfosCreator = value;

                if(value is BackupInfoCreator)
                {
                    this.type = JobType.Backup;
                }

                else if(value is SyncInfoCreator)
                {
                    this.type = JobType.Synchronization;
                }
            }
        }

        public List<SyncInfo> SyncInfos
        {
            get
            {
                return this.syncInfos;
            }
        }

        public int WrittenKiloBytes
        {
            get
            {
                return (int)(this.writtenBytes / 1024);
            }
        }

        public JobType Type
        {
            get
            {
                return this.type;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the job has started
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Raises the Started event
        /// </summary>
        protected virtual void OnStarted()
        {
            if(this.Started != null)
            {
                this.Started.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the job has finished
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Raises the Finished event
        /// </summary>
        protected virtual void OnFinished()
        {
            if(this.Finished != null)
            {
                this.Finished.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Occurs when a modified file has been copied
        /// </summary>
        public event EventHandler<FileModificationEventArgs> FileModified;

        /// <summary>
        /// Raises the FileModified event
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="target">The target file</param>
        protected virtual void OnFileModified(FileInfo source, FileInfo target)
        {
            if(this.FileModified != null)
            {
                this.FileModified.Invoke(this, new FileModificationEventArgs(source, target));
            }
        }

        /// <summary>
        /// Occurs when a file or an directory has been deleted
        /// </summary>
        public event EventHandler<DeletionEventArgs> FileOrDirectoryDeleted;

        /// <summary>
        /// Raises the FileOrDirectoryDeleted event
        /// </summary>
        /// <param name="source">The file or directory that has been deleted</param>
        protected virtual void OnFileOrDirectoryDeleted(FileSystemInfo source)
        {
            if(this.FileOrDirectoryDeleted != null)
            {
                this.FileOrDirectoryDeleted.Invoke(this, new DeletionEventArgs(source));
            }
        }

        /// <summary>
        /// Occurs when a new directory has been created
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreated;

        /// <summary>
        /// Raises the DirectoryCreated event
        /// </summary>
        /// <param name="source">The source directory</param>
        /// <param name="target">The target directory where the source directory has been created</param>
        protected virtual void OnDirectoryCreated(DirectoryInfo source, DirectoryInfo target)
        {
            if(this.DirectoryCreated != null)
            {
                this.DirectoryCreated.Invoke(this, new DirectoryCreationEventArgs(source, target));
            }
        }

        /// <summary>
        /// Occurs when a new file has been created
        /// </summary>
        public event EventHandler<FileCreationEventArgs> FileCreated;

        /// <summary>
        /// Raises the FileCreated event
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="target">The target directory where the source file has been copied to</param>
        protected virtual void OnFileCreated(FileInfo source, DirectoryInfo target)
        {
            if(this.FileCreated != null)
            {
                this.FileCreated.Invoke(this, new FileCreationEventArgs(source, target));
            }
        }

        /// <summary>
        /// Occurs when the creation of the sync infos has started
        /// </summary>
        public event EventHandler CreatingSyncInfosStarted;

        /// <summary>
        /// Raises the CreatingSyncInfosStarted event
        /// </summary>
        protected virtual void OnCreatingSyncInfosStarted()
        {
            if(this.CreatingSyncInfosStarted != null)
            {
                this.CreatingSyncInfosStarted.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the creation of the sync infos has finished
        /// </summary>
        public event EventHandler CreatingSyncInfosFinished;

        /// <summary>
        /// Raises the CreatingSyncInfosFinished event
        /// </summary>
        protected virtual void OnCreatingSyncInfosFinished()
        {
            if(this.CreatingSyncInfosFinished != null)
            {
                this.CreatingSyncInfosFinished.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the file synchronization has stared
        /// </summary>
        public event EventHandler SyncStarted;

        /// <summary>
        /// Raises the SyncStarted event
        /// </summary>
        protected virtual void OnSyncStarted()
        {
            if(this.SyncStarted != null)
            {
                this.SyncStarted.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the file synchronization has finished
        /// </summary>
        public event EventHandler SyncFinished;

        /// <summary>
        /// Raises the SyncFinished event
        /// </summary>
        protected virtual void OnSyncFinished()
        {
            if(this.SyncFinished != null)
            {
                this.SyncFinished.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Starts the job
        /// </summary>
        public void Start()
        {
            this.writtenBytes = 0;

            this.OnStarted();

            //Create sync infos
            this.OnCreatingSyncInfosStarted();
            this.syncInfos = (List<SyncInfo>)this.syncInfosCreator.CreateSyncInfos();
            this.OnCreatingSyncInfosFinished();

            //Start the sync
            this.OnSyncStarted();
            this.StartSync();
            this.OnSyncFinished();

            this.OnFinished();
        }

        public override string ToString()
        {
            return this.name;
        }
        #endregion

        #region Private methods
        private void StartSync()
        {
            foreach(SyncInfo syncInfo in this.syncInfos)
            {
                switch(syncInfo.SyncType)
                {
                    case SyncInfoType.FileCreation:
                        this.CopyFile(syncInfo, false);
                        break;

                    case SyncInfoType.DirectoryCreation:
                        Directory.CreateDirectory(Path.Combine(syncInfo.Target.FullName, syncInfo.Source.Name));
                        break;

                    case SyncInfoType.Deletion:
                        syncInfo.Target.Delete();
                        break;

                    case SyncInfoType.FileModification:
                        this.CopyFile(syncInfo, true);
                        break;
                }
            }
        }

        /// <summary>
        /// Copies the source file from the syncInfo to the target directory
        /// </summary>
        /// <param name="info">The sync info</param>
        /// <param name="overwrite">True for allowed overwriting, otherwise false</param>
        private void CopyFile(SyncInfo info, bool overwrite)
        {
            try
            {
                File.Copy(info.Source.FullName, Path.Combine(info.Target.FullName, info.Source.Name), overwrite);
            }

            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.writtenBytes += ((FileInfo)info.Source).Length;
        }
        #endregion
    }
}
