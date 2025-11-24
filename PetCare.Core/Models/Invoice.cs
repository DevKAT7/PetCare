namespace PetCare.Core.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateOnly InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }

        public int PetOwnerId { get; set; }
        public PetOwner PetOwner { get; set; } = null!;

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
    }
}
