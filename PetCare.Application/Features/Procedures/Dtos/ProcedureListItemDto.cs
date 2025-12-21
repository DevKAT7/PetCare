namespace PetCare.Application.Features.Procedures.Dtos
{
    public class ProcedureListItemDto
    {
        public int ProcedureId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
