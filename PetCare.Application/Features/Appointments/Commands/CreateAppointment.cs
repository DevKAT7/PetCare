using MediatR;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Core.Models;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Appointments.Commands
{
    public class CreateAppointmentCommand : IRequest<int>
    {
        public required AppointmentCreateModel Appointment { get; set; }
    }

    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateAppointmentHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var model = request.Appointment;

            var pet = await _context.Pets.FindAsync(new object[] { model.PetId }, cancellationToken);

            if (pet == null)
            {
                throw new NotFoundException("Pet", model.PetId);
            }

            var vet = await _context.Vets.FindAsync(new object[] { model.VetId }, cancellationToken);

            if (vet == null)
            {
                throw new NotFoundException("Vet", model.VetId);
            }

            var appointment = new Appointment
            {
                AppointmentDateTime = model.AppointmentDateTime,
                Description = model.Description,
                ReasonForVisit = model.ReasonForVisit,
                Diagnosis = model.Diagnosis,
                Notes = model.Notes,
                PetId = model.PetId,
                VetId = model.VetId,
                Status = Core.Enums.AppointmentStatus.Scheduled
            };

            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync(cancellationToken);

            return appointment.AppointmentId;
        }
    }
}
