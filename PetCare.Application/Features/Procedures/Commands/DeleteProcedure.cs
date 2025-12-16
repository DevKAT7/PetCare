using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Procedures.Commands
{
    public class DeleteProcedureCommand : IRequest<int>
    {
        public DeleteProcedureCommand(int id) => ProcedureId = id;
        public int ProcedureId { get; }
    }

    public class DeleteProcedureHandler : IRequestHandler<DeleteProcedureCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public DeleteProcedureHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteProcedureCommand request, CancellationToken cancellationToken)
        {
            var procedure = await _context.Procedures
                .FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId, cancellationToken);

            if (procedure == null)
            {
                throw new NotFoundException("Procedure", request.ProcedureId);
            }

            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync(cancellationToken);

            return procedure.ProcedureId;
        }
    }
}