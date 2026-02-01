using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.MedicalTests.Commands;
using PetCare.Application.Features.MedicalTests.Dtos;
using PetCare.Application.Features.MedicalTests.Queries;
using PetCare.Application.Interfaces;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalTestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorage;

        public MedicalTestsController(IMediator mediator, IFileStorageService fileStorage)
        {
            _mediator = mediator;
            _fileStorage = fileStorage;
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
        public async Task<IActionResult> Update(int id, [FromBody] MedicalTestUpdateModel request)
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

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadResult(int id)
        {
            var test = await _mediator.Send(new GetMedicalTestQuery { MedicalTestId = id });

            if (test == null || string.IsNullOrEmpty(test.AttachmentUrl))
                return NotFound();

            var fileBytes = await _fileStorage.GetFileAsync(test.AttachmentUrl, "medicaltests");

            if (fileBytes == null) return NotFound("File missing on server.");

            var contentType = "application/pdf";
            if (test.AttachmentUrl.EndsWith(".jpg")) contentType = "image/jpeg";

            return File(fileBytes, contentType, test.AttachmentUrl);
        }
    }
}
