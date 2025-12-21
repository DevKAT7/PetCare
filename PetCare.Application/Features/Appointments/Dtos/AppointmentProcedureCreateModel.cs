namespace PetCare.Application.Features.Appointments.Dtos
{
    public class AppointmentProcedureCreateModel
    {
        public int ProcedureId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? FinalPrice { get; set; }
    }
}
