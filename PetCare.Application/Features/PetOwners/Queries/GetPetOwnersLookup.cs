using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PetOwners.Queries
{
    public class PetOwnerLookupDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public class GetPetOwnersLookupQuery : IRequest<List<PetOwnerLookupDto>> { }

    public class GetPetOwnersLookupHandler : IRequestHandler<GetPetOwnersLookupQuery, List<PetOwnerLookupDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetPetOwnersLookupHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<PetOwnerLookupDto>> Handle(GetPetOwnersLookupQuery request, CancellationToken cancellationToken)
        {
            return await _context.PetOwners
                .Where(o => o.IsActive)
                .OrderBy(o => o.LastName)
                .Select(o => new PetOwnerLookupDto
                {
                    Id = o.PetOwnerId,
                    DisplayName = $"{o.LastName} {o.FirstName} ({o.PhoneNumber})"
                })
                .ToListAsync(cancellationToken);
        }
    }
}
