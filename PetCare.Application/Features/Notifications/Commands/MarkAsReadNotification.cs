using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Notifications.Commands
{
    public class MarkAsReadNotificationCommand : IRequest<int>
    {
        public int Id { get; }

        public MarkAsReadNotificationCommand(int id)
        {
            Id = id;
        }
    }

    public class MarkAsReadNotificationHandler : IRequestHandler<MarkAsReadNotificationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public MarkAsReadNotificationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(MarkAsReadNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == request.Id, cancellationToken);

            if (notification == null)
            {
                throw new NotFoundException("Notification", request.Id);
            }

            notification.ReadAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return notification.NotificationId;
        }
    }
}
