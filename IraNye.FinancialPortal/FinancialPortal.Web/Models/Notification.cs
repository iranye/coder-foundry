using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class Notification
    {
        public int Id { get; set; }
        
        [Required]
        public int BankAccountId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Subject must be between {2} and {1} characters long.", MinimumLength = 1)]
        public String Subject { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Body must be between {2} and {1} characters long.", MinimumLength = 1)]
        public String Body { get; set; }

        [Required]
        public String RecipientId { get; set; }
        public bool IsRead { get; set; }

        // Navs
        public virtual Household Household { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}
