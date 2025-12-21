using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vets.Commands
{
    public class UpdateVetCommand : IRequest<int>
    {
        public int VetId { get; }
        public VetUpdateModel Vet { get; set; } = null!;

        public UpdateVetCommand(int vetId, VetUpdateModel vetDto)
        {
            VetId = vetId;
            Vet = vetDto;
        }
    }

    public class UpdateVetHandler : IRequestHandler<UpdateVetCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UpdateVetHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(UpdateVetCommand command, CancellationToken cancellationToken)
        {
            var request = command.Vet;

            var vet = await _context.Vets
                .Include(v => v.User)
                .Include(v => v.SpecializationLinks)
                .FirstOrDefaultAsync(v => v.VetId == command.VetId, cancellationToken);

            if (vet == null)
            {
                throw new NotFoundException("Vet", command.VetId);
            }

            vet.FirstName = request.FirstName;
            vet.LastName = request.LastName;
            vet.Address = request.Address;
            vet.ProfilePictureUrl = request.ProfilePictureUrl;
            vet.Description = request.Description;

            var user = vet.User;
            bool userChanged = false;

            if (user.Email != request.Email)
            {
                await _userManager.SetEmailAsync(user, request.Email);
                await _userManager.SetUserNameAsync(user, request.Email);
                userChanged = true;
            }
            if (user.PhoneNumber != request.PhoneNumber)
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

            vet.SpecializationLinks.Clear();

            if (request.SpecializationIds != null && request.SpecializationIds.Any())
            {
                foreach (var specId in request.SpecializationIds.Distinct())
                {
                    vet.SpecializationLinks.Add(new VetSpecializationLink
                    {
                        VetSpecializationId = specId
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return vet.VetId;
        }
    }
}
