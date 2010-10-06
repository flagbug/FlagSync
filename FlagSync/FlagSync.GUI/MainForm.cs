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

        void jobWorker_FileDeletionError(object sender, FileDeletionErrorEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<FileDeletionErrorEventArgs>(jobWorker_FileDeletionError), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("CantDeleteDFile") + ": " + e.File.FullName, LogMessage.MessageType.ErrorMessage));
        }

        void jobWorker_DirectoryDeletionError(object sender, DirectoryDeletionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<DirectoryDeletionEventArgs>(jobWorker_DirectoryDeletionError), new object[] { sender, e });
                return;
            }

            this.AddLogMessage(new LogMessage(rm.GetString("CantDeleteDirectory") + ": " + e.Directory.FullName, LogMessage.MessageType.ErrorMessage));
        }

        void elapsedTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.elapsedTime = this.elapsedTime.Add(new TimeSpan(0, 0, 1));
        }

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void browseDirectoryAButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.jobSettingsDirectoryATextBox.Text = dialog.SelectedPath;
        }

        private void browseDirectoryBButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.jobSettingsDirectoryBTextBox.Text = dialog.SelectedPath;
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            if (this.DirectoriesExist())
            {
                this.StartJobWorker(true);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if(this.DirectoriesExist())
            {
                this.StartJobWorker(false);
            }
        }

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

        private void stopButton_Click(object sender, EventArgs e)
        {
            this.AddLogMessage(new LogMessage(rm.GetString("Stopping") + "...", LogMessage.MessageType.StatusMessage));
            this.jobWorker.Continue();
            this.jobWorker.Stop();
            this.pauseButton.Text = rm.GetString("Pause");
        }

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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        /// <summary>
        /// Checks if directory A and directory B exists and wirtes a log message if not
        /// </summary>
        /// <returns>True if both directories exists, otherwise false</returns>
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

        private void jobSettingsNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ((JobSettings)jobSettingsCheckedListBox.SelectedItem).Name = this.jobSettingsNameTextBox.Text;
            this.jobSettingsCheckedListBox.Invalidate();
        }

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

        private void jobSettingsDirectoryATextBox_TextChanged(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).DirectoryA = this.jobSettingsDirectoryATextBox.Text; ;
        }

        private void jobSettingsDirectoryBTextBox_TextChanged(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).DirectoryB = this.jobSettingsDirectoryBTextBox.Text;
        }

        private void deleteJobButton_Click(object sender, EventArgs e)
        {
            this.jobSettingsCheckedListBox.Items.Remove(this.jobSettingsCheckedListBox.SelectedItem);
            
            if (this.jobSettingsCheckedListBox.Items.Count == 0)
            {
                this.AddNewJob();
            }

            this.jobSettingsCheckedListBox.SelectedIndex = this.jobSettingsCheckedListBox.Items.Count - 1;
        }

        private void syncRadioButton_Click(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).SyncMode = JobWorker.SyncMode.Sync;
        }

        private void backupRadioButton_Click(object sender, EventArgs e)
        {
            ((JobSettings)this.jobSettingsCheckedListBox.SelectedItem).SyncMode = JobWorker.SyncMode.Backup;
        }

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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.jobSettingsSaveFileDialog.InitialDirectory = this.appDataPath;
            this.jobSettingsSaveFileDialog.ShowDialog(this);
            JobSettingSerializer.Save(this.JobSettings, this.jobSettingsSaveFileDialog.FileName);
        }

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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.jobWorker.Stop();
      
            Application.DoEvents();
            
            base.OnClosing(e);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChangeLanguage("en", true);
        }

        private void germanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChangeLanguage("de", true);
        }

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
    }
}
