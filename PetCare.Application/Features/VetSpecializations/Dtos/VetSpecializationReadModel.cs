using PetCare.Application.Features.Procedures.Dtos;

namespace PetCare.Application.Features.VetSpecializations.Dtos
{
    public class VetSpecializationReadModel
    {
        public int VetSpecializationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProcedureListItemDto> Procedures { get; set; } = new();
    }
}