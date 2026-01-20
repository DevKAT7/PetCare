namespace PetCare.Application.Features.PetOwners.Dtos
{
    public class PetOwnerReadModel
    {
        public int PetOwnerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<string> PetNames { get; set; } = new List<string>();
    }
}
