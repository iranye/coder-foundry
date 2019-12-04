﻿using System;
using System.Collections.Generic;
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
                return RedirectToAction("Dashboard", "Households");
            }

            if (ModelState.IsValid)
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || budget.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }

                var userId = User.Identity.GetUserId();
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return RedirectToAction("Index", "Households");
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
                if (budgetItem.CurrentAmount == default(Decimal))
                {
                    var currentAmount = budgetItemCurrentAmount.TrimStart(new Char[] { '$' });
                    budgetItem.CurrentAmount = Decimal.Parse(currentAmount);
                }
                _db.BudgetItems.Add(budgetItem);
                _db.SaveChanges();
            }
            return RedirectToAction("Details", "Budgets", new { id= budgetId });
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