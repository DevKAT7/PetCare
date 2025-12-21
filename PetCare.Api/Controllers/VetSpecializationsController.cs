using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.VetSpecializations.Commands;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Application.Features.VetSpecializations.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetSpecializationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VetSpecializationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllVetSpecializationsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetVetSpecializationQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VetSpecializationCreateModel model)
        {
            var command = new CreateVetSpecializationCommand { Specialization = model };
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VetSpecializationCreateModel model)
        {
            var command = new UpdateVetSpecializationCommand(id, model);
            var updatedId = await _mediator.Send(command);
            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteVetSpecializationCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
