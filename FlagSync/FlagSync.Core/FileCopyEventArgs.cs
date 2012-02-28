using System;
using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class FileCopyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Gets the source directory.
        /// </summary>
        /// <value>
        /// The source directory.
        /// </value>
        public IDirectoryInfo SourceDirectory { get; private set; }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>
        /// The target directory.
        /// </value>
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
}