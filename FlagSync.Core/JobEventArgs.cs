using System.IO;

namespace FlagSync.Core
{
    public class JobEventArgs : System.EventArgs
    {
        public JobSettings Job
        {
            get;
            private set;
        }

        public JobEventArgs(JobSettings job)
        {
            this.Job = job;
        }
    }

    public class FileProceededEventArgs : System.EventArgs
    {
        public FileInfo File
        {
            get;
            private set;
        }

        public FileProceededEventArgs(FileInfo file)
        {
            this.File = file;
        }
    }

    public class FileCopyEventArgs : System.EventArgs
    {
        public FileInfo File
        {
            get;
            private set;
        }

        public DirectoryInfo SourceDirectory
        {
            get;
            private set;
        }

        public DirectoryInfo TargetDirectory
        {
            get;
            private set;
        }

        public FileCopyEventArgs(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            this.File = file;
            this.SourceDirectory = sourceDirectory;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class FileCopyErrorEventArgs : System.EventArgs
    {
        public FileInfo File
        {
            get;
            private set;
        }

        public DirectoryInfo TargetDirectory
        {
            get;
            private set;
        }

        public FileCopyErrorEventArgs(FileInfo file, DirectoryInfo targetDirectory)
        {
            this.File = file;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class FileDeletionEventArgs : System.EventArgs
    {
        public FileInfo File
        {
            get;
            private set;
        }

        public FileDeletionEventArgs(FileInfo file)
        {
            this.File = file;
        }
    }

    public class DirectoryCreationEventArgs : System.EventArgs
    {
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        public DirectoryInfo TargetDirectory
        {
            get;
            private set;
        }

        public DirectoryCreationEventArgs(DirectoryInfo directory, DirectoryInfo targetDirectory)
        {
            this.Directory = directory;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class DirectoryDeletionEventArgs : System.EventArgs
    {
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        public DirectoryDeletionEventArgs(DirectoryInfo directory)
        {
            this.Directory = directory;
        }
    } 
}
