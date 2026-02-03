namespace PetCare.Shared.Dtos
{
    public class PetOwnerAuthDto
    {
        public int PetOwnerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
