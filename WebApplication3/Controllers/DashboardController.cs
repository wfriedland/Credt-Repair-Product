using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Verify()
        {
            return View();
        }
        public ActionResult Documents()
        {
            return View();
        }
        public ActionResult UpdateUserInfo()
        {
            // will need to set up a class User and pass in most of the information here to auto fill the fields in the update page
            return View();
        }
        public ActionResult Calendar()
        {
            return View();
        }
        public ActionResult AutopayUpdateForm()
        {
            return View();
        }
        public ActionResult DebtStatus()
        {
            return View();
        }
        public ActionResult RepaymentSchedule()
        {
            return View();
        }
        public ActionResult NewCreditReport()
        {
            return View();
        }
    }
}