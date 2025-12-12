using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Infrastructure.Data;
namespace PetCare.Application.Features.Vets.Queries
{
    public class GetAllVetsQuery : IRequest<List<VetReadModel>>
    {
    }

    public class GetAllVetsHandler : IRequestHandler<GetAllVetsQuery, List<VetReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllVetsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VetReadModel>> Handle(GetAllVetsQuery request, CancellationToken cancellationToken)
        {
            var vets = await _context.Vets
                .Where(v => v.IsActive)
                .Select(vet => new VetReadModel
                {
                    VetId = vet.VetId,
                    FirstName = vet.FirstName,
                    LastName = vet.LastName,
                    ProfilePictureUrl = vet.ProfilePictureUrl,
                    Description = vet.Description,
                    YearsOfExperience = vet.YearsOfExperience,
                    PhoneNumber = vet.User.PhoneNumber,
                    Specializations = vet.SpecializationLinks
                        .Select(link => link.VetSpecialization.Name)
                        .ToList()
                }).ToListAsync(cancellationToken);

            return vets;
        }
    }
}
