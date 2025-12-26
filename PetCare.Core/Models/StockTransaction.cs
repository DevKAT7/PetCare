using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class StockTransaction
    {
        [Key]
        public int StockTransactionId { get; set; }
        public int QuantityChange { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string Reason { get; set; } = null!;
        public int MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}
