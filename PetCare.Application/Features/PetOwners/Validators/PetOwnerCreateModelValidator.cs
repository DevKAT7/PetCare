using FluentValidation;
using PetCare.Application.Features.PetOwners.Dto;

namespace PetCare.Application.Features.PetOwners.Validators
{
    public class PetOwnerCreateModelValidator : AbstractValidator<PetOwnerCreateModel>
    {
        public PetOwnerCreateModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches("^[0-9+() -]*$").WithMessage("Invalid phone number format.")
                .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot be longer than 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot be longer than 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address cannot be longer than 500 characters.");

        }
    }
}
