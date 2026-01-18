using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Pets.Dto;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Pets.Queries
{
    public class GetAllPetsQuery : IRequest<List<PetReadModel>>
    {
        public string? SearchTerm { get; set; }
        public string? FilterSpecies { get; set; }
        public string? FilterSex { get; set; }
        public string? SortColumn { get; set; } = "Name";
        public string? SortDirection { get; set; } = "asc";
    }

    public class GetAllPetsHandler : IRequestHandler<GetAllPetsQuery, List<PetReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPetsHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<PetReadModel>> Handle(GetAllPetsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Pets
                .Include(p => p.PetOwner)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();

                bool isDate = DateOnly.TryParse(term, out var dateVal);

                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    (p.PetOwner.FirstName + " " + p.PetOwner.LastName).ToLower().Contains(term) ||
                    (isDate && p.DateOfBirth == dateVal)
                );
            }

            if (!string.IsNullOrWhiteSpace(request.FilterSpecies) && request.FilterSpecies != "All")
            {
                query = query.Where(p => p.Species == request.FilterSpecies);
            }

            if (!string.IsNullOrWhiteSpace(request.FilterSex) && request.FilterSex != "All")
            {
                bool isMale = request.FilterSex == "Male";
                query = query.Where(p => p.IsMale == isMale);
            }

            query = request.SortColumn switch
            {
                "Name" => request.SortDirection == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                "OwnerFullName" => request.SortDirection == "asc" ? query.OrderBy(p => p.PetOwner.LastName) : query.OrderByDescending(p => p.PetOwner.LastName),
                "Species" => request.SortDirection == "asc" ? query.OrderBy(p => p.Species) : query.OrderByDescending(p => p.Species),
                "DateOfBirth" => request.SortDirection == "asc" ? query.OrderBy(p => p.DateOfBirth) : query.OrderByDescending(p => p.DateOfBirth),
                "Sex" => request.SortDirection == "asc" ? query.OrderBy(p => p.IsMale) : query.OrderByDescending(p => p.IsMale),
                _ => query.OrderBy(p => p.Name)
            };

            return await query.Select(p => new PetReadModel
            {
                PetId = p.PetId,
                Name = p.Name,
                Species = p.Species,
                Breed = p.Breed,
                DateOfBirth = p.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                IsMale = p.IsMale,
                ImageUrl = p.ImageUrl,
                CreatedDate = p.CreatedDate,
                PetOwnerId = p.PetOwnerId,
                PetOwnerName = p.PetOwner.FirstName + " " + p.PetOwner.LastName
            }).ToListAsync(cancellationToken);
        }
    }
}
