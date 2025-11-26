using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class StockTransaction
    {
        [Key]
        public int StockTransactionId { get; set; }
        public int QuantityChange { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Required, MaxLength(100)]
        public string Reason { get; set; } = null!;
        [Required]
        public int MedicationId { get; set; }
        [ForeignKey(nameof(MedicationId))]
        public virtual Medication Medication { get; set; } = null!;
    }
}
