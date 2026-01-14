using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vets.Queries
{
    public class GetVetForEditQuery : IRequest<VetUpdateModel>
    {
        public int VetId { get; set; }
    }

    public class GetVetForEditHandler : IRequestHandler<GetVetForEditQuery, VetUpdateModel>
    {
        private readonly IApplicationDbContext _context;

        public GetVetForEditHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetUpdateModel> Handle(GetVetForEditQuery request, CancellationToken cancellationToken)
        {
            var vet = await _context.Vets
                .Include(v => v.User)
                .Where(v => v.VetId == request.VetId && v.IsActive)
                .Select(v => new VetUpdateModel
                {
                    Email = v.User.Email!,
                    PhoneNumber = v.User.PhoneNumber ?? string.Empty,
                    FirstName = v.FirstName ?? string.Empty,
                    LastName = v.LastName ?? string.Empty,
                    Address = v.Address ?? string.Empty,
                    ProfilePictureUrl = v.ProfilePictureUrl ?? string.Empty,
                    Description = v.Description ?? string.Empty,
                    SpecializationIds = v.SpecializationLinks.Select(sl => sl.VetSpecializationId).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (vet == null)
            {
                throw new NotFoundException($"Vet with ID: {request.VetId} was not found.");
            }

            return vet;
        }
    }
}
