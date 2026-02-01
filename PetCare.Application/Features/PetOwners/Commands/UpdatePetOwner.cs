using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.PetOwners.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.PetOwners.Commands
{
    public class UpdatePetOwnerCommand : IRequest<int>
    {
        public int PetOwnerId { get; }
        public PetOwnerUpdateModel PetOwner { get; set; } = null!;

        public UpdatePetOwnerCommand(int petOwnerId, PetOwnerUpdateModel petOwnerDto)
        {
            PetOwnerId = petOwnerId;
            PetOwner = petOwnerDto;
        }
    }

    public class UpdatePetOwnerHandler : IRequestHandler<UpdatePetOwnerCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UpdatePetOwnerHandler(IApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(UpdatePetOwnerCommand command, CancellationToken cancellationToken)
        {
            var request = command.PetOwner;

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {           
                var owner = await _context.PetOwners
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.PetOwnerId == command.PetOwnerId, cancellationToken);

                if (owner == null)
                {
                    throw new NotFoundException("PetOwner", command.PetOwnerId);
                }

                owner.FirstName = request.FirstName;
                owner.LastName = request.LastName;
                owner.Address = request.Address;
                owner.PhoneNumber = request.PhoneNumber;

                var user = owner.User;
                bool userChanged = false;

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && user.PhoneNumber != request.PhoneNumber)
                {
                    await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                    userChanged = true;
                }

                if (userChanged)
                {
                    var identityResult = await _userManager.UpdateAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        throw new Exception("User data update error: " + string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return owner.PetOwnerId;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
