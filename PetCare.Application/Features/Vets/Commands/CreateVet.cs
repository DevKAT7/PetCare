using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Core.Models;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vets.Commands
{
    public class CreateVetCommand : IRequest<int>
    {
        public VetCreateModel Vet { get; set; } = new();
    }

    public class CreateVetHandler : IRequestHandler<CreateVetCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateVetHandler(IApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(CreateVetCommand command, CancellationToken cancellationToken)
        {
            var request = command.Vet;

            var validationErrors = new Dictionary<string, string[]>();

            if (await _context.Vets.AnyAsync(v => v.Pesel == request.Pesel, cancellationToken))
            {
                validationErrors.Add("Vet.Pesel", new[] { "PESEL is already in use by another vet." });
            }

            if (await _context.Vets.AnyAsync(v => v.LicenseNumber == request.LicenseNumber, cancellationToken))
            {
                validationErrors.Add("Vet.LicenseNumber", new[] { "License number is already registered in the system." });
            }

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                IsActive = true,
                PhoneNumber = request.PhoneNumber,
                RequirePasswordChange = true
            };

            string temporaryPassword = request.Password;
            var identityResult = await _userManager.CreateAsync(user, temporaryPassword);

            if (!identityResult.Succeeded)
            {
                var identityErrors = identityResult.Errors.Select(e => e.Description).ToList();

                throw new BadRequestException("Unable to create user account.", identityErrors);
            }

            await _userManager.AddToRoleAsync(user, "Employee");

            var vet = new Vet
            {
                UserId = user.Id,
                FirstName = request.FirstName!,
                LastName = request.LastName!,
                Pesel = request.Pesel!,
                LicenseNumber = request.LicenseNumber!,
                HireDate = DateOnly.FromDateTime(request.HireDate),
                CareerStartDate = DateOnly.FromDateTime(request.CareerStartDate),
                Address = request.Address!,
                ProfilePictureUrl = request.ProfilePictureUrl!,
                Description = request.Description!,
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
