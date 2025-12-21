using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Medications.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Medications.Queries
{
    public class GetMedicationsQuery : IRequest<List<MedicationReadModel>> { }

    public class GetMedicationsHandler : IRequestHandler<GetMedicationsQuery, List<MedicationReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetMedicationsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MedicationReadModel>> Handle(GetMedicationsQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Medications.ToListAsync(cancellationToken);

            return items.Select(m => new MedicationReadModel
            {
                MedicationId = m.MedicationId,
                Name = m.Name,
                Description = m.Description,
                Manufacturer = m.Manufacturer,
                Price = m.Price,
                IsActive = m.IsActive
            }).ToList();
        }
    }
}
