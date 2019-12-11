using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    public class BankAccount
    {
        /// <summary>
        /// BankAccount PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key to Household
        /// </summary>
        public int HouseholdId { get; set; }

        /// <summary>
        /// BankAccount Name
        /// </summary>
        [StringLength(100, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// BankAccount Type
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// BankAccount Creation Date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Foreign Key to BankAccount Owner
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// BankAccount Starting Balance
        /// </summary>
        public decimal StartingBalance { get; set; }

        /// <summary>
        /// BankAccount Current Balance
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// BankAccount Low-Level Balance for triggering Notifications
        /// </summary>
        public decimal LowBalanceLevel { get; set; }
    }
}
