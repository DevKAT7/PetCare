using FluentValidation;
using PetCare.Application.Features.StockItems.Commands;
using PetCare.Application.Features.StockItems.Dtos;

namespace PetCare.Application.Features.StockItems.Validators
{
    public class UpdateStockItemCommandValidator : AbstractValidator<UpdateStockItemCommand>
    {
        public UpdateStockItemCommandValidator(IValidator<StockItemUpdateModel> stockItemModelValidator)
        {
            RuleFor(x => x.StockItem).SetValidator(stockItemModelValidator);
        }
    }
}
