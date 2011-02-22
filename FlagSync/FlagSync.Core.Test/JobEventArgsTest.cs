using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for JobEventArgsTest and is intended
    ///to contain all JobEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobEventArgsTest
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
        ///A test for JobEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void JobEventArgsConstructorTest()
        {
            JobSetting job = null; // TODO: Initialize to an appropriate value
            JobEventArgs target = new JobEventArgs(job);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Job
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void JobTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            JobEventArgs_Accessor target = new JobEventArgs_Accessor(param0); // TODO: Initialize to an appropriate value
            JobSetting expected = null; // TODO: Initialize to an appropriate value
            JobSetting actual;
            target.Job = expected;
            actual = target.Job;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}