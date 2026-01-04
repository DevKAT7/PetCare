using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Application.Features.VetSpecializations.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Admin.Vets
{
    [Authorize(Roles = "Admin")]
    public class EditVetModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditVetModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public VetUpdateModel Input { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadSpec();

            var query = new GetVetForEditQuery { VetId = Id };
            Input = await _mediator.Send(query);
        }

        public async Task LoadSpec()
        {
            var query = new GetAllVetSpecializationsQuery();
            var result = await _mediator.Send(query);

            ViewData["Specializations"] = new SelectList(result, "VetSpecializationId", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSpec();
                return Page();
            }

            try
            {
                var command = new UpdateVetCommand(Id, Input);
                var updatedId = await _mediator.Send(command);

                TempData["SuccessMessage"] = $"Vet updated successfully (ID: {updatedId}).";
                return RedirectToPage("./Index");
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
            catch (BadRequestException ex)
            {
                foreach (var error in ex.ValidationErrors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the vet.");
            }

            await LoadSpec();
            return Page();
        }
    }
}
