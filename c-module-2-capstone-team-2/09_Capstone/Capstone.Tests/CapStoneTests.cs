using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class CapstoneTests
    {
        private TransactionScope tran;
        private int campId;
        const string connString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";

        [TestInitialize]
        public void Setup()
        {
            this.tran = new TransactionScope();
            string script;

            using (StreamReader sr = new StreamReader("TestSetup.sql"))
            {
                script = sr.ReadToEnd();
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(script, conn);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    campId = Convert.ToInt32(rdr["camp_id"]);
                }
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.tran.Dispose();
        }

        [TestMethod]
        public void TestOnePark()
        {
            ParkSqlDAO dao = new ParkSqlDAO(connString);

            IList<Park> test = dao.GetAllParks();

            Assert.AreEqual(1, test.Count);
        }

        [TestMethod]
        public void TestThreeSites()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/11/11"), Convert.ToDateTime("2019/11/12"));

            Assert.AreEqual(3, test.Count);
        }

        [TestMethod]
        public void TestOverlappingRes()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/11/02"), Convert.ToDateTime("2019/11/03"));

            Assert.AreEqual(2, test.Count);
        }

        [TestMethod]
        public void TestStartBeforeRes()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/11/01"), Convert.ToDateTime("2019/11/02"));

            Assert.AreEqual(2, test.Count);
        }

        [TestMethod]
        public void TestTwoEndingAfterRes()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/11/03"), Convert.ToDateTime("2019/11/04"));

            Assert.AreEqual(2, test.Count);
        }

        [TestMethod]
        public void TestAllSitesBooked()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/12/02"), Convert.ToDateTime("2019/12/03"));

            Assert.AreEqual(0, test.Count);
        }
        [TestMethod]
        public void TestOneNightStay()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/12/01"), Convert.ToDateTime("2019/12/01"));

            Assert.AreEqual(2, test.Count);
        }

        [TestMethod]
        public void TestLongRes()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/11/01"), Convert.ToDateTime("2019/11/08"));

            Assert.AreEqual(2, test.Count);
        }
        [TestMethod]

        public void TestShortRes()
        {
            SiteSqlDAO dao = new SiteSqlDAO(connString);

            IList<Site> test = dao.ListAvailableSites(campId, Convert.ToDateTime("2019/10/15"), Convert.ToDateTime("2019/10/15"));

            Assert.AreEqual(2, test.Count);
        }

    }
}
