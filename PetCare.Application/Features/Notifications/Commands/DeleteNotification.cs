using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Notifications.Commands
{
    public class DeleteNotificationCommand : IRequest<int>
    {
        public int Id { get; }

        public DeleteNotificationCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteNotificationHandler : IRequestHandler<DeleteNotificationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteNotificationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == request.Id, cancellationToken);

            if (notification == null)
            {
                throw new NotFoundException("Notification", request.Id);
            }

            _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return notification.NotificationId;
        }
    }
}
