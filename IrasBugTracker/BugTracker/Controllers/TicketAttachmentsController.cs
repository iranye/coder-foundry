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
    [Authorize]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private readonly TicketHelper _ticketHelper = new TicketHelper();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketAttachment attachment, int id, HttpPostedFileBase attachmentFile, string attachmentDescription)
        {
            if (attachmentFile == null)
            {
                // TODO: Need a message in the View telling user to pick a file first...
                ModelState.AddModelError("validation-summary-errors", "Please select a File First");
                TempData["CustomError"] = "The item is removed from your cart";
                return RedirectToAction("Dashboard", "Tickets", new { id });
            }
            var ticket = _db.Tickets.Find(id);
            var userId = User.Identity.GetUserId();

            if (ticket == null)
            {
                return RedirectToAction("Index", "Tickets");
            }
            if (userId == null)
            {
                return RedirectToAction("Index", "Tickets");
            }

            if (ModelState.IsValid)
            {
                var canAddContent = _ticketHelper.CanAddContent(userId, ticket);

                if (canAddContent)
                {
                    if (HelperMethods.IsWebFriendlyImage(attachmentFile) || HelperMethods.IsWebFriendlyFile(attachmentFile))
                    {
                        attachment.TicketId = id;
                        attachment.CreatedById = userId;
                        attachment.CreatedDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        if (!String.IsNullOrWhiteSpace(attachmentDescription))
                        {
                            int maxDescLen = 250;
                            if (attachmentDescription.Length > maxDescLen)
                            {
                                attachment.Description = attachmentDescription.Substring(0, maxDescLen);
                            }
                            else
                            {
                                attachment.Description = attachmentDescription;
                            }

                        }
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

                        if (ticket.AssignedToId != null)
                        {
                            attachment.CreatedBy = _db.Users.Find(attachment.CreatedById);
                            var attachmentCreatedByName = attachment.CreatedBy.DisplayName == null ? "UNKNOWN" : attachment.CreatedBy.DisplayName;
                            var notificationCreationDateTime = DateTime.Now;
                            var notification = new TicketNotification
                            {
                                TicketId = ticket.Id,
                                Created = notificationCreationDateTime,
                                Subject = $"A ticket you're Assigned to '{ticket.Title}' has a new Attachment!",
                                IsRead = false,
                                RecipientId = ticket.AssignedToId,
                                NotificationBody =
                                    $"Ticket '{ticket.Title}' ({ticket.DisplayableId}) has a new Attachment from {attachmentCreatedByName}"
                            };
                            _db.TicketNotifications.Add(notification);
                            _db.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Dashboard", "Tickets", new { id });
        }
    }
}