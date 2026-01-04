using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vets.Commands
{
    public class CreateVetCommand : IRequest<int>
    {
        public string Email { get; set; } = string.Empty;
        // Hasło tymczasowe, które nada administrator
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Pesel { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public DateTime CareerStartDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new List<int>();
    }

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

                throw new BadRequestException("Unable to create user account.", errors);
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
