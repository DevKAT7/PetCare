using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Medications.Commands
{
    public class DeleteMedicationCommand : IRequest<int>
    {
        public int Id { get; }
        public DeleteMedicationCommand(int id) => Id = id;
    }

    public class DeleteMedicationHandler : IRequestHandler<DeleteMedicationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteMedicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteMedicationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Medications.FirstOrDefaultAsync(m => m.MedicationId == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Medication not found.");
            }

            _context.Medications.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.MedicationId;
        }
    }
}
