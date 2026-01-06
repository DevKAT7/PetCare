using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Pets.Queries
{
    public class PetLookupDto
    {
        public int PetId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public class GetPetsForLookupQuery : IRequest<List<PetLookupDto>> { }

    public class GetPetsForLookupHandler : IRequestHandler<GetPetsForLookupQuery, List<PetLookupDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetPetsForLookupHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PetLookupDto>> Handle(GetPetsForLookupQuery request, CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Include(p => p.PetOwner)
                .OrderBy(p => p.Name)
                .Select(p => new PetLookupDto
                {
                    PetId = p.PetId,
                    DisplayName = $"{p.Name} ({p.PetOwner.FirstName} {p.PetOwner.LastName})"
                })
                .ToListAsync(cancellationToken);
        }
    }
}
