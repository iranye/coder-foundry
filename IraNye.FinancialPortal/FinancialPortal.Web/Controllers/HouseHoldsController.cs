using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
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
            return View(houseHold);
        }

        public ActionResult Create()
        {
            var household = new Household {Greeting = "Welcome to our Household!"};
            return View(household);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Name = household.Name.Trim();
                if (_dbContext.Households.Any(h => h.Name.ToLower() == household.Name.ToLower()))
                {
                    ModelState.AddModelError("", "Household with that Name already exists.  Please enter another Name.");
                    return View(household);
                }
                household.Created = DateTime.Now;
                _dbContext.Households.Add(household);
                _dbContext.SaveChanges();

                //Update my User record to include the newly created Household Id
                var userId = User.Identity.GetUserId();
                var user = _dbContext.Users.Find(userId);
                user.HouseholdId = household.Id;
                _dbContext.SaveChanges();

                //Assign this person the role of HOH
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
                userManager.RemoveFromRole(userId, "Lobbyist");
                userManager.AddToRole(userId, "HeadOfHousehold");

                //Need to 'Reauthorize' so role will take effect
                await HelperMethods.ReauthorizeAsync();

                return RedirectToAction("Index");
            }

            return View(household);
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
            return View(houseHold);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household houseHold)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(houseHold).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(houseHold);
        }

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
            return View(houseHold);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household houseHold = _dbContext.Households.Find(id);
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
