using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Pets.Commands;
using PetCare.Shared.Dtos;
using PetCare.Application.Features.Pets.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Pets
{
    [Authorize(Roles = "Admin,Employee")]
    public class EditPetModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditPetModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public PetUpdateModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public List<string> SpeciesSuggestions { get; set; } = new();

        public string OwnerName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var pet = await _mediator.Send(new GetPetQuery(Id));

                Input = new PetUpdateModel
                {
                    Name = pet.Name,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    DateOfBirth = pet.DateOfBirth,
                    IsMale = pet.IsMale,
                    ImageUrl = pet.ImageUrl,
                    PetOwnerId = pet.PetOwnerId
                };

                OwnerName = pet.PetOwnerName;

                await LoadOptions();

                return Page();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        private async Task LoadOptions()
        {
            SpeciesSuggestions = await _mediator.Send(new GetUniqueSpeciesQuery());
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
                await _mediator.Send(new UpdatePetCommand(Id, Input));

                TempData["SuccessMessage"] = $"Patient details updated successfully.";
                return RedirectToPage("./Index");
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    foreach (var errorMessage in entry.Value)
                    {
                        ModelState.AddModelError($"Input.{entry.Key}", errorMessage);
                    }
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
            }

            await LoadOptions();

            var pet = await _mediator.Send(new GetPetQuery(Id));
            OwnerName = pet.PetOwnerName;

            return Page();
        }
    }
}