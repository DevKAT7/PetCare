using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetAppointmentQuery : IRequest<AppointmentReadModel>
    {
        public int AppointmentId { get; }
        public GetAppointmentQuery(int appointmentId) => AppointmentId = appointmentId;
    }

    public class GetAppointmentHandler : IRequestHandler<GetAppointmentQuery, AppointmentReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetAppointmentHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentReadModel> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment", request.AppointmentId);
            }

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
                VetId = appointment.VetId,
                VetName = appointment.Vet.FirstName + " " + appointment.Vet.LastName
            };
        }
    }
}
