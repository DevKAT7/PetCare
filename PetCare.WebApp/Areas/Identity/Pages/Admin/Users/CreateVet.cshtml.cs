using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.VetSpecializations.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Areas.Identity.Pages.Admin.Users
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
                // Tworzymy i wysy³amy komendê.
                // Tutaj zadzia³a ValidationBehavior i rzuci ValidationException w razie b³êdów.   
                var newVetId = await _mediator.Send(Input);

                TempData["SuccessMessage"] = $"Vet account created successfully (ID: {newVetId}).";
                return RedirectToPage("./Index");
            }
            catch (ValidationException ex)
            {
                // Iterujemy po s³owniku b³êdów: IDictionary<string, string[]> Errors
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;    // np. "Email"
                    string[] errorMessages = entry.Value; // np. ["Email jest wymagany", "Z³y format"]

                    foreach (var errorMessage in errorMessages)
                    {
                        // Dodajemy prefiks "Input.", bo tak nazywa siê w³aœciwoœæ w PageModel,
                        // aby formularz wiedzia³, pod którym polem wyœwietliæ b³¹d.
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the vet.");
            }

            await LoadSpec();
            return Page();
        }
    }
}
