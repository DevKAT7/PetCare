using PetCare.Core.Enums;

namespace PetCare.Application.Features.Notifications.Dtos
{
    public class NotificationReadModel
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
