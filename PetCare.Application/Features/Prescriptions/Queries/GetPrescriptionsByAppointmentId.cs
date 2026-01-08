using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Prescriptions.Queries
{
    public class GetPrescriptionsByAppointmentIdQuery : IRequest<List<PrescriptionReadModel>>
    {
        public int AppointmentId { get; set; }
    }

    public class GetPrescriptionsByAppointmentIdHandler : IRequestHandler<GetPrescriptionsByAppointmentIdQuery, List<PrescriptionReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetPrescriptionsByAppointmentIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PrescriptionReadModel>> Handle(GetPrescriptionsByAppointmentIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Prescriptions
                .Where(p => p.AppointmentId == request.AppointmentId)
                .ToListAsync(cancellationToken);

            return items.Select(entity => new PrescriptionReadModel
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
                MedicationName = entity.Medication?.Name ?? "Unknown Drug"
            }).ToList();
        }
    }
}
