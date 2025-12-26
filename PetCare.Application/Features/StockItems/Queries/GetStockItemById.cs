using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.StockItems.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockItems.Queries
{
    public class GetStockItemQuery : IRequest<StockItemReadModel>
    {
        public int Id { get; set; }
    }

    public class GetStockItemHandler : IRequestHandler<GetStockItemQuery, StockItemReadModel>
    {
        private readonly ApplicationDbContext _context;

        public GetStockItemHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockItemReadModel> Handle(GetStockItemQuery request, CancellationToken cancellationToken)
        {
            var stockItem = await _context.StockItems.Include(s => s.Medication)
                .FirstOrDefaultAsync(x => x.StockItemId == request.Id, cancellationToken);

            if (stockItem == null)
            {
                throw new NotFoundException("Stock item", request.Id);
            }

            return new StockItemReadModel
            {
                StockItemId = stockItem.StockItemId,
                CurrentStock = stockItem.CurrentStock,
                ReorderLevel = stockItem.ReorderLevel,
                MedicationId = stockItem.MedicationId,
                MedicationName = stockItem.Medication?.Name ?? string.Empty
            };
        }
    }
}
