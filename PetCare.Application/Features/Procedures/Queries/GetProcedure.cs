using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Procedures.Queries
{
    public class GetProcedureQuery : IRequest<ProcedureReadModel>
    {
        public GetProcedureQuery(int id) => ProcedureId = id;
        public int ProcedureId { get; }
    }

    public class GetProcedureHandler : IRequestHandler<GetProcedureQuery, ProcedureReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetProcedureHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProcedureReadModel> Handle(GetProcedureQuery request, CancellationToken cancellationToken)
        {
            var proc = await _context.Procedures
                .Where(p => p.ProcedureId == request.ProcedureId)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (proc == null)
            {
                throw new NotFoundException($"Procedure with id {request.ProcedureId} not found.");
            }

            return proc;
        }
    }
}