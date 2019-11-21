using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MainDashboardViewModel viewModel = new MainDashboardViewModel();
            return View(viewModel);
        }
    }
}