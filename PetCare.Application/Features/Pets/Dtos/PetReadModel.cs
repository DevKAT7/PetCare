namespace PetCare.Application.Features.Pets.Dto
{
    public class PetReadModel
    {
        public int PetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string? Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string? ImageUrl { get; set; }
        public int PetOwnerId { get; set; }
        public string PetOwnerName { get; set; } = string.Empty;
    }
}
