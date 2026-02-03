using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.MedicalTests.Commands;
using PetCare.Application.Features.MedicalTests.Dtos;
using PetCare.Application.Features.MedicalTests.Queries;
using PetCare.Application.Interfaces;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.MedicalTests
{
    [Authorize(Roles = "Admin, Employee")]
    public class EditMedicalTestModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorage;

        public EditMedicalTestModel(IMediator mediator, IFileStorageService fileStorageService)
        {
            _mediator = mediator;
            _fileStorage = fileStorageService;
        }

        [BindProperty]
        public MedicalTestUpdateModel Input { get; set; } = new();

        [BindProperty]
        public int MedicalTestId { get; set; }

        [BindProperty]
        public int AppointmentId { get; set; }

        public DateOnly TestDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var test = await _mediator.Send(new GetMedicalTestQuery { MedicalTestId = id });

                MedicalTestId = test.MedicalTestId;
                AppointmentId = test.AppointmentId;
                TestDate = test.TestDate;

                Input = new MedicalTestUpdateModel
                {
                    TestName = test.TestName,
                    Result = test.Result,
                    AttachmentUrl = test.AttachmentUrl
                };

                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (Input.AttachmentFile != null && Input.AttachmentFile.Length > 0)
                {
                    Input.AttachmentUrl = await _fileStorage.SaveFileAsync(Input.AttachmentFile, "medicaltests");
                }

                var command = new UpdateMedicalTestCommand(MedicalTestId, Input);
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Medical test updated successfully.";

                return RedirectToPage("/Appointments/Details", new { id = AppointmentId });
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var errorMessage in errorMessages)
                    {
                        ModelState.AddModelError($"Input.{propertyName}", errorMessage);
                    }
                }
            }
            catch (Exception) 
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the medical test.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _mediator.Send(new DeleteMedicalTestCommand(MedicalTestId));

            TempData["SuccessMessage"] = "Medical test deleted.";

            return RedirectToPage("/Appointments/Details", new { id = AppointmentId });
        }
    }
}
