using PetCare.Core.Enums;

namespace PetCare.Application.Features.Notifications.Dtos
{
    public class NotificationCreateModel
    {
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; } = NotificationType.General;
        public string UserId { get; set; } = string.Empty;
    }
}
