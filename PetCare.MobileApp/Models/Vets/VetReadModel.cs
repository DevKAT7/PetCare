namespace PetCare.MobileApp.Models.Vets
{
    public class VetReadModel
    {
        public int VetId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public List<string> Specializations { get; set; } = new List<string>();
    }
}
