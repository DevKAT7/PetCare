namespace PetCare.Application.Features.Vets.Dtos
{
    public class VetUpdateModel
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new List<int>();
    }
}
