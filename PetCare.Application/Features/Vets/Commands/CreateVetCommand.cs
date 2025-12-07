using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.Vets.Commands
{
    public class CreateVetCommand : IRequest<int>
    {
        [Required(ErrorMessage = "Powiązanie z użytkownikiem systemu jest wymagane.")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MaxLength(100, ErrorMessage = "Imię nie może być dłuższe niż 100 znaków.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(100, ErrorMessage = "Nazwisko nie może być dłuższe niż 100 znaków.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "PESEL jest wymagany.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL musi mieć dokładnie 11 cyfr.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "PESEL może składać się tylko z cyfr.")]
        public string Pesel { get; set; } = null!;

        [Required(ErrorMessage = "Numer licencji jest wymagany.")]
        [MaxLength(50, ErrorMessage = "Numer licencji zbyt długi.")]
        public string LicenseNumber { get; set; } = null!;

        [Required(ErrorMessage = "Data zatrudnienia jest wymagana.")]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia kariery jest wymagana.")]
        public DateOnly CareerStartDate { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [MaxLength(500)]
        public string Address { get; set; } = null!;

        [Url(ErrorMessage = "Niepoprawny format adresu URL.")]
        [MaxLength(500)]
        public string ProfilePictureUrl { get; set; } = null!;

        [Required(ErrorMessage = "Opis jest wymagany.")]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public List<int> SpecializationIds { get; set; } = new List<int>();
    }
}
