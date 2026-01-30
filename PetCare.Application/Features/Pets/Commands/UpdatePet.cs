using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Shared.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Pets.Commands
{
    public class UpdatePetCommand : IRequest<int>
    {
        public int PetId { get; }
        public PetUpdateModel Pet { get; }

        public UpdatePetCommand(int petId, PetUpdateModel pet)
        {
            PetId = petId;
            Pet = pet;
        }
    }

    public class UpdatePetHandler : IRequestHandler<UpdatePetCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePetHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.PetId == request.PetId, cancellationToken);

            if (pet == null)
            {
                throw new NotFoundException("Pet", request.PetId);
            }

            pet.Name = request.Pet.Name;
            pet.Species = request.Pet.Species;
            pet.Breed = request.Pet.Breed;
            pet.DateOfBirth = DateOnly.FromDateTime(request.Pet.DateOfBirth);
            pet.IsMale = request.Pet.IsMale;
            pet.ImageUrl = request.Pet.ImageUrl;
            pet.PetOwnerId = request.Pet.PetOwnerId;

            await _context.SaveChangesAsync(cancellationToken);

            return pet.PetId;
        }
    }
}
