using System;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Virtual;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for BackupJobTest and is intended
    ///to contain all BackupJobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BackupJobTest
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
        ///A test for BackupJob Constructor
        ///</summary>
        [TestMethod()]
        public void BackupJobConstructorTest()
        {
            JobSetting settings = new JobSetting();
            IFileSystem fileSystem = new VirtualFileSystem();
            BackupJob target = new BackupJob(settings, fileSystem);

            try
            {
                target = new BackupJob(null, fileSystem);
                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }

            try
            {
                target = new BackupJob(settings, null);
                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }
        }

        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        public void StartTest()
        {
            JobSetting settings = null; // TODO: Initialize to an appropriate value
            IFileSystem fileSystem = null; // TODO: Initialize to an appropriate value
            BackupJob target = new BackupJob(settings, fileSystem); // TODO: Initialize to an appropriate value
            bool preview = false; // TODO: Initialize to an appropriate value
            target.Start(preview);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}