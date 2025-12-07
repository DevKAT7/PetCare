using Microsoft.AspNetCore.Identity;
namespace PetCare.Core.Models
{
    public class User : IdentityUser
    {
        //in IdentityUser there is already Id, UserName, Email, PasswordHash, PhoneNumber
        public bool IsActive { get; set; } = true;
        public Vet? VetProfile { get; set; }
        public PetOwner? PetOwnerProfile { get; set; }
    }
}
