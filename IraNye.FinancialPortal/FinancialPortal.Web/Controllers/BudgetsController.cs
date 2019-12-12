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
    public class BudgetsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = _db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Households");
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }

            return View(budget);
        }

        public ActionResult Create()
        {
            int householdId = HelperMethods.GetCurrentUserHouseholdId().GetValueOrDefault();
            if (householdId == 0)
            {
                return RedirectToAction("Index", "Households");
            }

            return View(new Budget{HouseholdId = householdId, CurrentAmount = 0});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name,Description,TargetAmount,CurrentAmount")] Budget budget)
        {
            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                budget.OwnerId = userId;
                budget.Created = DateTime.Now;
                _db.Budgets.Add(budget);
                _db.SaveChanges();
                return RedirectToAction("Dashboard", "Households", new{id=budget.HouseholdId});
            }

            return View(budget);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();

            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Households");
            }

            Budget budget = _db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }
            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,Description,Created,OwnerId,TargetAmount,CurrentAmount")] Budget budget)
        {
            var userId = User.Identity.GetUserId();

            if (String.IsNullOrWhiteSpace(userId) || Helpers.HelperMethods.GetCurrentUserHouseholdId() == null)
            {
                return RedirectToAction("Index", "Households");
            }
            if (ModelState.IsValid)
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
                var ret = _db.Entry(budget).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Dashboard", "Households", new { id = budget.HouseholdId });
            }
            return View(budget);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();

            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Households");
            }

            Budget budget = _db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }

            var budgetItemsToDelete = new List<BudgetItem>();
            foreach (var budgetItem in budget.BudgetItems)
            {
                budgetItemsToDelete.Add(budgetItem);
            }
            budgetItemsToDelete.ForEach(b => _db.BudgetItems.Remove(b));
            _db.SaveChanges();

            _db.Budgets.Remove(budget);
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
