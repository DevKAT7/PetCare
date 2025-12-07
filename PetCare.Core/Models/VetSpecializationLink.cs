namespace PetCare.Core.Models
{
    public class VetSpecializationLink
    {
        public int VetId { get; set; }
        public Vet Vet { get; set; } = null!;
        public int VetSpecializationId { get; set; }
        public VetSpecialization VetSpecialization { get; set; } = null!;
    }
}
