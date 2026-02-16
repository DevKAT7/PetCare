using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Application.Features.VetSpecializations.Queries;
using PetCare.WebApp.Pages.Shared;

namespace PetCare.WebApp.Pages.Admin.Vets
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : BasePageModel
    {
        public IndexModel(IMediator mediator) : base(mediator)
        {
        }

        public List<VetReadModel> Vets { get; set; } = new();
        public List<SelectListItem>? SpecializationOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedSpecializationId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; } = "LastName";

        [BindProperty(SupportsGet = true)]
        public string SortDirection { get; set; } = "asc";

        public async Task OnGetAsync()
        {
            await LoadPageTextsAsync();

            var specs = await _mediator.Send(new GetAllVetSpecializationsQuery());
            SpecializationOptions = specs
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.VetSpecializationId.ToString()
                })
                .ToList();

            Vets = await _mediator.Send(new GetAllVetsQuery
            {
                SearchTerm = SearchTerm,
                SpecializationId = SelectedSpecializationId,
                SortColumn = SortColumn,
                SortDirection = SortDirection
            });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _mediator.Send(new DeleteVetCommand(id));
            TempData["SuccessMessage"] = "Vet deleted successfully.";
            return RedirectToPage();
        }
    }
}
