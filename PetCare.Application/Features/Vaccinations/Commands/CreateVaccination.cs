using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vaccinations.Commands
{
    public class CreateVaccinationCommand : IRequest<int>
    {
        public required VaccinationCreateModel Vaccination { get; set; }
    }

    public class CreateVaccinationHandler : IRequestHandler<CreateVaccinationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateVaccinationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateVaccinationCommand request, CancellationToken cancellationToken)
        {
            var model = request.Vaccination;

            var petExists = await _context.Pets.AnyAsync(p => p.PetId == model.PetId, cancellationToken);

            if (!petExists)
            {
                throw new NotFoundException("Pet not found.");
            }

            var appointmentExists = await _context.Appointments.AnyAsync(a => a.AppointmentId == model.AppointmentId, cancellationToken);

            if (!appointmentExists)
            {
                throw new NotFoundException("Appointment not found.");
            }

            var vaccination = new Vaccination
            {
                VaccineName = model.VaccineName,
                VaccinationDate = model.VaccinationDate,
                NextDueDate = model.NextDueDate,
                PetId = model.PetId,
                AppointmentId = model.AppointmentId
            };

            _context.Vaccinations.Add(vaccination);

            await _context.SaveChangesAsync(cancellationToken);

            return vaccination.VaccinationId;
        }
    }
}
