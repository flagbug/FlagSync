using System;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for JobTest and is intended
    ///to contain all JobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobTest
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

        internal virtual Job_Accessor CreateJob_Accessor()
        {
            // TODO: Instantiate an appropriate concrete class.
            Job_Accessor target = null;
            return target;
        }

        /// <summary>
        ///A test for CheckPause
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void CheckPauseTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            target.CheckPause();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        internal virtual Job CreateJob()
        {
            // TODO: Instantiate an appropriate concrete class.
            Job target = null;
            return target;
        }

        /// <summary>
        ///A test for Continue
        ///</summary>
        [TestMethod()]
        public void ContinueTest()
        {
            Job target = CreateJob(); // TODO: Initialize to an appropriate value
            target.Continue();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnCreatedDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnCreatedDirectoryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectoryCreationEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnCreatedDirectory(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnCreatedFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnCreatedFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnCreatedFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnCreatingDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnCreatingDirectoryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectoryCreationEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnCreatingDirectory(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnCreatingFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnCreatingFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnCreatingFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnDeletedDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnDeletedDirectoryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDeletedDirectory(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnDeletedFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnDeletedFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDeletedFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnDeletingDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnDeletingDirectoryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDeletingDirectory(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnDeletingFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnDeletingFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDeletingFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnDirectoryCreationError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnDirectoryCreationErrorTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectoryCreationEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDirectoryCreationError(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnDirectoryDeletionError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnDirectoryDeletionErrorTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDirectoryDeletionError(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnFileCopyError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnFileCopyErrorTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileCopyErrorEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnFileCopyError(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnFileDeletionError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnFileDeletionErrorTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileDeletionErrorEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnFileDeletionError(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnFileProgressChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnFileProgressChangedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            CopyProgressEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnFileProgressChanged(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnFinished
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnFinishedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnFinished(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnModifiedFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnModifiedFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnModifiedFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnModifyingFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnModifyingFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileCopyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnModifyingFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnProceededFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnProceededFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            FileProceededEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnProceededFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Pause
        ///</summary>
        [TestMethod()]
        public void PauseTest()
        {
            Job target = CreateJob(); // TODO: Initialize to an appropriate value
            target.Pause();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        public void StartTest()
        {
            Job target = CreateJob(); // TODO: Initialize to an appropriate value
            bool preview = false; // TODO: Initialize to an appropriate value
            target.Start(preview);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            Job target = CreateJob(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FileSystem
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void FileSystemTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileSystem_Accessor expected = null; // TODO: Initialize to an appropriate value
            IFileSystem_Accessor actual;
            target.FileSystem = expected;
            actual = target.FileSystem;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsPaused
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void IsPausedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.IsPaused = expected;
            actual = target.IsPaused;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsStopped
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void IsStoppedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.IsStopped = expected;
            actual = target.IsStopped;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Settings
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void SettingsTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            JobSetting expected = null; // TODO: Initialize to an appropriate value
            JobSetting actual;
            target.Settings = expected;
            actual = target.Settings;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WrittenBytes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void WrittenBytesTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Job_Accessor target = new Job_Accessor(param0); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.WrittenBytes = expected;
            actual = target.WrittenBytes;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}