using FluentValidation;
using PetCare.Application.Features.Invoices.Commands;
using PetCare.Application.Features.Invoices.Dto;

namespace PetCare.Application.Features.Invoices.Validators
{
    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator(IValidator<InvoiceCreateModel> invoiceModelValidator)
        {
            RuleFor(x => x.Invoice).SetValidator(invoiceModelValidator);
        }
    }
}
