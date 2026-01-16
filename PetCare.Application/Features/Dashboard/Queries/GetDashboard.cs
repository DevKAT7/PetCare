using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Dashboard.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

namespace PetCare.Application.Features.Dashboard.Queries
{
    public class GetDashboardDataQuery : IRequest<DashboardDto>
    {
    }

    public class GetDashboardDataHandler : IRequestHandler<GetDashboardDataQuery, DashboardDto>
    {
        private readonly IApplicationDbContext _context;

        public GetDashboardDataHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var data = new DashboardDto();

            //dzisiejsze wizyty
            data.AppointmentsTodayCount = await _context.Appointments
                .CountAsync(a => a.AppointmentDateTime.Date == today, cancellationToken);

            data.PatientsInWaitingRoomCount = await _context.Appointments
                .CountAsync(a => a.AppointmentDateTime.Date == today &&
                                 (a.Status == AppointmentStatus.Scheduled || a.Status == AppointmentStatus.Confirmed), cancellationToken);

            //dzisiejszy przychód (z faktur wystawionych dzisiaj)
            data.TodayRevenue = await _context.Invoices
                .Where(i => i.InvoiceDate == DateOnly.FromDateTime(today))
                .SumAsync(i => i.TotalAmount, cancellationToken);

            //nowi pacjenci w tym miesiącu
            data.NewPatientsThisMonth = await _context.Pets
                .CountAsync(p => p.CreatedDate >= firstDayOfMonth, cancellationToken);

            data.UpcomingAppointments = await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                .Where(a => a.AppointmentDateTime >= DateTime.Now)
                .OrderBy(a => a.AppointmentDateTime)
                .Take(10)
                .Select(a => new DashboardAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    Time = a.AppointmentDateTime,
                    PetName = a.Pet.Name,
                    PetSpecies = a.Pet.Species,
                    VetName = $"{a.Vet.FirstName} {a.Vet.LastName}",
                    Status = a.Status
                })
                .ToListAsync(cancellationToken);

            var todayDateOnly = DateOnly.FromDateTime(today);
            data.OverdueInvoices = await _context.Invoices
                .Include(i => i.PetOwner)
                .Where(i => !i.IsPaid && i.DueDate < todayDateOnly)
                .OrderBy(i => i.DueDate)
                .Take(5)
                .Select(i => new DashboardOverdueInvoiceDto
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceNumber = i.InvoiceNumber,
                    OwnerName = $"{i.PetOwner.FirstName} {i.PetOwner.LastName}",
                    Amount = i.TotalAmount,
                    DueDate = i.DueDate
                })
                .ToListAsync(cancellationToken);

            data.CurrentUnpaidInvoices = await _context.Invoices
                .Include(i => i.PetOwner)
                .Where(i => !i.IsPaid && i.DueDate >= todayDateOnly)
                .OrderBy(i => i.DueDate)
                .Take(5)
                .Select(i => new DashboardOverdueInvoiceDto
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceNumber = i.InvoiceNumber,
                    OwnerName = $"{i.PetOwner.FirstName} {i.PetOwner.LastName}",
                    Amount = i.TotalAmount,
                    DueDate = i.DueDate
                })
                .ToListAsync(cancellationToken);

            data.LowStockMedications = await _context.StockItems
                .Include(s => s.Medication)
                .Where(s => s.CurrentStock <= s.ReorderLevel)
                .Take(5)
                .Select(s => new DashboardLowStockDto 
                { 
                    MedicationName = s.Medication.Name, 
                    Quantity = s.CurrentStock 
                })
                .ToListAsync(cancellationToken);
            

            return data;
        }
    }
}
