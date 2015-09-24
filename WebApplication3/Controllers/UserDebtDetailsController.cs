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
    public class UserDebtDetailsController : Controller
    {
        private UserDebtDataEntities1 db = new UserDebtDataEntities1();

        // GET: UserDebtDetails
        public ActionResult Index()
        {
            //// use this to get actual ID
            //var userID = User.Identity.GetUserId();

            // for testing use this
            var userID = "7137f5a1-0e74-46a8-83b9-30dd1f67410e";

            var filtered = db.UserDebtDetails.Where(t => t.userID == userID);
            return View(filtered.ToList());
        }

        // GET: UserDebtDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDebtDetail userDebtDetail = db.UserDebtDetails.Find(id);
            if (userDebtDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDebtDetail);
        }

        // GET: UserDebtDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDebtDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,autopay,autopayDate,debtBad,debtCompanyName,debtCompanyStreeAddress,debtCompanyCtiy,debtCompanyState,debtCompanyZip,debtCompanyPhone,nickname,accountNumber,accountType,accountStatus,dateOpened,dateLastReported,creditLimit,monthlyPayment,balance,pastDueBalance,paymentHistory,comments,remarks,reportDate,userID")] UserDebtDetail userDebtDetail)
        {
            if (ModelState.IsValid)
            {
                db.UserDebtDetails.Add(userDebtDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDebtDetail);
        }

        // GET: UserDebtDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDebtDetail userDebtDetail = db.UserDebtDetails.Find(id);
            if (userDebtDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDebtDetail);
        }

        // POST: UserDebtDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,autopay,autopayDate,debtBad,debtCompanyName,debtCompanyStreeAddress,debtCompanyCtiy,debtCompanyState,debtCompanyZip,debtCompanyPhone,nickname,accountNumber,accountType,accountStatus,dateOpened,dateLastReported,creditLimit,monthlyPayment,balance,pastDueBalance,paymentHistory,comments,remarks,reportDate,userID")] UserDebtDetail userDebtDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDebtDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDebtDetail);
        }

        // GET: UserDebtDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDebtDetail userDebtDetail = db.UserDebtDetails.Find(id);
            if (userDebtDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDebtDetail);
        }

        // POST: UserDebtDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDebtDetail userDebtDetail = db.UserDebtDetails.Find(id);
            db.UserDebtDetails.Remove(userDebtDetail);
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
