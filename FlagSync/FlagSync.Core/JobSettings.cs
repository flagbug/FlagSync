using System;

namespace FlagSync.Core
{
    [Serializable]
    public class JobSettings
    {
        private string directoryA = "";
        public string DirectoryA
        {
            get
            {
                return this.directoryA;
            }

            set
            {
                this.directoryA = value;
            }
        }

        private string directoryB = "";
        public string DirectoryB
        {
            get
            {
                return this.directoryB;
            }

            set
            {
                this.directoryB = value;
            }
        }

        private JobWorker.SyncMode syncMode;
        public JobWorker.SyncMode SyncMode
        {
            get
            {
                return this.syncMode;
            }

            set
            {
                this.syncMode = value;
            }
        }

        private string name = "";
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public JobSettings(string name)
        {
            this.name = name;
        }

        public JobSettings()
        {

        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
