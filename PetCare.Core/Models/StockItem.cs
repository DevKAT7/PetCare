using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class StockItem
    {
        [Key]
        public int StockItemId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stan magazynowy nie może być ujemny.")]
        public int CurrentStock { get; set; }
        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }
        public int MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}
