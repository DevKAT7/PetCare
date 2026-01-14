using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Medications.Commands;
using PetCare.Application.Features.Medications.Dtos;
using PetCare.Application.Features.Medications.Queries;

namespace PetCare.WebApp.Pages.Medications
{
    [Authorize(Roles = "Admin,Employee")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator) => _mediator = mediator;

        public List<MedicationReadModel> Medications { get; set; } = new();

        [BindProperty]
        public MedicationCreateModel Input { get; set; } = new();

        [BindProperty]
        public int SelectedId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortColumn { get; set; } = "Name";

        [BindProperty(SupportsGet = true)]
        public string? SortDirection { get; set; } = "asc";

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid) 
            { 
                await LoadDataAsync(); 
                ViewData["ShowCreateModal"] = true; 
                return Page(); 
            }

            try
            {
                await _mediator.Send(new CreateMedicationCommand { Medication = Input });
                TempData["SuccessMessage"] = "Medication added successfully.";
                return RedirectToPage();
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"Input.{error.Key}", error.Value.First());
                }
                await LoadDataAsync();
                ViewData["ShowCreateModal"] = true;
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadDataAsync(); ViewData["ShowCreateModal"] = true; return Page();
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid) 
            { 
                await LoadDataAsync(); 
                ViewData["ShowEditModal"] = true; 
                return Page(); 
            }

            try
            {
                await _mediator.Send(new UpdateMedicationCommand(SelectedId, Input));
                TempData["SuccessMessage"] = "Medication updated successfully.";
                return RedirectToPage();
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"Input.{error.Key}", error.Value.First());
                }
                await LoadDataAsync();
                ViewData["ShowEditModal"] = true;
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadDataAsync(); ViewData["ShowEditModal"] = true; return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                await _mediator.Send(new DeleteMedicationCommand(SelectedId));
                TempData["SuccessMessage"] = "Medication deleted or archived successfully.";
                return RedirectToPage();
            }
            catch (InvalidOperationException ex)
            {
                TempData["WarningMessage"] = ex.Message;
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadDataAsync();
                var name = Medications.FirstOrDefault(m => m.MedicationId == SelectedId)?.Name;
                ViewData["DeleteName"] = name;
                ViewData["ShowDeleteModal"] = true;
                return Page();
            }
        }

        private async Task LoadDataAsync()
        {
            bool? isActive = FilterStatus switch { "active" => true, "inactive" => false, _ => null };

            Medications = await _mediator.Send(new GetMedicationsQuery
            {
                SearchTerm = SearchTerm,
                IsActive = isActive,
                SortColumn = SortColumn,
                SortDirection = SortDirection
            });
        }

        public string GetNextSort(string column)
        {
            if (SortColumn == column)
                return SortDirection == "asc" ? "desc" : "asc";
            return "asc";
        }

        public string GetSortIcon(string column)
        {
            if (SortColumn != column) return "bi-arrow-down-up text-muted opacity-25";
            return SortDirection == "asc" ? "bi-arrow-up-short" : "bi-arrow-down-short";
        }
    }
}
