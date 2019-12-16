using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        private MainDashboardViewModel _mainDashboardViewModel = null;

        public MainDashboardViewModel MainDashboardViewModel
        {
            get
            {
                if (_mainDashboardViewModel == null)
                {
                    _mainDashboardViewModel = new MainDashboardViewModel();
                }

                return _mainDashboardViewModel;
            }
        }

        public JsonResult BarChartDataForTransactions()
        {
            var transactionsBarChart = new BarChartTransactions();

            // Get only My Household transactions
            var householdTransactions = MainDashboardViewModel.Transactions.ToList();
            DateTime monthToReport = DateTime.Now.AddMonths(-12);
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime dateTimeReportCutoff = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            var oldestTransaction = householdTransactions.OrderBy(t => t.TransactionDateTime).FirstOrDefault();
            if (oldestTransaction != null)
            {
                if (monthToReport < oldestTransaction.TransactionDateTime)
                {
                    monthToReport = oldestTransaction.TransactionDateTime;
                }
                // Collect list of Months to report based on earliestDateToReport into labels
                var months = new List<string>();
                var totals = new List<decimal>();
                do
                {
                    months.Add(monthToReport.ToString("MMMM"));

                    // For each month, sum the total transactions amount and collect into values
                    decimal sum = 0m;
                    foreach (var transaction in householdTransactions.Where(t => t.TransactionDateTime.Month == monthToReport.Month))
                    {
                        sum += transaction.Amount;
                    }
                    totals.Add(sum);

                    monthToReport = monthToReport.AddMonths(1);
                } while (monthToReport < dateTimeReportCutoff);

                transactionsBarChart.labels = months.ToArray();
                transactionsBarChart.values = totals.ToArray();
            }

            return Json(transactionsBarChart);
        }

        public JsonResult BarChartDataForHousehold()
        {
            var householdBarChart = new BarChartTransactionsAndBudgets();

            // Get only MY transactions
            var householdTransactions = MainDashboardViewModel.Transactions.ToList();
            decimal totalMonthlyBudget = MainDashboardViewModel.TotalMonthlyBudgetDecimal;
            DateTime monthToReport = DateTime.Now.AddMonths(-12);
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime dateTimeReportCutoff = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            var oldestTransaction = householdTransactions.OrderBy(t => t.TransactionDateTime).FirstOrDefault();
            if (oldestTransaction != null)
            {
                if (monthToReport < oldestTransaction.TransactionDateTime)
                {
                    monthToReport = oldestTransaction.TransactionDateTime;
                }
                // Collect list of Months to report based on earliestDateToReport into labels
                var months = new List<string>();
                var transactionTotals = new List<decimal>();
                var budgetTotals = new List<decimal>();
                do
                {
                    months.Add(monthToReport.ToString("MMMM"));
                    decimal sum = 0m;
                    foreach (var transaction in householdTransactions.Where(t => t.TransactionDateTime.Month == monthToReport.Month)
                    )
                    {
                        sum += transaction.Amount;
                    }
                    transactionTotals.Add(sum);
                    budgetTotals.Add(totalMonthlyBudget);

                    monthToReport = monthToReport.AddMonths(1);
                } while (monthToReport < dateTimeReportCutoff);

                householdBarChart.labels = months.ToArray();
                householdBarChart.transactionvalues = transactionTotals.ToArray();
                householdBarChart.budgetvalues = budgetTotals.ToArray();
                // For each month, sum the total transactions amount and collect into values
            }

            return Json(householdBarChart);
        }

        public JsonResult BarChartDataTransactions()
        {
            var transactionsBarChart = new BarChartTransactions();
            return Json(transactionsBarChart);
        }

    }

    public class BarChartTransactions
    {
        public string[] labels = new[] { "August (Demo)", "September (Demo)", "October (Demo)", "November (Demo)", "December (Demo)" };
        public decimal[] values = new[] { 1024m, 1189m, 2324m, 1234m, 2678m };
    }

    public class BarChartTransactionsAndBudgets
    {
        public string[] labels = new[] { "August (Demo)", "September (Demo)", "October (Demo)", "November (Demo)", "December (Demo)" };
        public decimal[] transactionvalues = new[] { 1024m, 1189m, 2324m, 1234m, 2678m };
        public decimal[] budgetvalues = new[] { 2000m, 2000m, 2000m, 2000m, 2000m };
    }
}