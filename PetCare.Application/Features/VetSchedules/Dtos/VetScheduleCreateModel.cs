namespace PetCare.Application.Features.VetSchedules.Dto
{
    public class VetScheduleCreateModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int VetId { get; set; }
    }
}
