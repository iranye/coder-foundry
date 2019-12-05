using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Models;

namespace FinancialPortal.Web.Controllers
{
    public class ChartsController : Controller
    {
        public JsonResult BarChartData()
        {
            var barChartData = new List<BarChart>
            {
                new BarChart {label = "October", value = 4324},
                new BarChart {label = "November", value = 1234},
                new BarChart {label = "December", value = 5678}
            };
            return Json(barChartData);
        }

        public JsonResult TransactionsBarChartData()
        {
            var transactionsBarChart = new TransactionsBarChart();
            return Json(transactionsBarChart);
        }
    }

    public class TransactionsBarChart
    {
        public TransactionsBarChart()
        {
            
        }
        public string[] labels = new[] { "October", "November", "December" };
        public decimal[] values = new[] { 4324m, 1234m, 5678m };
    }
}