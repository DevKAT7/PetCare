namespace PetCare.Application.Features.Procedures.Dtos
{
    public class ProcedureCreateModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VetSpecializationId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
