using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.PetOwners.Dto
{
    public class PetOwnerCreateModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        [MaxLength(20, ErrorMessage = "Numer telefonu nie może być dłuższy niż 20 znaków.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MaxLength(100, ErrorMessage = "Imię nie może być dłuższe niż 100 znaków.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(100, ErrorMessage = "Nazwisko nie może być dłuższe niż 100 znaków.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [MaxLength(500)]
        public string Address { get; set; } = null!;
    }
}
