using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSchedules.Queries
{
    public class GetVetSchedulesByVetIdQuery : IRequest<List<VetScheduleReadModel>>
    {
        public int VetId { get; set; }
    }

    public class GetVetSchedulesByVetIdHandler : IRequestHandler<GetVetSchedulesByVetIdQuery, List<VetScheduleReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetVetSchedulesByVetIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VetScheduleReadModel>> Handle(GetVetSchedulesByVetIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.VetSchedules
                .Where(s => s.VetId == request.VetId)
                .ToListAsync(cancellationToken);

            return items.Select(entity => new VetScheduleReadModel
            {
                VetScheduleId = entity.VetScheduleId,
                DayOfWeek = entity.DayOfWeek,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                VetId = entity.VetId
            }).ToList();
        }
    }
}
