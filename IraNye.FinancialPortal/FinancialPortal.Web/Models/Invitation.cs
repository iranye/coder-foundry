using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string ExpirationStatus
        {
            get
            {
                string status = "Expired";
                var expirationDateTime = Created.AddDays(TTL);
                if (expirationDateTime > DateTime.Now)
                {
                    var expirationDateTimeEod = new DateTime(expirationDateTime.Year, expirationDateTime.Month, expirationDateTime.Day, 23, 59, 59);
                    var daysUntilExpiration = expirationDateTimeEod.Subtract(DateTime.Now).Days;
                    status = $"Expires in {daysUntilExpiration} day(s)";
                }
                return status;
            }
        }

        public bool IsValid { get; set; } // Sets to false once TTL expires

        // Navs
        public virtual Household Household { get; set; }
    }
}
