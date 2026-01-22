using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Procedures.Commands;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Application.Features.Procedures.Queries;
using PetCare.Application.Features.VetSpecializations.Queries;

namespace PetCare.WebApp.Pages.Procedures
{
    [Authorize(Roles = "Admin, Employee")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<ProcedureReadModel> Procedures { get; set; } = new();
        public SelectList? SpecializationOptions { get; set; }

        [BindProperty]
        public ProcedureCreateModel Input { get; set; } = new();

        [BindProperty]
        public int SelectedId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterSpecializationId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortColumn { get; set; } = "Name";

        [BindProperty(SupportsGet = true)]
        public string? SortDirection { get; set; } = "asc";

        public SelectList SpecializationFilterOptions { get; set; }

        public async Task OnGetAsync()
        {
            bool? isActive = FilterStatus switch
            {
                "active" => true,
                "inactive" => false,
                _ => null
            };

            Procedures = await _mediator.Send(new GetAllProceduresQuery
            {
                SearchTerm = SearchTerm,
                VetSpecializationId = FilterSpecializationId,
                IsActive = isActive,
                SortColumn = SortColumn,
                SortDirection = SortDirection
            });

            var specs = await _mediator.Send(new GetAllVetSpecializationsQuery());

            SpecializationOptions = new SelectList(specs, "VetSpecializationId", "Name");

            SpecializationFilterOptions = new SelectList(specs, "VetSpecializationId", "Name");
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!User.IsInRole("Admin")) return Forbid();

            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                ViewData["ShowCreateModal"] = true;
                return Page();
            }

            try
            {
                await _mediator.Send(new CreateProcedureCommand { Procedure = Input });
                TempData["SuccessMessage"] = "Procedure created successfully.";
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Unexpected error occurred.");
                await LoadDataAsync();
                ViewData["ShowCreateModal"] = true;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!User.IsInRole("Admin")) return Forbid();

            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                ViewData["ShowEditModal"] = true;
                return Page();
            }

            try
            {
                await _mediator.Send(new UpdateProcedureCommand(SelectedId, Input));
                TempData["SuccessMessage"] = "Procedure updated successfully.";
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Unexpected error occurred.");
                await LoadDataAsync();
                ViewData["ShowEditModal"] = true;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!User.IsInRole("Admin")) return Forbid();

            try
            {
                await _mediator.Send(new DeleteProcedureCommand(SelectedId));
                TempData["SuccessMessage"] = "Procedure deleted or deactivated successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadDataAsync();

                var procName = Procedures.FirstOrDefault(p => p.ProcedureId == SelectedId)?.Name;
                ViewData["DeleteName"] = procName;

                ViewData["ShowDeleteModal"] = true;
                return Page();
            }
        }

        private async Task LoadDataAsync()
        {
            Procedures = await _mediator.Send(new GetAllProceduresQuery());

            var specs = await _mediator.Send(new GetAllVetSpecializationsQuery());
            SpecializationOptions = new SelectList(specs, "VetSpecializationId", "Name");
            SpecializationFilterOptions = new SelectList(specs, "VetSpecializationId", "Name");
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