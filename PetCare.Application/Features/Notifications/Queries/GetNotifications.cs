using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<List<NotificationReadModel>> { }

    public class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, List<NotificationReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetNotificationsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationReadModel>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Select(n => new NotificationReadModel
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    Type = n.Type,
                    CreatedAt = n.CreatedAt,
                    ReadAt = n.ReadAt,
                    IsRead = n.IsRead,
                    UserId = n.UserId
                })
                .ToListAsync(cancellationToken);
        }
    }
}
