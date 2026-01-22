using PetCare.Core.Enums;

namespace PetCare.Application.Features.Appointments.Dtos
{
    public class AppointmentUpdateModel
    {
        public DateTime AppointmentDateTime { get; set; }
        public string? Description { get; set; }
        public string ReasonForVisit { get; set; } = null!;
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public AppointmentStatus Status { get; set; }
        public List<AppointmentProcedureReadModel> Procedures { get; set; } = new();
    }
}
