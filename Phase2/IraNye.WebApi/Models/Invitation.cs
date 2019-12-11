using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IraNye.WebApi.Models
{
    /// <summary>
    /// Invitation Data Model
    /// </summary>
    public class Invitation
    {
        /// <summary>
        /// Invitation PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key to Household
        /// </summary>
        [Required]
        public int HouseholdId { get; set; }

        /// <summary>
        /// Invitation Creation Date
        /// </summary>
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Invitation Recipient Email
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string RecipientEmail { get; set; }

        /// <summary>
        /// Invitation Code
        /// </summary>
        public Guid Code { get; set; }

        /// <summary>
        /// Invitation Time To Live
        /// </summary>
        public int TTL { get; set; }  //TimeToLive

        /// <summary>
        /// Invitation Expiration Status
        /// </summary>
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

        /// <summary>
        /// Invitation Is Valid Flag
        /// </summary>
        public bool IsValid { get; set; } // Sets to false once TTL expires
    }
}
