namespace PetCare.Application.Features.Vaccinations.Dtos
{
    public class VaccinationCreateModel
    {
        public string VaccineName { get; set; } = string.Empty;
        public DateOnly VaccinationDate { get; set; }
        public DateOnly? NextDueDate { get; set; }
        public int PetId { get; set; }
        public int AppointmentId { get; set; }
    }
}
