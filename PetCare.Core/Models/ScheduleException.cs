using PetCare.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class ScheduleException
    {
        [Key]
        public int ScheduleExceptionId { get; set; }
        public DateOnly ExceptionDate { get; set; }
        public bool IsFullDayAbsence { get; set; } = false;
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        [MaxLength(200)]
        public string? Reason { get; set; }
        public ScheduleExceptionStatus Status { get; set; } = ScheduleExceptionStatus.Pending;
        public int VetId { get; set; }
        public Vet Vet { get; set; } = null!;
    }
}
