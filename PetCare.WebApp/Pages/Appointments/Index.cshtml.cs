using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Core.Enums;

namespace PetCare.WebApp.Pages.Appointments
{
    [Authorize(Roles = "Admin, Employee")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<AppointmentReadModel> Appointments { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchPet { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchOwner { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchVetId { get; set; }

        [BindProperty(SupportsGet = true)]
        public AppointmentStatus? SearchStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; } = "Date";

        [BindProperty(SupportsGet = true)]
        public string SortDirection { get; set; } = "desc";

        public List<SelectListItem> VetOptions { get; set; } = new();
        public SelectList? StatusOptions { get; set; }
        public async Task OnGetAsync()
        {
            var vets = await _mediator.Send(new GetAllVetsQuery());
            VetOptions = vets.Select(v => new SelectListItem
            {
                Text = v.FullName,
                Value = v.VetId.ToString()
            }).ToList();

            StatusOptions = new SelectList(Enum.GetValues(typeof(AppointmentStatus)));

            var query = new GetAllAppointmentsQuery(
                petName: SearchPet,
                ownerName: SearchOwner,
                vetId: SearchVetId,
                status: SearchStatus,
                sortColumn: SortColumn,
                sortDirection: SortDirection
            );

            Appointments = await _mediator.Send(query);
        }
    }
}
