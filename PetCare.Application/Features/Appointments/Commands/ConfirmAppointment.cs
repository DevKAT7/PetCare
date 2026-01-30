using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Appointments.Commands
{
    public record ConfirmAppointmentCommand(int Id) : IRequest;

    public class ConfirmAppointmentHandler : IRequestHandler<ConfirmAppointmentCommand>
    {
        private readonly IApplicationDbContext _context;
        public ConfirmAppointmentHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == request.Id, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment", request.Id);
            }

            appointment.Status = Core.Enums.AppointmentStatus.Confirmed;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
