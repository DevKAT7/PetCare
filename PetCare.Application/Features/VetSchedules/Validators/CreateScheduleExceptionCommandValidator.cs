using FluentValidation;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class CreateScheduleExceptionCommandValidator : AbstractValidator<CreateScheduleExceptionCommand>
    {
        public CreateScheduleExceptionCommandValidator(IValidator<ScheduleExceptionCreateModel> scheduleExceptionValidator)
        {
            RuleFor(x => x.Exception).SetValidator(scheduleExceptionValidator);
        }
    }
}
