using FluentValidation;
using PetCare.Application.Features.StockItems.Dtos;

namespace PetCare.Application.Features.StockItems.Validators
{
    public class StockItemCreateModelValidator : AbstractValidator<StockItemCreateModel>
    {
        public StockItemCreateModelValidator()
        {
            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("Stan magazynowy nie może być ujemny.");

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Poziom ponownego zamówienia nie może być ujemny.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0).WithMessage("Id leku jest wymagane.");
        }
    }
}
