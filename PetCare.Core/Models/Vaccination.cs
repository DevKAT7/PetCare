using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class Vaccination
    {
        [Key]
        public int VaccinationId { get; set; }
        [MaxLength(200)]
        public string VaccineName { get; set; } = null!;
        public DateOnly VaccinationDate { get; set; }
        public DateOnly? NextDueDate { get; set; }
        public bool IsReminderSent { get; set; } = false;
        public int PetId { get; set; }
        public Pet Pet { get; set; } = null!;
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
    }
}
