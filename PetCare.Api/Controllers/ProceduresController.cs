using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Procedures.Commands;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Application.Features.Procedures.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProceduresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProceduresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? vetSpecializationId, [FromQuery] bool? isActive)
        {
            var query = new GetAllProceduresQuery(vetSpecializationId, isActive);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProcedureQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProcedureCreateModel model)
        {
            var command = new CreateProcedureCommand { Procedure = model };
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProcedureCreateModel model)
        {
            var command = new UpdateProcedureCommand(id, model);
            var updatedId = await _mediator.Send(command);
            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProcedureCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}