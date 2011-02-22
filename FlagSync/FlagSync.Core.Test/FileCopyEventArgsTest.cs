using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for FileCopyEventArgsTest and is intended
    ///to contain all FileCopyEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileCopyEventArgsTest
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
        ///A test for FileCopyEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void FileCopyEventArgsConstructorTest()
        {
            IFileInfo file = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo sourceDirectory = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo targetDirectory = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs target = new FileCopyEventArgs(file, sourceDirectory, targetDirectory);
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
            FileCopyEventArgs_Accessor target = new FileCopyEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileInfo expected = null; // TODO: Initialize to an appropriate value
            IFileInfo actual;
            target.File = expected;
            actual = target.File;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SourceDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void SourceDirectoryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs_Accessor target = new FileCopyEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            IDirectoryInfo expected = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo actual;
            target.SourceDirectory = expected;
            actual = target.SourceDirectory;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TargetDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void TargetDirectoryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileCopyEventArgs_Accessor target = new FileCopyEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            IDirectoryInfo expected = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo actual;
            target.TargetDirectory = expected;
            actual = target.TargetDirectory;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}