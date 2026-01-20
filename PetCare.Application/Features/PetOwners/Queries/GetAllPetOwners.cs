using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.PetOwners.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PetOwners.Queries
{
    public class GetAllPetOwnersQuery : IRequest<List<PetOwnerReadModel>>
    {
    }

    public class GetAllPetOwnersHandler : IRequestHandler<GetAllPetOwnersQuery, List<PetOwnerReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPetOwnersHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PetOwnerReadModel>> Handle(GetAllPetOwnersQuery request, CancellationToken cancellationToken)
        {
            var owners = await _context.PetOwners
                .Where(o => o.IsActive)
                .Select(o => new PetOwnerReadModel
                {
                    PetOwnerId = o.PetOwnerId,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    Address = o.Address,
                    PhoneNumber = o.PhoneNumber,
                    PetNames = o.Pets.Select(p => p.Name).ToList()
                })
                .ToListAsync(cancellationToken);

            return owners;
        }
    }
}
