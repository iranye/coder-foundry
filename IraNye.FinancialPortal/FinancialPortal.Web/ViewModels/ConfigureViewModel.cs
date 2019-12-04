using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Models;

namespace FinancialPortal.Web.ViewModels
{
    // Scotty Code
    public class ConfigureHouseholdViewModel
    {

    }
    public class ConfigureViewModel
    {
        public int HouseholdId { get; set; }

        public string BankName { get; set; }
        public float StartingBalance { get; set; }
        public AccountType AccountType { get; set; }

        public string BucketName { get; set; }
        public float BucketTarget { get; set; }

        public string BucketItemName { get; set; }
        public float BucketItemTarget { get; set; }
    }
}
