using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.MedicalTests.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.MedicalTests.Queries
{
    public class GetMedicalTestsByAppointmentIdQuery : IRequest<List<MedicalTestReadModel>>
    {
        public int AppointmentId { get; set; }
    }

    public class GetMedicalTestsByAppointmentIdHandler : IRequestHandler<GetMedicalTestsByAppointmentIdQuery, List<MedicalTestReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetMedicalTestsByAppointmentIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MedicalTestReadModel>> Handle(GetMedicalTestsByAppointmentIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.MedicalTests
                .Where(m => m.AppointmentId == request.AppointmentId)
                .OrderByDescending(m => m.TestDate)
                .ToListAsync(cancellationToken);

            return items.Select(entity => new MedicalTestReadModel
            {
                MedicalTestId = entity.MedicalTestId,
                TestName = entity.TestName,
                Result = entity.Result,
                TestDate = entity.TestDate,
                AttachmentUrl = entity.AttachmentUrl,
                PetId = entity.PetId,
                AppointmentId = entity.AppointmentId
            }).ToList();
        }
    }
}
