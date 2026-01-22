using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.PageTexts.Queries
{
    public record GetAllPageTextsForAdminQuery : IRequest<List<PageText>>;

    public class GetAllPageTextsForAdminHandler : IRequestHandler<GetAllPageTextsForAdminQuery, List<PageText>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPageTextsForAdminHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<PageText>> Handle(GetAllPageTextsForAdminQuery request, CancellationToken cancellationToken)
        {
            return await _context.PageTexts
                .OrderBy(t => t.Key)
                .ToListAsync(cancellationToken);
        }
    }
}
