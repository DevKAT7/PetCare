using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Prescriptions.Commands;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Features.Prescriptions.Queries;
using PetCare.Application.Interfaces;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDocumentGenerator _documentGenerator;

        public PrescriptionsController(IMediator mediator, IDocumentGenerator documentGenerator)
        {
            _mediator = mediator;
            _documentGenerator = documentGenerator;
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

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(int id)
        {
            var query = new GetPrescriptionQuery { Id = id };
            var prescription = await _mediator.Send(query);

            if (prescription == null) return NotFound();

            var document = _documentGenerator.GeneratePrescription(prescription, "standard_pdf");

            return File(document.Content, "application/pdf", document.FileName);
        }
    }
}
