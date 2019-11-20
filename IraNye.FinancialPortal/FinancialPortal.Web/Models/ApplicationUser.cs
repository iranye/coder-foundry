using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? HouseholdId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(90, ErrorMessage = "Display Name must be no longer than {1} characters long.")]
        public string DisplayName { get; set; }
        
        [StringLength(255)]
        public string AvatarPath { get; set; }

        // Navs
        public virtual Household Household { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}