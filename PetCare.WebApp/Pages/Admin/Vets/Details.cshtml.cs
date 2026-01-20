using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Application.Features.Vets.Queries;

namespace PetCare.WebApp.Pages.Admin.Vets
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public DetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public VetReadModel Vet { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var vet = await _mediator.Send(new GetVetQuery { VetId = id });

            if (vet == null)
            {
                return NotFound();
            }

            Vet = vet;
            return Page();
        }
    }
}