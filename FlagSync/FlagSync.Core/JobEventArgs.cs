using System;
using Rareform.Extensions;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for events of the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class JobEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the job.
        /// </summary>
        public Job Job { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobEventArgs"/> class.
        /// </summary>
        /// <param name="job">The job.</param>
        public JobEventArgs(Job job)
        {
            job.ThrowIfNull(() => job);

            this.Job = job;
        }
    }
}