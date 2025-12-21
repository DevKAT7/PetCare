using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSpecializations.Queries
{
    public class GetVetSpecializationQuery : IRequest<VetSpecializationReadModel>
    {
        public GetVetSpecializationQuery(int id) => VetSpecializationId = id;
        public int VetSpecializationId { get; }
    }

    public class GetVetSpecializationHandler : IRequestHandler<GetVetSpecializationQuery, VetSpecializationReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetVetSpecializationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetSpecializationReadModel> Handle(GetVetSpecializationQuery request, CancellationToken cancellationToken)
        {
            var spec = await _context.VetSpecializations
                .Where(s => s.VetSpecializationId == request.VetSpecializationId)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (spec == null) throw new NotFoundException($"Vet specialization with id {request.VetSpecializationId} not found.");

            return spec;
        }
    }
}
