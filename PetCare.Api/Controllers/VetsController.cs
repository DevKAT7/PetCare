using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Dtos;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VetReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VetReadModel>> GetVet(int id)
        {
            var query = new GetVetQuery { VetId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVet([FromBody] CreateVetCommand command)
        {
            var vetId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllVets), new { id = vetId }, vetId);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> UpdateVet(int id, [FromBody] VetUpdateModel request)
        {
            var command = new UpdateVetCommand(id, request);

            var vetId = await _mediator.Send(command);

            return Ok(vetId);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVet(int id)
        {
            var command = new DeleteVetCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
