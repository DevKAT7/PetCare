using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Dashboard.Dtos;
using PetCare.Application.Features.Dashboard.Queries;

namespace PetCare.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public DashboardDto Data { get; set; } = new();

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new GetDashboardDataQuery());
        }
    }
}
