using FluentValidation;
using PetCare.Application.Features.VetSchedules.Dto;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class VetScheduleCreateModelValidator : AbstractValidator<VetScheduleCreateModel>
    {
        public VetScheduleCreateModelValidator()
        {
            RuleFor(x => x.VetId)
                .GreaterThan(0).WithMessage("VetId jest wymagane.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Czas rozpoczęcia jest wymagany.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("Czas zakończenia jest wymagany.");

            RuleFor(x => x).Custom((m, ctx) =>
            {
                if (m.EndTime <= m.StartTime)
                {
                    ctx.AddFailure("EndTime", "Czas zakończenia musi być późniejszy niż czas rozpoczęcia.");
                }
            });
        }
    }
}
