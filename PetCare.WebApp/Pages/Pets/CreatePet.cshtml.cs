using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.PetOwners.Queries;
using PetCare.Application.Features.Pets.Commands;
using PetCare.Application.Features.Pets.Dto;
using PetCare.Application.Features.Pets.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Pets
{
    [Authorize(Roles = "Admin,Employee")]
    public class CreatePetModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreatePetModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public PetCreateModel Input { get; set; } = new();

        public SelectList OwnerOptions { get; set; }
        public List<string> SpeciesSuggestions { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadOptions();

            Input.DateOfBirth = DateTime.Today;
            Input.IsMale = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadOptions();
                return Page();
            }

            try
            {
                await _mediator.Send(new CreatePetCommand { Pet = Input });

                TempData["SuccessMessage"] = $"Patient '{Input.Name}' created successfully.";
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
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
            }

            await LoadOptions();
            return Page();
        }

        private async Task LoadOptions()
        {
            var owners = await _mediator.Send(new GetPetOwnersLookupQuery());
            OwnerOptions = new SelectList(owners, "Id", "DisplayName");

            SpeciesSuggestions = await _mediator.Send(new GetUniqueSpeciesQuery());
        }
    }
}
