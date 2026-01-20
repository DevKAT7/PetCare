namespace PetCare.Application.Features.VetSchedules.Dtos
{
    public class VetScheduleReadModel
    {
        public int VetScheduleId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int VetId { get; set; }
    }
}
