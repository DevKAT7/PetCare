using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.VetSpecializations.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Admin.Vets
{
    [Authorize(Roles = "Admin")]
    public class CreateVetModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateVetModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public CreateVetCommand Input { get; set; } = default!;

        public async Task OnGetAsync()
        {
            await LoadSpec();
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
                var newVetId = await _mediator.Send(Input);

                TempData["SuccessMessage"] = $"Vet account created successfully (ID: {newVetId}).";
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
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the vet.");
            }

            await LoadSpec();
            return Page();
        }
    }
}
