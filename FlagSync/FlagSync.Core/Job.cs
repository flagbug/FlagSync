using System;
using System.IO;

namespace FlagSync.Core
{
    public abstract class Job
    {
        /// <summary>
        /// Gets invoked when a file was proceed
        /// </summary>
        public event EventHandler<FileProceededEventArgs> FileProceeded;

        /// <summary>
        /// Gets invoked when the job has finished
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Gets invoked after a file deletion
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> FileDeleted;

        /// <summary>
        /// Gets invoked when a new file is found
        /// </summary>
        public event EventHandler<FileCopyEventArgs> FoundNewFile;

        /// <summary>
        /// Gets invoked when a modified file is found
        /// </summary>
        public event EventHandler<FileCopyEventArgs> FoundModifiedFile;

        /// <summary>
        /// Gets invoked when an error while copying occurs
        /// </summary>
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;

        /// <summary>
        /// Gets invoked when an error while deleting occurs
        /// </summary>
        public event EventHandler<FileDeletionErrorEventArgs> FileDeletionError;

        /// <summary>
        /// Gets invoked after a folder creation
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreated;

        /// <summary>
        /// Gets invoked after a folder deletion
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeleted;

        /// <summary>
        /// Gets invoked when an error occurs while a directory gets deleted
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;

        private JobSettings settings = new JobSettings();
        public JobSettings Settings
        {
            get
            {
                return this.settings;
            }
        }

        private bool paused;
        public bool Paused
        {
            get
            {
                return this.paused;
            }
        }

        private bool preview;
        public bool Preview
        {
            get
            {
                return this.preview;
            }
        }

        private bool stopped;
        public bool Stopped
        {
            get
            {
                return this.stopped;
            }
        }

        private long writtenBytes;
        /// <summary>
        /// The bytes that the job has written
        /// </summary>
        public long WrittenBytes
        {
            get
            {
                return this.writtenBytes;
            }
        }

        public abstract void Start();

