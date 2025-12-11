using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vets.Commands
{
    public class CreateVetHandler : IRequestHandler<CreateVetCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateVetHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(CreateVetCommand request, CancellationToken cancellationToken)
        {
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

            await _userManager.AddToRoleAsync(user, "Employee");

            var vet = new Vet
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Pesel = request.Pesel,
                LicenseNumber = request.LicenseNumber,
                HireDate = request.HireDate,
                CareerStartDate = request.CareerStartDate,
                Address = request.Address,
                ProfilePictureUrl = request.ProfilePictureUrl,
                Description = request.Description,
                IsActive = true,
                SpecializationLinks = new List<VetSpecializationLink>()
            };


            if (request.SpecializationIds != null && request.SpecializationIds.Any())
            {
                var existingSpecializations = await _context.VetSpecializations
                    .Where(s => request.SpecializationIds.Contains(s.VetSpecializationId))
                    .ToListAsync(cancellationToken);

                foreach (var specialization in existingSpecializations)
                {
                    vet.SpecializationLinks.Add(new VetSpecializationLink
                    {
                        VetSpecializationId = specialization.VetSpecializationId
                    });
                }
            }

            _context.Vets.Add(vet);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return vet.VetId;
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }
    }
}
