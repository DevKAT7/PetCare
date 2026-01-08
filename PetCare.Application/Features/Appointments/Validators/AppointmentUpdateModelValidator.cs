using FluentValidation;
using PetCare.Application.Features.Appointments.Dto;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class AppointmentUpdateModelValidator : AbstractValidator<AppointmentUpdateModel>
    {
        public AppointmentUpdateModelValidator()
        {
            RuleFor(x => x.AppointmentDateTime)
                .NotEmpty().WithMessage("Appointment date and time are required.");

            RuleFor(x => x.ReasonForVisit)
                .NotEmpty().WithMessage("Reason for visit is required.")
                .MaximumLength(200).WithMessage("Reason for visit cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Diagnosis)
                .MaximumLength(2000).WithMessage("Diagnosis cannot exceed 2000 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.");

            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("Pet is required.");

            RuleFor(x => x.VetId)
                .GreaterThan(0).WithMessage("Vet is required.");
        }
    }
}
