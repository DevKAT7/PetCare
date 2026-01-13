namespace PetCare.Application.Features.MedicalTests.Dtos
{
    public class MedicalTestCreateModel
    {
        public string TestName { get; set; } = string.Empty;
        public string? Result { get; set; }
        public DateOnly TestDate { get; set; }
        public string? AttachmentUrl { get; set; }
        public int PetId { get; set; }
        public int AppointmentId { get; set; }
    }
}
