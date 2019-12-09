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
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = _db.BankAccounts.Find(id);
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

            bankAccount.Owner = _db.Users.Find(bankAccount.OwnerId);
            foreach (var transaction in bankAccount.Transactions)
            {
                bankAccount.CurrentBalance -= transaction.Amount;
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
                _db.BankAccounts.Add(bankAccount);
                _db.SaveChanges();
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
            BankAccount bankAccount = _db.BankAccounts.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,AccountType,StartingBalance,Created,CurrentBalance,LowBalanceLevel")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || bankAccount.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
                _db.Entry(bankAccount).State = EntityState.Modified;
                var ret = _db.SaveChanges();
                return RedirectToAction("Dashboard", "Households", new {id = currentUserHouseholdId });
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
            BankAccount bankAccount = _db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || bankAccount.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }
            _db.BankAccounts.Remove(bankAccount);
            _db.SaveChanges();
            return RedirectToAction("Dashboard", "Households", new { id = currentUserHouseholdId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
