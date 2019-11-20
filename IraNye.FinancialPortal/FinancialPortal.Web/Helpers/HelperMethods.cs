using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPortal.Web.Helpers
{
    public static class HelperMethods
    {
        public static ApplicationDbContext _db = new ApplicationDbContext();
        public static ApplicationUser GetApplicationUserByUserId(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            return userManager.FindById(userId);
        }
    }
}
