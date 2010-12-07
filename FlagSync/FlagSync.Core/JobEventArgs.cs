using System.IO;

namespace FlagSync.Core
{
    public class FileDeletionErrorEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public FileInfo File
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeletionErrorEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileDeletionErrorEventArgs(FileInfo file)
        {
            this.File = file;
        }
    }

    public class JobEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <value>The job.</value>
        public JobSetting Job
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobEventArgs"/> class.
        /// </summary>
        /// <param name="job">The job.</param>
        public JobEventArgs(JobSetting job)
        {
            this.Job = job;
        }
    }

    public class FileProceededEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public FileInfo File
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProceededEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileProceededEventArgs(FileInfo file)
        {
            this.File = file;
        }
    }

    public class FileCopyEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public FileInfo File
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source directory.
        /// </summary>
        /// <value>The source directory.</value>
        public DirectoryInfo SourceDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public DirectoryInfo TargetDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopyEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public FileCopyEventArgs(FileInfo file, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            this.File = file;
            this.SourceDirectory = sourceDirectory;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class FileCopyErrorEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public FileInfo File
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public DirectoryInfo TargetDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopyErrorEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public FileCopyErrorEventArgs(FileInfo file, DirectoryInfo targetDirectory)
        {
            this.File = file;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class FileDeletionEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public FileInfo File
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeletionEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileDeletionEventArgs(FileInfo file)
        {
            this.File = file;
        }
    }

    public class DirectoryCreationEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public DirectoryInfo TargetDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCreationEventArgs"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public DirectoryCreationEventArgs(DirectoryInfo directory, DirectoryInfo targetDirectory)
        {
            this.Directory = directory;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class DirectoryDeletionEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryDeletionEventArgs"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public DirectoryDeletionEventArgs(DirectoryInfo directory)
        {
            this.Directory = directory;
        }
    }
}