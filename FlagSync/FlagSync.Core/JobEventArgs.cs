using System;
using FlagLib.Extensions;
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
            file.ThrowIfNull(() => file);

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
            job.ThrowIfNull(() => job);

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
            filePath.ThrowIfNull(() => filePath);

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
            file.ThrowIfNull(() => file);
            sourceDirectory.ThrowIfNull(() => sourceDirectory);
            targetDirectory.ThrowIfNull(() => targetDirectory);

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
            file.ThrowIfNull(() => file);
            targetDirectory.ThrowIfNull(() => targetDirectory);

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
            filePath.ThrowIfNull(() => filePath);

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
            directory.ThrowIfNull(() => directory);
            targetDirectory.ThrowIfNull(() => targetDirectory);

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
            directoryPath.ThrowIfNull(() => directoryPath);

            this.DirectoryPath = directoryPath;
        }
    }
}