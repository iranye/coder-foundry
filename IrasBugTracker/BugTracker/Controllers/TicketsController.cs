using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
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
        private readonly NotificationHelper _notificationHelper = new NotificationHelper();
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
            ViewBag.CanEdit = _ticketHelper.CanEdit(userId, ticket);

            var allowableFileExtensions = WebConfigurationManager.AppSettings["AllowableFileExtensions"];
            string[] allowableFileExtensionsArr = allowableFileExtensions.Split(',');

            foreach (var ticketAttachment in ticket.Attachments)
            {
                var fileExt = Path.GetExtension(ticketAttachment.MediaPath);
                if (allowableFileExtensionsArr.Contains(fileExt))
                {
                    ticketAttachment.MediaPath = $"/Uploads/file.png";
                }
            }

            return View(ticket);
        }

        public ActionResult Dashboard(int? id)
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
            ViewBag.CanEdit = _ticketHelper.CanEdit(userId, ticket);

            var allowableFileExtensions = WebConfigurationManager.AppSettings["AllowableFileExtensions"];
            string[] allowableFileExtensionsArr = allowableFileExtensions.Split(',');

            foreach (var ticketAttachment in ticket.Attachments)
            {
                var fileExt = Path.GetExtension(ticketAttachment.MediaPath);
                if (allowableFileExtensionsArr.Contains(fileExt))
                {
                    ticketAttachment.MediaPath = $"/Uploads/file.png";
                }
            }

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

            var userId = User.Identity.GetUserId();
            var userCanEditTicket = _ticketHelper.CanEdit(userId, ticket);

            if (!userCanEditTicket)
            {
                return RedirectToAction("Details", "Tickets", new { ticket.Id });
            }

            ViewBag.CanEdit = userCanEditTicket;
            ViewBag.CanChangeAssignment = _ticketHelper.CanChangeAssignment(userId, ticket);
            ViewBag.CanChangeStatus = _ticketHelper.CanChangeStatus(userId, ticket);
            ViewBag.CanChangeOwner = false;

            List<ApplicationUser> developers = _roleHelper.UsersInRole("Developer").ToList();
            developers.AddRange(_roleHelper.UsersInRole("DemoDeveloper"));
            List<ApplicationUser> developersOnProject = new List<ApplicationUser>();

            List<ApplicationUser> submitters = _roleHelper.UsersInRole("Submitter").ToList();
            submitters.AddRange(_roleHelper.UsersInRole("DemoSubmitter"));
            List<ApplicationUser> submittersOnProject = new List<ApplicationUser>();

            var project = _db.Projects.Find(ticket.ProjectId);
            if (project != null)
            {
                foreach (var dev in developers)
                {
                    if (project.Members.Select(m => m.Id).Contains(dev.Id))
                    {
                        developersOnProject.Add(dev);
                    }
                }

                foreach (var submitter in submitters)
                {
                    if (project.Members.Select(m => m.Id).Contains(submitter.Id))
                    {
                        submittersOnProject.Add(submitter);
                    }
                }
            }

            ViewBag.AssignedToId = new SelectList(developersOnProject, "Id", "DisplayName", ticket.AssignedToId);
            ViewBag.ProjectId = new SelectList(_db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(_db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(_db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(_db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.OwnerId = new SelectList(submittersOnProject, "Id", "DisplayName", ticket.OwnerId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Title,Description,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerId,AssignedToId")] Ticket ticket, string assigneeId, int statusId)
        {
            var userId = User.Identity.GetUserId();
            var oldTicket = _db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

            // When Developer makes an Edit, ticket.AssignedToId doesn't come to the Post method
            var userCanChangeAssignment = _ticketHelper.CanChangeAssignment(userId, oldTicket); // this is false for Role=Developers or lower Access Level
            if (!userCanChangeAssignment && ticket.AssignedToId == null && !String.IsNullOrWhiteSpace(assigneeId))
            {
                ticket.AssignedToId = assigneeId;
            }
            // When Developer makes an Edit, ticket.TicketStatusId doesn't come to the Post method
            if (ticket.TicketStatusId == 0 && ticket.TicketStatusId != statusId)
            {
                ticket.TicketStatusId = statusId;
            }
            
            var userCanEditTicket = _ticketHelper.CanEdit(userId, oldTicket);
            var userCanChangeStatus = _ticketHelper.CanChangeStatus(userId, ticket);

            if (!userCanEditTicket)
            {
                return RedirectToAction("Details", "Tickets", new { ticket.Id });
            }

            if (ModelState.IsValid)
            {
                if (ticket.AssignedToId != oldTicket.AssignedToId && !userCanChangeAssignment)
                {
                    // Should never reach this, but just in case...
                    ModelState.AddModelError("validation-summary-errors", "Insufficient Access to Change Ticket Assignment");
                    ticket.AssignedToId = oldTicket.AssignedToId;
                }

                ticket.Updated = DateTime.Now;
                ticket.Project = _db.Projects.Find(ticket.ProjectId);
                ticket.TicketPriority = _db.TicketPriorities.Find(ticket.TicketPriorityId);
                ticket.TicketStatus = _db.TicketStatuses.Find(ticket.TicketStatusId);
                ticket.TicketType = _db.TicketTypes.Find(ticket.TicketTypeId);
                ticket.AssignedTo = _db.Users.Find(ticket.AssignedToId);
                var ticketChanges = _ticketHistoryHelper.GetChanges(userId, oldTicket, ticket);
                
                if (ticketChanges.Count > 0)
                {
                    _db.Entry(ticket).State = EntityState.Modified;
                    _db.TicketHistorys.AddRange(ticketChanges);
                    _db.SaveChanges();

                    var ticketAssignmentNotifications = _notificationHelper.GetAssignmentNotifications(oldTicket, ticket);
                    if (ticketAssignmentNotifications.Count > 0)
                    {
                        // If Unassigned=>Assigned, change Status to Assigned
                        if (ticket.AssignedToId != null && ticket.TicketStatus.Name == "Open")
                        {
                            ticket.TicketStatusId = _db.TicketStatuses.FirstOrDefault(s => s.Name == "Assigned").Id;
                        }
                        // If Assigned=>UnAssigned, change Status to Open
                        if (ticket.AssignedToId == null && ticket.TicketStatus.Name == "Assigned")
                        {
                            ticket.TicketStatusId = _db.TicketStatuses.FirstOrDefault(s => s.Name == "Open").Id;
                        }
                        ticket.TicketStatus = _db.TicketStatuses.Find(ticket.TicketStatusId);
                        _db.TicketNotifications.AddRange(ticketAssignmentNotifications);
                        _db.SaveChanges();
                    }
                }
            }

            // Re-send Get-Edit-View
            ViewBag.CanEdit = userCanEditTicket;
            ViewBag.CanChangeAssignment = userCanChangeAssignment;
            ViewBag.CanChangeStatus = userCanChangeStatus;
            ViewBag.CanChangeOwner = false;

            List<ApplicationUser> developers = _roleHelper.UsersInRole("Developer").ToList();
            developers.AddRange(_roleHelper.UsersInRole("DemoDeveloper"));
            List<ApplicationUser> developersOnProject = new List<ApplicationUser>();

            List<ApplicationUser> submitters = _roleHelper.UsersInRole("Submitter").ToList();
            submitters.AddRange(_roleHelper.UsersInRole("DemoSubmitter"));
            List<ApplicationUser> submittersOnProject = new List<ApplicationUser>();

            var project = _db.Projects.Find(ticket.ProjectId);
            if (project != null)
            {
                foreach (var dev in developers)
                {
                    if (project.Members.Select(m => m.Id).Contains(dev.Id))
                    {
                        developersOnProject.Add(dev);
                    }
                }

                foreach (var submitter in submitters)
                {
                    if (project.Members.Select(m => m.Id).Contains(submitter.Id))
                    {
                        submittersOnProject.Add(submitter);
                    }
                }
            }

            ViewBag.AssignedToId = new SelectList(developersOnProject, "Id", "DisplayName", ticket.AssignedToId);
            ViewBag.ProjectId = new SelectList(_db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(_db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(_db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(_db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.OwnerId = new SelectList(submittersOnProject, "Id", "DisplayName", ticket.OwnerId);

            return View(ticket);
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
