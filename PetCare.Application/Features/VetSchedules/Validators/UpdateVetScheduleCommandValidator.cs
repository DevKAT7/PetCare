using FluentValidation;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class UpdateVetScheduleCommandValidator : AbstractValidator<UpdateVetScheduleCommand>
    {
        public UpdateVetScheduleCommandValidator(IValidator<VetScheduleCreateModel> scheduleValidator)
        {
            RuleFor(x => x.Schedule).SetValidator(scheduleValidator);
        }
    }
}
