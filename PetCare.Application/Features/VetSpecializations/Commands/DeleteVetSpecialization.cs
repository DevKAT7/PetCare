using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSpecializations.Commands
{
    public class DeleteVetSpecializationCommand : IRequest<int>
    {
        public DeleteVetSpecializationCommand(int id) => VetSpecializationId = id;
        public int VetSpecializationId { get; }
    }

    public class DeleteVetSpecializationHandler : IRequestHandler<DeleteVetSpecializationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteVetSpecializationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteVetSpecializationCommand request, CancellationToken cancellationToken)
        {
            var spec = await _context.VetSpecializations
                .Include(s => s.Procedures)
                .FirstOrDefaultAsync(s => s.VetSpecializationId == request.VetSpecializationId, cancellationToken);

            if (spec == null)
            {
                throw new NotFoundException("VetSpecialization", request.VetSpecializationId);
            }

            if (spec.Procedures.Any())
            {
                throw new Exception($"Cannot delete '{spec.Name}' because it contains {spec.Procedures.Count} " +
                    $"linked procedures. Please remove or reassign them first.");
            }

            _context.VetSpecializations.Remove(spec);
            await _context.SaveChangesAsync(cancellationToken);

            return spec.VetSpecializationId;
        }
    }
}