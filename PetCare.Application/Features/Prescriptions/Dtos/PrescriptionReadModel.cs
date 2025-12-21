namespace PetCare.Application.Features.Prescriptions.Dtos
{
    public class PrescriptionReadModel
    {
        public int PrescriptionId { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public int PacksToDispense { get; set; }
        public int AppointmentId { get; set; }
        public int MedicationId { get; set; }
    }
}
