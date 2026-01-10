using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Procedures.Queries
{
    public class GetAllProceduresQuery : IRequest<List<ProcedureReadModel>>
    {
        public GetAllProceduresQuery(int? vetSpecializationId = null, bool? isActive = null)
        {
            VetSpecializationId = vetSpecializationId;
            IsActive = isActive;
        }

        public int? VetSpecializationId { get; }
        public bool? IsActive { get; }
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
            var query = _context.Procedures.AsQueryable();

            if (request.VetSpecializationId.HasValue)
                query = query.Where(p => p.VetSpecializationId == request.VetSpecializationId.Value);

            if (request.IsActive.HasValue)
                query = query.Where(p => p.IsActive == request.IsActive.Value);

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