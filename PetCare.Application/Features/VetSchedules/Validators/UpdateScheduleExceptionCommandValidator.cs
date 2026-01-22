using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class UpdateScheduleExceptionCommandValidator : AbstractValidator<UpdateScheduleExceptionCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateScheduleExceptionCommandValidator(
            IValidator<ScheduleExceptionCreateModel> exceptionValidator,
            IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Exception).SetValidator(exceptionValidator);

            RuleFor(x => x.Exception.ExceptionDate)
                .MustAsync(async (command, date, cancellationToken) =>
                {
                    bool exists = await _context.ScheduleExceptions
                        .AnyAsync(e => e.VetId == command.Exception.VetId
                                    && e.ExceptionDate == date
                                    && e.ScheduleExceptionId != command.Id,
                                    cancellationToken);

                    return !exists;
                })
                .WithMessage("An exception for this date already exists.")
                .OverridePropertyName("ExceptionDate");
        }
    }
}
