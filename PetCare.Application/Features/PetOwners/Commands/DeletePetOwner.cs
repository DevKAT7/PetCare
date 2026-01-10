using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.PetOwners.Commands
{
    public class DeletePetOwnerCommand : IRequest<int>
    {
        public int PetOwnerId { get; }
        public DeletePetOwnerCommand(int petOwnerId)
        {
            PetOwnerId = petOwnerId;
        }
    }

    public class DeletePetOwnerHandler : IRequestHandler<DeletePetOwnerCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DeletePetOwnerHandler(IApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(DeletePetOwnerCommand command, CancellationToken cancellationToken)
        {
            var owner = await _context.PetOwners
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.PetOwnerId == command.PetOwnerId, cancellationToken);

            if (owner == null)
            {
                throw new NotFoundException("PetOwner", command.PetOwnerId);
            }

            owner.IsActive = false;
            owner.User.IsActive = false;

            await _userManager.UpdateAsync(owner.User);

            await _context.SaveChangesAsync(cancellationToken);

            return owner.PetOwnerId;
        }
    }
}
