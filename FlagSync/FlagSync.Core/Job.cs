using System;
using System.Threading;
using FlagLib.FileSystem;
using FlagSync.Core.AbstractFileSystem;

namespace FlagSync.Core
{
    internal abstract class Job
    {
        private volatile bool isPaused;
        private volatile bool isStopped;

        /// <summary>
        /// Gets the settings of the job.
        /// </summary>
        /// <value>The job settings.</value>
        public JobSetting Settings { get; private set; }

        public IFileSystem FileSystem { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is paused.
        /// </summary>
        /// <value>true if paused; otherwise, false.</value>
        public bool IsPaused
        {
            get { return this.isPaused; }
            private set { this.isPaused = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is stopped.
        /// </summary>
        /// <value>true if stopped; otherwise, false.</value>
        public bool IsStopped
        {
            get { return this.isStopped; }
            private set { this.isStopped = value; }
        }

        /// <summary>
        /// Gets the written bytes.
        /// </summary>
        /// <value>The written bytes.</value>
        public long WrittenBytes { get; private set; }

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
        /// Occurs when a directory creation error has occured.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreationError;

        /// <summary>
        /// Occurs when a directory deletion error has occured.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileSystem">The file system.</param>
        protected Job(JobSetting settings, IFileSystem fileSystem)
        {
            this.Settings = settings;
            this.FileSystem = fileSystem;
        }

        /// <summary>
        /// Starts the job.
        /// </summary>
        public abstract void Start(bool preview);

        /// <summary>
        /// Pauses the job
        /// </summary>
        public virtual void Pause()
        {
            this.IsPaused = true;
        }

        /// <summary>
        /// Continues the job (only after pause)
        /// </summary>
        public virtual void Continue()
        {
            this.IsPaused = false;
        }

        /// <summary>
        /// Stops the job (can't be continued)
        /// </summary>
        public virtual void Stop()
        {
            this.IsPaused = false;
            this.IsStopped = true;
        }

        /// <summary>
        /// Checks if the job is paused, when true,
        /// a loop will be enabled which causes the thread to sleep till the job gets continued
        /// </summary>
        protected void CheckPause()
        {
            while (this.IsPaused)
            {
                Thread.Sleep(500);
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

            Logger.Current.LogSucceed("Created new file: " + e.File.Name + " in source: " + e.SourceDirectory.FullName + ", copied to target: " + e.TargetDirectory.FullName);
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

            Logger.Current.LogSucceed("Modified file: " + e.File.Name + " in source: " + e.SourceDirectory.FullName + ", copied to target: " + e.TargetDirectory.FullName);
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

            Logger.Current.LogSucceed("Deleted file: " + e.FilePath);
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

            Logger.Current.LogSucceed("Created directory: " + e.Directory.Name + " in source: " + e.Directory.Parent.FullName + ", created in target: " + e.TargetDirectory.FullName);
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

            Logger.Current.LogSucceed("Deleted directory: " + e.DirectoryPath);
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

        /// <summary>
        /// Raises the <see cref="E:DirectoryCreationError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryCreationError(DirectoryCreationEventArgs e)
        {
            if (this.DirectoryCreationError != null)
            {
                this.DirectoryCreationError(this, e);
            }
        }
    }
}