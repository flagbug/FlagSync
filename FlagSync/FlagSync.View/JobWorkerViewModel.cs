﻿using System;
using System.ComponentModel;
using FlagSync.Core;
using System.Collections.Generic;

namespace FlagSync.View
{
    public class JobWorkerViewModel : INotifyPropertyChanged
    {
        #region Members
        private JobWorker jobWorker;
        private JobSetting currentJobSetting;
        private long countedBytes;
        private long proceededBytes;
        private int countedFiles;
        private int proceededFiles;
        private bool isCounting;
        #endregion

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
                    this.OnPropertyChanged("IsCounting");
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
                    this.OnPropertyChanged("CountedBytes");
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
                    this.OnPropertyChanged("ProceededBytes");
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
                    this.OnPropertyChanged("CountedFiles");
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
                    this.OnPropertyChanged("ProceededFiles");
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
                if(this.CurrentJobSettings != value)
                {
                    this.currentJobSetting = value;
                    this.OnPropertyChanged("CurrentJobSettings");
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Jobs the worker view model.
        /// </summary>
        public JobWorkerViewModel()
        {
            this.ResetJobWorker();
        }
        #endregion

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
        #endregion

        #region Protected methods
        /// <summary>
        /// Called when a property has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Handles the JobStarted event of the jobWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FlagSync.Core.JobEventArgs"/> instance containing the event data.</param>
        private void jobWorker_JobStarted(object sender, JobEventArgs e)
        {
            this.CurrentJobSettings = e.Job;
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

        private void jobWorker_FilesCounted(object sender, EventArgs e)
        {
            this.IsCounting = false;
            this.CountedBytes = this.jobWorker.FileCounterResult.CountedBytes;
            this.CountedFiles = this.jobWorker.FileCounterResult.CountedFiles;
        }
        #endregion
    }
}
