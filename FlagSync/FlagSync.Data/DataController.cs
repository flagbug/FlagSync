using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace FlagSync.Data
{
    public static class DataController
    {
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
