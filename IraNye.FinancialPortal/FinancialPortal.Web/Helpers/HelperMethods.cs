using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

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

        //public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        //{
        //    context.GetOwinContext().Authentication.SignOut();
        //    await context.GetOwinContext().Get<ApplicationSignInManager>()
        //        .SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //}

        public static async Task ReauthorizeAsync()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = DbContext.Users.Find(userId);
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
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
                DbContext.Database.Connection.Close();
            }

            return ret;
        }

        public static string GetCurrentUserAvatarPath(string userId)
        {
            var ret = "https://source.unsplash.com/QAB-WJcbgJk/60x60";

            if (!String.IsNullOrWhiteSpace(userId))
            {
                SqlParameter param1 = new SqlParameter("@UserId", userId);

                string dbEntry = DbContext.Database.SqlQuery<String>("GetAvatarPathByUserID @UserId", param1)
                    .FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(dbEntry))
                {
                    ret = dbEntry;
                }
            }

            return ret;
        }

        public static bool InviteeAlreadyRegistered(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
            var user = userManager.FindByEmail(email);
            return user != null;
        }

        public static int AddUserToHousehold(string email, int householdId)
        {
            int ret = 0;
            if (!String.IsNullOrWhiteSpace(email) && householdId > 0)
            {
                SqlParameter param1 = new SqlParameter("@Email", email);
                SqlParameter param2 = new SqlParameter("@HouseholdId", householdId);

                try
                {
                    DbContext.Database.ExecuteSqlCommand("AddUserToHouseholdByEmail @Email, @HouseholdId", param1, param2);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return ret;
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

            var invitation = DbContext.Invitations.FirstOrDefault(i => i.IsValid && i.RecipientEmail == emailFromCode);

            if (invitation == null || !invitation.IsValid)
            {
                return ret;
            }

            if (invitation.Code != new Guid(guid))
            {
                return ret;
            }

            if (invitation.ExpirationStatus == "Expired")
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

        public static bool EnsureDirectoryExists(string dirPath)
        {
            bool ret = false;
            if (!String.IsNullOrWhiteSpace(dirPath))
            {
                if (Directory.Exists(dirPath))
                {
                    ret = true;
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                    Thread.Sleep(1000);
                    ret = Directory.Exists(dirPath);
                }
            }
            return ret;
        }

        /// <summary>
        /// Check if image is within configured size and image format
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }

            bool ret = true;

            // Set a default of 3145728
            int maxSizeBytes = 3 * 1024 * 1024;
            if (Int32.TryParse(WebConfigurationManager.AppSettings["MaxImageUploadSize"], out var maxSize))
            {
                maxSizeBytes = maxSize;
            }

            if (file.ContentLength > maxSizeBytes || file.ContentLength < 1024)
            {
                ret = false;
            }
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    ret = ImageFormat.Jpeg.Equals(img.RawFormat) ||
                          ImageFormat.Png.Equals(img.RawFormat) ||
                          ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ret = false;
            }

            return ret;
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
