using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Shared.Dtos;

namespace PetCare.Application.Features.PetOwners.Queries
{
    public class GetPetOwnerByUserIdQuery : IRequest<PetOwnerAuthDto?>
    {
        public string UserId { get; set; }

        public GetPetOwnerByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetPetOwnerByUserIdHandler : IRequestHandler<GetPetOwnerByUserIdQuery, PetOwnerAuthDto?>
    {
        private readonly IApplicationDbContext _context;

        public GetPetOwnerByUserIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PetOwnerAuthDto?> Handle(GetPetOwnerByUserIdQuery request, CancellationToken cancellationToken)
        {
            var petOwner = await _context.PetOwners
                .Where(p => p.UserId == request.UserId)
                .Select(p => new PetOwnerAuthDto
                {
                    PetOwnerId = p.PetOwnerId,
                    FirstName = p.FirstName
                })
                .FirstOrDefaultAsync(cancellationToken);

            return petOwner;
        }
    }
}
