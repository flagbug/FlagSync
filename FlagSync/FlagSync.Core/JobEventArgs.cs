using System;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core
{
    public class FileDeletionErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeletionErrorEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileDeletionErrorEventArgs(IFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            this.File = file;
        }
    }

    public class JobEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <value>The job.</value>
        public JobSetting Job { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobEventArgs"/> class.
        /// </summary>
        /// <param name="job">The job.</param>
        public JobEventArgs(JobSetting job)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            this.Job = job;
        }
    }

    public class FileProceededEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        public long FileLength { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProceededEventArgs"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileLength">The file length.</param>
        public FileProceededEventArgs(string filePath, long fileLength)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            this.FilePath = filePath;
            this.FileLength = fileLength;
        }
    }

    public class FileCopyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Gets the source directory.
        /// </summary>
        /// <value>The source directory.</value>
        public IDirectoryInfo SourceDirectory { get; private set; }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public IDirectoryInfo TargetDirectory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopyEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public FileCopyEventArgs(IFileInfo file, IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (sourceDirectory == null)
                throw new ArgumentNullException("sourceDirectory");

            if (targetDirectory == null)
                throw new ArgumentNullException("targetDirectory");

            this.File = file;
            this.SourceDirectory = sourceDirectory;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class FileCopyErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public IDirectoryInfo TargetDirectory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopyErrorEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public FileCopyErrorEventArgs(IFileInfo file, IDirectoryInfo targetDirectory)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (targetDirectory == null)
                throw new ArgumentNullException("targetDirectory");

            this.File = file;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class FileDeletionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long FileSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeletionEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file path.</param>
        public FileDeletionEventArgs(string filePath, long fileSize)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            this.FilePath = filePath;
            this.FileSize = fileSize;
        }
    }

    public class DirectoryCreationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public IDirectoryInfo Directory { get; private set; }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public IDirectoryInfo TargetDirectory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCreationEventArgs"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public DirectoryCreationEventArgs(IDirectoryInfo directory, IDirectoryInfo targetDirectory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (targetDirectory == null)
                throw new ArgumentNullException("targetDirectory");

            this.Directory = directory;
            this.TargetDirectory = targetDirectory;
        }
    }

    public class DirectoryDeletionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <value>The directory path.</value>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryDeletionEventArgs"/> class.
        /// </summary>
        /// <param name="directory">The directory path.</param>
        public DirectoryDeletionEventArgs(string directoryPath)
        {
            if (directoryPath == null)
                throw new ArgumentNullException("directoryPath");

            this.DirectoryPath = directoryPath;
        }
    }
}