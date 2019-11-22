using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace FinancialPortal.Web.Helpers
{
    public static class HelperMethods
    {
        private static ApplicationDbContext _dbContext = null;

        internal static ApplicationDbContext DbContext => _dbContext ?? (_dbContext = new ApplicationDbContext());

        public static ApplicationUser GetApplicationUserByUserId(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
            return userManager.FindById(userId);
        }
        
        public static async Task ReauthorizeAsync()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = _dbContext.Users.Find(userId);
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }

        public static int? GetCurrentUserHouseholdId()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();

            int? ret = null;
            if (!String.IsNullOrWhiteSpace(currentUserId))
            {
                SqlParameter param1 = new SqlParameter("@UserId", currentUserId);

                ret = DbContext.Database.SqlQuery<int?>("GetCurrentUserHouseholdId @UserId", param1).FirstOrDefault();
            }

            return ret;
        }

        public static bool InviteeAlreadyRegistered(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
            var user = userManager.FindByEmail(email);
            return user != null;
        }

        public static bool InvitationRegistrationIsValid(string invitationCode, out string email, out int? householdId)
        {
            bool ret = false;
            email = "";
            householdId = null;

            var separator = '≡';
            if (String.IsNullOrWhiteSpace(invitationCode) || invitationCode.IndexOf(separator) < 0)
            {
                return ret;
            }

            var codeSplit = invitationCode.Split(separator);
            var emailFromCode = codeSplit[0];
            var guid = codeSplit[1];
            if (String.IsNullOrWhiteSpace(emailFromCode))
            {
                return ret;
            }
            if (String.IsNullOrWhiteSpace(guid))
            {
                return ret;
            }

            var invitation = DbContext.Invitations.FirstOrDefault(i => i.RecipientEmail == emailFromCode);

            if (invitation == null || !invitation.IsValid)
            {
                return ret;
            }

            if (invitation.Code != new Guid(guid))
            {
                return ret;
            }

            var expirationDateTime = invitation.Created.AddDays(0 - invitation.TTL);
            if (expirationDateTime > DateTime.Now)
            {
                invitation.IsValid = false;
                DbContext.SaveChangesAsync();
                return ret;
            }

            email = emailFromCode;
            householdId = invitation.HouseholdId;
            ret = true;
            return ret;
        }
    }
}
