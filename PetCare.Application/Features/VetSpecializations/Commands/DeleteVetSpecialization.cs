using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSpecializations.Commands
{
    public class DeleteVetSpecializationCommand : IRequest<int>
    {
        public DeleteVetSpecializationCommand(int id) => VetSpecializationId = id;
        public int VetSpecializationId { get; }
    }

    public class DeleteVetSpecializationHandler : IRequestHandler<DeleteVetSpecializationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public DeleteVetSpecializationHandler(ApplicationDbContext context)
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

            _context.VetSpecializations.Remove(spec);
            await _context.SaveChangesAsync(cancellationToken);

            return spec.VetSpecializationId;
        }
    }
}