using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSpecializations.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSpecializations.Commands
{
    public class UpdateVetSpecializationCommand : IRequest<int>
    {        
        public int Id { get; }
        public VetSpecializationCreateModel Model { get; }

        public UpdateVetSpecializationCommand(int id, VetSpecializationCreateModel model)
        {
            Id = id;
            Model = model;
        }
    }

    public class UpdateVetSpecializationHandler : IRequestHandler<UpdateVetSpecializationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVetSpecializationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateVetSpecializationCommand request, CancellationToken cancellationToken)
        {
            var spec = await _context.VetSpecializations
                .FirstOrDefaultAsync(s => s.VetSpecializationId == request.Id, cancellationToken);

            if (spec == null)
            {
                throw new NotFoundException("Vet specialization", request.Id);
            }

            spec.Name = request.Model.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return spec.VetSpecializationId;
        }
    }
}