using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FlagSync.Core
{
    public static class JobSettingSerializer
    {
        /// <summary>
        /// Saves a enumeration of job-settings
        /// </summary>
        /// <param name="settings">Enumeration of job-settings</param>
        /// <param name="path">Path where the XML-file should get saved</param>
        public static void Save(IEnumerable<JobSettings> settings, string path)
        {
            XmlSerializer serializer = new XmlSerializer(settings.GetType());

            using (TextWriter writer = new StreamWriter(path, false))
            {
                serializer.Serialize(writer, settings);
            }
        }

        /// <summary>
        /// Reads an XML-file where the job-settings are saved
        /// </summary>
        /// <param name="path">Path of the XML-file</param>
        /// <returns>An enumeration of job-settings</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the file can't be read</exception>
        public static IEnumerable<JobSettings> Read(string path)
        {
            List<JobSettings> settings = new List<JobSettings>();

            XmlSerializer serializer = new XmlSerializer(settings.GetType());
            TextReader reader = new StreamReader(path);

            try
            {
                settings = (List<JobSettings>)serializer.Deserialize(reader);
            }

            catch(InvalidOperationException)
            {
                throw;
            }

            finally
            {
                reader.Close();
            }

            return settings;
        }
    }
}
