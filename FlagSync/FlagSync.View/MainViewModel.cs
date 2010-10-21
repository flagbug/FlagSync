using System;
using System.IO;

namespace FlagSync.View
{
    public class MainViewModel
    {
        #region Members
        private JobSettingsViewModel jobSettingsViewModel = new JobSettingsViewModel();
        private JobWorkerViewModel jobWorkerViewModel = new JobWorkerViewModel();
        private string appDataFolderPath;
        private string logFilePath;
        #endregion

        #region Constructor
        public MainViewModel()
        {
            this.appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FlagSync";
            this.CreateAppDatafolder();
            this.logFilePath = this.appDataFolderPath + "\\log.txt";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the job settings view model.
        /// </summary>
        /// <value>The job settings view model.</value>
        public JobSettingsViewModel JobSettingsViewModel
        {
            get
            {
                return this.jobSettingsViewModel;
            }
        }

        /// <summary>
        /// Gets the job worker view model.
        /// </summary>
        /// <value>The job worker view model.</value>
        public JobWorkerViewModel JobWorkerViewModel
        {
            get
            {
                return this.jobWorkerViewModel;
            }
        }

        /// <summary>
        /// Gets the app data folder path.
        /// </summary>
        /// <value>The app data folder path.</value>
        public string AppDataFolderPath
        {
            get
            {
                return this.appDataFolderPath;
            }
        }

        /// <summary>
        /// Gets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public string LogFilePath
        {
            get
            {
                return this.logFilePath;
            }
        }
        #endregion

        #region Public methods
        public void CreateAppDatafolder()
        {
            Directory.CreateDirectory(this.AppDataFolderPath);
        }
        #endregion
    }
}
