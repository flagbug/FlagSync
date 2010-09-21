using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlagSync2.Core;

namespace FlagSync2.View
{
    public partial class MainForm : Form
    {
        private JobWorker worker;

        private IEnumerable<Job> CheckedJobs
        {
            get
            {
                return this.jobsCheckedListBox.CheckedItems.Cast<Job>();
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void newSynchronizationButton_Click(object sender, EventArgs e)
        {
            Job job = new Job();
            job.SyncInfosCreator = new BackupInfoCreator();

            this.AddJob(job);
        }

        private void jobsCheckedListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if(this.jobsCheckedListBox.SelectedItem != null)
            {
                this.ChangeJobConfigurationPanel((Job)this.jobsCheckedListBox.SelectedItem);
            }

            else
            {
                this.jobsCheckedListBox.SelectedIndex = 0;
            }
        }

        private void AddJob(Job job)
        {
            job.Name = "New Job " + (this.jobsCheckedListBox.Items.Count + 1);
            this.jobsCheckedListBox.Items.Add(job);
        }

        private void ChangeJobConfigurationPanel(Job job)
        {
            JobType type = ((Job)this.jobsCheckedListBox.SelectedItem).Type;
            JobConfigurationPanel panel = null;

            this.jobConfigurationPanel.Controls.Clear();

            switch(type)
            {
                case JobType.Backup:
                    panel = new BackupConfigurationPanel();
                    panel.Job = job;
                    this.jobConfigurationPanel.Controls.Add(panel);
                    break;

                case JobType.Synchronization:
                    break;
            }

            panel.JobNameChanged += new EventHandler(panel_JobNameChanged);
        }

        void panel_JobNameChanged(object sender, EventArgs e)
        {
            this.jobsCheckedListBox.Invalidate();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.worker = new JobWorker(this.CheckedJobs);
            this.worker.Start();
        }
    }
}
