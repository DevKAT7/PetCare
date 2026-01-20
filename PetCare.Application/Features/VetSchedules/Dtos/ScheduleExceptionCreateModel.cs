namespace PetCare.Application.Features.VetSchedules.Dtos
{
    public class ScheduleExceptionCreateModel
    {
        public DateOnly ExceptionDate { get; set; }
        public bool IsFullDayAbsence { get; set; } = false;
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? Reason { get; set; }
        public int VetId { get; set; }
    }
}
