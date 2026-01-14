using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Vaccinations.Commands;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Application.Features.Vaccinations.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Vaccinations
{
    public class EditVaccinationModel : PageModel
    {
        private readonly IMediator _mediator;
        public EditVaccinationModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public VaccinationUpdateModel Input { get; set; } = new();

        [BindProperty]
        public int VaccinationId { get; set; }

        [BindProperty]
        public int AppointmentId { get; set; }

        public DateOnly AdministeredDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var vac = await _mediator.Send(new GetVaccinationQuery { VaccinationId = id });
                VaccinationId = vac.VaccinationId;
                AppointmentId = vac.AppointmentId;
                AdministeredDate = vac.VaccinationDate;

                Input = new VaccinationUpdateModel
                {
                    VaccineName = vac.VaccineName,
                    NextDueDate = vac.NextDueDate
                };
                return Page();
            }
            catch { return NotFound(); }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var command = new UpdateVaccinationCommand(VaccinationId, Input);
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Vaccination updated successfully.";

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
                ModelState.AddModelError(string.Empty, "An error occurred while updating the vaccination.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _mediator.Send(new DeleteVaccinationCommand(VaccinationId));

            TempData["SuccessMessage"] = "Vaccination deleted.";

            return RedirectToPage("/Appointments/Details", new { id = AppointmentId });
        }
    }
}
