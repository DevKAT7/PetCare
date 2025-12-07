using MediatR;
namespace PetCare.Application.Features.Vets.Queries
{
    public class GetAllVetsQuery : IRequest<List<VetDto>>
    {
    }

    public class VetDto
    {
        public int VetId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Specializations { get; set; } = new List<string>();
    }
}
