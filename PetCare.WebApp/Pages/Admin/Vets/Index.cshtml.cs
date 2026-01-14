using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Application.Features.VetSpecializations.Queries;

namespace PetCare.WebApp.Pages.Admin.Vets
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
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
