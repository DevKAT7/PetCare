using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSchedules.Queries
{
    public class GetVetScheduleQuery : IRequest<VetScheduleReadModel>
    {
        public int VetScheduleId { get; set; }
    }

    public class GetVetScheduleHandler : IRequestHandler<GetVetScheduleQuery, VetScheduleReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetVetScheduleHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetScheduleReadModel> Handle(GetVetScheduleQuery request, CancellationToken cancellationToken)
        {
            var schedule = await _context.VetSchedules
                .FirstOrDefaultAsync(s => s.VetScheduleId == request.VetScheduleId, cancellationToken);

            if (schedule == null)
            {
                throw new NotFoundException("Vet schedule not found.");
            }

            return new VetScheduleReadModel
            {
                VetScheduleId = schedule.VetScheduleId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                VetId = schedule.VetId
            };
        }
    }
}
