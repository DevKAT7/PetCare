using FluentValidation;
using PetCare.Application.Features.VetSchedules.Dtos;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class VetScheduleCreateModelValidator : AbstractValidator<VetScheduleCreateModel>
    {
        public VetScheduleCreateModelValidator()
        {
            RuleFor(x => x.VetId)
                .GreaterThan(0).WithMessage("VetId is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .Must(BeValidTimeStep).WithMessage("Minutes must be 00 or 30.")
                .Must(BeInWorkingHours).WithMessage("Working hours are 09:00 - 16:30.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .Must(BeValidTimeStep).WithMessage("Minutes must be 00 or 30.")
                .Must(BeInWorkingHours).WithMessage("Working hours are 09:00 - 16:30.");

            RuleFor(x => x).Custom((m, ctx) =>
            {
                if (m.EndTime <= m.StartTime)
                {
                    ctx.AddFailure("EndTime", "End time must be later than start time.");
                }
            });
        }

        private bool BeValidTimeStep(TimeOnly time)
        {
            return time.Minute == 0 || time.Minute == 30;
        }

        private bool BeInWorkingHours(TimeOnly time)
        {
            var startLimit = new TimeOnly(9, 0);
            var endLimit = new TimeOnly(16, 30);

            return time >= startLimit && time <= endLimit;
        }
    }
}
