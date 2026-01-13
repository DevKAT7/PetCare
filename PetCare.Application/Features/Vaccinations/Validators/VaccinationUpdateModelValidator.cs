using FluentValidation;
using PetCare.Application.Features.Vaccinations.Dtos;

namespace PetCare.Application.Features.Vaccinations.Validators
{
    public class VaccinationUpdateModelValidator : AbstractValidator<VaccinationUpdateModel>
    {
        public VaccinationUpdateModelValidator()
        {
            RuleFor(x => x.VaccineName)
                .NotEmpty().WithMessage("Vaccine name is required.")
                .MaximumLength(200).WithMessage("Vaccine name can have a maximum of 200 characters.");

            RuleFor(x => x.NextDueDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("The next vaccination date must be in the future.");
        }
    }
}
