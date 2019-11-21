using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Models;

namespace FinancialPortal.Web.Controllers
{
    public class InvitationsController : Controller
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Invitations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(_db.Households, "Id", "Name");

            var myGuid = Guid.NewGuid();
            return View();
        }
    }
}