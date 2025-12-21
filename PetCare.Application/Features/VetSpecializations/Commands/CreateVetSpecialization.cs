using MediatR;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSpecializations.Commands
{
    public class CreateVetSpecializationCommand : IRequest<int>
    {
        public required VetSpecializationCreateModel Specialization { get; set; }
    }

    public class CreateVetSpecializationHandler : IRequestHandler<CreateVetSpecializationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateVetSpecializationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateVetSpecializationCommand request, CancellationToken cancellationToken)
        {
            var entity = new VetSpecialization
            {
                Name = request.Specialization.Name
            };

            _context.VetSpecializations.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.VetSpecializationId;
        }
    }
}