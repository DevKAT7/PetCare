using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class StockItem
    {
        [Key]
        public int StockItemId { get; set; }
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
        [Required]
        public int MedicationId { get; set; }
        [ForeignKey(nameof(MedicationId))]
        public virtual Medication Medication { get; set; } = null!;
    }
}
