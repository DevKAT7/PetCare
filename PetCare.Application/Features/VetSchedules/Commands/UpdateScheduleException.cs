using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class UpdateScheduleExceptionCommand : IRequest<int>
    {
        public int Id { get; }
        public ScheduleExceptionCreateModel Exception { get; set; }

        public UpdateScheduleExceptionCommand(int id, ScheduleExceptionCreateModel exception)
        {
            Id = id;
            Exception = exception;
        }
    }

    public class UpdateScheduleExceptionHandler : IRequestHandler<UpdateScheduleExceptionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateScheduleExceptionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateScheduleExceptionCommand request, CancellationToken cancellationToken)
        {
            var exception = await _context.ScheduleExceptions
                .FirstOrDefaultAsync(e => e.ScheduleExceptionId == request.Id, cancellationToken);

            if (exception == null)
            {
                throw new NotFoundException("ScheduleException", request.Id);
            }

            var model = request.Exception;

            exception.ExceptionDate = model.ExceptionDate;
            exception.IsFullDayAbsence = model.IsFullDayAbsence;
            exception.StartTime = model.StartTime;
            exception.EndTime = model.EndTime;
            exception.Reason = model.Reason;
            exception.VetId = model.VetId;

            await _context.SaveChangesAsync(cancellationToken);

            return exception.ScheduleExceptionId;
        }
    }
}
