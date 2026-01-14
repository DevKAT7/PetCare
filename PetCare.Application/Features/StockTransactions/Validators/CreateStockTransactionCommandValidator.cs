using FluentValidation;
using PetCare.Application.Features.StockTransactions.Commands;
using PetCare.Application.Features.StockTransactions.Dtos;

namespace PetCare.Application.Features.StockTransactions.Validators
{
    public class CreateStockTransactionCommandValidator : AbstractValidator<CreateStockTransactionCommand>
    {
        public CreateStockTransactionCommandValidator(IValidator<StockTransactionCreateModel> stockTransactionModelValidator)
        {
            RuleFor(x => x.StockTransaction).SetValidator(stockTransactionModelValidator);
        }
    }
}
