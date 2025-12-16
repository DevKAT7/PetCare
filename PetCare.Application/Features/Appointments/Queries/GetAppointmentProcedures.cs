using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetAppointmentProceduresQuery : IRequest<List<AppointmentProcedureReadModel>>
    {
        public GetAppointmentProceduresQuery(int appointmentId) => AppointmentId = appointmentId;
        public int AppointmentId { get; }
    }

    public class GetAppointmentProceduresHandler : IRequestHandler<GetAppointmentProceduresQuery, List<AppointmentProcedureReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAppointmentProceduresHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentProcedureReadModel>> Handle(GetAppointmentProceduresQuery request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentProcedures
                .Where(ap => ap.AppointmentId == request.AppointmentId)
                .Select(ap => new AppointmentProcedureReadModel
                {
                    ProcedureId = ap.ProcedureId,
                    ProcedureName = ap.Procedure.Name,
                    FinalPrice = ap.FinalPrice,
                    Quantity = ap.Quantity,
                    TotalPrice = ap.FinalPrice * ap.Quantity
                })
                .ToListAsync(cancellationToken);
        }
    }
}
