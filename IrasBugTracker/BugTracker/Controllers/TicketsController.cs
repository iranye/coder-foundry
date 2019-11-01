using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ProjectHelper _projectHelper = new ProjectHelper();
        private readonly RoleHelper _roleHelper = new RoleHelper();
        private readonly TicketHelper _ticketHelper = new TicketHelper();

        public ActionResult Index()
        {
            var tickets = _db.Tickets; // .Include(t => t.AssignedTo).Include(t => t.Owner).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(_ticketHelper.ListMyTickets());
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
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketPriorityId,TicketTypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                if (!String.IsNullOrWhiteSpace(currentUserId))
                {
                    ticket.OwnerId = currentUserId;
                    ticket.Created = DateTime.Now;

                    // TODO: Add (extension?) method to get default status
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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerId,AssignedToId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(ticket).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.AssignedToId = new SelectList(db.ApplicationUsers, "Id", "FirstName", ticket.AssignedToId);
            //ViewBag.OwnerId = new SelectList(db.ApplicationUsers, "Id", "FirstName", ticket.OwnerId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
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
