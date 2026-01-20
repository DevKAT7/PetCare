using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Invoices.Queries
{
    public class InvoicePrepareDto
    {
        public int AppointmentId { get; set; }
        public int PetOwnerId { get; set; }
        public string PetOwnerName { get; set; } = string.Empty;
        public List<InvoiceItemCreateModel> SuggestedItems { get; set; } = new();
    }

    public class GetInvoicePrepareModelQuery : IRequest<InvoicePrepareDto>
    {
        public int AppointmentId { get; set; }
        public GetInvoicePrepareModelQuery(int appointmentId) => AppointmentId = appointmentId;
    }

    public class GetInvoicePrepareModelHandler : IRequestHandler<GetInvoicePrepareModelQuery, InvoicePrepareDto>
    {
        private readonly IApplicationDbContext _context;

        public GetInvoicePrepareModelHandler(IApplicationDbContext context) => _context = context;

        public async Task<InvoicePrepareDto> Handle(GetInvoicePrepareModelQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Pet).ThenInclude(p => p.PetOwner)
                .Include(a => a.AppointmentProcedures).ThenInclude(ap => ap.Procedure)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null) throw new NotFoundException("Appointment", request.AppointmentId);

            var items = appointment.AppointmentProcedures.Select(ap => new InvoiceItemCreateModel
            {
                Description = ap.Procedure.Name,
                Quantity = ap.Quantity,
                UnitPrice = ap.FinalPrice
            }).ToList();

            return new InvoicePrepareDto
            {
                AppointmentId = appointment.AppointmentId,
                PetOwnerId = appointment.Pet.PetOwnerId,
                PetOwnerName = $"{appointment.Pet.PetOwner.FirstName} {appointment.Pet.PetOwner.LastName}",
                SuggestedItems = items
            };
        }
    }
}
