namespace PetCare.Application.Features.Appointments.Dtos
{
    public class AppointmentProcedureReadModel
    {
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; } = string.Empty;
        public decimal FinalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
