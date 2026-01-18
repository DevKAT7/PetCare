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
        }
    }
}
