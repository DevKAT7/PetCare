namespace PetCare.Core.Models
{
    public class MedicalTest
    {
        public int MedicalTestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public DateOnly TestDate { get; set; }
        public string AttachmentUrl { get; set; } = string.Empty;

        public int PetId { get; set; }
        public int AppointmentId { get; set; }
    }
}
