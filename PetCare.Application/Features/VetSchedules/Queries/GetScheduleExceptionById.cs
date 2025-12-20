using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSchedules.Queries
{
    public class GetScheduleExceptionQuery : IRequest<ScheduleExceptionReadModel>
    {
        public int Id { get; set; }
    }

    public class GetScheduleExceptionHandler : IRequestHandler<GetScheduleExceptionQuery, ScheduleExceptionReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetScheduleExceptionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ScheduleExceptionReadModel> Handle(GetScheduleExceptionQuery request, CancellationToken cancellationToken)
        {
            var exception = await _context.ScheduleExceptions
                .FirstOrDefaultAsync(e => e.ScheduleExceptionId == request.Id, cancellationToken);

            if (exception == null)
            {
                throw new NotFoundException("ScheduleException", request.Id);
            }

            return new ScheduleExceptionReadModel
            {
                ScheduleExceptionId = exception.ScheduleExceptionId,
                ExceptionDate = exception.ExceptionDate,
                IsFullDayAbsence = exception.IsFullDayAbsence,
                StartTime = exception.StartTime,
                EndTime = exception.EndTime,
                Reason = exception.Reason,
                VetId = exception.VetId
            };
        }
    }
}
