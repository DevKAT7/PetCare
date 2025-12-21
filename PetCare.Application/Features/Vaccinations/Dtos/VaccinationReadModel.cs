namespace PetCare.Application.Features.Vaccinations.Dtos
{
    public class VaccinationReadModel
    {
        public int VaccinationId { get; set; }
        public string VaccineName { get; set; } = string.Empty;
        public DateOnly VaccinationDate { get; set; }
        public DateOnly? NextDueDate { get; set; }
        public int PetId { get; set; }
        public int AppointmentId { get; set; }
    }
}