        protected Job(JobSettings settings, bool preview)
        {
            this.preview = preview;
            this.settings = settings;
        }

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
        /// <returns>True, if file copy has succeed, otherwise false</returns>
        protected bool CopyFile(FileInfo file, DirectoryInfo directory)
        {
            this.CheckPause();

            try
            {
                file.CopyTo(Path.Combine(directory.FullName, file.Name), true);
                this.writtenBytes += file.Length;
                return true;
            }

            catch (Exception e)
            {
                Logger.Instance.LogError("Exception at file copy: " + file.FullName);
                Logger.Instance.LogError(e.Message);

                this.OnFileCopyError(file, directory);

                return false;
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
                        if(preview)
                        {
                            this.OnNewDirectory(directory, target);
                        }

                        else
                        {
                            try
                            {
                                Directory.CreateDirectory(targetDirectory);
                                this.OnNewDirectory(directory, target);
                            }

                            catch(Exception e)
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
                    if(!File.Exists(Path.Combine(target.FullName, fileA.Name)))
                    {                        
                        if(preview)
                        {
                            this.OnFoundNewFile(fileA, source, target);
                        }

                        else
                        {
                            CopyFile(fileA, target);
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
                                    if(preview)
                                    {
                                        this.OnFoundModifiedFile(fileA, source, target);
                                    }

                                    else
                                    {
                                        CopyFile(fileA, target);
                                    }
                                }
                            }
                        }
                    }

                    this.OnFileProceeded(fileA);
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
        /// <returns>True, if file A is newer, otherwise false</returns>
        protected bool IsFileModified(FileInfo fileA, FileInfo fileB)
        {
            return fileA.LastWriteTime.CompareTo(fileB.LastWriteTime) > 0;
        }

        /// <summary>
        /// Raises the FoundNewFile event
        /// </summary>
        /// <param name="file">The file that got copied</param>
        /// <param name="sourceDirectory">The source directory of the file</param>
        /// <param name="targetDirectory">The target directory of the file</param>
        protected virtual void OnFoundNewFile(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            if (this.FoundNewFile != null)
            {
                this.FoundNewFile.Invoke(this, new FileCopyEventArgs(file, sourceDirectory, targetDirectory));
            }

            Logger.Instance.LogSucceed("Found new file: " + file.Name + " in source: " + sourceDirectory.FullName + ", copied to target: " + targetDirectory.FullName);
        }

        /// <summary>
        /// Raises the FoundModifiedFile event
        /// </summary>
        /// <param name="file">The file that got copied</param>
        /// <param name="sourceDirectory">The source directory of the file</param>
        /// <param name="targetDirectory">The target directory of the file</param>
        protected virtual void OnFoundModifiedFile(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            if (this.FoundModifiedFile != null)
            {
                this.FoundModifiedFile.Invoke(this, new FileCopyEventArgs(file, sourceDirectory, targetDirectory));
            }

            Logger.Instance.LogSucceed("Found modified file: " + file.Name + " in source: " + sourceDirectory.FullName + ", copied to target: " + targetDirectory.FullName);

        }

        /// <summary>
        /// Raises the NewDirectory event
        /// </summary>
        /// <param name="directory">The directory that gets created</param>
        /// <param name="targetDirectory">Target directoy</param>
        protected virtual void OnNewDirectory(DirectoryInfo directory, DirectoryInfo targetDirectory)
        {
            if (this.DirectoryCreated != null)
            {
                this.DirectoryCreated.Invoke(this, new DirectoryCreationEventArgs(directory, targetDirectory));
            }

            Logger.Instance.LogSucceed("Found new directory: " + directory.Name + " in source: " + directory.Parent.FullName + ", created in target: " + targetDirectory.FullName);

        }

        /// <summary>
        /// Raises the DeletedDirectory event
        /// </summary>
        /// <param name="directory">The directory that gets deleted</param>
        protected virtual void OnDeletedDirectory(DirectoryInfo directory)
        {
            if (this.DirectoryDeleted != null)
            {
                this.DirectoryDeleted.Invoke(this, new DirectoryDeletionEventArgs(directory));
            }

            Logger.Instance.LogSucceed("Deleted directory: " + directory.FullName);

        }

        /// <summary>
        /// Raises the FileCopyError event
        /// </summary>
        /// <param name="file">The file that should gets copied</param>
        /// <param name="targetDirectory">The directory where the file should get copied</param>
        protected virtual void OnFileCopyError(FileInfo file, DirectoryInfo targetDirectory)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError.Invoke(this, new FileCopyErrorEventArgs(file, targetDirectory));
            }
        }

        /// <summary>
        /// Raises the DeletedFile event
        /// </summary>
        /// <param name="file">The file that gets deleted</param>
        protected virtual void OnDeletedFile(FileInfo file)
        {
            if (this.FileDeleted != null)
            {
                this.FileDeleted.Invoke(this, new FileDeletionEventArgs(file));
            }

            Logger.Instance.LogSucceed("Deleted file: " + file.FullName);

        }

        /// <summary>
        /// Raises the Finished event
        /// </summary>
        protected virtual void OnFinished()
        {
            if (this.Finished != null)
            {
                this.Finished.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the FileProceeded event
        /// </summary>
        /// <param name="file">The file that gets proceeded</param>
        protected virtual void OnFileProceeded(FileInfo file)
        {
            if (this.FileProceeded != null)
            {
                this.FileProceeded.Invoke(this, new FileProceededEventArgs(file));
            }
        }

        /// <summary>
        /// Raises the DirectoryDeletionError event
        /// </summary>
        /// <param name="directory">The directory where the error occurs</param>
        protected virtual void OnDirectoryDeletionError(DirectoryInfo directory)
        {
            if (this.DirectoryDeletionError != null)
            {
                this.DirectoryDeletionError.Invoke(this, new DirectoryDeletionEventArgs(directory));
            }
        }

        /// <summary>
        /// Raises the FileDeletionError event
        /// </summary>
        /// <param name="file">The file where the error occurs</param>
        protected virtual void OnFileDeletionError(FileInfo file)
        {
            if (this.FileDeletionError != null)
            {
                this.FileDeletionError.Invoke(this, new FileDeletionErrorEventArgs(file));
            }
        }
    }
}
