using FluentValidation;
using PetCare.Application.Features.Appointments.Dto;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class AppointmentCreateModelValidator : AbstractValidator<AppointmentCreateModel>
    {
        public AppointmentCreateModelValidator()
        {
            RuleFor(x => x.AppointmentDateTime)
                .NotEmpty().WithMessage("Appointment date and time are required.")
                .Must(dt => dt > DateTime.Now.AddMinutes(-1)).WithMessage("Appointment date must be in the future.");

            RuleFor(x => x.ReasonForVisit)
                .NotEmpty().WithMessage("Reason for visit is required.")
                .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Diagnosis)
                .MaximumLength(2000).WithMessage("Diagnosis cannot exceed 2000 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.");

            RuleFor(x => x.PetId)
                .NotEmpty().WithMessage("Pet is required.")
                .GreaterThan(0).WithMessage("Invalid Pet ID.");

            RuleFor(x => x.VetId)
                .NotEmpty().WithMessage("Vet is required.")
                .GreaterThan(0).WithMessage("Invalid Vet ID.");
        }
    }
}
