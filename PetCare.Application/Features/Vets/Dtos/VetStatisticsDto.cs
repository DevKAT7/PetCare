namespace PetCare.Application.Features.Vets.Dtos
{
    public class VetStatisticsDto
    {
        public int TotalAppointments { get; set; }
        public int AppointmentsThisMonth { get; set; }
        public int UniquePatients { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
