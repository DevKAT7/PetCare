namespace PetCare.Application.Features.StockItems.Dtos
{
    public class StockItemReadModel
    {
        public int StockItemId { get; set; }
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
        public int MedicationId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
    }
}
