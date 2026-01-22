using FluentValidation;
using PetCare.Application.Features.VetSchedules.Dtos;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class ScheduleExceptionCreateModelValidator : AbstractValidator<ScheduleExceptionCreateModel>
    {
        public ScheduleExceptionCreateModelValidator()
        {
            RuleFor(x => x.VetId)
                            .GreaterThan(0).WithMessage("VetId is required.");

            RuleFor(x => x.ExceptionDate)
                .NotEmpty().WithMessage("Exception date is required.");

            RuleFor(x => x.Reason)
                .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters.");

            RuleFor(x => x).Custom((m, ctx) =>
            {
                if (!m.IsFullDayAbsence)
                {
                    if (!m.StartTime.HasValue)
                    {
                        ctx.AddFailure("StartTime", "Start time is required for partial absence.");
                    }

                    if (!m.EndTime.HasValue)
                    {
                        ctx.AddFailure("EndTime", "End time is required for partial absence.");
                    }

                    if (m.StartTime.HasValue && m.EndTime.HasValue && m.EndTime <= m.StartTime)
                    {
                        ctx.AddFailure("EndTime", "End time must be later than start time.");
                    }
                }
            });
        }
    }
}
