using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlagLib.Collections;
using FlagLib.Extensions;
using FlagLib.IO;
using FlagLib.Patterns.MVVM;
using FlagSync.Core;
using FlagSync.Data;

namespace FlagSync.View
{
    internal class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
    {
        private JobSettingViewModel selectedJobSetting;
        private JobWorker jobWorker;
        private JobViewModel currentJob;
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
        private readonly Timer updateTimer;
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
                long averageSpeed;

                if (this.averageSpeedCounts != 0)
                {
                    averageSpeed = this.averageSpeedTotal / this.averageSpeedCounts;
                }

                else
                {
                    averageSpeed = 0;
                }

                return averageSpeed.ToSizeString() + "/s";
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
        public JobViewModel CurrentJob
        {
            get { return this.currentJob; }
            private set
            {
                if (this.CurrentJob != value)
                {
                    this.currentJob = value;
                    this.OnPropertyChanged(vm => vm.CurrentJob);
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

        /// <summary>
        /// Gets the job settings.
        /// </summary>
        public ObservableCollection<JobSettingViewModel> JobSettings { get; private set; }

        /// <summary>
        /// Gets the current job settings panel.
        /// </summary>
        public ObservableCollection<UserControl> CurrentJobSettingsPanel { get; private set; }

        /// <summary>
        /// Gets or sets the selected job setting.
        /// </summary>
        /// <value>
        /// The selected job setting.
        /// </value>
        public JobSettingViewModel SelectedJobSetting
        {
            get { return this.selectedJobSetting; }
            set
            {
                if (this.SelectedJobSetting != value)
                {
                    this.selectedJobSetting = value;
                    this.OnPropertyChanged(vm => vm.SelectedJobSetting);

                    this.CurrentJobSettingsPanel.Clear();

                    switch (value.SyncMode)
                    {
                        case SyncMode.LocalBackup:
                        case SyncMode.LocalSynchronization:
                            this.CurrentJobSettingsPanel.Add(new LocalJobSettingsPanel(value));
                            break;

                        case SyncMode.FtpBackup:
                        case SyncMode.FtpSynchronization:
                            this.CurrentJobSettingsPanel.Add(new FtpJobSettingsPanel(value));
                            break;

                        case SyncMode.ITunes:
                            this.CurrentJobSettingsPanel.Add(new ITunesJobSettingsPanel(value));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether a new version of this application is available.
        /// </summary>
        /// <value>
        /// true if a new version of this application is available; otherwise, false.
        /// </value>
        public bool IsNewVersionAvailable
        {
            get
            {
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                return DataController.IsNewVersionAvailable(currentVersion);
            }
        }

        /// <summary>
        /// Gets the pause or continue job worker command.
        /// </summary>
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
                    arg => this.IsRunning
                );
            }
        }

        /// <summary>
        /// Gets the stop job worker command.
        /// </summary>
        public ICommand StopJobWorkerCommand
        {
            get
            {
                return new RelayCommand
                (
                    arg =>
                    {
                        this.jobWorker.Stop();
                        this.IsRunning = false;
                        this.ResetBytes();
                        this.AddStatusMessage(Properties.Resources.StoppedAllJobsMessage);
                    },
                    arg => this.IsRunning
                );
            }
        }

        /// <summary>
        /// Gets the delete selected job setting command.
        /// </summary>
        public ICommand DeleteSelectedJobSettingCommand
        {
            get
            {
                return new RelayCommand
                (
                    arg =>
                    {
                        this.JobSettings.Remove(this.SelectedJobSetting);

                        if (this.JobSettings.Count == 0)
                        {
                            this.AddNewJobSetting(SyncMode.LocalBackup);
                        }

                        this.SelectedJobSetting = this.JobSettings.Last();
                    }
                );
            }
        }

        /// <summary>
        /// Gets the exit application command.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new RelayCommand
                (
                    arg => Application.Current.Shutdown()
                );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            DataController.CreateAppDataFolder();

            this.JobSettings = new ObservableCollection<JobSettingViewModel>();
            this.CurrentJobSettingsPanel = new ObservableCollection<UserControl>();
            this.AddNewJobSetting(SyncMode.LocalBackup);
            this.SelectedJobSetting = this.JobSettings[0];

            this.LogMessages = new ThreadSafeObservableCollection<LogMessage>();
            this.updateTimer = new Timer(1000);
            this.updateTimer.Elapsed += new ElapsedEventHandler(updateTimer_Elapsed);
            this.ResetJobWorker();
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        /// <param name="jobSettings">The job settings.</param>
        /// <param name="preview">if set to true, a preview will be performed.</param>
        public void StartJobWorker(IEnumerable<JobSetting> jobSettings, bool preview)
        {
            this.ResetJobWorker();

            var jobs = jobSettings.Select(DataController.CreateJobFromSetting);

            if (jobs.All(this.CheckDirectoriesExist))
            {
                this.jobWorker.StartAsync(jobs, preview);

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
        /// Adds a new job setting with the specified mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void AddNewJobSetting(SyncMode mode)
        {
            var setting = new JobSettingViewModel(Properties.Resources.NewJobString + " " + (this.JobSettings.Count + 1));
            setting.SyncMode = mode;

            this.JobSettings.Add(setting);
        }

        /// <summary>
        /// Saves the job settings.
        /// </summary>
        /// <param name="path">The path.</param>
        public void SaveJobSettings(string path)
        {
            DataController.SaveJobSettings(this.JobSettings.Select(setting => setting.InternJobSetting), path);
        }

        /// <summary>
        /// Loads the job settings.
        /// </summary>
        /// <param name="path">The path.</param>
        public JobSettingsLoadingResult LoadJobSettings(string path)
        {
            IEnumerable<JobSetting> settings;

            var result = DataController.TryLoadJobSettings(path, out settings);

            if (result == JobSettingsLoadingResult.Succeed)
            {
                this.JobSettings.Clear();

                foreach (JobSetting setting in settings)
                {
                    this.JobSettings.Add(new JobSettingViewModel(setting));
                }

                if (settings.Any())
                {
                    this.SelectedJobSetting = this.JobSettings.First();
                }
            }

            return result;
        }

        /// <summary>
        /// Resets the job worker.
        /// </summary>
        private void ResetJobWorker()
        {
            this.jobWorker = new JobWorker();
            this.jobWorker.CreatedDirectory += jobWorker_CreatedDirectory;
            this.jobWorker.CreatedFile += jobWorker_CreatedFile;
            this.jobWorker.CreatingDirectory += jobWorker_CreatingDirectory;
            this.jobWorker.CreatingFile += jobWorker_CreatingFile;
            this.jobWorker.DeletedDirectory += jobWorker_DeletedDirectory;
            this.jobWorker.DeletedFile += jobWorker_DeletedFile;
            this.jobWorker.DeletingDirectory += jobWorker_DeletingDirectory;
            this.jobWorker.DeletingFile += jobWorker_DeletingFile;
            this.jobWorker.DirectoryDeletionError += jobWorker_DirectoryDeletionError;
            this.jobWorker.FileCopyError += jobWorker_FileCopyError;
            this.jobWorker.FileCopyProgressChanged += jobWorker_FileCopyProgressChanged;
            this.jobWorker.FileDeletionError += jobWorker_FileDeletionError;
            this.jobWorker.FilesCounted += jobWorker_FilesCounted;
            this.jobWorker.Finished += jobWorker_Finished;
            this.jobWorker.JobFinished += jobWorker_JobFinished;
            this.jobWorker.JobStarted += jobWorker_JobStarted;
            this.jobWorker.ModifiedFile += jobWorker_ModifiedFile;
            this.jobWorker.ModifyingFile += jobWorker_ModifyingFile;
            this.jobWorker.ProceededFile += jobWorker_ProceededFile;
            this.ResetMessages();
            this.ResetBytes();
            this.averageSpeedCounts = 0;
            this.averageSpeedTotal = 0;
        }

        /// <summary>
        /// Pauses the job worker.
        /// </summary>
        private void PauseJobWorker()
        {
            this.jobWorker.Pause();
            this.OnPropertyChanged(vm => vm.PauseOrContinueString);
            this.AddStatusMessage(Properties.Resources.PausedJobsMessage);
        }

        /// <summary>
        /// Continues the job worker.
        /// </summary>
        private void ContinueJobWorker()
        {
            this.jobWorker.Continue();
            this.OnPropertyChanged(vm => vm.PauseOrContinueString);
            this.AddStatusMessage(Properties.Resources.ContinuingJobsMessage);
            this.AddStatusMessage(Properties.Resources.StartingJobsMessage + " " + this.CurrentJob.Name + "...");
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
        /// <param name="job">The job.</param>
        /// <returns>
        /// A value indicating whether the both directories exist.
        /// </returns>
        private bool CheckDirectoriesExist(Job job)
        {
            bool exist = true;

            if (!job.DirectoryA.Exists)
            {
                this.AddStatusMessage(job.Name + ": " + Properties.Resources.DirectoryADoesntExistMessage);
                exist = false;
            }

            if (!job.DirectoryB.Exists)
            {
                this.AddStatusMessage(job.Name + ": " + Properties.Resources.DirectoryBDoesntExistMessage);
                exist = false;
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
        /// <param name="isErrorMessage">if set to <c>true</c> the log message is an error message.</param>
        /// <param name="fileSize">The size of the file.</param>
        private void AddLogMessage(string action, string type, string sourcePath, string targetPath, bool isErrorMessage, long? fileSize)
        {
            var message = new LogMessage(type, action, sourcePath, targetPath, isErrorMessage, fileSize);
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
            this.CurrentJob = new JobViewModel(e.Job);
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