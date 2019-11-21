using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialPortal.Web.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        [Required]
        public int HouseholdId { get; set; }
        
        [Required]
        public DateTime Created { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string RecipientEmail { get; set; }

        public Guid Code { get; set; }
        public int TTL { get; set; }  //TimeToLive
        public bool IsValid { get; set; } // Sets to false once TTL expires

        // Navs
        public virtual Household Household { get; set; }
    }
}
