using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Pets.Queries
{
    public class GetUniqueSpeciesQuery : IRequest<List<string>> { }

    public class GetUniqueSpeciesHandler : IRequestHandler<GetUniqueSpeciesQuery, List<string>>
    {
        private readonly IApplicationDbContext _context;

        public GetUniqueSpeciesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> Handle(GetUniqueSpeciesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Where(p => p.IsActive)
                .Select(p => p.Species)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);
        }
    }
}
