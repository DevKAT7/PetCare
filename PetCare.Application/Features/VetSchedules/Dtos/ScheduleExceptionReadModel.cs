using PetCare.Core.Enums;

namespace PetCare.Application.Features.VetSchedules.Dto
{
    public class ScheduleExceptionReadModel
    {
        public int ScheduleExceptionId { get; set; }
        public DateOnly ExceptionDate { get; set; }
        public bool IsFullDayAbsence { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? Reason { get; set; }
        public ScheduleExceptionStatus Status { get; set; }
        public int VetId { get; set; }
    }
}
