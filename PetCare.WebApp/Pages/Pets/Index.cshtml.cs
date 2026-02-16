using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Pets.Commands;
using PetCare.Application.Features.Pets.Dtos;
using PetCare.Application.Features.Pets.Queries;
using PetCare.WebApp.Pages.Shared;

namespace PetCare.WebApp.Pages.Pets
{
    [Authorize(Roles = "Admin,Employee")]
    public class IndexModel : BasePageModel
    {
        public IndexModel(IMediator mediator) : base(mediator)
        {
        }

        public List<PetReadModel> Pets { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterSpecies { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterSex { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; } = "Name";

        [BindProperty(SupportsGet = true)]
        public string SortDirection { get; set; } = "asc";

        public SelectList SpeciesOptions { get; set; }
        public SelectList SexOptions { get; set; }

        public async Task OnGetAsync()
        {
            await LoadPageTextsAsync();

            var speciesList = await _mediator.Send(new GetUniqueSpeciesQuery());
            SpeciesOptions = new SelectList(speciesList);

            SexOptions = new SelectList(new[]
            {
                new { Value = "Male", Text = "Male" },
                new { Value = "Female", Text = "Female" }
            }, "Value", "Text");

            Pets = await _mediator.Send(new GetAllPetsQuery
            {
                SearchTerm = SearchTerm,
                FilterSpecies = FilterSpecies,
                FilterSex = FilterSex,
                SortColumn = SortColumn,
                SortDirection = SortDirection
            });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _mediator.Send(new DeletePetCommand(id));
            TempData["SuccessMessage"] = "Patient archived successfully.";
            return RedirectToPage();
        }
    }
}
