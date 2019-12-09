using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        
        [StringLength(80)]
        public string Type { get; set; }
    }
}
