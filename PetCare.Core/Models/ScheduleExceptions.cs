namespace PetCare.Core.Models
{
    public class ScheduleExceptions
    {
        public int ScheduleExceptionsId { get; set; }
        public DateOnly ExceptionDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? Reason { get; set; }

        public int VetId { get; set; }
        public Vet Vet { get; set; } = null!;
    }
}
