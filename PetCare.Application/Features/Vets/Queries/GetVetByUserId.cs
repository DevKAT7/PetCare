using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Vets.Queries
{
    public record GetVetByUserIdQuery(string UserId) : IRequest<Vet?>;

    public class GetVetByUserIdHandler : IRequestHandler<GetVetByUserIdQuery, Vet?>
    {
        private readonly IApplicationDbContext _context;

        public GetVetByUserIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vet?> Handle(GetVetByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Vets
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.UserId == request.UserId, cancellationToken);
        }
    }
}
