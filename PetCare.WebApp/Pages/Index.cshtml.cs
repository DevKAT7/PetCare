using MediatR;
using Microsoft.AspNetCore.Authorization;
using PetCare.Application.Features.Dashboard.Dtos;
using PetCare.Application.Features.Dashboard.Queries;
using PetCare.WebApp.Pages.Shared;

namespace PetCare.WebApp.Pages
{
    [Authorize(Roles = "Admin,Employee")]
    public class IndexModel : BasePageModel
    {
        public IndexModel(IMediator mediator) : base(mediator)
        {
        }

        public DashboardDto Data { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadPageTextsAsync();
            Data = await _mediator.Send(new GetDashboardDataQuery());
        }
    }
}
