using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vets.Queries
{
    public class VetLookupDto
    {
        public int VetId { get; set; }
        public string FullName { get; set; }
    }

    public class GetVetsLookupQuery : IRequest<List<VetLookupDto>>
    {
    }

    public class GetVetsLookupHandler : IRequestHandler<GetVetsLookupQuery, List<VetLookupDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetVetsLookupHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<VetLookupDto>> Handle(GetVetsLookupQuery request, CancellationToken cancellationToken)
        {
            return await _context.Vets
                .AsNoTracking()
                .OrderBy(v => v.LastName)
                .Select(v => new VetLookupDto
                {
                    VetId = v.VetId,
                    FullName = $"{v.FirstName} {v.LastName}"
                })
                .ToListAsync(cancellationToken);
        }
    }
}
