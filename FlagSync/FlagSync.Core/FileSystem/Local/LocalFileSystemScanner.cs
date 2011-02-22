using System;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    class LocalFileSystemScanner : IFileSystemScanner
    {
        private DirectoryScanner scanner;

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
            this.scanner.Stop();
        }

        /// <summary>
        /// Starts the scanner.
        /// </summary>
        public void Start()
        {
            this.scanner.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileSystemScanner"/> class.
        /// </summary>
        /// <param name="path">The path to search.</param>
        public LocalFileSystemScanner(string path)
        {
            this.scanner = new DirectoryScanner(path);

            this.scanner.FileFound += (sender, e) =>
                {
                    if (this.FileFound != null)
                    {
                        this.FileFound(this, new FileFoundEventArgs(new LocalFileInfo(e.File)));
                    }
                };

            this.scanner.DirectoryFound += (sender, e) =>
                {
                    if (this.DirectoryFound != null)
                    {
                        this.DirectoryFound(this,
                            new DirectoryFoundEventArgs(new LocalDirectoryInfo(e.Directory)));
                    }
                };

            this.scanner.DirectoryProceeded += (sender, e) =>
                {
                    if (this.DirectoryProceeded != null)
                    {
                        this.DirectoryProceeded(this, EventArgs.Empty);
                    }
                };
        }
    }
}