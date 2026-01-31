using FluentValidation;
using PetCare.Application.Features.Pets.Dtos;

namespace PetCare.Application.Features.Pets.Validators
{
    public class PetCreateModelValidator : AbstractValidator<PetCreateModel>
    {
        public PetCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Species)
                .NotEmpty().WithMessage("Species is required.")
                .MaximumLength(50).WithMessage("Species cannot exceed 50 characters.");

            RuleFor(x => x.Breed)
                .MaximumLength(50).WithMessage("Breed cannot exceed 50 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(date => date.Date <= DateTime.Today)
                .WithMessage("Date of birth cannot be in the future.");

            RuleFor(x => x.PetOwnerId)
                .GreaterThan(0).WithMessage("Pet Owner is required.");

            RuleFor(x => x.ImageUrl)
                .Must(uri => string.IsNullOrEmpty(uri)
                             || uri.StartsWith("data:")
                             || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Invalid image URL.");
        }

    }
}
