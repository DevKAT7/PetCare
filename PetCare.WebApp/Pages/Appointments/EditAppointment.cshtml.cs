using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Appointments.Commands;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Features.Procedures.Queries;
using PetCare.Core.Enums;

namespace PetCare.WebApp.Pages.Appointments
{
    public class EditAppointmentModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditAppointmentModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public AppointmentUpdateModel UpdateModel { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Source { get; set; }

        [BindProperty]
        public AppointmentProcedureCreateModel NewProcedure { get; set; } = new();

        public int Id { get; set; }
        public string VetName { get; set; } = "";
        public string PetName { get; set; } = "";
        public string OwnerName { get; set; } = "";

        public SelectList? StatusOptions { get; set; }
        public SelectList? ProcedureOptions { get; set; }
        public List<AppointmentProcedureReadModel> ExistingProcedures { get; set; } = new();
        
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Id = id;

            var appointment = await _mediator.Send(new GetAppointmentQuery(id));

            if (appointment == null) return NotFound();

            UpdateModel = new AppointmentUpdateModel
            {
                AppointmentDateTime = appointment.AppointmentDateTime,
                PetId = appointment.PetId,
                VetId = appointment.VetId,
                ReasonForVisit = appointment.ReasonForVisit,
                Description = appointment.Description,
                Diagnosis = appointment.Diagnosis,
                Notes = appointment.Notes,
                Status = appointment.Status
            };

            ExistingProcedures = appointment.Procedures ?? new List<AppointmentProcedureReadModel>();

            var allProcedures = await _mediator.Send(new GetAllProceduresQuery(isActive: true));

            var procedureItems = allProcedures.Select(p => new
            {
                ProcedureId = p.ProcedureId,
                DisplayText = $"{p.Name} ({p.Price:C})"
            });

            ProcedureOptions = new SelectList(procedureItems, "ProcedureId", "DisplayText");

            await LoadDataForView(id, appointment.Status);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Id = id;

            ClearNewProcedureErrors();

            if (!ModelState.IsValid)
            {
                await LoadDataForView(id, UpdateModel.Status);
                return Page();
            }

            try
            {
                var command = new UpdateAppointmentCommand(id, UpdateModel);
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Visit details updated successfully.";
                return GetRedirectResult();
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var errorMessage in errorMessages)
                    {
                        ModelState.AddModelError($"UpdateModel.{propertyName}", errorMessage);
                    }
                }
            }
            catch (BadRequestException ex)
            {
                foreach (var error in ex.ValidationErrors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error updating appointment.");

            }

            await LoadDataForView(id, UpdateModel.Status);
            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            try
            {
                await _mediator.Send(new CancelAppointmentCommand(id));
                TempData["SuccessMessage"] = "Appointment has been cancelled.";
                return GetRedirectResult();
            }
            catch (Exception)
            {
                await LoadDataForView(id, AppointmentStatus.Cancelled);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddProcedureAsync(int id)
        {
            Id = id;

/*            if (NewProcedure.ProcedureId <= 0)
            {
                ModelState.AddModelError("NewProcedure.ProcedureId", "Please select a procedure.");
                await LoadDataForView(id, UpdateModel.Status);
                return await OnGetAsync(id);
            }*/

            try
            {
                var updateCommand = new UpdateAppointmentCommand(id, UpdateModel);
                await _mediator.Send(updateCommand);

                var addCommand = new AddProcedureToAppointmentCommand
                {
                    AppointmentId = id,
                    Model = NewProcedure
                };

                await _mediator.Send(addCommand);
                TempData["SuccessMessage"] = "Procedure added successfully.";
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    foreach (var err in entry.Value)
                    {
                        if (entry.Key.StartsWith("Quantity") || entry.Key.StartsWith("FinalPrice"))
                        {
                            ModelState.AddModelError($"NewProcedure.{entry.Key}", err);
                        }
                        else
                        {
                            ModelState.AddModelError($"UpdateModel.{entry.Key}", err);
                        }
                    }
                }
                await LoadDataForView(id, UpdateModel.Status);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error adding procedure: " + ex.Message);
                await LoadDataForView(id, UpdateModel.Status);
                return Page();
            }

            return RedirectToPage(new { Id = id, source = Source });
        }

        public async Task<IActionResult> OnPostRemoveProcedureAsync(int id, int procedureId)
        {
            Id = id;

            ClearNewProcedureErrors();

            try
            {
                var updateCommand = new UpdateAppointmentCommand(id, UpdateModel);
                await _mediator.Send(updateCommand);

                var removeCommand = new RemoveProcedureFromAppointmentCommand
                {
                    AppointmentId = id,
                    ProcedureId = procedureId
                };

                await _mediator.Send(removeCommand);
                TempData["SuccessMessage"] = "Procedure removed.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Could not remove procedure.";
            }

            return RedirectToPage(new { Id = id, source = Source });
        }

        private IActionResult GetRedirectResult()
        {
            if (Source == "calendar")
            {
                return RedirectToPage("/Calendar/Calendar");
            }

            return RedirectToPage("./Index");
        }

        private async Task LoadDataForView(int appointmentId, AppointmentStatus currentStatus)
        {
            var appointment = await _mediator.Send(new GetAppointmentQuery(appointmentId));

            if (appointment != null)
            {
                VetName = appointment.VetName;
                PetName = appointment.PetName;
                OwnerName = appointment.OwnerName;
            }

            var allowedStatuses = new List<AppointmentStatus> { currentStatus };

            if (currentStatus != AppointmentStatus.Completed && currentStatus != AppointmentStatus.Cancelled)
            {
                allowedStatuses.Add(AppointmentStatus.Completed);
                allowedStatuses.Add(AppointmentStatus.NoShow);
            }

            StatusOptions = new SelectList(allowedStatuses.Distinct());

            var allProcedures = await _mediator.Send(new GetAllProceduresQuery(isActive: true));
            var procedureItems = allProcedures.Select(p => new
            {
                ProcedureId = p.ProcedureId,
                DisplayText = $"{p.Name} ({p.Price:C})"
            });

            ProcedureOptions = new SelectList(procedureItems, "ProcedureId", "DisplayText");
        }

        private void ClearNewProcedureErrors()
        {
            var keys = ModelState.Keys.Where(k => k.StartsWith("NewProcedure")).ToList();
            foreach (var key in keys)
            {
                ModelState.Remove(key);
            }
        }
    }
}
