using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication3.Models;

namespace CRPTest.Controllers
{
    /// <summary>
    /// Summary description for StateMachineParse1
    /// </summary>
    [TestClass]
    public class StateMachineParse1
    {
        public StateMachineParse1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void SMParse1()
        {
            creditReportReader cr = new creditReportReader();
            List<UserDebtDetail> debtDetails;

            if (cr.init("creditReport.html") == true)
            {
                cr.parse();
                debtDetails = cr.getCreditDetails();
                Assert.AreEqual(debtDetails.Count, 6);
            }



        }
        [TestMethod]
        public void SMParse2()
        {
            creditReportReader cr = new creditReportReader();
            UserCreditSummary creditSum;

            if (cr.init("creditReport.html") == true)
            {
                cr.parse();
                creditSum = cr.getCreditSummary();
                Assert.AreEqual(creditSum.unsatisfactoryAccounts  , 5);
            }



        }
        [TestMethod]
        public void SMParse3()
        {
            List<UserDebtDetail> debtDetails;
            creditReportReader cr = new creditReportReader();

            if (cr.init("creditReport.html") == true)
            {
                cr.parse();
                debtDetails = cr.getCreditDetails();
                var balance = debtDetails.Find(x => x.debtCompanyName == "capital one").balance;
                Assert  .AreEqual(balance, 201);
            }



        }
    }
}
