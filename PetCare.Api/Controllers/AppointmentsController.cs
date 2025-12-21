using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Appointments.Commands;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Application.Features.Appointments.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? ownerId, [FromQuery] int? vetId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? status)
        {
            Core.Enums.AppointmentStatus? parsedStatus = null;
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Core.Enums.AppointmentStatus>(status, true, out var s)) parsedStatus = s;

            var query = new GetAllAppointmentsQuery(ownerId, vetId, from, to, parsedStatus);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentReadModel>> Get(int id)
        {
            var query = new GetAppointmentQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] AppointmentUpdateModel request)
        {
            var command = new UpdateAppointmentCommand(id, request);
            var appointmentId = await _mediator.Send(command);
            return Ok(appointmentId);
        }

        [HttpGet("{id}/procedures")]
        public async Task<IActionResult> GetProcedures(int id)
        {
            var query = new GetAppointmentProceduresQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/procedures")]
        public async Task<IActionResult> AddProcedure(int id, [FromBody] AppointmentProcedureCreateModel model)
        {
            var command = new AddProcedureToAppointmentCommand { AppointmentId = id, Model = model };
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPut("{id}/procedures/{procedureId}")]
        public async Task<IActionResult> UpdateProcedure(int id, int procedureId, [FromBody] AppointmentProcedureCreateModel model)
        {
            var command = new UpdateProcedureInAppointmentCommand { AppointmentId = id, ProcedureId = procedureId, Model = model };
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpDelete("{id}/procedures/{procedureId}")]
        public async Task<IActionResult> RemoveProcedure(int id, int procedureId)
        {
            var command = new RemoveProcedureFromAppointmentCommand { AppointmentId = id, ProcedureId = procedureId };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var command = new CancelAppointmentCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
