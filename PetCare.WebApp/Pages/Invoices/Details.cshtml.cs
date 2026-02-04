using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Invoices.Commands;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Features.Invoices.Queries;
using PetCare.WebApp.Pages.Shared;

namespace PetCare.WebApp.Pages.Invoices
{
    [Authorize(Roles = "Admin, Employee")]
    public class DetailsModel : BasePageModel
    {

        public DetailsModel(IMediator mediator) : base(mediator)
        {
        }

        public InvoiceReadModel Invoice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                await LoadPageTextsAsync();
                Invoice = await _mediator.Send(new GetInvoiceQuery(id));
                return Page();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostMarkAsPaidAsync(int id)
        {
            try
            {
                await _mediator.Send(new MarkInvoicePaidCommand(id, DateTime.Now));

                TempData["SuccessMessage"] = "Invoice marked as PAID.";
                return RedirectToPage(new { id });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
