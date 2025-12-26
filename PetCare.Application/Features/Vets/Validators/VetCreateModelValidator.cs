using FluentValidation;
using PetCare.Application.Features.Vets.Dto;

namespace PetCare.Application.Features.Vets.Validators
{
    public class VetCreateModelValidator : AbstractValidator<VetCreateModel>
    {
        public VetCreateModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Niepoprawny format adresu email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(8).WithMessage("Hasło musi mieć co najmniej 8 znaków.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
                .Matches("^[0-9+() -]*$").WithMessage("Niepoprawny format numeru telefonu.")
                .MaximumLength(20).WithMessage("Numer telefonu nie może być dłuższy niż 20 znaków.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(100).WithMessage("Imię nie może być dłuższe niż 100 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(100).WithMessage("Nazwisko nie może być dłuższe niż 100 znaków.");

            RuleFor(x => x.Pesel)
                .NotEmpty().WithMessage("PESEL jest wymagany.")
                .Length(11).WithMessage("PESEL musi mieć dokładnie 11 cyfr.")
                .Matches("^[0-9]*$").WithMessage("PESEL może składać się tylko z cyfr.");

            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("Numer licencji jest wymagany.")
                .MaximumLength(50).WithMessage("Numer licencji zbyt długi.");

            RuleFor(x => x.HireDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Data zatrudnienia nie może być w przyszłości.");

            RuleFor(x => x.CareerStartDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Data rozpoczęcia kariery nie może być w przyszłości.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres jest wymagany.")
                .MaximumLength(500).WithMessage("Adres nie może być dłuższy niż 500 znaków.");

            RuleFor(x => x.ProfilePictureUrl)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Niepoprawny format adresu URL.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Opis jest wymagany.")
                .MaximumLength(2000).WithMessage("Opis nie może być dłuższy niż 2000 znaków.");

            RuleForEach(x => x.SpecializationIds).GreaterThan(0).WithMessage("Niepoprawne id specjalizacji.");

            RuleFor(x => x).Custom((m, ctx) => {
                if (m.CareerStartDate > m.HireDate)
                {
                    ctx.AddFailure("CareerStartDate", "Data rozpoczęcia kariery nie może być późniejsza niż data zatrudnienia.");
                }
            });
        }
    }
}
