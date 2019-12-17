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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            transaction.BudgetItem = _db.BudgetItems.Find(transaction.BudgetItem.Id);
            transaction.BudgetItem.Budget = _db.Budgets.Find(transaction.BudgetItem.Budget.Id);
            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || transaction.BudgetItem == null
                                               || transaction.BudgetItem.Budget == null
                                               || transaction.BudgetItem.Budget.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }

            return View(transaction);
        }

        public ActionResult Create()
        {
            var householdViewModel = new MainDashboardViewModel();
            if (householdViewModel.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }
            if (householdViewModel.BankAccounts.Count == 0)
            {
                return RedirectToAction("Create", "BankAccounts");
            }
            if (householdViewModel.Budgets.Count == 0)
            {
                return RedirectToAction("Create", "Budgets");
            }
            if (householdViewModel.BudgetItems.Count == 0)
            {
                return RedirectToAction("Create", "BudgetItems");
            }
            ViewBag.BankAccountId = new SelectList(householdViewModel.BankAccounts, "Id", "Name");
            ViewBag.BudgetItemId = new SelectList(householdViewModel.BudgetItems, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,BudgetItemId,TransactionTypeId,Amount,Memo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.BankAccount = _db.BankAccounts.Find(transaction.BankAccountId);
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null
                    || transaction.BankAccount == null
                    || transaction.BankAccount.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }

                var userId = User.Identity.GetUserId();
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return RedirectToAction("Index", "Households");
                }
                transaction.TransactionDateTime = DateTime.Now;
                transaction.CreatedById = userId;
                _db.Transactions.Add(transaction);
                var ret = _db.SaveChanges();
                if (ret > 0)
                {
                    transaction.TransactionType = _db.TransactionTypes.Find(transaction.TransactionTypeId);

                    if (transaction.BudgetItemId != null)
                    {
                        transaction.BudgetItem = _db.BudgetItems.Find(transaction.BudgetItemId.GetValueOrDefault());
                    }
                    if (transaction.UpdateBalances())
                    {
                        _db.Entry(transaction.BankAccount).State = EntityState.Modified;
                        if (transaction.BudgetItem != null)
                        {
                            _db.Entry(transaction.BudgetItem).State = EntityState.Modified;
                        }
                        ret = _db.SaveChanges();
                    }
                }

                return RedirectToAction("Dashboard", "Households", new { id = currentUserHouseholdId });
            }

            var householdViewModel = new MainDashboardViewModel();
            if (householdViewModel.BankAccounts.Count == 0)
            {
                return RedirectToAction("Create", "BankAccounts");
            }
            if (householdViewModel.Budgets.Count == 0)
            {
                return RedirectToAction("Create", "Budgets");
            }
            if (householdViewModel.BudgetItems.Count == 0)
            {
                return RedirectToAction("Create", "BudgetItems");
            }
            ViewBag.BankAccountId = new SelectList(householdViewModel.BankAccounts, "Id", "Name");
            ViewBag.BudgetItemId = new SelectList(householdViewModel.BudgetItems, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type");
            return View(transaction);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            var householdViewModel = new MainDashboardViewModel();
            ViewBag.BankAccountId = new SelectList(householdViewModel.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetItemId = new SelectList(householdViewModel.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type", transaction.TransactionTypeId);
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BankAccountId,BudgetItemId,TransactionTypeId,Amount,TransactionDateTime,CreatedById,Memo")] Transaction transaction, string createdById, decimal oldAmount)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return RedirectToAction("Index", "Households");
                }

                transaction.BankAccount = _db.BankAccounts.Find(transaction.BankAccountId);
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null 
                    || transaction.BankAccount == null
                    || transaction.BankAccount.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }

                //transaction.CreatedById = createdById;
                _db.Entry(transaction).State = EntityState.Modified;
                var ret = _db.SaveChanges();
                if (ret > 0)
                {
                    transaction.TransactionType = _db.TransactionTypes.Find(transaction.TransactionTypeId);
                    
                    int budgetItemId = transaction.BudgetItemId.GetValueOrDefault();
                    transaction.BudgetItem = _db.BudgetItems.Find(budgetItemId);
                    if (transaction.UpdateBalances(isDeleted: false, oldAmount: oldAmount))
                    {
                        _db.Entry(transaction.BankAccount).State = EntityState.Modified;
                        if (transaction.BudgetItem != null)
                        {
                            _db.Entry(transaction.BudgetItem).State = EntityState.Modified;
                        }
                        ret = _db.SaveChanges();
                    }
                }
                return RedirectToAction("Details", "BankAccounts", new {id= transaction.BankAccount.Id});
            }

            var householdViewModel = new MainDashboardViewModel();
            ViewBag.BankAccountId = new SelectList(householdViewModel.BankAccounts, "Id", "Name");
            ViewBag.BudgetItemId = new SelectList(householdViewModel.BudgetItems, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type", transaction.TransactionTypeId);
            return View(transaction);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Households");
            }

            transaction.BankAccount = _db.BankAccounts.Find(transaction.BankAccountId);
            if (transaction.BankAccount == null)
            {
                return HttpNotFound();
            }
            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null || transaction.BankAccount.HouseholdId != currentUserHouseholdId)
            {
                return RedirectToAction("Index", "Households");
            }

            _db.Transactions.Remove(transaction);
            var ret = _db.SaveChanges();
            if (ret > 0)
            {
                transaction.TransactionType = _db.TransactionTypes.Find(transaction.TransactionTypeId);
                transaction.BankAccount = _db.BankAccounts.Find(transaction.BankAccountId);
                int budgetItemId = transaction.BudgetItemId.GetValueOrDefault();
                transaction.BudgetItem = _db.BudgetItems.Find(budgetItemId);
                if (transaction.UpdateBalances(isDeleted: true))
                {
                    _db.Entry(transaction.BankAccount).State = EntityState.Modified;
                    if (transaction.BudgetItem != null)
                    {
                        _db.Entry(transaction.BudgetItem).State = EntityState.Modified;
                    }
                    ret = _db.SaveChanges();
                }
            }
            return RedirectToAction("Details", "BankAccounts", new {id= transaction.BankAccountId});
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
