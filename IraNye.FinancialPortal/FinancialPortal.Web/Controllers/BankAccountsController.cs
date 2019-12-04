using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Households");
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || bankAccount.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }

            return View(bankAccount);
        }

        public ActionResult Create()
        {
            int householdId = HelperMethods.GetCurrentUserHouseholdId().GetValueOrDefault();
            if (householdId == 0)
            {
                return RedirectToAction("Index", "Households");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,AccountType,StartingBalance,CurrentBalance,LowBalanceLevel")] BankAccount bankAccount)
        {
            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || currentUserHouseholdId == 0)
            {
                return RedirectToAction("Index", "Households");
            }

            bankAccount.HouseholdId = currentUserHouseholdId.GetValueOrDefault();
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return RedirectToAction("Index", "Households");
                }

                bankAccount.OwnerId = userId;
                bankAccount.Created = DateTime.Now;
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Households", new {id=currentUserHouseholdId});
            }

            return View(bankAccount);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || bankAccount.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }
            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,AccountType,StartingBalance,CurrentBalance,LowBalanceLevel")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || bankAccount.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Households");
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Households");
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
