namespace PetCare.MobileApp.Models
{
    public class ProcedureReadModel
    {
        public int ProcedureId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int VetSpecializationId { get; set; }
        public string VetSpecializationName { get; set; } = string.Empty;
    }
}
