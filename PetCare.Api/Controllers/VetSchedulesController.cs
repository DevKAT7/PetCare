using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dtos;
using PetCare.Application.Features.VetSchedules.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetSchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VetSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("vet/{vetId}")]
        public async Task<IActionResult> GetByVet(int vetId)
        {
            var query = new GetVetSchedulesByVetIdQuery { VetId = vetId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetVetScheduleQuery { VetScheduleId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVetScheduleCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VetScheduleCreateModel request)
        {
            var command = new UpdateVetScheduleCommand(id, request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteVetScheduleCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
