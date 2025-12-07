using PetCare.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        [MaxLength(200)]
        public string ReasonForVisit { get; set; } = null!;
        [MaxLength(2000)]
        public string Diagnosis { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Notes { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; } = null!;
        public int VetId { get; set; }
        public Vet Vet { get; set; } = null!;
        public ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();
    }
}

