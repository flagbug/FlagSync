using System;
using System.Windows.Forms;

namespace FlagSync
{
    public partial class MainForm : Form
    {
        private Synchronizer synchronizer = new Synchronizer();
        private Synchronizer.SyncMode syncmode = Synchronizer.SyncMode.Sync;

        public MainForm()
        {
            InitializeComponent();

            this.synchronizer.FilesCounted += new EventHandler(synchronizer_OnHasCountFiles);
            this.synchronizer.FileProceeded += new EventHandler(synchronizer_OnProceededFile);
            this.synchronizer.Finished += new EventHandler(synchronizer_OnFinish);
            this.synchronizer.NewFile += new EventHandler<Synchronizer.FileCopyEventArgs>(synchronizer_OnNewFile);
            this.synchronizer.ModifiedFile += new EventHandler<Synchronizer.FileCopyEventArgs>(synchronizer_OnModifiedFile);
            this.synchronizer.FileCopyError += new EventHandler<Synchronizer.FileCopyErrorEventArgs>(synchronizer_OnFileCopyError);
            this.synchronizer.FileDeletion += new EventHandler<Synchronizer.FileDeletionEventArgs>(synchronizer_OnFileDeletion);
            this.synchronizer.DirectoryCreated += new EventHandler<Synchronizer.DirectoryCreationEventArgs>(synchronizer_OnDirectoryCreation);
            this.synchronizer.DirectoryDeleted += new EventHandler<Synchronizer.DirectoryDeletionEventArgs>(synchronizer_OnDirectoryDeletion);
        }

        void synchronizer_OnDirectoryDeletion(object sender, Synchronizer.DirectoryDeletionEventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler<Synchronizer.DirectoryDeletionEventArgs> onDirectoryDeletion =
                    new EventHandler<Synchronizer.DirectoryDeletionEventArgs>(synchronizer_OnDirectoryDeletion);

                this.Invoke(onDirectoryDeletion, new object[] { sender, e });
                return;
            }

            this.AddLog("Delete directory\r\n" +
            "Directory: \"" + e.Directory.FullName + "\"");
        }

        void synchronizer_OnDirectoryCreation(object sender, Synchronizer.DirectoryCreationEventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler<Synchronizer.DirectoryCreationEventArgs> onDirectoryCreation =
                    new EventHandler<Synchronizer.DirectoryCreationEventArgs>(synchronizer_OnDirectoryCreation);

                this.Invoke(onDirectoryCreation, new object[] { sender, e });
                return;
            }

