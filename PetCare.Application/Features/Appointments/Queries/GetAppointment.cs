using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetAppointmentQuery : IRequest<AppointmentReadModel>
    {
        public int AppointmentId { get; }
        public GetAppointmentQuery(int appointmentId) => AppointmentId = appointmentId;
    }

    public class GetAppointmentHandler : IRequestHandler<GetAppointmentQuery, AppointmentReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetAppointmentHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentReadModel> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Pet)
                    .ThenInclude(a => a.PetOwner)
                .Include(a => a.Vet)
                .Include(a => a.AppointmentProcedures)
                    .ThenInclude(ap => ap.Procedure)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment", request.AppointmentId);
            }

            var invoice = await _context.Invoices
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.AppointmentId == request.AppointmentId, cancellationToken);

            return new AppointmentReadModel
            {
                AppointmentId = appointment.AppointmentId,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Description = appointment.Description,
                Status = appointment.Status,
                ReasonForVisit = appointment.ReasonForVisit,
                Diagnosis = appointment.Diagnosis,
                Notes = appointment.Notes,
                PetId = appointment.PetId,
                PetName = appointment.Pet.Name,
                PetSpecies = appointment.Pet.Species,
                PetImageUrl = appointment.Pet.ImageUrl,
                OwnerName = appointment.Pet.PetOwner.FirstName + " " + appointment.Pet.PetOwner.LastName,
                VetId = appointment.VetId,
                VetName = appointment.Vet.FirstName + " " + appointment.Vet.LastName,
                InvoiceId = invoice?.InvoiceId,
                Procedures = appointment.AppointmentProcedures.Select(ap => new AppointmentProcedureReadModel
                {
                    ProcedureId = ap.ProcedureId,
                    ProcedureName = ap.Procedure.Name,
                    Quantity = ap.Quantity,
                    FinalPrice = ap.FinalPrice,
                    TotalPrice = ap.FinalPrice * ap.Quantity
                }).ToList()
            };
        }
    }
}
