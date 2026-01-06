using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSchedules.Validators
{
    public class CreateScheduleExceptionCommandValidator : AbstractValidator<CreateScheduleExceptionCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateScheduleExceptionCommandValidator(
            IValidator<ScheduleExceptionCreateModel> modelValidator,
            ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Exception).SetValidator(modelValidator);

            RuleFor(x => x.Exception.ExceptionDate)
                .MustAsync(async (command, date, cancellationToken) =>
                {
                    bool exists = await _context.ScheduleExceptions
                        .AnyAsync(e => e.VetId == command.Exception.VetId
                                    && e.ExceptionDate == date, cancellationToken);

                    return !exists;
                })
                .WithMessage("An exception has already been defined for this doctor on that day.")
                .OverridePropertyName("ExceptionDate");
        }
    }
}
