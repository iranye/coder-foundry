﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }

        [StringLength(100, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        public AccountType AccountType { get; set; }
        public DateTime Created { get; set; }
        public string OwnerId { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal LowBalanceLevel { get; set; }

        // Navs
        public virtual Household Household { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
