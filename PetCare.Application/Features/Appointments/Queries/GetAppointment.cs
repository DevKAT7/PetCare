using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetAppointmentQuery : IRequest<AppointmentReadModel>
    {
        public int AppointmentId { get; }
        public GetAppointmentQuery(int appointmentId) => AppointmentId = appointmentId;
    }

    public class GetAppointmentHandler : IRequestHandler<GetAppointmentQuery, AppointmentReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetAppointmentHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentReadModel> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var summary = await _context.AppointmentSummaryViews
                        .FirstOrDefaultAsync(v => v.AppointmentId == request.AppointmentId, cancellationToken);

            if (summary == null)
            {
                throw new NotFoundException("Appointment", request.AppointmentId);
            }

            var procedures = await _context.AppointmentProcedures
                .Include(ap => ap.Procedure)
                .Where(ap => ap.AppointmentId == request.AppointmentId)
                .Select(ap => new AppointmentProcedureReadModel
                {
                    ProcedureId = ap.ProcedureId,
                    ProcedureName = ap.Procedure.Name,
                    Quantity = ap.Quantity,
                    FinalPrice = ap.FinalPrice,
                    TotalPrice = ap.FinalPrice * ap.Quantity
                }).ToListAsync(cancellationToken);

            Enum.TryParse<Core.Enums.AppointmentStatus>(summary.Status, out var statusEnum);

            return new AppointmentReadModel
            {
                AppointmentId = summary.AppointmentId,
                AppointmentDateTime = summary.AppointmentDateTime,
                Description = summary.Description,
                Status = statusEnum,
                ReasonForVisit = summary.ReasonForVisit!,
                Diagnosis = summary.Diagnosis,
                Notes = summary.Notes,
                PetId = summary.PetId,
                PetName = summary.PetName!,
                PetSpecies = summary.PetSpecies!,
                PetImageUrl = summary.PetImageUrl,
                OwnerName = summary.OwnerName!,
                VetId = summary.VetId,
                VetName = summary.VetName!,
                InvoiceId = summary.InvoiceId,
                Procedures = procedures
            };
        }
    }
}
