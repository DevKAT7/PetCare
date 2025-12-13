using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vets.Commands
{
    public class DeleteVetCommand : IRequest<int>
    {
        public int VetId { get; }
        public DeleteVetCommand(int vetId)
        {
            VetId = vetId;
        }

        public class DeleteVetHandler : IRequestHandler<DeleteVetCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<User> _userManager;

            public DeleteVetHandler(ApplicationDbContext context, UserManager<User> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<int> Handle(DeleteVetCommand command, CancellationToken cancellationToken)
            {
                var vet = await _context.Vets
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(v => v.VetId == command.VetId, cancellationToken);

                if (vet == null)
                {
                    throw new NotFoundException("Vet", command.VetId);
                }

                vet.IsActive = false;
                vet.User.IsActive = false;

                await _userManager.UpdateAsync(vet.User);

                await _context.SaveChangesAsync(cancellationToken);

                return vet.VetId;
            }
        }
    }
}
