using System;
using System.IO;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for LocalFileInfoTest and is intended
    ///to contain all LocalFileInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocalFileInfoTest
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
        ///A test for LocalFileInfo Constructor
        ///</summary>
        [TestMethod()]
        public void LocalFileInfoConstructorTest()
        {
            FileInfo fileInfo = null; // TODO: Initialize to an appropriate value
            LocalFileInfo target = new LocalFileInfo(fileInfo);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Directory
        ///</summary>
        [TestMethod()]
        public void DirectoryTest()
        {
            FileInfo fileInfo = null; // TODO: Initialize to an appropriate value
            LocalFileInfo target = new LocalFileInfo(fileInfo); // TODO: Initialize to an appropriate value
            IDirectoryInfo actual;
            actual = target.Directory;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest()
        {
            FileInfo fileInfo = null; // TODO: Initialize to an appropriate value
            LocalFileInfo target = new LocalFileInfo(fileInfo); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.FullName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LastWriteTime
        ///</summary>
        [TestMethod()]
        public void LastWriteTimeTest()
        {
            FileInfo fileInfo = null; // TODO: Initialize to an appropriate value
            LocalFileInfo target = new LocalFileInfo(fileInfo); // TODO: Initialize to an appropriate value
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
            FileInfo fileInfo = null; // TODO: Initialize to an appropriate value
            LocalFileInfo target = new LocalFileInfo(fileInfo); // TODO: Initialize to an appropriate value
            long actual;
            actual = target.Length;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            FileInfo fileInfo = null; // TODO: Initialize to an appropriate value
            LocalFileInfo target = new LocalFileInfo(fileInfo); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Name;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}