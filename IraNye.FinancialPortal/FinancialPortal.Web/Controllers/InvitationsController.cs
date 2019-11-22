using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HouseholdId,RecipientEmail")] Invitation invitation, int id)
        {
            Household houseHold = _db.Households.Find(id);

            if (houseHold == null)
            {
                ModelState.AddModelError("Ticket", @"Failed to find associated Ticket");
                return RedirectToAction("Index", "Households");
            }

            invitation.RecipientEmail = invitation.RecipientEmail.Trim();

            // TODO: Check this in the client
            if (houseHold.Invitations.Any(i => i.RecipientEmail.ToLower() == invitation.RecipientEmail.ToLower()))
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

            var appName = "IraNye Financial Portal";
            string invitationCode = $"{invitation.RecipientEmail}≡{invitation.Code}";
            var callbackUrl = Url.Action("RegisterInvitee", "Account", new { code = invitationCode }, protocol: Request.Url.Scheme);

            var emailFrom = ConfigurationManager.AppSettings["emailFrom"];
            var emailTo = invitation.RecipientEmail;

            MailMessage mailMessage = new MailMessage(emailFrom, emailTo);
            mailMessage.Subject = "Ira Nye Financial Portal Site Invitation";
            mailMessage.Body = $"Please register in {appName} and join the {houseHold.Name} Household by clicking <a href=\"" + callbackUrl + "\">here</a>";
            mailMessage.IsBodyHtml = true;

            try
            {
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMessage);
                // UserManager.SendEmail(invitation.RecipientEmail, $"Invitation to {appName}", $"Please register in {appName} and join the by clicking <a href=\"" + callbackUrl + "\">here</a>");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

            return RedirectToAction("Dashboard", "Households", new { id });
        }
    }
}