using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Create()
        {
            var householdViewModel = new MainDashboardViewModel();
            if (householdViewModel.Budgets.Count == 0)
            {
                return RedirectToAction("Create", "Budgets");
            }
            ViewBag.BudgetId = new SelectList(householdViewModel.Budgets, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BudgetItem budgetItem, int? id, string budgetItemName, 
            string budgetItemDescription, string budgetItemTargetAmount, string budgetItemCurrentAmount)
        {
            var budgetId = id == null ? budgetItem.BudgetId : id;
            var budget = _db.Budgets.Find(budgetId);

            if (budget == null)
            {
                ModelState.AddModelError("Budget", @"Failed to find associated Budget");
                return RedirectToAction("Dashboard", "Home");
            }

            if (ModelState.IsValid)
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Dashboard", "Home");
                }

                var userId = User.Identity.GetUserId();
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return RedirectToAction("Dashboard", "Home");
                }

                budgetItem.BudgetId = budget.Id;
                budgetItem.Created = DateTime.Now;
                if (String.IsNullOrWhiteSpace(budgetItem.Name))
                {
                    budgetItem.Name = budgetItemName;
                }
                if (String.IsNullOrWhiteSpace(budgetItem.Description))
                {
                    budgetItem.Description = budgetItemDescription;
                }
                if (budgetItem.TargetAmount == default(Decimal))
                {
                    var targetAmount = budgetItemTargetAmount.TrimStart(new Char[] { '$' });
                    budgetItem.TargetAmount = Decimal.Parse(targetAmount);
                }

                var childBudgetItemsTargetTotal = 0m;
                foreach (var item in budget.BudgetItems)
                {
                    childBudgetItemsTargetTotal += item.TargetAmount;
                }

                childBudgetItemsTargetTotal += budgetItem.TargetAmount;
                if (childBudgetItemsTargetTotal > budget.TargetAmount)
                {
                    ModelState.AddModelError("Budget", @"Failed to find associated Budget");
                    return RedirectToAction("Details", "Budgets", new { id = budgetId });
                }

                if (budgetItem.CurrentAmount == default(Decimal) && !String.IsNullOrWhiteSpace(budgetItemCurrentAmount))
                {
                    var currentAmount = budgetItemCurrentAmount.TrimStart(new Char[] { '$' });
                    budgetItem.CurrentAmount = Decimal.Parse(currentAmount);
                }
                _db.BudgetItems.Add(budgetItem);
                _db.SaveChanges();
            }
            return RedirectToAction("Details", "Budgets", new { id= budgetId });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = _db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }

            budgetItem.Budget = _db.Budgets.Find(budgetItem.BudgetId);
            if (budgetItem.Budget == null)
            {
                return RedirectToAction("Index", "Households");
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || budgetItem.Budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }
            var householdViewModel = new MainDashboardViewModel();
            if (householdViewModel.Budgets.Count == 0)
            {
                return RedirectToAction("Create", "Budgets");
            }
            ViewBag.BudgetId = new SelectList(householdViewModel.Budgets, "Id", "Name");
            return View(budgetItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BudgetId,Name,Description,TargetAmount,CurrentAmount,Created")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                budgetItem.Budget = _db.Budgets.Find(budgetItem.BudgetId);
                if (budgetItem.Budget == null)
                {
                    return RedirectToAction("Index", "Households");
                }
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || budgetItem.Budget.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
                _db.Entry(budgetItem).State = EntityState.Modified;
                var ret = _db.SaveChanges();
                return RedirectToAction("Dashboard", "Households", new { id = currentUserHouseholdId });
            }
            return View(budgetItem);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var budgetItem = _db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            _db.BudgetItems.Remove(budgetItem);
            _db.SaveChanges();
            return RedirectToAction("Details", "Budgets", new { id = budgetItem.BudgetId });
        }
    }
}