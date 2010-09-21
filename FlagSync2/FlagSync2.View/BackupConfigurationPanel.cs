using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlagSync2.Core;
using System.IO;

namespace FlagSync2.View
{
    public partial class BackupConfigurationPanel : JobConfigurationPanel
    {
        public BackupConfigurationPanel() : base()
        {
            InitializeComponent();
        }

        protected override void UpdateJobConfiguration()
        {
            this.nameTextBox.Text = this.Job.Name;

            BackupInfoCreator creator = (BackupInfoCreator)Job.SyncInfosCreator;

            if(creator.Source != null)
            {
                this.sourceDirectoryTextBox.Text = creator.Source.FullName;
            }

            if(creator.Target != null)
            {
                this.targetDirectoryTextBox.Text = creator.Target.FullName;
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Job.Name = this.nameTextBox.Text;
            this.OnJobNameChanged();
        }

        private void sourceDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            this.UpdateDirectories();
        }

        private void UpdateDirectories()
        {
            string sourcePath = this.sourceDirectoryTextBox.Text;
            string targetPath = this.sourceDirectoryTextBox.Text;

            if(Directory.Exists(sourcePath) && Directory.Exists(targetPath))
            {
                this.Job.SyncInfosCreator = new BackupInfoCreator(
                    new DirectoryInfo(sourcePath), new DirectoryInfo(targetPath));
            }
        }

        private void targetDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            this.UpdateDirectories();
        }

        private void sourceFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog(this);
            this.sourceDirectoryTextBox.Text = dialog.SelectedPath;
        }

        private void targetDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog(this);
            this.targetDirectoryTextBox.Text = dialog.SelectedPath;
        }
    }
}
