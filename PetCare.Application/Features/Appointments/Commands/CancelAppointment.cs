using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

namespace PetCare.Application.Features.Appointments.Commands
{
    public class CancelAppointmentCommand : IRequest<int>
    {
        public int AppointmentId { get; }
        public CancelAppointmentCommand(int appointmentId) => AppointmentId = appointmentId;
    }

    public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CancelAppointmentHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<int> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Pet).ThenInclude(p => p.PetOwner)
                .Include(a => a.Vet)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment", request.AppointmentId);
            }

            appointment.Status = AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync(cancellationToken);

            if (appointment.Vet != null && appointment.Vet.UserId != null && appointment.Pet.PetOwner.UserId != null)
            {
                await _mediator.Send(new CreateNotificationCommand
                {
                    Notification = new NotificationCreateModel
                    {
                        UserId = appointment.Vet.UserId,
                        Type = NotificationType.AppointmentCancelled,
                        Message = $"Appointment with {appointment.Pet.Name} on " +
                        $"{appointment.AppointmentDateTime:yyyy-MM-dd HH:mm} has been CANCELLED."
                    }
                }, cancellationToken);

                await _mediator.Send(new CreateNotificationCommand
                {
                    Notification = new NotificationCreateModel
                    {
                        UserId = appointment.Pet.PetOwner.UserId,
                        Type = NotificationType.AppointmentCancelled,
                        Message = $"Appointment with {appointment.Pet.Name} on " +
                        $"{appointment.AppointmentDateTime:yyyy-MM-dd HH:mm} has been CANCELLED."
                    }
                }, cancellationToken);
            }

            return appointment.AppointmentId;
        }
    }
}
