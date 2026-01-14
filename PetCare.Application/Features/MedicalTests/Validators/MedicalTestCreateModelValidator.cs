using FluentValidation;
using PetCare.Application.Features.MedicalTests.Dtos;

namespace PetCare.Application.Features.MedicalTests.Validators
{
    public class MedicalTestCreateModelValidator : AbstractValidator<MedicalTestCreateModel>
    {
        public MedicalTestCreateModelValidator()
        {
            RuleFor(x => x.TestName)
                .NotEmpty().WithMessage("Test name is required.")
                .MaximumLength(200).WithMessage("Test name can have a maximum of 200 characters.");

            RuleFor(x => x.Result)
                .MaximumLength(2000).WithMessage("Result can have a maximum of 2000 characters.");

            RuleFor(x => x.TestDate)
                .NotEmpty().WithMessage("Test date is required.");

            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("Pet is required.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment is required.");
        }
    }
}
