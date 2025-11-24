using Microsoft.AspNetCore.Identity;
namespace PetCare.Core.Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
