using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Vaccinations.Commands;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Application.Features.Vaccinations.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VaccinationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPet([FromQuery] int petId)
        {
            var query = new GetVaccinationsByPetIdQuery { PetId = petId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetVaccinationQuery { VaccinationId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVaccinationCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VaccinationUpdateModel request)
        {
            var command = new UpdateVaccinationCommand(id, request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteVaccinationCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
