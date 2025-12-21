using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vaccinations.Queries
{
    public class GetVaccinationsByPetIdQuery : IRequest<List<VaccinationReadModel>>
    {
        public int PetId { get; set; }
    }

    public class GetVaccinationsByPetIdHandler : IRequestHandler<GetVaccinationsByPetIdQuery, List<VaccinationReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetVaccinationsByPetIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VaccinationReadModel>> Handle(GetVaccinationsByPetIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Set<Vaccination>()
                .Where(v => v.PetId == request.PetId)
                .ToListAsync(cancellationToken);

            return items.Select(entity => new VaccinationReadModel
            {
                VaccinationId = entity.VaccinationId,
                VaccineName = entity.VaccineName,
                VaccinationDate = entity.VaccinationDate,
                NextDueDate = entity.NextDueDate,
                PetId = entity.PetId,
                AppointmentId = entity.AppointmentId
            }).ToList();
        }
    }
}
