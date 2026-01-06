using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Commands
{
    public class UpdateAppointmentCommand : IRequest<int>
    {
        public int AppointmentId { get; }
        public AppointmentUpdateModel Appointment { get; }

        public UpdateAppointmentCommand(int appointmentId, AppointmentUpdateModel appointment)
        {
            AppointmentId = appointmentId;
            Appointment = appointment;
        }
    }

    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateAppointmentHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment", request.AppointmentId);
            }

            var model = request.Appointment;
            //TODO: zobacz pozniej czy bedziesz chciala umozliwiac zmiane zwierzaka i weterynarza w wizycie
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.PetId == model.PetId);

            if (pet == null)
            {
                throw new NotFoundException("Pet", model.PetId);
            }

            var vet = await _context.Vets.FirstOrDefaultAsync(v => v.VetId == model.VetId);

            if (vet == null)
            {
                throw new NotFoundException("Vet", model.VetId);
            }

            appointment.AppointmentDateTime = model.AppointmentDateTime;
            appointment.Description = model.Description;
            appointment.ReasonForVisit = model.ReasonForVisit;
            appointment.Diagnosis = model.Diagnosis;
            appointment.Notes = model.Notes;
            appointment.PetId = model.PetId;
            appointment.VetId = model.VetId;
            appointment.Status = model.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return appointment.AppointmentId;
        }
    }
}
