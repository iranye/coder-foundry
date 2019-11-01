using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,CommentBody")] TicketComment comment, int id)
        {
            var ticket = _db.Tickets.Find(id);

            if (ticket == null)
            {
                ModelState.AddModelError("Ticket", @"Failed to find associated Ticket");
                return RedirectToAction("Index", "Tickets");
            }

            if (ModelState.IsValid)
            {
                comment.TicketId = id;
                comment.AuthorId = User.Identity.GetUserId();
                comment.Created = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                _db.TicketComments.Add(comment);
                var res = _db.SaveChanges();
                if (res == 0)
                {
                    // Save to History
                }
            }

            return RedirectToAction("Details", "Tickets", new { id });
        }
    }
}