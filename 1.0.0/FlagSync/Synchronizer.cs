using System;
using System.IO;
using System.Xml;

namespace FlagSync
{
    class Synchronizer
    {
        #region Variables
        public enum SyncMode
        {
            Backup,
            Sync
        }

        #region Events
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler FilesCounted;
        public event EventHandler FileProceeded;
        public event EventHandler Finished;
        public event EventHandler<FileCopyEventArgs> NewFile;
        public event EventHandler<FileCopyEventArgs> ModifiedFile;
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;
        public event EventHandler<FileDeletionEventArgs> FileDeletion;
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreated;
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeleted;
        #endregion

        private DirectoryInfo directoryA;
        private DirectoryInfo directoryB;

        private System.Threading.Thread syncThread;

        private bool preview;
        private bool stop;

        public int CountedFiles
        {
            get;
            private set;
        }

        public bool Pause
        {
            get;
            set;
        }

        public long WrittenBytes
        {
            get;
            private set;
        }
        #endregion

        #region EventArgs
        public class FileCopyEventArgs : EventArgs
        {
            public FileInfo File
            {
                get;
                private set;
            }

            public DirectoryInfo SourceDirectory
            {
                get;
                private set;
            }

            public DirectoryInfo TargetDirectory
            {
                get;
                private set;
            }

            public FileCopyEventArgs(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
            {
                this.File = file;
                this.SourceDirectory = sourceDirectory;
                this.TargetDirectory = targetDirectory;
            }
        }

        public class FileCopyErrorEventArgs : EventArgs
        {
            public FileInfo File
            {
                get;
                private set;
            }

            public DirectoryInfo TargetDirectory
            {
                get;
                private set;
            }

            public FileCopyErrorEventArgs(FileInfo file, DirectoryInfo targetDirectory)
            {
                this.File = file;
                this.TargetDirectory = targetDirectory;
            }
        }

        public class FileDeletionEventArgs : EventArgs
        {
            public FileInfo File
            {
                get;
                private set;
            }

            public FileDeletionEventArgs(FileInfo file)
            {
                this.File = file;
            }
        }

        public class DirectoryCreationEventArgs : EventArgs
        {
            public DirectoryInfo Directory
            {
                get;
                private set;
            }

            public DirectoryInfo TargetDirectory
            {
                get;
                private set;
            }

            public DirectoryCreationEventArgs(DirectoryInfo directory, DirectoryInfo targetDirectory)
            {
                this.Directory = directory;
                this.TargetDirectory = targetDirectory;
            }
        }

        public class DirectoryDeletionEventArgs : EventArgs
        {
            public DirectoryInfo Directory
            {
                get;
                private set;
            }

            public DirectoryDeletionEventArgs(DirectoryInfo directory)
            {
                this.Directory = directory;
            }
        }
        #endregion

        #region Constructor
        public Synchronizer()
        {

        }
        #endregion

        #region Methods
        private int CountAllFiles(DirectoryInfo root)
        {
            int files = 0;

            files += root.GetFiles().Length;

            try
            {
                foreach(DirectoryInfo directory in root.GetDirectories())
                {
                    files += CountAllFiles(directory);
                }
            }

            catch(UnauthorizedAccessException)
            {

            }

            return files;
        }

        private void CheckPause()
        {
            while(Pause)
            {
                System.Threading.Thread.Sleep(250);
            }
        }

        public void Stop()
        {
            this.Pause = false;
            this.stop = true;
        }

        private void CopyFile(FileInfo file, DirectoryInfo directory)
        {
            this.CheckPause();

            try
            {
                file.CopyTo(directory.FullName + "//" + file.Name, true);
                this.WrittenBytes += file.Length;
            }

            catch(Exception e)
            {
                this.LogFileCopyError(file, directory);
            }
        }

