﻿using System;
using System.IO;

namespace FlagSync.Core
{
    public class Logger
    {
        private string path = String.Empty;
        private static Logger instance = null;

        /// <summary>
        /// Singleton instance of the logger (must be manually initialized)
        /// </summary>
        public static Logger Instance
        {
            get
            {
                return Logger.instance;
            }

            set
            {
                Logger.instance = value;
            }
        }

        /// <summary>
        /// Path of the log file
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }
        }

        /// <summary>
        /// Creates a new instance of the logger, with the given filepath
        /// </summary>
        /// <param name="path">The path of the log file</param>
        public Logger(string path)
        {
            this.path = path;

            try
            {
                File.Create(path);
            }

            catch(DirectoryNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="message">The error message</param>
        public void LogError(string message)
        {
            this.Log(message, "ERROR");
        }

        /// <summary>
        /// Logs a succeed message
        /// </summary>
        /// <param name="message">The succeed message</param>
        public void LogSucceed(string message)
        {
            this.Log(message, "SUCCEED");
        }

        /// <summary>
        /// Logs a status message
        /// </summary>
        /// <param name="message">The status message</param>
        public void LogStatusMessage(string message)
        {
            this.Log(message, "STATUS");
        }

        /// <summary>
        /// Adds a log
        /// </summary>
        /// <param name="log">The log message</param>
        /// <param name="type">The message type, e.g "ERROR" or "SUCCEED"</param>
        private void Log(string log, string type)
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(path, true);

                writer.WriteLine(DateTime.Now.ToString() + " " + type + ": " + log);

                writer.Close();
            }

            catch(IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
