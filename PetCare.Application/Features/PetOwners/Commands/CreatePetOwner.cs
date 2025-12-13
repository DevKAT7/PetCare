using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.PetOwners.Dto;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.PetOwners.Commands
{
    public class CreatePetOwnerCommand : IRequest<int>
    {
        public required PetOwnerCreateModel PetOwner { get; set; }
    }

    public class CreatePetOwnerHandler : IRequestHandler<CreatePetOwnerCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreatePetOwnerHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(CreatePetOwnerCommand command, CancellationToken cancellationToken)
        {
            var request = command.PetOwner;

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                IsActive = true,
                PhoneNumber = request.PhoneNumber
            };

            string temporaryPassword = request.Password;
            var identityResult = await _userManager.CreateAsync(user, temporaryPassword);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException("Nie udało się utworzyć konta użytkownika.", errors);
            }

            await _userManager.AddToRoleAsync(user, "Client");

            var owner = new PetOwner
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            _context.PetOwners.Add(owner);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return owner.PetOwnerId;
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }
    }
}