        private bool IsFileModified(FileInfo fileA, FileInfo fileB)
        {
            if(fileA.LastWriteTime.Year > fileB.LastWriteTime.Year &&
                fileA.LastWriteTime.Month > fileB.LastWriteTime.Month &&
                fileA.LastWriteTime.Day > fileB.LastWriteTime.Day &&
                fileA.LastWriteTime.Hour > fileB.LastWriteTime.Hour &&
                fileA.LastWriteTime.Minute > fileB.LastWriteTime.Minute &&
                fileA.LastWriteTime.Second > fileB.LastWriteTime.Second)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void Start(string directoryA, string directoryB, SyncMode syncMode, bool preview)
        {
            this.directoryA = new DirectoryInfo(directoryA);
            this.directoryB = new DirectoryInfo(directoryB);
            this.preview = preview;
            this.stop = false;
            this.WrittenBytes = 0;

            this.CountedFiles = this.CountAllFiles(this.directoryA);
            this.CountedFiles += this.CountAllFiles(this.directoryB);

            if(this.FilesCounted != null)
            {
                this.FilesCounted.Invoke(this, new EventArgs());
            }

            switch(syncMode)
            {
                case SyncMode.Sync:
                    syncThread = new System.Threading.Thread(this.Synchronize);
                    syncThread.Start();
                    break;

                case SyncMode.Backup:
                    syncThread = new System.Threading.Thread(this.Backup);
                    syncThread.Start();
                    break;
            }
        }
        #endregion

        #region Backup
        private void Backup()
        {
            this.BackupDirectories(this.directoryA, this.directoryB, this.preview);
            this.CheckDeletions(this.directoryB, this.directoryA, this.preview);

            if(this.Finished != null)
            {
                this.Finished.Invoke(this, new EventArgs());
            }
        }

        private void BackupDirectories(DirectoryInfo directoryA, DirectoryInfo directoryB, bool preview)
        {
            if(this.stop)
            {
                return;
            }

            this.BackupDirectory(directoryA, directoryB, preview);

            foreach(DirectoryInfo directory in directoryA.GetDirectories())
            {
                if(!Directory.Exists(directoryB.FullName + "\\" + directory.Name))
                {
                    this.LogNewDirectory(directory, directoryB);

                    if(!preview)
                    {
                        Directory.CreateDirectory(directoryB.FullName + "\\" + directory.Name);
                    }
                }

                this.BackupDirectories(directory, new DirectoryInfo(directoryB.FullName + "\\" + directory.Name), preview);
            }
        }

        private void BackupDirectory(DirectoryInfo directoryA, DirectoryInfo directoryB, bool preview)
        {
            this.CheckPause();

            foreach(FileInfo fileA in directoryA.GetFiles())
            {
                if(this.stop)
                {
                    return;
                }

                if(!File.Exists(directoryB.FullName + "\\" + fileA.Name))
                {
                    this.LogNewFile(fileA, directoryA, directoryB);

                    if(!preview)
                    {
                        CopyFile(fileA, directoryB);
                    }
                }

                if(directoryB.Exists)
                {
                    foreach(FileInfo fileB in directoryB.GetFiles())
                    {
                        if(this.stop)
                        {
                            return;
                        }

                        this.CheckPause();

                        //Check on modified file
                        if(fileA.Name == fileB.Name)
                        {
                            if(this.IsFileModified(fileA, fileB))
                            {
                                this.LogModifiedFile(fileA, directoryA, directoryB);

                                if(!preview)
                                {
                                    CopyFile(fileA, directoryB);
                                }
                            }
                        }
                    }
                }
            
                if(this.FileProceeded != null)
                {
                    this.FileProceeded.Invoke(this, new EventArgs());
                }
            }
        }

        private void CheckDeletions(DirectoryInfo directoryA, DirectoryInfo directoryB, bool preview)
        {
            if(this.stop)
            {
                return;
            }

            foreach(FileInfo file in directoryA.GetFiles())
            {
                if(!File.Exists(directoryB.FullName + "\\" + file.Name))
                {
                    this.LogDeletedFile(file);

                    if(!preview)
                    {
                        file.Delete();
                    }
                }

                if(this.FileProceeded != null)
                {
                    this.FileProceeded.Invoke(this, new EventArgs());
                }
            }

            foreach(DirectoryInfo directory in directoryA.GetDirectories())
            {
                if(!Directory.Exists(directoryB.FullName + "\\" + directory.Name))
                {
                    this.LogDeletedDirectory(directory);

                    if(!preview)
                    {
                        directory.Delete(true);
                    }
                }

                else if(Directory.Exists(directoryB.FullName + "\\" + directory.Name))
                {
                    this.CheckDeletions(directory, new DirectoryInfo(directoryB.FullName + "\\" + directory.Name), preview);
                }
            }
        }
        #endregion

        #region Sync
        private void Synchronize()
        {
            SyncAllDirectories(this.directoryA, this.directoryB, this.preview);

            SyncAllDirectories(this.directoryB, this.directoryA, this.preview);

            if(this.Finished != null)
            {
                this.Finished.Invoke(this, new EventArgs());
            }
        }

        private void SyncAllDirectories(DirectoryInfo directoryA, DirectoryInfo directoryB, bool preview)
        {
            if(this.stop)
            {
                return;
            }

            this.SyncDirectory(directoryA, directoryB, preview);

            foreach(DirectoryInfo directory in directoryA.GetDirectories())
            {
                if(!Directory.Exists(directoryB.FullName + "\\" + directory.Name))
                {
                    this.LogNewDirectory(directory, directoryB);

                    if(!preview)
                    {
                        Directory.CreateDirectory(directoryB.FullName + "\\" + directory.Name);
                    }
                }

                this.SyncAllDirectories(directory, new DirectoryInfo(directoryB.FullName + "\\" + directory.Name), preview);
            }
        }

        private void SyncDirectory(DirectoryInfo directoryA, DirectoryInfo directoryB, bool preview)
        {
            this.CheckPause();
            
            foreach(FileInfo fileA in directoryA.GetFiles())
            {
                if(this.stop)
                {
                    return;
                }

                if(!File.Exists(directoryB.FullName + "\\" + fileA.Name))
                {
                    this.LogNewFile(fileA, directoryA, directoryB);

                    if(!preview)
                    {
                        CopyFile(fileA, directoryB);
                    }
                }

                if(directoryB.Exists)
                {
                    foreach(FileInfo fileB in directoryB.GetFiles())
                    {
                        if(this.stop)
                        {
                            return;
                        }

                        this.CheckPause();

                        if(fileA.Name == fileB.Name)
                        {
                            if(this.IsFileModified(fileA, fileB))
                            {
                                this.LogModifiedFile(fileA, directoryA, directoryB);

                                if(!preview)
                                {
                                    CopyFile(fileA, directoryB);
                                }
                            }
                        }
                    }
                }

                if(this.FileProceeded != null)
                {
                    this.FileProceeded.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion

        #region Logs
        private void LogNewFile(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
           if(this.NewFile != null)
           {
               this.NewFile.Invoke(this, new FileCopyEventArgs(file, sourceDirectory, targetDirectory));
           }
        }

        private void LogModifiedFile(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            if(this.ModifiedFile != null)
            {
                this.ModifiedFile.Invoke(this, new FileCopyEventArgs(file, sourceDirectory, targetDirectory));
            }
        }

        private void LogDeletedFile(FileInfo file)
        {
            if(this.FileDeletion != null)
            {
                this.FileDeletion.Invoke(this, new FileDeletionEventArgs(file));
            }
        }

        private void LogNewDirectory(DirectoryInfo directory, DirectoryInfo targetDirectory)
        {
            if(this.DirectoryCreated != null)
            {
                this.DirectoryCreated.Invoke(this, new DirectoryCreationEventArgs(directory, targetDirectory));
            }
        }

        private void LogDeletedDirectory(DirectoryInfo directory)
        {
            if(this.DirectoryDeleted != null)
            {
                this.DirectoryDeleted.Invoke(this, new DirectoryDeletionEventArgs(directory));
            }
        }

        private void LogFileCopyError(FileInfo file, DirectoryInfo targetDirectory)
        {
            if(this.FileCopyError != null)
            {
                this.FileCopyError.Invoke(this, new FileCopyErrorEventArgs(file, targetDirectory));
            }
        }
        #endregion
    }
}
