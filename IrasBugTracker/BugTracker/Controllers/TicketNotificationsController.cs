using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketNotificationsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            List<TicketNotification> ticketNotifications = new List<TicketNotification>();
            if (!String.IsNullOrWhiteSpace(userId))
            {
                ticketNotifications = _db.TicketNotifications.Where(tn => tn.RecipientId == userId).OrderByDescending(tn => tn.Created).ToList();
            }
            return View(ticketNotifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dismiss(int unreadNotificationId)
        {
            var notification = _db.TicketNotifications.Find(unreadNotificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                var ret = _db.SaveChanges();
                return RedirectToAction("Dashboard", "Tickets", new { id = notification.TicketId });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}