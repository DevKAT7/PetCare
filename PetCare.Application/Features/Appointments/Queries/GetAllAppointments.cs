using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Common;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetAllAppointmentsQuery : IRequest<PaginatedResult<AppointmentReadModel>>
    {
        public int? PetOwnerId { get; }
        public int? VetId { get; }
        public DateTime? From { get; }
        public DateTime? To { get; }
        public AppointmentStatus? Status { get; }
        public string? PetName { get; }
        public string? OwnerName { get; }

        public string SortColumn { get; }
        public string SortDirection { get; }
        public int PageIndex { get; }
        public int PageSize { get; }

        public GetAllAppointmentsQuery(string? petName = null, string? ownerName = null,
            int? vetId = null, int? petOwnerId = null, DateTime? from = null, DateTime? to = null,
            AppointmentStatus? status = null, string sortColumn = "Date", string sortDirection = "desc",
            int pageSize = 14, int pageIndex = 1)
        {
            PetName = petName;
            OwnerName = ownerName;
            VetId = vetId;
            PetOwnerId = petOwnerId;
            From = from;
            To = to;
            Status = status;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}

public class GetAllAppointmentsHandler : IRequestHandler<GetAllAppointmentsQuery, PaginatedResult<AppointmentReadModel>>
{
    private readonly IApplicationDbContext _context;

    public GetAllAppointmentsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<AppointmentReadModel>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Appointments
            .Include(a => a.Pet)
                .ThenInclude(p => p.PetOwner)
            .Include(a => a.Vet)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.PetName))
        {
            query = query.Where(a => a.Pet.Name.Contains(request.PetName));
        }

        if (!string.IsNullOrEmpty(request.OwnerName))
        {
            query = query.Where(a => a.Pet.PetOwner.LastName.Contains(request.OwnerName)
                                  || a.Pet.PetOwner.FirstName.Contains(request.OwnerName));
        }

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

        bool isAsc = request.SortDirection?.ToLower() == "asc";

        query = request.SortColumn switch
        {
            "Patient" => isAsc ? query.OrderBy(x => x.Pet.Name) : query.OrderByDescending(x => x.Pet.Name),
            "Owner" => isAsc ? query.OrderBy(x => x.Pet.PetOwner.LastName) : query.OrderByDescending(x => x.Pet.PetOwner.LastName),
            "Vet" => isAsc ? query.OrderBy(x => x.Vet.LastName) : query.OrderByDescending(x => x.Vet.LastName),
            "Status" => isAsc ? query.OrderBy(x => x.Status) : query.OrderByDescending(x => x.Status),
            "Date" or _ => isAsc ? query.OrderBy(x => x.AppointmentDateTime) : query.OrderByDescending(x => x.AppointmentDateTime)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var list = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AppointmentReadModel
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
                OwnerName = a.Pet.PetOwner.FirstName + " " + a.Pet.PetOwner.LastName,
                VetId = a.VetId,
                VetName = a.Vet.FirstName + " " + a.Vet.LastName
            }).ToListAsync(cancellationToken);

        return new PaginatedResult<AppointmentReadModel>(list, totalCount, request.PageIndex, request.PageSize);
    }
}
