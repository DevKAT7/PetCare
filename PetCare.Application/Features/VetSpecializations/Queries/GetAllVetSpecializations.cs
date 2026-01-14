using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSpecializations.Queries
{
    public class GetAllVetSpecializationsQuery : IRequest<List<VetSpecializationReadModel>> { }

    public class GetAllVetSpecializationsHandler : IRequestHandler<GetAllVetSpecializationsQuery, List<VetSpecializationReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllVetSpecializationsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VetSpecializationReadModel>> Handle(GetAllVetSpecializationsQuery request, CancellationToken cancellationToken)
        {
            return await _context.VetSpecializations
                .Select(s => new VetSpecializationReadModel
                {
                    VetSpecializationId = s.VetSpecializationId,
                    Name = s.Name,
                    Procedures = s.Procedures.Select(p => new ProcedureListItemDto
                    {
                        ProcedureId = p.ProcedureId,
                        Name = p.Name,
                        Price = p.Price,
                        IsActive = p.IsActive
                    }).ToList()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
