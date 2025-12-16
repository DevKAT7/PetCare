using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.VetSpecializations.Commands
{
    public class UpdateVetSpecializationCommand : IRequest<int>
    {
        public UpdateVetSpecializationCommand(int id, VetSpecializationCreateModel model)
        {
            VetSpecializationId = id;
            Model = model;
        }

        public int VetSpecializationId { get; }
        public VetSpecializationCreateModel Model { get; }
    }

    public class UpdateVetSpecializationHandler : IRequestHandler<UpdateVetSpecializationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateVetSpecializationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateVetSpecializationCommand request, CancellationToken cancellationToken)
        {
            var spec = await _context.VetSpecializations
                .FirstOrDefaultAsync(s => s.VetSpecializationId == request.VetSpecializationId, cancellationToken);

            if (spec == null)
            {
                throw new NotFoundException($"Vet specialization with id {request.VetSpecializationId} not found.");
            }

            spec.Name = request.Model.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return spec.VetSpecializationId;
        }
    }
}