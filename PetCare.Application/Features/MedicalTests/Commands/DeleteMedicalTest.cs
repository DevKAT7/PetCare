using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.MedicalTests.Commands
{
    public class DeleteMedicalTestCommand : IRequest<int>
    {
        public int Id { get; }
        public DeleteMedicalTestCommand(int id) => Id = id;
    }

    public class DeleteMedicalTestHandler : IRequestHandler<DeleteMedicalTestCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteMedicalTestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteMedicalTestCommand request, CancellationToken cancellationToken)
        {
            var medicalTest = await _context.MedicalTests
                .FirstOrDefaultAsync(m => m.MedicalTestId == request.Id, cancellationToken);

            if (medicalTest == null)
            {
                throw new NotFoundException("Medical test not found.");
            }

            _context.MedicalTests.Remove(medicalTest);

            await _context.SaveChangesAsync(cancellationToken);

            return medicalTest.MedicalTestId;
        }
    }
}
