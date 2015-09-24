using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class NewUserController : Controller
    {
        private UserDebtDataEntities1 db = new UserDebtDataEntities1();

        private string userID()
        {
            string userID = "7137f5a1-0e74-46a8-83b9-30dd1f67410e";
            return userID;
        }
        
        // GET: NewUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AutoPay()
        {
            return View();
        }
        public ActionResult DebtStatus()
        {
            string newUserID = userID();

            var userSummary = db.UserCreditSummaries.Where(d => d.userID == newUserID);
            return View(userSummary.ToList());
        }
        public ActionResult Downloads()
        {
            return View();
        }
        public ActionResult EliminationStrategy()
        {
            return View();
        }
        public ActionResult InitialCreditReport()
        {
            string newUserID = userID();

            var dbList = db.UserDebtDetails.Where(d => d.userID == newUserID);
            return View(dbList.ToList());
        }
        public ActionResult VerifyCreditReport()
        {
            string newUserID = userID();

            var dbList = db.UserDebtDetails.Where(d => d.userID == newUserID);
            return View(dbList.ToList());
        }
        public ActionResult RepaymentSchedule()
        {
            string newUserID = userID();

            var dbList = db.UserDebtDetails.Where(d => d.userID == newUserID && d.balance>0);
            return View(dbList.ToList());            
        }   
    }
}