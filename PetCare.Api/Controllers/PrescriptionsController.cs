using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Prescriptions.Commands;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Features.Prescriptions.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetByAppointment([FromQuery] int appointmentId)
        {
            var query = new GetPrescriptionsByAppointmentIdQuery { AppointmentId = appointmentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetPrescriptionQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PrescriptionCreateModel request)
        {
            var command = new UpdatePrescriptionCommand(id, request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeletePrescriptionCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
