using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using FlagSync.Core;
using FlagSync.Core.FileSystem.Base;
using Rareform.Serialization;

namespace FlagSync.Data
{
    /// <summary>
    /// Provides methods for data-access.
    /// </summary>
    public static class DataController
    {
        /// <summary>
        /// Gets the app data folder path.
        /// </summary>
        public static string AppDataFolderPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlagSync");
            }
        }

        /// <summary>
        /// Creates the app data folder.
        /// </summary>
        public static void CreateAppDataFolder()
        {
            Directory.CreateDirectory(DataController.AppDataFolderPath);
        }

        /// <summary>
        /// Determines whether the iTunes application is opened.
        /// </summary>
        /// <returns>
        /// true if the iTunes application is opened; otherwise, false.
        /// </returns>
        public static bool IsITunesOpened()
        {
            return Process.GetProcessesByName("iTunes").Any();
        }

        /// <summary>
        /// Determines whether a new version of this application is available.
        /// </summary>
        /// <param name="currentVersion">The current version of the application.</param>
        /// <returns>
        /// true if a new version of this application is available; otherwise, false.
        /// </returns>
        public static bool IsNewVersionAvailable(Version currentVersion)
        {
            var client = new WebClient();
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

            var webVersion = new Version(versionString);

            Debug.WriteLine("Current version is " + currentVersion);

            bool newVersionAvailable = webVersion > currentVersion;

            Debug.WriteLine("New version available: " + newVersionAvailable);

            return newVersionAvailable;
        }

        /// <summary>
        /// Tries the load the job settings from the specified path.
        /// </summary>
        /// <param name="path">The path to the file with the serialized setzings.</param>
        /// <exception cref="CorruptSaveFileException">The save file is in an invalid state.</exception>
        /// <exception cref="ITunesNotOpenedException">The iTunes process is not started..</exception>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static IEnumerable<JobSetting> LoadJobSettings(string path)
        {
            IEnumerable<JobSetting> settings;

            try
            {
                settings = GenericXmlSerializer.DeserializeCollection<JobSetting>(path);
            }

            catch (InvalidOperationException ex)
            {
                throw new CorruptSaveFileException("The save file is corrupt.", ex);
            }

            if (settings.Any(setting => setting.FirstFileSystemSetting is ITunesFileSystemSetting && !DataController.IsITunesOpened()))
            {
                throw new ITunesNotOpenedException("iTunes is not opened.");
            }

            return settings;
        }

        /// <summary>
        /// Saves the job settings to the specified path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="path">The path.</param>
        public static void SaveJobSettings(IEnumerable<JobSetting> settings, string path)
        {
            GenericXmlSerializer.SerializeCollection(settings.ToList(), path);
        }

        /// <summary>
        /// Creates a job from the specified job setting.
        /// </summary>
        /// <param name="setting">The job setting.</param>
        /// <returns>
        /// A job that is created from the specified job setting.
        /// </returns>
        public static Job CreateJobFromSetting(JobSetting setting)
        {
            IFileSystem firstFileSystem = setting.FirstFileSystemSetting.GetFileSystem();
            IFileSystem secondFileSystem = setting.SecondFileSystemSetting.GetFileSystem();

            IDirectoryInfo firstRootDirectory = setting.FirstFileSystemSetting.GetRootDirectory();
            IDirectoryInfo secondRootDirectory = setting.SecondFileSystemSetting.GetRootDirectory();

            switch (setting.SyncMode)
            {
                case SyncMode.Backup:
                    return new BackupJob(setting.Name, firstFileSystem, secondFileSystem, firstRootDirectory, secondRootDirectory);

                case SyncMode.Synchronization:
                    return new SyncJob(setting.Name, firstFileSystem, secondFileSystem, firstRootDirectory, secondRootDirectory);
            }

            throw new InvalidOperationException();
        }
    }
}