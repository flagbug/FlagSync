using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using FlagSync.Core;

namespace FlagSync.GUI
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Resource manager for language
        /// </summary>
        private ResourceManager rm = new ResourceManager("FlagSync.GUI.Language", System.Reflection.Assembly.GetExecutingAssembly()); 

        /// <summary>
        /// Job worker handles the jobs
        /// </summary>
        private JobWorker jobWorker = new JobWorker();

        /// <summary>
        /// Timer to handle the elapsed time
        /// </summary>
        private System.Timers.Timer elapsedTimer = new System.Timers.Timer(1000);
        private TimeSpan elapsedTime = new TimeSpan();

        /// <summary>
        /// The path where the application data gets stored
        /// </summary>
        private readonly string appDataPath;

        /// <summary>
        /// An enumeration of the checked job settings
        /// </summary>
        private IEnumerable<JobSettings> CheckedJobSettings
        {
            get
            {
                List<JobSettings> settings = new List<JobSettings>();

                foreach (JobSettings item in this.jobSettingsCheckedListBox.CheckedItems)
                {
                    settings.Add(item);
                }

                return settings;
            }
        }

        /// <summary>
        /// An enumeration of all job settings
        /// </summary>
        private IEnumerable<JobSettings> JobSettings
        {
            get
            {
                List<JobSettings> settings = new List<JobSettings>();

                foreach(JobSettings item in this.jobSettingsCheckedListBox.Items)
                {
                    settings.Add(item);
                }

                return settings;
            }

            set
            {
                this.jobSettingsCheckedListBox.Items.Clear();

                foreach(JobSettings item in value)
                {
                    this.jobSettingsCheckedListBox.Items.Add(item);
                }
            }
        }

        public MainForm()
        {
            this.ChangeLanguage(Settings.Default.Language, false);
            
            InitializeComponent();

            this.ChangeLanguageToolStrip(Settings.Default.Language);
            
            this.elapsedTimer.Elapsed += new System.Timers.ElapsedEventHandler(elapsedTimer_Elapsed);
            this.jobWorker.FilesCounted += new EventHandler(jobWorker_FilesCounted);
            this.jobWorker.FileProceeded += new EventHandler<FileProceededEventArgs>(jobWorker_FileProceeded);
            this.jobWorker.Finished += new EventHandler(jobWorker_Finished);
            this.jobWorker.FoundNewerFile += new EventHandler<FileCopyEventArgs>(jobWorker_FoundNewerFile);
            this.jobWorker.FoundModifiedFile += new EventHandler<FileCopyEventArgs>(jobWorker_FoundModifiedFile);
            this.jobWorker.FileCopyError += new EventHandler<FileCopyErrorEventArgs>(jobWorker_FileCopyError);
            this.jobWorker.FileDeleted += new EventHandler<FileDeletionEventArgs>(jobWorker_FileDeleted);
            this.jobWorker.DirectoryCreated += new EventHandler<DirectoryCreationEventArgs>(jobWorker_DirectoryCreated);
            this.jobWorker.DirectoryDeleted += new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeleted);
            this.jobWorker.JobStarted += new EventHandler<JobEventArgs>(jobWorker_JobStarted);
            this.jobWorker.JobFinished += new EventHandler<JobEventArgs>(jobWorker_JobFinished);
            this.jobWorker.DirectoryDeletionError += new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeletionError);
            this.jobWorker.FileDeletionError += new EventHandler<FileDeletionErrorEventArgs>(jobWorker_FileDeletionError);

            this.AddNewJob();

            this.WindowState = FormWindowState.Maximized;

            this.appDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlagSync");
            this.CreateAppDataFolder();
            
        }

        /// <summary>
        /// Creates the app-data folder
        /// </summary>
        private void CreateAppDataFolder()
        {
            try
            {
                System.IO.Directory.CreateDirectory(this.appDataPath);
            }

            catch(Exception e)
            {
                this.AddLogMessage(new LogMessage(e.Message, LogMessage.MessageType.ErrorMessage));
            }
        }

        /// <summary>
        /// Handles the FileDeletionError event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        void jobWorker_FileDeletionError(object sender, FileDeletionErrorEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileDeletionErrorEventArgs>(jobWorker_FileDeletionError), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("CantDeleteFile") + ": " + e.File.FullName, LogMessage.MessageType.ErrorMessage));
        }

        /// <summary>
        /// Handles the DirectoryDeletionError event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        void jobWorker_DirectoryDeletionError(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeletionError), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("CantDeleteDirectory") + ": " + e.Directory.FullName, LogMessage.MessageType.ErrorMessage));
        }

        /// <summary>
        /// Handles the Elapsed event of the elapsedTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        void elapsedTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.elapsedTime = this.elapsedTime.Add(new TimeSpan(0, 0, 1));
        }

        /// <summary>
        /// Handles the JobFinished event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        void jobWorker_JobFinished(object sender, JobEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<JobEventArgs>(jobWorker_JobFinished), new object[] { sender, e });
                return;
            }
            
            this.AddLogMessage(new LogMessage(rm.GetString("FinishedJob") + ": " + e.Job.Name, LogMessage.MessageType.StatusMessage));

            this.currentDirectoryALabel.Text = "";
            this.currentDirectoryBLabel.Text = "";
            this.currentJobSyncModeLabel.Text = "";
            this.currentJobNameLabel.Text = "";
        }

        /// <summary>
        /// Handles the JobStarted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        void jobWorker_JobStarted(object sender, JobEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<JobEventArgs>(jobWorker_JobStarted), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("StartingJob") + ": " + e.Job.Name, LogMessage.MessageType.StatusMessage));

            this.currentJobNameLabel.Text = e.Job.Name;
            this.currentDirectoryALabel.Text = e.Job.DirectoryA;
            this.currentDirectoryBLabel.Text = e.Job.DirectoryB;

            switch (e.Job.SyncMode)
            {
                case JobWorker.SyncMode.Backup:
                    this.currentJobSyncModeLabel.Text = rm.GetString("BackupMode");
                    break;

                case JobWorker.SyncMode.Sync:
                    this.currentJobSyncModeLabel.Text = rm.GetString("SynchronizationMode");
                    break;
            }
        }

        /// <summary>
        /// Handles the DirectoryDeleted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        void jobWorker_DirectoryDeleted(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeleted), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("DeleteDirectory") + "\r\n" 
                + rm.GetString("Directory") + ": \"" + e.Directory.FullName + "\"", 
                LogMessage.MessageType.SuccessMessage));
        }

        /// <summary>
        /// Handles the DirectoryCreated event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        void jobWorker_DirectoryCreated(object sender, DirectoryCreationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<DirectoryCreationEventArgs>(jobWorker_DirectoryCreated), new object[] { sender, e });
                return;
            }
            
            this.AddLogMessage(new LogMessage(rm.GetString("CreateNewDirectory") + "\r\n" +
                        rm.GetString("Source") + ": \"" + e.Directory.FullName + "\"\r\n" +
                        rm.GetString("Target") + ": \"" + e.TargetDirectory.FullName + "\\" + e.Directory.Name + "\"",
                        LogMessage.MessageType.SuccessMessage));
        }

        /// <summary>
        /// Handles the FileDeleted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        void jobWorker_FileDeleted(object sender, FileDeletionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileDeletionEventArgs>(jobWorker_FileDeleted), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("DeleteFile") + "\r\n" +
                        rm.GetString("File") + ": \"" + e.File.FullName + "\"",
                        LogMessage.MessageType.SuccessMessage));
        }

        /// <summary>
        /// Handles the FileCopyError event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        void jobWorker_FileCopyError(object sender, FileCopyErrorEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileCopyErrorEventArgs>(jobWorker_FileCopyError), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("CantCopyFile") + ": \"" + e.File.Name + "\"" 
                + rm.GetString("ToDirectory") + ": " + e.TargetDirectory.FullName + "\"",
                LogMessage.MessageType.ErrorMessage));
        }

        /// <summary>
        /// Handles the FilesCounted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void jobWorker_FilesCounted(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(jobWorker_FilesCounted), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("FilesCounted") + ": " + this.jobWorker.FileCounterResult.CountedFiles, LogMessage.MessageType.StatusMessage));
            this.AddLogMessage(new LogMessage(rm.GetString("BytesCounted") + ": " + String.Format("{0:F2}", ((float)(jobWorker.FileCounterResult.CountedBytes * 1.0) / 1048576.0)) + " MB", LogMessage.MessageType.StatusMessage));

            this.progressBar.Maximum = (int)(jobWorker.FileCounterResult.CountedBytes / 1024);
        }

        /// <summary>
        /// Handles the Finished event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void jobWorker_Finished(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(jobWorker_Finished), new object[] { sender, e });
                return;
            }

            this.previewButton.Enabled = true;
            this.startButton.Enabled = true;
            this.pauseButton.Enabled = false;
            this.stopButton.Enabled = false;
            this.elapsedTimer.Stop();

            this.AddLogMessage(new LogMessage(rm.GetString("BytesWritten") + ": " + String.Format("{0:F2}", ((float)(jobWorker.TotalWrittenBytes * 1.0) / 1048576.0)) + " MB", LogMessage.MessageType.StatusMessage));
            this.AddLogMessage(new LogMessage(rm.GetString("Finished") + "...", LogMessage.MessageType.StatusMessage));
            this.AddLogMessage(new LogMessage(rm.GetString("ElapsedTime") + ": " + this.elapsedTime.ToString(), LogMessage.MessageType.StatusMessage));
        }

        /// <summary>
        /// Handles the FileProceeded event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        void jobWorker_FileProceeded(object sender, FileProceededEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileProceededEventArgs>(jobWorker_FileProceeded), new object[] { sender, e });
                return;
            }

            //Progress in KB
            int progress = (int)(e.File.Length / 1024);

            this.progressBar.Increment(progress);
        }

        /// <summary>
        /// Handles the FoundNewerFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        void jobWorker_FoundNewerFile(object sender, FileCopyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileCopyEventArgs>(jobWorker_FoundNewerFile), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("CreateNewFile") + "\r\n" +
                        rm.GetString("Source") + ": \"" + e.SourceDirectory.FullName + "\\" + e.File.Name + "\"\r\n" +
                        rm.GetString("Target") + ": \"" + e.TargetDirectory.FullName + "\\" + e.File.Name + "\"",
                        LogMessage.MessageType.SuccessMessage));
        }

        /// <summary>
        /// Handles the FoundModifiedFile event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        void jobWorker_FoundModifiedFile(object sender, FileCopyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileCopyEventArgs>(jobWorker_FoundModifiedFile), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage("Copy modified file\r\n" +
                        "Source: \"" + e.SourceDirectory.FullName + "\\" + e.File.Name + "\"\r\n" +
                        "Target: \"" + e.TargetDirectory.FullName + "\\" + e.File.Name + "\"",
                        LogMessage.MessageType.SuccessMessage));
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the browseDirectoryAButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void browseDirectoryAButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.jobSettingsDirectoryATextBox.Text = dialog.SelectedPath;
        }

        /// <summary>
        /// Handles the Click event of the browseDirectoryBButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void browseDirectoryBButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.jobSettingsDirectoryBTextBox.Text = dialog.SelectedPath;
        }

        /// <summary>
        /// Handles the Click event of the previewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void previewButton_Click(object sender, EventArgs e)
        {
            if (this.DirectoriesExist())
            {
                this.StartJobWorker(true);
            }
        }

        /// <summary>
        /// Handles the Click event of the startButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void startButton_Click(object sender, EventArgs e)
        {
            if(this.DirectoriesExist())
            {
                this.StartJobWorker(false);
            }
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        /// <param name="preview">if set to <c>true</c>, the jobWorker performs a preview (no files will be copied, deleted oder modified).</param>
        private void StartJobWorker(bool preview)
        {
            this.InitSync();
            
            string startLog = rm.GetString("Starting") + "...";

            if(preview)
            {
                startLog += " " + rm.GetString("PreviewInfo");
            }

            this.AddLogMessage(new LogMessage(startLog, LogMessage.MessageType.StatusMessage));
            this.AddLogMessage(new LogMessage(rm.GetString("CountingFiles") + "...", LogMessage.MessageType.StatusMessage));

            this.Update();

            this.elapsedTimer.Start();
            this.jobWorker.Start(this.CheckedJobSettings, preview);
        }

        /// <summary>
        /// Handles the Click event of the stopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void stopButton_Click(object sender, EventArgs e)
        {
            this.AddLogMessage(new LogMessage(rm.GetString("Stopping") + "...", LogMessage.MessageType.StatusMessage));
            this.jobWorker.Continue();
            this.jobWorker.Stop();
            this.pauseButton.Text = rm.GetString("Pause");
        }

        /// <summary>
        /// Handles the Click event of the pauseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (!this.jobWorker.Paused)
            {
                this.jobWorker.Pause();
                this.elapsedTimer.Stop();
                this.AddLogMessage(new LogMessage(rm.GetString("Paused") + "...", LogMessage.MessageType.StatusMessage));
                this.pauseButton.Text = rm.GetString("Resume");
            }

            else
            {
                this.jobWorker.Continue();
                this.elapsedTimer.Start();
                this.AddLogMessage(new LogMessage(rm.GetString("Resume") + "...", LogMessage.MessageType.StatusMessage));
                this.pauseButton.Text = rm.GetString("Pause");
            }
        }

        /// <summary>
        /// Handles the Click event of the aboutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
        
        /// <summary>
        /// Checks if directory A and directory B exists and wirtes a log message if not
        /// </summary>
        /// <returns>
        /// True if both directories exists, otherwise false
        /// </returns>
        private bool DirectoriesExist()
        {
            bool exist = true;

            foreach (JobSettings setting in this.CheckedJobSettings)
            {
                if (!System.IO.Directory.Exists(setting.DirectoryA))
                {
                    this.AddLogMessage(new LogMessage(setting.Name + ": " + rm.GetString("Directory") + "A: \"" + setting.DirectoryA + "\" " + rm.GetString("DoesNotExist") + "!",
                        LogMessage.MessageType.ErrorMessage));
                    exist = false;
                }

                if (!System.IO.Directory.Exists(setting.DirectoryB))
                {
                    this.AddLogMessage(new LogMessage(setting.Name + ": " + rm.GetString("Directory") + "B: \"" + setting.DirectoryB + "\" " + rm.GetString("DoesNotExist") + "!",
                        LogMessage.MessageType.ErrorMessage));
                    exist = false;
                }
            }

            return exist;
        }

        /// <summary>
        /// Adds a log message to the logTextBox
        /// </summary>
        /// <param name="log">The log message</param>
        private void AddLogMessage(LogMessage log)
        {
            int lenght = this.logTextBox.TextLength;

            this.logTextBox.AppendText(log.Message + Environment.NewLine + Environment.NewLine);

            this.logTextBox.Select(lenght, this.logTextBox.TextLength);

            Color textColor = Color.Black;

            switch(log.Type)
            {
                case LogMessage.MessageType.ErrorMessage:
                    textColor = Color.DarkRed;
                    break;

                case LogMessage.MessageType.StatusMessage:
                    textColor = Color.DarkBlue;
                    break;

                case LogMessage.MessageType.SuccessMessage:
                    textColor = Color.DarkGreen;
                    break;
            }

            this.logTextBox.SelectionColor = textColor;

            this.logTextBox.ScrollToCaret();
        }

        /// <summary>
        /// Initialises the user interface for a new sync
        /// </summary>
        private void InitSync()
        {
            this.progressBar.Maximum = 0;
            this.pauseButton.Enabled = true;
            this.stopButton.Enabled = true;
            this.previewButton.Enabled = false;
            this.startButton.Enabled = false;
            this.logTextBox.Text = "";
            this.elapsedTime = new TimeSpan();

            Logger.Instance = new Logger(System.IO.Path.Combine(this.appDataPath, "log.txt"));
        }

        /// <summary>
        /// Handles the TextChanged event of the jobSettingsNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobSettingsNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ((JobSettings)jobSettingsCheckedListBox.SelectedItem).Name = this.jobSettingsNameTextBox.Text;
            this.jobSettingsCheckedListBox.Invalidate();
        }

        /// <summary>
        /// Handles the Click event of the newJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void newJobButton_Click(object sender, EventArgs e)
        {
            this.AddNewJob();
        }

        /// <summary>
        /// Adds a new and empty job to the listbox
        /// </summary>
        private void AddNewJob()
        {
            this.jobSettingsCheckedListBox.Items.Add(new JobSettings(rm.GetString("NewJob") + " " + (this.jobSettingsCheckedListBox.Items.Count + 1)));
            this.jobSettingsCheckedListBox.SelectedItem = this.jobSettingsCheckedListBox.Items[this.jobSettingsCheckedListBox.Items.Count - 1];
            this.jobSettingsCheckedListBox.SetItemChecked(this.jobSettingsCheckedListBox.SelectedIndex, true);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the jobSettingsCheckedListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobSettingsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            JobSettings setting = (JobSettings)this.jobSettingsCheckedListBox.SelectedItem;

            if(setting != null)
            {
                this.jobSettingsDirectoryATextBox.Text = setting.DirectoryA;
                this.jobSettingsDirectoryBTextBox.Text = setting.DirectoryB;
                this.jobSettingsNameTextBox.Text = setting.Name;

                this.backupRadioButton.Checked = false;
                this.syncRadioButton.Checked = false;

                switch(setting.SyncMode)
                {
                    case JobWorker.SyncMode.Backup:
                        this.backupRadioButton.Checked = true;
                        break;

                    case JobWorker.SyncMode.Sync:
                        this.syncRadioButton.Checked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the jobSettingsDirectoryATextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobSettingsDirectoryATextBox_TextChanged(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).DirectoryA = this.jobSettingsDirectoryATextBox.Text; ;
        }

        /// <summary>
        /// Handles the TextChanged event of the jobSettingsDirectoryBTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void jobSettingsDirectoryBTextBox_TextChanged(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).DirectoryB = this.jobSettingsDirectoryBTextBox.Text;
        }

        /// <summary>
        /// Handles the Click event of the deleteJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void deleteJobButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedJob();
        }

        /// <summary>
        /// Handles the Click event of the syncRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void syncRadioButton_Click(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).SyncMode = JobWorker.SyncMode.Sync;
        }

        /// <summary>
        /// Handles the Click event of the backupRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void backupRadioButton_Click(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).SyncMode = JobWorker.SyncMode.Backup;
        }

        /// <summary>
        /// Handles the Click event of the loadToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.jobSettingsOpenFileDialog.InitialDirectory = this.appDataPath;
            this.jobSettingsOpenFileDialog.ShowDialog();

            if (!String.IsNullOrEmpty(this.jobSettingsOpenFileDialog.FileName))
            {
                try
                {
                    this.JobSettings = (List<JobSettings>)JobSettingSerializer.Read(this.jobSettingsOpenFileDialog.FileName);
                    this.AddLogMessage(new LogMessage(rm.GetString("JobSettingsLoadSucceed"), LogMessage.MessageType.StatusMessage));
                }

                catch(InvalidOperationException)
                {
                    this.AddLogMessage(new LogMessage(rm.GetString("JobSettingsLoadError"), LogMessage.MessageType.ErrorMessage));
                }

                this.jobSettingsCheckedListBox.SelectedIndex = 0;

                for(int i = 0; i < this.jobSettingsCheckedListBox.Items.Count; i++)
                {
                    this.jobSettingsCheckedListBox.SetItemCheckState(i, CheckState.Checked);
                }
            }

            else
            {
                this.AddLogMessage(new LogMessage(rm.GetString("NoValidFileSelected") + "!", LogMessage.MessageType.ErrorMessage));
            }
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.jobSettingsSaveFileDialog.InitialDirectory = this.appDataPath;
            this.jobSettingsSaveFileDialog.ShowDialog(this);
            JobSettingSerializer.Save(this.JobSettings, this.jobSettingsSaveFileDialog.FileName);
        }

        /// <summary>
        /// Changes the language of the GUI
        /// </summary>
        /// <param name="name">Name of the culture</param>
        /// <param name="restart">True, if the application should be restarted, otherwise false</param>
        private void ChangeLanguage(string name, bool restart)
        {
            Settings.Default.Language = name;
            Settings.Default.Save();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);

            if (restart)
            {
                Application.Restart();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.jobWorker.Stop();
      
            Application.DoEvents();
            
            base.OnClosing(e);
        }

        /// <summary>
        /// Handles the Click event of the englishToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChangeLanguage("en", true);
        }

        /// <summary>
        /// Handles the Click event of the germanToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void germanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChangeLanguage("de", true);
        }

        /// <summary>
        /// Changes the checked state of the language toolstrip item
        /// </summary>
        /// <param name="language">Name of the culture</param>
        private void ChangeLanguageToolStrip(string language)
        {
            if (language == "de")
            {
                this.germanToolStripMenuItem.CheckState = CheckState.Checked;
            }

            else if (language == "en")
            {
                this.englishToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        /// <summary>
        /// Deletes the currently selected job
        /// </summary>
        private void DeleteSelectedJob()
        {
            this.jobSettingsCheckedListBox.Items.Remove(this.jobSettingsCheckedListBox.SelectedItem);

            if (this.jobSettingsCheckedListBox.Items.Count == 0)
            {
                this.AddNewJob();
            }

            this.jobSettingsCheckedListBox.SelectedIndex = this.jobSettingsCheckedListBox.Items.Count - 1;
        }

        /// <summary>
        /// Handles the KeyDown event of the jobSettingsCheckedListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void jobSettingsCheckedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.DeleteSelectedJob();
            }
        }
    }
}
