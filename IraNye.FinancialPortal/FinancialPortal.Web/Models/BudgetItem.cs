using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        
        [StringLength(100, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }

        // Navs
        public virtual Budget Budget { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
