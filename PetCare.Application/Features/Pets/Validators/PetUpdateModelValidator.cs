using FluentValidation;
using PetCare.Application.Features.Pets.Dto;

namespace PetCare.Application.Features.Pets.Validators
{
    public class PetUpdateModelValidator : AbstractValidator<PetUpdateModel>
    {
        public PetUpdateModelValidator()
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
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of birth cannot be in the future.");

            RuleFor(x => x.ImageUrl)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Invalid image URL.");
        }

    }
}
