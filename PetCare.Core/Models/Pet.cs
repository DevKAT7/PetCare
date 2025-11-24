namespace PetCare.Core.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; } = null!;
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public int PetOwnerId { get; set; }
        public PetOwner PetOwner { get; set; } = null!;
    }
}
