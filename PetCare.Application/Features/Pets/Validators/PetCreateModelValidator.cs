using FluentValidation;
using PetCare.Application.Features.Pets.Dto;

namespace PetCare.Application.Features.Pets.Validators
{
    public class PetCreateModelValidator : AbstractValidator<PetCreateModel>
    {
        public PetCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa jest wymagana.")
                .MaximumLength(50).WithMessage("Nazwa nie może przekraczać 50 znaków.");

            RuleFor(x => x.Species)
                .NotEmpty().WithMessage("Gatunek jest wymagany.")
                .MaximumLength(50).WithMessage("Gatunek nie może przekraczać 50 znaków.");

            RuleFor(x => x.Breed)
                .MaximumLength(50).WithMessage("Rasa nie może przekraczać 50 znaków.");

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data urodzenia nie może być w przyszłości.");

            RuleFor(x => x.PetOwnerId)
                .GreaterThan(0).WithMessage("PetOwnerId jest wymagany.");

            RuleFor(x => x.ImageUrl)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Niepoprawny URL obrazu.");
        }
    }
}
