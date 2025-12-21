namespace PetCare.Application.Features.Medications.Dtos
{
    public class MedicationReadModel
    {
        public int MedicationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
