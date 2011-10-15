using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using FlagLib.Patterns.MVVM;
using FlagSync.Data;

namespace FlagSync.View
{
    internal class MainWindowViewModel
    {
        private JobSettingsViewModel jobSettingsViewModel = new JobSettingsViewModel();
        private JobWorkerViewModel jobWorkerViewModel = new JobWorkerViewModel();

        /// <summary>
        /// Gets the exit application command.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new RelayCommand
                (
                    arg => Application.Current.Shutdown()
                );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            DataController.CreateAppDataFolder();
        }

        /// <summary>
        /// Gets a value indicating whether iTunes is installed on the computer.
        /// </summary>
        /// <value>
        /// true if iTunes is installed on the computer; otherwise, false.
        /// </value>
        public static bool IsITunesOpened
        {
            get
            {
                return DataController.IsITunesOpened();
            }
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
                return DataController.AppDataFolderPath;
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
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                return DataController.IsNewVersionAvailable(currentVersion);
            }
        }
    }
}