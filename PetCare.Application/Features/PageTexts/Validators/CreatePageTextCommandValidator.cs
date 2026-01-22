using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.PageTexts.Commands;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PageTexts.Validators
{
    public class CreatePageTextCommandValidator : AbstractValidator<CreatePageTextCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePageTextCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Key)
                .NotEmpty().WithMessage("Key is required.")
                .MaximumLength(50).WithMessage("Key must not exceed 50 characters.")
                .MustAsync(BeUniqueKey).WithMessage("This Key already exists in the database.");

            RuleFor(v => v.Value)
                .NotEmpty().WithMessage("Value is required.");
        }

        private async Task<bool> BeUniqueKey(string key, CancellationToken cancellationToken)
        {
            return !await _context.PageTexts
                .AnyAsync(x => x.Key == key, cancellationToken);
        }
    }
}
