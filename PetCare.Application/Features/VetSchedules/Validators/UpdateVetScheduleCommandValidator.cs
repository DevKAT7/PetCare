using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class UpdateVetScheduleCommandValidator : AbstractValidator<UpdateVetScheduleCommand>
    {
            private readonly ApplicationDbContext _context;

        public UpdateVetScheduleCommandValidator(
            IValidator<VetScheduleCreateModel> scheduleValidator,
            ApplicationDbContext context)
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
