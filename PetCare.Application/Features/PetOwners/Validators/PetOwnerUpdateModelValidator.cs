using FluentValidation;
using PetCare.Application.Features.PetOwners.Dtos;

namespace PetCare.Application.Features.PetOwners.Validators
{
    public class PetOwnerUpdateModelValidator : AbstractValidator<PetOwnerUpdateModel>
    {
        public PetOwnerUpdateModelValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Invalid email address format.");
            });

            When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .MinimumLength(9).WithMessage("Phone number must be at least 9 characters long.")
                    .Matches("^[0-9+() -]*$").WithMessage("Invalid phone number format.")
                    .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(100).WithMessage("First name cannot be longer than 100 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(100).WithMessage("Last name cannot be longer than 100 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.Address), () =>
            {
                RuleFor(x => x.Address)
                    .MaximumLength(500).WithMessage("Address cannot be longer than 500 characters.");
            });
        }

    }
}
