using MediatR;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vets.Commands
{
    public class CreateVetHandler : IRequestHandler<CreateVetCommand, int>
    {
        private readonly ApplicationDbContext _context;
        public CreateVetHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateVetCommand request, CancellationToken cancellationToken)
        {
            var vet = new Vet
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Pesel = request.Pesel,
                LicenseNumber = request.LicenseNumber,
                HireDate = request.HireDate,
                CareerStartDate = request.CareerStartDate,
                Address = request.Address,
                ProfilePictureUrl = request.ProfilePictureUrl,
                Description = request.Description,
                IsActive = true
            };

            foreach (var specId in request.SpecializationIds)
            {
                vet.SpecializationLinks.Add(new VetSpecializationLink
                {
                    VetSpecializationId = specId
                });
            }
            _context.Vets.Add(vet);
            await _context.SaveChangesAsync(cancellationToken);
            return vet.VetId;
        }
    }
}
