using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlagSync.Core.Test
{
    /// <summary>
    ///This is a test class for JobSettingTest and is intended
    ///to contain all JobSettingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobSettingTest
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
        ///A test for JobSetting Constructor
        ///</summary>
        [TestMethod()]
        public void JobSettingConstructorTest()
        {
            JobSetting target = new JobSetting();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for JobSetting Constructor
        ///</summary>
        [TestMethod()]
        public void JobSettingConstructorTest1()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            JobSetting target = new JobSetting(name);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            JobSetting target = new JobSetting(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DirectoryA
        ///</summary>
        [TestMethod()]
        public void DirectoryATest()
        {
            JobSetting target = new JobSetting(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DirectoryA = expected;
            actual = target.DirectoryA;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DirectoryB
        ///</summary>
        [TestMethod()]
        public void DirectoryBTest()
        {
            JobSetting target = new JobSetting(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DirectoryB = expected;
            actual = target.DirectoryB;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsIncluded
        ///</summary>
        [TestMethod()]
        public void IsIncludedTest()
        {
            JobSetting target = new JobSetting(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.IsIncluded = expected;
            actual = target.IsIncluded;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            JobSetting target = new JobSetting(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SyncMode
        ///</summary>
        [TestMethod()]
        public void SyncModeTest()
        {
            JobSetting target = new JobSetting(); // TODO: Initialize to an appropriate value
            SyncMode expected = new SyncMode(); // TODO: Initialize to an appropriate value
            SyncMode actual;
            target.SyncMode = expected;
            actual = target.SyncMode;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}