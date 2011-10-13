using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace FlagSync.Data
{
    /// <summary>
    /// Provides methods for data-access
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

            Debug.WriteLine("Current version is " + currentVersion.ToString());

            bool newVersionAvailable = webVersion > currentVersion;

            Debug.WriteLine("New version available: " + newVersionAvailable);

            return newVersionAvailable;
        }
    }
}
