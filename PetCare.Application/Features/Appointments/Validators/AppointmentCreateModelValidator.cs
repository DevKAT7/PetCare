using FluentValidation;
using PetCare.Application.Features.Appointments.Dto;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class AppointmentCreateModelValidator : AbstractValidator<AppointmentCreateModel>
    {
        public AppointmentCreateModelValidator()
        {
            RuleFor(x => x.AppointmentDateTime)
                .NotEmpty().WithMessage("Data i godzina wizyty są wymagane.")
                .Must(dt => dt > DateTime.UtcNow.AddMinutes(-1)).WithMessage("Data wizyty musi być w przyszłości.");

            RuleFor(x => x.ReasonForVisit)
                .NotEmpty().WithMessage("Powód wizyty jest wymagany.")
                .MaximumLength(200).WithMessage("Powód wizyty nie może przekraczać 200 znaków.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis nie może przekraczać 500 znaków.");

            RuleFor(x => x.Diagnosis)
                .MaximumLength(2000).WithMessage("Diagnoza nie może przekraczać 2000 znaków.");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notatki nie mogą przekraczać 2000 znaków.");

            RuleFor(x => x.PetId)
                .NotEmpty().WithMessage("PetId jest wymagany.")
                .GreaterThan(0).WithMessage("PetId musi być większe od 0.");

            RuleFor(x => x.VetId)
                .NotEmpty().WithMessage("VetIt jest wymagany.")
                .GreaterThan(0).WithMessage("VetId musi być większe od 0.");
        }
    }
}
