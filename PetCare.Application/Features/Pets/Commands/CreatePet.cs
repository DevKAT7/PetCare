using MediatR;
using PetCare.Application.Features.Pets.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Pets.Commands
{
    public class CreatePetCommand : IRequest<int>
    {
        public required PetCreateModel Pet { get; set; }
    }

    public class CreatePetHandler : IRequestHandler<CreatePetCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePetHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePetCommand command, CancellationToken cancellationToken)
        {
            var model = command.Pet;

            var pet = new Pet
            {
                Name = model.Name,
                Species = model.Species,
                Breed = model.Breed,
                DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth),
                IsMale = model.IsMale,
                ImageUrl = model.ImageUrl,
                CreatedDate = DateTime.UtcNow,
                PetOwnerId = model.PetOwnerId,
                IsActive = true
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync(cancellationToken);

            return pet.PetId;
        }
    }
}
