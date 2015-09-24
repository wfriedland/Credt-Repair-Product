using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class UserCreditSummariesController : Controller
    {
        private UserDebtDataEntities1 db = new UserDebtDataEntities1();

        // GET: UserCreditSummaries
        public ActionResult Index()
        {
            //// use this to get actual ID
            //var userID = User.Identity.GetUserId();

            // for testing use this
            var userID = "7137f5a1-0e74-46a8-83b9-30dd1f67410e";

            var filtered = db.UserCreditSummaries.Where(t => t.userID == userID);
            return View(filtered.ToList());
        }

        // GET: UserCreditSummaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCreditSummary userCreditSummary = db.UserCreditSummaries.Find(id);
            if (userCreditSummary == null)
            {
                return HttpNotFound();
            }
            return View(userCreditSummary);
        }

        // GET: UserCreditSummaries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserCreditSummaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "summaryID,totalAccounts,openAccounts,closedAccounts,totalBalance,monthlyPayments,unsatisfactoryAccounts,derogatoryAccounts,inquiresLast2Years,publicRecords,userID,reportDate,creditScore")] UserCreditSummary userCreditSummary)
        {
            if (ModelState.IsValid)
            {
                db.UserCreditSummaries.Add(userCreditSummary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userCreditSummary);
        }

        // GET: UserCreditSummaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCreditSummary userCreditSummary = db.UserCreditSummaries.Find(id);
            if (userCreditSummary == null)
            {
                return HttpNotFound();
            }
            return View(userCreditSummary);
        }

        // POST: UserCreditSummaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "summaryID,totalAccounts,openAccounts,closedAccounts,totalBalance,monthlyPayments,unsatisfactoryAccounts,derogatoryAccounts,inquiresLast2Years,publicRecords,userID,reportDate,creditScore")] UserCreditSummary userCreditSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userCreditSummary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userCreditSummary);
        }

        // GET: UserCreditSummaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCreditSummary userCreditSummary = db.UserCreditSummaries.Find(id);
            if (userCreditSummary == null)
            {
                return HttpNotFound();
            }
            return View(userCreditSummary);
        }

        // POST: UserCreditSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserCreditSummary userCreditSummary = db.UserCreditSummaries.Find(id);
            db.UserCreditSummaries.Remove(userCreditSummary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
