using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Pets.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Pets.Queries
{
    public class GetAllPetsByOwnerQuery : IRequest<List<PetReadModel>>
    {
        public int? PetOwnerId { get; }
        public GetAllPetsByOwnerQuery(int? petOwnerId = null) => PetOwnerId = petOwnerId;
    }

    public class GetAllPetsByOwnerHandler : IRequestHandler<GetAllPetsByOwnerQuery, List<PetReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPetsByOwnerHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PetReadModel>> Handle(GetAllPetsByOwnerQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Pets
                .Include(p => p.PetOwner)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (request.PetOwnerId.HasValue)
            {
                query = query.Where(p => p.PetOwnerId == request.PetOwnerId.Value);
            }

            var list = await query.Select(p => new PetReadModel
            {
                PetId = p.PetId,
                Name = p.Name,
                Species = p.Species,
                Breed = p.Breed,
                DateOfBirth = p.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                IsMale = p.IsMale,
                ImageUrl = p.ImageUrl,
                PetOwnerId = p.PetOwnerId,
                CreatedDate = p.CreatedDate,
                PetOwnerName = p.PetOwner.FirstName + " " + p.PetOwner.LastName
            }).ToListAsync(cancellationToken);

            return list;
        }
    }
}
