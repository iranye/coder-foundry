using System;
using System.Collections.Generic;
using System.Data;
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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = _db.Transactions.Include(t => t.BankAccount).Include(t => t.BudgetItem).Include(t => t.CreatedBy).Include(t => t.TransactionType);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
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
            return View(transaction);
        }

        public ActionResult Create()
        {
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,BudgetItemId,TransactionTypeId,Amount,Memo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.BudgetItem = _db.BudgetItems.Find(transaction.BudgetItemId);
                if (transaction.BudgetItem == null)
                {
                    return RedirectToAction("Index", "Households");
                }
                transaction.BudgetItem.Budget = _db.Budgets.Find(transaction.BudgetItem.BudgetId);

                if (transaction.BudgetItem.Budget == null)
                {
                    return RedirectToAction("Index", "Households");
                }
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || transaction.BudgetItem.Budget.HouseholdId != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }

                var userId = User.Identity.GetUserId();
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return RedirectToAction("Index", "Households");
                }
                transaction.Created = DateTime.Now;
                transaction.CreatedById = userId;
                _db.Transactions.Add(transaction);
                _db.SaveChanges();
                
                return RedirectToAction("Dashboard", "Households", new { id = currentUserHouseholdId });
            }

            ViewBag.BankAccountId = new SelectList(_db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetItemId = new SelectList(_db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type", transaction.TransactionTypeId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
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
            ViewBag.BankAccountId = new SelectList(_db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetItemId = new SelectList(_db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type", transaction.TransactionTypeId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BankAccountId,BudgetItemId,TransactionTypeId,Amount,Memo")] Transaction transaction)
        {
            //if (ModelState.IsValid)
            //{
            //    var userId = User.Identity.GetUserId();
            //    if (String.IsNullOrWhiteSpace(userId))
            //    {
            //        return RedirectToAction("Index", "Households");
            //    }
            //    transaction.Budget = _db budget
            //    var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            //    if (currentUserHouseholdId == null || .HouseholdId != currentUserHouseholdId)
            //    {
            //        return RedirectToAction("Index", "Households");
            //    }

            //    transaction.Created = DateTime.Now;
            //    _db.Entry(transaction).State = EntityState.Modified;
            //    _db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.BankAccountId = new SelectList(_db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            //ViewBag.BudgetItemId = new SelectList(_db.BudgetItems, "Id", "Name", transaction.BudgetItemId);

            //ViewBag.TransactionTypeId = new SelectList(_db.TransactionTypes, "Id", "Type", transaction.TransactionTypeId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
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
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
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
