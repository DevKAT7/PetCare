using FluentValidation;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class CreateVetScheduleCommandValidator : AbstractValidator<CreateVetScheduleCommand>
    {
        public CreateVetScheduleCommandValidator(IValidator<VetScheduleCreateModel> scheduleValidator)
        {
            RuleFor(x => x.Schedule).SetValidator(scheduleValidator);
        }
    }
}
