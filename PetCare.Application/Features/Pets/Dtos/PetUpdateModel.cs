using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.Pets.Dto
{
    public class PetUpdateModel
    {
        public string Name { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string? Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string? ImageUrl { get; set; }
    }
}
