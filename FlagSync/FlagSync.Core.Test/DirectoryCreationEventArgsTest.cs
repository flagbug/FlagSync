using System;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Virtual;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for DirectoryCreationEventArgsTest and is intended
    ///to contain all DirectoryCreationEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DirectoryCreationEventArgsTest
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
        ///A test for DirectoryCreationEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void DirectoryCreationEventArgsConstructorTest()
        {
            IDirectoryInfo directory = new VirtualDirectoryInfo("TestDirectory", null, false, true);
            IDirectoryInfo targetDirectory = new VirtualDirectoryInfo("TestTarget", null, false, true);

            DirectoryCreationEventArgs target = new DirectoryCreationEventArgs(directory, targetDirectory);

            try
            {
                target = new DirectoryCreationEventArgs(null, targetDirectory);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }

            try
            {
                target = new DirectoryCreationEventArgs(directory, null);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }
        }

        /// <summary>
        ///A test for Directory
        ///</summary>
        [TestMethod()]
        public void DirectoryTest()
        {
            IDirectoryInfo directory = new VirtualDirectoryInfo("TestDirectory", null, false, true);
            IDirectoryInfo targetDirectory = new VirtualDirectoryInfo("TestTarget", null, false, true);

            DirectoryCreationEventArgs target = new DirectoryCreationEventArgs(directory, targetDirectory);

            IDirectoryInfo expected = directory;
            IDirectoryInfo actual = target.Directory;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TargetDirectory
        ///</summary>
        [TestMethod()]
        public void TargetDirectoryTest()
        {
            IDirectoryInfo directory = new VirtualDirectoryInfo("TestDirectory", null, false, true);
            IDirectoryInfo targetDirectory = new VirtualDirectoryInfo("TestTarget", null, false, true);

            DirectoryCreationEventArgs target = new DirectoryCreationEventArgs(directory, targetDirectory);

            IDirectoryInfo expected = targetDirectory;
            IDirectoryInfo actual = target.TargetDirectory;

            Assert.AreEqual(expected, actual);
        }
    }
}