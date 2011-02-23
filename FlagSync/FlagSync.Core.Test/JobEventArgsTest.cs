using System;
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

        #endregion Additional test attributes

        /// <summary>
        ///A test for JobEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void JobEventArgsConstructorTest()
        {
            JobSetting job = new JobSetting();

            JobEventArgs target = new JobEventArgs(job);

            try
            {
                target = new JobEventArgs(null);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }
        }

        /// <summary>
        ///A test for Job
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FlagSync.Core.dll")]
        public void JobTest()
        {
            JobSetting setting = new JobSetting("TestSetting");
            JobEventArgs target = new JobEventArgs(setting);

            JobSetting expected = setting;
            JobSetting actual = target.Job;

            Assert.AreEqual(expected, actual);
        }
    }
}