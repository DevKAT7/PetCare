using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.PetOwners.Dto
{
    public class PetOwnerCreateModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
