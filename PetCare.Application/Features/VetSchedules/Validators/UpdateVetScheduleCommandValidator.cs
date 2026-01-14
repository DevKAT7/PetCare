using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class UpdateVetScheduleCommandValidator : AbstractValidator<UpdateVetScheduleCommand>
    {
            private readonly IApplicationDbContext _context;

        public UpdateVetScheduleCommandValidator(
            IValidator<VetScheduleCreateModel> scheduleValidator,
            IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Schedule).SetValidator(scheduleValidator);

            RuleFor(x => x.Schedule.DayOfWeek)
                .MustAsync(async (command, dayOfWeek, cancellationToken) =>
                {
                    bool exists = await _context.VetSchedules
                        .AnyAsync(s => s.VetId == command.Schedule.VetId
                                    && s.DayOfWeek == dayOfWeek
                                    && s.VetScheduleId != command.Id,
                                    cancellationToken);

                    return !exists;
                })
                .WithMessage("Schedule for this day already exists.")
                .OverridePropertyName("DayOfWeek");
        }
    }
}
