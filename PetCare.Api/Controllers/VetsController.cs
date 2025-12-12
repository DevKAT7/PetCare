using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVets()
        {
            var query = new GetAllVetsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVet([FromBody] CreateVetCommand command)
        {
            var vetId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllVets), new { id = vetId }, vetId);
        }
    }
}
