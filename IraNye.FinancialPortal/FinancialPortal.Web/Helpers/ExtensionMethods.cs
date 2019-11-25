using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
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

        public static string GetAvatarPath(this IPrincipal principal)
        {
            var userId = principal.Identity.GetUserId();
            return HelperMethods.GetCurrentUserAvatarPath(userId);
        }

        public static string Massaged(this string str)
        {
            return str.Trim().ToLower();
        }
    }
}
