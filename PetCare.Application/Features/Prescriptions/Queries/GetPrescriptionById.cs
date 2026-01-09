using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Prescriptions.Queries
{
    public class GetPrescriptionQuery : IRequest<PrescriptionReadModel>
    {
        public int Id { get; set; }
    }

    public class GetPrescriptionHandler : IRequestHandler<GetPrescriptionQuery, PrescriptionReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetPrescriptionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionReadModel> Handle(GetPrescriptionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Prescriptions
                .Include(p => p.Medication)
                .FirstOrDefaultAsync(p => p.PrescriptionId == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Prescription not found.");
            }

            return new PrescriptionReadModel
            {
                PrescriptionId = entity.PrescriptionId,
                Dosage = entity.Dosage,
                Frequency = entity.Frequency,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Instructions = entity.Instructions,
                PacksToDispense = entity.PacksToDispense,
                AppointmentId = entity.AppointmentId,
                MedicationId = entity.MedicationId,
                MedicationName = entity.Medication.Name
            };
        }
    }
}
