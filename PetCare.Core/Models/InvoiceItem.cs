namespace PetCare.Core.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public string Description { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}
