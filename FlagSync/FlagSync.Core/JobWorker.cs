using System;
using System.Collections.Generic;
using System.Threading;

namespace FlagSync.Core
{
    public class JobWorker
    {
        public enum SyncMode
        {
            Backup,
            Sync
        }

        public event EventHandler FilesCounted;
        public event EventHandler Finished;
        public event EventHandler<FileProceededEventArgs> FileProceeded;
        public event EventHandler<FileCopyEventArgs> FoundNewerFile;
        public event EventHandler<FileCopyEventArgs> FoundModifiedFile;
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;
        public event EventHandler<FileDeletionEventArgs> FileDeleted;
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreated;
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeleted;
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;
        public event EventHandler<JobEventArgs> JobStarted;
        public event EventHandler<JobEventArgs> JobFinished;
        public event EventHandler<FileDeletionErrorEventArgs> FileDeletionError;

        private Job currentJob;

        private Queue<Job> jobQueue = new Queue<Job>();

        public long TotalWrittenBytes
        {
            get;
            private set;
        }

        public FileCounter.FileCounterResults FileCounterResult
        {
            get;
            private set;
        }

        private volatile bool paused;
        public bool Paused
        {
            get
            {
                return this.paused;
            }
        }

        public void Stop()
        {
            if (currentJob != null)
            {
                this.currentJob.Stop();
                this.jobQueue.Clear();
            }
        }

        public void Pause()
        {
            if (currentJob != null)
            {
                this.currentJob.Pause();
                this.paused = true;
            }
        }

        public void Continue()
        {
            if (currentJob != null)
            {
                this.currentJob.Continue();
                this.paused = false;
            }
        }

        private void DoNextJob()
        {
            if(this.jobQueue.Count > 0)
            {
                this.currentJob = this.jobQueue.Dequeue();
                this.InitEvents();
                this.OnJobStarted();
                this.currentJob.Start();   
            }

            else
            {
                this.OnFinished();
            }
        }

        private void Start()
        {
            Logger.Instance.LogStatusMessage("Start counting files");

            this.FileCounterResult = this.GetFileCounterResults();

            if(this.FilesCounted != null)
            {
                this.FilesCounted.Invoke(this, new EventArgs());
            }

            Logger.Instance.LogStatusMessage("Finished counting files");

            this.DoNextJob();
        }
        
        public void Start(IEnumerable<JobSettings> jobs, bool preview)
        {
            this.TotalWrittenBytes = 0;

            this.QueueJobs(jobs, preview);

            ThreadPool.QueueUserWorkItem(new WaitCallback(callback => this.Start()));
        }

        private void QueueJobs(IEnumerable<JobSettings> jobs, bool preview)
        {
            foreach (JobSettings job in jobs)
            {
                switch (job.SyncMode)
                {
                    case SyncMode.Backup:
                        this.jobQueue.Enqueue(new BackupJob(job, preview));
                        break;

                    case SyncMode.Sync:
                        this.jobQueue.Enqueue(new SyncJob(job, preview));
                        break;
                }
            }
        }

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

        private void InitEvents()
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

        void currentJob_FileDeletionError(object sender, FileDeletionErrorEventArgs e)
        {
            if (this.FileDeletionError != null)
            {
                this.FileDeletionError.Invoke(this, e);
            }
        }

        void currentJob_DirectoryDeletionError(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeletionError != null)
            {
                this.DirectoryDeletionError.Invoke(this, e);
            }
        }

        void currentJob_FoundNewerFile(object sender, FileCopyEventArgs e)
        {
            if (this.FoundNewerFile != null)
            {
                this.FoundNewerFile.Invoke(this, e);
            }
        }

        void currentJob_FoundModifiedFile(object sender, FileCopyEventArgs e)
        {
            if (this.FoundModifiedFile != null)
            {
                this.FoundModifiedFile.Invoke(this, e);
            }
        }

        void currentJob_Finished(object sender, EventArgs e)
        {
            Job job = (Job)sender;

            this.OnJobFinished(job.Settings);

            this.TotalWrittenBytes += job.WrittenBytes;

            this.DoNextJob();
        }

        void currentJob_FileProceeded(object sender, FileProceededEventArgs e)
        {
            if (this.FileProceeded != null)
            {
                this.FileProceeded.Invoke(this, e);
            }
        }

        void currentJob_FileDeleted(object sender, FileDeletionEventArgs e)
        {
            if (this.FileDeleted != null)
            {
                this.FileDeleted.Invoke(this, e);
            }
        }

        void currentJob_FileCopyError(object sender, FileCopyErrorEventArgs e)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError.Invoke(this, e);
            }
        }

        void currentJob_DirectoryDeleted(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeleted != null)
            {
                this.DirectoryDeleted.Invoke(this, e);
            }
        }

        void currentJob_DirectoryCreated(object sender, DirectoryCreationEventArgs e)
        {
            if (this.DirectoryCreated != null)
            {
                this.DirectoryCreated.Invoke(this, e);
            }
        }

        private void OnFinished()
        {
            if (this.Finished != null)
            {
                this.Finished.Invoke(this, new EventArgs());
            }

            Logger.Instance.LogStatusMessage("Finished work");
        }

        private void OnJobStarted()
        {
            if(this.JobStarted != null)
            {
                this.JobStarted.Invoke(this, new JobEventArgs(this.currentJob.Settings));
            }

            Logger.Instance.LogStatusMessage("Started job: " + this.currentJob.Settings.Name);
        }

        private void OnJobFinished(JobSettings job)
        {
            if(this.JobFinished != null)
            {
                this.JobFinished.Invoke(this, new JobEventArgs(job));
            }

            Logger.Instance.LogStatusMessage("Finished job: " + job.Name);
        }
    }
}
