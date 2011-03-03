using System;
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

        #endregion Additional test attributes

        /// <summary>
        ///A test for JobSetting Constructor
        ///</summary>
        [TestMethod()]
        public void JobSettingConstructorTest()
        {
            JobSetting target = new JobSetting();
        }

        /// <summary>
        ///A test for JobSetting Constructor
        ///</summary>
        [TestMethod()]
        public void JobSettingConstructorTest1()
        {
            string name = "TestSetting";

            JobSetting target = new JobSetting(name);

            Assert.AreEqual(name, target.Name);

            try
            {
                target = new JobSetting(null);

                Assert.Fail("Constructor must throw argument null exception");
            }

            catch (ArgumentNullException) { }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception has been thrown: " + e.ToString());
            }
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            JobSetting target = new JobSetting("TestSetting");

            string expected = "TestSetting";
            string actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DirectoryA
        ///</summary>
        [TestMethod()]
        public void DirectoryATest()
        {
            JobSetting target = new JobSetting();

            string expected = @"C:\SomeFolderA\SomeSubFolderA";
            string actual;

            target.DirectoryA = expected;
            actual = target.DirectoryA;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DirectoryB
        ///</summary>
        [TestMethod()]
        public void DirectoryBTest()
        {
            JobSetting target = new JobSetting();

            string expected = @"C:\SomeFolderB\SomeSubFolderB";
            string actual;

            target.DirectoryB = expected;
            actual = target.DirectoryB;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsIncluded
        ///</summary>
        [TestMethod()]
        public void IsIncludedTest()
        {
            JobSetting target = new JobSetting();

            bool expected = true;
            bool actual;

            target.IsIncluded = expected;
            actual = target.IsIncluded;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            JobSetting target = new JobSetting();

            string expected = "TestJobSetting";
            string actual;

            target.Name = expected;
            actual = target.Name;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SyncMode
        ///</summary>
        [TestMethod()]
        public void SyncModeTest()
        {
            JobSetting target = new JobSetting();

            SyncMode expected = SyncMode.LocalSynchronization;
            SyncMode actual;

            target.SyncMode = expected;
            actual = target.SyncMode;

            Assert.AreEqual(expected, actual);
        }
    }
}