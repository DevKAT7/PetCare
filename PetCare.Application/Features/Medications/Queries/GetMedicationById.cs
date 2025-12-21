using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Medications.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Medications.Queries
{
    public class GetMedicationQuery : IRequest<MedicationReadModel>
    {
        public int Id { get; set; }
    }

    public class GetMedicationHandler : IRequestHandler<GetMedicationQuery, MedicationReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetMedicationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicationReadModel> Handle(GetMedicationQuery request, CancellationToken cancellationToken)
        {
            var medication = await _context.Medications.FirstOrDefaultAsync(x => x.MedicationId == request.Id, cancellationToken);

            if (medication == null)
            {
                throw new NotFoundException("Medication not found.");
            }

            return new MedicationReadModel
            {
                MedicationId = medication.MedicationId,
                Name = medication.Name,
                Description = medication.Description,
                Manufacturer = medication.Manufacturer,
                Price = medication.Price,
                IsActive = medication.IsActive
            };
        }
    }
}
