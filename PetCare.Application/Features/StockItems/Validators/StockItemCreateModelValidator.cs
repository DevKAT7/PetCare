using FluentValidation;
using PetCare.Application.Features.StockItems.Dtos;

namespace PetCare.Application.Features.StockItems.Validators
{
    public class StockItemCreateModelValidator : AbstractValidator<StockItemCreateModel>
    {
        public StockItemCreateModelValidator()
        {
            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock level cannot be negative.");

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Reorder level cannot be negative.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0).WithMessage("Medication is required.");
        }
    }
}
