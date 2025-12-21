using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Pets.Dto;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Pets.Queries
{
    public class GetPetQuery : IRequest<PetReadModel>
    {
        public int PetId { get; }
        public GetPetQuery(int petId) => PetId = petId;
    }

    public class GetPetHandler : IRequestHandler<GetPetQuery, PetReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetPetHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PetReadModel> Handle(GetPetQuery request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets
                .Include(p => p.PetOwner)
                .FirstOrDefaultAsync(p => p.PetId == request.PetId && p.IsActive, cancellationToken);

            if (pet == null)
            {
                throw new NotFoundException("Pet", request.PetId);
            }

            return new PetReadModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                DateOfBirth = pet.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                IsMale = pet.IsMale,
                ImageUrl = pet.ImageUrl,
                PetOwnerId = pet.PetOwnerId,
                PetOwnerName = $"{pet.PetOwner.FirstName} {pet.PetOwner.LastName}"
            };
        }
    }
}
