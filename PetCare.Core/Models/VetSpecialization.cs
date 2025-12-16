using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class VetSpecialization
    {
        [Key]
        public int VetSpecializationId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
        public ICollection<VetSpecializationLink>? VetLinks { get; set; }
    }
}
