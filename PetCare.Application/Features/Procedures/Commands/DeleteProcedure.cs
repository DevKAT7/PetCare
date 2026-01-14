using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Procedures.Commands
{
    public class DeleteProcedureCommand : IRequest<int>
    {
        public DeleteProcedureCommand(int id) => ProcedureId = id;
        public int ProcedureId { get; }
    }

    public class DeleteProcedureHandler : IRequestHandler<DeleteProcedureCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProcedureHandler(IApplicationDbContext context)
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

            if (!procedure.IsActive)
            {
                throw new Exception("This procedure is already archived/inactive.");
            }

            bool isUsedInAppointments = await _context.AppointmentProcedures
            .AnyAsync(ap => ap.ProcedureId == request.ProcedureId, cancellationToken);

            if (isUsedInAppointments)
            {
                procedure.IsActive = false;

                procedure.Name += " (Archived)"; 
            }
            else
            {
                _context.Procedures.Remove(procedure);
            }
            await _context.SaveChangesAsync(cancellationToken);

            return procedure.ProcedureId;
        }
    }
}