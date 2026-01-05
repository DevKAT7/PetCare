using FluentValidation;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class UpdateScheduleExceptionCommandValidator : AbstractValidator<UpdateScheduleExceptionCommand>
    {
        public UpdateScheduleExceptionCommandValidator(IValidator<ScheduleExceptionCreateModel> exceptionValidator)
        {
            RuleFor(x => x.Exception).SetValidator(exceptionValidator);
        }
    }
}
