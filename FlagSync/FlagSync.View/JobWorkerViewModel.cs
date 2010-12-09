using System;
using System.Collections.Generic;
using System.IO;
using FlagLib.Collections;
using FlagLib.Patterns;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobWorkerViewModel : ViewModelBase<JobWorkerViewModel>
    {
        #region Members

        private JobWorker jobWorker;
        private JobSetting currentJobSetting;
        private long countedBytes;
        private long proceededBytes;
        private int countedFiles;
        private int proceededFiles;
        private bool isCounting;
        private string statusMessages = String.Empty;
        private string lastStatusMessage = String.Empty;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the job worker is counting.
        /// </summary>
        /// <value>true if the job worker is counting; otherwise, false.</value>
        public bool IsCounting
        {
            get
            {
                return this.isCounting;
            }

            private set
            {
                if (this.IsCounting != value)
                {
                    this.isCounting = value;
                    this.OnPropertyChanged(view => view.IsCounting);
                }
            }
        }

        /// <summary>
        /// Gets the counted bytes.
        /// </summary>
        /// <value>The counted bytes.</value>
        public long CountedBytes
        {
            get
            {
                return this.countedBytes;
            }

            private set
            {
                if (this.CountedBytes != value)
                {
                    this.countedBytes = value;
                    this.OnPropertyChanged(view => view.CountedBytes);
                }
            }
        }

        /// <summary>
        /// Gets the proceeded bytes.
        /// </summary>
        /// <value>The proceeded bytes.</value>
        public long ProceededBytes
        {
            get
            {
                return this.proceededBytes;
            }

            private set
            {
                if (this.ProceededBytes != value)
                {
                    this.proceededBytes = value;
                    this.OnPropertyChanged(view => view.ProceededBytes);
                }
            }
        }

        /// <summary>
        /// Gets the counted files.
        /// </summary>
        /// <value>The counted files.</value>
        public int CountedFiles
        {
            get
            {
                return this.countedFiles;
            }

            private set
            {
                if (this.CountedFiles != value)
                {
                    this.countedFiles = value;
                    this.OnPropertyChanged(view => view.CountedFiles);
                }
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

            private set
            {
                if (this.ProceededFiles != value)
                {
                    this.proceededFiles = value;
                    this.OnPropertyChanged(view => view.ProceededFiles);
                }
            }
        }

        /// <summary>
        /// Gets the job settings of the current running job.
        /// </summary>
        /// <value>The job settings of the current running job.</value>
        public JobSetting CurrentJobSettings
        {
            get
            {
                return this.currentJobSetting;
            }

            private set
            {
                if (this.CurrentJobSettings != value)
                {
                    this.currentJobSetting = value;
                    this.OnPropertyChanged(view => view.CurrentJobSettings);
                }
            }
        }

        /// <summary>
        /// Gets the log messages.
        /// </summary>
        /// <value>The log messages.</value>
        public ThreadSafeObservableCollection<LogMessageViewModel> LogMessages { get; private set; }

        /// <summary>
        /// Gets the status messages.
        /// </summary>
        /// <value>The status messages.</value>
        public string StatusMessages
        {
            get { return this.statusMessages; }
            private set
            {
                if (this.statusMessages != value)
                {
                    this.statusMessages = value;
                    this.OnPropertyChanged(view => view.StatusMessages);
                }
            }
        }

        /// <summary>
        /// Gets the last status message.
        /// </summary>
        /// <value>The last status message.</value>
        public string LastStatusMessage
        {
            get { return this.lastStatusMessage; }
            private set
            {
                this.lastStatusMessage = value;
                this.OnPropertyChanged(view => view.LastStatusMessage);
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Jobs the worker view model.
        /// </summary>
        public JobWorkerViewModel()
        {
            this.LogMessages = new ThreadSafeObservableCollection<LogMessageViewModel>();

            this.ResetJobWorker();
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Resets the job worker.
        /// </summary>
        public void ResetJobWorker()
        {
            this.jobWorker = new JobWorker();
            this.jobWorker.JobStarted += new EventHandler<JobEventArgs>(jobWorker_JobStarted);
            this.jobWorker.FileProceeded += new EventHandler<FileProceededEventArgs>(jobWorker_FileProceeded);
            this.jobWorker.FilesCounted += new EventHandler(jobWorker_FilesCounted);
            this.jobWorker.DirectoryCreated += new EventHandler<DirectoryCreationEventArgs>(jobWorker_DirectoryCreated);
            this.jobWorker.DirectoryDeleted += new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeleted);
            this.jobWorker.FileDeleted += new EventHandler<FileDeletionEventArgs>(jobWorker_FileDeleted);
            this.jobWorker.FoundModifiedFile += new EventHandler<FileCopyEventArgs>(jobWorker_FoundModifiedFile);
            this.jobWorker.FoundNewerFile += new EventHandler<FileCopyEventArgs>(jobWorker_FoundNewerFile);
            this.jobWorker.JobFinished += new EventHandler<JobEventArgs>(jobWorker_JobFinished);
            this.jobWorker.Starting += new EventHandler(jobWorker_Starting);
            this.jobWorker.Finished += new EventHandler(jobWorker_Finished);

            this.LogMessages.Clear();
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        /// <param name="jobSettings">The job settings.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        public void StartJobWorker(IEnumerable<JobSetting> jobSettings, bool preview)
        {
            this.jobWorker.Start(jobSettings, preview);
            this.IsCounting = true;
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Adds a status message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AddStatusMessage(string message)
        {
            this.StatusMessages += message + Environment.NewLine;
            this.LastStatusMessage = message;
        }

        /// <summary>
        /// Handles the Finished event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobWorker_Finished(object sender, EventArgs e)
        {
            this.AddStatusMessage("Finished all jobs.");
        }

        /// <summary>
        /// Handles the Starting event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobWorker_Starting(object sender, EventArgs e)
        {
            this.AddStatusMessage("Starting jobs.");
            this.AddStatusMessage("Counting files...");
        }

        /// <summary>
        /// Handles the JobFinished event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void jobWorker_JobFinished(object sender, JobEventArgs e)
        {
            this.AddStatusMessage("Finished job: " + e.Job.Name);
        }

        /// <summary>
        /// Handles the JobStarted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void jobWorker_JobStarted(object sender, JobEventArgs e)
        {
            this.CurrentJobSettings = e.Job;
            this.AddStatusMessage("Proceeding job: " + e.Job.Name + "...");
        }

        /// <summary>
        /// Handles the FileProceeded event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        private void jobWorker_FileProceeded(object sender, FileProceededEventArgs e)
        {
            this.ProceededFiles++;
            this.ProceededBytes += e.File.Length;
        }

        /// <summary>
        /// Handles the FilesCounted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobWorker_FilesCounted(object sender, EventArgs e)
        {
            this.IsCounting = false;
            this.CountedBytes = this.jobWorker.FileCounterResult.CountedBytes;
            this.CountedFiles = this.jobWorker.FileCounterResult.CountedFiles;

            this.AddStatusMessage("Finished file counting.");
        }

        /// <summary>
        /// Handles the FoundNewerFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        void jobWorker_FoundNewerFile(object sender, FileCopyEventArgs e)
        {
            this.LogMessages.Add(
                new LogMessageViewModel("File", "Created", e.File.FullName, e.TargetDirectory.FullName));
        }

        /// <summary>
        /// Handles the FoundModifiedFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        void jobWorker_FoundModifiedFile(object sender, FileCopyEventArgs e)
        {
            this.LogMessages.Add(
                new LogMessageViewModel("File", "Modified", e.File.FullName, Path.Combine(e.TargetDirectory.FullName, e.File.Name)));
        }

        /// <summary>
        /// Handles the FileDeleted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        void jobWorker_FileDeleted(object sender, FileDeletionEventArgs e)
        {
            this.LogMessages.Add(
                new LogMessageViewModel("File", "Deleted", e.File.FullName, String.Empty));
        }

        /// <summary>
        /// Handles the DirectoryDeleted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        void jobWorker_DirectoryDeleted(object sender, DirectoryDeletionEventArgs e)
        {
            this.LogMessages.Add(
                    new LogMessageViewModel("Directory", "Deleted", e.Directory.FullName, String.Empty));
        }

        /// <summary>
        /// Handles the DirectoryCreated event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        void jobWorker_DirectoryCreated(object sender, DirectoryCreationEventArgs e)
        {
            this.LogMessages.Add(
                new LogMessageViewModel("Directory", "Created", e.Directory.FullName, e.TargetDirectory.FullName));
        }

        #endregion Private methods
    }
}