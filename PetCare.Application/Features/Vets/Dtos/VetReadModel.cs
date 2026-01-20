namespace PetCare.Application.Features.Vets.Dtos
{
    public class VetReadModel
    {
        public int VetId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Pesel { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public DateOnly CareerStartDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public List<string> Specializations { get; set; } = new List<string>();
    }
}
