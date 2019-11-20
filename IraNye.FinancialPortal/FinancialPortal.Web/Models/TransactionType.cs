using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        
        [StringLength(80)]
        public string Type { get; set; }
    }
}
