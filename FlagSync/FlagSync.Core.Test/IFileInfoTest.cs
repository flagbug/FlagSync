using System;
using FlagSync.Core.FileSystem.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for IFileInfoTest and is intended
    ///to contain all IFileInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IFileInfoTest
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

        internal virtual IFileInfo CreateIFileInfo()
        {
            // TODO: Instantiate an appropriate concrete class.
            IFileInfo target = null;
            return target;
        }

        /// <summary>
        ///A test for Directory
        ///</summary>
        [TestMethod()]
        public void DirectoryTest()
        {
            IFileInfo target = CreateIFileInfo(); // TODO: Initialize to an appropriate value
            IDirectoryInfo actual;
            actual = target.Directory;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LastWriteTime
        ///</summary>
        [TestMethod()]
        public void LastWriteTimeTest()
        {
            IFileInfo target = CreateIFileInfo(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = target.LastWriteTime;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Length
        ///</summary>
        [TestMethod()]
        public void LengthTest()
        {
            IFileInfo target = CreateIFileInfo(); // TODO: Initialize to an appropriate value
            long actual;
            actual = target.Length;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}