using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class MedicalTest
    {
        [Key]
        public int MedicalTestId { get; set; }
        [MaxLength(200)]
        public string TestName { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string Result { get; set; } = string.Empty;
        public DateOnly TestDate { get; set; }
        public string AttachmentUrl { get; set; } = string.Empty;
        public int PetId { get; set; }
        public Pet Pet { get; set; } = null!;
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
    }
}
