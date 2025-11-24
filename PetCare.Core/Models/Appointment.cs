namespace PetCare.Core.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ReasonForVisit { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public int PetId { get; set; }
        public int VetId { get; set; }
    }
}
