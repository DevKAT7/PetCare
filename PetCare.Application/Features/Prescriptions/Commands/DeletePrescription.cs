using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Prescriptions.Commands
{
    public class DeletePrescriptionCommand : IRequest<int>
    {
        public int Id { get; }
        public DeletePrescriptionCommand(int id) => Id = id;
    }

    public class DeletePrescriptionHandler : IRequestHandler<DeletePrescriptionCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public DeletePrescriptionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(p => p.PrescriptionId == request.Id, cancellationToken);

            if (prescription == null)
            {
                throw new NotFoundException("Prescription not found.");
            }

            _context.Prescriptions.Remove(prescription);

            await _context.SaveChangesAsync(cancellationToken);

            return prescription.PrescriptionId;
        }
    }
}
