using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using FlagSync.Core.FileSystem.Base;
using Rareform.Extensions;

namespace FlagSync.Core.FileSystem
{
    internal class FileSystemScanner
    {
        private readonly IDirectoryInfo rootDirectory;

        /// <summary>
        /// Gets or sets a value indicating whether the scanner is stopped.
        /// </summary>
        /// <value>true if the scanner is stopped; otherwise, false.</value>
        public bool IsStopped { get; private set; }

        /// <summary>
        /// Occurs when a file has been found.
        /// </summary>
        public event EventHandler<FileFoundEventArgs> FileFound;

        /// <summary>
        /// Occurs when a directory has been found.
        /// </summary>
        public event EventHandler<DirectoryFoundEventArgs> DirectoryFound;

        /// <summary>
        /// Occurs when a directory has been proceeded.
        /// </summary>
        public event EventHandler DirectoryProceeded;

        /// <summary>
        /// Stops the scanner.
        /// </summary>
        public void Stop()
        {
            this.IsStopped = true;
        }

        /// <summary>
        /// Starts the scanner.
        /// </summary>
        public void Start()
        {
            this.ScanDirectories(this.rootDirectory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemScanner"/> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory.</param>
        /// <exception cref="System.ArgumentException">
        /// The exception that is thrown, if the root directory doesn't exists
        /// </exception>
        public FileSystemScanner(IDirectoryInfo rootDirectory)
        {
            rootDirectory.ThrowIfNull(() => rootDirectory);

            if (!rootDirectory.Exists)
                throw new ArgumentException("The root directory must exist!");

            this.rootDirectory = rootDirectory;
        }

        /// <summary>
        /// Raises the <see cref="FileFound"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FileFoundEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileFound(FileFoundEventArgs e)
        {
            this.FileFound.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DirectoryFound"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DirectoryFoundEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryFound(DirectoryFoundEventArgs e)
        {
            this.DirectoryFound.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DirectoryProceeded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryProceeded(EventArgs e)
        {
            this.DirectoryProceeded.RaiseSafe(this, e);
        }

        /// <summary>
        /// Scans a directory recursively.
        /// </summary>
        /// <param name="rootDirectory">The root directory.</param>
        private void ScanDirectories(IDirectoryInfo rootDirectory)
        {
            if (this.IsStopped) { return; }

            try
            {
                if (rootDirectory.Exists)
                {
                    IEnumerable<IFileInfo> files = rootDirectory.GetFiles();

                    foreach (IFileInfo file in files)
                    {
                        if (this.IsStopped) { return; }

                        if (file.Exists)
                        {
                            this.OnFileFound(new FileFoundEventArgs(file));
                        }
                    }

                    IEnumerable<IDirectoryInfo> directories = rootDirectory.GetDirectories();

                    foreach (IDirectoryInfo directory in directories)
                    {
                        if (this.IsStopped)
                        {
                            return;
                        }

                        if (directory.Name == "$RECYCLE.BIN" || !directory.Exists)
                        {
                            continue;
                        }

                        this.OnDirectoryFound(new DirectoryFoundEventArgs(directory));
                        this.ScanDirectories(directory);
                    }
                }
            }

            // Catch the exceptions and don't handle anything,
            // we want to skip those files or directories
            catch (UnauthorizedAccessException)
            {
            }

            catch (SecurityException)
            {
            }

            catch (IOException)
            {
            }

            this.OnDirectoryProceeded(EventArgs.Empty);
        }
    }
}