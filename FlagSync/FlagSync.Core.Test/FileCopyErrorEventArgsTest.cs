using System;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Virtual;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for FileCopyErrorEventArgsTest and is intended
    ///to contain all FileCopyErrorEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileCopyErrorEventArgsTest
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
        ///A test for FileCopyErrorEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void FileCopyErrorEventArgsConstructorTest()
        {
            VirtualDirectoryInfo rootDirectory = new VirtualDirectoryInfo("Root", null, false, true);
            IFileInfo file = new VirtualFileInfo("TestFile", 1024, DateTime.Now, rootDirectory);
            IDirectoryInfo targetDirectory = new VirtualDirectoryInfo("TestDirectory", null, false, true);

            FileCopyErrorEventArgs target = new FileCopyErrorEventArgs(file, targetDirectory);

            try
            {
                target = new FileCopyErrorEventArgs(null, targetDirectory);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }

            try
            {
                target = new FileCopyErrorEventArgs(file, null);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }
        }

        /// <summary>
        ///A test for File
        ///</summary>
        [TestMethod()]
        public void FileTest()
        {
            VirtualDirectoryInfo rootDirectory = new VirtualDirectoryInfo("Root", null, false, true);
            IFileInfo file = new VirtualFileInfo("TestFile", 1024, DateTime.Now, rootDirectory);
            IDirectoryInfo targetDirectory = new VirtualDirectoryInfo("TestDirectory", null, false, true);

            FileCopyErrorEventArgs target = new FileCopyErrorEventArgs(file, targetDirectory);

            IFileInfo expected = file;
            IFileInfo actual = target.File;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TargetDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void TargetDirectoryTest()
        {
            VirtualDirectoryInfo rootDirectory = new VirtualDirectoryInfo("Root", null, false, true);
            IFileInfo file = new VirtualFileInfo("TestFile", 1024, DateTime.Now, rootDirectory);
            IDirectoryInfo targetDirectory = new VirtualDirectoryInfo("TestDirectory", null, false, true);

            FileCopyErrorEventArgs target = new FileCopyErrorEventArgs(file, targetDirectory);

            IDirectoryInfo expected = targetDirectory;
            IDirectoryInfo actual = target.TargetDirectory;

            Assert.AreEqual(expected, actual);
        }
    }
}