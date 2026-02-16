using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.PageTexts.Queries
{
    public record GetAllPageTextsForAdminQuery(string? SearchTerm = null) : IRequest<List<PageText>>;

    public class GetAllPageTextsForAdminHandler : IRequestHandler<GetAllPageTextsForAdminQuery, List<PageText>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPageTextsForAdminHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<PageText>> Handle(GetAllPageTextsForAdminQuery request, CancellationToken cancellationToken)
        {
            var query = _context.PageTexts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();

                query = query.Where(t => t.Key.ToLower().Contains(term)
                                      || t.Value.ToLower().Contains(term));
            }

            return await query
                .OrderBy(t => t.Key)
                .ToListAsync(cancellationToken);
        }
    }
}
