using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    /// <summary>
    /// Transaction Data Model
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key to BankAccount
        /// </summary>
        public int BankAccountId { get; set; }

        /// <summary>
        /// Foreign Key to BudgetItem
        /// </summary>
        public int? BudgetItemId { get; set; }

        /// <summary>
        /// Transaction Type Id
        /// </summary>
        public int TransactionTypeId { get; set; }

        /// <summary>
        /// Foreign Key to User who Created
        /// </summary>
        public string CreatedById { get; set; }

        /// <summary>
        /// Transaction Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Transaction Creation Date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Transaction Memo
        /// </summary>
        [StringLength(100)]
        public string Memo { get; set; }
    }
}
