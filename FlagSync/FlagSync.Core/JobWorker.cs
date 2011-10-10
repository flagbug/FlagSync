using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using FlagLib.IO;
using FlagSync.Core.FileSystem;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Ftp;
using FlagSync.Core.FileSystem.ITunes;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    public sealed class JobWorker
    {
        /// <summary>
        /// Occurs when a file has been proceeded.
        /// </summary>
        public event EventHandler<FileProceededEventArgs> ProceededFile;

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
        public event EventHandler<DataTransferEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Occurs when the job worker has finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs when the files had been counted.
        /// </summary>
        public event EventHandler FilesCounted;

        /// <summary>
        /// Occurs when a job has started.
        /// </summary>
        public event EventHandler<JobEventArgs> JobStarted;

        /// <summary>
        /// Occurs when a job has finished.
        /// </summary>
        public event EventHandler<JobEventArgs> JobFinished;

        private Job currentJob;
        private Queue<Job> jobQueue = new Queue<Job>();
        private long totalWrittenBytes;
        private int proceededFiles;
        private FileCounterResult fileCounterResult;
        private bool performPreview;

        /// <summary>
        /// Gets the total written bytes.
        /// </summary>
        /// <value>The total written bytes.</value>
        public long TotalWrittenBytes
        {
            get { return this.totalWrittenBytes; }
        }

        /// <summary>
        /// Gets the proceeded files.
        /// </summary>
        /// <value>The proceeded files.</value>
        public int ProceededFiles
        {
            get { return this.proceededFiles; }
        }

        /// <summary>
        /// Gets the file counter result.
        /// </summary>
        /// <value>The file counter result.</value>
        public FileCounterResult FileCounterResult
        {
            get { return this.fileCounterResult; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JobWorker"/> is paused.
        /// </summary>
        /// <value>true if paused; otherwise, false.</value>
        public bool IsPaused
        {
            get { return this.currentJob == null ? false : this.currentJob.IsPaused; }
        }

        /// <summary>
        /// Stops the job worker.
        /// </summary>
        public void Stop()
        {
            if (currentJob != null)
            {
                this.currentJob.Stop();
                this.jobQueue.Clear();
            }
        }

        /// <summary>
        /// Pauses the job worker.
        /// </summary>
        public void Pause()
        {
            if (currentJob != null)
            {
                this.currentJob.Pause();
            }
        }

        /// <summary>
        /// Continues the job worker.
        /// </summary>
        public void Continue()
        {
            if (currentJob != null)
            {
                this.currentJob.Continue();
            }
        }

        /// <summary>
        /// Starts the specified jobs.
        /// </summary>
        /// <param name="jobSettings">The job settings.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        public void Start(IEnumerable<Job> jobs, bool preview)
        {
            this.totalWrittenBytes = 0;

            foreach (Job job in jobs)
            {
                this.jobQueue.Enqueue(job);
            }

            ThreadPool.QueueUserWorkItem(callback => this.Start(preview));
        }

        /// <summary>
        /// Does the next job.
        /// </summary>
        private void DoNextJob()
        {
            if (this.jobQueue.Count > 0)
            {
                this.currentJob = this.jobQueue.Dequeue();
                this.InitializeJobEvents(this.currentJob);
                this.OnJobStarted(new JobEventArgs(this.currentJob.Settings));
                this.currentJob.Start(this.performPreview);
            }

            else
            {
                this.OnFinished(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        private void Start(bool preview)
        {
            this.performPreview = preview;

            Logger.Current.LogStatusMessage("Start counting files");

            this.fileCounterResult = this.GetFileCounterResults();

            if (this.FilesCounted != null)
            {
                this.FilesCounted.Invoke(this, new EventArgs());
            }

            Logger.Current.LogStatusMessage("Finished counting files");

            this.DoNextJob();
        }

        /// <summary>
        /// Queues the jobs.
        /// </summary>
        /// <param name="jobs">The jobs.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        private void QueueJobs(IEnumerable<JobSetting> jobs)
        {
            foreach (JobSetting jobSetting in jobs)
            {
                switch (jobSetting.SyncMode)
                {
                    case SyncMode.LocalBackup:
                        this.jobQueue.Enqueue(new LocalBackupJob(jobSetting));
                        break;

                    case SyncMode.LocalSynchronization:
                        this.jobQueue.Enqueue(new LocalSyncJob(jobSetting));
                        break;

                    case SyncMode.ITunes:
                        this.jobQueue.Enqueue(new ITunesJob(jobSetting));
                        break;

                    case SyncMode.FtpBackup:
                        this.jobQueue.Enqueue(new FtpBackupJob(jobSetting));
                        break;

                    case SyncMode.FtpSynchronization:
                        this.jobQueue.Enqueue(new FtpSyncJob(jobSetting));
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the file counter results.
        /// </summary>
        /// <returns></returns>
        private FileCounterResult GetFileCounterResults()
        {
            FileCounterResult result = new FileCounterResult();

            foreach (Job job in this.jobQueue)
            {
                //TODO: Make generic for all settings

                IDirectoryInfo directoryA = null;
                IDirectoryInfo directoryB = null;

                switch (job.Settings.SyncMode)
                {
                    case SyncMode.LocalBackup:
                    case SyncMode.LocalSynchronization:
                        directoryA = new LocalDirectoryInfo(new DirectoryInfo(job.Settings.DirectoryA));
                        directoryB = new LocalDirectoryInfo(new DirectoryInfo(job.Settings.DirectoryB));
                        break;

                    case SyncMode.ITunes:
                        directoryA = new ITunesDirectoryInfo(job.Settings.ITunesPlaylist);
                        directoryB = new LocalDirectoryInfo(new DirectoryInfo(job.Settings.DirectoryB));
                        break;

                    case SyncMode.FtpBackup:
                    case SyncMode.FtpSynchronization:
                        directoryA = new LocalDirectoryInfo(new DirectoryInfo(job.Settings.DirectoryB));
                        directoryB = new FtpDirectoryInfo(job.Settings.FtpAddress,
                            new FlagFtp.FtpClient(new NetworkCredential(job.Settings.FtpUserName, job.Settings.FtpPassword)));
                        break;
                }

                result += FileCounter.CountFiles(directoryA);
                result += FileCounter.CountFiles(directoryB);
            }

            return result;
        }

        /// <summary>
        /// Initializes the  job events.
        /// </summary>
        /// <param name="job">The job.</param>
        private void InitializeJobEvents(Job job)
        {
            job.CreatedDirectory += new EventHandler<DirectoryCreationEventArgs>(currentJob_CreatedDirectory);
            job.CreatedFile += new EventHandler<FileCopyEventArgs>(currentJob_CreatedFile);
            job.CreatingDirectory += new EventHandler<DirectoryCreationEventArgs>(currentJob_CreatingDirectory);
            job.CreatingFile += new EventHandler<FileCopyEventArgs>(currentJob_CreatingFile);
            job.DeletedDirectory += new EventHandler<DirectoryDeletionEventArgs>(currentJob_DeletedDirectory);
            job.DeletedFile += new EventHandler<FileDeletionEventArgs>(currentJob_DeletedFile);
            job.DeletingDirectory += new EventHandler<DirectoryDeletionEventArgs>(currentJob_DeletingDirectory);
            job.DeletingFile += new EventHandler<FileDeletionEventArgs>(currentJob_DeletingFile);
            job.DirectoryDeletionError += new EventHandler<DirectoryDeletionEventArgs>(currentJob_DirectoryDeletionError);
            job.FileCopyError += new EventHandler<FileCopyErrorEventArgs>(currentJob_FileCopyError);
            job.FileCopyProgressChanged += new EventHandler<DataTransferEventArgs>(currentJob_FileCopyProgressChanged);
            job.FileDeletionError += new EventHandler<FileDeletionErrorEventArgs>(currentJob_FileDeletionError);
            job.Finished += new EventHandler(currentJob_Finished);
            job.ModifiedFile += new EventHandler<FileCopyEventArgs>(currentJob_ModifiedFile);
            job.ModifyingFile += new EventHandler<FileCopyEventArgs>(currentJob_ModifyingFile);
            job.ProceededFile += new EventHandler<FileProceededEventArgs>(currentJob_ProceededFile);
        }

        /// <summary>
        /// Handles the ProceededFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        private void currentJob_ProceededFile(object sender, FileProceededEventArgs e)
        {
            if (this.ProceededFile != null)
            {
                this.ProceededFile(this, e);
            }

            this.proceededFiles++;
        }

        /// <summary>
        /// Handles the ModifyingFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void currentJob_ModifyingFile(object sender, FileCopyEventArgs e)
        {
            if (this.ModifyingFile != null)
            {
                this.ModifyingFile(this, e);
            }
        }

        /// <summary>
        /// Handles the ModifiedFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void currentJob_ModifiedFile(object sender, FileCopyEventArgs e)
        {
            if (this.ModifiedFile != null)
            {
                this.ModifiedFile(this, e);
            }
        }

        /// <summary>
        /// Handles the FileDeletionError event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        private void currentJob_FileDeletionError(object sender, FileDeletionErrorEventArgs e)
        {
            if (this.FileDeletionError != null)
            {
                this.FileDeletionError(this, e);
            }
        }

        /// <summary>
        /// Handles the FileCopyProgressChanged event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagLib.IO.DataTransferEventArgs"/> instance containing the event data.</param>
        private void currentJob_FileCopyProgressChanged(object sender, DataTransferEventArgs e)
        {
            if (this.FileCopyProgressChanged != null)
            {
                this.FileCopyProgressChanged(this, e);
            }
        }

        /// <summary>
        /// Handles the FileCopyError event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        private void currentJob_FileCopyError(object sender, FileCopyErrorEventArgs e)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError(this, e);
            }
        }

        /// <summary>
        /// Handles the DirectoryDeletionError event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        private void currentJob_DirectoryDeletionError(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeletionError != null)
            {
                this.DirectoryDeletionError(this, e);
            }
        }

        /// <summary>
        /// Handles the DeletingFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        private void currentJob_DeletingFile(object sender, FileDeletionEventArgs e)
        {
            if (this.DeletingFile != null)
            {
                this.DeletingFile(this, e);
            }
        }

        /// <summary>
        /// Handles the DeletingDirectory event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        private void currentJob_DeletingDirectory(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DeletingDirectory != null)
            {
                this.DeletingDirectory(this, e);
            }
        }

        /// <summary>
        /// Handles the DeletedFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        private void currentJob_DeletedFile(object sender, FileDeletionEventArgs e)
        {
            if (this.DeletedFile != null)
            {
                this.DeletedFile(this, e);
            }
        }

        /// <summary>
        /// Handles the DeletedDirectory event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        private void currentJob_DeletedDirectory(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DeletedDirectory != null)
            {
                this.DeletedDirectory(this, e);
            }
        }

        /// <summary>
        /// Handles the CreatingFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void currentJob_CreatingFile(object sender, FileCopyEventArgs e)
        {
            if (this.CreatingFile != null)
            {
                this.CreatingFile(this, e);
            }
        }

        /// <summary>
        /// Handles the CreatingDirectory event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        private void currentJob_CreatingDirectory(object sender, DirectoryCreationEventArgs e)
        {
            if (this.CreatingDirectory != null)
            {
                this.CreatingDirectory(this, e);
            }
        }

        /// <summary>
        /// Handles the CreatedFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void currentJob_CreatedFile(object sender, FileCopyEventArgs e)
        {
            if (this.CreatedFile != null)
            {
                this.CreatedFile(this, e);
            }
        }

        /// <summary>
        /// Handles the CreatedDirectory event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        private void currentJob_CreatedDirectory(object sender, DirectoryCreationEventArgs e)
        {
            if (this.CreatedDirectory != null)
            {
                this.CreatedDirectory(this, e);
            }
        }

        /// <summary>
        /// Handles the Finished event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void currentJob_Finished(object sender, EventArgs e)
        {
            Job job = (Job)sender;

            this.OnJobFinished(new JobEventArgs(job.Settings));

            this.totalWrittenBytes += job.WrittenBytes;

            this.DoNextJob();
        }

        /// <summary>
        /// Raises the <see cref="E:Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnFinished(EventArgs e)
        {
            if (this.Finished != null)
            {
                this.Finished(this, e);
            }

            Logger.Current.LogStatusMessage("Finished work");
        }

        /// <summary>
        /// Raises the <see cref="E:JobStarted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void OnJobStarted(JobEventArgs e)
        {
            if (this.JobStarted != null)
            {
                this.JobStarted(this, e);
            }

            Logger.Current.LogStatusMessage("Started job: " + e.Job.Name);
        }

        /// <summary>
        /// Raises the <see cref="E:JobFinished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void OnJobFinished(JobEventArgs e)
        {
            if (this.JobFinished != null)
            {
                this.JobFinished(this, e);
            }

            Logger.Current.LogStatusMessage("Finished job: " + e.Job.Name);
        }
    }
}