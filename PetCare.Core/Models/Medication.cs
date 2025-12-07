using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Manufacturer { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Prescription> Prescriptions { get; set; }
            = new HashSet<Prescription>();
        public StockItem? StockItem { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }
             = new HashSet<StockTransaction>();
    }
}
