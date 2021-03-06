﻿using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private RoleHelper _roleHelper = new RoleHelper();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _dbContext.Users.Find(userId);

            // If user is a member of a household, go to Dashboard (Home Dash or Household Dash)
            ViewBag.UserIsHouseholdMember = user.HouseholdId != null;
            return View(_dbContext.Households.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household houseHold = _dbContext.Households.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Households");
            }

            var currentRole = _roleHelper.GetRoleByUserId(userId);
            if (currentRole != "Admin")
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || houseHold.Id != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
            }

            return View(houseHold);
        }

        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Household houseHold = _dbContext.Households.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index");
            }

            var currentRole = _roleHelper.GetRoleByUserId(userId);
            if (currentRole != "Admin")
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || houseHold.Id != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
            }
            
            return View(houseHold);
        }

        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var user = _dbContext.Users.Find(userId);

            // If user is a member of a household, go to Dashboard (Home Dash or Household Dash)
            if (user.HouseholdId != null)
            {
                return RedirectToAction("Dashboard", "Households");
            }

            var household = new Household {Greeting = "Welcome to our Household!"};
            return View(household);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = _dbContext.Users.Find(userId);

                // If user is already a member of a household, go to Dashboard
                if (user.HouseholdId != null)
                {
                    return RedirectToAction("Dashboard", "Households");
                }
                household.Name = household.Name.Trim();
                if (_dbContext.Households.Any(h => h.Name.ToLower() == household.Name.ToLower()))
                {
                    ModelState.AddModelError("Name", "Household with that Name already exists.  Please enter another Name.");
                    return View(household);
                }
                household.Created = DateTime.Now;
                _dbContext.Households.Add(household);
                _dbContext.SaveChanges();

                //Update my User record to include the newly created Household Id
                user.HouseholdId = household.Id;
                _dbContext.SaveChanges();

                //Assign this person the role of HOH
                _roleHelper.AddUserToRole(userId, "HeadOfHousehold");

                //Need to 'Reauthorize' so role will take effect
                await HelperMethods.ReauthorizeAsync();

                return View("Dashboard", household);
            }

            return View(household);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SwitchFromHeadOfHouseholdToMember()
        //{
        //    // Allow HeadOfHousehold to set another Member as HeadOfHousehold then original HeadOfHousehold become Member
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Leave(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            int househouseId = id.GetValueOrDefault();
            var userId = User.Identity.GetUserId();
            var user = _dbContext.Users.Find(userId);
            if (user == null || user.HouseholdId == null)
            {
                return RedirectToAction("Index");
            }

            var myRole = _roleHelper.GetRoleByUserId(userId);
            if (myRole != "Member")
            {
                ModelState.AddModelError("Leave", "Only users with Role=Member can Leave a Household.");
                var household = _dbContext.Households.Find(househouseId);
                return View("Dashboard", household);
            }

            _roleHelper.RemoveUserFromRole(userId, "Member");
            user.HouseholdId = null;

            var ret = _dbContext.SaveChanges();

            //Need to 'Reauthorize' so role will take effect
            await HelperMethods.ReauthorizeAsync();
            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household houseHold = _dbContext.Households.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index");
            }

            var currentRole = _roleHelper.GetRoleByUserId(userId);
            if (currentRole != "Admin")
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || houseHold.Id != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
            }

            return View(houseHold);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household houseHold)
        {
            var userId = User.Identity.GetUserId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index");
            }

            var currentRole = _roleHelper.GetRoleByUserId(userId);
            if (currentRole != "Admin")
            {
                var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
                if (currentUserHouseholdId == null || houseHold.Id != currentUserHouseholdId)
                {
                    return RedirectToAction("Index", "Households");
                }
            }

            if (ModelState.IsValid)
            {
                _dbContext.Entry(houseHold).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(houseHold);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household houseHold = _dbContext.Households.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            _dbContext.Households.Remove(houseHold);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
