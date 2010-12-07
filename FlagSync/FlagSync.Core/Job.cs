using System;
using System.IO;

namespace FlagSync.Core
{
    public abstract class Job
    {
        #region Members

        private JobSetting setting;
        private bool paused;
        private bool preview;
        private bool stopped;
        private long writtenBytes;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the job settings.
        /// </summary>
        /// <value>The job settings.</value>
        public JobSetting Settings
        {
            get
            {
                return this.setting;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Job"/> is paused.
        /// </summary>
        /// <value>true if paused; otherwise, false.</value>
        public bool Paused
        {
            get
            {
                return this.paused;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Job"/> is previewed.
        /// </summary>
        /// <value>true if preview; otherwise, false.</value>
        public bool Preview
        {
            get
            {
                return this.preview;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Job"/> is stopped.
        /// </summary>
        /// <value>true if stopped; otherwise, false.</value>
        public bool Stopped
        {
            get
            {
                return this.stopped;
            }
        }

        /// <summary>
        /// Gets the written bytes.
        /// </summary>
        /// <value>The written bytes.</value>
        public long WrittenBytes
        {
            get
            {
                return this.writtenBytes;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Occurs when a file has been proceeded.
        /// </summary>
        public event EventHandler<FileProceededEventArgs> FileProceeded;

        /// <summary>
        /// Occurs when the job has finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs when a file has been deleted.
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> FileDeleted;

        /// <summary>
        /// Occurs when a new file has been found.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> FoundNewFile;

        /// <summary>
        /// Occurs when a modified file has been found.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> FoundModifiedFile;

        /// <summary>
        /// Occurs when a file copy error has been catched.
        /// </summary>
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;

        /// <summary>
        /// Occurs when a file deletion error has been catched.
        /// </summary>
        public event EventHandler<FileDeletionErrorEventArgs> FileDeletionError;

        /// <summary>
        /// Occurs when a directory has been created.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreated;

        /// <summary>
        /// Occurs when a directory has been deleted.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeleted;

        /// <summary>
        /// Occurs when a directory deletion error has been catched.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;

        #endregion Events

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="preview">if set to true no files will be deleted, mofified or copied.</param>
        protected Job(JobSetting setting, bool preview)
        {
            this.preview = preview;
            this.setting = setting;
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Starts the job.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Pauses the job
        /// </summary>
        public void Pause()
        {
            this.paused = true;
        }

        /// <summary>
        /// Continues the job (only after pause)
        /// </summary>
        public void Continue()
        {
            this.paused = false;
        }

        /// <summary>
        /// Stops the job (can't be continued)
        /// </summary>
        public void Stop()
        {
            this.paused = false;
            this.stopped = true;
        }

        #endregion Public methods

        #region Protected methods

        /// <summary>
        /// Checks the if the job is paused. If true, a loop will be enabled, till the jab gets continued
        /// </summary>
        protected void CheckPause()
        {
            while (paused)
            {
                System.Threading.Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Copyies a file to the specified directory
        /// </summary>
        /// <param name="file">File to copy</param>
        /// <param name="directory">Target directory</param>
        protected void CopyFile(FileInfo file, DirectoryInfo directory)
        {
            this.CheckPause();

            try
            {
                file.CopyTo(Path.Combine(directory.FullName, file.Name), true);
                this.writtenBytes += file.Length;
            }

            catch (Exception e)
            {
                Logger.Instance.LogError("Exception at file copy: " + file.FullName);
                Logger.Instance.LogError(e.Message);

                this.OnFileCopyError(new FileCopyErrorEventArgs(file, directory));

                throw;
            }
        }

        /// <summary>
        /// Backups a directory and its sub folders
        /// </summary>
        /// <param name="source">The source directory</param>
        /// <param name="target">The target directory</param>
        /// <param name="preview">True, if changes should get performed, otherwise false (if you want to see what will happen when you perform a backup)</param>
        protected void BackupDirectories(DirectoryInfo source, DirectoryInfo target, bool preview)
        {
            this.BackupDirectory(source, target, preview);

            try
            {
                foreach (DirectoryInfo directory in source.GetDirectories())
                {
                    if (this.stopped)
                    {
                        return;
                    }

                    string targetDirectory = Path.Combine(target.FullName, directory.Name);

                    if (!Directory.Exists(targetDirectory))
                    {
                        if (preview)
                        {
                            this.OnNewDirectory(new DirectoryCreationEventArgs(directory, target));
                        }

                        else
                        {
                            try
                            {
                                Directory.CreateDirectory(targetDirectory);
                                this.OnNewDirectory(new DirectoryCreationEventArgs(directory, target));
                            }

                            catch (Exception e)
                            {
                                Logger.Instance.LogError("Exception at directory creation: " + targetDirectory);
                                Logger.Instance.LogError(e.Message);
                            }
                        }
                    }

                    this.BackupDirectories(directory, new DirectoryInfo(targetDirectory), preview);
                }
            }

            catch (System.UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + source.FullName);
            }
        }

        /// <summary>
        /// Checks if file A is newer than file B
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <returns>
        /// True, if file A is newer, otherwise false
        /// </returns>
        protected bool IsFileModified(FileInfo fileA, FileInfo fileB)
        {
            return fileA.LastWriteTime.CompareTo(fileB.LastWriteTime) > 0;
        }

        /// <summary>
        /// Raises the <see cref="E:FoundNewFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFoundNewFile(FileCopyEventArgs e)
        {
            if (this.FoundNewFile != null)
            {
                this.FoundNewFile.Invoke(this, e);
            }

            Logger.Instance.LogSucceed("Found new file: " + e.File.Name + " in source: " + e.SourceDirectory.FullName + ", copied to target: " + e.TargetDirectory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:FoundModifiedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFoundModifiedFile(FileCopyEventArgs e)
        {
            if (this.FoundModifiedFile != null)
            {
                this.FoundModifiedFile.Invoke(this, e);
            }

            Logger.Instance.LogSucceed("Found modified file: " + e.File.Name + " in source: " + e.SourceDirectory.FullName + ", copied to target: " + e.TargetDirectory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:NewDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnNewDirectory(DirectoryCreationEventArgs e)
        {
            if (this.DirectoryCreated != null)
            {
                this.DirectoryCreated.Invoke(this, e);
            }

            Logger.Instance.LogSucceed("Found new directory: " + e.Directory.Name + " in source: " + e.Directory.Parent.FullName + ", created in target: " + e.TargetDirectory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:DeletedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedDirectory(DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeleted != null)
            {
                this.DirectoryDeleted.Invoke(this, e);
            }

            Logger.Instance.LogSucceed("Deleted directory: " + e.Directory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:FileCopyError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileCopyError(FileCopyErrorEventArgs e)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletedFile"/> event.
        /// </summary>
        /// <param name="?">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedFile(FileDeletionEventArgs e)
        {
            if (this.FileDeleted != null)
            {
                this.FileDeleted.Invoke(this, e);
            }

            Logger.Instance.LogSucceed("Deleted file: " + e.File.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            if (this.Finished != null)
            {
                this.Finished.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileProceeded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileProceeded(FileProceededEventArgs e)
        {
            if (this.FileProceeded != null)
            {
                this.FileProceeded.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DirectoryDeletionError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryDeletionError(DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeletionError != null)
            {
                this.DirectoryDeletionError.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileDeletionError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileDeletionError(FileDeletionErrorEventArgs e)
        {
            if (this.FileDeletionError != null)
            {
                this.FileDeletionError.Invoke(this, e);
            }
        }

        #endregion Protected methods

        #region Private methods

        /// <summary>
        /// Backups a single directory, without sub folders
        /// </summary>
        /// <param name="source">The source directory</param>
        /// <param name="target">The target directory</param>
        /// <param name="preview">True, if changes should get performed, otherwise false (if you want to see what will happen when you perform a backup)</param>
        private void BackupDirectory(DirectoryInfo source, DirectoryInfo target, bool preview)
        {
            this.CheckPause();

            try
            {
                foreach (FileInfo fileA in source.GetFiles())
                {
                    if (this.stopped)
                    {
                        return;
                    }

                    //Check if fileA isn't already in target directory
                    if (!File.Exists(Path.Combine(target.FullName, fileA.Name)))
                    {
                        if (preview)
                        {
                            this.OnFoundNewFile(new FileCopyEventArgs(fileA, source, target));
                        }

                        else
                        {
                            try
                            {
                                CopyFile(fileA, target);
                                this.OnFoundNewFile(new FileCopyEventArgs(fileA, source, target));
                            }

                            catch (Exception)
                            {
                            }
                        }
                    }

                    if (target.Exists)
                    {
                        foreach (FileInfo fileB in target.GetFiles())
                        {
                            if (this.stopped)
                            {
                                return;
                            }

                            this.CheckPause();

                            //Check on modified file
                            if (fileA.Name.Equals(fileB.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                if (this.IsFileModified(fileA, fileB))
                                {
                                    if (preview)
                                    {
                                        this.OnFoundModifiedFile(new FileCopyEventArgs(fileA, source, target));
                                    }

                                    else
                                    {
                                        try
                                        {
                                            CopyFile(fileA, target);
                                            this.OnFoundModifiedFile(new FileCopyEventArgs(fileA, source, target));
                                        }

                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }

                    this.OnFileProceeded(new FileProceededEventArgs(fileA));
                }
            }

            catch (System.UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + source.FullName);
            }
        }

        #endregion Private methods
    }
}