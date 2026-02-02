using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        [MaxLength(30)]
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly InvoiceDate { get; set; }
        public DateOnly DueDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount cannot be negative.")]
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateOnly? PaymentDate { get; set; }
        public int PetOwnerId { get; set; }
        public PetOwner PetOwner { get; set; } = null!;

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
            = new HashSet<InvoiceItem>();
    }
}
