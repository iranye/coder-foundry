using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BudgetItem budgetItem, int id, string budgetItemName, 
            string budgetItemDescription, string budgetItemTargetAmount, string budgetItemCurrentAmount)
        {
            var budget = _db.Budgets.Find(id);

            if (budget == null)
            {
                ModelState.AddModelError("Budget", @"Failed to find associated Budget");
                return RedirectToAction("Dashboard", "Households");
            }

            if (ModelState.IsValid)
            {
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

                budgetItem.BudgetId = id;
                budgetItem.Created = DateTime.Now;
                budgetItem.Name = budgetItemName;
                budgetItem.Description = budgetItemDescription;
                var targetAmount = budgetItemTargetAmount.TrimStart(new Char[] {'$'});
                budgetItem.TargetAmount = Decimal.Parse(targetAmount);
                var currentAmount = budgetItemTargetAmount.TrimStart(new Char[] { '$' });
                budgetItem.CurrentAmount = Decimal.Parse(currentAmount);
                _db.BudgetItems.Add(budgetItem);
                _db.SaveChanges();
            }
            return RedirectToAction("Details", "Budgets", new { id });
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