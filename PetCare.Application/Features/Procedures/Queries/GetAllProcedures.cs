using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Procedures.Queries
{
    public class GetAllProceduresQuery : IRequest<List<ProcedureReadModel>>
    {
        public GetAllProceduresQuery() { }

        public GetAllProceduresQuery(int? vetSpecializationId = null, bool? isActive = null)
        {
            VetSpecializationId = vetSpecializationId;
            IsActive = isActive;
        }

        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; } = "Name";
        public string? SortDirection { get; set; } = "asc";
        public int? VetSpecializationId { get; init; }
        public bool? IsActive { get; set; }
    }

    public class GetAllProceduresHandler : IRequestHandler<GetAllProceduresQuery, List<ProcedureReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProceduresHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProcedureReadModel>> Handle(GetAllProceduresQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Procedures
                .Include(p => p.VetSpecialization)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term));
            }

            if (request.VetSpecializationId.HasValue)
                query = query.Where(p => p.VetSpecializationId == request.VetSpecializationId.Value);

            if (request.IsActive.HasValue)
                query = query.Where(p => p.IsActive == request.IsActive.Value);

            bool isDesc = request.SortDirection?.ToLower() == "desc";

            query = request.SortColumn switch
            {
                "Price" => isDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "Specialization" => isDesc ? query.OrderByDescending(p => p.VetSpecialization.Name) : query.OrderBy(p => p.VetSpecialization.Name),
                "Status" => isDesc ? query.OrderByDescending(p => p.IsActive) : query.OrderBy(p => p.IsActive),
                _ => isDesc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)
            };

            return await query
                .Select(p => new ProcedureReadModel
                {
                    ProcedureId = p.ProcedureId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    IsActive = p.IsActive,
                    VetSpecializationId = p.VetSpecializationId,
                    VetSpecializationName = p.VetSpecialization.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}