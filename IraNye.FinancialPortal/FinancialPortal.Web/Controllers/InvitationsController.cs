using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

            if (String.IsNullOrWhiteSpace(invitation.RecipientEmail))
            {
                return RedirectToAction("Dashboard", "Households", new { id });
            }

            string recipientEmail = "";
            string invitationCode = "";
            string callbackUrl = "";

            int ttlDays = 21;
            var invitationTtl = ConfigurationManager.AppSettings["InvitationTtlDays"];
            if (Int32.TryParse(invitationTtl, out var ttl))
            {
                ttlDays = ttl;
            }

            invitation.RecipientEmail = invitation.RecipientEmail.Trim();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            ApplicationUser existingUser = userManager.FindByEmail(invitation.RecipientEmail);
            if (existingUser == null)
            {
                Invitation existingInvitation = _db.Invitations.FirstOrDefault(i => i.RecipientEmail.ToLower() == invitation.RecipientEmail.ToLower());

                if (existingInvitation != null)
                {
                    existingInvitation.TTL = ttlDays;
                    var ret = _db.SaveChanges();
                    recipientEmail = existingInvitation.RecipientEmail;
                    invitationCode = $"{existingInvitation.RecipientEmail}≡{existingInvitation.Code}";
                }
                else
                {
                    invitation.HouseholdId = id;
                    invitation.IsValid = true;
                    invitation.Code = Guid.NewGuid();
                    invitation.TTL = ttlDays;
                    invitation.Created = DateTime.Now;

                    _db.Invitations.Add(invitation);

                    houseHold.Invitations.Add(invitation);
                    var ret = _db.SaveChanges();
                    recipientEmail = invitation.RecipientEmail;
                    invitationCode = $"{invitation.RecipientEmail}≡{invitation.Code}";
                }
                callbackUrl = Url.Action("RegisterInvitee", "Account", new { code = invitationCode }, protocol: Request.Url.Scheme);
            }
            else
            {
                if (existingUser.HouseholdId == id)
                {
                    return RedirectToAction("Dashboard", "Households", new { id });
                }
                invitation.HouseholdId = id;
                invitation.IsValid = true;
                invitation.Code = Guid.NewGuid();
                invitation.TTL = ttlDays;
                invitation.Created = DateTime.Now;

                _db.Invitations.Add(invitation);

                houseHold.Invitations.Add(invitation);
                var ret = _db.SaveChanges();
                recipientEmail = invitation.RecipientEmail;
                invitationCode = $"{invitation.RecipientEmail}≡{invitation.Code}";
                callbackUrl = Url.Action("LoginInvitee", "Account", new { code = invitationCode }, protocol: Request.Url.Scheme);
            }
            
            var userId = User.Identity.GetUserId();
            var senderName = "UNKNOWN";
            if (!String.IsNullOrWhiteSpace(userId))
            {
                var user = _db.Users.Find(userId);
                senderName = user.DisplayName;
            }

            await SendInvitationEmail(recipientEmail, senderName, houseHold.Name, callbackUrl);

            return RedirectToAction("Dashboard", "Households", new { id });
        }

        private async Task<bool> SendInvitationEmail(string emailTo, string senderName, string householdName, string callbackUrl)
        {
            var appName = "IraNye Financial Portal";
            var emailFrom = ConfigurationManager.AppSettings["emailFrom"];

            MailMessage mailMessage = new MailMessage(emailFrom, emailTo);
            mailMessage.Subject = $"{senderName} has sent you an invitation to join a great Financial Portal Site!";
            mailMessage.Body = $"Please register in {appName} and join the {householdName} Household by clicking <a href=\"" + callbackUrl + "\">here</a>";
            mailMessage.IsBodyHtml = true;

            try
            {
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
                return false;
            }
            return true;
        }
    }
}