namespace PetCare.MobileApp.Models.Invoices
{
    public class InvoiceItemReadModel
    {
        public int InvoiceItemId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}