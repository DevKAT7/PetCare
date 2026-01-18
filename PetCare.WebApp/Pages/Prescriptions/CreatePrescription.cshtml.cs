using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Medications.Queries;
using PetCare.Application.Features.Prescriptions.Commands;
using PetCare.Application.Features.Prescriptions.Dtos;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Prescriptions
{
    [Authorize(Roles = "Employee")]
    public class CreatePrescriptionModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreatePrescriptionModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public PrescriptionCreateModel Input { get; set; } = new();

        public SelectList MedicationOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int appointmentId)
        {
            Input.AppointmentId = appointmentId;
            Input.StartDate = DateOnly.FromDateTime(DateTime.Today);
            Input.EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));

            await LoadOptions();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadOptions();
                return Page();
            }

            try
            {
                var prescriptionId = await _mediator.Send(new CreatePrescriptionCommand { Prescription = Input });

                TempData["SuccessMessage"] = "Prescription issued successfully.";

                return RedirectToPage("/Appointments/Details", new { id = Input.AppointmentId });
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

                await LoadOptions();
                return Page();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadOptions();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
                await LoadOptions();
                return Page();
            }
        }

        private async Task LoadOptions()
        {
            var medicationsList = await _mediator.Send(new GetMedicationsQuery());

            var medItems = medicationsList.Select(m => new
            {
                Id = m.MedicationId,
                DisplayName = m.CurrentStock > 0
                    ? $"{m.Name} - {m.Price:C} (Stock: {m.CurrentStock})"
                    : $"{m.Name} - {m.Price:C} [OUT OF STOCK]"
            });

            MedicationOptions = new SelectList(medItems, "Id", "DisplayName");
        }
    }
}

