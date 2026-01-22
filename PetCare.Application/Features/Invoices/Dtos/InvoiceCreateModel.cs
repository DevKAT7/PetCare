namespace PetCare.Application.Features.Invoices.Dtos
{
    public class InvoiceItemCreateModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class InvoiceCreateModel
    {
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public int PetOwnerId { get; set; }
        public int AppointmentId { get; set; }
        public List<InvoiceItemCreateModel> Items { get; set; } = new List<InvoiceItemCreateModel>();
    }
}
