using FluentValidation;
using PetCare.Application.Features.Prescriptions.Dtos;

namespace PetCare.Application.Features.Prescriptions.Validators
{
    public class PrescriptionCreateModelValidator : AbstractValidator<PrescriptionCreateModel>
    {
        public PrescriptionCreateModelValidator()
        {
            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dawkowanie jest wymagane.")
                .MaximumLength(100).WithMessage("Dawkowanie może mieć maksymalnie 100 znaków.");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Częstotliwość jest wymagana.")
                .MaximumLength(100).WithMessage("Częstotliwość może mieć maksymalnie 100 znaków.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Data rozpoczęcia jest wymagana.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Data zakończenia jest wymagana.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Data zakończenia nie może być przed datą rozpoczęcia.");

            RuleFor(x => x.Instructions)
                .MaximumLength(500).WithMessage("Instrukcje mogą mieć maksymalnie 500 znaków.");

            RuleFor(x => x.PacksToDispense)
                .InclusiveBetween(1, 20).WithMessage("Ilość paczek musi być między 1 a 20.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("AppointmentId jest wymagane.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0).WithMessage("MedicationId jest wymagane.");
        }
    }
}
