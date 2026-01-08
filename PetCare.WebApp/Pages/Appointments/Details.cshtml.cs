using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Features.Prescriptions.Queries;

namespace PetCare.WebApp.Pages.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public DetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public AppointmentReadModel Appointment { get; private set; } = default!;

        public List<PrescriptionReadModel> Prescriptions { get; private set; } = new();

        public int petAge { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Appointment = await _mediator.Send(new GetAppointmentQuery(id));
            }
            catch (Exception)
            {
                return NotFound();
            }

            Prescriptions = await _mediator.Send(new GetPrescriptionsByAppointmentIdQuery { AppointmentId = id });

            return Page();
        }
    }
}
