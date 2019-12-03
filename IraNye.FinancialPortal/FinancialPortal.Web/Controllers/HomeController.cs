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

            MainDashboardViewModel viewModel = new MainDashboardViewModel();

            if (viewModel.Household == null)
            {
                return RedirectToAction("Index", "Households");
            }

            return View(viewModel);
        }


    }
}