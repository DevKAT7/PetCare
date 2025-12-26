using MediatR;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<int>
    {
        public required NotificationCreateModel Notification { get; set; }
    }

    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateNotificationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var model = request.Notification;

            var notification = new Notification
            {
                Message = model.Message,
                Type = model.Type,
                UserId = model.UserId,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return notification.NotificationId;
        }
    }
}
