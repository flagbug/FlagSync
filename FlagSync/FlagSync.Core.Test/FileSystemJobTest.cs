using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for FileSystemJobTest and is intended
    ///to contain all FileSystemJobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileSystemJobTest
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

        internal virtual FileSystemJob_Accessor CreateFileSystemJob_Accessor()
        {
            // TODO: Instantiate an appropriate concrete class.
            FileSystemJob_Accessor target = null;
            return target;
        }

        /// <summary>
        ///A test for BackupDirectoryRecursively
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void BackupDirectoryRecursivelyTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            string sourceDirectoryPath = string.Empty; // TODO: Initialize to an appropriate value
            string targetDirectoryPath = string.Empty; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.BackupDirectoryRecursively(sourceDirectoryPath, targetDirectoryPath, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CheckDeletionsRecursively
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void CheckDeletionsRecursivelyTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            string sourceDirectoryPath = string.Empty; // TODO: Initialize to an appropriate value
            string targetDirectoryPath = string.Empty; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.CheckDeletionsRecursively(sourceDirectoryPath, targetDirectoryPath, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for IsFileModified
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void IsFileModifiedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileInfo fileA = null; // TODO: Initialize to an appropriate value
            IFileInfo fileB = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsFileModified(fileA, fileB);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OnProceededFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void OnProceededFileTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            FileProceededEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnProceededFile(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PerformDirectoryCreationOperation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void PerformDirectoryCreationOperationTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            IDirectoryInfo sourceDirectory = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo targetDirectory = null; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.PerformDirectoryCreationOperation(sourceDirectory, targetDirectory, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PerformDirectoryDeletionOperation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void PerformDirectoryDeletionOperationTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            IDirectoryInfo directory = null; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.PerformDirectoryDeletionOperation(directory, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PerformFileCreationOperation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void PerformFileCreationOperationTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileInfo sourceFile = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo targetDirectory = null; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.PerformFileCreationOperation(sourceFile, targetDirectory, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PerformFileDeletionOperation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void PerformFileDeletionOperationTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileInfo file = null; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.PerformFileDeletionOperation(file, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PerformFileModificationOperation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void PerformFileModificationOperationTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            FileSystemJob_Accessor target = new FileSystemJob_Accessor(param0); // TODO: Initialize to an appropriate value
            IFileInfo sourceFile = null; // TODO: Initialize to an appropriate value
            IDirectoryInfo targetDirectory = null; // TODO: Initialize to an appropriate value
            bool execute = false; // TODO: Initialize to an appropriate value
            target.PerformFileModificationOperation(sourceFile, targetDirectory, execute);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}