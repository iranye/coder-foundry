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
    }
}
