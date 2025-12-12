using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class Vet
    {
        [Key]
        public int VetId { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        [MaxLength(500)]
        public string Address { get; set; } = null!;
        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL musi mieć 11 cyfr.")]
        public string Pesel { get; set; } = null!;
        [MaxLength(50)]
        public string LicenseNumber { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public DateOnly CareerStartDate { get; set; }
        [Url]
        public string ProfilePictureUrl { get; set; } = null!;
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<VetSpecializationLink> SpecializationLinks { get; set; } =
        new HashSet<VetSpecializationLink>();
        [NotMapped]
        public int YearsOfExperience
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var age = today.Year - CareerStartDate.Year;

                if (CareerStartDate > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
