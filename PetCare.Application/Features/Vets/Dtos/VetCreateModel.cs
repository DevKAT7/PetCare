using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.Vets.Dto
{
    public class VetCreateModel
    {
        public string Email { get; set; } = null!;
        // Hasło tymczasowe, które nada administrator
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Pesel { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public DateOnly CareerStartDate { get; set; }
        public string Address { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new List<int>();
    }
}
