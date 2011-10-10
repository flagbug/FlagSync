using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using FlagLib.Collections;
using FlagLib.IO;
using FlagLib.Patterns.MVVM;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobWorkerViewModel : ViewModelBase<JobWorkerViewModel>
    {
        private JobWorker jobWorker;
        private JobSettingViewModel currentJobSetting;
        private DateTime startTime;
        private long countedBytes;
        private long proceededBytes;
        private int countedFiles;
        private int proceededFiles;
        private bool isCounting;
        private bool isRunning;
        private string statusMessages = String.Empty;
        private string lastStatusMessage = String.Empty;
        private int lastLogMessageIndex;
        private Timer updateTimer;
        private long averageSpeedTotal;
        private int averageSpeedCounts;

        /// <summary>
        /// Gets a value indicating whether the job worker is counting.
        /// </summary>
        /// <value>true if the job worker is counting; otherwise, false.</value>
        public bool IsCounting
        {
            get { return this.isCounting; }
            private set
            {
                if (this.IsCounting != value)
                {
                    this.isCounting = value;
                    this.OnPropertyChanged(vm => vm.IsCounting);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whetherthe job worker is started.
        /// </summary>
        /// <value>true if the job worker is started; otherwise, false.</value>
        public bool IsRunning
        {
            get { return this.isRunning; }
            set
            {
                if (this.IsRunning != value)
                {
                    this.isRunning = value;
                    this.OnPropertyChanged(vm => vm.IsRunning);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the job worker is paused.
        /// </summary>
        /// <value>true if the job worker is paused; otherwise, false.</value>
        public bool IsPaused
        {
            get { return this.jobWorker.IsPaused; }
        }

        /// <summary>
        /// Gets the counted bytes.
        /// </summary>
        /// <value>The counted bytes.</value>
        public long CountedBytes
        {
            get { return this.countedBytes; }
            private set
            {
                if (this.CountedBytes != value)
                {
                    this.countedBytes = value;
                    this.OnPropertyChanged(vm => vm.CountedBytes);
                }
            }
        }

        /// <summary>
        /// Gets the proceeded bytes.
        /// </summary>
        /// <value>The proceeded bytes.</value>
        public long ProceededBytes
        {
            get { return this.proceededBytes; }
            private set
            {
                if (value > this.CountedBytes)
                {
                    Debug.WriteLine(string.Format("Proceeded bytes exceeding range! {0} of maximum {1}", value, this.CountedBytes));
                    this.ProceededBytes = this.CountedBytes;
                }

                else if (this.ProceededBytes != value)
                {
                    this.proceededBytes = value;
                    this.OnPropertyChanged(vm => vm.ProceededBytes);
                }
            }
        }

        /// <summary>
        /// Gets the average speed in Megabytes per second.
        /// </summary>
        public string AverageSpeed
        {
            get
            {
                double averageSpeed;

                if (this.averageSpeedCounts != 0)
                {
                    averageSpeed = (this.averageSpeedTotal / this.averageSpeedCounts) / 1024.0 / 1024.0;
                }

                else
                {
                    averageSpeed = 0;
                }

                return averageSpeed.ToString("#0.00", CultureInfo.InvariantCulture) + " MB/s";
            }
        }

        /// <summary>
        /// Gets the total progress percentage.
        /// </summary>
        /// <value>The total progress percentage.</value>
        public double TotalProgressPercentage
        {
            get { return ((double)this.ProceededBytes / (double)this.CountedBytes) * 100.0; }
        }

        /// <summary>
        /// Gets the counted files.
        /// </summary>
        /// <value>The counted files.</value>
        public int CountedFiles
        {
            get { return this.countedFiles; }
            private set
            {
                if (this.CountedFiles != value)
                {
                    this.countedFiles = value;
                    this.OnPropertyChanged(vm => vm.CountedFiles);
                }
            }
        }

        /// <summary>
        /// Gets the proceeded files.
        /// </summary>
        /// <value>The proceeded files.</value>
        public int ProceededFiles
        {
            get { return this.proceededFiles; }
            private set
            {
                if (this.ProceededFiles != value)
                {
                    this.proceededFiles = value;
                    this.OnPropertyChanged(vm => vm.ProceededFiles);
                }
            }
        }

        /// <summary>
        /// Gets the job settings of the current running job.
        /// </summary>
        /// <value>The job settings of the current running job.</value>
        public JobSettingViewModel CurrentJobSetting
        {
            get { return this.currentJobSetting; }
            private set
            {
                if (this.CurrentJobSetting != value)
                {
                    this.currentJobSetting = value;
                    this.OnPropertyChanged(vm => vm.CurrentJobSetting);
                }
            }
        }

        /// <summary>
        /// Gets the log messages.
        /// </summary>
        /// <value>The log messages.</value>
        public ThreadSafeObservableCollection<LogMessage> LogMessages { get; private set; }

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
                    this.OnPropertyChanged(vm => vm.StatusMessages);
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
                if (this.statusMessages != value)
                {
                    this.lastStatusMessage = value;
                    this.OnPropertyChanged(vm => vm.LastStatusMessage);
                }
            }
        }

        /// <summary>
        /// Gets the index of the last log message.
        /// </summary>
        /// <value>The index of the last log message.</value>
        public int LastLogMessageIndex
        {
            get { return this.lastLogMessageIndex; }
            set
            {
                if (this.lastLogMessageIndex != value)
                {
                    this.lastLogMessageIndex = value;
                    this.OnPropertyChanged(vm => vm.LastLogMessageIndex);
                }
            }
        }

        /// <summary>
        /// Gets the progress of the current file.
        /// </summary>
        public int CurrentProgress
        {
            get
            {
                if (this.LastLogMessage == null)
                {
                    return 0;
                }

                else
                {
                    return this.LastLogMessage.Progress;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the job worker performs a preview.
        /// </summary>
        /// <value>true if the job worker performs a preview; otherwise, false.</value>
        public bool IsPreview { get; private set; }

        /// <summary>
        /// Gets the pause or continue string.
        /// </summary>
        /// <value>The pause or continue string.</value>
        public string PauseOrContinueString
        {
            get { return this.IsPaused ? Properties.Resources.ContinueString : Properties.Resources.PauseString; }
        }

        /// <summary>
        /// Gets or sets the current log message.
        /// </summary>
        /// <value>
        /// The current log message.
        /// </value>
        public LogMessage LastLogMessage { get; set; }

        public ICommand PauseOrContinueJobWorkerCommand
        {
            get
            {
                return new RelayCommand
                (
                    arg =>
                    {
                        if (this.IsPaused)
                        {
                            this.ContinueJobWorker();
                        }

                        else
                        {
                            this.PauseJobWorker();
                        }
                    },
                    arg =>
                    {
                        return this.IsRunning;
                    }
                );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobWorkerViewModel"/> class.
        /// </summary>
        public JobWorkerViewModel()
        {
            this.LogMessages = new ThreadSafeObservableCollection<LogMessage>();
            this.updateTimer = new Timer(1000);
            this.updateTimer.Elapsed += new ElapsedEventHandler(updateTimer_Elapsed);
            this.ResetJobWorker();
        }

        /// <summary>
        /// Resets the job worker.
        /// </summary>
        public void ResetJobWorker()
        {
            this.jobWorker = new JobWorker();
            this.jobWorker.CreatedDirectory += new EventHandler<DirectoryCreationEventArgs>(jobWorker_CreatedDirectory);
            this.jobWorker.CreatedFile += new EventHandler<FileCopyEventArgs>(jobWorker_CreatedFile);
            this.jobWorker.CreatingDirectory += new EventHandler<DirectoryCreationEventArgs>(jobWorker_CreatingDirectory);
            this.jobWorker.CreatingFile += new EventHandler<FileCopyEventArgs>(jobWorker_CreatingFile);
            this.jobWorker.DeletedDirectory += new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DeletedDirectory);
            this.jobWorker.DeletedFile += new EventHandler<FileDeletionEventArgs>(jobWorker_DeletedFile);
            this.jobWorker.DeletingDirectory += new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DeletingDirectory);
            this.jobWorker.DeletingFile += new EventHandler<FileDeletionEventArgs>(jobWorker_DeletingFile);
            this.jobWorker.DirectoryDeletionError += new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeletionError);
            this.jobWorker.FileCopyError += new EventHandler<FileCopyErrorEventArgs>(jobWorker_FileCopyError);
            this.jobWorker.FileCopyProgressChanged += new EventHandler<DataTransferEventArgs>(jobWorker_FileCopyProgressChanged);
            this.jobWorker.FileDeletionError += new EventHandler<FileDeletionErrorEventArgs>(jobWorker_FileDeletionError);
            this.jobWorker.FilesCounted += new EventHandler(jobWorker_FilesCounted);
            this.jobWorker.Finished += new EventHandler(jobWorker_Finished);
            this.jobWorker.JobFinished += new EventHandler<JobEventArgs>(jobWorker_JobFinished);
            this.jobWorker.JobStarted += new EventHandler<JobEventArgs>(jobWorker_JobStarted);
            this.jobWorker.ModifiedFile += new EventHandler<FileCopyEventArgs>(jobWorker_ModifiedFile);
            this.jobWorker.ModifyingFile += new EventHandler<FileCopyEventArgs>(jobWorker_ModifyingFile);
            this.jobWorker.ProceededFile += new EventHandler<FileProceededEventArgs>(jobWorker_ProceededFile);
            this.ResetMessages();
            this.ResetBytes();
            this.averageSpeedCounts = 0;
            this.averageSpeedTotal = 0;
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        /// <param name="jobSettings">The job settings.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        public void StartJobWorker(IEnumerable<JobSetting> jobSettings, bool preview)
        {
            if (jobSettings.All(setting => this.CheckDirectoriesExist(setting)))
            {
                this.jobWorker.Start(jobSettings, preview);
                this.IsCounting = true;
                this.IsRunning = true;
                this.IsPreview = preview;
                this.startTime = DateTime.Now;
                this.updateTimer.Start();
                this.AddStatusMessage(Properties.Resources.StartingJobsMessage);
                this.AddStatusMessage(Properties.Resources.CountingFilesMessage);
            }
        }

        /// <summary>
        /// Pauses the job worker.
        /// </summary>
        public void PauseJobWorker()
        {
            this.jobWorker.Pause();
            this.OnPropertyChanged(vm => vm.PauseOrContinueString);
            this.AddStatusMessage(Properties.Resources.PausedJobsMessage);
        }

        /// <summary>
        /// Continues the job worker.
        /// </summary>
        public void ContinueJobWorker()
        {
            this.jobWorker.Continue();
            this.OnPropertyChanged(vm => vm.PauseOrContinueString);
            this.AddStatusMessage(Properties.Resources.ContinuingJobsMessage);
            this.AddStatusMessage(Properties.Resources.StartingJobsMessage + " " + this.CurrentJobSetting.Name + "...");
        }

        /// <summary>
        /// Stops the job worker.
        /// </summary>
        public void StopJobWorker()
        {
            this.jobWorker.Stop();
            this.IsRunning = false;
            this.ResetBytes();
            this.AddStatusMessage(Properties.Resources.StoppedAllJobsMessage);
        }

        /// <summary>
        /// Handles the Elapsed event of the updateTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.OnPropertyChanged(vm => vm.AverageSpeed);
        }

        /// <summary>
        /// Checks if the specified the directories exist and add a status message, if not.
        /// </summary>
        /// <param name="jobSetting">The job setting.</param>
        /// <returns>A value indicating whether the both directories exist.</returns>
        private bool CheckDirectoriesExist(JobSetting jobSetting)
        {
            bool exist = true;

            //TODO: Add existance checks
            switch (jobSetting.SyncMode)
            {
                case SyncMode.LocalBackup:
                case SyncMode.LocalSynchronization:
                    if (!Directory.Exists(jobSetting.DirectoryA))
                    {
                        this.AddStatusMessage(jobSetting.Name + ": " + Properties.Resources.DirectoryADoesntExistMessage);
                        exist = false;
                    }

                    if (!Directory.Exists(jobSetting.DirectoryB))
                    {
                        this.AddStatusMessage(jobSetting.Name + ": " + Properties.Resources.DirectoryBDoesntExistMessage);
                        exist = false;
                    }
                    break;

                case SyncMode.ITunes:
                case SyncMode.FtpBackup:
                case SyncMode.FtpSynchronization:
                    if (!Directory.Exists(jobSetting.DirectoryB))
                    {
                        this.AddStatusMessage(jobSetting.Name + ": " + Properties.Resources.DirectoryBDoesntExistMessage);
                        exist = false;
                    }
                    break;
            }

            return exist;
        }

        /// <summary>
        /// Resets the proceeded and counted bytes to avoid that the statusbar is filled at startup of the application.
        /// </summary>
        private void ResetBytes()
        {
            this.ProceededBytes = 0;
            this.CountedBytes = 1024;
            this.OnPropertyChanged(vm => vm.TotalProgressPercentage);
        }

        /// <summary>
        /// Resets the status and log messages.
        /// </summary>
        private void ResetMessages()
        {
            this.LogMessages.Clear();
            this.StatusMessages = String.Empty;
            this.lastStatusMessage = String.Empty;
        }

        /// <summary>
        /// Adds a status message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AddStatusMessage(string message)
        {
            this.StatusMessages += DateTime.Now.ToString("HH:mm:ss") + ": " + message + Environment.NewLine;
            this.LastStatusMessage = message;
        }

        /// <summary>
        /// Adds the log message.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="type">The type.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        private void AddLogMessage(string action, string type, string sourcePath, string targetPath, bool isErrorMessage, long? fileSize)
        {
            LogMessage message = new LogMessage(type, action, sourcePath, targetPath, isErrorMessage, fileSize);
            this.LogMessages.Add(message);
            this.LastLogMessage = message;
            this.LastLogMessageIndex = this.LogMessages.Count;

            this.OnPropertyChanged(vm => vm.LastLogMessage);
        }

        /// <summary>
        /// Deletes the last log message.
        /// </summary>
        private void DeleteLastLogMessage()
        {
            this.LogMessages.Remove(this.LastLogMessage);
        }

        /// <summary>
        /// Handles the Finished event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobWorker_Finished(object sender, EventArgs e)
        {
            this.IsRunning = false;
            this.OnPropertyChanged(vm => vm.PauseOrContinueString);
            this.AddStatusMessage(Properties.Resources.FinishedAllJobsMessage);
            this.AddStatusMessage(Properties.Resources.ElapsedTimeMessage + " " + new DateTime((DateTime.Now - this.startTime).Ticks).ToString("HH:mm:ss"));

            //HACK:
            this.ProceededBytes = this.CountedBytes;
            this.OnPropertyChanged(vm => vm.TotalProgressPercentage);
        }

        /// <summary>
        /// Handles the JobFinished event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void jobWorker_JobFinished(object sender, JobEventArgs e)
        {
            this.AddStatusMessage(Properties.Resources.FinishedJobMessage + " " + e.Job.Name);
        }

        /// <summary>
        /// Handles the JobStarted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void jobWorker_JobStarted(object sender, JobEventArgs e)
        {
            this.CurrentJobSetting = new JobSettingViewModel(e.Job);
            this.AddStatusMessage(Properties.Resources.StartingJobMessage + " " + e.Job.Name + "...");
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

            this.ProceededFiles = 0;
            this.CountedFiles = this.jobWorker.FileCounterResult.CountedFiles;

            this.AddStatusMessage(Properties.Resources.FinishedFileCountingMessage);
        }

        /// <summary>
        /// Handles the FileDeletionError event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        private void jobWorker_FileDeletionError(object sender, FileDeletionErrorEventArgs e)
        {
            this.DeleteLastLogMessage();
            this.AddLogMessage(Properties.Resources.DeletionErrorString, Properties.Resources.FileString, e.File.FullName, String.Empty, true, e.File.Length);
        }

        /// <summary>
        /// Handles the FileCopyError event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        private void jobWorker_FileCopyError(object sender, FileCopyErrorEventArgs e)
        {
            this.DeleteLastLogMessage();
            this.AddLogMessage(Properties.Resources.CopyErrorString, Properties.Resources.FileString, e.File.FullName, e.TargetDirectory.FullName, true, e.File.Length);
        }

        /// <summary>
        /// Handles the DirectoryDeletionError event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        private void jobWorker_DirectoryDeletionError(object sender, DirectoryDeletionEventArgs e)
        {
            this.DeleteLastLogMessage();
            this.AddLogMessage(Properties.Resources.DeletionErrorString, Properties.Resources.DirectoryString, e.DirectoryPath, String.Empty, true, null);
        }

        /// <summary>
        /// Handles the ProceededFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        private void jobWorker_ProceededFile(object sender, FileProceededEventArgs e)
        {
            if (this.IsRunning)
            {
                this.ProceededFiles++;
                this.ProceededBytes += e.FileLength;
                this.OnPropertyChanged(vm => vm.TotalProgressPercentage);
            }
        }

        /// <summary>
        /// Handles the ModifyingFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void jobWorker_ModifyingFile(object sender, FileCopyEventArgs e)
        {
            this.AddLogMessage(Properties.Resources.ModifyingString, Properties.Resources.FileString, e.File.FullName, e.TargetDirectory.FullName, false, e.File.Length);
        }

        /// <summary>
        /// Handles the ModifiedFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void jobWorker_ModifiedFile(object sender, FileCopyEventArgs e)
        {
            this.LastLogMessage.Progress = 100;
        }

        /// <summary>
        /// Handles the FileCopyProgressChanged event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagLib.IO.DataTransferEventArgs"/> instance containing the event data.</param>
        private void jobWorker_FileCopyProgressChanged(object sender, DataTransferEventArgs e)
        {
            if (e.TotalBytes == 0)
                return;

            this.LastLogMessage.Progress = (int)(((double)e.TransferredBytes / (double)e.TotalBytes) * 100);
            this.OnPropertyChanged(vm => vm.CurrentProgress);

            this.averageSpeedTotal += e.AverageSpeed;
            this.averageSpeedCounts++;
        }

        /// <summary>
        /// Handles the DeletingFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        private void jobWorker_DeletingFile(object sender, FileDeletionEventArgs e)
        {
            this.AddLogMessage(Properties.Resources.DeletingString, Properties.Resources.FileString, e.FilePath, String.Empty, false, e.FileSize);
        }

        /// <summary>
        /// Handles the DeletedFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        private void jobWorker_DeletedFile(object sender, FileDeletionEventArgs e)
        {
            this.LastLogMessage.Progress = 100;
        }

        /// <summary>
        /// Handles the DeletingDirectory event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        private void jobWorker_DeletingDirectory(object sender, DirectoryDeletionEventArgs e)
        {
            this.AddLogMessage(Properties.Resources.DeletingString, Properties.Resources.DirectoryString, e.DirectoryPath, String.Empty, false, null);
        }

        /// <summary>
        /// Handles the DeletedDirectory event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        private void jobWorker_DeletedDirectory(object sender, DirectoryDeletionEventArgs e)
        {
            this.LastLogMessage.Progress = 100;
        }

        /// <summary>
        /// Handles the CreatingFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void jobWorker_CreatingFile(object sender, FileCopyEventArgs e)
        {
            this.AddLogMessage(Properties.Resources.CreatingString, Properties.Resources.FileString, e.File.FullName, e.TargetDirectory.FullName, false, e.File.Length);
        }

        /// <summary>
        /// Handles the CreatedFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        private void jobWorker_CreatedFile(object sender, FileCopyEventArgs e)
        {
            this.LastLogMessage.Progress = 100;
        }

        /// <summary>
        /// Handles the CreatingDirectory event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        private void jobWorker_CreatingDirectory(object sender, DirectoryCreationEventArgs e)
        {
            this.AddLogMessage(Properties.Resources.CreatingString, Properties.Resources.DirectoryString, e.Directory.FullName, e.TargetDirectory.FullName, false, null);
        }

        /// <summary>
        /// Handles the CreatedDirectory event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        private void jobWorker_CreatedDirectory(object sender, DirectoryCreationEventArgs e)
        {
            this.LastLogMessage.Progress = 100;
        }
    }
}