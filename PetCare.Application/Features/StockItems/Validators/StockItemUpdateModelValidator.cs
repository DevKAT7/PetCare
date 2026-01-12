using FluentValidation;
using PetCare.Application.Features.StockItems.Dtos;

namespace PetCare.Application.Features.StockItems.Validators
{
    public class StockItemUpdateModelValidator : AbstractValidator<StockItemUpdateModel>
    {
        public StockItemUpdateModelValidator()
        {
            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock level cannot be negative.");

            RuleFor(x => x.ReorderLevel)
                    .GreaterThanOrEqualTo(0).WithMessage("Reorder level cannot be negative.");
        }
    }
}
