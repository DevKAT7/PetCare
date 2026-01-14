using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.PetOwners.Dto;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PetOwners.Queries
{
    public class GetPetOwnerQuery : IRequest<PetOwnerReadModel>
    {
        public int PetOwnerId { get; }
        public GetPetOwnerQuery(int petOwnerId)
        {
            PetOwnerId = petOwnerId;
        }
    }

    public class GetPetOwnerHandler : IRequestHandler<GetPetOwnerQuery, PetOwnerReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetPetOwnerHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PetOwnerReadModel> Handle(GetPetOwnerQuery request, CancellationToken cancellationToken)
        {
            var owner = await _context.PetOwners
                .Include(o => o.Pets)
                .Where(o => o.PetOwnerId == request.PetOwnerId && o.IsActive)
                .Select(o => new PetOwnerReadModel
                {
                    PetOwnerId = o.PetOwnerId,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    Address = o.Address,
                    PhoneNumber = o.PhoneNumber,
                    PetNames = o.Pets.Select(p => p.Name).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (owner == null)
            {
                throw new NotFoundException($"Nie znaleziono właściciela o ID: {request.PetOwnerId}");
            }

            return owner;
        }
    }
}
