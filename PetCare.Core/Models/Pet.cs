using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [MaxLength(50)]
        public string Species { get; set; } = null!;
        [MaxLength(50)]
        public string? Breed { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public bool IsActive { get; set; } = true;
        [Url]
        public string? ImageUrl { get; set; }
        public int PetOwnerId { get; set; }
        public PetOwner PetOwner { get; set; } = null!;
        public ICollection<Vaccination> Vaccinations { get; set; }
            = new HashSet<Vaccination>();
        public ICollection<MedicalTest> MedicalTests { get; set; }
            = new HashSet<MedicalTest>();
        public ICollection<Appointment> Appointments { get; set; }
            = new HashSet<Appointment>();
    }
}
