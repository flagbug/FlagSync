using System;
using System.IO;
using System.Security;
using FlagLib.FileSystem;

namespace FlagSync.Core
{
    /// <summary>
    /// A backup-job performs a synchronization only from directory A to directory B, but can check on deleted files
    /// </summary>
    public class BackupJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="preview">if set to <c>true</c>, no files will be deleted, copied or modified).</param>
        public BackupJob(JobSetting settings, bool preview)
            : base(settings, preview)
        {
        }

        /// <summary>
        /// Starts the BackupJob, copies new and modified files from directory A to directory B and finally checks for deletions
        /// </summary>*/
        public override void Start()
        {
            //Backup directoryA to directoryB and then check for deletions
            this.BackupDirectories(new DirectoryInfo(this.Settings.DirectoryA), new DirectoryInfo(this.Settings.DirectoryB), this.Preview);
            this.CheckDeletions(new DirectoryInfo(this.Settings.DirectoryB), new DirectoryInfo(this.Settings.DirectoryA), this.Preview);

            this.OnFinished(EventArgs.Empty);
        }

        /// <summary>
        /// Checks for deleted files in directory B, which aren't in directory A
        /// </summary>
        /// <param name="sourceDirectory">The source directory</param>
        /// <param name="targetDirectory">The target directory</param>
        /// <param name="preview">True, if you want to see what will happen when you perform a backup)</param>
        private void CheckDeletions(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, bool preview)
        {
            if (this.Stopped) { return; }

            this.CheckFileDeletions(sourceDirectory, targetDirectory, preview);

            this.CheckDirectoryDeletions(sourceDirectory, targetDirectory, preview);
        }

        /// <summary>
        /// Checks the directory recursively for directories that are not in the source directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="preview">if set to <c>true</c> a preview will be performed.</param>
        private void CheckDirectoryDeletions(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, bool preview)
        {
            try
            {
                foreach (DirectoryInfo directory in sourceDirectory.GetDirectories())
                {
                    if (!Directory.Exists(Path.Combine(targetDirectory.FullName, directory.Name)))
                    {
                        //Set all files in the directory and it's subdirectories as proceeded,
                        //because the whole directory gets deleted
                        DirectoryScanner scanner = new DirectoryScanner(directory.FullName);

                        scanner.FileFound += (sender, e) =>
                            {
                                this.OnProceededFile(new FileProceededEventArgs(e.File));
                            };

                        scanner.Start();

                        this.OnDeletingDirectory(new DirectoryDeletionEventArgs(directory));

                        if (!preview)
                        {
                            try
                            {
                                directory.Delete(true);
                                this.OnDeletedDirectory(new DirectoryDeletionEventArgs(directory));
                            }

                            catch (IOException)
                            {
                                Logger.Instance.LogError("IOException at directory deletion: " + directory.FullName);
                                this.OnDirectoryDeletionError(new DirectoryDeletionEventArgs(directory));
                            }

                            catch (SecurityException)
                            {
                                Logger.Instance.LogError("SecurityException at directory deletion: " + directory.FullName);
                                this.OnDirectoryDeletionError(new DirectoryDeletionEventArgs(directory));
                            }

                            catch (UnauthorizedAccessException)
                            {
                                Logger.Instance.LogError("UnauthorizedAccessException at directory deletion: " + directory.FullName);
                                this.OnDirectoryDeletionError(new DirectoryDeletionEventArgs(directory));
                            }
                        }
                    }

                    else
                    {
                        this.CheckDeletions(directory, new DirectoryInfo(Path.Combine(targetDirectory.FullName, directory.Name)), preview);
                    }
                }
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + sourceDirectory.FullName);
            }
        }

        /// <summary>
        /// Checks a directory for files that are not in the source directory and deletes them.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="preview">if set to <c>true</c> a preview will be performed.</param>
        private void CheckFileDeletions(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, bool preview)
        {
            try
            {
                foreach (FileInfo file in sourceDirectory.GetFiles())
                {
                    if (!File.Exists(Path.Combine(targetDirectory.FullName, file.Name)))
                    {
                        this.OnDeletingFile(new FileDeletionEventArgs(file));

                        if (!preview)
                        {
                            try
                            {
                                file.Delete();
                                this.OnDeletedFile(new FileDeletionEventArgs(file));
                            }

                            catch (IOException)
                            {
                                Logger.Instance.LogError("IOException at file deletion: " + file.FullName);
                                this.OnFileDeletionError(new FileDeletionErrorEventArgs(file));
                            }

                            catch (SecurityException)
                            {
                                Logger.Instance.LogError("SecurityException at file deletion: " + file.FullName);
                                this.OnFileDeletionError(new FileDeletionErrorEventArgs(file));
                            }

                            catch (UnauthorizedAccessException)
                            {
                                Logger.Instance.LogError("UnauthorizedAccessException at file deletion: " + file.FullName);
                                this.OnFileDeletionError(new FileDeletionErrorEventArgs(file));
                            }
                        }
                    }

                    this.OnProceededFile(new FileProceededEventArgs(file));
                }
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + sourceDirectory.FullName);
            }
        }
    }
}