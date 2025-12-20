using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.MedicalTests.Dto;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.MedicalTests.Queries
{
    public class GetMedicalTestsByPetIdQuery : IRequest<List<MedicalTestReadModel>>
    {
        public int PetId { get; set; }
    }

    public class GetMedicalTestsByPetIdHandler : IRequestHandler<GetMedicalTestsByPetIdQuery, List<MedicalTestReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetMedicalTestsByPetIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MedicalTestReadModel>> Handle(GetMedicalTestsByPetIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.MedicalTests
                .Where(m => m.PetId == request.PetId)
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
