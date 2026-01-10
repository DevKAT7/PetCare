using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Pets.Dto;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Pets.Queries
{
    public class GetAllPetsQuery : IRequest<List<PetReadModel>>
    {
        public int? PetOwnerId { get; }
        public GetAllPetsQuery(int? petOwnerId = null) => PetOwnerId = petOwnerId;
    }

    public class GetAllPetsHandler : IRequestHandler<GetAllPetsQuery, List<PetReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPetsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PetReadModel>> Handle(GetAllPetsQuery request, CancellationToken cancellationToken)
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
                PetOwnerName = p.PetOwner.FirstName + " " + p.PetOwner.LastName
            }).ToListAsync(cancellationToken);

            return list;
        }
    }
}
