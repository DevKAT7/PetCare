using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.StockItems.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.StockItems.Queries
{
    public class GetStockItemsQuery : IRequest<List<StockItemReadModel>> { }

    public class GetStockItemsHandler : IRequestHandler<GetStockItemsQuery, List<StockItemReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetStockItemsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockItemReadModel>> Handle(GetStockItemsQuery request, CancellationToken cancellationToken)
        {
            return await _context.StockItems
                .Include(s => s.Medication)
                .Select(stockItem => new StockItemReadModel
                {
                    StockItemId = stockItem.StockItemId,
                    CurrentStock = stockItem.CurrentStock,
                    ReorderLevel = stockItem.ReorderLevel,
                    MedicationId = stockItem.MedicationId,
                    MedicationName = stockItem.Medication != null ? stockItem.Medication.Name : string.Empty
                })
                .ToListAsync(cancellationToken);
        }
    }
}
