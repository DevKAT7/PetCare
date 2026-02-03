using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Pets.Commands;
using PetCare.Application.Features.Pets.Dtos;
using PetCare.Application.Features.Pets.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPets([FromQuery] int? ownerId)
        {
            var query = new GetAllPetsByOwnerQuery(ownerId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDetailDto>> GetPet(int id)
        {
            var query = new GetPetDetailQuery { PetId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePet([FromBody] PetCreateModel model)
        {
            var command = new CreatePetCommand { Pet = model };

            var petId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetPet), new { id = petId }, petId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdatePet(int id, [FromBody] PetUpdateModel request)
        {
            var command = new UpdatePetCommand(id, request);
            var petId = await _mediator.Send(command);
            return Ok(petId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var command = new DeletePetCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
