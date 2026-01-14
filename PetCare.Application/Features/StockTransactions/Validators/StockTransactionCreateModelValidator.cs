using FluentValidation;
using PetCare.Application.Features.StockTransactions.Dtos;

namespace PetCare.Application.Features.StockTransactions.Validators
{
    public class StockTransactionCreateModelValidator : AbstractValidator<StockTransactionCreateModel>
    {
        public StockTransactionCreateModelValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Transaction reason is required.")
                .MaximumLength(100).WithMessage("Transaction reason can have a maximum of 100 characters.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0).WithMessage("Medication is required.");
        }
    }
}
