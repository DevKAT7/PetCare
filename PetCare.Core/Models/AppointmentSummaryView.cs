namespace PetCare.Core.Models
{
    public class AppointmentSummaryView
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }

        public int PetId { get; set; }
        public string? PetName { get; set; }
        public string? PetSpecies { get; set; }
        public string? PetImageUrl { get; set; }

        public string? OwnerName { get; set; }

        public int VetId { get; set; }
        public string? VetName { get; set; }

        public int? InvoiceId { get; set; }
    }
}
