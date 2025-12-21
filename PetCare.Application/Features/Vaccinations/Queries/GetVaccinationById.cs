using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vaccinations.Queries
{
    public class GetVaccinationQuery : IRequest<VaccinationReadModel>
    {
        public int VaccinationId { get; set; }
    }

    public class GetVaccinationHandler : IRequestHandler<GetVaccinationQuery, VaccinationReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetVaccinationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VaccinationReadModel> Handle(GetVaccinationQuery request, CancellationToken cancellationToken)
        {
            var vaccination = await _context.Set<Vaccination>()
                .FirstOrDefaultAsync(v => v.VaccinationId == request.VaccinationId, cancellationToken);

            if (vaccination == null)
            {
                throw new NotFoundException("Vaccination not found.");
            }

            return new VaccinationReadModel
            {
                VaccinationId = vaccination.VaccinationId,
                VaccineName = vaccination.VaccineName,
                VaccinationDate = vaccination.VaccinationDate,
                NextDueDate = vaccination.NextDueDate,
                PetId = vaccination.PetId,
                AppointmentId = vaccination.AppointmentId
            };
        }
    }
}
