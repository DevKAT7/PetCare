namespace PetCare.Core.Models
{
    public class Vet
    {
        public int VetId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Pesel { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public int YearsOfExperience { get; set; }
        public string ProfilePictureUrl { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int VetSpezializationId { get; set; }
        public VetSpezialization VetSpezialization { get; set; } = null!;
    }
}
