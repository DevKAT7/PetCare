using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Features.VetSchedules.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages
{
    [Authorize(Roles = "Admin, Employee")]
    public class ManageScheduleModel : PageModel
    {
        private readonly IMediator _mediator;

        public ManageScheduleModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public int VetId { get; set; }

        public List<VetScheduleReadModel> WeeklySchedule { get; set; } = new();
        public List<ScheduleExceptionReadModel> Exceptions { get; set; } = new();

        public string VetName { get; set; } = "";

        public VetScheduleCreateModel? NewSchedule { get; set; }
        public VetScheduleCreateModel UpdateSchedule { get; set; } = new();
        public ScheduleExceptionCreateModel? NewException { get; set; }
        public ScheduleExceptionCreateModel UpdateException { get; set; } = new();
        public List<SelectListItem>? WorkingDaysOptions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var vetDto = await _mediator.Send(new GetVetQuery { VetId = VetId });
                VetName = vetDto.FullName;
            }
            catch
            {
                VetName = $"Vet #{VetId}";
            }

            var scheduleQuery = new GetVetSchedulesByVetIdQuery { VetId = VetId };
            WeeklySchedule = await _mediator.Send(scheduleQuery);

            WeeklySchedule = WeeklySchedule
                .OrderBy(x => x.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)x.DayOfWeek)
                .ToList();

            var exceptionQuery = new GetScheduleExceptionsByVetIdQuery { VetId = VetId };
            Exceptions = await _mediator.Send(exceptionQuery);

            NewSchedule = new VetScheduleCreateModel { VetId = VetId };
            NewException = new ScheduleExceptionCreateModel { VetId = VetId, ExceptionDate = DateOnly.FromDateTime(DateTime.Now) };

            WorkingDaysOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Monday", Value = DayOfWeek.Monday.ToString() },
                new SelectListItem { Text = "Tuesday", Value = DayOfWeek.Tuesday.ToString() },
                new SelectListItem { Text = "Wednesday", Value = DayOfWeek.Wednesday.ToString() },
                new SelectListItem { Text = "Thursday", Value = DayOfWeek.Thursday.ToString() },
                new SelectListItem { Text = "Friday", Value = DayOfWeek.Friday.ToString() }
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAddScheduleAsync(
            [Bind(Prefix = nameof(NewSchedule))] VetScheduleCreateModel inputSchedule)
        {
            NewSchedule = inputSchedule;
            NewSchedule.VetId = VetId;

            ModelState.ClearValidationState(nameof(NewException));
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith(nameof(NewException))))
            {
                ModelState.Remove(key);
            }

            // Walidacja biznesowa (np. czy taki dzieñ ju¿ istnieje) powinna byæ w Handlerze lub tu
            // Tutaj prosty przyk³ad sprawdzenia duplikatów w pamiêci (opcjonalnie):
            /* var existing = await _mediator.Send(new GetVetSchedulesByVetIdQuery { VetId = VetId });
            if (existing.Any(x => x.DayOfWeek == NewSchedule.DayOfWeek))
            {
                ModelState.AddModelError("NewSchedule.DayOfWeek", "Ten dzieñ jest ju¿ zdefiniowany.");
            }
            */

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var command = new CreateVetScheduleCommand { Schedule = NewSchedule };
            await _mediator.Send(command);

            return RedirectToPage(new { vetId = VetId });
        }

        public async Task<IActionResult> OnPostAddExceptionAsync(
            [Bind(Prefix = nameof(NewException))] ScheduleExceptionCreateModel inputException)
        {
            NewException = inputException;
            NewException.VetId = VetId;

            ModelState.ClearValidationState(nameof(NewSchedule));
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith(nameof(NewSchedule))))
            {
                ModelState.Remove(key);
            }

            if (NewException.ExceptionDate == default)
            {
                ModelState.AddModelError("NewException.ExceptionDate", "Date is required.");
            }

            if (!NewException.IsFullDayAbsence && (NewException.StartTime == null || NewException.EndTime == null))
            {
                ModelState.AddModelError("NewException.StartTime", "Start and End time are required for partial absence.");
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                var command = new CreateScheduleExceptionCommand { Exception = NewException };
                await _mediator.Send(command);

                return RedirectToPage(new { vetId = VetId });
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var message in errorMessages)
                    {
                        string key = $"{nameof(NewException)}.{propertyName}";
                        ModelState.AddModelError(key, message);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating exception.");
            }

            ViewData["OpenModal"] = "addExceptionModal";

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateScheduleAsync(int id, 
            [Bind(Prefix = nameof(UpdateSchedule))] VetScheduleCreateModel inputSchedule)
        {
            inputSchedule.VetId = VetId;
            UpdateSchedule = inputSchedule;

            ModelState.ClearValidationState(nameof(NewSchedule));
            ModelState.ClearValidationState(nameof(NewException));

            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith(nameof(UpdateSchedule))))
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var command = new UpdateVetScheduleCommand(id, inputSchedule);
            await _mediator.Send(command);

            return RedirectToPage(new { vetId = VetId });
        }

        public async Task<IActionResult> OnPostDeleteScheduleAsync(int scheduleId)
        {
            await _mediator.Send(new DeleteVetScheduleCommand(scheduleId));

            return RedirectToPage(new { vetId = VetId });
        }

        public async Task<IActionResult> OnPostDeleteExceptionAsync(int exceptionId)
        {
            await _mediator.Send(new DeleteScheduleExceptionCommand(exceptionId));
            return RedirectToPage(new { vetId = VetId });
        }

        public async Task<IActionResult> OnPostUpdateExceptionAsync(int id,
            [Bind(Prefix = nameof(UpdateException))] ScheduleExceptionCreateModel inputException)
        {
            inputException.VetId = VetId;
            UpdateException = inputException;

            foreach (var key in ModelState.Keys.Where(k => !k.StartsWith(nameof(UpdateException))))
            {
                ModelState.Remove(key);
            }

            if (!UpdateException.IsFullDayAbsence && (UpdateException.StartTime == null || UpdateException.EndTime == null))
            {
                ModelState.AddModelError("UpdateException.StartTime", "Start and End time are required for partial absence.");
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                var command = new UpdateScheduleExceptionCommand(id, inputException);
                await _mediator.Send(command);

                return RedirectToPage(new { vetId = VetId });
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var message in errorMessages)
                    {
                        string key = $"{nameof(UpdateException)}.{propertyName}";
                        ModelState.AddModelError(key, message);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating exception.");
            }

            ViewData["OpenModal"] = "editExceptionModal";
            await OnGetAsync();
            return Page();
        }
    }
}