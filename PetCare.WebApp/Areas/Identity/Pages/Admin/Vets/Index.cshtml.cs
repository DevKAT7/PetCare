using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Application.Features.Vets.Queries;

namespace PetCare.WebApp.Areas.Identity.Pages.Admin.Vets
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<VetReadModel> Vets { get; set; } = new();

        public async Task OnGetAsync()
        {
            var query = new GetAllVetsQuery();
            Vets = await _mediator.Send(query);
        }
    }
}
