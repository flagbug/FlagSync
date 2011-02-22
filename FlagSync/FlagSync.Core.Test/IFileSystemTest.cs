using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for IFileSystemTest and is intended
    ///to contain all IFileSystemTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IFileSystemTest
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

        internal virtual IFileSystem CreateIFileSystem()
        {
            // TODO: Instantiate an appropriate concrete class.
            IFileSystem target = null;
            return target;
        }

        /// <summary>
        ///A test for CreateDirectoryInfo
        ///</summary>
        [TestMethod()]
        public void CreateDirectoryInfoTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            IDirectoryInfo expected = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo actual;
            actual = target.CreateDirectoryInfo(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateFileCounter
        ///</summary>
        [TestMethod()]
        public void CreateFileCounterTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            IFileCounter expected = null; // TODO: Initialize to an appropriate value
            IFileCounter actual;
            actual = target.CreateFileCounter();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateFileInfo
        ///</summary>
        [TestMethod()]
        public void CreateFileInfoTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            IFileInfo expected = null; // TODO: Initialize to an appropriate value
            IFileInfo actual;
            actual = target.CreateFileInfo(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateFileSystemScanner
        ///</summary>
        [TestMethod()]
        public void CreateFileSystemScannerTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            IFileSystemScanner expected = null; // TODO: Initialize to an appropriate value
            IFileSystemScanner actual;
            actual = target.CreateFileSystemScanner(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DirectoryExists
        ///</summary>
        [TestMethod()]
        public void DirectoryExistsTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.DirectoryExists(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FileExists
        ///</summary>
        [TestMethod()]
        public void FileExistsTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.FileExists(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryCopyFile
        ///</summary>
        [TestMethod()]
        public void TryCopyFileTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            IFileInfo sourceFile = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo targetDirectory = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TryCopyFile(sourceFile, targetDirectory);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryCreateDirectory
        ///</summary>
        [TestMethod()]
        public void TryCreateDirectoryTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            IDirectoryInfo sourceDirectory = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo targetDirectory = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TryCreateDirectory(sourceDirectory, targetDirectory);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryDeleteDirectory
        ///</summary>
        [TestMethod()]
        public void TryDeleteDirectoryTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            IDirectoryInfo directory = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TryDeleteDirectory(directory);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryDeleteFile
        ///</summary>
        [TestMethod()]
        public void TryDeleteFileTest()
        {
            IFileSystem target = CreateIFileSystem(); // TODO: Initialize to an appropriate value
            IFileSystemInfo file = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.TryDeleteFile(file);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}