using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class PetOwner
    {
        [Key]
        public int PetOwnerId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [MaxLength(500)]
        public string Address { get; set; } = null!;
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
