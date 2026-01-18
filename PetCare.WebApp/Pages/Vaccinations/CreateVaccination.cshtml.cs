using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Vaccinations.Commands;
using PetCare.Application.Features.Vaccinations.Dtos;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Vaccinations
{
    [Authorize(Roles = "Admin,Employee")]
    public class CreateVaccinationModel : PageModel
    {
        private readonly IMediator _mediator;
        public CreateVaccinationModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public VaccinationCreateModel Input { get; set; } = new();

        public void OnGet(int appointmentId, int petId)
        {
            Input.AppointmentId = appointmentId;
            Input.PetId = petId;
            Input.VaccinationDate = DateOnly.FromDateTime(DateTime.Today);

            Input.NextDueDate = DateOnly.FromDateTime(DateTime.Today.AddYears(1));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var command = new CreateVaccinationCommand { Vaccination = Input };
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Vaccination added successfully!";
                return RedirectToPage("/Appointments/Details", new { id = Input.AppointmentId });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"NewAppointment.{error.Key}", error.Value.First());
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
