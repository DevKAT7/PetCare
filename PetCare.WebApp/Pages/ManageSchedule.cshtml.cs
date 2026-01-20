using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Application.Features.VetSchedules.Commands;
using PetCare.Application.Features.VetSchedules.Dtos;
using PetCare.Application.Features.VetSchedules.Queries;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages
{
    [Authorize(Roles = "Admin, Employee")]
    public class ManageScheduleModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ManageScheduleModel(IMediator mediator, ApplicationDbContext context,
                UserManager<User> userManager)
        {
            _mediator = mediator;
            _context = context;
            _userManager = userManager;
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
            if (!await IsAuthorizedToManageExceptionsAsync())
            {
                return Forbid();
            }

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
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            NewSchedule = inputSchedule;
            NewSchedule.VetId = VetId;

            ModelState.ClearValidationState(nameof(NewException));
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith(nameof(NewException))))
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                ViewData["OpenModal"] = "addScheduleModal";
                await OnGetAsync();
                return Page();
            }

            try
            {
                var command = new CreateVetScheduleCommand { Schedule = NewSchedule };
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
                        string key = $"{nameof(NewSchedule)}.{propertyName}";
                        ModelState.AddModelError(key, message);
                    }
                }

                ViewData["OpenModal"] = "addScheduleModal";
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding schedule.");
                ViewData["OpenModal"] = "addScheduleModal";
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddExceptionAsync(
            [Bind(Prefix = nameof(NewException))] ScheduleExceptionCreateModel inputException)
        {
            if (!await IsAuthorizedToManageExceptionsAsync())
            {
                return Forbid();
            }

            NewException = inputException;
            NewException.VetId = VetId;

            ModelState.ClearValidationState(nameof(NewSchedule));
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith(nameof(NewSchedule))))
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                ViewData["OpenModal"] = "addExceptionModal";
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

                ViewData["OpenModal"] = "addExceptionModal";
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding exception.");
                ViewData["OpenModal"] = "addExceptionModal";
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateScheduleAsync(int id,
            [Bind(Prefix = nameof(UpdateSchedule))] VetScheduleCreateModel inputSchedule)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

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
                ViewData["OpenModal"] = "editScheduleModal";
                ViewData["EditScheduleId"] = id;
                await OnGetAsync();
                return Page();
            }

            try
            {
                var command = new UpdateVetScheduleCommand(id, inputSchedule);
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
                        string key = $"{nameof(UpdateSchedule)}.{propertyName}";
                        ModelState.AddModelError(key, message);
                    }
                }

                ViewData["OpenModal"] = "editScheduleModal";
                ViewData["EditScheduleId"] = id;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while editing schedule.");
                ViewData["OpenModal"] = "editScheduleModal";
                ViewData["EditScheduleId"] = id;
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateExceptionAsync(int id,
            [Bind(Prefix = nameof(UpdateException))] ScheduleExceptionCreateModel inputException)
        {
            if (!await IsAuthorizedToManageExceptionsAsync())
            {
                return Forbid();
            }

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
                ViewData["OpenModal"] = "editExceptionModal";
                ViewData["EditExceptionId"] = id;
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

                ViewData["OpenModal"] = "editExceptionModal";
                ViewData["EditExceptionId"] = id;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating exception.");
                ViewData["OpenModal"] = "editExceptionModal";
                ViewData["EditExceptionId"] = id;
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteScheduleAsync(int scheduleId)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            await _mediator.Send(new DeleteVetScheduleCommand(scheduleId));

            return RedirectToPage(new { vetId = VetId });
        }

        public async Task<IActionResult> OnPostDeleteExceptionAsync(int exceptionId)
        {
            if (!await IsAuthorizedToManageExceptionsAsync())
            {
                return Forbid();
            }

            await _mediator.Send(new DeleteScheduleExceptionCommand(exceptionId));
            return RedirectToPage(new { vetId = VetId });
        }

        private async Task<bool> IsAuthorizedToManageExceptionsAsync()
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }

            if (User.IsInRole("Employee"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return false;

                var vet = await _context.Vets.FirstOrDefaultAsync(v => v.UserId == user.Id);

                if (vet == null || vet.VetId != VetId)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public async Task<IActionResult> OnPostApproveExceptionAsync(int exceptionId)
        {
            if (!User.IsInRole("Admin")) return Forbid();
            await _mediator.Send(new ApproveScheduleExceptionCommand { ExceptionId = exceptionId });
            return RedirectToPage(new { vetId = VetId });
        }

        public async Task<IActionResult> OnPostRejectExceptionAsync(int exceptionId)
        {
            if (!User.IsInRole("Admin")) return Forbid();
            await _mediator.Send(new RejectScheduleExceptionCommand { ExceptionId = exceptionId });
            return RedirectToPage(new { vetId = VetId });
        }
    }
}