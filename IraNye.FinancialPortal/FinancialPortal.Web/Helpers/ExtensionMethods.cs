using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this IPrincipal principal)
        {
            var userId = principal.Identity.GetUserId();
            var currentUser = HelperMethods.GetApplicationUserByUserId(userId);
            var displayName = currentUser == null ? "UNKNOWN" : currentUser.DisplayName;

            return displayName;
        }
    }
}
