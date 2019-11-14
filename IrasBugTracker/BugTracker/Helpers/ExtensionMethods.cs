using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;

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

        public static string GenerateSlug(this string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^.a-z0-9\s-]", "");

            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // cut and trim 
            var extension = Path.GetExtension(str);
            var baseName = str;
            if (!String.IsNullOrWhiteSpace(extension))
            {
                baseName = str.Substring(0, str.IndexOf(extension));
            }
            
            baseName = baseName.Substring(0, baseName.Length <= 45 ? baseName.Length : 45).Trim();
            str = Regex.Replace(baseName, @"\s", "-"); // hyphens   
            return str + extension;
        }

        public static string ApplyDateTimeStamp(this string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
        }

        public static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
