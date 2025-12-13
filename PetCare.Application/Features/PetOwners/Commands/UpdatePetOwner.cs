using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.PetOwners.Dto;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.PetOwners.Commands
{
    public class UpdatePetOwnerCommand : IRequest<int>
    {
        public int PetOwnerId { get; }
        public PetOwnerUpdateModel PetOwnerDto { get; set; } = null!;

        public UpdatePetOwnerCommand(int petOwnerId, PetOwnerUpdateModel petOwnerDto)
        {
            PetOwnerId = petOwnerId;
            PetOwnerDto = petOwnerDto;
        }
    }

    public class UpdatePetOwnerHandler : IRequestHandler<UpdatePetOwnerCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UpdatePetOwnerHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(UpdatePetOwnerCommand command, CancellationToken cancellationToken)
        {
            var request = command.PetOwnerDto;

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

            var user = owner.User;
            bool userChanged = false;

            if (!string.IsNullOrWhiteSpace(request.Email) && user.Email != request.Email)
            {
                await _userManager.SetEmailAsync(user, request.Email);
                await _userManager.SetUserNameAsync(user, request.Email);
                userChanged = true;
            }

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
                    throw new Exception("Błąd aktualizacji danych użytkownika: " + string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return owner.PetOwnerId;
        }
    }
}
