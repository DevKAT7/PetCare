using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Application.Interfaces;
namespace PetCare.Application.Features.Vets.Queries
{
    public class GetAllVetsQuery : IRequest<List<VetReadModel>>
    {
        public string? SearchTerm { get; set; }
        public int? SpecializationId { get; set; }
        public string SortColumn { get; set; } = "LastName";
        public string SortDirection { get; set; } = "asc";
    }

    public class GetAllVetsHandler : IRequestHandler<GetAllVetsQuery, List<VetReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllVetsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VetReadModel>> Handle(GetAllVetsQuery request, CancellationToken cancellationToken)
        {

            var query = _context.Vets
                .Include(v => v.User)
                .AsNoTracking()
                .Where(v => v.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(v =>
                    v.LastName.ToLower().Contains(term) ||
                    v.FirstName.ToLower().Contains(term) ||
                    v.Pesel.Contains(term));
            }

            if (request.SpecializationId.HasValue)
            {
                query = query.Where(v => v.SpecializationLinks
                    .Any(link => link.VetSpecializationId == request.SpecializationId));
            }

            if (request.SortDirection?.ToLower() == "desc")
            {
                query = request.SortColumn switch
                {
                    "FirstName" => query.OrderByDescending(v => v.FirstName),
                    "LastName" => query.OrderByDescending(v => v.LastName),
                    _ => query.OrderByDescending(v => v.LastName)
                };
            }
            else
            {
                query = request.SortColumn switch
                {
                    "FirstName" => query.OrderBy(v => v.FirstName),
                    "LastName" => query.OrderBy(v => v.LastName),
                    _ => query.OrderBy(v => v.LastName)
                };
            }

            var vets = await query.Select(vet => new VetReadModel
            {
                VetId = vet.VetId,
                FirstName = vet.FirstName,
                LastName = vet.LastName,
                Pesel = vet.Pesel,
                ProfilePictureUrl = vet.ProfilePictureUrl,
                Description = vet.Description,
                YearsOfExperience = vet.YearsOfExperience,
                CareerStartDate = vet.CareerStartDate,
                PhoneNumber = vet.User != null ? vet.User.PhoneNumber : null,
                Email = vet.User != null ? vet.User.Email : "",
                Specializations = vet.SpecializationLinks
                    .Select(link => link.VetSpecialization.Name)
                    .ToList()
            }).ToListAsync(cancellationToken);

            return vets;
        }
    }
}

