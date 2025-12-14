using FluentValidation;
using PetCare.Application.Features.Invoices.Dto;

namespace PetCare.Application.Features.Invoices.Validators
{
    public class InvoiceCreateModelValidator : AbstractValidator<InvoiceCreateModel>
    {
        public InvoiceCreateModelValidator()
        {
            RuleFor(x => x.InvoiceDate).NotEmpty().WithMessage("Data faktury jest wymagana.");
            RuleFor(x => x.DueDate).NotEmpty().WithMessage("Termin płatności jest wymagany.");
            RuleFor(x => x.PetOwnerId).GreaterThan(0).WithMessage("PetOwnerId jest wymagane.");
            RuleFor(x => x.AppointmentId).GreaterThan(0).WithMessage("AppointmentId jest wymagane.");

            RuleFor(x => x.Items).NotNull().WithMessage("Items nie może być puste.");
            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.Description).NotEmpty().WithMessage("Opis pozycji jest wymagany.");
                items.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Cena jednostkowa nie może być ujemna.");
                items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Ilość musi być większa od 0.");
            });
        }
    }
}
