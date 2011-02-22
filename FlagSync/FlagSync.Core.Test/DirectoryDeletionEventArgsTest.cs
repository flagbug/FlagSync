using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for DirectoryDeletionEventArgsTest and is intended
    ///to contain all DirectoryDeletionEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DirectoryDeletionEventArgsTest
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

        #endregion

        /// <summary>
        ///A test for DirectoryDeletionEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void DirectoryDeletionEventArgsConstructorTest()
        {
            string directoryPath = string.Empty; // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs target = new DirectoryDeletionEventArgs(directoryPath);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for DirectoryPath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void DirectoryPathTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            DirectoryDeletionEventArgs_Accessor target = new DirectoryDeletionEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DirectoryPath = expected;
            actual = target.DirectoryPath;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}