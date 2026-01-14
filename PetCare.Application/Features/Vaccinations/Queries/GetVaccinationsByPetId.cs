using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vaccinations.Queries
{
    public class GetVaccinationsByPetIdQuery : IRequest<List<VaccinationReadModel>>
    {
        public int PetId { get; set; }
    }

    public class GetVaccinationsByPetIdHandler : IRequestHandler<GetVaccinationsByPetIdQuery, List<VaccinationReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetVaccinationsByPetIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VaccinationReadModel>> Handle(GetVaccinationsByPetIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Vaccinations
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
