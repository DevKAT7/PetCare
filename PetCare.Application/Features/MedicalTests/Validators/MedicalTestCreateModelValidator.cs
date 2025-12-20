using FluentValidation;
using PetCare.Application.Features.MedicalTests.Dto;

namespace PetCare.Application.Features.MedicalTests.Validators
{
    public class MedicalTestCreateModelValidator : AbstractValidator<MedicalTestCreateModel>
    {
        public MedicalTestCreateModelValidator()
        {
            RuleFor(x => x.TestName)
                .NotEmpty().WithMessage("Nazwa badania jest wymagana.")
                .MaximumLength(200).WithMessage("Nazwa badania może mieć maksymalnie 200 znaków.");

            RuleFor(x => x.Result)
                .NotEmpty().WithMessage("Wynik jest wymagany.")
                .MaximumLength(2000).WithMessage("Wynik może mieć maksymalnie 2000 znaków.");

            RuleFor(x => x.TestDate)
                .NotEmpty().WithMessage("Data badania jest wymagana.");

            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("PetId jest wymagane.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("AppointmentId jest wymagane.");
        }
    }
}
