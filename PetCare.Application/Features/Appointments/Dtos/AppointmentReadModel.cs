using PetCare.Core.Enums;

namespace PetCare.Application.Features.Appointments.Dto
{
    public class AppointmentReadModel
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Description { get; set; }
        public AppointmentStatus Status { get; set; }
        public string ReasonForVisit { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public int VetId { get; set; }
        public string VetName { get; set; } = string.Empty;
    }
}
