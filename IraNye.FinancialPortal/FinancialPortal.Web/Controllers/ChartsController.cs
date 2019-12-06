﻿using System;
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
        private MainDashboardViewModel _mainDashboardViewModel = new MainDashboardViewModel();

        public JsonResult BarChartDataForTransactions()
        {
            var transactionsBarChart = new BarChartTransactions();

            // Get only My Household transactions
            var householdTransactions = _mainDashboardViewModel.Transactions.ToList();
            DateTime monthToReport = DateTime.Now.AddMonths(-12);
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime dateTimeReportCutoff = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            var oldestTransaction = householdTransactions.OrderBy(t => t.Created).FirstOrDefault();
            if (oldestTransaction != null)
            {
                if (monthToReport < oldestTransaction.Created)
                {
                    monthToReport = oldestTransaction.Created;
                }
                // Collect list of Months to report based on earliestDateToReport into labels
                var months = new List<string>();
                var totals = new List<decimal>();
                do
                {
                    months.Add(monthToReport.ToString("MMMM"));

                    // For each month, sum the total transactions amount and collect into values
                    decimal sum = 0m;
                    foreach (var transaction in householdTransactions.Where(t => t.Created.Month == monthToReport.Month))
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
            var householdTransactions = _mainDashboardViewModel.Transactions.ToList();
            DateTime monthToReport = DateTime.Now.AddMonths(-12);
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime dateTimeReportCutoff = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            var oldestTransaction = householdTransactions.OrderBy(t => t.Created).FirstOrDefault();
            if (oldestTransaction != null)
            {
                if (monthToReport < oldestTransaction.Created)
                {
                    monthToReport = oldestTransaction.Created;
                }
                // Collect list of Months to report based on earliestDateToReport into labels
                var months = new List<string>();
                var transactionTotals = new List<decimal>();
                var budgetTotals = new List<decimal>();
                do
                {
                    months.Add(monthToReport.ToString("MMMM"));
                    decimal sum = 0m;
                    foreach (var transaction in householdTransactions.Where(t => t.Created.Month == monthToReport.Month)
                    )
                    {
                        sum += transaction.Amount;
                    }
                    transactionTotals.Add(sum);

                    monthToReport = monthToReport.AddMonths(1);
                } while (monthToReport < dateTimeReportCutoff);

                householdBarChart.labels = months.ToArray();
                householdBarChart.transactionvalues = transactionTotals.ToArray();
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
        public string[] labels = new[] { "October", "November", "December" };
        public decimal[] values = new[] { 4324m, 1234m, 5678m };
    }

    public class BarChartTransactionsAndBudgets
    {
        public string[] labels = new[] { "October", "November", "December" };
        public decimal[] transactionvalues = new[] { 4324m, 1234m, 5678m };
        public decimal[] budgetvalues = new[] { 4324m, 1234m, 5678m };
    }
}