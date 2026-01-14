using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.MedicalTests.Commands;
using PetCare.Application.Features.MedicalTests.Dtos;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.MedicalTests
{
    public class CreateMedicalTestModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateMedicalTestModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public MedicalTestCreateModel Input { get; set; } = new();

        public void OnGet(int appointmentId, int petId)
        {
            Input.AppointmentId = appointmentId;
            Input.PetId = petId;
            Input.TestDate = DateOnly.FromDateTime(DateTime.Today);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var command = new CreateMedicalTestCommand { MedicalTest = Input };
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Medical test ordered successfully!";
                return RedirectToPage("/Appointments/Details", new { id = Input.AppointmentId }); ;
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"Input.{error.Key}", error.Value.First());
                }
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating medical test: {ex.Message}");
                return Page();
            }
        }
    }
}
