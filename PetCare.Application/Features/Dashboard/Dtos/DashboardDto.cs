using PetCare.Core.Enums;

namespace PetCare.Application.Features.Dashboard.Dtos
{
    public class DashboardDto
    {
        public int AppointmentsTodayCount { get; set; }
        public int PatientsInWaitingRoomCount { get; set; }
        public decimal TodayRevenue { get; set; }
        public int NewPatientsThisMonth { get; set; }

        public List<DashboardAppointmentDto> UpcomingAppointments { get; set; } = new();
        public List<DashboardOverdueInvoiceDto> OverdueInvoices { get; set; } = new();

        public List<DashboardLowStockDto> LowStockItems { get; set; } = new();
    }

    public class DashboardAppointmentDto
    {
        public int AppointmentId { get; set; }
        public DateTime Time { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string PetSpecies { get; set; } = string.Empty;
        public string VetName { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }
    }

    public class DashboardOverdueInvoiceDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly DueDate { get; set; }
    }

    public class DashboardLowStockDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
