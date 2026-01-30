using PetCare.MobileApp.Enums;

namespace PetCare.MobileApp.Models.Appointments
{
    public class AppointmentReadModel
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Description { get; set; }
        public AppointmentStatus Status { get; set; }
        public string ReasonForVisit { get; set; } = string.Empty;
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string PetSpecies { get; set; } = string.Empty;
        public string? PetImageUrl { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public int VetId { get; set; }
        public string VetName { get; set; } = string.Empty;
        public int? InvoiceId { get; set; }
        public List<AppointmentProcedureReadModel> Procedures { get; set; } = new();
    }
}
