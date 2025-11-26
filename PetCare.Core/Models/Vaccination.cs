namespace PetCare.Core.Models
{
    public class Vaccination
    {
        public int VaccinationId { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateOnly VaccinationDate { get; set; }
        public DateOnly? NextDueDate { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; } = null!;
        public int AppointmentId { get; set; }
    }
}
