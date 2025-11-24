namespace PetCare.Core.Models
{
    public class Procedure
    {
        public int ProcedureId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public int VetSpezializationId { get; set; }
        public VetSpezialization VetSpezialization { get; set; } = null!;
    }
}
