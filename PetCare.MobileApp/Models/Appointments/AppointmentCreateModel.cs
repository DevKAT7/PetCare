using System.ComponentModel.DataAnnotations;

namespace PetCare.MobileApp.Models.Appointments
{
    public class AppointmentCreateModel
    {
        [Required(ErrorMessage = "Appointment date and time are required.")]
        public DateTime AppointmentDateTime { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Reason for visit is required.")]
        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters.")]
        public string ReasonForVisit { get; set; } = null!;

        [StringLength(2000, ErrorMessage = "Diagnosis cannot exceed 2000 characters.")]
        public string? Diagnosis { get; set; }

        [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters.")]
        public string? Notes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Pet is required.")]
        public int PetId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vet is required.")]
        public int VetId { get; set; }
    }
}
