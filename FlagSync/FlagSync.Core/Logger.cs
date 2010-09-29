using System;
using System.IO;

namespace FlagSync.Core
{
    public class Logger
    {
        private static Logger instance;
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

        private string path = String.Empty;
        private string fileName = String.Empty;

        public string Path
        {
            get
            {
                return this.path;
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public Logger(string path)
        {
            this.path = path;
            File.Create(path);
        }

        public void LogError(string log)
        {
            DateTime time = DateTime.Now;

            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(path, true);            
                
                writer.WriteLine(time.ToString() + " ERROR: " + log);

                writer.Close();
            }

            catch
            {
                throw;
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

