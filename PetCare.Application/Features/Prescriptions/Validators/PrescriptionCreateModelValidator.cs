using FluentValidation;
using PetCare.Application.Features.Prescriptions.Dtos;

namespace PetCare.Application.Features.Prescriptions.Validators
{
    public class PrescriptionCreateModelValidator : AbstractValidator<PrescriptionCreateModel>
    {
        public PrescriptionCreateModelValidator()
        {
            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dosage is required.")
                .MaximumLength(100).WithMessage("Dosage cannot exceed 100 characters.");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency is required.")
                .MaximumLength(100).WithMessage("Frequency cannot exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date cannot be earlier than start date.");

            RuleFor(x => x.Instructions)
                .MaximumLength(500).WithMessage("Instructions cannot exceed 500 characters.");

            RuleFor(x => x.PacksToDispense)
                .InclusiveBetween(1, 20).WithMessage("Packs to dispense must be between 1 and 20.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment ID is required.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0).WithMessage("Medication selection is required.");
        }
    }
}