            this.AddLog("Create new directory\r\n" +
                        "Source: \"" + e.Directory.FullName + "\"\r\n" +
                        "Target: \"" + e.TargetDirectory.FullName + "\\" + e.Directory.Name + "\"");
        }

        void synchronizer_OnFileDeletion(object sender, Synchronizer.FileDeletionEventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler<Synchronizer.FileDeletionEventArgs> onFileDeletion =
                    new EventHandler<Synchronizer.FileDeletionEventArgs>(synchronizer_OnFileDeletion);

                this.Invoke(onFileDeletion, new object[] { sender, e });
                return;
            }

            this.AddLog("Delete file\r\n" +
                        "File: \"" + e.File.FullName + "\"");
        }

        void synchronizer_OnFileCopyError(object sender, Synchronizer.FileCopyErrorEventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler<Synchronizer.FileCopyErrorEventArgs> onFileCopyError =
                    new EventHandler<Synchronizer.FileCopyErrorEventArgs>(synchronizer_OnHasCountFiles);

                this.Invoke(onFileCopyError, new object[] { sender, e });
                return;
            }

            this.AddLog("Can not copy file: \"" + e.File.Name + "\" into directory: " + e.TargetDirectory.FullName + "\"");
        }

        void synchronizer_OnHasCountFiles(object sender, EventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler onHasCountFiles = new EventHandler(synchronizer_OnHasCountFiles);
                this.Invoke(onHasCountFiles, new object[] { sender, e });
                return;
            }

            this.progressBar.Maximum = this.synchronizer.CountedFiles;
        }

        void synchronizer_OnFinish(object sender, EventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler onFinish = new EventHandler(synchronizer_OnFinish);
                this.Invoke(onFinish, new object[] { sender, e });
                return;
            }

            this.previewButton.Enabled = true;
            this.startButton.Enabled = true;
            this.pauseButton.Enabled = false;
            this.stopButton.Enabled = false;

            this.AddLog("Bytes written: " + ((synchronizer.WrittenBytes * 1.0) / 1048576.0) + " MB");
            this.AddLog("Finished...");
        }

        void synchronizer_OnProceededFile(object sender, EventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler onProceededFile = new EventHandler(synchronizer_OnProceededFile);
                this.Invoke(onProceededFile, new object[] { sender, e });
                return;
            }

            this.progressBar.PerformStep();
        }

        void synchronizer_OnNewFile(object sender, Synchronizer.FileCopyEventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler<Synchronizer.FileCopyEventArgs> onNewFile =
                    new EventHandler<Synchronizer.FileCopyEventArgs>(synchronizer_OnNewFile);
                this.Invoke(onNewFile, new object[] { sender, e });
                return;
            }

            this.AddLog("Create new file\r\n" +
                        "Source: \"" + e.SourceDirectory.FullName + "\\" + e.File.Name + "\"\r\n" +
                        "Target: \"" + e.TargetDirectory.FullName + "\\" + e.File.Name + "\"");
        }

        void synchronizer_OnModifiedFile(object sender, Synchronizer.FileCopyEventArgs e)
        {
            if(this.InvokeRequired)
            {
                EventHandler<Synchronizer.FileCopyEventArgs> onModifiedFile =
                    new EventHandler<Synchronizer.FileCopyEventArgs>(synchronizer_OnModifiedFile);
                this.Invoke(onModifiedFile, new object[] { sender, e });
                return;
            }

            this.AddLog("Copy modified file\r\n" +
                        "Source: \"" + e.SourceDirectory.FullName + "\\" + e.File.Name + "\"\r\n" +
                        "Target: \"" + e.TargetDirectory.FullName + "\\" + e.File.Name + "\"");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void browseDirectoryAButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.directoryATextBox.Text = dialog.SelectedPath;
        }

        private void browseDirectoryBButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.directoryBTextBox.Text = dialog.SelectedPath;
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            if(this.DirectoriesExist())
            {
                this.InitSync();

                this.AddLog("Starting Preview...");

                this.synchronizer.Start(this.directoryATextBox.Text, this.directoryBTextBox.Text, this.syncmode, true);
            }
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            if(this.DirectoriesExist())
            {
                this.InitSync();

                switch(this.syncmode)
                {
                    case Synchronizer.SyncMode.Sync:
                        this.AddLog("Starting Sychronization...");
                        break;

                    case Synchronizer.SyncMode.Backup:
                        this.AddLog("Starting Backup...");
                        break;
                }

                this.synchronizer.Start(this.directoryATextBox.Text, this.directoryBTextBox.Text, this.syncmode, false);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            this.AddLog("Stopping...");
            this.synchronizer.Stop();
            this.pauseButton.Text = "Pause";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.synchronizer.Stop();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if(!this.synchronizer.Pause)
            {
                this.synchronizer.Pause = true;
                this.AddLog("Paused...");
                this.pauseButton.Text = "Resume";
            }

            else
            {
                this.synchronizer.Pause = false;
                this.AddLog("Resume...");
                this.pauseButton.Text = "Pause";
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void syncModeButton_Click(object sender, EventArgs e)
        {
            switch(this.syncmode)
            {
                case Synchronizer.SyncMode.Backup:
                    this.syncModeButton.Text = "A<->B";
                    this.syncmode = Synchronizer.SyncMode.Sync;
                    break;

                case Synchronizer.SyncMode.Sync:
                    this.syncModeButton.Text = "A-->B";
                    this.syncmode = Synchronizer.SyncMode.Backup;
                    break;
            }
        }

        private bool DirectoriesExist()
        {
            bool exist = true;

            if(!System.IO.Directory.Exists(this.directoryATextBox.Text))
            {
                this.AddLog("Directory A: \"" + this.directoryATextBox.Text + "\" does not exist!");
                exist = false;
            }

            if(!System.IO.Directory.Exists(this.directoryBTextBox.Text))
            {
                this.AddLog("Directory B: \"" + this.directoryBTextBox.Text + "\" does not exist!");
                exist = false;
            }

            return exist;
        }

        private void AddLog(string log)
        {
            this.logTextBox.Text += log + Environment.NewLine + Environment.NewLine;
            this.logTextBox.SelectionStart = this.logTextBox.TextLength;
            this.logTextBox.ScrollToCaret();
        }

        private void InitSync()
        {
            this.progressBar.Maximum = 0;
            this.pauseButton.Enabled = true;
            this.stopButton.Enabled = true;
            this.previewButton.Enabled = false;
            this.startButton.Enabled = false;
            this.logTextBox.Text = "";
        }
    }
}
