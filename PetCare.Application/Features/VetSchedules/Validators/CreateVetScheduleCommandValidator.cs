using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class CreateVetScheduleCommandValidator : AbstractValidator<CreateVetScheduleCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateVetScheduleCommandValidator(
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
                                    && s.DayOfWeek == dayOfWeek, cancellationToken);

                    return !exists;
                })
                .WithMessage("The schedule for this day of the week already exists. Use edit.")
                .OverridePropertyName("DayOfWeek");
        }
    }
}
