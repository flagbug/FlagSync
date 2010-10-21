using System;
using System.Collections.Generic;
using System.Threading;

namespace FlagSync.Core
{
    public class JobWorker
    {
        /// <summary>
        /// Sync mode of a job
        /// </summary>
        public enum SyncMode
        {
            /// <summary>
            /// Backup mode
            /// </summary>
            Backup,
            /// <summary>
            /// Synchronization mode
            /// </summary>
            Synchronization
        }

        /// <summary>
        /// Occurs when the files had been counted.
        /// </summary>
        public event EventHandler FilesCounted;

        /// <summary>
        /// Occurs when the job worker has finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs when a file has been proceeded.
        /// </summary>
        public event EventHandler<FileProceededEventArgs> FileProceeded;

        /// <summary>
        /// Occurs when a newer file has been found.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> FoundNewerFile;

        /// <summary>
        /// Occurs when a file has been modified.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> FoundModifiedFile;

        /// <summary>
        /// Occurs when a file copy error has been catched.
        /// </summary>
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;

        /// <summary>
        /// Occurs when file has been deleted.
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> FileDeleted;

        /// <summary>
        /// Occurs when a directory has been created.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreated;

        /// <summary>
        /// Occurs when a directory has been deleted.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeleted;

        /// <summary>
        /// Occurs when directory deletion error has been catched.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;

        /// <summary>
        /// Occurs when a job has started.
        /// </summary>
        public event EventHandler<JobEventArgs> JobStarted;

        /// <summary>
        /// Occurs when a job has finished.
        /// </summary>
        public event EventHandler<JobEventArgs> JobFinished;

        /// <summary>
        /// Occurs when file deletion error has been catched.
        /// </summary>
        public event EventHandler<FileDeletionErrorEventArgs> FileDeletionError;

        private Job currentJob;
        private Queue<Job> jobQueue = new Queue<Job>();
        private long totalWrittenBytes;
        private int proceededFiles;
        private FileCounter.FileCounterResults fileCounterResult;
        private volatile bool paused;

        /// <summary>
        /// Gets the total written bytes.
        /// </summary>
        /// <value>The total written bytes.</value>
        public long TotalWrittenBytes
        {
            get
            {
                return this.totalWrittenBytes;
            }
        }

        /// <summary>
        /// Gets the proceeded files.
        /// </summary>
        /// <value>The proceeded files.</value>
        public int ProceededFiles
        {
            get
            {
                return this.proceededFiles;
            }
        }

        /// <summary>
        /// Gets the file counter result.
        /// </summary>
        /// <value>The file counter result.</value>
        public FileCounter.FileCounterResults FileCounterResult
        {
            get
            {
                return this.fileCounterResult;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JobWorker"/> is paused.
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
                this.paused = true;
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
                this.paused = false;
            }
        }

