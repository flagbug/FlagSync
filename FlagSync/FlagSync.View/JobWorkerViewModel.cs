using System;
using System.ComponentModel;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobWorkerViewModel : INotifyPropertyChanged
    {
        #region Members
        private JobWorker jobWorker;
        private JobSetting currentJobSetting;
        #endregion

        #region Properties
        public JobWorker JobWorker
        {
            get
            {
                return this.jobWorker;
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
        #endregion
    }
}
