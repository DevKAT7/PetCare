namespace PetCare.Application.Features.StockItems.Dtos
{
    public class StockItemCreateModel
    {
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
        public int MedicationId { get; set; }
    }
}
