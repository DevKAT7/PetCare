using FluentValidation;
using PetCare.Application.Features.StockItems.Commands;
using PetCare.Application.Features.StockItems.Dtos;

namespace PetCare.Application.Features.StockItems.Validators
{
    public class CreateStockItemCommandValidator : AbstractValidator<CreateStockItemCommand>
    {
        public CreateStockItemCommandValidator(IValidator<StockItemCreateModel> stockItemModelValidator)
        {
            RuleFor(x => x.StockItem).SetValidator(stockItemModelValidator);
        }
    }
}
