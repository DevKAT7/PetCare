using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.MedicalTests.Commands;
using PetCare.Application.Features.MedicalTests.Dto;
using PetCare.Application.Features.MedicalTests.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalTestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalTestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPet([FromQuery] int petId)
        {
            var query = new GetMedicalTestsByPetIdQuery { PetId = petId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetMedicalTestQuery { MedicalTestId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicalTestCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicalTestCreateModel request)
        {
            var command = new UpdateMedicalTestCommand(id, request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteMedicalTestCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
