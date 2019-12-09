using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public int? BudgetItemId { get; set; }
        public int TransactionTypeId { get; set; }
        public string CreatedById { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        
        [StringLength(100)]
        public string Memo { get; set; }
    }
}
