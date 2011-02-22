using System;
using System.Collections.Generic;
using FlagLib.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for JobWorkerTest and is intended
    ///to contain all JobWorkerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobWorkerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        /// <summary>
        ///A test for JobWorker Constructor
        ///</summary>
        [TestMethod()]
        public void JobWorkerConstructorTest()
        {
            JobWorker target = new JobWorker();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Continue
        ///</summary>
        [TestMethod()]
        public void ContinueTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            target.Continue();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DoNextJob
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void DoNextJobTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            target.DoNextJob();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetFileCounterResults
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void GetFileCounterResultsTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            FileCounterResults expected = null; // TODO: Initialize to an appropriate value
            FileCounterResults actual;
            actual = target.GetFileCounterResults();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InitializeJobEvents
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void InitializeJobEventsTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            Job_Accessor job = null; // TODO: Initialize to an appropriate value
            target.InitializeJobEvents(job);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnFinished
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnFinishedTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnFinished(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnJobFinished
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnJobFinishedTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            JobEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnJobFinished(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnJobStarted
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnJobStartedTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            JobEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnJobStarted(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Pause
        ///</summary>
        [TestMethod()]
        public void PauseTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            target.Pause();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for QueueJobs
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void QueueJobsTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            IEnumerable<JobSetting> jobs = null; // TODO: Initialize to an appropriate value
            target.QueueJobs(jobs);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void StartTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            bool preview = false; // TODO: Initialize to an appropriate value
            target.Start(preview);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        public void StartTest1()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            IEnumerable<JobSetting> jobSettings = null; // TODO: Initialize to an appropriate value
            bool preview = false; // TODO: Initialize to an appropriate value
            target.Start(jobSettings, preview);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_CreatedDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_CreatedDirectoryTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DirectoryCreationEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_CreatedDirectory(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_CreatedFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_CreatedFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_CreatedFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_CreatingDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_CreatingDirectoryTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DirectoryCreationEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_CreatingDirectory(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_CreatingFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_CreatingFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_CreatingFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_DeletedDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_DeletedDirectoryTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_DeletedDirectory(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_DeletedFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_DeletedFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_DeletedFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_DeletingDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_DeletingDirectoryTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_DeletingDirectory(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_DeletingFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_DeletingFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_DeletingFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_DirectoryDeletionError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_DirectoryDeletionErrorTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_DirectoryDeletionError(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_FileCopyError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_FileCopyErrorTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileCopyErrorEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_FileCopyError(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_FileCopyProgressChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_FileCopyProgressChangedTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CopyProgressEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_FileCopyProgressChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_FileDeletionError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_FileDeletionErrorTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileDeletionErrorEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_FileDeletionError(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_Finished
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_FinishedTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_Finished(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_ModifiedFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_ModifiedFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_ModifiedFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_ModifyingFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_ModifyingFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_ModifyingFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for currentJob_ProceededFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void currentJob_ProceededFileTest()
        {
            JobWorker_Accessor target = new JobWorker_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FileProceededEventArgs e = null; // TODO: Initialize to an appropriate value
            target.currentJob_ProceededFile(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FileCounterResult
        ///</summary>
        [TestMethod()]
        public void FileCounterResultTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            FileCounterResults actual;
            actual = target.FileCounterResult;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsPaused
        ///</summary>
        [TestMethod()]
        public void IsPausedTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsPaused;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ProceededFiles
        ///</summary>
        [TestMethod()]
        public void ProceededFilesTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.ProceededFiles;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TotalWrittenBytes
        ///</summary>
        [TestMethod()]
        public void TotalWrittenBytesTest()
        {
            JobWorker target = new JobWorker(); // TODO: Initialize to an appropriate value
            long actual;
            actual = target.TotalWrittenBytes;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}