using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Core.Enums;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetAllAppointmentsQuery : IRequest<List<AppointmentReadModel>>
    {
        public int? PetOwnerId { get; }
        public int? VetId { get; }
        public DateTime? From { get; }
        public DateTime? To { get; }
        public AppointmentStatus? Status { get; }

        public GetAllAppointmentsQuery(int? petOwnerId = null, int? vetId = null, DateTime? from = null, DateTime? to = null, AppointmentStatus? status = null)
        {
            PetOwnerId = petOwnerId;
            VetId = vetId;
            From = from;
            To = to;
            Status = status;
        }
    }

    public class GetAllAppointmentsHandler : IRequestHandler<GetAllAppointmentsQuery, List<AppointmentReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllAppointmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentReadModel>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                .AsQueryable();

            if (request.PetOwnerId.HasValue)
            {
                query = query.Where(a => a.Pet.PetOwnerId == request.PetOwnerId.Value);
            }

            if (request.VetId.HasValue)
            {
                query = query.Where(a => a.VetId == request.VetId.Value);
            }

            if (request.From.HasValue)
            {
                query = query.Where(a => a.AppointmentDateTime >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                query = query.Where(a => a.AppointmentDateTime <= request.To.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status.Value);
            }

            var list = await query.Select(a => new AppointmentReadModel
            {
                AppointmentId = a.AppointmentId,
                AppointmentDateTime = a.AppointmentDateTime,
                Description = a.Description,
                Status = a.Status,
                ReasonForVisit = a.ReasonForVisit,
                Diagnosis = a.Diagnosis,
                Notes = a.Notes,
                PetId = a.PetId,
                PetName = a.Pet.Name,
                VetId = a.VetId,
                VetName = a.Vet.FirstName + " " + a.Vet.LastName
            }).ToListAsync(cancellationToken);

            return list;
        }
    }
}
