using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Appointments.Commands;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Features.Pets.Queries;
using PetCare.Application.Features.Vets.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Appointments
{
    [Authorize(Roles = "Admin, Employee")]
    public class CreateAppointmentModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateAppointmentModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public AppointmentCreateModel NewAppointment { get; set; } = new();

        public List<SelectListItem> VetOptions { get; set; } = new();
        public List<SelectListItem> PetOptions { get; set; } = new();

        public async Task<IActionResult> OnGetTimeSlotsAsync(int vetId, string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                return new JsonResult(new List<string>());
            }

            var query = new GetVetAvailabilityQuery { VetId = vetId, Date = parsedDate };
            var slots = await _mediator.Send(query);

            return new JsonResult(slots.Select(s => s.ToString(@"hh\:mm")).ToList());
        }

        public async Task<IActionResult> OnGetAsync()
        {
            NewAppointment.AppointmentDateTime = DateTime.Today.AddDays(1).AddHours(9);

            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            try
            {
                var command = new CreateAppointmentCommand { Appointment = NewAppointment };
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Appointment scheduled successfully!";
                return RedirectToPage("./Index");
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"NewAppointment.{error.Key}", error.Value.First());
                }
                await LoadDropdownsAsync();
                return Page();
            }
        }

        private async Task LoadDropdownsAsync()
        {
            var vets = await _mediator.Send(new GetAllVetsQuery());
            VetOptions = vets.Select(v => new SelectListItem
            {
                Value = v.VetId.ToString(),
                Text = v.FullName
            }).ToList();

            var pets = await _mediator.Send(new GetPetsForLookupQuery());
            PetOptions = pets.Select(p => new SelectListItem
            {
                Value = p.PetId.ToString(),
                Text = p.DisplayName
            }).ToList();
        }
    }
}
