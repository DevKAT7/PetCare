using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Core.Models;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vaccinations.Commands
{
    public class UpdateVaccinationCommand : IRequest<int>
    {
        public int Id { get; }
        public VaccinationCreateModel Vaccination { get; set; }

        public UpdateVaccinationCommand(int id, VaccinationCreateModel model)
        {
            Id = id;
            Vaccination = model;
        }
    }

    public class UpdateVaccinationHandler : IRequestHandler<UpdateVaccinationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVaccinationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateVaccinationCommand request, CancellationToken cancellationToken)
        {
            var vaccination = await _context.Vaccinations
                .FirstOrDefaultAsync(v => v.VaccinationId == request.Id, cancellationToken);

            if (vaccination == null)
            {
                throw new NotFoundException("Vaccination", request.Id);
            }

            var model = request.Vaccination;

            vaccination.VaccineName = model.VaccineName;
            vaccination.VaccinationDate = model.VaccinationDate;
            vaccination.NextDueDate = model.NextDueDate;
            vaccination.PetId = model.PetId;
            vaccination.AppointmentId = model.AppointmentId;

            await _context.SaveChangesAsync(cancellationToken);

            return vaccination.VaccinationId;
        }
    }
}
