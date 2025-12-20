using FluentValidation;
using PetCare.Application.Features.VetSchedules.Dto;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class ScheduleExceptionCreateModelValidator : AbstractValidator<ScheduleExceptionCreateModel>
    {
        public ScheduleExceptionCreateModelValidator()
        {
            RuleFor(x => x.VetId)
                .GreaterThan(0).WithMessage("VetId jest wymagane.");

            RuleFor(x => x.ExceptionDate)
                .NotEmpty().WithMessage("Data wyjątku w grafiku jest wymagana.");

            RuleFor(x => x.Reason)
                .MaximumLength(200).WithMessage("Powód może mieć maksymalnie 200 znaków.");

            RuleFor(x => x).Custom((m, ctx) =>
            {
                if (!m.IsFullDayAbsence)
                {
                    if (!m.StartTime.HasValue)
                    {
                        ctx.AddFailure("StartTime", "Czas rozpoczęcia jest wymagany jeśli nie jest to cały dzień nieobecności.");
                    }

                    if (!m.EndTime.HasValue)
                    {
                        ctx.AddFailure("EndTime", "Czas zakończenia jest wymagany jeśli nie jest to cały dzień nieobecności.");
                    }

                    if (m.StartTime.HasValue && m.EndTime.HasValue && m.EndTime <= m.StartTime)
                    {
                        ctx.AddFailure("EndTime", "Czas zakończenia musi być późniejszy niż czas rozpoczęcia.");
                    }
                }
            });
        }
    }
}
