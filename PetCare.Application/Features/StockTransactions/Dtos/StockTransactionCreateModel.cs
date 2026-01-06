namespace PetCare.Application.Features.StockTransactions.Dtos
{
    public class StockTransactionCreateModel
    {
        public int QuantityChange { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int MedicationId { get; set; }
    }
}
