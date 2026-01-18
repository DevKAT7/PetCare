using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Invoices.Dto;
using PetCare.Application.Features.Invoices.Queries;

namespace PetCare.WebApp.Pages.Invoices
{
    [Authorize(Roles = "Admin,Employee")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<InvoiceReadModel> Invoices { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; } = "InvoiceDate";

        [BindProperty(SupportsGet = true)]
        public string SortDirection { get; set; } = "desc";

        public SelectList StatusOptions { get; set; }

        public async Task OnGetAsync()
        {
            StatusOptions = new SelectList(new[]
            {
                new { Value = "Paid", Text = "Paid" },
                new { Value = "Unpaid", Text = "Unpaid" },
                new { Value = "Overdue", Text = "Overdue" }
            }, "Value", "Text");

            Invoices = await _mediator.Send(new GetAllInvoicesQuery
            {
                SearchTerm = SearchTerm,
                FilterStatus = FilterStatus,
                SortColumn = SortColumn,
                SortDirection = SortDirection
            });
        }
    }
}
