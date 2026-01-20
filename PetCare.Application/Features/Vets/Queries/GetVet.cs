using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vets.Queries
{
    public class GetVetQuery : IRequest<VetReadModel>
    {
        public int VetId { get; set; }
    }

    public class GetVetHandler : IRequestHandler<GetVetQuery, VetReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetVetHandler(IApplicationDbContext context)
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
                    HireDate = vet.HireDate,
                    Address = vet.Address,
                    Pesel = vet.Pesel,
                    PhoneNumber = vet.User.PhoneNumber,
                    Email = vet.User.Email,
                    LicenseNumber = vet.LicenseNumber,
                    Specializations = vet.SpecializationLinks
                            .Select(link => link.VetSpecialization.Name)
                            .ToList()
                })
                     .FirstOrDefaultAsync(cancellationToken);

            if (vet == null)
            {
                throw new NotFoundException($"Vet with ID: {request.VetId} was not found.");
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - vet.CareerStartDate.Year;
            if (vet.CareerStartDate > today.AddYears(-age)) age--;

            vet.YearsOfExperience = age;

            return vet;
        }
    }
}
