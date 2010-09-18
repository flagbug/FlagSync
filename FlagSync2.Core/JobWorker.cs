using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FlagSync2.Core
{
    public class JobWorker
    {
        #region Private fields
        private Job currentJob;
        private Queue<Job> jobQueue = new Queue<Job>();
        #endregion

        #region Public properties
        public Job CurrentJob
        {
            get
            {
                return this.currentJob;
            }
        }
        #endregion

        #region Constructor
        public JobWorker(IEnumerable<Job> jobs)
        {
            foreach(Job job in jobs)
            {
                this.jobQueue.Enqueue(job);
            }
        }
        #endregion

        #region Public methods
        public void Start()
        {
            this.StartJob(this.jobQueue.Dequeue());
        }
        #endregion

        #region Private methods
        private void StartJob(Job job)
        {
            this.currentJob = job;
            ThreadPool.QueueUserWorkItem(this.ExecuteJob, job);
            job.Finished += new EventHandler(job_Finished);
        }

        void job_Finished(object sender, EventArgs e)
        {
            this.StartJob(this.jobQueue.Dequeue());
        }

        private void ExecuteJob(object job)
        {
            ((Job)job).Start();
        }
        #endregion
    }
}
