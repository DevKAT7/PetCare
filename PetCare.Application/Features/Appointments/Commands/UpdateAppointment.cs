using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

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
        private readonly IApplicationDbContext _context;

        public UpdateAppointmentHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.AppointmentProcedures)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment", request.AppointmentId);
            }

            var model = request.Appointment;

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

            _context.AppointmentProcedures.RemoveRange(appointment.AppointmentProcedures);

            foreach (var procDto in model.Procedures)
            {
                var newProc = new AppointmentProcedure
                {
                    AppointmentId = appointment.AppointmentId,
                    ProcedureId = procDto.ProcedureId,
                    Quantity = procDto.Quantity,
                    FinalPrice = procDto.FinalPrice
                };
                _context.AppointmentProcedures.Add(newProc);
            }
            await _context.SaveChangesAsync(cancellationToken);

            return appointment.AppointmentId;
        }
    }
}
