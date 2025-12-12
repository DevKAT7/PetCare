using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Dto;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Vets.Queries
{
    public class GetVetQuery : IRequest<VetReadModel>
    {
        public int VetId { get; set; }
    }

    public class GetVetHandler : IRequestHandler<GetVetQuery, VetReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetVetHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetReadModel> Handle(GetVetQuery request, CancellationToken cancellationToken)
        {
            var vet = await _context.Vets
                .Where(v => v.VetId == request.VetId && v.IsActive)
                .Select(vet => new VetReadModel
                    {
                        VetId = vet.VetId,
                        FirstName = vet.FirstName,
                        LastName = vet.LastName,
                        ProfilePictureUrl = vet.ProfilePictureUrl,
                        Description = vet.Description,
                        CareerStartDate = vet.CareerStartDate,
                        PhoneNumber = vet.User.PhoneNumber,
                        Specializations = vet.SpecializationLinks
                            .Select(link => link.VetSpecialization.Name)
                            .ToList()
                     })
                     .FirstOrDefaultAsync(cancellationToken);

            if (vet == null)
            {
                throw new NotFoundException($"Nie znaleziono weterynarza o ID: {request.VetId}");
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - vet.CareerStartDate.Year;
            if (vet.CareerStartDate > today.AddYears(-age)) age--;

            vet.YearsOfExperience = age;

            return vet;
        }
    }
}
