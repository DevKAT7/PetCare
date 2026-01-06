namespace PetCare.Application.Features.StockTransactions.Dtos
{
    public class StockTransactionReadModel
    {
        public int StockTransactionId { get; set; }
        public int QuantityChange { get; set; }
        public DateTime Timestamp { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int MedicationId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
    }
}
