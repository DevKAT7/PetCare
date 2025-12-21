using FluentValidation;
using PetCare.Application.Features.Medications.Dtos;

namespace PetCare.Application.Features.Medications.Validators
{
    public class MedicationCreateModelValidator : AbstractValidator<MedicationCreateModel>
    {
        public MedicationCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa leku jest wymagana.")
                .MaximumLength(200).WithMessage("Nazwa może mieć maksymalnie 200 znaków.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Opis leku jest wymagany.")
                .MaximumLength(500).WithMessage("Opis może mieć maksymalnie 500 znaków.");

            RuleFor(x => x.Manufacturer)
                .NotEmpty().WithMessage("Producent leku jest wymagany.")
                .MaximumLength(100).WithMessage("Producent może mieć maksymalnie 100 znaków.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Cena musi być nieujemna.");
        }
    }
}
