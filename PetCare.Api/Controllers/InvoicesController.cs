using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Invoices.Commands;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Features.Invoices.Queries;
using PetCare.Application.Interfaces;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDocumentGenerator _documentGenerator;

        public InvoicesController(IMediator mediator, IDocumentGenerator documentGenerator)
        {
            _mediator = mediator;
            _documentGenerator = documentGenerator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceReadModel>> Get(int id)
        {
            var query = new GetInvoiceQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(int ownerId, [FromQuery] string? status = null)
        {
            var query = new GetInvoicesByOwnerQuery(ownerId, status);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPost("{id}/pay")]
        public async Task<IActionResult> MarkPaid(int id, [FromBody] DateTime paymentDate)
        {
            var command = new MarkInvoicePaidCommand(id, paymentDate);
            var invoiceId = await _mediator.Send(command);
            return Ok(invoiceId);
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(int id)
        {
            var invoiceQuery = new GetInvoiceQuery(id);
            var invoice = await _mediator.Send(invoiceQuery);

            if (invoice == null) return NotFound();

            var document = _documentGenerator.GenerateInvoice(invoice);

            return File(document.Content, "application/pdf", document.FileName);
        }
    }
}
