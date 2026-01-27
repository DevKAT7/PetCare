namespace PetCare.Shared.Dtos
{
    public class PetUpdateModel
    {
        public string Name { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string? Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string? ImageUrl { get; set; }
        public int PetOwnerId { get; set; }
    }
}
