namespace PetCare.Core.Models
{
    public class Procedure
    {
        public int ProcedureId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public int VetSpecializationId { get; set; }
        public VetSpecialization VetSpecialization { get; set; } = null!;
        public ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();
    }
}
