using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vets.Queries
{
    public class GetVetStatisticsQuery : IRequest<VetStatisticsDto>
    {
        public int VetId { get; set; }
    }

    public class GetVetStatisticsHandler : IRequestHandler<GetVetStatisticsQuery, VetStatisticsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetVetStatisticsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetStatisticsDto> Handle(GetVetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var totalAppointments = await _context.Appointments
                .CountAsync(a => a.VetId == request.VetId, cancellationToken);

            var monthAppointments = await _context.Appointments
                .CountAsync(a => a.VetId == request.VetId && a.AppointmentDateTime >= startOfMonth, cancellationToken);

            var uniquePatients = await _context.Appointments
                .Where(a => a.VetId == request.VetId)
                .Select(a => a.PetId)
                .Distinct()
                .CountAsync(cancellationToken);

            var revenue = await _context.Invoices
                .Where(i => i.Appointment.VetId == request.VetId)
                .SumAsync(i => i.TotalAmount, cancellationToken);

            return new VetStatisticsDto
            {
                TotalAppointments = totalAppointments,
                AppointmentsThisMonth = monthAppointments,
                UniquePatients = uniquePatients,
                TotalRevenue = revenue
            };
        }
    }
}
