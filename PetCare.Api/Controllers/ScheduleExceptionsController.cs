using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Features.VetSchedules.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleExceptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScheduleExceptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetByVet([FromQuery] int vetId)
        {
            var query = new GetScheduleExceptionsByVetIdQuery { VetId = vetId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetScheduleExceptionQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleExceptionCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleExceptionCreateModel request)
        {
            var command = new UpdateScheduleExceptionCommand(id, request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteScheduleExceptionCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
