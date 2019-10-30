using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;
using System.Security.Principal;

namespace BugTracker.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this IPrincipal principal)
        {
            // var displayName = principal.Identity.FirstOrDefault(c => c.Type == "DisplayName");
            var userId = principal.Identity.GetUserId();
            return new RoleHelper().GetDisplayName(userId);
        }
    }
}
