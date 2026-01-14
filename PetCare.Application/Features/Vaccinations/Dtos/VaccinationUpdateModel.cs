namespace PetCare.Application.Features.Vaccinations.Dtos
{
    public class VaccinationUpdateModel
    {
        public string VaccineName { get; set; } = string.Empty;
        public DateOnly? NextDueDate { get; set; }
    }
}
