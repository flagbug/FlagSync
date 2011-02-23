using FlagSync.Core.FileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for LocalFileCounterTest and is intended
    ///to contain all LocalFileCounterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocalFileCounterTest
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
        ///A test for LocalFileCounter Constructor
        ///</summary>
        [TestMethod()]
        public void LocalFileCounterConstructorTest()
        {
            LocalFileCounter target = new LocalFileCounter();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CountFiles
        ///</summary>
        [TestMethod()]
        public void CountFilesTest()
        {
            LocalFileCounter_Accessor target = new LocalFileCounter_Accessor(); // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            FileCounterResults expected = null; // TODO: Initialize to an appropriate value
            FileCounterResults actual;
            actual = target.CountFiles(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CountJobFiles
        ///</summary>
        [TestMethod()]
        public void CountJobFilesTest()
        {
            LocalFileCounter target = new LocalFileCounter(); // TODO: Initialize to an appropriate value
            JobSetting settings = null; // TODO: Initialize to an appropriate value
            FileCounterResults expected = null; // TODO: Initialize to an appropriate value
            FileCounterResults actual;
            actual = target.CountJobFiles(settings);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}