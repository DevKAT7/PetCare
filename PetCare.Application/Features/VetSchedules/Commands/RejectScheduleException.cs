using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class RejectScheduleExceptionCommand : IRequest<Unit>
    {
        public int ExceptionId { get; set; }
    }

    public class RejectScheduleExceptionHandler : IRequestHandler<RejectScheduleExceptionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public RejectScheduleExceptionHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RejectScheduleExceptionCommand request, CancellationToken cancellationToken)
        {
            var exception = await _context.ScheduleExceptions
                .Include(e => e.Vet).ThenInclude(v => v.User)
                .FirstOrDefaultAsync(e => e.ScheduleExceptionId == request.ExceptionId, cancellationToken);

            if (exception == null) throw new NotFoundException("Exception", request.ExceptionId);

            exception.Status = ScheduleExceptionStatus.Rejected;
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new CreateNotificationCommand
            {
                Notification = new NotificationCreateModel
                {
                    UserId = exception.Vet.UserId,
                    Type = NotificationType.VacationRejected,
                    Message = $"Your leave request for {exception.ExceptionDate:yyyy-MM-dd} has been REJECTED."
                }
            });

            return Unit.Value;
        }
    }
}
