using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.PageTexts.Queries
{
    public record GetPageTextByIdQuery(int Id) : IRequest<PageText?>;

    public class GetPageTextByIdHandler : IRequestHandler<GetPageTextByIdQuery, PageText?>
    {
        private readonly IApplicationDbContext _context;

        public GetPageTextByIdHandler(IApplicationDbContext context) => _context = context;

        public async Task<PageText?> Handle(GetPageTextByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.PageTexts.FirstOrDefaultAsync(x => x.PageTextId == request.Id, cancellationToken);
        }
    }
}