        /// <summary>
        /// Does the next job.
        /// </summary>
        private void DoNextJob()
        {
            if(this.jobQueue.Count > 0)
            {
                this.currentJob = this.jobQueue.Dequeue();
                this.InitializeEvents();
                this.OnJobStarted(new JobEventArgs(this.currentJob.Settings));
                this.currentJob.Start();   
            }

            else
            {
                this.OnFinished(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        private void Start()
        {
            Logger.Instance.LogStatusMessage("Start counting files");

            this.fileCounterResult = this.GetFileCounterResults();

            if(this.FilesCounted != null)
            {
                this.FilesCounted.Invoke(this, new EventArgs());
            }

            Logger.Instance.LogStatusMessage("Finished counting files");

            this.DoNextJob();
        }

        /// <summary>
        /// Starts the specified jobs.
        /// </summary>
        /// <param name="jobs">The jobs.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        public void Start(IEnumerable<JobSetting> jobs, bool preview)
        {
            this.totalWrittenBytes = 0;

            this.QueueJobs(jobs, preview);

            ThreadPool.QueueUserWorkItem(new WaitCallback(callback => this.Start()));
        }

        /// <summary>
        /// Queues the jobs.
        /// </summary>
        /// <param name="jobs">The jobs.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        private void QueueJobs(IEnumerable<JobSetting> jobs, bool preview)
        {
            foreach (JobSetting job in jobs)
            {
                switch (job.SyncMode)
                {
                    case SyncMode.Backup:
                        this.jobQueue.Enqueue(new BackupJob(job, preview));
                        break;

                    case SyncMode.Synchronization:
                        this.jobQueue.Enqueue(new SyncJob(job, preview));
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the file counter results.
        /// </summary>
        /// <returns></returns>
        private FileCounter.FileCounterResults GetFileCounterResults()
        {
            FileCounter.FileCounterResults result = new FileCounter.FileCounterResults();

            FileCounter counter = new FileCounter();

            foreach (Job job in this.jobQueue)
            {
                result += counter.CountJobFiles(job.Settings);
            }
            
            return result;
        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        private void InitializeEvents()
        {
            this.currentJob.DirectoryCreated += new EventHandler<DirectoryCreationEventArgs>(currentJob_DirectoryCreated);
            this.currentJob.DirectoryDeleted += new EventHandler<DirectoryDeletionEventArgs>(currentJob_DirectoryDeleted);
            this.currentJob.FileCopyError += new EventHandler<FileCopyErrorEventArgs>(currentJob_FileCopyError);
            this.currentJob.FileDeleted += new EventHandler<FileDeletionEventArgs>(currentJob_FileDeleted);
            this.currentJob.FileProceeded += new EventHandler<FileProceededEventArgs>(currentJob_FileProceeded);
            this.currentJob.Finished += new EventHandler(currentJob_Finished);
            this.currentJob.FoundModifiedFile += new EventHandler<FileCopyEventArgs>(currentJob_FoundModifiedFile);
            this.currentJob.FoundNewFile += new EventHandler<FileCopyEventArgs>(currentJob_FoundNewerFile);
            this.currentJob.DirectoryDeletionError += new EventHandler<DirectoryDeletionEventArgs>(currentJob_DirectoryDeletionError);
            this.currentJob.FileDeletionError += new EventHandler<FileDeletionErrorEventArgs>(currentJob_FileDeletionError);
        }

        /// <summary>
        /// Handles the FileDeletionError event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        void currentJob_FileDeletionError(object sender, FileDeletionErrorEventArgs e)
        {
            if (this.FileDeletionError != null)
            {
                this.FileDeletionError.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the DirectoryDeletionError event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        void currentJob_DirectoryDeletionError(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeletionError != null)
            {
                this.DirectoryDeletionError.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the FoundNewerFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        void currentJob_FoundNewerFile(object sender, FileCopyEventArgs e)
        {
            if (this.FoundNewerFile != null)
            {
                this.FoundNewerFile.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the FoundModifiedFile event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        void currentJob_FoundModifiedFile(object sender, FileCopyEventArgs e)
        {
            if (this.FoundModifiedFile != null)
            {
                this.FoundModifiedFile.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the Finished event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void currentJob_Finished(object sender, EventArgs e)
        {
            Job job = (Job)sender;

            this.OnJobFinished(new JobEventArgs(job.Settings));

            this.totalWrittenBytes += job.WrittenBytes;

            this.DoNextJob();
        }

        /// <summary>
        /// Handles the FileProceeded event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        void currentJob_FileProceeded(object sender, FileProceededEventArgs e)
        {
            this.proceededFiles++;

            if (this.FileProceeded != null)
            {
                this.FileProceeded.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the FileDeleted event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        void currentJob_FileDeleted(object sender, FileDeletionEventArgs e)
        {
            if (this.FileDeleted != null)
            {
                this.FileDeleted.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the FileCopyError event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        void currentJob_FileCopyError(object sender, FileCopyErrorEventArgs e)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the DirectoryDeleted event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        void currentJob_DirectoryDeleted(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeleted != null)
            {
                this.DirectoryDeleted.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the DirectoryCreated event of the currentJob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        void currentJob_DirectoryCreated(object sender, DirectoryCreationEventArgs e)
        {
            if (this.DirectoryCreated != null)
            {
                this.DirectoryCreated.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnFinished(EventArgs e)
        {
            if (this.Finished != null)
            {
                this.Finished.Invoke(this, e);
            }

            Logger.Instance.LogStatusMessage("Finished work");
        }

        /// <summary>
        /// Raises the <see cref="E:JobStarted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void OnJobStarted(JobEventArgs e)
        {
            if(this.JobStarted != null)
            {
                this.JobStarted.Invoke(this, e);
            }

            Logger.Instance.LogStatusMessage("Started job: " + e.Job.Name);
        }

        private void OnJobFinished(JobEventArgs e)
        {
            if(this.JobFinished != null)
            {
                this.JobFinished.Invoke(this, e);
            }

            Logger.Instance.LogStatusMessage("Finished job: " + e.Job.Name);
        }
    }
}
