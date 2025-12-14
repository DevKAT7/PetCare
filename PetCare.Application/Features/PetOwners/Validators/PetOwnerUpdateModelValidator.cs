using FluentValidation;
using PetCare.Application.Features.PetOwners.Dto;

namespace PetCare.Application.Features.PetOwners.Validators
{
    public class PetOwnerUpdateModelValidator : AbstractValidator<PetOwnerUpdateModel>
    {
        public PetOwnerUpdateModelValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).EmailAddress().WithMessage("Niepoprawny format adresu email.");
            });

            When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Matches("^[0-9+() -]*$").WithMessage("Niepoprawny format numeru telefonu.")
                    .MaximumLength(20).WithMessage("Numer telefonu nie może być dłuższy niż 20 znaków.");
            });

            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName).MaximumLength(100).WithMessage("Imię nie może być dłuższe niż 100 znaków.");
            });

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName).MaximumLength(100).WithMessage("Nazwisko nie może być dłuższe niż 100 znaków.");
            });

            When(x => !string.IsNullOrEmpty(x.Address), () =>
            {
                RuleFor(x => x.Address).MaximumLength(500).WithMessage("Adres nie może być dłuższy niż 500 znaków.");
            });
        }
    }
}
