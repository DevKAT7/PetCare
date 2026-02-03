using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.MedicalTests.Queries;
using PetCare.Application.Interfaces;

namespace PetCare.WebApp.Pages
{
    public class DownloadFileModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorage;

        public DownloadFileModel(IMediator mediator, IFileStorageService fileStorage)
        {
            _mediator = mediator;
            _fileStorage = fileStorage;
        }

        public async Task<IActionResult> OnGetAsync(int fileId)
        {
            var test = await _mediator.Send(new GetMedicalTestQuery { MedicalTestId = fileId });

            if (test == null || string.IsNullOrEmpty(test.AttachmentUrl))
            {
                return NotFound();
            }

            var fileBytes = await _fileStorage.GetFileAsync(test.AttachmentUrl, "medicaltests");

            if (fileBytes == null)
            {
                return NotFound();
            }

            var contentType = "application/pdf";
            var ext = Path.GetExtension(test.AttachmentUrl).ToLower();
            if (ext == ".jpg" || ext == ".jpeg") contentType = "image/jpeg";
            else if (ext == ".png") contentType = "image/png";

            return File(fileBytes, contentType, test.AttachmentUrl);
        }
    }
}
