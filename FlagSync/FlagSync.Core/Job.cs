using System;
using System.IO;
using FlagLib.FileSystem;

namespace FlagSync.Core
{
    public abstract class Job
    {
        #region Fields

        FileCopyOperation fileCopyOperation = new FileCopyOperation();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the settings of the job.
        /// </summary>
        /// <value>The job settings.</value>
        public JobSetting Settings { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is paused.
        /// </summary>
        /// <value>true if paused; otherwise, false.</value>
        public bool Paused { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is previewed.
        /// </summary>
        /// <value>true if preview; otherwise, false.</value>
        public bool Preview { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is stopped.
        /// </summary>
        /// <value>true if stopped; otherwise, false.</value>
        public bool Stopped { get; private set; }

        /// <summary>
        /// Gets the written bytes.
        /// </summary>
        /// <value>The written bytes.</value>
        public long WrittenBytes { get; private set; }

        #endregion Properties

        #region Events

        /// <summary>
        /// Occurs when a file has been proceeded.
        /// </summary>
        public event EventHandler<FileProceededEventArgs> ProceededFile;

        /// <summary>
        /// Occurs when the job has finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs before a file gets deleted.
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> DeletingFile;

        /// <summary>
        /// Occurs when a file has been deleted.
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> DeletedFile;

        /// <summary>
        /// Occurs before a new file gets created.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> CreatingFile;

        /// <summary>
        /// Occurs when a new file has been created.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> CreatedFile;

        /// <summary>
        /// Occurs before a file gets modified.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> ModifyingFile;

        /// <summary>
        /// Occurs when a file has been modified.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> ModifiedFile;

        /// <summary>
        /// Occurs before a new directory gets created.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> CreatingDirectory;

        /// <summary>
        /// Occurs when a new directory has been created.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> CreatedDirectory;

        /// <summary>
        /// Occurs before a directory has been deleted.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DeletingDirectory;

        /// <summary>
        /// Occurs when a directory has been deleted.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DeletedDirectory;

        /// <summary>
        /// Occurs when a file copy error has occured.
        /// </summary>
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;

        /// <summary>
        /// Occurs when a file deletion error has occured.
        /// </summary>
        public event EventHandler<FileDeletionErrorEventArgs> FileDeletionError;

        /// <summary>
        /// Occurs when a directory deletion error has been catched.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        #endregion Events

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="preview">if set to true no files will be deleted, mofified or copied.</param>
        protected Job(JobSetting settings, bool preview)
        {
            fileCopyOperation.CopyProgressUpdated += new EventHandler<CopyProgressEventArgs>(fileCopyOperation_CopyProgressUpdated);
            this.Preview = preview;
            this.Settings = settings;
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
            this.Paused = true;
        }

        /// <summary>
        /// Continues the job (only after pause)
        /// </summary>
        public void Continue()
        {
            this.Paused = false;
        }

        /// <summary>
        /// Stops the job (can't be continued)
        /// </summary>
        public void Stop()
        {
            this.Paused = false;
            this.Stopped = true;
        }

        #endregion Public methods

        #region Protected methods

        /// <summary>
        /// Backups a directory and its sub folders
        /// </summary>
        /// <param name="sourceDirectory">The source directory</param>
        /// <param name="targetDirectory">The target directory</param>
        /// <param name="preview">True, if changes should get performed, otherwise false (if you want to see what will happen when you perform a backup)</param>
        protected void BackupDirectories(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, bool preview)
        {
            this.BackupDirectory(sourceDirectory, targetDirectory, preview);

            try
            {
                foreach (DirectoryInfo directory in sourceDirectory.GetDirectories())
                {
                    if (this.Stopped) { return; }

                    string targetSubDirectory = Path.Combine(targetDirectory.FullName, directory.Name);

                    if (!Directory.Exists(targetSubDirectory))
                    {
                        this.OnCreatingDirectory(new DirectoryCreationEventArgs(directory, targetDirectory));

                        if (!preview)
                        {
                            try
                            {
                                Directory.CreateDirectory(targetSubDirectory);
                                this.OnCreatedDirectory(new DirectoryCreationEventArgs(directory, targetDirectory));
                            }

                            catch (UnauthorizedAccessException e)
                            {
                                Logger.Instance.LogError("UnauthorizedAccessException at directory creation: " + targetSubDirectory);
                                Logger.Instance.LogError(e.Message);
                            }

                            catch (PathTooLongException e)
                            {
                                Logger.Instance.LogError("PathTooLongException at directory creation: " + targetSubDirectory);
                                Logger.Instance.LogError(e.Message);
                            }

                            catch (IOException e)
                            {
                                Logger.Instance.LogError("IOException at directory creation: " + targetSubDirectory);
                                Logger.Instance.LogError(e.Message);
                            }
                        }
                    }

                    this.BackupDirectories(directory, new DirectoryInfo(targetSubDirectory), preview);
                }
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + sourceDirectory.FullName);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatingFile(FileCopyEventArgs e)
        {
            if (this.CreatingFile != null)
            {
                this.CreatingFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatedFile(FileCopyEventArgs e)
        {
            if (this.CreatedFile != null)
            {
                this.CreatedFile(this, e);
            }

            Logger.Instance.LogSucceed("Created new file: " + e.File.Name + " in source: " + e.SourceDirectory.FullName + ", copied to target: " + e.TargetDirectory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:ModifyingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModifyingFile(FileCopyEventArgs e)
        {
            if (this.ModifyingFile != null)
            {
                this.ModifyingFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ModifiedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModifiedFile(FileCopyEventArgs e)
        {
            if (this.ModifiedFile != null)
            {
                this.ModifiedFile(this, e);
            }

            Logger.Instance.LogSucceed("Modified file: " + e.File.Name + " in source: " + e.SourceDirectory.FullName + ", copied to target: " + e.TargetDirectory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:DeletingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingFile(FileDeletionEventArgs e)
        {
            if (this.DeletingFile != null)
            {
                this.DeletingFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedFile(FileDeletionEventArgs e)
        {
            if (this.DeletedFile != null)
            {
                this.DeletedFile(this, e);
            }

            Logger.Instance.LogSucceed("Deleted file: " + e.File.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:CreatingDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatingDirectory(DirectoryCreationEventArgs e)
        {
            if (this.CreatingDirectory != null)
            {
                this.CreatingDirectory(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatedDirectory(DirectoryCreationEventArgs e)
        {
            if (this.CreatedDirectory != null)
            {
                this.CreatedDirectory(this, e);
            }

            Logger.Instance.LogSucceed("Created directory: " + e.Directory.Name + " in source: " + e.Directory.Parent.FullName + ", created in target: " + e.TargetDirectory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:DeletingDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingDirectory(DirectoryDeletionEventArgs e)
        {
            if (this.DeletingDirectory != null)
            {
                this.DeletingDirectory(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedDirectory(DirectoryDeletionEventArgs e)
        {
            if (this.DeletedDirectory != null)
            {
                this.DeletedDirectory(this, e);
            }

            Logger.Instance.LogSucceed("Deleted directory: " + e.Directory.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            if (this.Finished != null)
            {
                this.Finished(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ProceededFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        protected virtual void OnProceededFile(FileProceededEventArgs e)
        {
            if (this.ProceededFile != null)
            {
                this.ProceededFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileCopyError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileCopyError(FileCopyErrorEventArgs e)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError(this, e);
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
                this.DirectoryDeletionError(this, e);
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
                this.FileDeletionError(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileProgressChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagLib.FileSystem.CopyProgressEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileProgressChanged(CopyProgressEventArgs e)
        {
            if (this.FileCopyProgressChanged != null)
            {
                this.FileCopyProgressChanged(this, e);
            }
        }

        #endregion Protected methods

        #region Private methods

        /// <summary>
        /// Checks the if the job is paused. If true, a loop will be enabled, till the job gets continued
        /// </summary>
        private void CheckPause()
        {
            while (this.Paused)
            {
                System.Threading.Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Copyies a file to the specified directory
        /// </summary>
        /// <param name="file">File to copy</param>
        /// <param name="directory">Target directory</param>
        private void CopyFile(FileInfo file, DirectoryInfo directory)
        {
            this.CheckPause();

            //try
            {
                this.fileCopyOperation.CopyFile(file.FullName, Path.Combine(directory.FullName, file.Name));
            }

            /*
        catch (Exception e)
        {
            Logger.Instance.LogError("Exception at file copy: " + file.FullName);
            Logger.Instance.LogError(e.Message);

            throw;
        }
             * */
        }

        /// <summary>
        /// Handles the CopyProgressUpdated event of the fileCopyOperation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagLib.FileSystem.CopyProgressEventArgs"/> instance containing the event data.</param>
        private void fileCopyOperation_CopyProgressUpdated(object sender, CopyProgressEventArgs e)
        {
            this.OnFileProgressChanged(e);
        }

        /// <summary>
        /// Checks if file A is newer than file B
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <returns>
        /// True, if file A is newer, otherwise false
        /// </returns>
        private bool IsFileModified(FileInfo fileA, FileInfo fileB)
        {
            return fileA.LastWriteTime.CompareTo(fileB.LastWriteTime) > 0;
        }

        /// <summary>
        /// Backups a single directory, without sub folders
        /// </summary>
        /// <param name="sourceDirectory">The source directory</param>
        /// <param name="targetDirectory">The target directory</param>
        /// <param name="preview">True, if changes should get performed, otherwise false (if you want to see what will happen when you perform a backup)</param>
        private void BackupDirectory(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, bool preview)
        {
            this.CheckPause();

            try
            {
                foreach (FileInfo fileA in sourceDirectory.GetFiles())
                {
                    if (this.Stopped) { return; }

                    string fileBPath = Path.Combine(targetDirectory.FullName, fileA.Name);

                    //Check if fileA isn't already in target directory
                    if (!File.Exists(fileBPath))
                    {
                        this.OnCreatingFile(new FileCopyEventArgs(fileA, sourceDirectory, targetDirectory));

                        if (!preview)
                        {
                            try
                            {
                                this.CopyFile(fileA, targetDirectory);
                                this.OnCreatedFile(new FileCopyEventArgs(fileA, sourceDirectory, targetDirectory));
                            }

                            catch (IOException)
                            {
                                this.OnFileCopyError(new FileCopyErrorEventArgs(fileA, targetDirectory));
                            }
                        }
                    }

                    else
                    {
                        if (this.Stopped) { return; }
                        this.CheckPause();

                        FileInfo fileB = new FileInfo(fileBPath);

                        //Check for modified file
                        if (String.Equals(fileA.Name, fileB.Name, StringComparison.OrdinalIgnoreCase)
                            && this.IsFileModified(fileA, fileB))
                        {
                            this.OnModifyingFile(new FileCopyEventArgs(fileA, sourceDirectory, targetDirectory));

                            if (!preview)
                            {
                                try
                                {
                                    this.CopyFile(fileA, targetDirectory);
                                    this.OnModifiedFile(new FileCopyEventArgs(fileA, sourceDirectory, targetDirectory));
                                }

                                catch (IOException)
                                {
                                    this.OnFileCopyError(new FileCopyErrorEventArgs(fileA, targetDirectory));
                                }
                            }
                        }
                    }

                    this.OnProceededFile(new FileProceededEventArgs(fileA));
                }
            }

            catch (System.UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + sourceDirectory.FullName);
            }
        }

        #endregion Private methods
    }
}