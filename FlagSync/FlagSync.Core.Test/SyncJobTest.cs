using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for SyncJobTest and is intended
    ///to contain all SyncJobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SyncJobTest
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
        ///A test for SyncJob Constructor
        ///</summary>
        [TestMethod()]
        public void SyncJobConstructorTest()
        {
            JobSetting settings = null; // TODO: Initialize to an appropriate value
            IFileSystem fileSystem = null; // TODO: Initialize to an appropriate value
            SyncJob target = new SyncJob(settings, fileSystem);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        public void StartTest()
        {
            JobSetting settings = null; // TODO: Initialize to an appropriate value
            IFileSystem fileSystem = null; // TODO: Initialize to an appropriate value
            SyncJob target = new SyncJob(settings, fileSystem); // TODO: Initialize to an appropriate value
            bool preview = false; // TODO: Initialize to an appropriate value
            target.Start(preview);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}