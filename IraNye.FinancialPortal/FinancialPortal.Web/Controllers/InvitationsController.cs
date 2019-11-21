using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;

namespace FinancialPortal.Web.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HouseholdId,RecipientEmail")] Invitation invitation, int id)
        {
            Household houseHold = _db.Households.Find(id);

            if (houseHold == null)
            {
                ModelState.AddModelError("Ticket", @"Failed to find associated Ticket");
                return RedirectToAction("Index", "Households");
            }

            if (houseHold.Invitations.Any(i => i.RecipientEmail.Massaged() == invitation.RecipientEmail.Massaged()))
            {
                ViewBag.Message = "The Email entered already exists";
                return RedirectToAction("Dashboard", "Households", new { id });
            }

            invitation.HouseholdId = id;
            invitation.IsValid = true;
            invitation.Code = Guid.NewGuid();
            invitation.TTL = 22;
            invitation.Created = DateTime.Now;

            _db.Invitations.Add(invitation);

            houseHold.Invitations.Add(invitation);
            _db.SaveChanges();

            
            return RedirectToAction("Dashboard", "Households", new { id });
        }
    }
}