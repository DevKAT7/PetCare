using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Features.VetSchedules.Dto
{
    public class ScheduleExceptionCreateModel
    {
        public DateOnly ExceptionDate { get; set; }
        public bool IsFullDayAbsence { get; set; } = false;
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        [MaxLength(200)]
        public string? Reason { get; set; }
        public int VetId { get; set; }
    }
}
