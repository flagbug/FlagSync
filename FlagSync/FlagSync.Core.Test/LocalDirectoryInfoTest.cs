using System.IO;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for LocalDirectoryInfoTest and is intended
    ///to contain all LocalDirectoryInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocalDirectoryInfoTest
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
        ///A test for LocalDirectoryInfo Constructor
        ///</summary>
        [TestMethod()]
        public void LocalDirectoryInfoConstructorTest()
        {
            DirectoryInfo directoryInfo = null; // TODO: Initialize to an appropriate value
            LocalDirectoryInfo target = new LocalDirectoryInfo(directoryInfo);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest()
        {
            DirectoryInfo directoryInfo = null; // TODO: Initialize to an appropriate value
            LocalDirectoryInfo target = new LocalDirectoryInfo(directoryInfo); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.FullName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            DirectoryInfo directoryInfo = null; // TODO: Initialize to an appropriate value
            LocalDirectoryInfo target = new LocalDirectoryInfo(directoryInfo); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Name;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Parent
        ///</summary>
        [TestMethod()]
        public void ParentTest()
        {
            DirectoryInfo directoryInfo = null; // TODO: Initialize to an appropriate value
            LocalDirectoryInfo target = new LocalDirectoryInfo(directoryInfo); // TODO: Initialize to an appropriate value
            IDirectoryInfo actual;
            actual = target.Parent;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}