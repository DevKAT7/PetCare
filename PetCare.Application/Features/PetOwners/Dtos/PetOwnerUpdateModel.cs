namespace PetCare.Application.Features.PetOwners.Dtos
{
    public class PetOwnerUpdateModel
    {
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
