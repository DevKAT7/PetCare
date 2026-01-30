namespace PetCare.Shared.Dtos
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
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}
