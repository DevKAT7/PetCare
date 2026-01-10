using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Pets.Commands
{
    public class DeletePetCommand : IRequest<int>
    {
        public int PetId { get; }
        public DeletePetCommand(int petId) => PetId = petId;
    }

    public class DeletePetHandler : IRequestHandler<DeletePetCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeletePetHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeletePetCommand request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.PetId == request.PetId, cancellationToken);

            if (pet == null)
            {
                throw new NotFoundException("Pet", request.PetId);
            }

            pet.IsActive = false;

            await _context.SaveChangesAsync(cancellationToken);

            return pet.PetId;
        }
    }
}
