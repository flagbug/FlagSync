using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for FileProceededEventArgsTest and is intended
    ///to contain all FileProceededEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileProceededEventArgsTest
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
        ///A test for FileProceededEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void FileProceededEventArgsConstructorTest()
        {
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            long fileLength = 0; // TODO: Initialize to an appropriate value
            FileProceededEventArgs target = new FileProceededEventArgs(filePath, fileLength);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for FileLength
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void FileLengthTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileProceededEventArgs_Accessor target = new FileProceededEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.FileLength = expected;
            actual = target.FileLength;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FilePath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void FilePathTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileProceededEventArgs_Accessor target = new FileProceededEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FilePath = expected;
            actual = target.FilePath;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}