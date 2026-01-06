using FluentValidation;
using PetCare.Application.Features.StockTransactions.Dtos;

namespace PetCare.Application.Features.StockTransactions.Validators
{
    public class StockTransactionCreateModelValidator : AbstractValidator<StockTransactionCreateModel>
    {
        public StockTransactionCreateModelValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Powód transakcji jest wymagany.")
                .MaximumLength(100).WithMessage("Powód może mieć maksymalnie 100 znaków.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0).WithMessage("Id leku jest wymagane.");
        }
    }
}
