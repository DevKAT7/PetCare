namespace PetCare.Core.Models
{
    public class VetSpezialization
    {
        public int VetSpezializationId { get; set; }
        public string Name { get; set; } = null!;

        public List<Procedure> Procedures { get; set; } = new();
    }
}
