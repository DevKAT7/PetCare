using FluentValidation;
using PetCare.Application.Features.Invoices.Dto;

namespace PetCare.Application.Features.Invoices.Validators
{
    public class InvoiceCreateModelValidator : AbstractValidator<InvoiceCreateModel>
    {
        public InvoiceCreateModelValidator()
        {
            RuleFor(x => x.InvoiceDate)
                .NotEmpty().WithMessage("Invoice date is required.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required.");

            RuleFor(x => x.PetOwnerId)
                .GreaterThan(0).WithMessage("PetOwner is required.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment is required.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items cannot be null.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.Description)
                    .NotEmpty().WithMessage("Item description is required.");

                items.RuleFor(i => i.UnitPrice)
                    .GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");

                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            });
        }
    }
}
