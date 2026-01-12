using FluentValidation;
using PetCare.Application.Features.Medications.Dtos;

namespace PetCare.Application.Features.Medications.Validators
{
    public class MedicationCreateModelValidator : AbstractValidator<MedicationCreateModel>
    {
        public MedicationCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Medication name is required.")
                .MaximumLength(200).WithMessage("Medication name can have a maximum of 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Medication description is required.")
                .MaximumLength(500).WithMessage("Medication description can have a maximum of 500 characters.");

            RuleFor(x => x.Manufacturer)
                .NotEmpty().WithMessage("Medication manufacturer is required.")
                .MaximumLength(100).WithMessage("Manufacturer name can have a maximum of 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than zero.");
        }
    }
}
