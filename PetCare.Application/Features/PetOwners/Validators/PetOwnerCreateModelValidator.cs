using FluentValidation;
using PetCare.Application.Features.PetOwners.Dto;

namespace PetCare.Application.Features.PetOwners.Validators
{
    public class PetOwnerCreateModelValidator : AbstractValidator<PetOwnerCreateModel>
    {
        public PetOwnerCreateModelValidator()
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

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres jest wymagany.")
                .MaximumLength(500).WithMessage("Adres nie może być dłuższy niż 500 znaków.");
        }
    }
}
