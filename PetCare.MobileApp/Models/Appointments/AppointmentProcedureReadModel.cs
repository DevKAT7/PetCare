namespace PetCare.MobileApp.Models.Appointments
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
