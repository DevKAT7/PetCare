using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PageTexts.Queries
{
    public class GetAllPageTextsQuery : IRequest<Dictionary<string, string>>
    {
    }

    public class GetAllPageTextsHandler : IRequestHandler<GetAllPageTextsQuery, Dictionary<string, string>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPageTextsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> Handle(GetAllPageTextsQuery request, CancellationToken cancellationToken)
        {
            return await _context.PageTexts
                .ToDictionaryAsync(t => t.Key, t => t.Value, cancellationToken);
        }
    }
}
