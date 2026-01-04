namespace PetCare.Application.Features.Vets.Dtos
{
    public class VetCreateModel
    {
        public string Email { get; set; } = string.Empty;
        // Hasło tymczasowe, które nada administrator
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Pesel { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public DateTime CareerStartDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new List<int>();
    }
}
