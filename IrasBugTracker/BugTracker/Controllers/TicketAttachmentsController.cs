using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private readonly TicketHelper _ticketHelper = new TicketHelper();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketAttachment attachment, int id, HttpPostedFileBase attachmentFile)
        {
            var ticket = _db.Tickets.Find(id);

            if (ticket == null)
            {
                ModelState.AddModelError("Ticket", @"Failed to find associated Ticket");
                return RedirectToAction("Index", "Tickets");
            }

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var canAddContent = _ticketHelper.CanAddContent(userId, ticket);

                if (canAddContent)
                {
                    attachment.TicketId = id;
                    attachment.CreatedById = userId;
                    attachment.CreatedDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                    if (HelperMethods.IsWebFriendlyImage(attachmentFile))
                    {
                        // Run filename through URL Friendly method then Apply a timestamp to filename to avoid naming collisions
                        var fileName = attachmentFile.FileName.GenerateSlug();
                        var massagedFileName = fileName.ApplyDateTimeStamp();
                        var dirPath = Server.MapPath("~/Uploads/");

                        if (HelperMethods.EnsureDirectoryExists(dirPath))
                        {
                            var filePath = Path.Combine(dirPath, massagedFileName);
                            attachmentFile.SaveAs(filePath);
                            attachment.MediaPath = $"/Uploads/{massagedFileName}";
                            ticket.Attachments.Add(attachment);
                            var res = _db.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Details", "Tickets", new { id });
        }
    }
}