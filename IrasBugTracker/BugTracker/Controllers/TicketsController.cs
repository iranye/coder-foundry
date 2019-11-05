using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleHelper _roleHelper = new RoleHelper();
        private readonly TicketHelper _ticketHelper = new TicketHelper();
        private readonly TicketHistoryHelper _ticketHistoryHelper = new TicketHistoryHelper();

        public ActionResult Index()
        {
            var tickets = _db.Tickets; // .Include(t => t.AssignedTo).Include(t => t.Owner).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(_ticketHelper.ListMyTickets());
        }

        public ActionResult AssignToUser(int? id)
        {
            // DO NOT USE: ASSIGN USERS IN Ticket Details Page
            var ticket = _db.Tickets.Find(id);
            if (ticket == null)
            {
                return RedirectToAction("Index");
            }

            var users = _roleHelper.UsersInRole("Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FullName", ticket.AssignedToId);
            return RedirectToAction("Details", "Tickets", new { ticket.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket(Ticket model) // TODO: Fix this to take "Selected UserId from DropDown" & TicketID
        {
            var ticket = _db.Tickets.Find(model.Id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ticket.AssignedToId = model.AssignedToId;
            _db.SaveChanges();

            var callbackUrl = Url.Action("Details", "Tickets", new {id = ticket.Id}, protocol: Request.Url.Scheme);

            try
            {
                PersonalEmail svc = new PersonalEmail();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = null;

                msg.Body = "You have been assigned a new Ticket." + Environment.NewLine +
                           "Please click the following link to view the details " +
                           "<a href=\"" + callbackUrl + "\">New Ticket</a>";
                msg.Destination = user.Email;
                msg.Subject = "BugTracker Ticket Notification";
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var users = _roleHelper.UsersInRole("Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "DisplayName", ticket.AssignedToId);

            var userId = User.Identity.GetUserId();
            ViewBag.CanAddContent = _ticketHelper.CanAddContent(userId, ticket);
            return View(ticket);
        }

        [Authorize(Roles = "Submitter,DemoSubmitter")]
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = _db.Users.Find(userId);
            ViewBag.ProjectId = new SelectList(user.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(_db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(_db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter,DemoSubmitter")]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketPriorityId,TicketTypeId")] Ticket ticket, HttpPostedFileBase attachmentFile)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                if (!String.IsNullOrWhiteSpace(currentUserId))
                {
                    ticket.OwnerId = currentUserId;
                    ticket.Created = DateTime.Now;
                    TicketAttachment attachment = null;

                    if (HelperMethods.IsWebFriendlyImage(attachmentFile))
                    {
                        attachment = new TicketAttachment
                        {
                            CreatedById = currentUserId,
                            CreatedDateTime = DateTime.Now,
                        };

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
                        }
                    }

                    // TODO: Add (extension?) method to get default status (or implement Enums)
                    var ticketStatusOpen = _db.TicketStatuses.FirstOrDefault(s => s.Name == "Open");
                    ticket.TicketStatusId = ticketStatusOpen.Id;
                    var ticketPriorityUnknown = _db.TicketPriorities.FirstOrDefault(s => s.Name == "Unknown");
                    ticket.TicketPriorityId = ticketPriorityUnknown.Id;
                    _db.Tickets.Add(ticket);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            string userId = User.Identity.GetUserId();
            ApplicationUser user = _db.Users.Find(userId);
            ViewBag.ProjectId = new SelectList(user.Projects, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(_db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AssignedToId = new SelectList(db.ApplicationUsers, "Id", "FirstName", ticket.AssignedToId);
            //ViewBag.OwnerId = new SelectList(db.ApplicationUsers, "Id", "FirstName", ticket.OwnerId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerId,AssignedToId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = _db.Tickets.Find(ticket.Id);
                ticket.Updated = DateTime.Now;


                _db.Entry(ticket).State = EntityState.Modified;
                _db.SaveChanges();

                _ticketHistoryHelper.RecordHistoricalChanges(oldTicket, ticket);

                var user = _db.Users.Find(User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            //ViewBag.AssignedToId = new SelectList(db.ApplicationUsers, "Id", "FirstName", ticket.AssignedToId);
            //ViewBag.OwnerId = new SelectList(db.ApplicationUsers, "Id", "FirstName", ticket.OwnerId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return RedirectToAction("Details", "Tickets", new {ticket.Id});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
