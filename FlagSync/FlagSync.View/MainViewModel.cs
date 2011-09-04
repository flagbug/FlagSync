using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
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
        /// Gets a value indicating whether a new version of this application is available.
        /// </summary>
        /// <value>
        /// true if a new version of this application is available; otherwise, false.
        /// </value>
        public bool IsNewVersionAvailable
        {
            get
            {
                WebClient client = new WebClient();
                string versionString;

                try
                {
                    Debug.WriteLine("Checking for newer version...");
                    versionString = client.DownloadString("http://flagbug.bitbucket.org/flagsyncversion");
                    Debug.WriteLine("Version on server is " + versionString);
                }

                catch (WebException)
                {
                    Debug.WriteLine("Exception while retrieving the new version.");
                    return false;
                }

                Version webVersion = new Version(versionString);

                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                Debug.WriteLine("Current version is " + currentVersion.ToString());

                bool newVersionAvailable = webVersion > currentVersion;

                Debug.WriteLine("New version available: " + newVersionAvailable);

                return newVersionAvailable;
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