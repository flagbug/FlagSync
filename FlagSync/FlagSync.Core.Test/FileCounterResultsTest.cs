using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for FileCounterResultsTest and is intended
    ///to contain all FileCounterResultsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileCounterResultsTest
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

        #endregion

        /// <summary>
        ///A test for FileCounterResults Constructor
        ///</summary>
        [TestMethod()]
        public void FileCounterResultsConstructorTest()
        {
            FileCounterResults target = new FileCounterResults();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for FileCounterResults Constructor
        ///</summary>
        [TestMethod()]
        public void FileCounterResultsConstructorTest1()
        {
            int countedFiles = 0; // TODO: Initialize to an appropriate value
            long countedBytes = 0; // TODO: Initialize to an appropriate value
            FileCounterResults target = new FileCounterResults(countedFiles, countedBytes);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod()]
        public void op_AdditionTest()
        {
            FileCounterResults a = null; // TODO: Initialize to an appropriate value
            FileCounterResults b = null; // TODO: Initialize to an appropriate value
            FileCounterResults expected = null; // TODO: Initialize to an appropriate value
            FileCounterResults actual;
            actual = (a + b);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CountedBytes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void CountedBytesTest()
        {
            FileCounterResults_Accessor target = new FileCounterResults_Accessor(); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.CountedBytes = expected;
            actual = target.CountedBytes;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CountedFiles
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void CountedFilesTest()
        {
            FileCounterResults_Accessor target = new FileCounterResults_Accessor(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CountedFiles = expected;
            actual = target.CountedFiles;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}