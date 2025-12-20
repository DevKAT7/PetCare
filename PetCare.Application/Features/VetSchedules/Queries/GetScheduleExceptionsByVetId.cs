using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSchedules.Queries
{
    public class GetScheduleExceptionsByVetIdQuery : IRequest<List<ScheduleExceptionReadModel>>
    {
        public int VetId { get; set; }
    }

    public class GetScheduleExceptionsByVetIdHandler : IRequestHandler<GetScheduleExceptionsByVetIdQuery, List<ScheduleExceptionReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetScheduleExceptionsByVetIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ScheduleExceptionReadModel>> Handle(GetScheduleExceptionsByVetIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.ScheduleExceptions
                .Where(e => e.VetId == request.VetId)
                .ToListAsync(cancellationToken);

            return items.Select(entity => new ScheduleExceptionReadModel
            {
                ScheduleExceptionId = entity.ScheduleExceptionId,
                ExceptionDate = entity.ExceptionDate,
                IsFullDayAbsence = entity.IsFullDayAbsence,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Reason = entity.Reason,
                VetId = entity.VetId
            }).ToList();
        }
    }
}
