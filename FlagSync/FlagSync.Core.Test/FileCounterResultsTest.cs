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

        #endregion Additional test attributes

        /// <summary>
        ///A test for FileCounterResults Constructor
        ///</summary>
        [TestMethod()]
        public void FileCounterResultsConstructorTest()
        {
            FileCounterResults target = new FileCounterResults();

            Assert.AreEqual(0, target.CountedBytes);
            Assert.AreEqual(0, target.CountedFiles);
        }

        /// <summary>
        ///A test for FileCounterResults Constructor
        ///</summary>
        [TestMethod()]
        public void FileCounterResultsConstructorTest1()
        {
            int countedFiles = 50;
            long countedBytes = 2048;
            FileCounterResults target = new FileCounterResults(countedFiles, countedBytes);
        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod()]
        public void op_AdditionTest()
        {
            FileCounterResults a = new FileCounterResults(50, 2048);
            FileCounterResults b = new FileCounterResults(10, 1024);

            FileCounterResults expected = new FileCounterResults(60, 3072);
            FileCounterResults actual = (a + b);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CountedBytes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void CountedBytesTest()
        {
            FileCounterResults target = new FileCounterResults(50, 2048);

            long expected = 2048;
            long actual = target.CountedBytes;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CountedFiles
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void CountedFilesTest()
        {
            FileCounterResults target = new FileCounterResults(50, 2048);

            int expected = 50;
            int actual = target.CountedFiles;

            Assert.AreEqual(expected, actual);
        }
    }
}