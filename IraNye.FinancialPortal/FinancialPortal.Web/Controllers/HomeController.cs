using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            MainDashboardViewModel viewModel = new MainDashboardViewModel();
            return View(viewModel);
        }
        
        // Scotty Code
        public ActionResult Dashboard()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId == null)
            {
                return RedirectToAction("Index", "Households");
            }
            MainDashboardViewModel viewModel = new MainDashboardViewModel();
            viewModel.Household = _db.Households.AsNoTracking().FirstOrDefault(h => h.Id == currentUserHouseholdId);

            viewModel.BankAccounts  = _db.BankAccounts.AsNoTracking().Where(b => b.HouseholdId == currentUserHouseholdId).ToList();
            viewModel.Budgets = _db.Budgets.AsNoTracking().Where(b => b.HouseholdId == currentUserHouseholdId).ToList();

            viewModel.BudgetItems.AddRange(viewModel.Budgets.SelectMany(b => b.BudgetItems));

            var transactions = _db.Transactions.AsNoTracking();
            foreach (var transaction in transactions)
            {
                if (viewModel.BankAccounts.Any(ba => ba.Id == transaction.BankAccountId))
                {
                    viewModel.Transactions.Add(transaction);
                }
            }

            return View(viewModel);
        }


    }
}