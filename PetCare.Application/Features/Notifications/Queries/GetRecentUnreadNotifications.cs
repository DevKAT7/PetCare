using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Notifications.Queries
{
    public record GetRecentUnreadNotificationsQuery(string UserId) : IRequest<List<Notification>>;

    public class GetRecentUnreadNotificationsHandler : IRequestHandler<GetRecentUnreadNotificationsQuery, List<Notification>>
    {
        private readonly IApplicationDbContext _context;

        public GetRecentUnreadNotificationsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> Handle(GetRecentUnreadNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Where(n => n.UserId == request.UserId && n.ReadAt == null)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync(cancellationToken);
        }
    }
}
