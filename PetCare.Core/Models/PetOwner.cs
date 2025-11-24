namespace PetCare.Core.Models
{
    public class PetOwner
    {
        public int PetOwnerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
