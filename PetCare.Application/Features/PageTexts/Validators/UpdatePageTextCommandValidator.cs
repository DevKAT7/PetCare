using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.PageTexts.Commands;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PageTexts.Validators
{
    public class UpdatePageTextCommandValidator : AbstractValidator<UpdatePageTextCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePageTextCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .GreaterThan(0);

            RuleFor(v => v.Key)
                .NotEmpty().WithMessage("Key is required.")
                .MaximumLength(50).WithMessage("Key must not exceed 50 characters.")
                .MustAsync(BeUniqueKeyExcludingCurrent).WithMessage("This Key is already taken by another entry.");

            RuleFor(v => v.Value)
                .NotEmpty().WithMessage("Value is required.");
        }

        private async Task<bool> BeUniqueKeyExcludingCurrent(UpdatePageTextCommand command, string key, CancellationToken cancellationToken)
        {
            return !await _context.PageTexts
                .AnyAsync(x => x.Key == key && x.PageTextId != command.Id, cancellationToken);
        }
    }
}
