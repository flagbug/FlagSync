using System;
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

        #endregion Additional test attributes

        /// <summary>
        ///A test for FileProceededEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void FileProceededEventArgsConstructorTest()
        {
            string filePath = @"C:\SomeDirectory\SomeFile.txt";
            long fileLength = 2048;
            FileProceededEventArgs target = new FileProceededEventArgs(filePath, fileLength);

            Assert.AreEqual(filePath, target.FilePath);
            Assert.AreEqual(fileLength, target.FileLength);

            try
            {
                target = new FileProceededEventArgs(null, 0);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }
        }

        /// <summary>
        ///A test for FileLength
        ///</summary>
        [TestMethod()]
        public void FileLengthTest()
        {
            long size = 2048;
            FileProceededEventArgs target = new FileProceededEventArgs(@"C:\SomePath\SomeFile.txt", size);

            long expected = size;
            long actual = target.FileLength;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FilePath
        ///</summary>
        [TestMethod()]
        public void FilePathTest()
        {
            string path = @"C:\SomePath\SomeFile.txt";
            FileProceededEventArgs target = new FileProceededEventArgs(path, 2048);

            string expected = path;
            string actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }
    }
}