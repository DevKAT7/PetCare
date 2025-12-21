using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        [MaxLength(100)]
        public string Dosage { get; set; } = null!;
        [MaxLength(100)]
        public string Frequency { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        [MaxLength(500)]
        public string Instructions { get; set; } = null!;
        [Range(1, 20)]
        public int PacksToDispense { get; set; } = 1;
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public int MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}
