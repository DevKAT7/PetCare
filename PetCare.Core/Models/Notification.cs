using PetCare.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        [MaxLength(500)]
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; } = NotificationType.General;
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsRead => ReadAt.HasValue;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
