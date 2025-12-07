using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class InvoiceItem
    {
        [Key]
        public int InvoiceItemId { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}
