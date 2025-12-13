using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.Vets.Dto
{
    public class VetUpdateModel
    {
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        [MaxLength(20, ErrorMessage = "Numer telefonu nie może być dłuższy niż 20 znaków.")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Imię nie może być dłuższe niż 100 znaków.")]
        public string FirstName { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Nazwisko nie może być dłuższe niż 100 znaków.")]
        public string LastName { get; set; } = null!;

        [MaxLength(500)]
        public string Address { get; set; } = null!;

        [Url(ErrorMessage = "Niepoprawny format adresu URL.")]
        [MaxLength(500)]
        public string ProfilePictureUrl { get; set; } = null!;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public List<int> SpecializationIds { get; set; } = new List<int>();
    }
}
