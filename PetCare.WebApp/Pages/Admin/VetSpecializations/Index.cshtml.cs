using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSpecializations.Commands;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Application.Features.VetSpecializations.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Admin.VetSpecializations
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<VetSpecializationReadModel> Specializations { get; set; } = new();

        [BindProperty]
        public VetSpecializationCreateModel Input { get; set; } = new();

        [BindProperty]
        public int SelectedId { get; set; }

        public async Task OnGetAsync()
        {
            Specializations = await _mediator.Send(new GetAllVetSpecializationsQuery());
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
                await _mediator.Send(new CreateVetSpecializationCommand { Specialization = Input });

                TempData["SuccessMessage"] = "Specialization created successfully.";
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
                ModelState.AddModelError("", "An unexpected error occurred while creating.");
                await LoadDataAsync();
                ViewData["ShowCreateModal"] = true;
                return Page();
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
                await _mediator.Send(new UpdateVetSpecializationCommand(SelectedId, Input));

                TempData["SuccessMessage"] = "Specialization updated successfully.";
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
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred while updating.");
                await LoadDataAsync();
                ViewData["ShowEditModal"] = true;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                await _mediator.Send(new DeleteVetSpecializationCommand(SelectedId));
                TempData["SuccessMessage"] = "Specialization deleted successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadDataAsync();

                var specName = Specializations.FirstOrDefault(s => s.VetSpecializationId == SelectedId)?.Name;
                ViewData["DeleteName"] = specName;

                ViewData["ShowDeleteModal"] = true;
                return Page();
            }
        }

        private async Task LoadDataAsync()
        {
            Specializations = await _mediator.Send(new GetAllVetSpecializationsQuery());
        }
    }
}