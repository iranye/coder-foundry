using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        
        [StringLength(100, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
