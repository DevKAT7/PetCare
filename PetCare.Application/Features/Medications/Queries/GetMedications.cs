using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Medications.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Medications.Queries
{
    public class GetMedicationsQuery : IRequest<List<MedicationReadModel>> 
    {
        public GetMedicationsQuery() { }

        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public string? SortColumn { get; set; } = "Name";
        public string? SortDirection { get; set; } = "asc";
    }

    public class GetMedicationsHandler : IRequestHandler<GetMedicationsQuery, List<MedicationReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetMedicationsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MedicationReadModel>> Handle(GetMedicationsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Medications.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(m =>
                    m.Name.ToLower().Contains(term) ||
                    m.Manufacturer.ToLower().Contains(term));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(m => m.IsActive == request.IsActive.Value);
            }

            bool isDesc = request.SortDirection?.ToLower() == "desc";
            query = request.SortColumn switch
            {
                "Manufacturer" => isDesc ? query.OrderByDescending(m => m.Manufacturer) : query.OrderBy(m => m.Manufacturer),
                "Price" => isDesc ? query.OrderByDescending(m => m.Price) : query.OrderBy(m => m.Price),
                "Status" => isDesc ? query.OrderByDescending(m => m.IsActive) : query.OrderBy(m => m.IsActive),
                _ => isDesc ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name)
            };

            return await query.Select(m => new MedicationReadModel
            {
                MedicationId = m.MedicationId,
                Name = m.Name,
                Description = m.Description,
                Manufacturer = m.Manufacturer,
                Price = m.Price,
                CurrentStock = m.StockItem != null ? m.StockItem.CurrentStock : 0,
                IsActive = m.IsActive
            }).ToListAsync(cancellationToken);
        }
    }
}
