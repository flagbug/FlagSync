using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using FlagFtp;
using FlagLib.Serialization;
using FlagSync.Core;
using FlagSync.Core.FileSystem.ITunes;
using FlagSync.Core.FileSystem.Local;

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
        /// true if a new version of this application is availabl; otherwise, false.
        /// </returns>
        public static bool IsNewVersionAvailable(Version currentVersion)
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

            Debug.WriteLine("Current version is " + currentVersion);

            bool newVersionAvailable = webVersion > currentVersion;

            Debug.WriteLine("New version available: " + newVersionAvailable);

            return newVersionAvailable;
        }

        /// <summary>
        /// Tries the load the job settings from the specified path.
        /// </summary>
        /// <param name="path">The path to the file with the serialized setzings.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static JobSettingsLoadingResult TryLoadJobSettings(string path, out IEnumerable<JobSetting> settings)
        {
            settings = new List<JobSetting>();

            try
            {
                settings = GenericXmlSerializer.DeserializeCollection<JobSetting>(path);
            }

            catch (InvalidOperationException)
            {
                return JobSettingsLoadingResult.CorruptFile;
            }

            return settings.Any(setting => setting.SyncMode == SyncMode.ITunes) && !DataController.IsITunesOpened()
                       ? JobSettingsLoadingResult.ITunesNotOpened
                       : JobSettingsLoadingResult.Succeed;
        }

        /// <summary>
        /// Saves the job settings to the specified path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="path">The path.</param>
        public static void SaveJobSettings(IEnumerable<JobSetting> settings, string path)
        {
            GenericXmlSerializer.SerializeCollection<JobSetting>(settings.ToList(), path);
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
            switch (setting.SyncMode)
            {
                case SyncMode.LocalBackup:
                    {
                        var source = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryA));
                        var target = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryB));

                        return new LocalBackupJob(setting.Name, source, target);
                    }

                case SyncMode.LocalSynchronization:
                    {
                        var directoryA = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryA));
                        var directoryB = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryB));

                        return new LocalSyncJob(setting.Name, directoryA, directoryB);
                    }

                case SyncMode.ITunes:
                    {
                        var source = new ITunesDirectoryInfo(setting.ITunesPlaylist);
                        var target = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryB));

                        return new ITunesJob(setting.Name, source, target);
                    }

                case SyncMode.FtpBackup:
                    {
                        var source = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryA));

                        var client = new FtpClient(new NetworkCredential(setting.FtpUserName, setting.FtpPassword));
                        var target = new FlagSync.Core.FileSystem.Ftp.FtpDirectoryInfo(setting.FtpAddress, client);

                        return new FtpBackupJob(setting.Name, source, target, new Uri(setting.FtpAddress), setting.FtpUserName, setting.FtpPassword);
                    }

                case SyncMode.FtpSynchronization:
                    {
                        var directoryA = new LocalDirectoryInfo(new DirectoryInfo(setting.DirectoryA));

                        var client = new FtpClient(new NetworkCredential(setting.FtpUserName, setting.FtpPassword));
                        var directoryB = new FlagSync.Core.FileSystem.Ftp.FtpDirectoryInfo(setting.FtpAddress, client);

                        return new FtpBackupJob(setting.Name, directoryA, directoryB,
                            new Uri(setting.FtpAddress), setting.FtpUserName, setting.FtpPassword);
                    }
            }

            throw new NotSupportedException();
        }
    }
}