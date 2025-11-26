using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Dosage { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Frequency { get; set; } = null!;
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [MaxLength(500)]
        public string Instructions { get; set; } = null!;
        [Required]
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment { get; set; } = null!;
        [Required]
        public int MedicationId { get; set; }
        [ForeignKey(nameof(MedicationId))]
        public virtual Medication Medication { get; set; } = null!;
        [Required]
        [Range(1, 20)]
        public int PacksToDispense { get; set; } = 1;
    }
}
