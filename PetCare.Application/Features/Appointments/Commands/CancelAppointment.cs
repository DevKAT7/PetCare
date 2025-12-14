using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Core.Enums;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Commands
{
    public class CancelAppointmentCommand : IRequest<int>
    {
        public int AppointmentId { get; }
        public CancelAppointmentCommand(int appointmentId) => AppointmentId = appointmentId;
    }

    public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CancelAppointmentHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);
            if (appointment == null) throw new NotFoundException("Appointment", request.AppointmentId);

            appointment.Status = AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync(cancellationToken);

            return appointment.AppointmentId;
        }
    }
}
