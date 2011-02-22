using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for FileDeletionErrorEventArgsTest and is intended
    ///to contain all FileDeletionErrorEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileDeletionErrorEventArgsTest
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
        ///A test for FileDeletionErrorEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void FileDeletionErrorEventArgsConstructorTest()
        {
            IFileInfo file = null; // TODO: Initialize to an appropriate value
            FileDeletionErrorEventArgs target = new FileDeletionErrorEventArgs(file);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for File
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void FileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileDeletionErrorEventArgs_Accessor target = new FileDeletionErrorEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileInfo expected = null; // TODO: Initialize to an appropriate value
            IFileInfo actual;
            target.File = expected;
            actual = target.File;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}