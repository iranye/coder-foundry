using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
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
    }
}
