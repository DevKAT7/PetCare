namespace PetCare.MobileApp.Models.Invoices
{
    public class InvoiceReadModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int PetOwnerId { get; set; }
        public string PetOwnerName { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
        public List<InvoiceItemReadModel> Items { get; set; } = new List<InvoiceItemReadModel>();
    }
}