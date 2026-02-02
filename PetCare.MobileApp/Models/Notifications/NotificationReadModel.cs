using PetCare.MobileApp.Enums;

namespace PetCare.MobileApp.Models.Notifications
{
    public class NotificationReadModel
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
