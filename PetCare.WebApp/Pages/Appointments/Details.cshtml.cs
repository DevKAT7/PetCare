using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Features.MedicalTests.Dtos;
using PetCare.Application.Features.MedicalTests.Queries;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Features.Prescriptions.Queries;
using PetCare.Application.Interfaces;

namespace PetCare.WebApp.Pages.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IDocumentGenerator _documentGenerator;

        public DetailsModel(IMediator mediator, IDocumentGenerator documentGenerator)
        {
            _mediator = mediator;
            _documentGenerator = documentGenerator;
        }

        public AppointmentReadModel Appointment { get; private set; } = default!;

        public List<PrescriptionReadModel> Prescriptions { get; private set; } = new();

        public List<MedicalTestReadModel> MedicalTests { get; set; } = new();

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

            MedicalTests = await _mediator.Send(new GetMedicalTestsByAppointmentIdQuery { AppointmentId = id });

            return Page();
        }

        public async Task<IActionResult> OnGetDownloadPrescriptionAsync(int prescriptionId, string format)
        {
            var rx = await _mediator.Send(new GetPrescriptionQuery { Id = prescriptionId });

            if (rx == null) return NotFound();

            var fileData = _documentGenerator.GeneratePrescription(rx, format);

            return File(fileData.Content, fileData.ContentType, fileData.FileName);
        }
    }
}
