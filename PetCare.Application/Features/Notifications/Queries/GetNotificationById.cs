using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Notifications.Queries
{
    public class GetNotificationQuery : IRequest<NotificationReadModel>
    {
        public int Id { get; set; }
    }

    public class GetNotificationHandler : IRequestHandler<GetNotificationQuery, NotificationReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetNotificationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationReadModel> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == request.Id, cancellationToken);

            if (notification == null)
            {
                throw new NotFoundException("Notification", request.Id);
            }

            return new NotificationReadModel
            {
                NotificationId = notification.NotificationId,
                Message = notification.Message,
                Type = notification.Type,
                CreatedAt = notification.CreatedAt,
                ReadAt = notification.ReadAt,
                IsRead = notification.IsRead,
                UserId = notification.UserId
            };
        }
    }
}
