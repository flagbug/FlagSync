using System;
using System.Collections.Generic;
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

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Jobs the worker view model.
        /// </summary>
        public JobWorkerViewModel()
        {
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
        }

        #endregion Private methods
    }
}