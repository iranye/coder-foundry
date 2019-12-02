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
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = _dbContext.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
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

            return View(new Budget{HouseholdId = householdId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name,Description,TargetAmount,CurrentAmount")] Budget budget)
        {
            var userId = User.Identity.GetUserId();

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }
            if (ModelState.IsValid)
            {
                budget.OwnerId = userId;
                budget.Created = DateTime.Now;
                _dbContext.Budgets.Add(budget);
                _dbContext.SaveChanges();
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

            Budget budget = _dbContext.Budgets.Find(id);
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
                _dbContext.Entry(budget).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Households");
            }
            return View(budget);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
