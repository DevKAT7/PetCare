using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.PetOwners.Commands;
using PetCare.Application.Features.PetOwners.Dtos;
using PetCare.Application.Features.PetOwners.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetOwnersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PetOwnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPetOwners()
        {
            var query = new GetAllPetOwnersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PetOwnerReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PetOwnerReadModel>> GetPetOwner(int id)
        {
            var query = new GetPetOwnerQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePetOwner([FromBody] CreatePetOwnerCommand command)
        {
            var ownerId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPetOwner), new { id = ownerId }, ownerId);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> UpdatePetOwner(int id, [FromBody] PetOwnerUpdateModel request)
        {
            var command = new UpdatePetOwnerCommand(id, request);

            var ownerId = await _mediator.Send(command);

            return Ok(ownerId);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePetOwner(int id)
        {
            var command = new DeletePetOwnerCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
