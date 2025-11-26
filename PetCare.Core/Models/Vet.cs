using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class Vet
    {
        public int VetId { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(500)]
        public string Address { get; set; } = null!;
        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL musi mieć 11 cyfr.")]
        public string Pesel { get; set; } = null!;
        [MaxLength(50)]
        public string LicenseNumber { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public DateOnly CareerStartDate { get; set; }
        public string ProfilePictureUrl { get; set; } = null!;
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        //maybe change to many-to-many
        public int VetSpezializationId { get; set; }
        public virtual VetSpezialization VetSpezialization { get; set; } = null!;
        [NotMapped]
        public int YearsOfExperience => DateTime.Now.Year - CareerStartDate.Year;
    }
}
