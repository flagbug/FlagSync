using System;
using System.IO;
using FlagSync.Core;

namespace FlagSync.View
{
    public class MainViewModel
    {
        private JobSettingsViewModel jobSettingsViewModel = new JobSettingsViewModel();
        private JobWorkerViewModel jobWorkerViewModel = new JobWorkerViewModel();
        private string appDataFolderPath;
        private string logFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.appDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlagSync");
            this.CreateAppDatafolder();
            this.logFilePath = Path.Combine(this.appDataFolderPath, "log.txt");
            Logger.Current = new Logger(this.LogFilePath);
        }

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

        /// <summary>
        /// Creates the app datafolder.
        /// </summary>
        private void CreateAppDatafolder()
        {
            Directory.CreateDirectory(this.AppDataFolderPath);
        }
    }
}