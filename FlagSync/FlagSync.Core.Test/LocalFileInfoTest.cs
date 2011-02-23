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
            string path = @"C:\SomeFolder\SomeFile.txt";
            FileInfo fileInfo = new FileInfo(path);

            LocalFileInfo target = new LocalFileInfo(fileInfo);

            try
            {
                target = new LocalFileInfo(null);

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
            FileInfo fileInfo = new FileInfo(@"C:\SomeFolder\SomeFile.txt");

            LocalFileInfo target = new LocalFileInfo(fileInfo);
            IDirectoryInfo actual = target.Directory;

            Assert.AreEqual(@"C:\SomeFolder", actual.FullName);
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest()
        {
            FileInfo fileInfo = new FileInfo(@"C:\SomeFolder\SomeFile.txt");
            LocalFileInfo target = new LocalFileInfo(fileInfo);

            string actual = target.FullName;

            Assert.AreEqual(@"C:\SomeFolder\SomeFile.txt", actual);
        }

        /// <summary>
        ///A test for LastWriteTime
        ///</summary>
        [TestMethod()]
        public void LastWriteTimeTest()
        {
            FileInfo fileInfo = new FileInfo(@"C:\SomeFolder\SomeFile.txt");
            LocalFileInfo target = new LocalFileInfo(fileInfo);

            DateTime actual = target.LastWriteTime;

            Assert.AreEqual(fileInfo.LastWriteTime, actual);
        }

        /// <summary>
        ///A test for Length
        ///</summary>
        [TestMethod()]
        public void LengthTest()
        {
            FileInfo fileInfo = new FileInfo(@"C:\SomeFolder\SomeFile.txt");
            LocalFileInfo target = new LocalFileInfo(fileInfo);

            long actual = target.Length;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            FileInfo fileInfo = new FileInfo(@"C:\SomeFolder\SomeFile.txt");
            LocalFileInfo target = new LocalFileInfo(fileInfo);

            string actual = target.Name;

            Assert.AreEqual("SomeFile.txt", actual);
        }
    }
}