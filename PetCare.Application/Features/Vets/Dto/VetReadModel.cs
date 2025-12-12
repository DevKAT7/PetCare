namespace PetCare.Application.Features.Vets.Dto
{
    public class VetReadModel
    {
        public int VetId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly CareerStartDate { get; set; }
        public int YearsOfExperience { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Specializations { get; set; } = new List<string>();
    }
}
