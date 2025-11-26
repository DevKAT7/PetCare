namespace PetCare.Core.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        public string UserId { get; set; } = null!;
    }
}
