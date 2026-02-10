using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Notifications.Commands
{
    public class MarkAllNotificationsAsReadCommand : IRequest
    {
    }

    public class MarkAllNotificationsAsReadHandler : IRequestHandler<MarkAllNotificationsAsReadCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MarkAllNotificationsAsReadHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null) return;

            var userId = _userManager.GetUserId(user);

            if (string.IsNullOrEmpty(userId)) return;

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.ReadAt == null)
                .ToListAsync(cancellationToken);

            if (notifications.Any())
            {
                var now = DateTime.Now;
                foreach (var notification in notifications)
                {
                    notification.ReadAt = now;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
