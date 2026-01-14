using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Pets.Dtos;
using PetCare.Application.Features.Pets.Queries;

namespace PetCare.WebApp.Pages.Pets
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public DetailsModel(IMediator mediator) => _mediator = mediator;

        public PetDetailDto Pet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Pet = await _mediator.Send(new GetPetDetailQuery { PetId = id });
                return Page();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
