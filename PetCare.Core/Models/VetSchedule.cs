namespace PetCare.Core.Models
{
    public class VetSchedule
    {
        public int VetScheduleId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int VetId { get; set; }
        public Vet Vet { get; set; } = null!;
    }
}
